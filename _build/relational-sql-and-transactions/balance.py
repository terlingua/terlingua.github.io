import sys, pathlib, yaml, collections, itertools
SRC = pathlib.Path(__file__).parent / "src"
files = [f for ch in sorted(p for p in SRC.iterdir() if p.is_dir()) for f in sorted(ch.glob("*.md"))]
def dist(qs, s, ns):
    c = collections.Counter()
    for k, (q, n) in enumerate(zip(qs, ns), start=1):
        first = (k + s) % n
        for j in range(len(q["correct"])): c[(first + j) % n] += 1
    return c
for s, f in enumerate(files, start=1):
    raw = f.read_text(); meta = yaml.safe_load(raw[3:raw.index("\n---", 3)])
    qs = meta["questions"]; ns = [len(q["correct"]) + len(q["distractors"]) for q in qs]
    c = dist(qs, s, ns); tot = sum(c.values()); top = max(c.values())
    if top / tot > 0.40:
        fixes = []
        for k, q in enumerate(qs):
            if q["type"] == "single":
                for newn in (4, 5):
                    if newn != ns[k]:
                        ns2 = list(ns); ns2[k] = newn; c2 = dist(qs, s, ns2)
                        if max(c2.values()) / sum(c2.values()) <= 0.40: fixes.append((k + 1, q["id"], newn))
        print(f"s{s} {f.name}: {dict(c)} fixes: {fixes}")
