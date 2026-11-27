---
title: "Code Points, Scalar Values and Code Units"
questions:
  - id: "code-unit-facts"
    type: "multiple"
    prompt: "Which statements about Unicode encoding forms are correct? Select all that apply."
    explanation: |-
      An encoding form maps each Unicode scalar value to a sequence of fixed-size *code units*.

      Correct:

      - UTF-8 uses 8-bit code units and needs 1 to 4 of them per scalar value (RFC 3629 limits it to 4).
      - UTF-16 uses 16-bit code units and needs 1 or 2 of them; scalar values above U+FFFF take two (a surrogate pair).
      - UTF-32 uses 32-bit code units, exactly one per scalar value.

      Wrong:

      - "Every code point fits in one UTF-16 code unit" was true of the original 16-bit design (UCS-2) but has been false since Unicode 2.0 extended the codespace to U+10FFFF.
      - "A UTF-8 code unit is the same thing as a character" ignores multi-byte sequences: é is 2 code units, € is 3, 😀 is 4.
    options:
      - text: "Every code point fits in one UTF-16 code unit."
      - text: "UTF-8 needs 1 to 4 eight-bit code units per scalar value."
        correct: true
      - text: "UTF-16 needs 1 or 2 sixteen-bit code units per scalar value."
        correct: true
      - text: "UTF-32 uses exactly one 32-bit code unit per scalar value."
        correct: true
      - text: "A UTF-8 code unit is the same thing as a character."
  - id: "size-of-codespace"
    type: "single"
    prompt: "How many code points does the Unicode codespace contain?"
    explanation: |-
      The codespace is U+0000 through U+10FFFF: 0x10FFFF + 1 = 0x110000 = 17 × 65,536 = 1,114,112 code points, arranged as 17 planes of 65,536.

      "1,112,064" is the number of *scalar values*: the codespace minus the 2,048 surrogate code points U+D800–U+DFFF.

      "65,536" is only plane 0, the Basic Multilingual Plane.

      "2,097,152" is 2\^21, the number of values a 21-bit field could hold; Unicode stops at U+10FFFF because that is the limit UTF-16 can address.
    options:
      - text: "1,114,112"
        correct: true
      - text: "1,112,064"
      - text: "65,536"
      - text: "2,097,152"
  - id: "not-a-scalar-value"
    type: "single"
    prompt: "Which of these code points is **NOT** a Unicode scalar value?"
    explanation: |-
      The Unicode Standard defines a scalar value as any code point except the surrogate range U+D800–U+DFFF. Surrogate code points are reserved for UTF-16's internal mechanism and never represent characters on their own, so U+D800 is not a scalar value. There are 1,114,112 − 2,048 = 1,112,064 scalar values.

      U+FFFF is a *noncharacter* (permanently reserved for internal use), but it is still a scalar value and every UTF can encode it.

      U+0000 (NULL) is a valid scalar value, even though C strings treat its encoding as a terminator.

      U+10FFFF is the last code point of plane 16 and is a scalar value (also a noncharacter).
    options:
      - text: "U+FFFF"
      - text: "U+D800"
        correct: true
      - text: "U+0000"
      - text: "U+10FFFF"
  - id: "plane-of-grinning-face"
    type: "single"
    prompt: "In which plane is U+1F600 GRINNING FACE?"
    explanation: |-
      The plane number is the code point divided by 0x10000 (65,536), i.e. the bits above the low 16: 0x1F600 >> 16 = 0x1 = plane 1, the Supplementary Multilingual Plane, where most emoji live. Anything outside plane 0 is a supplementary (sometimes called astral) code point and needs a surrogate pair in UTF-16 and 4 bytes in UTF-8.

      "Plane 0" holds U+0000–U+FFFF; 0x1F600 is larger than 0xFFFF.

      "Plane 15" (U+F0000–U+FFFFF) is a private-use plane.

      "Plane 31" reads the hex digits `1F` as the plane; the plane is only the digits above the last four hex digits, here `1`.
    options:
      - text: "Plane 0"
      - text: "Plane 15"
      - text: "Plane 1"
        correct: true
      - text: "Plane 31"
  - id: "length-python-versus-csharp"
    type: "single"
    prompt: "What do Python's `len(\"😀\")` and C#'s `\"😀\".Length` return?"
    explanation: |-
      They count different units.

      - A Python 3 `str` is a sequence of code points, so `len("😀")` is 1.
      - A C# `string` is a sequence of UTF-16 code units (`char`). U+1F600 is above U+FFFF, so it is stored as the surrogate pair `D83D DE00` and `Length` is 2. Java's `String.length()` and JavaScript's `.length` count the same way.

      "1 and 1" assumes every language counts "characters" the same way.

      "2 and 2" assumes Python also stores UTF-16 internally.

      "4 and 2" confuses Python's `len` on a `str` with the length of its UTF-8 encoding (`len("😀".encode("utf-8"))` is 4).
    options:
      - text: "1 and 1"
      - text: "2 and 2"
      - text: "4 and 2"
      - text: "1 and 2"
        correct: true
  - id: "encode-lone-surrogate"
    type: "single"
    prompt: "What happens when this Python line runs?\n\n```python\n\"\\ud83d\".encode(\"utf-8\")\n```"
    explanation: |-
      Python lets a `str` contain any code point, including a lone surrogate such as U+D83D. But UTF-8 (like every Unicode encoding form) is defined only for scalar values, and U+D800–U+DFFF are excluded. The strict UTF-8 codec therefore raises `UnicodeEncodeError: 'utf-8' codec can't encode character '\ud83d' in position 0: surrogates not allowed`.

      "It returns `b'\xed\xa0\xbd'`" is the CESU-8/WTF-8-style generalized encoding of the surrogate. That byte sequence is ill-formed UTF-8, and Python produces it only if you opt in with `errors="surrogatepass"`.

      "It returns `b'\xf0\x9f\x98\x80'`" is the UTF-8 encoding of U+1F600; it would need both halves of the pair, which a lone high surrogate does not have.

      "It returns `b'?'`" describes `errors="replace"`, which is not the default.
    options:
      - text: "It raises `UnicodeEncodeError`."
        correct: true
      - text: "It returns `b'\\xed\\xa0\\xbd'`."
      - text: "It returns `b'\\xf0\\x9f\\x98\\x80'`."
      - text: "It returns `b'?'`."
