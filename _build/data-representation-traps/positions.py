import sys
from collections import Counter
s = int(sys.argv[1])
spec = sys.argv[2:]  # like 4 4 5:2 4 5:3
allc, first = Counter(), Counter()
tot = 0
for k, item in enumerate(spec, 1):
    n, c = (item.split(":") + ["1"])[:2]
    n, c = int(n), int(c)
    f = (k + s) % n
    first[f] += 1
    for j in range(c):
        allc[(f + j) % n] += 1
        tot += 1
    print(f"k={k} n={n} first={f}")
print("all", dict(allc), max(allc.values()) / tot, "first", dict(first), max(first.values()) / len(spec))
