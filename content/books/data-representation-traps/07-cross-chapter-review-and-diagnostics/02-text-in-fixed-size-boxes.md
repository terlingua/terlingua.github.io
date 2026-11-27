---
title: "Review: Text in Fixed-Size Boxes"
questions:
  - id: "byte-truncated-status-text"
    type: "single"
    prompt: "Bug report: \"Status messages sometimes end in a black diamond with a question mark.\" The producer limits each message to 7 bytes; the consumer decodes leniently. What does this print?\n\n```python\nraw = \"Café ☕\".encode(\"utf-8\")[:7]\nprint(repr(raw.decode(\"utf-8\", errors=\"replace\")))\n```"
    explanation: |-
      Byte counts (Chapter 3): C, a, f are 1 byte each; é (U+00E9) is 2 bytes (`C3 A9`); the space is 1; ☕ (U+2615) is 3 bytes (`E2 98 95`). Total 9 bytes. Cutting at 7 keeps `C a f C3 A9 20 E2`: the lone lead byte `E2` is a truncated sequence, which the replace handler turns into one U+FFFD. Output: `'Café �'`; Python's `repr` shows U+FFFD as the character itself, a black diamond with a question mark.

      Minimal fix: truncate on a code-point boundary by backing up over continuation bytes (`10xxxxxx`) to the last lead byte, or better, on a grapheme boundary (Chapter 4) so that combining sequences and emoji stay whole.

      "`'Café ☕'`" assumes the limit counts characters.

      "`'Café'`" assumes the decoder silently drops the fragment and the trailing space.

      "`'Caf� ☕'`" assumes the cut damaged é; é ends at byte 5, well inside the limit.
    options:
      - text: "`'Café �'`"
        correct: true
      - text: "`'Café ☕'`"
      - text: "`'Café'`"
      - text: "`'Caf� ☕'`"
  - id: "substring-splits-emoji-preview"
    type: "single"
    prompt: "Bug report: \"Notification previews for some messages end in a replacement character.\" The service does this:\n\n```csharp\nstring text = \"Great job 👍 team\";\nstring preview = text.Length > 11 ? text.Substring(0, 11) : text;\nbyte[] payload = Encoding.UTF8.GetBytes(preview);\n```\n\nWhat is the root cause, and what is the minimal correct fix?"
    explanation: |-
      `"Great job "` is 10 UTF-16 code units; 👍 (U+1F44D) occupies units 10 and 11 as the surrogate pair `D83D DC4D` (Chapter 3). `Substring(0, 11)` keeps only the high surrogate. A lone surrogate has no UTF-8 encoding, so `Encoding.UTF8.GetBytes` substitutes U+FFFD (`EF BF BD`). The limit was counted in UTF-16 code units, which are neither characters users see nor bytes.

      Minimal fix: truncate by grapheme clusters (Chapter 4), e.g. take text elements from `StringInfo.GetTextElementEnumerator` until the budget is reached, so `👍` and any modifier or ZWJ sequence stay intact.

      "UTF-8 cannot encode emoji; switch the payload to UTF-16" is false: UTF-8 encodes every scalar value; the input was ill-formed.

      "Increase the limit to 12 code units for this message" fixes this message and breaks the next one with an emoji at a different offset.

      "Call `Normalize()` on the text before `Substring`" does not help: normalization does not change surrogate pairs.
    options:
      - text: "UTF-8 cannot encode emoji; switch the payload to UTF-16."
      - text: "It splits a surrogate pair; cut on grapheme boundaries."
        correct: true
      - text: "Increase the limit to 12 code units for this message."
      - text: "Call `Normalize()` on the text before `Substring`."
  - id: "duplicate-accented-emails"
    type: "single"
    prompt: "Bug report: \"Two accounts exist for josé@example.com, although the `email` column has a unique index.\" Hex dumps show one value contains `C3 A9` and the other `65 CC 81` where the é is. What is the root cause, and what is the fix?"
    explanation: |-
      `C3 A9` is UTF-8 for U+00E9 (precomposed é); `65 CC 81` is `e` followed by U+0301 COMBINING ACUTE ACCENT (Chapter 3 byte patterns). The strings are canonically equivalent (Chapter 4) and look identical, but they are different code-point sequences, so a byte- or code-point-based unique index treats them as distinct.

      Fix: normalize identifiers (at least to NFC; for case-insensitive keys, NFKC plus case folding) before storing them, and put the unique constraint on the normalized key.

      "The database collation is broken" misplaces the problem: most collations and indexes compare stored values without normalizing them, by design.

      "One client sent UTF-16 instead of UTF-8" is ruled out by the dumps: both values are valid UTF-8.

      "The unique index ignores accents" would merge the two, not keep them apart.
    options:
      - text: "The database collation is broken."
      - text: "One client sent UTF-16 instead of UTF-8."
      - text: "Different normalization forms; store NFC keys."
        correct: true
      - text: "The unique index ignores accents."
  - id: "bom-breaks-header-lookup"
    type: "single"
    prompt: "Bug report: \"CSV import fails with `KeyError: 'id'`, but the header clearly starts with id.\" The importer runs:\n\n```python\nwith open(path, encoding=\"utf-8\") as f:\n    header = f.readline().rstrip(\"\\n\").split(\",\")\nprint(len(header[0]), header[0] == \"id\")\n```\n\nThe file was exported by a spreadsheet application. What does the diagnostic print, and what is the minimal fix?"
    explanation: |-
      Spreadsheet exports often begin with the UTF-8 byte order mark `EF BB BF` (Chapter 3). The `utf-8` codec decodes it as U+FEFF, an invisible character, so the first field is U+FEFF followed by `id`: length 3, not equal to `"id"`. Output: `3 False`.

      Minimal fix: open with `encoding="utf-8-sig"`, which strips one leading BOM if present and otherwise behaves like `utf-8`.

      "2 True" would mean there is no BOM and the lookup would succeed.

      "5 False" counts the BOM's 3 bytes plus 2 letters, but `len` on a decoded `str` counts code points.

      "3 True" is contradictory: a 3-character string cannot equal a 2-character one.
    options:
      - text: "`2 True`; the bug is elsewhere"
      - text: "`5 False`; decode with `latin-1`"
      - text: "`3 True`; the bug is elsewhere"
      - text: "`3 False`; open with `utf-8-sig`"
        correct: true
  - id: "varchar-ten-five-emoji"
    type: "multiple"
    prompt: "A display name of five emoji, `👋👋👋👋👋` (five times U+1F44B), must be stored. Which column definitions accept it without truncation or error? Select all that apply."
    explanation: |-
      Each U+1F44B is 1 code point, 2 UTF-16 code units and 4 UTF-8 bytes (Chapter 3), so the value is 5 code points, 10 UTF-16 units, 20 UTF-8 bytes. What `N` counts is vendor-specific, which is exactly why this matters:

      Correct:

      - PostgreSQL `VARCHAR(10)` in a UTF8 database limits *characters* (code points): 5 ≤ 10.
      - MySQL `VARCHAR(10) CHARACTER SET utf8mb4` limits characters: 5 ≤ 10.
      - SQL Server `NVARCHAR(10)` limits byte-pairs, i.e. UTF-16 code units: 10 ≤ 10, exactly at the limit.

      Wrong:

      - SQL Server `VARCHAR(10)` with a UTF-8 collation limits *bytes*: 20 > 10.
      - MySQL `VARCHAR(10) CHARACTER SET utf8mb3` cannot store supplementary characters at all, since utf8mb3 stops at 3 bytes per character.
    options:
      - text: "SQL Server `VARCHAR(10)` with a UTF-8 collation"
      - text: "MySQL `VARCHAR(10) CHARACTER SET utf8mb3`"
      - text: "PostgreSQL `VARCHAR(10)` in a UTF8 database"
        correct: true
      - text: "MySQL `VARCHAR(10) CHARACTER SET utf8mb4`"
        correct: true
      - text: "SQL Server `NVARCHAR(10)`"
        correct: true
  - id: "mojibake-diagnosis"
    type: "single"
    prompt: "Bug report: \"Customer names like Zoë show up as ZoÃ« in exported reports.\" Which mistake produces exactly `ZoÃ«`?"
    explanation: |-
      ë is U+00EB, encoded in UTF-8 as `C3 AB` (Chapter 3). Decoding those two bytes as Windows-1252 (or Latin-1) maps `C3` to Ã and `AB` to «, giving `ZoÃ«`. The signature of UTF-8-read-as-Latin-1 is an Ã or Â followed by another symbol for every non-ASCII character.

      Fix: decode the export as UTF-8 (and declare the encoding in the file or HTTP header).

      "Latin-1 bytes were decoded as UTF-8" goes the other way: ë in Latin-1 is the single byte `EB`, an invalid UTF-8 lead without continuation, which a decoder turns into U+FFFD, not Ã«.

      "UTF-16 bytes were decoded as UTF-8" would produce NUL characters between ASCII letters.

      "The name was normalized to NFD" does not explain it: NFD turns ë into `e` + U+0308, which displays as ë; normalization never introduces Ã.
    options:
      - text: "Latin-1 bytes were decoded as UTF-8."
      - text: "UTF-8 bytes were decoded as Windows-1252."
        correct: true
      - text: "UTF-16 bytes were decoded as UTF-8."
      - text: "The name was normalized to NFD."
