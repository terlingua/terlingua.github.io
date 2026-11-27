---
title: "Case Mapping and Collation"
questions:
  - id: "turkish-toupper-bug"
    type: "single"
    prompt: "A C# service runs with the current culture set to Turkish. What does this print?\n\n```csharp\nCultureInfo.CurrentCulture = new CultureInfo(\"tr-TR\");\nConsole.WriteLine(\"file\".ToUpper() == \"FILE\");\n```"
    explanation: |-
      `ToUpper()` without arguments uses the current culture. Turkish has a dotted/dotless i distinction: lowercase `i` uppercases to `İ` (U+0130 LATIN CAPITAL LETTER I WITH DOT ABOVE), and uppercase `I` lowercases to `ı` (U+0131). So `"file".ToUpper()` is `"FİLE"`, which is not equal to `"FILE"`: the program prints `False`. This is the classic "Turkish I" bug in keyword, file-extension and protocol comparisons.

      "True" assumes case mapping is the same in every culture.

      "It throws `CultureNotFoundException`" is wrong: `tr-TR` is a standard culture.

      "It depends on the console font" confuses rendering with string content; the comparison is decided by code points.
    options:
      - text: "True"
      - text: "False"
        correct: true
      - text: "It throws `CultureNotFoundException`."
      - text: "It depends on the console font."
  - id: "sharp-s-uppercase"
    type: "single"
    prompt: "What does this Python line print?\n\n```python\nprint(\"straße\".upper(), len(\"straße\".upper()))\n```"
    explanation: |-
      Unicode's full case mapping (SpecialCasing.txt) uppercases ß (U+00DF) to the two letters `SS`; Python's `str.upper` applies full mappings. The result is `STRASSE`, 7 code points, one longer than the 6-letter input. Case mapping can change length, so it cannot be done in place on fixed-size buffers.

      "STRAßE 6" assumes ß has no uppercase and is left alone; that is what *simple* one-to-one mapping (used by .NET's `ToUpperInvariant`, for example) does.

      "STRAẞE 6" uses U+1E9E LATIN CAPITAL LETTER SHARP S. It exists, but it is not the default uppercase mapping.

      "STRASSE 6" gets the string right but miscounts its length.
    options:
      - text: "STRAßE 6"
      - text: "STRAẞE 6"
      - text: "STRASSE 7"
        correct: true
      - text: "STRASSE 6"
  - id: "casefold-versus-lower"
    type: "single"
    prompt: "What does this Python line print?\n\n```python\nprint(\"Straße\".lower() == \"strasse\", \"Straße\".casefold() == \"strasse\")\n```"
    explanation: |-
      `lower()` applies the lowercase mapping: ß is already lowercase, so the result is `"straße"`, which differs from `"strasse"` → `False`. `casefold()` applies Unicode *case folding* (CaseFolding.txt, full folding), which is designed for caseless matching and maps ß to `ss` → `True`.

      Output: `False True`.

      "True True" assumes lowercasing expands ß.

      "False False" assumes no caseless operation treats ß and ss as equal.

      "True False" reverses the two operations.

      Case folding, not lowercasing, is the Unicode-recommended basis for case-insensitive comparison.
    options:
      - text: "True True"
      - text: "False False"
      - text: "True False"
      - text: "False True"
        correct: true
  - id: "ordinal-sort-order"
    type: "single"
    prompt: "What does Python return for `sorted([\"Zebra\", \"apple\", \"Éclair\"])`?"
    explanation: |-
      Python compares strings by code point. `Z` is U+005A, `a` is U+0061 and `É` is U+00C9, so the order is Zebra (0x5A) < apple (0x61) < Éclair (0xC9): `['Zebra', 'apple', 'Éclair']`. Every uppercase ASCII letter sorts before every lowercase one, and accented letters sort after all of ASCII.

      "['apple', 'Éclair', 'Zebra']" is what a linguistic collation (UTS #10 with English or root rules) produces; Python's default comparison is not linguistic.

      "['apple', 'Zebra', 'Éclair']" is case-insensitive ASCII order, which `sorted` does not apply without a `key`.

      "['Éclair', 'Zebra', 'apple']" assumes accented characters sort first.
    options:
      - text: "['Zebra', 'apple', 'Éclair']"
        correct: true
      - text: "['apple', 'Éclair', 'Zebra']"
      - text: "['apple', 'Zebra', 'Éclair']"
      - text: "['Éclair', 'Zebra', 'apple']"
  - id: "header-comparison-choice"
    type: "single"
    prompt: "A C# service must match the HTTP header name `content-type` case-insensitively against whatever a client sends. Which comparison is the right choice?"
    explanation: |-
      Protocol identifiers (header names, file extensions, keywords, enum names) are not natural-language text, so they must be compared without linguistic rules. `StringComparison.OrdinalIgnoreCase` compares code units after simple case mapping that is independent of culture, so it gives the same answer on every machine and never hits the Turkish I.

      "`StringComparison.CurrentCultureIgnoreCase`" depends on the server's culture: under `tr-TR`, `"CONTENT-TYPE"` and `"content-type"` do not match because I and i are not case pairs in Turkish.

      "`StringComparison.InvariantCultureIgnoreCase`" avoids the Turkish problem but still applies linguistic collation rules meant for human text (and is slower); protocol identifiers need exact code-unit comparison.

      "Call `ToLower()` on both strings and compare with `==`" uses the current culture again and allocates two strings; it has the same Turkish failure.
    options:
      - text: "`StringComparison.CurrentCultureIgnoreCase`"
      - text: "`StringComparison.OrdinalIgnoreCase`"
        correct: true
      - text: "`StringComparison.InvariantCultureIgnoreCase`"
      - text: "Call `ToLower()` on both strings and compare with `==`"
  - id: "locale-dependent-collation"
    type: "multiple"
    prompt: "Consider sorting the words `zebra`, `äpple` and `apple`. Which statements are correct? Select all that apply."
    explanation: |-
      Collation is language-specific; UTS #10 (the Unicode Collation Algorithm) supplies a default ordering, and CLDR supplies per-language tailorings.

      Correct:

      - Swedish treats ä as a separate letter after z, so Swedish collation gives apple, zebra, äpple.
      - German treats ä as a variant of a (a secondary, accent-level difference), so German collation gives apple, äpple, zebra.
      - Code-point order gives apple, zebra, äpple, because ä is U+00E4, larger than every ASCII letter.

      Wrong:

      - "The UCA default order puts `äpple` after `zebra`" is false: the root order also treats ä as a with a diacritic, giving apple, äpple, zebra.
      - "Collation is identical in every locale because each character has one Unicode weight" ignores tailoring; the same three words already sort differently in Swedish and German.

      Note that Swedish order and code-point order agree here only by coincidence.
    options:
      - text: "The UCA default order puts `äpple` after `zebra`."
      - text: "Collation is identical in every locale because each character has one Unicode weight."
      - text: "Swedish collation orders them apple, zebra, äpple."
        correct: true
      - text: "German collation orders them apple, äpple, zebra."
        correct: true
      - text: "Code-point order puts `äpple` last."
        correct: true
---

## Learning objectives

- Explain why case mapping is language-sensitive and can change string length.
- Use case folding rather than lowercasing for caseless matching.
- Distinguish code-point (ordinal) ordering from linguistic collation.
- Pick ordinal or culture-sensitive comparison according to what the string represents.

## The invariant

**Case mapping** is defined by the Unicode Character Database. `UnicodeData.txt` gives simple one-to-one mappings; `SpecialCasing.txt` adds full mappings that change length (ß → SS, ŉ → ʼN) and language-specific rules (Turkish and Azeri dotted and dotless i, Lithuanian dot retention). **Case folding** (`CaseFolding.txt`) is a separate mapping designed for caseless comparison: it maps ß to ss and is mostly, but not entirely, the same as lowercasing.

Three consequences:

1. Case mapping depends on language. `i` ↔ `I` is correct for English but wrong for Turkish, where `i` ↔ `İ` and `ı` ↔ `I`.
2. Case mapping can change length, so "uppercase in place" and "same length after upper()" are invalid assumptions.
3. Lowercase-then-compare is not caseless matching; fold both sides (Unicode's recommendation) or use an API that does.

**Collation** is the ordering of strings for humans. Code-point order (what `<` does in Python, JavaScript and Java, and what `StringComparison.Ordinal` does in .NET) is deterministic and fast, but it puts all uppercase ASCII before lowercase and accented letters after `z`. Linguistic order is defined by **UTS #10, the Unicode Collation Algorithm**: each character maps to multi-level weights (base letter, accent, case, then tie-breakers), and CLDR tailors those weights per language. The same words can sort differently in Swedish and German, and a user expects their own language's order.

The engineering rule follows directly: **identifiers get ordinal comparison; human text gets linguistic comparison with an explicit locale.** Mixing them produces either the Turkish I bug (linguistic rules applied to protocol strings) or user-hostile sorting (code-point order applied to names).

## Worked example

| Operation | Input | Result | Rule |
|---|---|---|---|
| Python `upper()` | straße | STRASSE (7) | full case mapping |
| Python `lower()` | Straße | straße | ß is already lowercase |
| Python `casefold()` | Straße | strasse | full case folding |
| .NET `ToUpper()` in tr-TR | file | FİLE | Turkish tailoring |
| JS `toLocaleLowerCase("tr")` | I | ı | Turkish tailoring |
| Python `sorted` | Zebra, apple, Éclair | Zebra, apple, Éclair | code-point order |
| `Intl.Collator("en")` | Zebra, apple, Éclair | apple, Éclair, Zebra | UCA with English rules |
| `Intl.Collator("sv")` | zebra, äpple, apple | apple, zebra, äpple | Swedish tailoring |

```javascript
const words = ["zebra", "äpple", "apple"];
console.log([...words].sort(new Intl.Collator("de").compare));  // [ 'apple', 'äpple', 'zebra' ]
console.log([...words].sort(new Intl.Collator("sv").compare));  // [ 'apple', 'zebra', 'äpple' ]
console.log([...words].sort());                                  // [ 'apple', 'zebra', 'äpple' ]
```

```csharp
bool isJson = string.Equals(ext, ".JSON", StringComparison.OrdinalIgnoreCase);  // identifier
int order = string.Compare(a, b, new CultureInfo("de-DE"), CompareOptions.None); // human text
```

## Where this shows up today

As of November 2026, collation in browsers, .NET (on ICU) and most databases comes from ICU and CLDR data, which are updated alongside each Unicode release. The same `ORDER BY` or `Intl.Collator` call can therefore order rare characters differently after a runtime or database upgrade, which matters for paginated APIs that use sort keys as cursors.

## Pitfalls checklist

- Calling culture-sensitive `ToUpper`/`ToLower` on keywords, file extensions or protocol tokens.
- Implementing case-insensitive equality as lowercase-and-compare instead of case folding or an ordinal-ignore-case API.
- Assuming uppercase and lowercase strings have the same length.
- Sorting user-visible names by code point, or sorting identifiers with a culture that can change.
- Persisting sort order or cursors that depend on a collation version without recording that version.

## Key takeaways

- Case mapping is language-sensitive and may change length (ß → SS); Turkish i is the classic trap.
- Use case folding (or ordinal-ignore-case APIs) for caseless matching.
- Code-point order is not alphabetical order; UTS #10 plus CLDR tailorings define linguistic order per language.
- Identifiers: ordinal comparison. Human text: linguistic comparison with an explicit locale.
