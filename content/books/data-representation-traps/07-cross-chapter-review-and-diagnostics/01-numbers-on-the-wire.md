---
title: "Review: Numbers on the Wire"
questions:
  - id: "latency-quantized-to-238-ns"
    type: "single"
    prompt: "Bug report: \"Our tracing library records `start` and `end` as Unix time in seconds stored in a `double` and reports `end - start`. Sub-microsecond spans show up only as 0, 0.238 µs, 0.477 µs or 0.715 µs, and once in a while a span is negative.\" What is the root cause, and what is the minimal correct fix?"
    explanation: |-
      Two invariants combine:

      - **Floating-point spacing (Chapter 2).** Unix time today is about 1.8 × 10\^9 s, in [2\^30, 2\^31). The binary64 ULP there is 2\^(30 − 52) = 2\^−22 s ≈ 238.4 ns, so every stored timestamp is a multiple of 238.4 ns and every difference is too: 0, 238 ns, 477 ns, 715 ns.
      - **Wall versus monotonic clocks (Chapter 5).** Unix time comes from the wall clock, which NTP can step backward, producing the occasional negative span.

      The fix addresses both: read a monotonic clock as an integer count of nanoseconds (for example `time.monotonic_ns()` or `Stopwatch.GetTimestamp()`) and subtract integers.

      "The CPU timer only ticks every 238 ns; nothing can be done" blames hardware; the quantum is exactly the binary64 ULP at this magnitude, which a hardware tick would not match.

      "Switch the timestamps to binary32 so the values are smaller" makes it far worse: the binary32 ULP at 1.8 × 10\^9 is 128 s.

      "Round each span to the nearest microsecond before reporting it" hides the symptom and keeps both the quantization and the negative spans.
    options:
      - text: "The CPU timer only ticks every 238 ns; nothing can be done."
      - text: "Switch the timestamps to binary32 so the values are smaller."
      - text: "Round each span to the nearest microsecond before reporting it."
      - text: "Binary64 spacing and clock steps; use monotonic integer ns."
        correct: true
  - id: "invoice-rounding-off-by-a-cent"
    type: "single"
    prompt: "Bug report: \"Three items at 1.005 each should total 3.015, which our policy rounds half-up to 3.02, but the invoice says 3.01.\" What does this Python expression return, and why?\n\n```python\nround(1.005 * 3, 2)\n```"
    explanation: |-
      The literal 1.005 is stored as the nearest binary64 value, 1.00499999999999989341858963598497211933135986328125, slightly below 1.005 (Chapter 2: decimal fractions with a factor 5 in the denominator are inexact). Multiplying by 3 and rounding gives 3.0149999999999997, which is below the halfway point 3.015, so rounding to two places gives 3.01. No tie occurs, so the rounding mode is irrelevant.

      Minimal fix: compute money in decimal or integer minor units and state the rounding mode, e.g. `(Decimal("1.005") * 3).quantize(Decimal("0.01"), rounding=ROUND_HALF_UP)` → `Decimal("3.02")`.

      "3.02, because `round()` rounds half up" is wrong on two counts: the value is not a tie, and Python's `round` uses half-even anyway.

      "3.01, because `round()` uses banker's rounding on the tie 3.015" misdiagnoses: half-even would give 3.02 for an exact 3.015 (2 is even), and there is no tie in binary.

      "3.015, because `round()` keeps three decimals for exact inputs" invents behavior; `round(x, 2)` always returns two decimals.
    options:
      - text: "3.01, because 1.005 × 3 is stored as 3.0149999999999997."
        correct: true
      - text: "3.02, because `round()` rounds half up."
      - text: "3.01, because `round()` uses banker's rounding on the tie 3.015."
      - text: "3.015, because `round()` keeps three decimals for exact inputs."
  - id: "nan-breaks-json-consumer"
    type: "single"
    prompt: "Bug report: \"The Python sensor service sends readings to a browser dashboard. Whenever a sensor is offline the dashboard shows nothing at all.\" The service does this:\n\n```python\njson.dumps({\"temp\": float(\"nan\")})   # '{\"temp\": NaN}'\n```\n\nWhat happens in the browser's `JSON.parse`, and what is the minimal fix?"
    explanation: |-
      RFC 8259 JSON has no literal for NaN or infinities (Chapter 2). Python's `json.dumps` emits the non-standard token `NaN` by default (`allow_nan=True`), and JavaScript's strict `JSON.parse` rejects it with a `SyntaxError`, so the whole payload is lost, not just one reading.

      Minimal fix: represent "no reading" explicitly, for example `None` → `null`, and call `json.dumps(..., allow_nan=False)` so any stray NaN fails at the producer instead of the consumer.

      "It parses to the number `NaN`, so the bug is in the chart code" assumes JavaScript accepts the token because `NaN` exists in JavaScript; JSON is a separate grammar.

      "It parses to null" describes what `JSON.stringify(NaN)` produces in the other direction.

      "It parses to the string `"NaN"`; the chart cannot plot strings" assumes a lenient coercion that `JSON.parse` does not perform.
    options:
      - text: "It parses to the number `NaN`, so the bug is in the chart code."
      - text: "It throws `SyntaxError`; send `null` and set `allow_nan=False`."
        correct: true
      - text: "It parses to `null`, and the chart silently skips null values."
      - text: "It parses to the string `\"NaN\"`; the chart cannot plot strings."
  - id: "float32-longitude-resolution"
    type: "single"
    prompt: "Bug report: \"Delivery pins snap to a grid.\" The mobile app stores longitude as a binary32 `float`. Near −122.4° (San Francisco), what is the spacing between representable longitudes, and roughly how far is that on the ground at the equator (1° ≈ 111.32 km)?"
    explanation: |-
      Chapter 1 and 2 arithmetic: |−122.4| lies in [64, 128) = [2\^6, 2\^7), so the binary32 ULP is 2\^(6 − 23) = 2\^−17 ≈ 7.63 × 10\^−6 degrees. On the ground: 7.63 × 10\^−6 × 111,320 m ≈ 0.85 m at the equator (about 0.67 m at San Francisco's latitude, where a degree of longitude is shorter). Pins therefore land on a grid of under a metre, which is visible at high zoom. binary64 gives 2\^(6 − 52) ≈ 1.4 × 10\^−14 degrees, far below a millimetre.

      "About 1 millimetre" and "About 1 centimetre" are binary32 spacings only for longitudes below roughly 0.1° and 1° respectively; the spacing grows with magnitude, and 122° is in a much higher binade.

      "About 85 metres" is 100 times the real spacing; binary32 spacing reaches that size (about 7.6 × 10\^−4 degrees) only for magnitudes in the thousands.
    options:
      - text: "About 1 millimetre"
      - text: "About 1 centimetre"
      - text: "About 0.85 metres"
        correct: true
      - text: "About 85 metres"
  - id: "wire-format-practices"
    type: "multiple"
    prompt: "A team is writing JSON API guidelines after these incidents. Which rules prevent the numeric bugs in this section? Select all that apply."
    explanation: |-
      Correct:

      - 64-bit identifiers as JSON strings: binary64 parsers (JavaScript's `JSON.parse`) cannot hold integers above 2\^53 exactly (Chapter 2).
      - Money as decimal strings or integer minor units with a currency code: binary floats cannot represent most decimal amounts and hide rounding (Chapter 2).
      - Timestamps as RFC 3339 strings with an offset, or integer epoch units with the unit named: no floating-point quantization, no naive local times (Chapters 5 and 6).

      Wrong:

      - "Send durations as binary32 seconds to save space" loses precision quickly: at 10\^5 s (about a day) the binary32 ULP is 2\^(16 − 23) ≈ 7.8 ms.
      - "Send missing measurements as the bare token `NaN`" is not valid JSON and breaks strict parsers.
    options:
      - text: "Send durations as binary32 seconds to save space."
      - text: "Send 64-bit IDs as JSON strings."
        correct: true
      - text: "Send money as decimal strings or integer minor units."
        correct: true
      - text: "Send timestamps as RFC 3339 strings or named integer epoch units."
        correct: true
      - text: "Send missing measurements as the bare token `NaN`."
  - id: "flaky-parallel-sum-test"
    type: "single"
    prompt: "Bug report: \"`assert total == 0.6` passes on a developer laptop and fails on the 32-core CI runner, where `total` prints `0.6000000000000001`.\" The total is computed with a parallel reduction whose chunking depends on the core count. What is the root cause, and the minimal fix?"
    explanation: |-
      Floating-point addition is not associative (Chapter 2): `(0.1 + 0.2) + 0.3` is 0.6000000000000001 while `0.1 + (0.2 + 0.3)` is 0.6. A parallel reduction groups the additions differently for different chunk counts, so the last bit of the result depends on the machine. The test encodes an exact-equality expectation that floating point never promised.

      Minimal fix: compare with a tolerance suited to the magnitude (`math.isclose(total, 0.6)`), or, if bit-reproducibility is a requirement, use a deterministic reduction order or an exact/compensated summation.

      "The CI runner's CPU has a floating-point bug" blames hardware; IEEE 754 operations are correctly rounded on both machines, but they are different operations in a different order.

      "A race condition corrupts `total` when many cores run" is unlikely given a one-ULP difference; a race would produce lost updates, not a last-bit change.

      "The CI image uses binary32 by default" is not a thing: the language decides the type, and binary32 would differ far more than one binary64 ULP.
    options:
      - text: "Reduction order differs; compare with a tolerance."
        correct: true
      - text: "The CI runner's CPU has a floating-point bug."
      - text: "A race condition corrupts `total` when many cores run."
      - text: "The CI image uses binary32 by default."
---

## Learning objectives

- Diagnose numeric bugs that appear only after values cross a process or format boundary.
- Combine floating-point spacing, special values and rounding with serialization and clock rules.
- Choose minimal fixes that remove the root cause rather than the symptom.

## The invariant

Every bug in this section is a representation changing at a boundary. When a number crosses a boundary, three questions locate the root cause:

1. **What type holds it on each side?** binary64, binary32, decimal, integer, or text. The weakest type in the chain sets the precision: 53 bits for a JavaScript number, 24 for a `float`, 2\^53 for JSON integers parsed by `JSON.parse`.
2. **What values can the format express?** JSON has no NaN or infinity; decimal strings can be exact; binary floats cannot represent most decimal fractions; integers have hard ranges.
3. **What does the value mean?** An instant needs a clock and a time scale; an amount needs a currency and a rounding mode; an identifier needs exact equality, not arithmetic.

Applying the earlier chapters' invariants:

| Symptom | Invariant | Chapter |
|---|---|---|
| Values quantized to an odd step | ULP(x) = 2\^(e − p + 1) at the value's magnitude | 1, 2 |
| Off-by-one-cent totals | decimal literals are rounded into binary; round once, explicitly | 2 |
| Payload rejected | NaN/∞ have no JSON encoding | 2 |
| Last-bit differences across machines | non-associative addition under different reduction orders | 2 |
| Negative durations | wall clocks step; durations need monotonic clocks | 5 |

The minimal fix usually changes the representation (integer, decimal, string, monotonic clock), not the arithmetic around it.

A practical diagnostic routine follows from this: reproduce the bad value, print it exactly (hex pattern, `Decimal(x)`, or the raw bytes on the wire), and compare it with the value one step earlier in the pipeline. The step where the exact value first changes is the boundary with the weak representation. Symptoms that look random (a cent here, a few hundred nanoseconds there, one failed parse per day) are almost always deterministic consequences of a fixed rule applied at that boundary, which is why they can be fixed for good rather than patched with tolerances or retries.

## Worked example

A trace span recorded as two binary64 Unix-second timestamps:

| Step | Value |
|---|---|
| start (seconds) | 1795737600.0000001 → stored as 1795737600.0 (ULP 2\^−22 ≈ 238 ns) |
| end (seconds) | 1795737600.0000003 → stored as 1795737600.0000002384 |
| end − start | 2.384185791015625e-07 s (one ULP), not 2e-07 |

```python
import math, time
print(math.ulp(1_795_737_600.0))                 # 2.384185791015625e-07
t0 = time.monotonic_ns()
t1 = time.monotonic_ns()
print(t1 - t0 >= 0)                              # True: integer nanoseconds, monotonic
```

```csharp
long t0 = System.Diagnostics.Stopwatch.GetTimestamp();
long t1 = System.Diagnostics.Stopwatch.GetTimestamp();
TimeSpan span = System.Diagnostics.Stopwatch.GetElapsedTime(t0, t1);  // .NET 7 and later
```

## Where this shows up today

As of November 2026, observability formats commonly carry timestamps as integer nanoseconds since the epoch (OpenTelemetry's protocol uses unsigned 64-bit nanoseconds), precisely to avoid the binary64 quantization shown here, while many dashboards still convert to JavaScript numbers for display.

## Pitfalls checklist

- Converting integer nanoseconds or IDs to `double` "just for display" and then reusing the value.
- Letting a language's JSON encoder emit non-standard tokens.
- Asserting exact equality on results of parallel or reordered floating-point work.
- Storing coordinates, timestamps or money in binary32 to save space.
- Rounding money implicitly through `round()` on binary floats.

## Key takeaways

- Find the weakest representation in the chain; it determines the bug.
- binary64 Unix seconds are quantized to about 238 ns today; use integer monotonic nanoseconds for durations.
- Money: decimal or integer minor units, one explicit rounding step.
- JSON: IDs and money as strings, no NaN, timestamps in RFC 3339.