---

## Learning objectives

- Diagnose truncation bugs by identifying which unit a limit counts.
- Combine byte-pattern, surrogate and grapheme knowledge to choose a safe cut point.
- Recognize normalization, BOM and mojibake signatures from hex dumps and lengths.
- Map application length limits onto vendor-specific `VARCHAR(N)` semantics.

## The invariant

Every "box" a string must fit into counts in some unit, and the units differ:

| Box | Counts | Source of the rule |
|---|---|---|
| Network or queue payload limit | UTF-8 bytes | protocol |
| `string.Length`, `.length`, `Substring` | UTF-16 code units | .NET, JavaScript, Java runtimes |
| Python `len`, slicing | code points | Python |
| PostgreSQL `VARCHAR(N)` | characters (code points) | vendor |
| MySQL `VARCHAR(N)` | characters, in the column's character set | vendor |
| SQL Server `NVARCHAR(N)` / `VARCHAR(N)` | UTF-16 code units / bytes | vendor |
| What users count | grapheme clusters | UAX #29 |

Bugs appear where a value is measured in one unit and cut in another, or where the cut lands inside a multi-unit sequence. The invariants from earlier chapters tell you which cuts are safe:

- **UTF-8**: never cut before a continuation byte (`10xxxxxx`); back up to a lead byte (Chapter 3).
- **UTF-16**: never cut between a high and a low surrogate (Chapter 3).
- **Users**: never cut inside a grapheme cluster (Chapter 4).
- **Equality and uniqueness**: compare normalized keys, not raw code points (Chapter 4).

