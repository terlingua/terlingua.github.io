# NOTES — data-representation-traps

Decisions and conflicts, logged as the build proceeds. Source-of-truth order: compiler source > existing books > prompt.

## Repository facts learned (Step 0)

- `dump.txt` (project doc) read first; full repo cloned read-only from `github.com/terlingua/terlingua.github.io` at commit `033766a2` (2026-11-27) to read existing books. Only files under `content/books/data-representation-traps/` and `_build/data-representation-traps/` are created.
- Author id: `terlingua-team` exists in `content/authors/terlingua-team.yml` → used.
- Date format in existing `book.yml`: unquoted `YYYY-MM-DD` (e.g. `date: 2026-11-10`). Used `date: 2026-11-27`. `BookManifest.Date` is `DateTime`; YamlDotNet parses the plain scalar. A quoted date would also parse, but unquoted matches existing books.
- `publishBefore` defaults to `DateTime.UtcNow`; `2026-11-27` (midnight) is not in the future on the build day, so the book is not skipped.
- Naming: `ContentNaming` regexes — book slug `^[a-z0-9]+(?:-[a-z0-9]+)*$`; numbered entries `^(\d+)[-_]([a-z0-9]+(?:-[a-z0-9]+)*)$`. Files/folders starting with `.` or `_` are hidden (ignored).
- YAML keys: camelCase naming convention, unmatched properties ignored. Book: `title` (required), `author`, `summary`, `date`, `updated`, `tags`, `image`, `draft`. Chapter: `title` (required), `summary`, `draft`. Subchapter: `title` (required), `draft`, `questions`.
- Questions (`QuestionCompiler`): `id` optional (defaults `q1`…) but must match slug regex and be unique; `prompt` required; `type` optional: `single` | `multiple` (case-insensitive after trim); unknown type = error; `single` with >1 correct = error; ≥2 options; ≥1 correct. Option `id` optional (defaults `a`, `b`, …), `text` required, `correct` is a YAML bool (missing = false).
- `explanation` optional in the compiler; the prompt makes it mandatory → stricter rule applied.
- Front matter delimiter: content must start with `---`; the block ends at the FIRST occurrence of `\n---` anywhere after it. Therefore no line inside front matter may start with `---` (indented lines inside block scalars are safe). The body is everything after.
- Code fences: `SyntaxHighlighter` handles `csharp`(`cs`), `javascript`(`js`), `typescript`(`ts`), `python`, `sql`, `bash`(`sh`,`shell`,`zsh`), `json`, `yaml`(`yml`), `html`(`xml`), `css`. Intersection with the prompt's set = all eight of `csharp python javascript sql bash json yaml html`. Unknown languages would still render (plain), but are disallowed by the validator.

## Conflicts and resolutions

1. **Block scalars.** Existing books never use `explanation: |-` (59 files use folded `>`). YamlDotNet supports literal block scalars (YAML 1.1/1.2 core syntax), so `|-` is accepted by the compiler; prompt prefers it → used `|-` for every explanation.
2. **Real compiler not runnable.** The sandbox cannot reach `dot.net`, `builds.dotnet.microsoft.com` or `api.nuget.org` (proxy 403), so the .NET 11 SDK and the Markdig/YamlDotNet packages cannot be installed; `dotnet run --project tools/ObserverMagazine.ContentProcessor` was not possible. Mitigation: `validate.py` contains a faithful Python port of `FrontMatterParser.Parse` (delimiter logic), `ContentNaming`, `BookCompiler` (manifest/title/draft/numbering/duplicate checks) and `QuestionCompiler` (ids, options, correct counts, type resolution), parsed with PyYAML (YAML 1.1, which is stricter than YamlDotNet about booleans — every string is double-quoted, so the difference cannot bite). The author should run `dotnet run --project tools/ObserverMagazine.ContentProcessor` once locally; see REPORT.md.
3. **`draft: false`.** Existing book.yml files omit `draft`; the prompt requires `draft: false`. Harmless (default false) → included.
4. **`type` required by prompt, optional in compiler** → always written.
5. **Answer positions.** The prompt's formula is applied mechanically by `normalize.py`: options are authored in any order, then the script moves correct options to index `(k + s) mod n` (and the following indexes, wrapping, for `multiple`) while keeping distractors in authored order. Explanations never reference positions, so reordering is safe. `s` is the 1-based section number across the whole book (1–28).
6. **The 40% rule** is checked over all correct options (including every correct option of `multiple` questions) per section, and also over first-correct positions.
7. **Book `summary`/`title`** taken verbatim from the prompt.
8. **Section count:** 28 (4+5+4+4+4+4+3).

## Dated-claim verification log (all checked 2026-11-27)

