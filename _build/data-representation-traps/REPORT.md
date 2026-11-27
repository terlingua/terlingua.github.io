# REPORT — Data Representation Traps: Floating-Point, Unicode, and Time

Built 2026-11-27. Book slug `data-representation-traps`, author `terlingua-team`, `date: 2026-11-27`.

## Contents

| Chapter | Sections | Questions |
|---|---|---|
| 01-binary-floating-point-anatomy | 4 | 24 |
| 02-floating-point-arithmetic-and-traps | 5 | 30 |
| 03-code-points-utf-8-and-utf-16 | 4 | 25 |
| 04-text-length-indexing-normalization-and-sorting | 4 | 24 |
| 05-calendars-epochs-and-clocks | 4 | 24 |
| 06-time-zones-dst-and-serialization | 4 | 24 |
| 07-cross-chapter-review-and-diagnostics | 3 | 18 |
| **Total** | **28** | **169** (141 single, 28 multiple) |

Prose per section (excluding code and tables): 408–666 words; 14,015 words in total.

## Validation

`validate.py` checks every HARD FORMAT RULE: naming regexes, book/chapter keys, double-quoted strings, `|-` explanations, no `---` lines inside front matter, 5–7 questions, ids (slug, unique, not reserved), `type` present and consistent (single = 1 correct of 4–5; multiple = 2–3 correct of 5, prompt ends with "Select all that apply."), `correct: true` only on right answers, no option letters/positions in explanations, no "all/none of the above", body heading order and bullet counts, code-fence languages, prose length, "Where this shows up today" ≤ 20% (warning above 15.5%) with an "As of November 2026" statement, no literal invisible/combining characters, answer positions `(k + s) mod n` (and following indexes for multiple), the 40% position-concentration rule (on all correct options and on first-correct positions), and correct-answer length balance. It also contains a Python port of the content processor's `FrontMatterParser`, `ContentNaming`, `BookCompiler` and `QuestionCompiler` rules.

Final output (`validate_output.txt`):

```
chapters=7 sections=28 questions=169 single=141 multiple=28 correct-is-longest=21/141
prose words: min=408 max=666 total=14015
0 error(s), 0 warning(s)
```

YAML was also parsed with ruamel.yaml (YAML 1.2) and PyYAML (YAML 1.1); both produce identical data for all 36 files.

**Real compiler not run.** The sandbox cannot reach `dot.net`, `builds.dotnet.microsoft.com` or `api.nuget.org`, so the .NET 11 SDK and NuGet packages could not be installed. Please run once locally:

```bash
dotnet run --project tools/ObserverMagazine.ContentProcessor
dotnet test
```

## Math audit

`check_math.py` recomputes every numeric claim used in answers and worked examples: bit patterns and decoded values (struct, NumPy float16/float32), ULPs, exact decimal expansions (Decimal/Fraction), ties, Kahan and naive sums, UTF-8/UTF-16 encodings and byte counts, normalization results, IDNA, Unix-time conversions and overflows, leap-year counts, TAI/GPS offsets, smear and slew rates, DST transitions and offsets via `zoneinfo`, ISO week dates. It also runs 14 JavaScript snippets in Node.js 22 (NaN/zero semantics, JSON parsing, `Intl.Segmenter`, `Intl.Collator`, `TextDecoder`, `encodeURIComponent`, Date parsing). Result: **216 checks passed, 14 JavaScript checks run** (`check_math_output.txt`).

## Question audit

- Every question was re-read; correct marks, explanations and distractor rationales were cross-checked. `audit.py` verifies that each distractor is addressed in its explanation by its option text and that multiple-answer explanations separate Correct/Wrong; its 16 remaining hits are confirmed false positives (prefix matches).
- Two sections lacked a code-output question and were fixed (s=4, s=18).
- Quotes in explanations were aligned to exact option text.

## Dated claims and how they were verified (2026-11-27)

| Claim | Source checked |
|---|---|
| Unicode 18.0 is current, released September 2026 | unicode.org/versions/latest → Unicode18.0.0; Unicode beta page; GCC libstdc++ patch (17 Sept 2026) |
| 28th CGPM Resolution 5: continuous UTC from 20 May 2027, abs(UT1 − UTC) ≤ 3,600 s | bipm.org/en/cgpm-2026 (lists Resolution 5); BIPM Draft Resolution C text |
| No leap second at end of 2026; TAI − UTC = 37 s | IERS Bulletin C 72 (6 Jul 2026) via tz mailing list |
| Temporal reached TC39 Stage 4 (March 2026) | Igalia announcement; proposal README |
| MySQL 8.4: `utf8` = deprecated alias of `utf8mb3` (BMP only); `TIMESTAMP` range ends 2038-01-19 03:14:07 UTC | MySQL 8.4 Reference Manual |
| Python 3.12 `sum()` uses Neumaier summation | Python 3.12 What's New |
| glibc 2.34 `_TIME_BITS=64` | glibc/gnulib announcements |
| OCP MX v1.1 (October 2026); IEEE P3109 still a draft | OCP spec PDF; IEEE P3109 interim report v4.0 (June 2026) |
| US and EU DST dates in 2026 | timeanddate.com, Live Science |

## Unresolved uncertainties and how they were handled

- **Not verifiable / dropped:** the ECMAScript edition that contains Temporal (sources disagree); exact Unicode 18.0 character totals (13,047 vs 13,007); the NumPy version that introduced pairwise summation; specific browser versions. These are stated generically or omitted.
- **Stated from long-standing documentation without a fresh fetch:** .NET behaviors (`Math.Round` half-even default, `AddYears` clamping, `ToUpperInvariant` simple case mapping, `Stopwatch.GetElapsedTime` in .NET 7+, `TimeProvider` in .NET 8, IANA IDs on all platforms since .NET 6), PostgreSQL `timestamptz` semantics, SQL Server `varchar`/`nvarchar` length units, RFC 3339 `-00:00` semantics and lowercase `t`/`z`, RFC 2781 big-endian default, UTS #39 skeleton guidance. All are vendor-labeled in the text; none is the sole basis of an answer.
- **C# snippets were not executed** (no .NET SDK); their outputs were reasoned from documented behavior and, where possible, cross-checked by computing the same values in Python.
- **Markdig rendering** was not executed; escaping of `^` and `==` was added defensively because `UseAdvancedExtensions()` enables superscript and mark syntax.

## Files

- `content/books/data-representation-traps/` — `book.yml`, 7 × `chapter.yml`, 28 section files.
- `_build/data-representation-traps/` — `plan.md`, `progress.md`, `NOTES.md`, `REPORT.md`, `common.py`, `normalize.py` (answer placement, canonical YAML emission, Markdig escaping), `validate.py`, `check_math.py`, `audit.py`, `fix_quotes.py`, `positions.py`, and the three `*_output.txt` logs. Delete `_build/` when done.
