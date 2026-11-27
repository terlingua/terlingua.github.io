---
title: "The Four Lengths of a String"
questions:
  - id: "family-emoji-lengths"
    type: "single"
    prompt: "The family emoji (man, woman, girl) is the sequence U+1F468, U+200D (ZERO WIDTH JOINER), U+1F469, U+200D, U+1F467. What are its UTF-8 byte length, UTF-16 code-unit length, scalar-value count and extended-grapheme-cluster count?"
    explanation: |-
      Add up each unit:

      - UTF-8 bytes: three emoji × 4 bytes + two ZWJ × 3 bytes (U+200D is in U+0800–U+FFFF) = 12 + 6 = 18.
      - UTF-16 units: three emoji × 2 (surrogate pairs) + two ZWJ × 1 = 8.
      - Scalar values: 5.
      - Extended grapheme clusters (UAX #29): 1. Rule GB11 forbids a break between an Extended_Pictographic character followed by ZWJ and the next Extended_Pictographic character, so the whole sequence is one cluster.

      "18, 8, 5, 5" stops at scalar values and treats each code point as a user-perceived character.

      "16, 8, 5, 1" counts the ZWJ as 2 bytes in UTF-8; U+200D needs 3.

      "18, 5, 5, 1" equates UTF-16 units with code points, forgetting the surrogate pairs.
    options:
      - text: "18, 8, 5, 5"
      - text: "16, 8, 5, 1"
      - text: "18, 5, 5, 1"
      - text: "18, 8, 5, 1"
        correct: true
  - id: "skin-tone-length-javascript"
    type: "single"
    prompt: "What does JavaScript return for `\"👍🏽\".length`? The string is U+1F44D THUMBS UP SIGN followed by U+1F3FD EMOJI MODIFIER FITZPATRICK TYPE-4."
    explanation: |-
      `length` counts UTF-16 code units. Both code points are in plane 1, so each needs a surrogate pair: 2 + 2 = 4.

      "1" is the grapheme-cluster count: UAX #29 keeps an emoji modifier attached to its base (the modifier has Grapheme_Cluster_Break=Extend), so users see one thumbs-up.

      "2" is the code-point count, what `[..."👍🏽"].length` returns.

      "8" is the UTF-8 byte count (4 + 4).
    options:
      - text: "4"
        correct: true
      - text: "1"
      - text: "2"
      - text: "8"
  - id: "three-regional-indicators"
    type: "single"
    prompt: "A string contains three regional indicator symbols: U+1F1FA (U), U+1F1F8 (S), U+1F1E6 (A). How many extended grapheme clusters does UAX #29 find?"
    explanation: |-
      Flags are pairs of regional indicators. UAX #29 rules GB12 and GB13 join regional indicators in pairs counted from the start of the run, never three together. So U+1F1FA U+1F1F8 form one cluster (🇺🇸) and the leftover U+1F1E6 forms a second cluster on its own: 2.

      "1" assumes any run of regional indicators is one unit.

      "3" ignores the pairing rule and treats each indicator as its own character.

      "6" is the UTF-16 code-unit count (three surrogate pairs).

      Inserting or deleting one indicator in a run re-pairs everything after it, which is why flag strings can turn into different flags when edited.
    options:
      - text: "1"
      - text: "2"
        correct: true
      - text: "3"
      - text: "6"
  - id: "grapheme-counting-apis"
    type: "multiple"
    prompt: "Which of these return the number of extended grapheme clusters (user-perceived characters) in a string? Select all that apply."
    explanation: |-
      Correct:

      - JavaScript's `Intl.Segmenter` with `granularity: "grapheme"` yields one segment per extended grapheme cluster; counting the segments gives the user-perceived length.
      - .NET's `StringInfo.LengthInTextElements` counts text elements, which since .NET 5 are UAX #29 extended grapheme clusters.

      Wrong:

      - Python's `len()` on a `str` counts code points; Python's standard library has no grapheme segmentation (third-party packages provide it).
      - Java's `String.length()` counts UTF-16 code units.
      - C#'s `string.Length` counts UTF-16 code units too.
    options:
      - text: "Python `len(s)`"
      - text: "Java `s.length()`"
      - text: "C# `s.Length`"
      - text: "JavaScript: the number of segments from `new Intl.Segmenter(undefined, { granularity: \"grapheme\" }).segment(s)`"
        correct: true
      - text: ".NET `new StringInfo(s).LengthInTextElements`"
        correct: true
  - id: "truncate-splits-cluster"
    type: "single"
    prompt: "A display name is stored in decomposed form: `e` followed by U+0301 COMBINING ACUTE ACCENT. What does this print?\n\n```python\ns = \"cafe\" + chr(0x301)\nprint(s[:4], len(s))\n```"
    explanation: |-
      The string has 5 code points (c, a, f, e, U+0301) but 4 grapheme clusters ("c", "a", "f", "é"). Slicing by code point to 4 keeps c, a, f, e and drops the combining accent, so the user sees `cafe`. `len(s)` is 5.

      Output: `cafe 5`.

      "café 4" assumes Python counts and slices user-perceived characters.

      "café 5" assumes the slice keeps the accent because it "belongs" to the e; code-point slicing knows nothing about clusters.

      "cafe 4" assumes `len` counts graphemes.

      "caf 5" assumes the accent counts as part of the fourth position and pushes the `e` out; the accent is the fifth code point.

      The fix is to truncate on grapheme boundaries (or normalize to NFC first when a precomposed character exists, which works here but not for emoji or most combining sequences).
    options:
      - text: "café 4"
      - text: "café 5"
      - text: "cafe 4"
      - text: "caf 5"
      - text: "cafe 5"
        correct: true
  - id: "cluster-size-limit"
    type: "single"
    prompt: "Is there a maximum number of code points in a single extended grapheme cluster?"
    explanation: |-
      No. UAX #29 attaches any number of Extend characters (combining marks, emoji modifiers, variation selectors) and ZWJ-joined pictographs to a cluster, and its rules impose no length limit. Text such as "Zalgo" strings stacks dozens or hundreds of combining marks on one base letter. UAX #15 defines an optional *Stream-Safe Text Format* that limits runs of non-starters to 30 to bound normalization buffers, but that is a processing convention, not a limit on grapheme clusters.

      "Yes, 2 code points" fits a simple accented letter but not emoji sequences such as the five-code-point family.

      "Yes, 4 bytes of UTF-8" confuses the maximum length of one *code point's* encoding with a cluster.

      "Yes, the length of the longest emoji ZWJ sequence" assumes only emoji form multi-code-point clusters; combining sequences have no fixed maximum.
    options:
      - text: "No; any number of extending characters can attach."
        correct: true
      - text: "Yes, 2 code points."
      - text: "Yes, 4 bytes of UTF-8."
      - text: "Yes, the length of the longest emoji ZWJ sequence."
---

## Learning objectives

- Compute a string's UTF-8 byte length, UTF-16 length, scalar-value count and grapheme-cluster count.
- Recognize combining sequences, emoji modifiers, ZWJ sequences and regional-indicator flags.
- Choose the right length for storage limits, cursor movement and user-facing truncation.
- Use the grapheme APIs available in mainstream runtimes.

## The invariant

A string has at least four lengths, and each answers a different question:

| Length | Unit | Question it answers |
|---|---|---|
| UTF-8 bytes | 8-bit code units | How much storage or bandwidth? |
| UTF-16 code units | 16-bit units | What do JavaScript, Java and .NET index? |
| Scalar values | code points | What does Python index? |
| Extended grapheme clusters | user-perceived characters | What does a person count, select or delete? |

Extended grapheme clusters are defined by **UAX #29, Unicode Text Segmentation**, as a set of rules over character properties. The cases that matter most in practice:

- **Combining marks** (Grapheme_Cluster_Break=Extend) attach to the preceding base: `e` + U+0301 is one cluster.
- **Emoji modifiers** (skin tones U+1F3FB–U+1F3FF) attach to the preceding emoji.
- **ZWJ sequences**: Extended_Pictographic + U+200D + Extended_Pictographic does not break (rule GB11), so families, professions and multi-part emoji are one cluster.
- **Regional indicators** pair up from the start of a run (rules GB12, GB13): two make a flag, a third starts a new cluster.
- **Hangul syllables** decomposed into jamo, and CR LF, are single clusters.

The rules depend only on Unicode properties, not on fonts. A ZWJ sequence that a font cannot draw as one glyph still forms one cluster; it is merely displayed as several pictures. Clusters have no maximum length.

Display width is yet another measure. Terminal columns are estimated with East Asian Width properties and emoji presentation, and terminals and libraries disagree on emoji widths, so "width" is not reliably derivable from any of the four lengths.

## Worked example

| String | Code points | UTF-8 bytes | UTF-16 units | Scalars | Graphemes |
|---|---|---|---|---|---|
| é (decomposed) | 0065 0301 | 3 | 2 | 2 | 1 |
| 👍🏽 | 1F44D 1F3FD | 8 | 4 | 2 | 1 |
| 🇺🇸 | 1F1FA 1F1F8 | 8 | 4 | 2 | 1 |
| family (ZWJ sequence) | 1F468 200D 1F469 200D 1F467 | 18 | 8 | 5 | 1 |

```javascript
const seg = new Intl.Segmenter("en", { granularity: "grapheme" });
const family = String.fromCodePoint(0x1F468, 0x200D, 0x1F469, 0x200D, 0x1F467);
console.log(new TextEncoder().encode(family).length,  // 18
            family.length,                            // 8
            [...family].length,                       // 5
            [...seg.segment(family)].length);         // 1
```

```csharp
using System.Globalization;
string family = char.ConvertFromUtf32(0x1F468) + "\u200D" + char.ConvertFromUtf32(0x1F469)
              + "\u200D" + char.ConvertFromUtf32(0x1F467);
Console.WriteLine(family.Length);                                  // 8
Console.WriteLine(family.EnumerateRunes().Count());                // 5
Console.WriteLine(new StringInfo(family).LengthInTextElements);    // 1
```

Truncating for display should cut on cluster boundaries: take whole segments from `Intl.Segmenter` or `StringInfo.GetTextElementEnumerator` until the budget is used up.

## Where this shows up today

As of November 2026, the current version of the Unicode Standard is 18.0, released in September 2026, and each annual release adds emoji and sequences that older segmentation data and fonts do not know about. Fixed-width assumptions ("an emoji is 2 UTF-16 units", "a character is at most 4 bytes") keep failing as multi-code-point sequences become common in names, messages and form fields.

## Pitfalls checklist

- Using UTF-16 length or code-point count for user-facing limits such as "max 20 characters".
- Truncating or reversing strings by code unit or code point, splitting clusters and pairs.
- Moving a cursor or deleting with backspace by code point instead of by cluster.
- Assuming the longest character is one emoji code point; clusters are unbounded.
- Deriving terminal column width from any of the four lengths.

## Key takeaways

- Bytes, UTF-16 units, scalar values and grapheme clusters are four different lengths.
- UAX #29 defines grapheme clusters from properties: combining marks, modifiers, ZWJ sequences and flag pairs.
- Use grapheme segmentation APIs for anything a human sees; use byte length for storage limits.
- Segmentation data changes with Unicode versions; keep runtime ICU or Unicode data current.
