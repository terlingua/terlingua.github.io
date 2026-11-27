---
title: "Special Encodings and Small Formats"
questions:
  - id: "decode-ff800000"
    type: "single"
    prompt: "What does this Python line print?\n\n```python\nimport struct\nprint(struct.unpack(\">f\", bytes.fromhex(\"ff800000\"))[0])\n```"
    explanation: |-
      `struct.unpack(">f", …)` reinterprets the four bytes as a big-endian binary32. Fields of `0xFF800000` = `1111 1111 1000 0000 …`: sign 1, exponent `11111111` (all ones), fraction 0. An all-ones exponent with a zero fraction is an infinity; the sign bit makes it −infinity, which Python prints as `-inf`.

      "nan" is wrong because a NaN needs an all-ones exponent *and* a non-zero fraction. Here the fraction is zero.

      "-0.0" would need an all-zeros exponent and fraction: `0x80000000`.

      "-3.4028234663852886e+38" is the most negative finite binary32, `0xFF7FFFFF` (exponent 254, fraction all ones), one step below this pattern.
    options:
      - text: "nan"
      - text: "-inf"
        correct: true
      - text: "-0.0"
      - text: "-3.4028234663852886e+38"
  - id: "signaling-or-quiet"
    type: "single"
    prompt: "Using the encoding recommended by IEEE 754-2008 and 2019 (most significant fraction bit = 1 means quiet), what is the binary32 pattern `0x7FA00000`?"
    explanation: |-
      Split `0x7FA00000` = `0111 1111 1010 0000 …`: sign 0, exponent `11111111`, fraction `0100000…` (non-zero). All-ones exponent with a non-zero fraction is a NaN. The recommended rule looks at the fraction's most significant bit (bit 22): here it is 0, so this is a **signaling** NaN; the remaining fraction bits (`100…`) are its payload and keep it distinct from infinity.

      "A quiet NaN" would need bit 22 set, as in `0x7FC00000`, the usual default quiet NaN.

      "+infinity" needs a zero fraction (`0x7F800000`).

      "The largest finite float" has exponent `11111110`, not all ones (`0x7F7FFFFF`).
    options:
      - text: "A quiet NaN"
      - text: "+infinity"
      - text: "A signaling NaN"
        correct: true
      - text: "The largest finite float"
  - id: "count-nan-patterns"
    type: "single"
    prompt: "How many distinct binary32 bit patterns are NaNs?"
    explanation: |-
      A NaN is any pattern with the exponent field all ones and a non-zero fraction. The fraction has 23 bits, so there are 2\^23 − 1 non-zero fractions; each can be combined with either sign bit: 2 × (2\^23 − 1) = 2\^24 − 2 = 16,777,214.

      "8,388,607" = 2\^23 − 1 forgets that the sign bit can be 0 or 1 for NaNs too (the sign of a NaN carries no meaning but still exists in the bits).

      "16,777,216" = 2\^24 counts the two zero-fraction patterns, which are ±infinity, not NaNs.

      "2" confuses the bit level with the conceptual "quiet NaN and signaling NaN" classes; each class spans millions of payloads.
    options:
      - text: "8,388,607"
      - text: "16,777,216"
      - text: "2"
      - text: "16,777,214"
        correct: true
  - id: "binary16-maximum"
    type: "single"
    prompt: "IEEE 754 binary16 (half precision) has 1 sign bit, 5 exponent bits (bias 15) and 10 fraction bits. What is its largest finite value?"
    explanation: |-
      The largest normal stored exponent is 2\^5 − 2 = 30 (31 is reserved), so e = 30 − 15 = 15. The largest significand is 1.1111111111 (ten ones) = 2 − 2\^−10. Value = (2 − 2\^−10) × 2\^15 = 65536 − 32 = 65504.

      "65535" assumes the format behaves like an unsigned 16-bit integer; only 11 bits of precision exist, so near 2\^15 the spacing is 2\^(15−10) = 32.

      "65536" = 2\^16 is out of range: it would need e = 16, which is the reserved all-ones exponent (infinity).

      "32768" = 2\^15 ignores the fraction bits of the largest significand.
    options:
      - text: "65504"
        correct: true
      - text: "65535"
      - text: "65536"
      - text: "32768"
  - id: "bfloat16-of-pi"
    type: "single"
    prompt: "bfloat16 is the upper 16 bits of a binary32 pattern (1 sign, 8 exponent, 7 fraction bits). Converting `(float)Math.PI` = `0x40490FDB` to bfloat16 keeps `0x4049` (the discarded half `0x0FDB` is below the halfway point). What value does `0x4049` represent?"
    explanation: |-
      `0x4049` = `0100 0000 0100 1001`: sign 0, exponent `10000000` = 128 → e = 1, fraction `1001001`. Significand = 1.1001001 in binary = 1 + 1/2 + 1/16 + 1/128 = 1.5703125. Value = 1.5703125 × 2 = 3.140625.

      Rounding check: the discarded bits `0x0FDB` are less than `0x8000` (half of the lower 16-bit range), so round-to-nearest keeps `0x4049`.

      "3.1415927" assumes that because bfloat16 keeps binary32's 8-bit exponent it keeps its precision too. It keeps the *range*, but only 8 bits of precision (7 stored + hidden).

      "3.1415" truncates decimal digits; bit truncation does not correspond to cutting decimal digits.

      "3.125" keeps only the first 4 fraction bits (1.1001 × 2), which is what a much narrower format would do.
    options:
      - text: "3.1415927"
      - text: "3.140625"
        correct: true
      - text: "3.1415"
      - text: "3.125"
  - id: "fp8-e4m3-versus-e5m2"
    type: "multiple"
    prompt: "Compare the two OCP 8-bit floating-point formats, E4M3 (4 exponent, 3 fraction bits) and E5M2 (5 exponent, 2 fraction bits). Which statements are correct? Select all that apply."
    explanation: |-
      The invariant is the trade between exponent bits (range) and fraction bits (precision) at a fixed width.

      Correct:

      - E5M2 covers a wider range: its largest finite value is 57344 = 1.75 × 2\^15, versus 448 for E4M3.
      - E4M3 has finer spacing: 3 fraction bits give 8 steps per binade, against 4 steps for E5M2, so its relative precision is twice as fine.
      - In the OCP 8-bit specification, E4M3 gives up infinities and keeps only one NaN pattern per sign (`S.1111.111`), reclaiming the other all-ones-exponent patterns as finite values. That is how its maximum reaches 1.75 × 2\^8 = 448 instead of the IEEE-style 1.875 × 2\^7 = 240.

      Wrong:

      - "Both have the same range because both are 8 bits wide" ignores that range is set by the exponent field, not the total width.
      - "E5M2 is more precise because it has more exponent bits" mixes up the roles: exponent bits buy range, fraction bits buy precision.

      This question is the one in this section that depends on a published specification rather than pure arithmetic; the range-versus-precision reasoning holds for any split of 8 bits.
    options:
      - text: "E5M2 covers a wider range than E4M3."
        correct: true
      - text: "E4M3 has finer spacing between neighboring values than E5M2."
        correct: true
      - text: "In the OCP specification, E4M3 has no encoding for infinity."
        correct: true
      - text: "Both have the same range because both are 8 bits wide."
      - text: "E5M2 is more precise because it has more exponent bits."