- Unicode: `unicode.org/versions/latest` redirects to `Unicode18.0.0`; Unicode 18.0 released September 2026 (planned 15 Sept 2026 per unicode.org beta page; GCC libstdc++ patch dated 17 Sept 2026 updates to 18.0.0). Character totals differ between sources (13,047 vs 13,007 new) → no counts stated.
- CGPM: 28th CGPM (13–15 Oct 2026) adopted Resolution 5 "On the technical actions needed to ensure the continuity of UTC" (listed on bipm.org/en/cgpm-2026). Draft Resolution C text (bipm.org): continuous UTC effective 20 May 2027; max |UT1−UTC| = 3 600 s. 27th CGPM (2022) Resolution 4 is the earlier "in, or before, 2035" decision.
- IERS Bulletin C 72 (6 Jul 2026): no leap second end of December 2026; UTC−TAI = −37 s since 2017-01-01.
- Temporal: reached TC39 Stage 4 at the March 2026 plenary (Igalia announcement 2026-03-13; proposal README says Stage 4). Which ECMAScript edition includes it is reported inconsistently → not stated.
- MySQL 8.4 Reference Manual: `utf8` is a deprecated alias for `utf8mb3`; `utf8mb3` is BMP-only, max 3 bytes, deprecated; `utf8mb4` recommended. `TIMESTAMP` range '1970-01-01 00:00:01' UTC to '2038-01-19 03:14:07' UTC.
- Python 3.12 What's New: `sum()` uses Neumaier summation for floats.
- glibc 2.34: `_TIME_BITS=64` (with `_FILE_OFFSET_BITS=64`) gives 64-bit `time_t` on 32-bit Linux ABIs.
- IEEE P3109 (Arithmetic Formats for Machine Learning): still a draft / active PAR (interim report v4.0 dated June 2026). OCP MX specification v1.1 dated October 2026. OCP FP8 E4M3/E5M2 layouts from the OCP 8-bit floating point spec.
- Not stated because unverifiable or volatile: specific browser versions shipping Temporal, Linux kernel version numbers, current tzdata release name, NumPy internals beyond "pairwise".

## Section-level decisions

- Product facts used only in "Where this shows up today" or labeled vendor-specific; no answer depends solely on a dated fact. Questions that touch vendor behavior (e.g. MySQL `utf8mb3`) are explained from the invariant (4-byte UTF-8 for astral code points).

## Decisions made while writing (chronological)

- **Write-tool escape hazard.** Escapes such as `́` and `‍` typed into files were turned into the literal characters by the file-writing tool (seen in s=12, s=14, s=15). Fixed each case, then added guards: `normalize.py` rejects format, control, private-use, surrogate and combining characters (Cf/Cc/Co/Cs/Mn/Me) in YAML strings, and `validate.py` rejects them in bodies. Code samples now use `chr(0x301)` or explicit `"́"` escapes that were verified on disk.
- **ZWJ emoji are never written literally** (they contain U+200D); they are described by code points instead.
- **Markdig emphasis extras.** `MarkdownProcessor` uses `UseAdvancedExtensions()`, which enables `^superscript^` and `==mark==`. Prose such as "2^−126 … 2^−149" or "x == y … x == y" in one paragraph would render as superscript/highlight. `normalize.py` therefore escapes `^` as `\^` and `==` as `\==` outside code spans and fences in bodies, prompts, options and explanations (titles and chapter summaries are plain text and left alone). Checked that no `$`, `~`, `++`, `{`, `[^` or definition-list `: ` lines occur outside code.
- **Table pipes.** A `|` inside a table cell (bitwise OR in the UTF-16 decode formula) broke a table in s=13; rewrote the formula with `+`. A table-shape check found no other issues.
- **Mix rules per section** (≥2 compute/trace, ≥1 spot-the-bug/what-prints) were checked by hand; s=4 and s=18 lacked a what-prints question, so their first/second questions were rewritten as code-output questions (Python `struct` decoding of `0xFF800000`; JavaScript `| 0` int32 wrap).
- **Correct-answer length.** validate.py fails a section if the correct option is the unique longest in more than half of its single-answer questions (book-wide limit 40%); several option sets were rebalanced. Final: 21 of 141.
- **Explanation quotes** were normalized (fix_quotes.py) so every distractor is referred to by its exact option text; audit.py's remaining 16 hits are verified false positives (prefix matches such as "Plane 15" vs "Plane 1", or explanations that discuss a distractor inside a sentence instead of quoting it).
- **Dated questions** (at most one per section): s=4 OCP FP8 details, s=9 ISO 4217 exponents, s=13 MySQL 8.4 `utf8`, s=18 MySQL 8.4 `TIMESTAMP`, s=20 CGPM 2026 Resolution 5, s=24 tzdata packaging. Each explanation states the timeless rule that also justifies the answer.
- **Removed unverified version claims**: NumPy's pairwise-summation start version (not confirmed in primary sources) was replaced by a reference to the current NumPy documentation; exact Unicode 18.0 character counts omitted; ECMAScript edition containing Temporal omitted.
- **Plan deviations**: s=5 replaced "shortest round-trip printing" (covered in s=2) with a float-literal-versus-double question; s=13 has 7 questions; s=25 uses 2027-01-01 (week-year 2026-W53) for the ISO week question because it exposes the `%Y`/`%G` trap; s=18's wraparound question became a JavaScript `| 0` question.
