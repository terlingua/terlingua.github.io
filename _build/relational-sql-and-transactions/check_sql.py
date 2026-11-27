#!/usr/bin/env python3
"""Execution audit: run every `checks:` entry in the section sources.

Check kinds (key `engine`):
  sqlite | postgres | both : setup (script), query, then one of
      expect: rows (list of lists), ordered: bool (default false)
      expect_error: substring of the error message (sqlite) or SQLSTATE (postgres)
      expect_contains / expect_not_contains: substring of the joined plan text (EXPLAIN output)
  schedule : PostgreSQL interleaving. setup, steps: [[session, sql], ...],
      Session 0 means "sleep for the given seconds" (expect "ok").
      expect: list aligned with steps, each one of
        "ok" | "blocked" | "err:<SQLSTATE>" | rows | "blocked->ok" | "blocked->err:<SQLSTATE>"
      final: {query, expect, ordered}
  python : code (asserts)
Postgres connection: unix socket /tmp/pg, port 5433, user postgres.
"""
import decimal
import json
import pathlib
import queue
import sqlite3
import sys
import threading
import time
import traceback

import yaml

BUILD = pathlib.Path(__file__).resolve().parent
SRC = BUILD / "src"
PG = dict(host="/tmp/pg", port=5433, user="postgres", dbname="postgres")

try:
    import psycopg2
    import psycopg2.extensions
except ImportError:  # pragma: no cover
    psycopg2 = None

failures = []
counts = {"sqlite": 0, "postgres": 0, "schedule": 0, "python": 0}
schema_seq = [0]


def norm(v):
    if isinstance(v, decimal.Decimal):
        f = float(v)
        return int(f) if f == int(f) else round(f, 6)
    if isinstance(v, float):
        return int(v) if v == int(v) else round(v, 6)
    if isinstance(v, bool):
        return v
    return v


def norm_rows(rows):
    return [[norm(c) for c in r] for r in rows]


def same(actual, expected, ordered):
    a = norm_rows(actual)
    e = norm_rows(expected)
    if ordered:
        return a == e
    key = lambda r: [(x is None, str(type(x)), str(x)) for x in r]
    return sorted(a, key=key) == sorted(e, key=key)


def fresh_schema(conn):
    schema_seq[0] += 1
    name = f"ck{schema_seq[0]}"
    cur = conn.cursor()
    cur.execute(f"DROP SCHEMA IF EXISTS {name} CASCADE; CREATE SCHEMA {name};")
    return name


def pg_connect(schema=None):
    conn = psycopg2.connect(**PG)
    conn.autocommit = True
    if schema:
        conn.cursor().execute(f"SET search_path = {schema}")
    return conn


def run_sqlite(check, where):
    counts["sqlite"] += 1
    conn = sqlite3.connect(":memory:")
    try:
        setup = check.get("setup")
        for stmt in (setup if isinstance(setup, list) else [setup] if setup else []):
            conn.executescript(stmt)
        try:
            cur = conn.execute(check["query"])
            rows = [list(r) for r in cur.fetchall()]
        except sqlite3.Error as ex:
            if "expect_error" in check and check["expect_error"].lower() in str(ex).lower():
                return
            failures.append(f"{where} [sqlite] error: {ex}")
            return
        evaluate(rows, check, where, "sqlite")
    finally:
        conn.close()


def run_postgres(check, where):
    counts["postgres"] += 1
    admin = pg_connect()
    schema = fresh_schema(admin)
    conn = pg_connect(schema)
    try:
        cur = conn.cursor()
        setup = check.get("setup")
        for stmt in (setup if isinstance(setup, list) else [setup] if setup else []):
            cur.execute(stmt)
        try:
            cur.execute(check["query"])
            rows = [list(r) for r in cur.fetchall()] if cur.description else []
        except psycopg2.Error as ex:
            if "expect_error" in check and (check["expect_error"] == ex.pgcode or check["expect_error"].lower() in str(ex).lower()):
                return
            failures.append(f"{where} [postgres] error: {ex.pgcode} {ex}")
            return
        evaluate(rows, check, where, "postgres")
    finally:
        conn.close()
        admin.cursor().execute(f"DROP SCHEMA {schema} CASCADE")
        admin.close()


def evaluate(rows, check, where, engine):
    if "expect_error" in check:
        failures.append(f"{where} [{engine}] expected error {check['expect_error']} but got {rows}")
        return
    if "expect" in check:
        if not same(rows, check["expect"], check.get("ordered", False)):
            failures.append(f"{where} [{engine}] expected {check['expect']} got {norm_rows(rows)}")
    text = "\n".join(" ".join(str(c) for c in r) for r in rows)
    if "expect_contains" in check and check["expect_contains"] not in text:
        failures.append(f"{where} [{engine}] plan lacks '{check['expect_contains']}':\n{text}")
    if "expect_not_contains" in check and check["expect_not_contains"] in text:
        failures.append(f"{where} [{engine}] plan unexpectedly has '{check['expect_not_contains']}':\n{text}")


