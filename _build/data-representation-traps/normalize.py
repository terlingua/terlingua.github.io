import re
import sys
import unicodedata
import yaml
from common import sections, split_front_matter

INDENT_EXPL = "      "


def dq(value):
    if not isinstance(value, str):
        raise ValueError(f"expected string, got {value!r}")
    for ch in value:
        if ch in "\r\t":
            raise ValueError(f"control character in {value!r}")
        if ch != "\n" and unicodedata.category(ch) in ("Cf", "Cc", "Co", "Cs", "Mn", "Me"):
            raise ValueError(f"invisible/format character U+{ord(ch):04X} in {value!r}")
    return '"' + value.strip("\n").replace("\\", "\\\\").replace('"', '\\"').replace("\n", "\\n") + '"'


def block(value):
    lines = value.strip("\n").split("\n")
    if lines[0].startswith(" "):
        raise ValueError("explanation must not start with a space")
    for line in lines:
        for ch in line:
            if unicodedata.category(ch) in ("Cf", "Cc", "Co", "Cs", "Mn", "Me") and ch != "\t":
                raise ValueError(f"invisible character U+{ord(ch):04X} in explanation")
        if line.startswith("---"):
            raise ValueError("line starting with --- in explanation")
    return "|-\n" + "\n".join((INDENT_EXPL + l.rstrip()) if l.strip() else "" for l in lines)


CODE = re.compile(r"```.*?```|`[^`\n]*`", re.S)


def escape_md(text):
    """Escape Markdig emphasis-extra delimiters (^ superscript, == mark) outside code."""
    out, pos = [], 0
    for m in CODE.finditer(text):
        out.append(_esc(text[pos:m.start()]))
        out.append(m.group(0))
        pos = m.end()
    out.append(_esc(text[pos:]))
    return "".join(out)


def _esc(t):
    t = re.sub(r"(?<!\\)\^", r"\\^", t)
    t = re.sub(r"(?<![\\=])==", r"\\==", t)
    return t


def place(options, k, s):
    n = len(options)
    correct = [o for o in options if o.get("correct") is True]
    wrong = [o for o in options if o.get("correct") is not True]
    first = (k + s) % n
    targets = sorted((first + j) % n for j in range(len(correct)))
    out = [None] * n
    for t, o in zip(targets, correct):
        out[t] = o
    it = iter(wrong)
    for i in range(n):
        if out[i] is None:
            out[i] = next(it)
    return out


def emit(fm, body):
    lines = ["---", f"title: {dq(fm['title'])}", "questions:"]
    for q in fm["questions"]:
        lines.append(f"  - id: {dq(q['id'])}")
        lines.append(f"    type: {dq(q['type'])}")
        lines.append(f"    prompt: {dq(q['prompt'])}")
        lines.append(f"    explanation: {block(q['explanation'])}")
        lines.append("    options:")
        for o in q["options"]:
            lines.append(f"      - text: {dq(o['text'])}")
            if o.get("correct") is True:
                lines.append("        correct: true")
    lines.append("---")
    return "\n".join(lines) + "\n\n" + body.strip("\n") + "\n"


def main(only=None):
    for s, (_, path) in enumerate(sections(), start=1):
        if only and only not in path:
            continue
        raw = open(path, encoding="utf-8").read()
        yb, body = split_front_matter(raw)
        fm = yaml.safe_load(yb)
        for k, q in enumerate(fm["questions"], start=1):
            q["options"] = place(q["options"], k, s)
            q["prompt"] = escape_md(q["prompt"])
            q["explanation"] = escape_md(q["explanation"])
            for o in q["options"]:
                o["text"] = escape_md(o["text"])
        body = escape_md(body)
        out = emit(fm, body)
        check = yaml.safe_load(split_front_matter(out)[0])
        assert check == fm, f"round-trip mismatch in {path}"
        if out != raw:
            open(path, "w", encoding="utf-8").write(out)
            print(f"normalized s={s} {path}")


if __name__ == "__main__":
    main(sys.argv[1] if len(sys.argv) > 1 else None)
