import re
import yaml
from common import sections, split_front_matter


def norm(t):
    t = t.replace("`", "").replace("\\", "")
    t = re.sub(r"[\s\.\,;:!?\"']+", " ", t).strip().lower()
    return t


def main():
    problems = 0
    for s, (_, path) in enumerate(sections(), start=1):
        fm = yaml.safe_load(split_front_matter(open(path, encoding="utf-8").read())[0])
        for k, q in enumerate(fm["questions"], start=1):
            loc = f"s={s} {q['id']}"
            expl = q["explanation"]
            quoted = [norm(m) for m in re.findall(r'^\s*(?:- )?"(.+?)"(?= )', expl, flags=re.M)]
            wrong_section = expl.split("Wrong:")[1] if "Wrong:" in expl else expl
            for o in q["options"]:
                t = norm(o["text"])
                hits = [x for x in quoted if x and (t.startswith(x) or x.startswith(t))]
                if o.get("correct"):
                    if hits and q["type"] == "single":
                        print(f"{loc}: correct option quoted like a distractor: {o['text']}")
                        problems += 1
                else:
                    mentioned = hits or t[:25] in norm(wrong_section)
                    if not mentioned:
                        print(f"{loc}: distractor not addressed in explanation: {o['text']}")
                        problems += 1
            if q["type"] == "multiple":
                if "Correct:" not in expl or "Wrong:" not in expl:
                    print(f"{loc}: multiple question lacks Correct:/Wrong: structure")
                    problems += 1
                else:
                    corr_part = expl.split("Wrong:")[0]
                    for o in q["options"]:
                        if not o.get("correct") and norm(o["text"])[:30] in norm(corr_part) and norm(o["text"])[:30] not in norm(wrong_section):
                            print(f"{loc}: wrong option appears only in the Correct part: {o['text']}")
                            problems += 1
    print(f"audit: {problems} potential problem(s)")


if __name__ == "__main__":
    main()
