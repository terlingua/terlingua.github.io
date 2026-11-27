---
title: "UTF-16 Surrogate Pairs"
questions:
  - id: "encode-grinning-face-utf16"
    type: "single"
    prompt: "What are the UTF-16 code units of U+1F600?"
    explanation: |-
      Code points above U+FFFF are encoded as a surrogate pair (Unicode Standard; RFC 2781 describes the same algorithm):

      1. Subtract 0x10000: 0x1F600 − 0x10000 = 0xF600, a 20-bit value `0000 1111 0110 0000 0000`.
      2. High surrogate = 0xD800 + top 10 bits: `0000111101` = 0x3D → 0xD83D.
      3. Low surrogate = 0xDC00 + low 10 bits: `1000000000` = 0x200 → 0xDE00.

      Result: `D83D DE00`.

      "`D87D DE00`" skips step 1: 0x1F600 >> 10 = 0x7D, so the high half comes out 0x40 too large (0x10000 >> 10 = 0x40).

      "`F0 9F 98 80`" is the UTF-8 encoding (four bytes), not UTF-16.

      "`0001 F600`" splits the UTF-32 value into two 16-bit halves; UTF-16 never does that, because 0x0001 would decode as U+0001 on its own.
    options:
      - text: "`D87D DE00`"
      - text: "`F0 9F 98 80`"
      - text: "`D83D DE00`"
        correct: true
      - text: "`0001 F600`"
  - id: "decode-surrogate-pair"
    type: "single"
    prompt: "Which code point does the UTF-16 sequence `D83C DF89` encode?"
    explanation: |-
      High surrogate offset: 0xD83C − 0xD800 = 0x3C. Low surrogate offset: 0xDF89 − 0xDC00 = 0x389. Combine: (0x3C << 10) | 0x389 = 0xF000 | 0x389 = 0xF389. Add 0x10000: U+1F389 (🎉 PARTY POPPER).

      "U+F389" forgets to add 0x10000 back.

      "U+3CF89" concatenates the offsets as hex digits instead of shifting the high part by 10 bits.

      "U+D83CDF89" treats the pair as one 32-bit number; that is far beyond U+10FFFF.
    options:
      - text: "U+F389"
      - text: "U+3CF89"
      - text: "U+D83CDF89"
      - text: "U+1F389"
        correct: true
  - id: "slice-splits-pair"
    type: "single"
    prompt: "What happens when this JavaScript runs?\n\n```javascript\nconst preview = \"😀abc\".slice(0, 1);\nconsole.log(encodeURIComponent(preview));\n```"
    explanation: |-
      `slice` indexes UTF-16 code units. "😀" occupies indexes 0 and 1 (`D83D DE00`), so `slice(0, 1)` returns a one-unit string containing the lone high surrogate U+D83D. JavaScript strings may hold unpaired surrogates, but `encodeURIComponent` must encode them as UTF-8, and a lone surrogate has no UTF-8 encoding, so it throws `URIError: URI malformed`.

      "It prints `%F0%9F%98%80`" assumes `slice(0, 1)` returns the whole emoji; it returns half of it.

      "It prints `%ED%A0%BD`" is the CESU-8 form of the surrogate, which `encodeURIComponent` refuses to produce.

      "It prints an empty string" assumes invalid input is dropped; the specification requires an exception.
    options:
      - text: "It throws `URIError: URI malformed`."
        correct: true
      - text: "It prints `%F0%9F%98%80`."
      - text: "It prints `%ED%A0%BD`."
      - text: "It prints an empty string."
  - id: "utf8-versus-utf16-size"
    type: "single"
    prompt: "A document contains 1,000 CJK ideographs from the BMP (U+4E00–U+9FFF) and 1,000 emoji from plane 1. How many bytes does it take in UTF-8 and in UTF-16 (no BOM)?"
    explanation: |-
      Per-character costs:

      - BMP ideographs (U+0800–U+FFFF): 3 bytes in UTF-8, one 16-bit unit = 2 bytes in UTF-16.
      - Plane-1 emoji: 4 bytes in UTF-8, a surrogate pair = 4 bytes in UTF-16.

      UTF-8: 1000 × 3 + 1000 × 4 = 7,000 bytes. UTF-16: 1000 × 2 + 1000 × 4 = 6,000 bytes.

      "5,000 and 4,000" assumes 2 bytes for every CJK character in UTF-8 and 2 for emoji in UTF-16 (forgetting surrogate pairs).

      "7,000 and 4,000" forgets that supplementary characters cost two code units in UTF-16.

      "6,000 and 6,000" assumes UTF-8 and UTF-16 cost the same for CJK text; UTF-8 is larger for BMP characters above U+07FF and smaller for ASCII.
    options:
      - text: "5,000 and 4,000"
      - text: "7,000 and 6,000"
        correct: true
      - text: "7,000 and 4,000"
      - text: "6,000 and 6,000"
  - id: "utf16le-with-bom"
    type: "single"
    prompt: "What bytes does a UTF-16 encoder that writes little-endian with a byte order mark produce for the string `A`?"
    explanation: |-
      The BOM is U+FEFF. In little-endian order its two bytes are written least significant first: `FF FE`. Then `A` = U+0041 is written as `41 00`. Result: `FF FE 41 00`. A reader that sees `FF FE` knows to read little-endian; `FE FF` means big-endian. (U+FFFE is a noncharacter, so a byte-swapped BOM cannot be mistaken for text.)

      "`FE FF 00 41`" is the big-endian form.

      "`FF FE 00 41`" mixes a little-endian BOM with big-endian text, which a reader would decode as U+4100.

      "`EF BB BF 41`" is the UTF-8 BOM followed by UTF-8 `A`.
    options:
      - text: "`FE FF 00 41`"
      - text: "`FF FE 00 41`"
      - text: "`FF FE 41 00`"
        correct: true
      - text: "`EF BB BF 41`"
  - id: "mysql-utf8mb3-emoji"
    type: "single"
    prompt: "In MySQL 8.4 with strict SQL mode, inserting `😀` into a column declared `CHARACTER SET utf8` fails with \"Incorrect string value: '\\xF0\\x9F\\x98\\x80'\". What is the root cause?"
    explanation: |-
      In MySQL 8.4, `utf8` is a deprecated alias for `utf8mb3`, an encoding that stores UTF-8 sequences of at most 3 bytes and therefore only BMP characters (U+0000–U+FFFF). U+1F600 is in plane 1, and every supplementary code point needs a 4-byte UTF-8 sequence (`F0 9F 98 80` here), so the value is rejected. The fix is `utf8mb4`, which MySQL's documentation recommends for all new applications.

      The invariant behind the answer does not depend on MySQL's version: anything that caps UTF-8 at 3 bytes cannot store supplementary characters. The vendor-specific part is only that `utf8` meant `utf8mb3`.

      "MySQL stores text as UTF-16 and rejects surrogate pairs" is wrong: the error shows UTF-8 bytes, and `utf8mb3` is a UTF-8 variant.

      "The collation cannot sort emoji" confuses ordering with storage; collations never reject values on insert.

      "The client sent Latin-1 bytes instead of UTF-8" would produce different bytes (mojibake), not a correct 4-byte UTF-8 sequence.
    options:
      - text: "MySQL stores text as UTF-16 and rejects surrogate pairs."
      - text: "The collation cannot sort emoji."
      - text: "The client sent Latin-1 bytes instead of UTF-8."
      - text: "`utf8` here is a 3-byte-maximum encoding, and the emoji needs 4."
        correct: true
  - id: "surrogate-facts"
    type: "multiple"
    prompt: "Which statements about surrogates are correct? Select all that apply."
    explanation: |-
      Correct:

      - High (leading) surrogates are U+D800–U+DBFF and low (trailing) surrogates are U+DC00–U+DFFF: 1,024 each, and 1,024 × 1,024 = 1,048,576 = 2\^20 pairs, exactly the number of supplementary code points.
      - A well-formed pair is always high followed by low; a low before a high, or either one alone, is ill-formed UTF-16.
      - JavaScript, Java and .NET strings can hold lone surrogates, because they are sequences of 16-bit units with no well-formedness check.

      Wrong:

      - "UTF-8 also uses surrogate pairs for emoji" is false: UTF-8 encodes the scalar value directly in 4 bytes; surrogate bytes in UTF-8 are ill-formed.
      - "Every 16-bit code unit at or above `0xD800` is a surrogate" is false: U+E000–U+FFFF are ordinary BMP code points (private use area, CJK compatibility, specials).
    options:
      - text: "High surrogates are U+D800–U+DBFF and low surrogates U+DC00–U+DFFF."
        correct: true
      - text: "A well-formed pair is always a high surrogate followed by a low one."
        correct: true
      - text: "JavaScript, Java and .NET strings can contain lone surrogates."
        correct: true
      - text: "UTF-8 also uses surrogate pairs for emoji."
      - text: "Every 16-bit code unit at or above `0xD800` is a surrogate."
