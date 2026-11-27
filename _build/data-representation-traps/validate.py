import os
import re
import sys
import datetime
from collections import Counter
import yaml
import unicodedata
from common import BOOK, SLUG, NUMBERED, hidden, chapters, sections, split_front_matter

ALLOWED_LANGS = {"csharp", "python", "javascript", "sql", "bash", "json", "yaml", "html"}
RESERVED_IDS = {"yes", "no", "true", "false", "null", "on", "off", "y", "n", "~"}
HEADINGS = ["Learning objectives", "The invariant", "Worked example",
            "Where this shows up today", "Pitfalls checklist", "Key takeaways"]
STRING_KEYS = ("title", "id", "prompt", "text", "type", "summary")

errors = []
warnings = []
stats = {"sections": 0, "questions": 0, "single": 0, "multiple": 0, "words": []}
longest_correct = [0, 0]


def err(loc, msg):
    errors.append(f"{loc}: {msg}")


def warn(loc, msg):
    warnings.append(f"{loc}: {msg}")


def rel(p):
    return os.path.relpath(p, os.path.dirname(BOOK))


def check_book():
    path = os.path.join(BOOK, "book.yml")
    raw = open(path, encoding="utf-8").read()
    data = yaml.safe_load(raw)
    for key in ("title", "author", "summary", "date", "tags", "draft"):
        if key not in data:
            err(rel(path), f"missing {key}")
    if data.get("draft") is not False:
        err(rel(path), "draft must be false")
    if data.get("author") != "terlingua-team":
        err(rel(path), "author must be terlingua-team")
    if data.get("date") != datetime.date(2026, 11, 27):
        err(rel(path), f"date must be 2026-11-27, got {data.get('date')!r}")
    if not os.path.exists(os.path.join(BOOK, "..", "..", "authors", f"{data.get('author')}.yml")):
        err(rel(path), "author profile missing")
    if data.get("tags") != ["fundamentals", "floating-point", "unicode", "time", "engineering"]:
        err(rel(path), "tags mismatch")
    if not SLUG.match(os.path.basename(BOOK)):
        err(rel(BOOK), "bad book slug")


def check_chapter(cdir):
    name = os.path.basename(cdir)
    if not NUMBERED.match(name):
        err(rel(cdir), "bad chapter folder name")
    path = os.path.join(cdir, "chapter.yml")
    if not os.path.exists(path):
        err(rel(cdir), "chapter.yml missing")
        return
    data = yaml.safe_load(open(path, encoding="utf-8").read())
    for key in ("title", "summary", "draft"):
        if key not in data:
            err(rel(path), f"missing {key}")
    if data.get("draft") is not False:
        err(rel(path), "draft must be false")
    if not isinstance(data.get("title"), str) or not data["title"].strip():
        err(rel(path), "title required")


def raw_quote_checks(loc, yb):
    for i, line in enumerate(yb.split("\n"), start=1):
        if line.startswith("---"):
            err(loc, f"front matter line {i} starts with ---")
        m = re.match(r"^\s*(?:-\s+)?([a-zA-Z]+):\s*(.*)$", line)
        if not m:
            continue
        key, val = m.group(1), m.group(2)
        if key in STRING_KEYS and not val.startswith('"'):
            err(loc, f"line {i}: {key} value must be double-quoted")
        if key == "explanation" and val != "|-":
            err(loc, f"line {i}: explanation must use |- block scalar")
        if key == "correct" and val != "true":
            err(loc, f"line {i}: correct must be 'true' when present")


def words(text):
    return len([w for w in re.split(r"\s+", text) if w])