---

## Learning objectives

- Decode the all-ones exponent field into infinities and NaNs, and tell quiet from signaling NaNs.
- Count the bit patterns in each special class.
- Apply the same sign/exponent/fraction layout to binary16, bfloat16 and 8-bit formats.
- Reason about the range-versus-precision trade-off of a format from its field widths.

## The invariant

IEEE 754-2019 reserves the two extreme values of the exponent field. The all-zeros field holds ±0 and subnormals (previous section). The all-ones field holds the non-finite values:

| Exponent field | Fraction | Meaning | binary32 examples |
|---|---|---|---|
| all ones | 0 | ±infinity | `0x7F800000`, `0xFF800000` |
| all ones | ≠ 0, top bit 1 | quiet NaN | `0x7FC00000` |
| all ones | ≠ 0, top bit 0 | signaling NaN | `0x7FA00000` |
| all zeros | 0 | ±0 | `0x00000000`, `0x80000000` |

The quiet/signaling convention in the table is the one the 2008 and 2019 revisions recommend (and nearly all current hardware uses); the 1985 standard left it to implementations. A quiet NaN propagates through arithmetic silently; a signaling NaN raises the invalid-operation exception when used and is normally converted to a quiet one. The remaining fraction bits are the NaN's *payload*, which the standard suggests be propagated to give diagnostic information. In binary32 there are 2 × (2\^23 − 1) = 16,777,214 NaN patterns, so code that compares NaNs bit-for-bit is comparing payloads.

