import pathlib, yaml, collections, itertools
SRC = pathlib.Path(__file__).parent / "src"
files = [f for ch in sorted(p for p in SRC.iterdir() if p.is_dir()) for f in sorted(ch.glob("*.md"))]
def dist(qs, s, ns):
    c = collections.Counter()
    for k, (q, n) in enumerate(zip(qs, ns), start=1):
        first = (k + s) % n
        for j in range(len(q["correct"])): c[(first + j) % n] += 1
    return c
for s, f in enumerate(files, start=1):
    if s not in (9, 28, 29): continue
    raw = f.read_text(); meta = yaml.safe_load(raw[3:raw.index("\n---", 3)])
    qs = meta["questions"]; ns = [len(q["correct"]) + len(q["distractors"]) for q in qs]
    singles=[i for i,q in enumerate(qs) if q["type"]=="single"]
    for a,b in itertools.combinations(singles,2):
        ns2=list(ns); ns2[a]=9-ns2[a]; ns2[b]=9-ns2[b]
        c=dist(qs,s,ns2)
        if max(c.values())/sum(c.values())<=0.4: print(s, qs[a]["id"], ns2[a], qs[b]["id"], ns2[b])