def body_checks(loc, body):
    if not body.startswith("## Learning objectives"):
        err(loc, "body must start with '## Learning objectives'")
    heads = re.findall(r"^## (.+)$", body, flags=re.M)
    if heads != HEADINGS:
        err(loc, f"headings {heads} != {HEADINGS}")
    for m in re.finditer(r"^```(.*)$", body, flags=re.M):
        pass
    fences = re.findall(r"^```(\S*)\s*$", body, flags=re.M)
    if len(fences) % 2:
        err(loc, "unbalanced code fences")
    for i, lang in enumerate(fences):
        if i % 2 == 0:
            if not lang:
                err(loc, "code fence without language")
            elif lang not in ALLOWED_LANGS:
                err(loc, f"code fence language '{lang}' not allowed")
        elif lang:
            err(loc, "closing fence has text")
    parts = re.split(r"^## (.+)$", body, flags=re.M)
    sections_map = {parts[i]: parts[i + 1] for i in range(1, len(parts) - 1, 2)}

    def bullets(name):
        return len(re.findall(r"^- ", sections_map.get(name, ""), flags=re.M))

    for name, lo, hi in (("Learning objectives", 3, 5), ("Pitfalls checklist", 3, 6), ("Key takeaways", 3, 5)):
        n = bullets(name)
        if not lo <= n <= hi:
            err(loc, f"{name} has {n} bullets (want {lo}-{hi})")
    prose = re.sub(r"^```.*?^```", " ", body, flags=re.M | re.S)
    prose_no_tables = "\n".join(l for l in prose.split("\n") if not l.lstrip().startswith("|"))
    total = words(prose_no_tables)
    stats["words"].append(total)
    if not 400 <= total <= 1200:
        err(loc, f"prose word count {total} outside 400-1200")
    today = sections_map.get("Where this shows up today", "")
    tw = words(re.sub(r"^```.*?^```", " ", today, flags=re.M | re.S))
    if total and tw / total > 0.20:
        err(loc, f"'Where this shows up today' is {tw}/{total} = {tw/total:.0%} (> 20%)")
    elif total and tw / total > 0.155:
        warn(loc, f"'Where this shows up today' is {tw/total:.0%} of prose")
    if "As of November 2026" not in today:
        err(loc, "'Where this shows up today' lacks an 'As of November 2026' statement")
    for ch in body:
        if unicodedata.category(ch) in ("Mn", "Me", "Cf", "Co", "Cs") and ord(ch) != 0xFE0F:
            err(loc, f"literal combining/format character U+{ord(ch):04X} in body; write an escape")
            break
    if re.search(r"\b(and so on|similar for the rest|TODO|TBD|lorem)\b", body, flags=re.I):
        err(loc, "placeholder wording in body")


def question_checks(loc, s, fm):
    qs = fm.get("questions") or []
    if not 5 <= len(qs) <= 7:
        err(loc, f"{len(qs)} questions (want 5-7)")
    seen = set()
    positions = Counter()
    first_positions = Counter()
    total_correct = 0
    longest = 0
    singles = 0
    for k, q in enumerate(qs, start=1):
        ql = f"{loc} q{k}"
        for key in ("id", "prompt", "type", "explanation", "options"):
            if key not in q:
                err(ql, f"missing {key}")
        qid = q.get("id")
        if not isinstance(qid, str) or not SLUG.match(qid):
            err(ql, f"bad id {qid!r}")
        elif qid.lower() in RESERVED_IDS:
            err(ql, f"reserved id {qid!r}")
        if qid in seen:
            err(ql, f"duplicate id {qid}")
        seen.add(qid)
        prompt = q.get("prompt", "")
        if not isinstance(prompt, str) or not prompt.strip():
            err(ql, "empty prompt")
        expl = q.get("explanation", "")
        if not isinstance(expl, str) or len(expl.strip()) < 200:
            err(ql, "explanation missing or too short")
        if re.search(r"\b[Oo]ption\s+[A-Ea-e1-5]\b|\b(first|second|third|fourth|fifth|last|top|bottom)\s+option\b|(?:^|\s)\(\s*[a-eA-E]\s*\)", expl or "", flags=re.M):
            err(ql, "explanation refers to options by letter/position")
        low = (prompt + " " + " ".join(o.get("text", "") for o in q.get("options", []))).lower()
        if "all of the above" in low or "none of the above" in low:
            err(ql, "all/none of the above")
        if re.search(r"\bnot\b", prompt) and "**NOT**" not in prompt:
            warn(ql, "negation in stem without **NOT** emphasis")
        opts = q.get("options") or []
        for o in opts:
            if set(o) - {"text", "correct"}:
                err(ql, f"unexpected option keys {set(o)}")
            if "correct" in o and o["correct"] is not True:
                err(ql, "correct key present but not true")
            if not isinstance(o.get("text"), str) or not o["text"].strip():
                err(ql, "option without text")
        texts = [o.get("text", "") for o in opts]
        if len(set(texts)) != len(texts):
            err(ql, "duplicate option text")
        correct_idx = [i for i, o in enumerate(opts) if o.get("correct") is True]
        n = len(opts)
        t = q.get("type")
        if t == "single":
            singles += 1
            stats["single"] += 1
            if len(correct_idx) != 1:
                err(ql, f"single with {len(correct_idx)} correct")
            if not 4 <= n <= 5:
                err(ql, f"single with {n} options")
            if correct_idx:
                lens = [len(x) for x in texts]
                c = correct_idx[0]
                if lens[c] == max(lens) and lens.count(max(lens)) == 1:
                    longest += 1
        elif t == "multiple":
            stats["multiple"] += 1
            if not 2 <= len(correct_idx) <= 3:
                err(ql, f"multiple with {len(correct_idx)} correct")
            if n != 5:
                err(ql, f"multiple with {n} options (want 5)")
            if not prompt.rstrip().endswith("Select all that apply."):
                err(ql, "multiple prompt must end with 'Select all that apply.'")
        else:
            err(ql, f"bad type {t!r}")
        if n and correct_idx:
            first = (k + s) % n
            expected = sorted((first + j) % n for j in range(len(correct_idx)))
            if sorted(correct_idx) != expected:
                err(ql, f"correct positions {correct_idx} != expected {expected} (k={k}, s={s}, n={n})")
            first_positions[first] += 1
            for i in correct_idx:
                positions[i] += 1
                total_correct += 1
    for name, counter, tot in (("all-correct", positions, total_correct), ("first-correct", first_positions, len(qs))):
        if tot and max(counter.values()) / tot > 0.40:
            err(loc, f"{name} position concentration {dict(counter)} exceeds 40%")
    if singles and longest / singles > 0.5:
        err(loc, f"correct option is the unique longest in {longest}/{singles} single questions")
    longest_correct[0] += longest
    longest_correct[1] += singles
    stats["questions"] += len(qs)


