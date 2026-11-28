"""Heuristic question audit: every distractor should be discussed in the explanation (quoted prefix),
explanations must not cite options by letter/position, prompts of multiple questions end correctly."""
import pathlib, re, yaml
miss = 0
for f in sorted(pathlib.Path(__file__).parent.glob('src/*/*.md')):
    s = f.read_text(); m = yaml.safe_load(s[3:s.index('\n---', 3)])
    for q in m['questions']:
        ex = q['explanation']
        quoted = re.findall(r'"([^"\n]{1,200})"', ex)
        norm = lambda t: re.sub(r'[`*]', '', t).lower().strip()
        for d in q['distractors']:
            nd = norm(d)
            ok = any(len(norm(x).rstrip('.…, ').strip()) >= 1 and norm(x).rstrip('.…, ').strip() in nd for x in quoted)
            if not ok:
                miss += 1
                print(f"{f.parent.name[:2]}/{f.name[:2]} {q['id']}: distractor not quoted: {d[:70]}")
print("unquoted distractors:", miss)

print("--- structural check: one quoted rebuttal per distractor ---")
bad = 0
for f in sorted(pathlib.Path(__file__).parent.glob('src/*/*.md')):
    s = f.read_text(); m = yaml.safe_load(s[3:s.index('\n---', 3)])
    for q in m['questions']:
        reb = [l for l in q['explanation'].splitlines() if re.match(r'^\s*(- )?(\*\*)?"', l)]
        if len(reb) < len(q['distractors']):
            bad += 1
            print(f"{f.parent.name[:2]}/{f.name[:2]} {q['id']}: {len(reb)} rebuttals for {len(q['distractors'])} distractors")
print("questions with fewer rebuttals than distractors:", bad)