Nothing about this layout is specific to 32 or 64 bits. Any binary format is defined by its exponent width w and fraction width t, with bias 2\^(w−1) − 1, the hidden bit for normal numbers and the same reserved fields (unless a format deliberately reclaims them):

| Format | w | t | Precision | Largest finite | Origin |
|---|---|---|---|---|---|
| binary16 | 5 | 10 | 11 bits | 65504 | IEEE 754 |
| bfloat16 | 8 | 7 | 8 bits | ≈ 3.39 × 10\^38 | upper half of binary32 |
| E5M2 | 5 | 2 | 3 bits | 57344 | OCP 8-bit spec, IEEE-like specials |
| E4M3 | 4 | 3 | 4 bits | 448 | OCP 8-bit spec, no infinities |

Two design rules fall out:

1. **Exponent bits buy range; fraction bits buy precision.** bfloat16 keeps binary32's exponent, so anything a `float` can hold without overflow fits, but with only about 2–3 significant decimal digits. binary16 keeps more precision but overflows above 65504.
2. **Truncating binary32 to bfloat16 is a bit operation.** The upper 16 bits are already a valid bfloat16; correct conversion rounds the lower half to nearest even rather than simply dropping it.

## Worked example

Decode binary16 `0x7BFF`: `0111 1011 1111 1111` → sign 0, exponent `11110` = 30, fraction `1111111111`. Value = (1 + 1023/1024) × 2\^(30 − 15) = (2047/1024) × 32768 = 65504. That is the binary16 maximum; `0x7C00` is +infinity.

Convert π from binary32 to bfloat16 by keeping the top half: `0x40490FDB` → `0x4049`:

| Field | Bits | Meaning |
|---|---|---|
| Sign | 0 | positive |
| Exponent | `10000000` | e = 128 − 127 = 1 |
| Fraction | `1001001` | 1.1001001₂ = 1.5703125 |
| Value | | 1.5703125 × 2 = 3.140625 |

```python
import numpy as np
print(float(np.float16(65504.0)), np.float16(65520.0))  # 65504.0 inf
print(np.finfo(np.float16).eps)                         # 0.000977 (2**-10)
```

The second print shows rounding to infinity: 65520 lies exactly halfway between the largest finite value 65504 and the first value past the range (65536), and round-to-nearest-even resolves the tie as an overflow.

## Where this shows up today

As of November 2026, bfloat16 and binary16 are routine storage and compute formats in ML accelerators and GPUs, and the OCP 8-bit formats E4M3 and E5M2 are widely used for training and inference. The OCP Microscaling (MX) specification, at version 1.1 (October 2026), adds block formats in which a group of small elements shares one scale factor. IEEE P3109, a project to standardize arithmetic formats for machine learning aligned with IEEE 754-2019, is still at the draft stage. All of them reuse the sign/exponent/fraction idea from this chapter.

## Pitfalls checklist

- Treating every all-ones exponent as NaN, or every NaN as the single pattern `0x7FC00000`.
- Comparing floats by bits to detect "the" NaN; payloads and signs vary.
- Assuming bfloat16 keeps binary32 precision because it keeps binary32 range.
- Assuming 8-bit formats follow every IEEE convention; OCP E4M3 has no infinities.
- Converting to a narrower format by truncation when the hardware path rounds to nearest even, then wondering why results differ.

## Key takeaways

- All-ones exponent: infinity when the fraction is zero, NaN otherwise; the top fraction bit separates quiet from signaling.
- binary32 has 16,777,214 NaN patterns; NaN identity is not a single bit pattern.
- Every binary format is just (w, t, bias); binary16 maxes out at 65504.
- bfloat16 trades precision for binary32's range; FP8 variants trade the same way at 8 bits.