def compiler_port(loc, fm):
    title = fm.get("title")
    if not isinstance(title, str) or not title.strip():
        err(loc, "[compiler] title is required")
    if fm.get("draft"):
        err(loc, "[compiler] draft subchapter")
    seen = set()
    for i, q in enumerate(fm.get("questions") or []):
        qid = (q.get("id") or "").strip() or f"q{i+1}"
        if not SLUG.match(qid):
            err(loc, f"[compiler] question '{qid}' id invalid")
            continue
        if qid in seen:
            err(loc, f"[compiler] duplicate question id {qid}")
        seen.add(qid)
        if not str(q.get("prompt") or "").strip():
            err(loc, f"[compiler] {qid} prompt is required")
        opts = q.get("options") or []
        oseen = set()
        for j, o in enumerate(opts):
            oid = (o.get("id") or "").strip() or (chr(97 + j) if j < 26 else f"o{j+1}")
            if not SLUG.match(oid) or oid in oseen:
                err(loc, f"[compiler] {qid} option id {oid} invalid/duplicate")
            oseen.add(oid)
            if not str(o.get("text") or "").strip():
                err(loc, f"[compiler] {qid} option {oid} has no text")
            if "correct" in o and not isinstance(o["correct"], bool):
                err(loc, f"[compiler] {qid} correct is not a boolean")
        cc = sum(1 for o in opts if o.get("correct") is True)
        if len(opts) < 2:
            err(loc, f"[compiler] {qid} at least 2 options are required")
        if cc == 0:
            err(loc, f"[compiler] {qid} at least one option must be marked correct")
        t = q.get("type")
        if t is not None:
            if not isinstance(t, str):
                err(loc, f"[compiler] {qid} type not a string")
            else:
                nt = t.strip().lower()
                if nt not in ("", "single", "multiple"):
                    err(loc, f"[compiler] {qid} unknown type {t}")
                if nt == "single" and cc > 1:
                    err(loc, f"[compiler] {qid} single but >1 correct")


def main():
    check_book()
    chs = chapters()
    for order, slug, cdir in chs:
        if order < 0:
            err(rel(cdir), "[compiler] name must look like '01-my-slug'")
        check_chapter(cdir)
        for f in os.listdir(cdir):
            if f == "chapter.yml" or hidden(f):
                continue
            if not f.endswith(".md"):
                err(rel(os.path.join(cdir, f)), "unexpected file")
            elif not NUMBERED.match(f[:-3]):
                err(rel(os.path.join(cdir, f)), "[compiler] bad section file name")
    for s, (cdir, path) in enumerate(sections(), start=1):
        loc = rel(path)
        stats["sections"] += 1
        raw = open(path, encoding="utf-8").read()
        yb, body = split_front_matter(raw)
        if yb is None:
            err(loc, "no front matter")
            continue
        raw_quote_checks(loc, yb)
        try:
            fm = yaml.safe_load(yb)
        except yaml.YAMLError as e:
            err(loc, f"[compiler] invalid front matter: {e}")
            continue
        compiler_port(loc, fm)
        question_checks(loc, s, fm)
        body_checks(loc, body)
    if longest_correct[1] and longest_correct[0] / longest_correct[1] > 0.40:
        err("book", f"correct option is unique-longest in {longest_correct[0]}/{longest_correct[1]} singles")
    print(f"chapters={len(chs)} sections={stats['sections']} questions={stats['questions']} "
          f"single={stats['single']} multiple={stats['multiple']} "
          f"correct-is-longest={longest_correct[0]}/{longest_correct[1]}")
    if stats["words"]:
        print(f"prose words: min={min(stats['words'])} max={max(stats['words'])} total={sum(stats['words'])}")
    for w in warnings:
        print("WARNING:", w)
    for e in errors:
        print("ERROR:", e)
    print(f"{len(errors)} error(s), {len(warnings)} warning(s)")
    return 1 if errors else 0


if __name__ == "__main__":
    sys.exit(main())