---

## Learning objectives

- Define code point, scalar value, code unit and encoding form precisely.
- Compute the size of the codespace, the number of scalar values and the plane of a code point.
- Predict how different runtimes measure and index strings.
- Explain why lone surrogates cannot be encoded in any UTF.

## The invariant

The Unicode Standard separates three layers that everyday speech collapses into "character":

| Term | Definition | Range or size |
|---|---|---|
| Code point | Any integer in the Unicode codespace | U+0000 to U+10FFFF (1,114,112 values) |
| Scalar value | A code point that is not a surrogate | 1,112,064 values (excludes U+D800–U+DFFF) |
| Code unit | The fixed-size unit of an encoding form | 8 bits (UTF-8), 16 (UTF-16), 32 (UTF-32) |

The codespace is divided into 17 planes of 65,536 code points: plane 0, the Basic Multilingual Plane (BMP), holds most modern scripts; plane 1 holds historic scripts, symbols and most emoji; planes 2 and 3 hold CJK ideographs; planes 15 and 16 are private use. The upper bound U+10FFFF exists because it is exactly what UTF-16's surrogate pairs can reach (65,536 + 2\^20 = 1,114,112).

The three Unicode encoding forms (UTF-8, UTF-16, UTF-32) are each a bijection between **scalar values** and code-unit sequences. Surrogate code points are not scalar values, so a "string" containing a lone surrogate cannot be encoded by any of them; such data is ill-formed text, however it got there.

Two more categories are easy to confuse with surrogates. *Noncharacters* (U+FFFE, U+FFFF, the last two code points of every plane, and U+FDD0–U+FDEF) are scalar values reserved for internal use; they are valid in every UTF and must not be rejected as ill-formed. *Private-use* code points are scalar values whose meaning is agreed privately. Only surrogates are structurally unencodable.

Above the code-unit layer sit further layers covered in the next chapter: user-perceived characters (grapheme clusters) can contain several scalar values.

## Worked example

Take the string "a€😀" (U+0061, U+20AC, U+1F600):

| Measure | a | € | 😀 | Total |
|---|---|---|---|---|
| Code points / scalar values | 1 | 1 | 1 | 3 |
| UTF-8 code units (bytes) | 1 | 3 | 4 | 8 |
| UTF-16 code units | 1 | 1 | 2 | 4 |
| UTF-32 code units | 1 | 1 | 1 | 3 |
| Plane | 0 | 0 | 1 | |

```python
s = "a€😀"
print(len(s), len(s.encode("utf-8")), len(s.encode("utf-16-le")) // 2)  # 3 8 4
```

```csharp
var s = "a€😀";
Console.WriteLine(s.Length);                         // 4  (UTF-16 code units)
Console.WriteLine(s.EnumerateRunes().Count());       // 3  (scalar values)
Console.WriteLine(Encoding.UTF8.GetByteCount(s));    // 8
```

```javascript
const s = "a€😀";
console.log(s.length, [...s].length);                // 4 3
```

`System.Text.Rune` in .NET and JavaScript's string iterator both walk scalar values; `Length`/`length` and indexing walk UTF-16 code units.

## Where this shows up today

As of November 2026, the major runtimes still differ in their in-memory unit: JavaScript, Java and .NET strings are sequences of UTF-16 code units; CPython stores code points using 1, 2 or 4 bytes each depending on the widest character in the string (PEP 393); Go and Rust strings are UTF-8 bytes; Swift has stored native strings as UTF-8 since Swift 5. "Length" therefore counts a different unit in each.

## Pitfalls checklist

- Using "character" in a spec or API without saying which unit is meant.
- Assuming a `char` (C#, Java) holds a whole character; it holds one UTF-16 code unit.
- Treating the BMP as "all of Unicode"; emoji and many CJK characters are outside it.
- Accepting lone surrogates from user input or JSON escapes and failing later at encode time.
- Assuming that a language's `len` counts the same thing as a database's character limit.

## Key takeaways

- Code points run from U+0000 to U+10FFFF (1,114,112 values) in 17 planes.
- Scalar values are code points minus the 2,048 surrogates; only they can be encoded.
- Code units are encoding-specific: 8, 16 or 32 bits.
- Every runtime's "length" counts some unit; know which one before you index or truncate.
