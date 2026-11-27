---
title: "Cancellation and Non-Associativity"
questions:
  - id: "association-order-output"
    type: "single"
    prompt: "What does this Python line print?\n\n```python\nprint((0.1 + 0.2) + 0.3, 0.1 + (0.2 + 0.3))\n```"
    explanation: |-
      Each `+` rounds its exact result to the nearest double, so the grouping decides which rounding errors occur.

      - Left: `0.1 + 0.2` rounds to 0.3000000000000000444…; adding the double `0.3` (0.29999999999999998889…) gives an exact sum of 0.6000000000000000333…, which rounds to the double printed as `0.6000000000000001`.
      - Right: `0.2 + 0.3` happens to round to exactly the double `0.5` (the errors of the two literals cancel), and `0.1 + 0.5` rounds to the double printed as `0.6`.

      Output: `0.6000000000000001 0.6`. Floating-point addition is commutative (a + b \== b + a always) but not associative.

      "0.6 0.6" assumes associativity, which holds for real numbers but not for rounded arithmetic.

      "0.6000000000000001 0.6000000000000001" assumes the error comes only from the literals and is therefore the same either way.

      "0.6 0.6000000000000001" swaps the results; it is the left grouping that inherits the extra error from `0.1 + 0.2`.
    options:
      - text: "0.6000000000000001 0.6"
        correct: true
      - text: "0.6 0.6"
      - text: "0.6000000000000001 0.6000000000000001"
      - text: "0.6 0.6000000000000001"
  - id: "absorbed-addend"
    type: "single"
    prompt: "In binary64, what is the value of `(1e16 + 1.0) - 1e16`?"
    explanation: |-
      The ULP at 1e16 is 2 (1e16 lies in [2\^53, 2\^54)). The exact sum 1e16 + 1 is exactly halfway between 1e16 and 1e16 + 2. Ties-to-even: 1e16 = 5 × 10\^15 × 2, and its significand in units of the ULP, 5,000,000,000,000,000, is even, so the sum rounds to 1e16. Subtracting 1e16 then gives exactly 0.0. The 1.0 was absorbed, and the subtraction exposed the loss as a 100% error.

      "1.0" is the real-number answer; it assumes the intermediate sum was exact.

      "2.0" is what you get if the tie went upward to 1e16 + 2; ties go to the even significand here.

      "1e-16" confuses machine epsilon with the absolute spacing at this magnitude.
    options:
      - text: "1.0"
      - text: "0.0"
        correct: true
      - text: "2.0"
      - text: "1e-16"
  - id: "relative-error-after-cancellation"
    type: "single"
    prompt: "Two sensor readings, `a = 1000.0001` and `b = 1000.0000`, are each known only to a relative accuracy of 1e-8 (an absolute uncertainty of up to 1e-5 each). What is the worst-case relative error of `a - b`?"
    explanation: |-
      The subtraction itself is exact here (Sterbenz: if b/2 ≤ a ≤ 2b, a − b is exact in floating point), but it cannot remove the errors already in its inputs. Each input carries up to 1000 × 10\^−8 = 10\^−5 absolute error. The difference is 10\^−4, and its worst-case absolute error is the sum 2 × 10\^−5. Relative error: 2 × 10\^−5 / 10\^−4 = 0.2 = 20%.

      This is catastrophic cancellation: the leading digits that agree cancel, leaving the noisy trailing digits as the result.

      "About 1e-8" assumes relative errors carry through subtraction unchanged; they are amplified by |a| / |a − b| = 10\^7 here.

      "About 2e-8" adds the two input relative errors without that amplification factor.

      "Zero, because the subtraction is exact" confuses the rounding of the operation with the uncertainty of the operands.
    options:
      - text: "About 1e-8"
      - text: "About 2e-8"
      - text: "About 20%"
        correct: true
      - text: "Zero, because the subtraction is exact"
  - id: "quadratic-small-root"
    type: "single"
    prompt: "For x² + 1e8·x + 1 = 0 the small root is about `-1e-8`. This code returns `-7.450580596923828e-09`. Which change computes the small root accurately?\n\n```python\nimport math\na, b, c = 1.0, 1e8, 1.0\nd = math.sqrt(b*b - 4*a*c)\nsmall = (-b + d) / (2*a)\n```"
    explanation: |-
      `d` = 99999999.99999999 is within one ULP of `b`, so `-b + d` cancels almost every significant bit and leaves a single rounding unit (about −1.49 × 10\^−8), giving −7.45 × 10\^−9, which is 25% off. Using the identity x1 × x2 = c / a, compute the large root without cancellation (−b − d has the same signs, so nothing cancels) and derive the small one: `small = (2*c) / (-b - d)` = 2 / (−2 × 10\^8) = −1e-08 exactly.

      "Compute in float32 so the error is smaller" makes precision worse (24 bits instead of 53).

      "Add a tiny epsilon to `d` before subtracting" changes the answer by an arbitrary amount and does not recover the lost bits.

      "Swap the operands to compute `d - b` instead" is the same subtraction with the same cancellation; reordering two terms does not change rounding of their difference.
    options:
      - text: "Compute in float32 so the error is smaller."
      - text: "Add a tiny epsilon to `d` before subtracting."
      - text: "Swap the operands to compute `d - b` instead."
      - text: "Use `small = (2*c) / (-b - d)`."
        correct: true
  - id: "kahan-versus-naive-trace"
    type: "single"
    prompt: "What do these two loops return for `xs = [1e16, 1.0, 1.0]`?\n\n```python\ndef naive(xs):\n    s = 0.0\n    for x in xs:\n        s += x\n    return s\n\ndef kahan(xs):\n    s, c = 0.0, 0.0\n    for x in xs:\n        y = x - c\n        t = s + y\n        c = (t - s) - y\n        s = t\n    return s\n```"
    explanation: |-
      Naive: s = 1e16; s + 1.0 ties and rounds to 1e16 (ULP is 2, even significand wins); the second 1.0 is lost the same way. Result `1e+16`.

      Kahan trace:

      1. x = 1e16: y = 1e16, t = 1e16, c = (1e16 − 0) − 1e16 = 0, s = 1e16.
      2. x = 1.0: y = 1.0, t = 1e16 + 1 → 1e16, c = (1e16 − 1e16) − 1.0 = −1.0 (the lost part, negated), s = 1e16.
      3. x = 1.0: y = 1.0 − (−1.0) = 2.0, t = 1e16 + 2 = 10000000000000002 (exact, one ULP up), c = 2.0 − 2.0 = 0, s = 1.0000000000000002e16.

      Results: `1e+16` and `1.0000000000000002e+16`. The compensation term `c` carries the rounding error of each addition into the next one.

      "1.0000000000000002e+16 for both" assumes the naive loop is exact.

      "1e+16 for both" assumes the compensation cannot recover anything; it recovers exactly the error that rounding discarded.

      "1e+16 and 1.0000000000000001e+16" invents a value that does not exist: the doubles next to 1e16 are 2 apart.

      "1.0000000000000002e+16 and 1e+16" swaps the two results.
    options:
      - text: "1.0000000000000002e+16 for both"
      - text: "1e+16 for both"
      - text: "1e+16 and 1.0000000000000002e+16"
        correct: true
      - text: "1e+16 and 1.0000000000000001e+16"
      - text: "1.0000000000000002e+16 and 1e+16"
  - id: "summation-strategies"
    type: "multiple"
    prompt: "You must sum ten million doubles of mixed magnitudes. Which techniques reduce the accumulated rounding error compared with a simple left-to-right loop? Select all that apply."
    explanation: |-
      A left-to-right loop has a worst-case error bound that grows roughly in proportion to n (each of n − 1 additions can add half an ULP of a growing partial sum).

      Correct:

      - Pairwise (cascade) summation adds numbers in a balanced tree; the error bound grows like log2(n) instead of n.
      - Kahan or Neumaier compensated summation carries each addition's rounding error forward, giving an error bound essentially independent of n (Neumaier also handles an addend larger than the running sum).
      - For values of one sign, adding in order of increasing magnitude keeps small terms from being absorbed by a large partial sum early.

      Wrong:

      - "Accumulate in float32 for speed, then convert the total to double" uses 24-bit precision, so absorption starts at much smaller ratios of sum to addend.
      - "Shuffle the input randomly before summing" changes which errors occur but does not reduce their expected size in a way you can rely on; it also makes results non-reproducible.
    options:
      - text: "Pairwise (tree) summation."
        correct: true
      - text: "Accumulate in float32 for speed, then convert the total to double."
      - text: "Shuffle the input randomly before summing."
      - text: "Kahan or Neumaier compensated summation."
        correct: true
      - text: "For same-sign values, adding the smallest magnitudes first."
        correct: true
