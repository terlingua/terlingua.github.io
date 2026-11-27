---
title: "UTF-8 Byte Patterns"
questions:
  - id: "bytes-for-u0800"
    type: "single"
    prompt: "How many bytes does UTF-8 use to encode U+0800?"
    explanation: |-
      UTF-8 sequence length depends only on the scalar value's range (RFC 3629):

      - U+0000–U+007F: 1 byte (7 payload bits)
      - U+0080–U+07FF: 2 bytes (11 payload bits)
      - U+0800–U+FFFF: 3 bytes (16 payload bits)
      - U+10000–U+10FFFF: 4 bytes (21 payload bits)

      U+0800 = 2048 needs 12 significant bits, more than the 11 that a 2-byte sequence carries, so it takes 3 bytes: `E0 A0 80`.

      "2" applies to U+0080–U+07FF; U+07FF (`DF BF`) is the last 2-byte code point.

      "4" is for supplementary-plane code points only.

      "1" would mean ASCII; only U+0000–U+007F encode as a single byte.
    options:
      - text: "3"
        correct: true
      - text: "2"
      - text: "4"
      - text: "1"
  - id: "encode-euro-sign"
    type: "single"
    prompt: "What are the UTF-8 bytes of U+20AC EURO SIGN?"
    explanation: |-
      U+20AC is in U+0800–U+FFFF, so it uses the 3-byte template `1110xxxx 10xxxxxx 10xxxxxx`.

      Bits of 0x20AC (16 bits): `0010 0000 1010 1100`. Split 4 / 6 / 6: `0010`, `000010`, `101100`.

      - Lead: `1110` + `0010` = `11100010` = `E2`
      - Continuation: `10` + `000010` = `10000010` = `82`
      - Continuation: `10` + `101100` = `10101100` = `AC`

      Result: `E2 82 AC`.

      "20 AC" is the UTF-16 big-endian code unit, not UTF-8.

      "C2 82 AC" puts the 2-byte lead prefix `110` on a 3-byte sequence; a decoder would read `C2 82` as U+0082 and then reject a stray continuation byte.

      "E2 82 AC 00" adds padding; UTF-8 is variable-length with no fixed width.
    options:
      - text: "20 AC"
      - text: "E2 82 AC"
        correct: true
      - text: "C2 82 AC"
      - text: "E2 82 AC 00"
  - id: "decode-four-byte-sequence"
    type: "single"
    prompt: "Which code point do the UTF-8 bytes `F0 9F 92 A9` encode?"
    explanation: |-
      `F0` = `11110000` is a 4-byte lead (prefix `11110`), contributing 3 payload bits `000`. Each continuation byte (`10xxxxxx`) contributes 6:

      - `9F` = `10 011111` → `011111`
      - `92` = `10 010010` → `010010`
      - `A9` = `10 101001` → `101001`

      Concatenate: `000 011111 010010 101001` = `0 0001 1111 0100 1010 1001` = 0x1F4A9, i.e. U+1F4A9.

      "U+F09F92A9" reads the bytes as one number; prefixes must be stripped, and no code point exceeds U+10FFFF.

      "U+9F92A9" drops the lead byte and still keeps the prefixes.

      "U+D83D U+DCA9" is the UTF-16 surrogate pair for the same character, not a code point; surrogates never appear in UTF-8.
    options:
      - text: "U+F09F92A9"
      - text: "U+9F92A9"
      - text: "U+1F4A9"
        correct: true
      - text: "U+D83D U+DCA9"
  - id: "utf8-byte-count-mixed"
    type: "single"
    prompt: "How many bytes is the string `Grüße, 東京 👋` in UTF-8 (no BOM)? It contains 11 code points: `G r ü ß e , space 東 京 space 👋`."
    explanation: |-
      Count by range:

      - ASCII (1 byte each): G, r, e, comma, space, space → 6 bytes
      - U+0080–U+07FF (2 bytes each): ü (U+00FC), ß (U+00DF) → 4 bytes
      - U+0800–U+FFFF (3 bytes each): 東 (U+6771), 京 (U+4EAC) → 6 bytes
      - Supplementary (4 bytes): 👋 (U+1F44B) → 4 bytes

      Total: 6 + 4 + 6 + 4 = 20 bytes.

      "11" is the number of code points.

      "12" is the number of UTF-16 code units (11 code points, one of which needs a surrogate pair).

      "24" is the UTF-16 byte count (12 units × 2).
    options:
      - text: "11"
      - text: "12"
      - text: "24"
      - text: "20"
        correct: true
  - id: "count-chars-bug"
    type: "single"
    prompt: "This method is meant to count code points in valid UTF-8. What does `CountChars(Encoding.UTF8.GetBytes(\"héllo\"))` return?\n\n```csharp\nstatic int CountChars(byte[] utf8)\n{\n    int n = 0;\n    foreach (byte b in utf8)\n        if ((b & 0x80) == 0) n++;\n    return n;\n}\n```"
    explanation: |-
      The condition `(b & 0x80) == 0` is true only for ASCII bytes (`0xxxxxxx`). It skips both bytes of é (`C3 A9`), so the method counts h, l, l, o and returns 4.

      The correct test counts every byte that is *not* a continuation byte: `(b & 0xC0) != 0x80`. Lead bytes (`0xxxxxxx`, `110xxxxx`, `1110xxxx`, `11110xxx`) mark exactly one code point each, giving 5 for "héllo".

      "5" is the intended answer, which the bug does not produce.

      "6" is the byte count (h, `C3`, `A9`, l, l, o).

      "3" would come from also skipping one of the l's; nothing in the code does that.
    options:
      - text: "4"
        correct: true
      - text: "5"
      - text: "6"
      - text: "3"
  - id: "self-synchronizing-properties"
    type: "multiple"
    prompt: "Which properties of well-formed UTF-8 are correct? Select all that apply."
    explanation: |-
      Correct:

      - Continuation bytes always match `10xxxxxx` and lead bytes never do, so from any byte offset you can find the start of the current code point by backing up at most 3 bytes. This is self-synchronization.
      - Bytes 0x00–0x7F appear only as ASCII characters, never inside a multi-byte sequence, so byte-wise searches for `/`, `\n` or `,` cannot match in the middle of a character.
      - Sorting UTF-8 strings byte by byte gives the same order as sorting by code point, because lead bytes grow with the range and payload bits are stored most significant first.

      Wrong:

      - "You must decode from the start of the buffer to find a character boundary" is true of some legacy multi-byte encodings (such as Shift_JIS), not of UTF-8.
      - "A lead byte does not tell you how long its sequence is" is false: the number of leading 1 bits (0, 2, 3 or 4) states the length.
    options:
      - text: "You must decode from the start of the buffer to find a character boundary."
      - text: "A lead byte does not tell you how long its sequence is."
      - text: "From any offset, the current code point starts at most 3 bytes back."
        correct: true
      - text: "ASCII byte values never occur inside a multi-byte sequence."
        correct: true
      - text: "Byte-wise ordering of UTF-8 strings equals code point ordering."
        correct: true
