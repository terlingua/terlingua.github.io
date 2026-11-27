---
title: "Invalid and Overlong UTF-8"
questions:
  - id: "never-valid-byte-values"
    type: "multiple"
    prompt: "Which byte values can **never** appear anywhere in well-formed UTF-8? Select all that apply."
    explanation: |-
      Well-formedness (RFC 3629, and Table 3-7 of the Unicode Standard's conformance chapter) rules out several byte values entirely.

      Correct:

      - `0xC0` and `0xC1` could only start a 2-byte sequence for a value below U+0080, which is always an overlong form.
      - `0xF5`–`0xF7` would start 4-byte sequences above U+10FFFF, `0xF8`–`0xFD` were the 5- and 6-byte leads of the obsolete RFC 2279, and `0xFE` and `0xFF` were never used by any version. So `0xF5` and `0xFF` are never valid.

      Wrong (these bytes are valid):

      - `0x80` is a continuation byte (`10000000`); it is valid after a lead byte, for example as the last byte of `F0 9F 98 80` (😀).
      - `0xED` is the lead byte for U+D000–U+D7FF, provided the second byte is `80`–`9F` (which keeps the value below the surrogates). `ED 9F BF` encodes U+D7FF.
    options:
      - text: "`0xC0`"
        correct: true
      - text: "`0x80`"
      - text: "`0xED`"
      - text: "`0xF5`"
        correct: true
      - text: "`0xFF`"
        correct: true
  - id: "overlong-slash"
    type: "single"
    prompt: "A lenient decoder accepts the two bytes `C0 AF`. What does it produce, and why must a conforming decoder reject them?"
    explanation: |-
      `C0` = `110 00000` (2-byte lead, payload `00000`); `AF` = `10 101111` (payload `101111`). Payload `00000101111` = 0x2F = `/`. The correct encoding of `/` is the single byte `2F`, so `C0 AF` is an *overlong* form. RFC 3629 and the Unicode Standard require decoders to treat overlong forms as ill-formed, because a filter that scans bytes for `2F` (to block `../` path traversal, for example) would miss the disguised slash that a lenient decoder later turns back into `/`. This exact pattern was exploited against web servers around 2000.

      "U+00AF ¯" reads the second byte as Latin-1; in UTF-8 it is a continuation byte, not a character.

      "U+00C0 À" reads the first byte as Latin-1.

      "U+FFFD, because `C0` is a continuation byte" misclassifies `C0` (`110xxxxx` is a lead pattern); a conforming decoder does emit U+FFFD here, but because `C0` is never valid, not because of its pattern.
    options:
      - text: "U+00AF `¯`, a legal two-byte form"
      - text: "U+00C0 `À`, because `C0` is its Latin-1 value"
      - text: "U+002F `/`, an overlong form that evades byte-level filters"
        correct: true
      - text: "U+FFFD, because `C0` is a continuation byte"
  - id: "encoded-surrogate"
    type: "single"
    prompt: "Is the byte sequence `ED A0 80` well-formed UTF-8?"
    explanation: |-
      Apply the 3-byte template: `ED` = `1110 1101` → `1101`; `A0` = `10 100000` → `100000`; `80` = `10 000000` → `000000`. Payload `1101 100000 000000` = 0xD800, a surrogate code point. UTF-8 encodes only scalar values, so this sequence is ill-formed. (It is what CESU-8 and WTF-8 produce for surrogates; those are not UTF-8.)

      "Yes, it is the UTF-8 encoding of U+D800" ignores that surrogates have no UTF-8 encoding.

      "No, because `ED` is never a valid lead byte" is false: `ED 9F BF` (U+D7FF) is valid. The rule is that after `ED` the second byte must be in `80`–`9F`.

      "Yes, but only when the file starts with a BOM" invents a dependency; a BOM changes nothing about well-formedness.
    options:
      - text: "Yes, it is the UTF-8 encoding of U+D800."
      - text: "No, because `ED` is never a valid lead byte."
      - text: "Yes, but only when the file starts with a BOM."
      - text: "No, because it encodes a surrogate, which is not a scalar value."
        correct: true
  - id: "five-byte-form"
    type: "single"
    prompt: "A decoder receives `F8 88 80 80 80`. Under the 1998 RFC 2279 rules this was the 5-byte encoding of U+200000. How does an RFC 3629 decoder treat it?"
    explanation: |-
      RFC 2279 allowed sequences of up to 6 bytes for values up to 2\^31 − 1. RFC 3629 (2003) restricted UTF-8 to the Unicode range U+0000–U+10FFFF, which never needs more than 4 bytes, and declared the longer forms invalid. Lead byte `F8` (`11111000`) can therefore not start any sequence: the input is ill-formed and must be rejected or replaced (Python reports "invalid start byte").

      Trace of the old value: payload bits `000` + `001000` + `000000` + `000000` + `000000` = 0x200000, which is above U+10FFFF anyway.

      "It decodes to U+200000" applies the obsolete RFC.

      "It clamps the value to U+10FFFF" invents a repair; no standard decoder clamps.

      "It decodes the bytes as five Latin-1 characters" is a fallback some legacy tools use, not UTF-8 decoding.
    options:
      - text: "It rejects the sequence as ill-formed."
        correct: true
      - text: "It decodes to U+200000."
      - text: "It clamps the value to U+10FFFF."
      - text: "It decodes the bytes as five Latin-1 characters."
  - id: "bom-in-decoded-text"
    type: "single"
    prompt: "What does this Python line print?\n\n```python\ndata = b\"\\xef\\xbb\\xbfid\"\nprint(len(data.decode(\"utf-8\")), len(data.decode(\"utf-8-sig\")))\n```"
    explanation: |-
      `EF BB BF` is U+FEFF ZERO WIDTH NO-BREAK SPACE encoded in UTF-8; at the start of a file it acts as a byte order mark (BOM). UTF-8 has no byte order, so the mark only signals "this is UTF-8".

      - The `utf-8` codec decodes the bytes literally: U+FEFF, `i`, `d` → length 3. A CSV header field would be U+FEFF followed by `id`, not `"id"`, so a lookup for `"id"` fails.
      - The `utf-8-sig` codec strips one leading BOM if present → `"id"`, length 2.

      Output: `3 2`.

      "2 2" assumes the plain codec drops the BOM (JavaScript's `TextDecoder` does by default; Python's `utf-8` codec does not).

      "5 2" counts bytes instead of decoded code points.

      "3 3" assumes `utf-8-sig` keeps the mark.
    options:
      - text: "2 2"
      - text: "3 2"
        correct: true
      - text: "5 2"
      - text: "3 3"
  - id: "truncated-sequence-replacement"
    type: "single"
    prompt: "A message body was cut after the first two bytes of `€` (`E2 82 AC`), leaving the bytes `61 E2 82`. How many U+FFFD replacement characters does a decoder that follows the Unicode Standard's recommended \"maximal subpart\" practice produce?"
    explanation: |-
      `61` decodes to `a`. `E2 82` is a valid *prefix* of a 3-byte sequence (lead `E2`, then a continuation byte in the allowed range) that ends early. The Unicode Standard's recommended practice (U+FFFD Substitution of Maximal Subparts, in the conformance chapter) replaces each maximal subpart of an ill-formed sequence with a single U+FFFD, so the result is `a` + one U+FFFD. Python, browsers' `TextDecoder` and .NET's `Encoding.UTF8` all produce `a�` here.

      "2" replaces each bad byte separately. That is what happens for bytes that cannot begin any valid prefix together, such as `C0 AF`, which yields two U+FFFD.

      "0" assumes the decoder silently drops the fragment; dropping hides corruption and is discouraged for security reasons.

      "3" counts all three bytes, including the valid `a`.
    options:
      - text: "2"
      - text: "0"
      - text: "1"
        correct: true
      - text: "3"
---

## Learning objectives

- Identify ill-formed UTF-8: overlong forms, encoded surrogates, out-of-range values, bad lead bytes and truncation.
- Explain the security reason overlong forms must be rejected.
- Predict how decoders replace ill-formed input with U+FFFD.
- Handle the UTF-8 byte order mark at boundaries.

## The invariant

A UTF-8 byte sequence is well-formed only if it matches exactly one of the permitted patterns. The Unicode Standard's conformance chapter tabulates them (Table 3-7, "Well-Formed UTF-8 Byte Sequences"); RFC 3629 states the same rules:

| Code points | Byte 1 | Byte 2 | Byte 3 | Byte 4 |
|---|---|---|---|---|
| U+0000–U+007F | 00–7F | | | |
| U+0080–U+07FF | C2–DF | 80–BF | | |
| U+0800–U+0FFF | E0 | A0–BF | 80–BF | |
| U+1000–U+CFFF | E1–EC | 80–BF | 80–BF | |
| U+D000–U+D7FF | ED | 80–9F | 80–BF | |
| U+E000–U+FFFF | EE–EF | 80–BF | 80–BF | |
| U+10000–U+3FFFF | F0 | 90–BF | 80–BF | 80–BF |
| U+40000–U+FFFFF | F1–F3 | 80–BF | 80–BF | 80–BF |
| U+100000–U+10FFFF | F4 | 80–8F | 80–BF | 80–BF |

Everything else is ill-formed. The table encodes five rules at once:

1. **No overlong forms.** `C0`, `C1` never appear; after `E0` the second byte starts at `A0`; after `F0` at `90`.
2. **No surrogates.** After `ED` the second byte stops at `9F`, excluding U+D800–U+DFFF.
3. **Nothing above U+10FFFF.** After `F4` the second byte stops at `8F`; `F5`–`FF` never appear.
4. **No 5- or 6-byte forms** (allowed by the obsolete RFC 2279).
5. **No truncation or stray continuation bytes.**

A conforming decoder must not interpret ill-formed input as characters. It either fails or replaces it with U+FFFD REPLACEMENT CHARACTER. Overlong forms are the classic security reason: if validation (say, rejecting `../`) runs on bytes and a lenient decoder runs later, `C0 AF` slips past as "not a slash" and becomes `/` after decoding.

**Byte order mark.** U+FEFF at the start of a stream is a BOM. In UTF-16 and UTF-32 it reveals byte order; in UTF-8 (`EF BB BF`) it only signals the encoding. The Unicode Standard permits but does not recommend it for UTF-8, RFC 8259 forbids JSON generators from adding one, and Unix tools treat it as content. Decoders that do not strip it hand your code an invisible U+FEFF at the start of the first field.

## Worked example

Classify each input (Python's strict decoder in comments):

| Bytes | Problem | Result with `errors="replace"` |
|---|---|---|
| `C0 AF` | overlong `/` | two U+FFFD |
| `ED A0 80` | encoded surrogate U+D800 | three U+FFFD |
| `F4 90 80 80` | above U+10FFFF | four U+FFFD |
| `61 E2 82` | truncated € | `a` + one U+FFFD |
| `EF BB BF 69 64` | BOM, well-formed | U+FEFF + `id` |

```python
for raw in (b"\xc0\xaf", b"\xed\xa0\x80", b"a\xe2\x82"):
    try:
        raw.decode("utf-8")
    except UnicodeDecodeError as e:
        print(raw.hex(" "), "->", e.reason)
# c0 af -> invalid start byte
# ed a0 80 -> invalid continuation byte
# 61 e2 82 -> unexpected end of data
```

## Where this shows up today

As of November 2026, browsers and JavaScript's `TextDecoder` strip a leading UTF-8 BOM by default, while Python's `utf-8` codec keeps it (use `utf-8-sig`), and spreadsheet applications commonly write a BOM into exported CSV files. Data pipelines that mix these tools still produce header columns that silently fail to match.

## Pitfalls checklist

- Validating or filtering bytes before decoding with a lenient decoder.
- Accepting overlong forms, surrogates or values above U+10FFFF "to be robust".
- Truncating a byte buffer mid-sequence and then decoding it as if it were whole.
- Silently dropping invalid bytes instead of failing or substituting U+FFFD.
- Comparing the first CSV header with a literal without stripping a BOM.

## Key takeaways

- Well-formed UTF-8 is a small, exact table; everything else is ill-formed.
- Overlong forms, encoded surrogates, bytes `C0`, `C1`, `F5`–`FF` and truncated sequences are all invalid.
- Decoders must reject or replace with U+FFFD; the recommended practice replaces each maximal subpart once.
- A UTF-8 BOM is legal but unhelpful; strip it at input boundaries.
