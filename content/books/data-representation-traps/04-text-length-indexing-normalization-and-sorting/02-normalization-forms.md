---
title: "Normalization Forms"
questions:
  - id: "nfc-of-decomposed-e"
    type: "single"
    prompt: "What is the NFC form of `e` followed by U+0301 COMBINING ACUTE ACCENT, and how many UTF-8 bytes does it take?"
    explanation: |-
      NFC first decomposes canonically, then recomposes canonical pairs that have a primary composite. `e` + U+0301 has the primary composite U+00E9 LATIN SMALL LETTER E WITH ACUTE, so NFC yields the single code point U+00E9. In UTF-8 that is `C3 A9`, 2 bytes, down from 3 (`65 CC 81`).

      "Unchanged, 3 bytes" describes NFD, which leaves the decomposed sequence as is.

      "`e` alone, 1 byte" assumes normalization strips accents; no normalization form removes combining marks (that is a separate, lossy "accent folding" step).

      "U+00E9, 3 bytes" gets the composition right but the byte count wrong: U+00E9 is in U+0080–U+07FF, which UTF-8 encodes in 2 bytes.
    options:
      - text: "U+00E9, 2 bytes"
        correct: true
      - text: "Unchanged, 3 bytes"
      - text: "`e` alone, 1 byte"
      - text: "U+00E9, 3 bytes"
  - id: "canonical-equivalence-pairs"
    type: "multiple"
    prompt: "Which pairs are **canonically** equivalent, so that NFC (and NFD) maps both sides to the same sequence? Select all that apply."
    explanation: |-
      Canonical equivalence means the two sequences represent the same abstract character and must look and behave the same; NFC and NFD unify them. Compatibility equivalence is weaker (same "meaning" but a formatting difference) and only NFKC and NFKD unify it.

      Correct (canonical):

      - U+00E9 and `e` + U+0301: precomposed versus decomposed é.
      - U+212B ANGSTROM SIGN and U+00C5 LATIN CAPITAL LETTER A WITH RING ABOVE: U+212B has a *singleton* canonical decomposition to U+00C5, so NFC replaces it.
      - U+2126 OHM SIGN and U+03A9 GREEK CAPITAL LETTER OMEGA: another singleton; NFC yields U+03A9.

      Wrong (compatibility only):

      - U+FB01 LATIN SMALL LIGATURE FI and `fi`: NFC keeps the ligature; only NFKC/NFKD expand it.
      - U+2460 CIRCLED DIGIT ONE and `1`: NFKC maps ① to 1; NFC keeps ①.
    options:
      - text: "U+FB01 (ﬁ ligature) and `fi`"
      - text: "U+2460 (①) and `1`"
      - text: "U+00E9 and `e` + U+0301"
        correct: true
      - text: "U+212B ANGSTROM SIGN and U+00C5"
        correct: true
      - text: "U+2126 OHM SIGN and U+03A9"
        correct: true
  - id: "nfkc-versus-nfc-ligature"
    type: "single"
    prompt: "What does this Python line print?\n\n```python\nimport unicodedata as ud\nword = chr(0xFB01) + \"le\"          # \"ﬁle\" with the fi ligature\nprint(ud.normalize(\"NFKC\", word) == \"file\", ud.normalize(\"NFC\", word) == \"file\")\n```"
    explanation: |-
      U+FB01 has a *compatibility* decomposition `<compat> 0066 0069` and no canonical decomposition. NFKC applies compatibility decompositions, so `ﬁle` becomes `file` → `True`. NFC applies only canonical decompositions, so the ligature stays and the comparison is `False`.

      Output: `True False`.

      "True True" assumes NFC also expands ligatures.

      "False False" assumes no normalization form touches ligatures.

      "False True" reverses the roles of the canonical and compatibility forms.
    options:
      - text: "True True"
      - text: "False False"
      - text: "True False"
        correct: true
      - text: "False True"
  - id: "hangul-nfd-bytes"
    type: "single"
    prompt: "The Hangul syllable 한 is U+D55C. How many UTF-8 bytes does its NFD form take?"
    explanation: |-
      Precomposed Hangul syllables decompose algorithmically (Unicode Standard, Hangul syllable decomposition) into leading consonant, vowel and optional trailing consonant jamo: U+D55C → U+1112 U+1161 U+11AB. Each jamo is in U+0800–U+FFFF, so each takes 3 UTF-8 bytes: 3 × 3 = 9 bytes. The NFC form is the single U+D55C, 3 bytes.

      "3" is the NFC size.

      "6" assumes two jamo; 한 has a final consonant (ㄴ, U+11AB), so it decomposes into three.

      "4" assumes the decomposed form is a supplementary character; jamo are in the BMP.
    options:
      - text: "3"
      - text: "6"
      - text: "4"
      - text: "9"
        correct: true
  - id: "normalize-then-concatenate"
    type: "single"
    prompt: "A service stores each field in NFC. A suffix field may start with a combining mark. Is the concatenation guaranteed to be NFC?\n\n```python\nimport unicodedata as ud\na = ud.normalize(\"NFC\", \"e\")\nb = ud.normalize(\"NFC\", chr(0x301))\njoined = a + b\n```"
    explanation: |-
      No. Normalization is not closed under concatenation: two NFC strings can join into a non-NFC string when the second begins with a character that combines with the end of the first. Here `joined` is `e` + U+0301, while `ud.normalize("NFC", joined)` is U+00E9. Comparisons or hashes computed on `joined` will not match the same text entered precomposed. The fix is to normalize *after* building the final string (UAX #15 also describes how to limit re-normalization to the boundary region).

      "Yes, normalized pieces always concatenate to a normalized string" is the misconception being tested.

      "Yes, because U+0301 alone is already in NFC" is true of each piece but says nothing about the combination.

      "No, because NFC strings may never contain U+0301" is false: U+0301 is allowed in NFC whenever no precomposed character exists for the combination (for example after `q`).
    options:
      - text: "No; re-normalize after joining, since NFC is not closed under concatenation."
        correct: true
      - text: "Yes, normalized pieces always concatenate to a normalized string."
      - text: "Yes, because U+0301 alone is already in NFC."
      - text: "No, because NFC strings may never contain U+0301."
  - id: "normalization-stability"
    type: "single"
    prompt: "Which statement about Unicode normalization is guaranteed?"
    explanation: |-
      Normalization forms are idempotent: applying NFC to an NFC string returns it unchanged, so NFC(NFC(s)) \== NFC(s) for every s (the same holds for NFD, NFKC and NFKD). Unicode's Normalization Stability Policy additionally guarantees that a string normalized under one version stays normalized under later versions, as long as it contains only characters assigned in the earlier version.

      "NFD(NFC(s)) \== s for every string s" fails for singletons: s = U+2126 OHM SIGN → NFC gives U+03A9 → NFD gives U+03A9, not U+2126. Normalization deliberately loses the distinction.

      "NFC output can change between Unicode versions for any string" contradicts the stability policy.

      "NFKC preserves meaning, so it is safe for storing documents" is false: NFKC maps `x²` to `x2` and `①` to `1`, which changes content. It is meant for identifiers and matching, not for storing text.
    options:
      - text: "NFD(NFC(s)) \\== s for every string s."
      - text: "NFC(NFC(s)) \\== NFC(s) for every string s."
        correct: true
      - text: "NFC output can change between Unicode versions for any string."
      - text: "NFKC preserves meaning, so it is safe for storing documents."
---

## Learning objectives

- Distinguish canonical from compatibility equivalence.
- Predict the NFC, NFD, NFKC and NFKD forms of common sequences.
- Explain why normalization must be reapplied after concatenation.
- Choose a normalization form for storage, comparison and identifiers.

## The invariant

The same text can be encoded in different code-point sequences. **UAX #15, Unicode Normalization Forms**, defines two kinds of equivalence and four forms:

| | Canonical decomposition only | Plus compatibility decomposition |
|---|---|---|
| Decomposed | NFD | NFKD |
| Recomposed | NFC | NFKC |

- **Canonical equivalence**: sequences that represent the same character and should be indistinguishable to users, e.g. U+00E9 versus `e` + U+0301, or U+212B ANGSTROM SIGN versus U+00C5. NFD and NFC unify them.
- **Compatibility equivalence**: sequences that are "the same character" only after dropping a formatting distinction, e.g. the ligature ﬁ versus `fi`, superscript ² versus `2`, circled ① versus `1`, full-width Ａ versus `A`. Only NFKD and NFKC unify them, and they lose information doing so.

The algorithm: decompose recursively (canonical, or canonical plus compatibility), put combining marks into canonical order by their combining class, and, for the C forms, recompose pairs that have a primary composite. Properties that follow:

- **Idempotent**: normalizing a normalized string changes nothing.
- **Not closed under concatenation**: NFC(a) + NFC(b) may not be NFC.
- **Not length-preserving**: Hangul syllables triple in code points under NFD; singletons such as U+2126 disappear under NFC.
- **Stable across versions** for assigned characters (Unicode's normalization stability policy).

Practical rule: store and transmit text in NFC (the form most input produces and the one the W3C recommends for web content), normalize at input boundaries, and compare canonically equivalent strings by normalizing both sides. Use NFKC (usually with case folding) only for identifiers and search keys, never as a storage transform for documents.

## Worked example

| Input | NFD | NFC | NFKC |
|---|---|---|---|
| é (U+00E9) | 0065 0301 | 00E9 | 00E9 |
| Å (U+212B) | 0041 030A | 00C5 | 00C5 |
| ﬁ (U+FB01) | FB01 | FB01 | 0066 0069 |
| x² | 0078 00B2 | 0078 00B2 | 0078 0032 |
| 한 (U+D55C) | 1112 1161 11AB | D55C | D55C |

```python
import unicodedata as ud
decomposed = "e" + chr(0x301)
precomposed = chr(0xE9)
print(decomposed == precomposed)                                   # False
print(ud.normalize("NFC", decomposed) == precomposed)              # True
print(len(ud.normalize("NFD", chr(0xD55C)).encode("utf-8")))       # 9
```

```csharp
string decomposed = "e\u0301";
Console.WriteLine(decomposed == "é");                                        // False
Console.WriteLine(decomposed.Normalize(NormalizationForm.FormC) == "é");     // True
Console.WriteLine(string.Equals(decomposed, "é", StringComparison.Ordinal)); // False
```

## Where this shows up today

As of November 2026, input methods, copy-paste between applications and some file systems still produce decomposed text, while most web content is NFC. Search boxes, deduplication jobs and uniqueness constraints that compare raw code points therefore keep reporting "different" strings that users cannot tell apart.

## Pitfalls checklist

- Comparing user-visible text by raw code points without normalizing.
- Normalizing pieces and then concatenating, assuming the result stays normalized.
- Using NFKC as a storage transform and silently changing documents (x² → x2).
- Assuming NFD(NFC(s)) round-trips to the original sequence.
- Normalizing in one service and not another, so hashes and keys disagree.

## Key takeaways

- NFC/NFD unify canonical equivalents; NFKC/NFKD also fold compatibility variants and lose information.
- Normalization is idempotent and version-stable, but not closed under concatenation.
- Store text in NFC; compare by normalizing both sides; reserve NFKC for identifiers and matching.
- Byte and code-point lengths change under normalization, so normalize before enforcing limits.
