import re, difflib, yaml
from common import sections, split_front_matter

def key(t):
    return re.sub(r"[`\\]", "", t).rstrip(".").strip()

changed = 0
for s, (_, path) in enumerate(sections(), 1):
    raw = open(path, encoding="utf-8").read()
    yb, body = split_front_matter(raw)
    fm = yaml.safe_load(yb)
    new_raw = raw
    for q in fm["questions"]:
        distractors = [o["text"] for o in q["options"] if not o.get("correct")]
        expl = q["explanation"]
        new_expl = expl
        for m in re.finditer(r'^(\s*(?:- )?)"(.+?)"(?= )', expl, flags=re.M):
            quoted = m.group(2)
            best, score = None, 0
            for d in distractors:
                r = difflib.SequenceMatcher(None, key(quoted).lower(), key(d).lower()).ratio()
                if r > score:
                    best, score = d, r
            target = best.rstrip(".") if best else None
            if best and score >= 0.55 and quoted != target:
                new_expl = new_expl.replace(f'"{quoted}"', f'"{target}"', 1)
                print(f"s={s} {q['id']}: {quoted!r} -> {target!r} ({score:.2f})")
        if new_expl != expl:
            old_block = "\n".join(("      " + l) if l.strip() else "" for l in expl.split("\n"))
            new_block = "\n".join(("      " + l) if l.strip() else "" for l in new_expl.split("\n"))
            assert old_block in new_raw, q["id"]
            new_raw = new_raw.replace(old_block, new_block, 1)
            changed += 1
    if new_raw != raw:
        open(path, "w", encoding="utf-8").write(new_raw)
print("questions changed:", changed)
