#!/usr/bin/env python3
"""Validate content/books/relational-sql-and-transactions against the compiler rules
(ported from tools/ObserverMagazine.ContentProcessor) and the book's HARD FORMAT RULES."""
import collections
import datetime
import pathlib
import re
import sys

import yaml

ROOT = pathlib.Path(__file__).resolve().parents[2]
SLUG = "relational-sql-and-transactions"
BOOK = ROOT / "content" / "books" / SLUG
AUTHORS = ROOT / "content" / "authors"

SLUG_RE = re.compile(r"^[a-z0-9]+(?:-[a-z0-9]+)*$")
NUMBERED_RE = re.compile(r"^(\d+)[-_]([a-z0-9]+(?:-[a-z0-9]+)*)$")
STRICT_NUMBERED_RE = re.compile(r"^\d{2}-[a-z0-9]+(?:-[a-z0-9]+)*$")
RESERVED_IDS = {"yes", "no", "true", "false", "null", "on", "off", "y", "n", "~"}
LANGS = {"csharp", "python", "javascript", "sql", "bash", "json", "yaml", "html"}
HEADINGS = ["Learning objectives", "The invariant", "Worked example",
            "Where this shows up today", "Pitfalls checklist", "Key takeaways"]
POSITION_WORDS = re.compile(
    r"(?i:\boption\s+[A-Ea-e1-5]\b|\b(?:first|second|third|fourth|fifth|last)\s+(?:option|choice|answer)\b|\bchoice\s+[A-E]\b)|^\s*\([a-e]\)\s",
    re.MULTILINE)

errors, warnings = [], []
stats = collections.Counter()


def err(loc, msg):
    errors.append(f"{loc}: {msg}")


def warn(loc, msg):
    warnings.append(f"{loc}: {msg}")


def is_hidden(p):
    return p.name.startswith(".") or p.name.startswith("_")


def front_matter(raw):
    if not raw.startswith("---"):
        return None, raw
    end = raw.find("\n---", 3)
    if end < 0:
        return None, raw
    return raw[3:end].strip(), raw[end + 4:].lstrip("\r\n")


def check_book_yml():
    path = BOOK / "book.yml"
    if not path.exists():
        err(path, "file is missing")
        return
    raw = path.read_text(encoding="utf-8")
    data = yaml.safe_load(raw)
    for key in ["title", "author", "summary", "date", "tags", "draft"]:
        if key not in data:
            err(path, f"missing key {key}")
    unknown = set(data) - {"title", "author", "summary", "date", "updated", "tags", "image", "draft"}
    if unknown:
        err(path, f"unknown keys {unknown}")
    if data.get("draft") is not False:
        err(path, "draft must be false")
    if not isinstance(data.get("date"), datetime.date):
        err(path, "date must be an unquoted ISO date")
    elif data["date"] > datetime.date.today():
        err(path, "date is in the future; the compiler would skip the book")
    if not (AUTHORS / f"{data.get('author')}.yml").exists():
        err(path, f"author profile {data.get('author')} not found")
    if not isinstance(data.get("tags"), list) or not all(isinstance(t, str) for t in data["tags"]):
        err(path, "tags must be a list of strings")
    for line in raw.splitlines():
        m = re.match(r"^(title|author|summary):\s*(.*)$", line)
        if m and not m.group(2).startswith('"'):
            err(path, f"{m.group(1)} is not double-quoted")


def check_chapter_yml(chapter):
    path = chapter / "chapter.yml"
    if not path.exists():
        err(path, "file is missing")
        return
    raw = path.read_text(encoding="utf-8")
    data = yaml.safe_load(raw) or {}
    if not isinstance(data.get("title"), str) or not data["title"].strip():
        err(path, "title is required")
    if data.get("draft") is not False:
        err(path, "draft must be false")
    if not isinstance(data.get("summary"), str):
        err(path, "summary must be a string")
    unknown = set(data) - {"title", "summary", "draft"}
    if unknown:
        err(path, f"unknown keys {unknown}")
    for line in raw.splitlines():
        m = re.match(r"^(title|summary):\s*(.*)$", line)
        if m and not m.group(2).startswith('"'):
            err(path, f"{m.group(1)} is not double-quoted")


def words(text):
    return len(text.split())


def strip_code(body):
    out, in_code = [], False
    for line in body.split("\n"):
        if line.startswith("```"):
            in_code = not in_code
            continue
        if not in_code:
            out.append(line)
    return "\n".join(out)


def split_sections(body):
    parts = collections.OrderedDict()
    current = None
    for line in body.split("\n"):
        m = re.match(r"^## (.+)$", line)
        if m:
            current = m.group(1).strip()
            parts[current] = []
        elif current:
            parts[current].append(line)
    return {k: "\n".join(v) for k, v in parts.items()}


