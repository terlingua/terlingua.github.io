---
title: "ISO 8601, RFC 3339 and Database Types"
questions:
  - id: "valid-rfc-3339"
    type: "multiple"
    prompt: "Which strings are valid RFC 3339 `date-time` values? Select all that apply."
    explanation: |-
      RFC 3339 is a strict profile of ISO 8601 for internet protocols: full date, `T`, full time with optional fractional seconds, and a **mandatory** offset that is either `Z` or `±hh:mm`. The RFC's grammar notes that `T` and `Z` may also be written in lowercase.

      Correct:

      - `2026-11-27T10:00:00Z`: UTC with `Z`.
      - `2026-11-27T10:00:00.123+05:45`: fractional seconds and a non-hour offset are both allowed.
      - `2026-11-27t10:00:00z`: lowercase `t` and `z` are permitted.

      Wrong:

      - `2026-11-27T10:00:00` has no offset. ISO 8601 allows it (as an unqualified local time), but RFC 3339 does not, because its whole purpose is an unambiguous instant.
      - `2026-W48-5` is an ISO 8601 week date; RFC 3339 supports only calendar dates.
    options:
      - text: "`2026-11-27T10:00:00`"
      - text: "`2026-11-27T10:00:00Z`"
        correct: true
      - text: "`2026-11-27T10:00:00.123+05:45`"
        correct: true
      - text: "`2026-11-27t10:00:00z`"
        correct: true
      - text: "`2026-W48-5`"
  - id: "minus-zero-offset"
    type: "single"
    prompt: "In RFC 3339, what does the offset `-00:00` mean, as in `2026-11-27T15:00:00-00:00`?"
    explanation: |-
      RFC 3339 defines `-00:00` as the "unknown local offset" convention: the time is correct as UTC, but the local offset of the place where it was generated is unknown. `Z` and `+00:00` assert that UTC is the preferred reference or the local offset really is zero; `-00:00` explicitly says "not known". The instant is the same in all three cases (15:00Z).

      "It is invalid, because zero has no sign" is wrong; RFC 3339 gives the negative form a specific meaning.

      "It means UTC−24:00, which wraps to the previous day" invents arithmetic that does not exist; the offset is zero.

      "The local time is 15:00 somewhere; the instant is unknown" reverses the meaning: the instant is known; only the local offset is not.
    options:
      - text: "It is invalid, because zero has no sign."
      - text: "It means UTC−24:00, which wraps to the previous day."
      - text: "The local time is 15:00 somewhere; the instant is unknown."
      - text: "The instant is 15:00Z, but the local offset is unknown."
        correct: true
  - id: "javascript-date-only-parsing"
    type: "single"
    prompt: "In a Node.js process whose local zone is `America/New_York`, what does this print?\n\n```javascript\nconsole.log(new Date(\"2026-11-27\").toISOString(),\n            new Date(\"2026-11-27T00:00\").toISOString());\n```"
    explanation: |-
      ECMAScript's Date Time String Format rule: when the offset is absent, **date-only** forms are interpreted as UTC and **date-time** forms as local time.

      - `"2026-11-27"` → 2026-11-27T00:00:00Z → `2026-11-27T00:00:00.000Z`.
      - `"2026-11-27T00:00"` → midnight local (EST, −05:00) → 05:00Z → `2026-11-27T05:00:00.000Z`.

      Output: `2026-11-27T00:00:00.000Z 2026-11-27T05:00:00.000Z`. In zones west of UTC, the date-only form displayed in local time shows the *previous* day, a classic off-by-one-day bug in birthday and due-date fields.

      "Both `2026-11-27T05:00:00.000Z`" assumes both forms are local.

      "Both `2026-11-27T00:00:00.000Z`" assumes both forms are UTC, as ISO 8601-minded readers often expect.

      "`…T05:00:00.000Z` then `…T00:00:00.000Z`" swaps the two rules.
    options:
      - text: "`…T00:00:00.000Z` then `…T05:00:00.000Z`"
        correct: true
      - text: "Both `2026-11-27T05:00:00.000Z`"
      - text: "Both `2026-11-27T00:00:00.000Z`"
      - text: "`…T05:00:00.000Z` then `…T00:00:00.000Z`"
  - id: "naive-versus-aware-comparison"
    type: "single"
    prompt: "What happens when this Python code runs?\n\n```python\nfrom datetime import datetime, timezone\nnaive = datetime(2026, 11, 27, 10, 0)\naware = datetime(2026, 11, 27, 10, 0, tzinfo=timezone.utc)\nprint(naive < aware)\n```"
    explanation: |-
      A naive `datetime` has no offset, so it does not identify an instant; an aware one does. Python refuses to guess and raises `TypeError: can't compare offset-naive and offset-aware datetimes`. (Equality comparison between them returns `False` instead of raising.) The fix is to make every timestamp aware at the boundary where it enters the program, for example by parsing RFC 3339 strings with `datetime.fromisoformat`, which accepts `Z` since Python 3.11.

      "It prints `False`, because they are equal" assumes the naive value is UTC.

      "It prints `True`, assuming local time west of UTC" assumes Python applies the system zone; it does not.

      "It prints `False` after a `DeprecationWarning`" invents a lenient mode that does not exist for ordering comparisons.
    options:
      - text: "It prints `False`, because they are equal."
      - text: "It raises `TypeError`."
        correct: true
      - text: "It prints `True`, assuming local time west of UTC."
      - text: "It prints `False` after a `DeprecationWarning`."
  - id: "postgres-timestamptz-semantics"
    type: "single"
    prompt: "PostgreSQL: a `timestamptz` column receives `'2026-11-27 10:00:00-05:00'`. Later a session with `TimeZone = 'UTC'` selects it. What comes back, and what was stored?"
    explanation: |-
      PostgreSQL's `timestamp with time zone` converts the input to an instant (stored internally as UTC) and **does not store the offset or zone** that came with it. On output it converts the instant to the session's `TimeZone`. 10:00 at −05:00 is 15:00Z, so a UTC session sees `2026-11-27 15:00:00+00`.

      Vendor-specific notes: PostgreSQL's `timestamp without time zone` silently ignores an offset in its input, and SQL Server's `datetimeoffset` is the type that *does* store the offset.

      "`2026-11-27 10:00:00-05`, because the original offset is stored" describes `datetimeoffset`-style storage, not `timestamptz`.

      "`2026-11-27 10:00:00+00`, because the offset is dropped" describes `timestamp without time zone` behavior, then mislabels it as UTC.

      "The zone name `America/New_York` is stored and returned" is wrong: an input offset is not a zone, and `timestamptz` stores neither.
    options:
      - text: "`2026-11-27 10:00:00-05`, because the original offset is stored"
      - text: "`2026-11-27 10:00:00+00`, because the offset is dropped"
      - text: "`2026-11-27 15:00:00+00`; only the instant was stored"
        correct: true
      - text: "The zone name `America/New_York` is stored and returned"
  - id: "iso-week-year"
    type: "single"
    prompt: "2027-01-01 is a Friday. What is its ISO 8601 week date?"
    explanation: |-
      ISO 8601 weeks start on Monday, and week 1 of a week-based year is the week containing the year's first Thursday (equivalently, the week containing January 4). 2027-01-04 is a Monday, so ISO week 2027-W01 starts on 2027-01-04. The days 2026-12-28 (Monday) through 2027-01-03 (Sunday) belong to the last week of week-year 2026, which is week 53 because 2026 has 53 ISO weeks (it starts on a Thursday). Friday is weekday 5. Result: `2026-W53-5`.

      "`2027-W01-5`" assumes the week-based year equals the calendar year; it does not around New Year.

      "`2027-W00-5`" uses a week-0 convention from some non-ISO week numbering (for example `strftime`'s `%U`/`%W`), not ISO 8601.

      "`2026-W52-5`" assumes every year has 52 weeks; years that start on a Thursday (and leap years that start on a Wednesday) have 53.

      In code this is the classic `%Y` versus `%G` (and `yyyy` versus `YYYY` in Java `DateTimeFormatter` patterns) bug: formatting a calendar date with the week-based year prints 2026 for January 1, 2027.
    options:
      - text: "`2027-W01-5`"
      - text: "`2027-W00-5`"
      - text: "`2026-W52-5`"
      - text: "`2026-W53-5`"
        correct: true
---

## Learning objectives

- Distinguish ISO 8601's many forms from the RFC 3339 profile used on the wire.
- Explain the meaning of `Z`, `+00:00` and `-00:00`.
- Predict how runtimes parse timestamps without offsets.
- Map naive and aware values onto database types correctly.

## The invariant

**ISO 8601** is a family of notations: calendar dates (`2026-11-27`), ordinal dates (`2026-331`), week dates (`2026-W48-5`), basic format without separators (`20261127T100000Z`), reduced precision, durations and intervals, and times **with or without** an offset. A time without an offset is a local time of unspecified place.

**RFC 3339** is a profile of ISO 8601 for internet protocols. It requires a full calendar date, a full time and an offset (`Z` or `±hh:mm`); it permits a fractional second, a leap second (`:60`), and lowercase `t`/`z`; and it defines `-00:00` as "UTC is known, local offset unknown". JSON Schema's `date-time` format, OpenAPI and most web APIs refer to it.

A timestamp in a program or database is one of two kinds, and every type system draws this line somewhere:

| Kind | Meaning | Examples |
|---|---|---|
| Aware / instant | identifies a point on the UTC timeline | RFC 3339 string, Python aware `datetime`, .NET `DateTimeOffset`, Java `Instant`/`OffsetDateTime`, PostgreSQL `timestamptz`, SQL Server `datetimeoffset` |
| Naive / local | wall-clock fields without an offset | ISO 8601 without offset, Python naive `datetime`, .NET `DateTime` with `Kind = Unspecified`, Java `LocalDateTime`, SQL `TIMESTAMP WITHOUT TIME ZONE` |

Rules that follow:

1. **Serialize instants as RFC 3339 with an explicit offset** (normally `Z`). Never emit a naive timestamp across a process boundary unless the contract defines its zone.
2. **Parse at the boundary into aware values.** Runtimes fill in missing offsets differently: ECMAScript treats a date-only string as UTC but a date-time without offset as local; Python keeps it naive and refuses to compare it with aware values.
3. **Know what your database stores.** PostgreSQL `timestamptz` stores only the instant (not the input offset or a zone); SQL Server `datetimeoffset` stores the offset; MySQL `TIMESTAMP` converts from the session zone to UTC and back, while `DATETIME` stores fields unchanged. To keep a zone, store the zone ID in its own column.
4. **Week-based years differ from calendar years** for a few days around New Year; use the week-year field only together with week numbers.

## Worked example

The same instant in several notations:

| Notation | Value | Standard |
|---|---|---|
| RFC 3339, UTC | `2026-11-27T15:00:00Z` | RFC 3339 / ISO 8601 |
| RFC 3339, New York offset | `2026-11-27T10:00:00-05:00` | RFC 3339 / ISO 8601 |
| Unknown local offset | `2026-11-27T15:00:00-00:00` | RFC 3339 |
| ISO basic format | `20261127T150000Z` | ISO 8601 only |
| ISO week date | `2026-W48-5T15:00:00Z` | ISO 8601 only |
| ISO ordinal date | `2026-331T15:00:00Z` | ISO 8601 only |

```python
from datetime import datetime, timezone
ts = datetime.fromisoformat("2026-11-27T10:00:00-05:00")
print(ts.astimezone(timezone.utc).isoformat().replace("+00:00", "Z"))  # 2026-11-27T15:00:00Z
print(tuple(datetime(2027, 1, 1).isocalendar()))                        # (2026, 53, 5)
```

```csharp
var dto = DateTimeOffset.Parse("2026-11-27T10:00:00-05:00", CultureInfo.InvariantCulture);
Console.WriteLine(dto.UtcDateTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture)); // 2026-11-27T15:00:00Z
```

## Where this shows up today

As of November 2026, JavaScript's Temporal API, which separates instants (`Temporal.Instant`), zoned date-times and plain local date-times, reached Stage 4 at TC39 in March 2026; .NET has `DateTimeOffset` and, since .NET 8, `TimeProvider` for testable clocks; Java has had `java.time` since Java 8. All of them encode the aware-versus-naive distinction in the type system.

## Pitfalls checklist

- Emitting timestamps without an offset in APIs, logs or files.
- Parsing date-only strings in JavaScript and displaying them in local time (off by one day west of UTC).
- Mixing naive and aware values, or treating naive values as UTC by assumption.
- Expecting PostgreSQL `timestamptz` to remember the original offset or zone.
- Formatting dates with the ISO week-based year (`%G`, `YYYY`) instead of the calendar year.

## Key takeaways

- ISO 8601 is a family; RFC 3339 is the strict, offset-required profile for the wire.
- `Z`/`+00:00` mean UTC; `-00:00` means UTC known, local offset unknown.
- Aware (instant) and naive (local) values are different types; convert at the boundary.
- Databases differ: `timestamptz` stores an instant, `datetimeoffset` stores an offset, neither stores a zone ID.
