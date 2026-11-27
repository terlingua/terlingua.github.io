---
title: "Three Fields and the Bias"
questions:
  - id: "binary32-field-widths"
    type: "single"
    prompt: "How are the 32 bits of an IEEE 754 binary32 (`float`) value divided?"
    explanation: |-
      IEEE 754-2019 defines binary32 as 1 sign bit, an 8-bit biased exponent field and a 23-bit trailing significand (fraction) field: 1 + 8 + 23 = 32.

      "1 sign, 8 exponent, 24 fraction" confuses the stored fraction with the precision. The precision is 24 bits, but only 23 are stored; the leading bit is implied by the exponent field (covered in the next section). 1 + 8 + 24 = 33 would not even fit in 32 bits.

      "1 sign, 7 exponent, 24 fraction" assumes a 7-bit exponent like some historical formats; with 7 bits the bias would be 63 and the range far smaller than the roughly 10\^±38 that `float` actually covers.

      "1 sign, 11 exponent, 20 fraction" borrows binary64's 11-bit exponent width and squeezes the fraction to make the total 32; no IEEE format has that layout.
    options:
      - text: "1 sign, 8 exponent, 24 fraction"
      - text: "1 sign, 7 exponent, 24 fraction"
      - text: "1 sign, 8 exponent, 23 fraction"
        correct: true
      - text: "1 sign, 11 exponent, 20 fraction"
  - id: "binary64-bias"
    type: "single"
    prompt: "binary64 (`double`) has an 11-bit exponent field. What is its exponent bias?"
    explanation: |-
      The bias of an IEEE 754 binary format with a k-bit exponent field is 2\^(k−1) − 1. For k = 11: 2\^10 − 1 = 1024 − 1 = 1023. A stored field E represents the exponent E − 1023 for normal numbers, so stored values 1…2046 cover exponents −1022…+1023.

      "1024" is 2\^10 without the − 1; with that bias the exponent range would be lopsided (−1023…+1022) and 1.0 would not be encoded as `0x3FF0000000000000`.

      "2047" is the all-ones field value 2\^11 − 1, which is reserved for infinities and NaNs, not the bias.

      "127" is the binary32 bias (2\^7 − 1 for k = 8), not the binary64 one.
    options:
      - text: "1024"
      - text: "2047"
      - text: "127"
      - text: "1023"
        correct: true
  - id: "stored-exponent-of-32"
    type: "single"
    prompt: "What unsigned value is stored in the 8-bit exponent field when `32.0f` is encoded as binary32?"
    explanation: |-
      32 = 1.0 × 2\^5, so the true exponent is 5. The stored field is the true exponent plus the bias: 5 + 127 = 132, which is `0b10000100` (`0x84`). The full pattern is sign 0, exponent `10000100`, fraction all zeros, i.e. `0x42000000`.

      "5" stores the true exponent without the bias, which is the two's-complement-style mistake the bias exists to avoid.

      "133" is an off-by-one that usually comes from writing 32 as 0.1 × 2\^6 (binary point after the leading 1 is the IEEE convention, so 1.0 × 2\^5 is the normalized form).

      "159" is 32 + 127, adding the bias to the value instead of to the exponent.
    options:
      - text: "132"
        correct: true
      - text: "5"
      - text: "133"
      - text: "159"
  - id: "xor-sign-bit-output"
    type: "single"
    prompt: "What does this C# snippet print?\n\n```csharp\nfloat x = 2.5f;\nint bits = BitConverter.SingleToInt32Bits(x) ^ unchecked((int)0x80000000);\nConsole.WriteLine(BitConverter.Int32BitsToSingle(bits));\n```"
    explanation: |-
      Bit 31 of a binary32 pattern is the sign bit and nothing else. XOR with `0x80000000` flips exactly that bit and leaves the exponent and fraction untouched, so the result is the same magnitude with the opposite sign: −2.5. IEEE 754 sign-magnitude encoding makes negation a pure bit operation (this is how `negate` is specified: it only changes the sign bit, even for zero and NaN).

      Trace: 2.5 = 1.25 × 2\^1 → sign 0, exponent 1 + 127 = 128 = `0x80`, fraction 0.25 → `0x40200000`. XOR `0x80000000` gives `0xC0200000`: sign 1, same exponent and fraction → −2.5.

      "2.5" assumes the XOR has no effect on a float; it does, because the bits are reinterpreted, not converted.

      "-1.25" assumes the flip also shifts the exponent; the exponent field is bits 30–23 and is not touched.

      "A huge negative number such as -1.07E+09" confuses the reinterpreted int (which is negative, −1071644672) with the float the code prints after converting the bits back.
    options:
      - text: "2.5"
      - text: "-2.5"
        correct: true
      - text: "-1.25"
      - text: "A huge negative number such as -1.07E+09"
  - id: "why-biased-exponents"
    type: "multiple"
    prompt: "Which statements about the biased exponent encoding are correct? Select all that apply."
    explanation: |-
      Correct:

      - Because the exponent field sits above the fraction and is stored as an unsigned biased value, larger non-negative floats have larger bit patterns when read as unsigned integers. That is why radix sorts and "next float" tricks work on the raw bits of non-negative values.
      - The all-zeros field (zero and subnormals) and the all-ones field (infinities and NaNs) are reserved; normal numbers use stored values 1 through 2\^k − 2, which is 1…254 in binary32.

      Wrong:

      - "The exponent is stored in two's complement, so negative exponents have the top bit set" is the misconception the bias replaces. With two's complement, the pattern for 2\^−1 would compare larger than the pattern for 2\^1.
      - "The binary32 bias is 128, half of the 256 possible field values" is off by one; the formula 2\^(k−1) − 1 gives 127.
      - "Negative floats also sort correctly when their bits are read as signed integers" fails because the encoding is sign-magnitude: −1.0 (`0xBF800000`) and −2.0 (`0xC0000000`) read as signed 32-bit integers are −1082130432 and −1073741824, so −2.0 would sort above −1.0. Sorting code must flip the remaining bits of negative values first.
    options:
      - text: "The exponent is stored in two's complement, so negative exponents have the top bit set."
      - text: "For non-negative values, comparing bit patterns as unsigned integers orders them by value."
        correct: true
      - text: "The all-zeros and all-ones exponent fields are reserved for special encodings."
        correct: true
      - text: "The binary32 bias is 128, half of the 256 possible field values."
      - text: "Negative floats also sort correctly when their bits are read as signed integers."
  - id: "binary64-pattern-of-one"
    type: "single"
    prompt: "What is the 64-bit hexadecimal pattern of the binary64 value `1.0`?"
    explanation: |-
      1.0 = 1.0 × 2\^0. Sign 0; stored exponent 0 + 1023 = 1023 = `0b01111111111`; fraction all zeros. The top 12 bits are therefore `0` followed by `01111111111`, i.e. `0011 1111 1111` = `0x3FF`, followed by 52 zero bits: `0x3FF0000000000000`.

      "0x3F80000000000000" is the binary32 pattern of 1.0 (`0x3F800000`) padded with zeros. Widening a float to a double re-encodes the exponent with the larger bias; it does not pad the bits.

      "0x4000000000000000" is 2.0: stored exponent 1024 = `0x400` in the top 12 bits.

      "0x0000000000000001" treats the value like an integer. Read as a binary64 it is the smallest subnormal, about 4.9 × 10\^−324.
    options:
      - text: "0x3F80000000000000"
      - text: "0x4000000000000000"
      - text: "0x0000000000000001"
      - text: "0x3FF0000000000000"
        correct: true