def bullets(text):
    return [l for l in text.split("\n") if re.match(r"^- ", l)]


def check_body(loc, body):
    if not body.startswith("## Learning objectives"):
        err(loc, "body must start with '## Learning objectives'")
    in_code = False
    for i, line in enumerate(body.split("\n"), start=1):
        if line.startswith("```"):
            if not in_code:
                lang = line[3:].strip()
                if lang not in LANGS:
                    err(loc, f"body line {i}: code fence language '{lang}' not allowed")
            in_code = not in_code
    if in_code:
        err(loc, "unclosed code fence")
    sections = split_sections(body)
    names = list(sections)
    required = [h for h in names if h in HEADINGS]
    if required != HEADINGS:
        err(loc, f"headings must be {HEADINGS} in order; got {names}")
    extra = [h for h in names if h not in HEADINGS and h != "Sources"]
    if extra:
        err(loc, f"unexpected headings {extra}")
    if "Sources" in names and names[-1] != "Sources":
        err(loc, "Sources must be the last heading")
    lo = bullets(sections.get("Learning objectives", ""))
    if not 3 <= len(lo) <= 5:
        err(loc, f"Learning objectives needs 3-5 bullets, has {len(lo)}")
    pit = bullets(sections.get("Pitfalls checklist", ""))
    if not 3 <= len(pit) <= 6:
        err(loc, f"Pitfalls checklist needs 3-6 bullets, has {len(pit)}")
    kt = bullets(sections.get("Key takeaways", ""))
    if not 3 <= len(kt) <= 5:
        err(loc, f"Key takeaways needs 3-5 bullets, has {len(kt)}")
    prose_body = "\n".join(v for k, v in sections.items() if k != "Sources")
    prose = strip_code(prose_body)
    prose_lines = [l for l in prose.split("\n") if not l.lstrip().startswith("|")]
    n = words("\n".join(prose_lines))
    stats["prose_words"] += n
    if not 400 <= n <= 1200:
        err(loc, f"prose word count {n} outside 400-1200")
    today = strip_code(sections.get("Where this shows up today", ""))
    t = words(today)
    total = words(strip_code(prose_body))
    share = t / max(total, 1)
    if share > 0.20:
        err(loc, f"'Where this shows up today' is {share:.0%} of the section (limit about 15%)")
    elif share > 0.16:
        warn(loc, f"'Where this shows up today' is {share:.0%} of the section")
    if "As of November 2026" not in today:
        warn(loc, "'Where this shows up today' has no 'As of November 2026' claim")
    if re.search(r"all of the above|none of the above", body, re.IGNORECASE):
        err(loc, "body mentions all/none of the above")


def raw_quote_check(loc, fm_raw):
    for i, line in enumerate(fm_raw.split("\n"), start=1):
        m = re.match(r"^\s*(?:- )?(title|id|type|prompt|text):\s*(.*)$", line)
        if m and not m.group(2).startswith('"'):
            err(loc, f"front matter line {i}: {m.group(1)} value is not double-quoted")
        if re.match(r"^---", line):
            err(loc, f"front matter line {i} starts with ---")
        if "\t" in line:
            err(loc, f"front matter line {i} contains a tab")


