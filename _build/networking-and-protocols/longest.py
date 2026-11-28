import pathlib,yaml,collections
per=collections.Counter(); tot=0; rows=[]
for f in sorted(pathlib.Path('src').glob('*/*.md')):
    s=f.read_text(); m=yaml.safe_load(s[3:s.index('\n---',3)])
    for q in m['questions']:
        lc=max(len(t) for t in q['correct']); ld=max(len(t) for t in q['distractors'])
        if lc>ld:
            per[f.parent.name[:2]+'/'+f.name[:2]]+=1; tot+=1
            rows.append((f.parent.name[:2]+'/'+f.name[:2], q['id'], lc, ld))
for r in rows: print(*r)
print(tot, dict(per))