class Session:
    def __init__(self, schema):
        self.conn = pg_connect(schema)
        self.q = queue.Queue()
        self.t = threading.Thread(target=self.loop, daemon=True)
        self.t.start()

    def loop(self):
        while True:
            item = self.q.get()
            if item is None:
                return
            sql, slot, done = item
            try:
                cur = self.conn.cursor()
                cur.execute(sql)
                slot["result"] = [list(r) for r in cur.fetchall()] if cur.description else "ok"
            except psycopg2.Error as ex:
                slot["result"] = f"err:{ex.pgcode}"
            done.set()


def run_schedule(check, where):
    counts["schedule"] += 1
    admin = pg_connect()
    schema = fresh_schema(admin)
    setup = pg_connect(schema)
    for stmt in (check["setup"] if isinstance(check["setup"], list) else [check["setup"]]):
        setup.cursor().execute(stmt)
    setup.close()
    sessions = {}
    slots = []
    for sess, sql in check["steps"]:
        if sess == 0:
            time.sleep(float(sql))
            slots.append({"initial": "done", "result": "ok", "event": threading.Event()})
            slots[-1]["event"].set()
            continue
        if sess not in sessions:
            sessions[sess] = Session(schema)
        slot = {}
        done = threading.Event()
        sessions[sess].q.put((sql, slot, done))
        finished = done.wait(check.get("wait", 0.8))
        slot["initial"] = "done" if finished else "blocked"
        slot["event"] = done
        slots.append(slot)
    for slot in slots:
        slot["event"].wait(5)
    for i, (slot, exp) in enumerate(zip(slots, check["expect"])):
        res = slot.get("result", "hung")
        if slot["initial"] == "blocked":
            got = f"blocked->{res}" if isinstance(res, str) else "blocked->" + json.dumps(norm_rows(res))
        else:
            got = res
        ok = False
        if isinstance(exp, str):
            if exp == "blocked":
                ok = slot["initial"] == "blocked"
            else:
                ok = got == exp
        else:
            ok = isinstance(res, list) and slot["initial"] == "done" and same(res, exp, True)
        if not ok:
            failures.append(f"{where} [schedule] step {i + 1} {check['steps'][i]} expected {exp} got {got}")
    for s in sessions.values():
        s.q.put(None)
        try:
            s.conn.close()
        except Exception:
            pass
    if "final" in check:
        conn = pg_connect(schema)
        cur = conn.cursor()
        cur.execute(check["final"]["query"])
        rows = [list(r) for r in cur.fetchall()]
        if not same(rows, check["final"]["expect"], check["final"].get("ordered", False)):
            failures.append(f"{where} [schedule] final expected {check['final']['expect']} got {norm_rows(rows)}")
        conn.close()
    admin.cursor().execute(f"DROP SCHEMA {schema} CASCADE")
    admin.close()


def run_python(check, where):
    counts["python"] += 1
    try:
        exec(check["code"], {"math": __import__("math")})
    except Exception:
        failures.append(f"{where} [python] {traceback.format_exc(limit=1)}")


def main():
    total = 0
    for src in sorted(SRC.rglob("*.md")):
        raw = src.read_text(encoding="utf-8")
        fm = raw[3:raw.index("\n---", 3)]
        meta = yaml.safe_load(fm)
        rel = src.relative_to(SRC)
        for k, question in enumerate(meta.get("questions", []), start=1):
            for c, check in enumerate(question.get("checks", []) or [], start=1):
                where = f"{rel} q{k} '{question['id']}' check {c}"
                engine = check["engine"]
                total += 1
                if engine in ("sqlite", "both"):
                    run_sqlite(check, where)
                if engine in ("postgres", "both"):
                    run_postgres(check, where)
                if engine == "schedule":
                    run_schedule(check, where)
                if engine == "python":
                    run_python(check, where)
        for c, check in enumerate(meta.get("body_checks", []) or [], start=1):
            where = f"{rel} body check {c}"
            total += 1
            engine = check["engine"]
            if engine in ("sqlite", "both"):
                run_sqlite(check, where)
            if engine in ("postgres", "both"):
                run_postgres(check, where)
            if engine == "schedule":
                run_schedule(check, where)
            if engine == "python":
                run_python(check, where)
    print(f"checks: {total} entries; runs: {counts}")
    if failures:
        print(f"FAILURES ({len(failures)}):")
        for f in failures:
            print("  - " + f)
        return 1
    print("all checks passed")
    return 0


if __name__ == "__main__":
    sys.exit(main())
