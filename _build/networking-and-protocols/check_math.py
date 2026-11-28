#!/usr/bin/env python3
"""Recompute every number used in the book.

Each source file under src/ may carry `body_checks:` (front matter, for figures in the prose and worked
example) and per-question `checks:`. Both are lists of Python statements, usually `assert` lines, run in a
namespace with ipaddress, math, hashlib, base64 and the helpers below. A failing assert is reported with
its location."""
import base64
import hashlib
import ipaddress
import math
import pathlib
import sys

import yaml

SRC = pathlib.Path(__file__).resolve().parent / "src"


def net(cidr):
    return ipaddress.ip_network(cidr, strict=False)


def lpm(addr, routes):
    a = ipaddress.ip_address(addr)
    best = None
    for r in routes:
        n = ipaddress.ip_network(r)
        if a in n and (best is None or n.prefixlen > best.prefixlen):
            best = n
    return str(best)


def usable_hosts(cidr):
    n = ipaddress.ip_network(cidr)
    if n.version == 4 and n.prefixlen <= 30:
        return n.num_addresses - 2
    return n.num_addresses


def summarize(cidrs):
    return [str(x) for x in ipaddress.collapse_addresses(ipaddress.ip_network(c) for c in cidrs)]


def rto_update(srtt, rttvar, r, alpha=1 / 8, beta=1 / 4, g=0.0, k=4, min_rto=1.0):
    rttvar = (1 - beta) * rttvar + beta * abs(srtt - r)
    srtt = (1 - alpha) * srtt + alpha * r
    return srtt, rttvar, max(min_rto, srtt + max(g, k * rttvar))


def slow_start_rtts(iw, target):
    cwnd, rtts = iw, 0
    while cwnd < target:
        cwnd *= 2
        rtts += 1
    return rtts


def ws_accept(key):
    guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
    return base64.b64encode(hashlib.sha1((key + guid).encode()).digest()).decode()


def close(a, b, tol=1e-6):
    return abs(a - b) <= tol * max(1.0, abs(a), abs(b))


def main():
    files = [f for ch in sorted(p for p in SRC.iterdir() if p.is_dir()) for f in sorted(ch.glob("*.md"))]
    total, failures = 0, []
    for f in files:
        raw = f.read_text(encoding="utf-8")
        meta = yaml.safe_load(raw[3:raw.index("\n---", 3)])
        blocks = [("body", c) for c in meta.get("body_checks") or []]
        for q in meta.get("questions") or []:
            blocks += [(q["id"], c) for c in q.get("checks") or []]
        for loc, code in blocks:
            total += 1
            ns = dict(ipaddress=ipaddress, math=math, hashlib=hashlib, base64=base64, net=net, lpm=lpm,
                      usable_hosts=usable_hosts, summarize=summarize, rto_update=rto_update,
                      slow_start_rtts=slow_start_rtts, ws_accept=ws_accept, close=close)
            try:
                exec(code, ns)
            except Exception as ex:  # noqa: BLE001
                failures.append(f"{f.parent.name}/{f.name} [{loc}]: {type(ex).__name__}: {ex}\n    {code.strip()}")
    for x in failures:
        print("FAIL:", x)
    print(f"checks: {total} run, {len(failures)} failed, {len(files)} source files")
    return 1 if failures else 0


if __name__ == "__main__":
    sys.exit(main())