---

## Learning objectives

- Explain why floating-point addition is commutative but not associative.
- Predict when a small addend is absorbed by a large one.
- Quantify the relative error amplification of subtracting nearly equal numbers.
- Choose reformulations and summation algorithms that limit error growth.

## The invariant

Every IEEE 754 operation is correctly rounded: fl(a ⊕ b) = (a + b)(1 + δ) with |δ| ≤ u, where u = 2\^−53 for binary64. Two consequences follow, and neither depends on language or hardware:

1. **Order matters.** fl(fl(a + b) + c) and fl(a + fl(b + c)) round different intermediate values, so they can differ. Commutativity survives (a + b and b + a are the same exact sum, rounded once), associativity and distributivity do not.
2. **Subtraction amplifies existing error.** If a and b carry relative errors εa and εb, then a − b carries a relative error up to (|a|εa + |b|εb) / |a − b|. When a ≈ b, the denominator is tiny and the error explodes. The subtraction itself is exact when b/2 ≤ a ≤ 2b (the Sterbenz lemma). The damage comes from the *inputs*, which were rounded before. Goldberg calls this catastrophic cancellation; when the inputs are exact it is merely benign cancellation.

Absorption is the extreme case of (1): if |y| is at most half an ULP of x, then x + y \== x. Repeated in a loop, an accumulator can stop growing entirely.

