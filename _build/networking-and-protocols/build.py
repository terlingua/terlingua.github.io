#!/usr/bin/env python3
"""Emit content/books/networking-and-protocols section files from _build sources.

Source files: _build/networking-and-protocols/src/NN-chapter/NN-section.md
Front matter keys per question: id, type, prompt, explanation, correct (list), distractors (list),
checks (list, used only by check_sql.py). Options are placed by the answer-position rule:
first correct option at index (k + s) mod n, further correct options at the next indexes (mod n).
"""
import json
import pathlib
import sys

import yaml

ROOT = pathlib.Path(__file__).resolve().parents[2]
BUILD = ROOT / "_build" / "networking-and-protocols"
SRC = BUILD / "src"
OUT = ROOT / "content" / "books" / "networking-and-protocols"


def split_front_matter(raw: str):
    if not raw.startswith("---"):
        raise ValueError("missing front matter")
    end = raw.index("\n---", 3)
    return raw[3:end].strip(), raw[end + 4:].lstrip("\r\n")


def q(s: str) -> str:
    return json.dumps(s, ensure_ascii=False)


def section_sources():
    files = []
    for chapter in sorted(p for p in SRC.iterdir() if p.is_dir()):
        for f in sorted(chapter.glob("*.md")):
            files.append(f)
    return files


def place(correct, distractors, k, s):
    n = len(correct) + len(distractors)
    first = (k + s) % n
    slots = [None] * n
    for j, text in enumerate(correct):
        slots[(first + j) % n] = (text, True)
    it = iter(distractors)
    for i in range(n):
        if slots[i] is None:
            slots[i] = (next(it), False)
    return slots


def render(meta, body, s):
    lines = ["---", f"title: {q(meta['title'])}", "questions:"]
    for k, question in enumerate(meta["questions"], start=1):
        lines.append(f"  - id: {q(question['id'])}")
        lines.append(f"    type: {q(question['type'])}")
        lines.append(f"    prompt: {q(question['prompt'].strip())}")
        lines.append("    explanation: |-")
        for line in question["explanation"].strip("\n").split("\n"):
            lines.append(("      " + line) if line.strip() else "")
        lines.append("    options:")
        for text, correct in place(question["correct"], question["distractors"], k, s):
            lines.append(f"      - text: {q(text)}")
            if correct:
                lines.append("        correct: true")
    lines.append("---")
    return "\n".join(lines) + "\n" + body.rstrip() + "\n"


def main():
    sources = section_sources()
    written = 0
    for s, src in enumerate(sources, start=1):
        fm, body = split_front_matter(src.read_text(encoding="utf-8"))
        meta = yaml.safe_load(fm)
        out = OUT / src.parent.name / src.name
        out.parent.mkdir(parents=True, exist_ok=True)
        out.write_text(render(meta, body, s), encoding="utf-8")
        written += 1
    print(f"built {written} section file(s)")
    return 0


if __name__ == "__main__":
    sys.exit(main())
