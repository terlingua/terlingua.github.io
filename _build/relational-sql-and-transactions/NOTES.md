# NOTES — relational-sql-and-transactions

Running log of decisions, rule sources and conflicts. Unattended run, 2026-11-27.

## Environment

- No local checkout existed. `dump.txt` (project doc, export of 2026-11-27T15:29Z) was read first.
  The public repository was then cloned (`git clone --depth 1`, HEAD 033766a, 2026-11-27) to read
  existing books, which the dump inventories but does not include. Authenticated GitHub API use was
  blocked by the sandbox, so the PAT was not used; the anonymous clone sufficed.
- No .NET SDK in the sandbox and the egress proxy blocks dot.net, builds.dotnet.microsoft.com and
  api.nuget.org, so the real ContentProcessor cannot be built or run here. Decision: port the
  compiler's validation rules faithfully into `validate.py` (see "Compiler rules" below) and run
  that. Logged as unresolved in REPORT.md; the author should run
  `dotnet run --project tools/ObserverMagazine.ContentProcessor` once before merging.
- PostgreSQL 16.15 is installed in the sandbox and was started on a private socket
  (`/tmp/pg`, port 5433). It is used for MVCC/isolation experiments and PostgreSQL-specific
  semantics. SQLite (Python `sqlite3`, library 3.53.1) is used for portable queries.
- Web access works through WebSearch/WebFetch; dated claims were checked against official docs.

## Compiler rules (source wins)

From `tools/ObserverMagazine.ContentProcessor/Books/*.cs` and `FrontMatterParser.cs`:

- Book folder: `^[a-z0-9]+(?:-[a-z0-9]+)*$`. Chapter folders and section files:
  `^(\d+)[-_]([a-z0-9]+(?:-[a-z0-9]+)*)$` (number, hyphen or underscore, slug). Names starting
  with `.` or `_` are ignored (hidden).
- `book.yml`: Title (required), Author, Summary, Date (DateTime), Updated, Tags (string[]), Image,
  Draft. Future-dated books (Date > UtcNow at build) are skipped.
- `chapter.yml`: Title (required), Summary, Draft.
- Section front matter: Title (required), Draft, Questions. Front matter must start at byte 0 with
  `---`; it ends at the first occurrence of `\n---` anywhere after it. Therefore no line inside
  the front matter may start with `---` (indented lines are fine).
- YAML: YamlDotNet with camelCase naming and IgnoreUnmatchedProperties. Unknown keys are silently
  ignored (so a typo like `corect:` would silently produce a wrong option — validate.py rejects
  unknown keys).
- Question: `id` optional (defaults q1..), must match slug regex, unique per section. `prompt`
  required. `type`: null/empty -> inferred (multiple if >1 correct); `single` errors if >1 correct;
  `multiple` always checkboxes; anything else errors. Comparison is trimmed + lower-invariant.
- Options: at least 2; at least one `correct`; option ids optional (default a, b, c...), slug regex,
  unique; `text` required. `correct` is a YAML bool.
- Prompt and explanation are rendered with Markdig (UseAdvancedExtensions => pipe tables work);
  option text is rendered inline (single paragraph unwrapped).
- Block scalars: standard YAML, supported by YamlDotNet. No existing book uses them (all use plain
  or quoted single-line scalars). Decision: use `explanation: |-` as the prompt prefers; a real
  compiler test was impossible (no SDK), so validate.py parses with PyYAML and the risk is noted in
  REPORT.md. Block scalars contain no tabs and no lines starting with `---`.
- SyntaxHighlighter languages: csharp (cs), javascript (js), typescript (ts), python, sql, bash
  (sh, shell, zsh), json, yaml (yml), html (xml), css. Intersection with the prompt's allow-list =
  csharp, python, javascript, sql, bash, json, yaml, html. This book uses only `sql` and `python`.

## Style decisions copied from existing books

- Author id: `terlingua-team` exists in `content/authors/` and is the most common id in
  existing `book.yml` files (15 plain + 3 quoted vs 5 `kushal`). Used.
- Date format: unquoted ISO date `2026-11-27` (every existing book uses this form).
- Conflict: the prompt says "double-quote every string value". `date` is a DateTime and `draft` /
  `correct` are bools, so they stay unquoted (quoting `false` would make it a string that YamlDotNet
  would still coerce, but existing books never quote them). Tags are double-quoted strings, like
  the DDIA companion.
- Existing books use a `## Sources` section at the end of the body (timeless-dotnet). Decision:
  add a final `## Sources` heading after Key takeaways, matching house style; the prompt's six
  required headings all appear in order before it. Logged as a conflict below.

## Authoring pipeline (decision)

- Sections are authored as source files in `_build/relational-sql-and-transactions/src/` with
  `correct:` and `distractors:` lists and optional executable `checks:`. `build.py` emits the
  final `.md` files with every string double-quoted, explanations as `|-` block scalars, and
  options placed by the answer-position rule. This makes the position rule mechanical and keeps
  explanations position-free (they cite option text only).
- Answer-position rule: question k (1-based) in book section s (1-based, book order): first correct
  option at index (k + s) mod n; further correct options of `multiple` questions at the next
  indexes, wrapping modulo n. Distractors fill the remaining slots in authored order.
- `check_sql.py` runs every `checks:` entry: SQLite and/or PostgreSQL queries with expected rows,
  interleaved two-session schedules on PostgreSQL (threads, so blocking is observable), and Python
  arithmetic assertions.

## Conflicts and resolutions

1. Prompt "run the repo's real build" vs no SDK/NuGet access -> ported validation (above).
2. Prompt "double-quote every string" vs YAML dates/bools -> dates and bools unquoted.
3. Prompt section structure vs house `## Sources` -> both (Sources appended last).
4. README says "explanation optional" -> prompt is stricter (required); stricter rule applied.
5. Multiple-answer wrap-around: "next valid index(es)" interpreted as wrapping modulo n.
6. Prompt asks for 3–5 sections in Chapter 7 "may have 3–4"; 3 review sections were written (30 total, within 25–32).
7. Option-length bias: the validator warns when the correct option is strictly longest in more than half a
   section's questions; after rebalancing, no section exceeds 3 of 6 and the book-wide rate is 33%.
8. Section 3 has no dated claim, so its "today" block carries no "As of November 2026" sentence (validator warning kept).
9. Engine verification: PostgreSQL 16.15 (local) and SQLite 3.53.1 executed; PostgreSQL 19, MySQL 8.4,
   SQL Server 2025, CockroachDB v26.2 and Spanner claims taken only from their official documentation.
