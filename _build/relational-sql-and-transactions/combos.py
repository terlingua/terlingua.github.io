import pathlib, yaml, collections, itertools, sys
exec(open('balance.py').read().split("for s, f in enumerate")[0])
want=set(int(x) for x in sys.argv[1:])
for s, f in enumerate(files, start=1):
    if s not in want: continue
    raw=f.read_text(); qs=yaml.safe_load(raw[3:raw.index("\n---",3)])["questions"]
    ns=[len(q["correct"])+len(q["distractors"]) for q in qs]
    singles=[k for k,q in enumerate(qs) if q["type"]=="single"]
    best=[]
    for combo in itertools.product([4,5],repeat=len(singles)):
        ns2=list(ns)
        for k,n in zip(singles,combo): ns2[k]=n
        c=dist(qs,s,ns2)
        if max(c.values())/sum(c.values())<=0.40:
            best.append((sum(1 for k in singles if ns2[k]!=ns[k]),[(qs[k]['id'],ns2[k]) for k in singles if ns2[k]!=ns[k]]))
    best.sort(); print(s, best[:6])