---

## Learning objectives

- Name the three fields of IEEE 754 binary32 and binary64 and their widths.
- Compute the exponent bias from the field width and apply it in both directions.
- Explain why a biased exponent makes non-negative floats sort like unsigned integers.
- Flip, test or extract the sign of a float with pure bit operations.

## The invariant

IEEE 754-2019 (the current revision of the floating-point standard) defines binary interchange formats as three bit fields, most significant first:

| Format | Total | Sign | Exponent field (w) | Trailing significand (t) | Bias = 2\^(w−1) − 1 |
|---|---|---|---|---|---|
| binary32 (`float`) | 32 | 1 | 8 | 23 | 127 |
| binary64 (`double`) | 64 | 1 | 11 | 52 | 1023 |

For a *normal* number with sign bit S, stored exponent E (neither all zeros nor all ones) and fraction F, the value is

(−1)\^S × 1.F × 2\^(E − bias)

where 1.F means "binary 1 followed by the fraction bits after the binary point". The two reserved field values are E = 0 (zero and subnormals) and E = all ones (infinities and NaNs); both get their own sections.

Three consequences follow directly from this layout and hold in every language and on every conforming CPU:

1. **The encoding is sign-magnitude.** The sign bit is independent of everything else. Negation, absolute value and copy-sign are defined by the standard as operations on that bit alone, so they are exact and never round. That is also why +0 and −0 both exist.
2. **The exponent is biased, not two's complement.** Storing E = e + bias turns the signed range of exponents into an unsigned field. Normal binary32 numbers use E = 1…254, meaning e = −126…+127.
3. **Non-negative floats are ordered like unsigned integers.** The exponent sits above the fraction, and a larger exponent always means a larger magnitude, so for x, y ≥ 0, x < y exactly when bits(x) < bits(y) as unsigned integers. Negative values are ordered in reverse, because their magnitude grows while the sign bit stays set.