Two families of remedies follow from the invariant:

- **Reformulate to avoid subtracting nearly equal quantities.** Standard examples: the small root of a quadratic via x1 = c / (a·x2); `log1p(x)` instead of `log(1 + x)`; `expm1(x)` instead of `exp(x) − 1`; and computing a variance in one pass with Welford's update instead of E[x²] − E[x]².
- **Sum more carefully.** Pairwise summation bounds the error by O(u log n · Σ|xi|); Kahan/Neumaier compensated summation by about 2u · Σ|xi| plus a second-order term, independent of n; exact algorithms such as Python's `math.fsum` return the correctly rounded sum.

## Worked example

The quadratic x² + 10\^8·x + 1 = 0 has roots near −10\^8 and −10\^−8.

| Step | Naive small root | Stable small root |
|---|---|---|
| d = √(b² − 4ac) | 99999999.99999999 | 99999999.99999999 |
| numerator | −b + d = −1.4901161193847656e-08 (cancellation) | −b − d rounds to −2e8 (no cancellation) |
| result | −7.450580596923828e-09 (25% error) | 2c / (−b − d) = −1e-08 |

Summation of `[1e16, 1.0, 1.0]`:

```python
import math
xs = [1e16, 1.0, 1.0]
total = 0.0
for x in xs:
    total += x
print(total)          # 1e+16                    plain loop absorbs both 1.0s
print(math.fsum(xs))  # 1.0000000000000002e+16   correctly rounded
```

```csharp
double total = 0;
foreach (var x in new[] { 1e16, 1.0, 1.0 }) total += x;
Console.WriteLine(total);   // 1E+16
```

## Where this shows up today

As of November 2026, Python's built-in `sum()` has used Neumaier compensated summation for floats since Python 3.12, and NumPy's `np.sum` documentation describes partial pairwise summation for floating-point arrays, so a plain hand-written loop can disagree with both in the last digits. Parallel and GPU reductions change the association order with the thread or block count, which is why the same job can produce slightly different totals on different hardware.

## Pitfalls checklist

- Expecting bit-identical results after reordering a sum or parallelizing a reduction.
- Computing variance as mean of squares minus square of mean on data with a large mean.
- Writing `log(1 + x)` or `exp(x) - 1` for small x instead of `log1p`/`expm1`.
- Assuming an exact subtraction yields an accurate result when its inputs were rounded.
- Accumulating long sums in a single naive running total, especially in binary32.

## Key takeaways

- Floating-point addition is commutative but not associative; grouping changes results.
- Absorption: anything below half an ULP of the running total disappears.
- Subtracting nearly equal rounded values amplifies their relative error by |a| / |a − b|.
- Reformulate to avoid cancellation; use pairwise, compensated or exact summation for long sums.