def check_section(loc, path, s, position_totals):
    raw = path.read_text(encoding="utf-8")
    fm_raw, body = front_matter(raw)
    if fm_raw is None:
        err(loc, "missing front matter")
        return
    raw_quote_check(loc, fm_raw)
    try:
        meta = yaml.safe_load(fm_raw)
    except yaml.YAMLError as ex:
        err(loc, f"invalid front matter: {ex}")
        return
    unknown = set(meta) - {"title", "questions"}
    if unknown:
        err(loc, f"unknown front matter keys {unknown}")
    if not isinstance(meta.get("title"), str) or not meta["title"].strip():
        err(loc, "title is required")
    qs = meta.get("questions") or []
    if not 5 <= len(qs) <= 7:
        err(loc, f"needs 5-7 questions, has {len(qs)}")
    seen = set()
    pos_counter = collections.Counter()
    longest_correct = 0
    for k, q in enumerate(qs, start=1):
        ql = f"{loc} question {k}"
        if set(q) != {"id", "prompt", "type", "explanation", "options"}:
            err(ql, f"keys must be id, prompt, type, explanation, options; got {sorted(q)}")
        qid = q.get("id")
        if not isinstance(qid, str) or not SLUG_RE.match(qid) or qid.lower() in RESERVED_IDS:
            err(ql, f"bad id {qid!r}")
        if qid in seen:
            err(ql, f"duplicate id {qid}")
        seen.add(qid)
        stats["questions"] += 1
        for key in ("prompt", "explanation"):
            if not isinstance(q.get(key), str) or not q[key].strip():
                err(ql, f"{key} must be a non-empty string")
        expl = q.get("explanation") or ""
        prompt = q.get("prompt") or ""
        if POSITION_WORDS.search(expl):
            err(ql, f"explanation refers to options by position: {POSITION_WORDS.search(expl).group(0)!r}")
        if re.search(r"all of the above|none of the above", prompt + " ".join(str(o.get("text")) for o in q.get("options") or []), re.IGNORECASE):
            err(ql, "all/none of the above is not allowed")
        opts = q.get("options") or []
        for o in opts:
            if not isinstance(o, dict) or not set(o) <= {"text", "correct"} or "text" not in o:
                err(ql, f"bad option {o!r}")
                continue
            if not isinstance(o["text"], str) or not o["text"].strip():
                err(ql, f"option text must be a non-empty string: {o!r}")
            if "correct" in o and o["correct"] is not True:
                err(ql, "wrong options must omit `correct`; correct ones use `correct: true`")
        texts = [o.get("text") for o in opts]
        if len(set(texts)) != len(texts):
            err(ql, "duplicate option text")
        correct_idx = [i for i, o in enumerate(opts) if o.get("correct") is True]
        n = len(opts)
        typ = q.get("type")
        stats[f"type_{typ}"] += 1
        if typ == "single":
            if len(correct_idx) != 1:
                err(ql, f"single needs exactly 1 correct, has {len(correct_idx)}")
            if not 4 <= n <= 5:
                err(ql, f"single needs 4-5 options, has {n}")
            if prompt.rstrip().endswith("Select all that apply."):
                err(ql, "single prompt must not end with 'Select all that apply.'")
        elif typ == "multiple":
            if not 2 <= len(correct_idx) <= 3:
                err(ql, f"multiple needs 2-3 correct, has {len(correct_idx)}")
            if n != 5:
                err(ql, f"multiple needs 5 options, has {n}")
            if not prompt.rstrip().endswith("Select all that apply."):
                err(ql, "multiple prompt must end with 'Select all that apply.'")
        else:
            err(ql, f"type must be single or multiple, got {typ!r}")
        if n and correct_idx:
            first = (k + s) % n
            expected = sorted((first + j) % n for j in range(len(correct_idx)))
            if sorted(correct_idx) != expected:
                err(ql, f"correct positions {correct_idx} but rule (k={k}, s={s}, n={n}) wants {expected}")
            for i in correct_idx:
                pos_counter[i] += 1
                position_totals[i] += 1
            lengths = [len(t or "") for t in texts]
            if max(lengths[i] for i in correct_idx) == max(lengths) and lengths.count(max(lengths)) == 1:
                longest_correct += 1
    total_correct = sum(pos_counter.values())
    if total_correct:
        top_pos, top = pos_counter.most_common(1)[0]
        if top / total_correct > 0.40:
            err(loc, f"position {top_pos} holds {top}/{total_correct} = {top / total_correct:.0%} of correct answers (> 40%)")
    if qs and longest_correct / len(qs) > 0.5:
        warn(loc, f"correct option is the longest in {longest_correct}/{len(qs)} questions")
    stats["longest_correct"] += longest_correct
    check_body(loc, body)


def main():
    if not BOOK.exists():
        print(f"missing {BOOK}")
        return 1
    if not SLUG_RE.match(BOOK.name):
        err(BOOK, "bad book slug")
    check_book_yml()
    s = 0
    position_totals = collections.Counter()
    chapters = sorted(p for p in BOOK.iterdir() if p.is_dir() and not is_hidden(p))
    for chapter in chapters:
        if not STRICT_NUMBERED_RE.match(chapter.name) or not NUMBERED_RE.match(chapter.name):
            err(chapter, "chapter folder must look like NN-chapter-slug")
        check_chapter_yml(chapter)
        stats["chapters"] += 1
        files = sorted(p for p in chapter.iterdir() if p.is_file() and not is_hidden(p))
        sections = [p for p in files if p.suffix == ".md"]
        others = [p.name for p in files if p.suffix != ".md" and p.name != "chapter.yml"]
        if others:
            err(chapter, f"unexpected files {others}")
        if not 3 <= len(sections) <= 5:
            err(chapter, f"needs 3-5 sections, has {len(sections)}")
        for sec in sections:
            s += 1
            stats["sections"] += 1
            if not STRICT_NUMBERED_RE.match(sec.stem):
                err(sec, "section file must look like NN-subchapter-slug.md")
            check_section(f"{chapter.name}/{sec.name}", sec, s, position_totals)
    for e in errors:
        print("ERROR:", e)
    for w in warnings:
        print("WARNING:", w)
    print("stats:", dict(stats))
    print("correct-answer positions (book):", dict(sorted(position_totals.items())))
    print(f"{len(errors)} error(s), {len(warnings)} warning(s)")
    return 1 if errors else 0


if __name__ == "__main__":
    sys.exit(main())