Converting between the two formats is a re-encoding, not a reinterpretation: widening a binary32 to binary64 keeps the value, re-biases the exponent from 127 to 1023 and appends 29 zero fraction bits.

## Worked example

Encode 32.0 as binary32. 32 = 1.0 × 2\^5, so S = 0, e = 5, E = 5 + 127 = 132 = `10000100`, F = 0.

| Field | Sign | Exponent | Fraction |
|---|---|---|---|
| Bits | 0 | 10000100 | 00000000000000000000000 |

Grouped into nibbles the pattern is `0100 0010 0000 0000 …` = `0x42000000`. The same steps for 1.0 give E = 127 = `01111111`, pattern `0x3F800000`.

You can check this in any language by reinterpreting the bits rather than converting the value:

```python
import struct
for x in (1.0, 32.0, -2.5):
    print(x, struct.pack(">f", x).hex(), struct.pack(">d", x).hex())
# 1.0 3f800000 3ff0000000000000
# 32.0 42000000 4040000000000000
# -2.5 c0200000 c004000000000000
```

```csharp
Console.WriteLine($"{BitConverter.SingleToInt32Bits(32.0f):X8}");
Console.WriteLine($"{BitConverter.DoubleToInt64Bits(32.0):X16}");
```

prints `42000000` and `4040000000000000`. Note how 32.0 as binary64 has stored exponent 5 + 1023 = 1028 = `0x404`.

## Where this shows up today

As of November 2026, binary32 and binary64 are the hardware formats behind C#'s `float`/`double`, Java's `float`/`double`, JavaScript's `Number` (always binary64) and CPython's `float` (a C `double`, binary64 on every mainstream platform). Database `REAL`/`DOUBLE PRECISION` columns, Protocol Buffers' `float`/`double` and most GPU shader math use the same layouts. Engineers meet raw bit patterns when debugging serialization, writing hash or equality functions for floats, and reading binary file formats.

## Pitfalls checklist

- Reading the bias as 128 or 1024 (forgetting the − 1) and getting every decoded exponent off by one.
- Assuming the exponent is two's complement and expecting 2\^−1 to have the sign of its exponent visible in the top bit.
- Casting a float to an integer (value conversion) when you meant to reinterpret its bits, or the reverse.
- Sorting raw bit patterns of mixed-sign floats as signed integers; negative values come out in reverse order.
- Assuming a `double` holding a widened `float` has the `float`'s bits followed by zeros.

## Key takeaways

- binary32 is 1 + 8 + 23 bits with bias 127; binary64 is 1 + 11 + 52 bits with bias 1023.
- Value = (−1)\^S × 1.F × 2\^(E − bias) for normal numbers; E = 0 and E = all ones are reserved.
- Sign-magnitude makes negation and absolute value single-bit operations.
- Biased exponents make non-negative floats compare like unsigned integers.