A limit is only meaningful together with its unit, so write both into the contract: "at most 64 UTF-8 bytes" or "at most 20 grapheme clusters", never "at most 64 characters". When a value has to satisfy several boxes at once (a user-facing limit in graphemes, a database column in code points and a queue in bytes), validate against each unit separately; no single count implies the others, because a grapheme cluster can be arbitrarily many code points and a code point can be up to 4 bytes.

And three signatures identify encoding mix-ups at a glance:

| Symptom | Cause |
|---|---|
| `Ã©`, `Ã«`, `â‚¬` | UTF-8 decoded as Windows-1252/Latin-1 |
| U+FFFD (black diamond) | invalid or truncated UTF-8, or a lone surrogate encoded |
| Invisible first character, `len` one too large | UTF-8 BOM decoded as U+FEFF |

## Worked example

Truncate "Café ☕" to a 7-byte budget safely:

| Byte index | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 |
|---|---|---|---|---|---|---|---|---|---|
| Byte | 43 | 61 | 66 | C3 | A9 | 20 | E2 | 98 | 95 |
| Role | lead | lead | lead | lead | cont | lead | lead | cont | cont |

The naive cut at index 7 keeps the lead byte `E2` without its continuations. Backing up from index 7 to the last lead byte gives index 6, so the safe cut keeps bytes 0–5: "Café " (6 bytes).

```python
def truncate_utf8(data: bytes, limit: int) -> bytes:
    if len(data) <= limit:
        return data
    cut = limit
    while cut > 0 and (data[cut] & 0xC0) == 0x80:
        cut -= 1
    return data[:cut]

print(truncate_utf8("Café ☕".encode(), 7).decode())   # 'Café ' (no replacement character)
```

This keeps code points whole; for user-visible text, cut on grapheme clusters as shown in Chapter 4.

## Where this shows up today

As of November 2026, push-notification services, SMS gateways and message queues still impose byte or code-unit limits, and database migrations between vendors carry `VARCHAR(N)` definitions whose `N` silently changes meaning. Emoji-heavy user content keeps exposing every mismatch.

## Pitfalls checklist

- Truncating byte buffers or UTF-16 strings at a fixed index.
- Copying `VARCHAR(N)` definitions across databases without checking what `N` counts.
- Building unique keys from raw, unnormalized input.
- Decoding files without considering a BOM, or exporting UTF-8 without declaring it.
- Treating U+FFFD in output as a display problem rather than as evidence of data loss.

## Key takeaways

- Every limit counts a unit; find it before you cut.
- Safe cut points: UTF-8 lead bytes, outside surrogate pairs, and grapheme boundaries for users.
- Mojibake, replacement characters and invisible BOMs each have a recognizable signature.
- Normalize before comparing or enforcing uniqueness.