---

## Learning objectives

- Encode a scalar value as UTF-8 by hand using the four templates.
- Decode a UTF-8 byte sequence back to its code point.
- Compute the UTF-8 byte length of a string from its code points.
- Use self-synchronization and ASCII transparency correctly in byte-level code.

## The invariant

UTF-8 is specified by RFC 3629 (and identically in the Unicode Standard). Each scalar value is written with one of four bit templates; the payload bits fill the `x` positions most significant first:

| Scalar value range | Bytes | Byte 1 | Byte 2 | Byte 3 | Byte 4 | Payload bits |
|---|---|---|---|---|---|---|
| U+0000–U+007F | 1 | `0xxxxxxx` | | | | 7 |
| U+0080–U+07FF | 2 | `110xxxxx` | `10xxxxxx` | | | 11 |
| U+0800–U+FFFF | 3 | `1110xxxx` | `10xxxxxx` | `10xxxxxx` | | 16 |
| U+10000–U+10FFFF | 4 | `11110xxx` | `10xxxxxx` | `10xxxxxx` | `10xxxxxx` | 21 |

Design properties that follow from the templates:

- **ASCII compatibility.** ASCII text is byte-for-byte valid UTF-8, and every byte below 0x80 *is* an ASCII character. Parsers for ASCII delimiters work unchanged.
- **Self-synchronization.** Lead bytes and continuation bytes are distinguishable by their top bits, so a reader dropped at an arbitrary offset finds the next boundary within 3 bytes, and corruption affects at most one character.
- **Length from the lead byte.** The count of leading 1 bits gives the sequence length.
- **Order preservation.** Comparing UTF-8 byte strings lexicographically gives code point order.
- **Exactly one encoding per scalar value.** The shortest template must be used; longer ("overlong") forms are invalid. The next section covers what decoders must reject.

## Worked example

Encode U+00E9 (é), U+20AC (€) and U+1F600 (😀):

| Character | Bits used | Template split | Bytes |
|---|---|---|---|
| é U+00E9 | `000 1110 1001` (11 bits) | `00011` / `101001` | `C3 A9` |
| € U+20AC | `0010 0000 1010 1100` | `0010` / `000010` / `101100` | `E2 82 AC` |
| 😀 U+1F600 | `0 0001 1111 0110 0000 0000` (21 bits) | `000` / `011111` / `011000` / `000000` | `F0 9F 98 80` |

```python
for ch in "é€😀":
    print(f"U+{ord(ch):04X}", ch.encode("utf-8").hex(" ").upper())
# U+00E9 C3 A9
# U+20AC E2 82 AC
# U+1F600 F0 9F 98 80
```

Finding the start of a code point from a byte offset uses only the continuation-byte test:

```csharp
static int StartOfCodePoint(ReadOnlySpan<byte> utf8, int i)
{
    while (i > 0 && (utf8[i] & 0xC0) == 0x80) i--;
    return i;
}
```

## Where this shows up today

As of November 2026, UTF-8 is the default or required encoding for most interchange formats: JSON exchanged between systems must be UTF-8 (RFC 8259), the WHATWG Encoding and HTML standards require UTF-8 for new web documents, and the large majority of web pages declare it. Byte-length limits in protocols, databases and message queues are therefore usually UTF-8 byte limits, not character limits.

## Pitfalls checklist

- Counting characters by counting bytes, or by counting only ASCII bytes.
- Assuming every non-ASCII character is 2 bytes; CJK is 3 and emoji are 4.
- Treating UTF-8 bytes as Latin-1 (or Windows-1252) and producing mojibake such as "Ã©" for "é".
- Splitting a byte buffer at an arbitrary offset without backing up to a lead byte.
- Reading code points out of the raw byte values without stripping the template prefixes.

## Key takeaways

- Four templates: 1, 2, 3 or 4 bytes for 7, 11, 16 or 21 payload bits.
- Lead bytes state the length; continuation bytes are always `10xxxxxx`.
- ASCII is preserved, boundaries resynchronize within 3 bytes, and byte order equals code point order.
- UTF-8 byte length = Σ (1, 2, 3 or 4) by each code point's range.