---

## Learning objectives

- Encode a supplementary code point into a UTF-16 surrogate pair and decode a pair back.
- Explain how UTF-16 indexing and slicing create lone surrogates.
- Compare UTF-8 and UTF-16 sizes for different scripts.
- Read and write UTF-16 byte order marks.

## The invariant

UTF-16 represents each BMP scalar value (U+0000–U+D7FF, U+E000–U+FFFF) as one 16-bit code unit equal to the value. Supplementary scalar values (U+10000–U+10FFFF) use two units drawn from the surrogate range, which the Unicode Standard reserves exactly for this purpose:

| Step | Formula |
|---|---|
| Offset | v′ = v − 0x10000 (a 20-bit number) |
| High (leading) surrogate | 0xD800 + (v′ >> 10), range D800–DBFF |
| Low (trailing) surrogate | 0xDC00 + (v′ & 0x3FF), range DC00–DFFF |
| Decode | v = ((hi − 0xD800) << 10) + (lo − 0xDC00) + 0x10000 |

Properties:

- **Self-synchronizing at the unit level.** High and low ranges are disjoint, so from any unit you know whether you are at the start, end or middle of a pair.
- **Fixed upper bound.** 1,024 high × 1,024 low = 2\^20 supplementary code points, which is why the Unicode codespace ends at U+10FFFF.
- **Byte order.** Code units are 16-bit integers, so serialized UTF-16 has an endianness: UTF-16BE, UTF-16LE, or "UTF-16" with a BOM (`FE FF` or `FF FE`). RFC 2781 defines the labels and says that unmarked "UTF-16" should be read as big-endian.

