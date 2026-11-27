import os
import re

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
BOOK = os.path.join(ROOT, "content", "books", "data-representation-traps")

NUMBERED = re.compile(r"^(\d+)[-_]([a-z0-9]+(?:-[a-z0-9]+)*)$")
SLUG = re.compile(r"^[a-z0-9]+(?:-[a-z0-9]+)*$")


def hidden(name):
    return name.startswith(".") or name.startswith("_")


def numbered(entries):
    out = []
    for path in entries:
        name = os.path.basename(path)
        if name.endswith(".md"):
            name = name[:-3]
        m = NUMBERED.match(name)
        out.append((int(m.group(1)) if m else -1, m.group(2) if m else None, path))
    return sorted(out, key=lambda e: (e[0], e[2]))


def chapters():
    dirs = [os.path.join(BOOK, d) for d in os.listdir(BOOK)
            if os.path.isdir(os.path.join(BOOK, d)) and not hidden(d)]
    return numbered(dirs)


def sections():
    result = []
    for _, _, cdir in chapters():
        files = [os.path.join(cdir, f) for f in os.listdir(cdir)
                 if f.endswith(".md") and not hidden(f)]
        for entry in numbered(files):
            result.append((cdir, entry[2]))
    return result


def split_front_matter(raw):
    if not raw.startswith("---"):
        return None, raw
    end = raw.find("\n---", 3)
    if end < 0:
        return None, raw
    yaml_block = raw[3:end].strip()
    body = raw[end + 4:].lstrip("\r\n")
    return yaml_block, body