Runtimes whose strings are arrays of UTF-16 units (JavaScript, Java, .NET) expose those units through `length`, indexing and slicing. Any operation that cuts at an arbitrary unit index can split a pair and produce a lone surrogate. The string remains a valid object in memory but is no longer well-formed Unicode, and the failure appears later, wherever the text is encoded to UTF-8, URL-encoded, serialized or validated.

## Worked example

Encode U+1F600 and measure a mixed string:

| Item | Value |
|---|---|
| v − 0x10000 | 0xF600 = `0000111101 1000000000` |
| High | 0xD800 + 0x03D = 0xD83D |
| Low | 0xDC00 + 0x200 = 0xDE00 |

```csharp
string s = "😀abc";
Console.WriteLine(s.Length);                       // 5 (code units)
Console.WriteLine(char.IsHighSurrogate(s[0]));     // True
Console.WriteLine(s.Substring(0, 1).Length);       // 1 (a lone high surrogate)
var safe = s[..(char.IsHighSurrogate(s[0]) ? 2 : 1)];
Console.WriteLine(safe);                           // 😀
```

```python
print("😀".encode("utf-16-be").hex(" "))   # d8 3d de 00
print("A".encode("utf-16").hex(" "))      # ff fe 41 00  (CPython writes a BOM in native little-endian order)
```

The second Python line shows the BOM behavior of the generic `utf-16` codec: it writes a BOM in the machine's native order, which is little-endian on mainstream hardware; use `utf-16-le` or `utf-16-be` to control the output explicitly.

## Where this shows up today

As of November 2026, the MySQL 8.4 Reference Manual still documents `utf8` as a deprecated alias for `utf8mb3`, limited to the BMP, and recommends `utf8mb4` for all new applications. Schemas created years ago with `utf8` keep failing on emoji and supplementary CJK characters until they are converted.

## Pitfalls checklist

- Forgetting to subtract (or add back) 0x10000 when converting.
- Slicing or truncating UTF-16 strings at arbitrary indexes and creating lone surrogates.
- Reversing a UTF-16 string unit by unit, which swaps the halves of every pair.
- Reading unmarked UTF-16 with the wrong byte order.
- Assuming a 3-byte UTF-8 limit (MySQL `utf8mb3`, some older libraries) covers all of Unicode.

## Key takeaways

- Supplementary code points become two units: D800 + high 10 bits, DC00 + low 10 bits of v − 0x10000.
- High and low surrogate ranges are disjoint, and pairs must be high-then-low.
- UTF-16 indexing exposes code units; careless slicing produces lone surrogates that fail at encode time.
- UTF-16 needs a byte order; UTF-8 does not.
