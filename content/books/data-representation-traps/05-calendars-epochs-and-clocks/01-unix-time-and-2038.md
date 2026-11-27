---
title: "Unix Time and the Year 2038"
questions:
  - id: "max-signed-32-bit-time"
    type: "single"
    prompt: "A signed 32-bit `time_t` holds at most 2\\^31 − 1 = 2,147,483,647 seconds after the Unix epoch. Which instant is that?"
    explanation: |-
      2,147,483,647 s ÷ 86,400 s/day = 24,855 days remainder 11,647 s. 24,855 days after 1970-01-01 is 2038-01-19 (68 years with 17 leap days: 68 × 365 + 17 = 24,837 days to 2038-01-01, plus 18 more days). 11,647 s = 3 h 14 min 7 s. Result: 2038-01-19T03:14:07Z.

      "2038-01-19T03:14:08Z" is the first second that does **not** fit; adding 1 overflows.

      "2106-02-07T06:28:15Z" is the limit of an *unsigned* 32-bit counter (2\^32 − 1).

      "2037-12-31T23:59:59Z" assumes the limit falls on a year boundary; it is set by the binary range, not the calendar.
    options:
      - text: "2038-01-19T03:14:08Z"
      - text: "2106-02-07T06:28:15Z"
      - text: "2037-12-31T23:59:59Z"
      - text: "2038-01-19T03:14:07Z"
        correct: true
  - id: "int32-truncation-wraps"
    type: "single"
    prompt: "JavaScript `Date` values are binary64 milliseconds, so they have no 2038 limit. What does this code print?\n\n```javascript\nconst ms = Date.UTC(2038, 0, 19, 3, 14, 8);   // 2038-01-19T03:14:08Z\nconst seconds = (ms / 1000) | 0;              // \"truncate to an integer\"\nconsole.log(new Date(seconds * 1000).toISOString());\n```"
    explanation: |-
      `ms / 1000` is 2,147,483,648 = 2\^31, one past the signed 32-bit maximum. ECMAScript's bitwise operators first convert their operands with ToInt32, which wraps modulo 2\^32 into −2\^31 … 2\^31 − 1, so `2147483648 | 0` is −2,147,483,648. That is 24,856 days before the epoch plus 74,752 s (−24,856 × 86,400 + 74,752 = −2,147,483,648): 1901-12-13 at 20:45:52. Output: `1901-12-13T20:45:52.000Z`.

      The `| 0` idiom silently reintroduces the 32-bit limit into a language whose dates do not have it. `Math.trunc(ms / 1000)` keeps the full range.

      "2038-01-19T03:14:08.000Z" assumes `| 0` only drops the fraction; it also wraps anything outside the int32 range.

      "1970-01-01T00:00:00.000Z" assumes overflow resets to zero; two's-complement wraparound jumps to the most negative value.

      "It throws a RangeError" assumes JavaScript reports the overflow; ToInt32 never throws, and the resulting date is valid.
    options:
      - text: "`1901-12-13T20:45:52.000Z`"
        correct: true
      - text: "`2038-01-19T03:14:08.000Z`"
      - text: "`1970-01-01T00:00:00.000Z`"
      - text: "It throws a `RangeError`."
  - id: "unix-time-of-a-date"
    type: "single"
    prompt: "What is the Unix time (seconds) of 2026-11-27T00:00:00Z?"
    explanation: |-
      POSIX defines Unix time as days × 86,400 + seconds of the day, ignoring leap seconds. Days from 1970-01-01 to 2026-11-27: 56 years (1970–2025) = 56 × 365 + 14 leap days (1972, 1976, …, 2024) = 20,454 days; plus Jan–Oct 2026 = 31+28+31+30+31+30+31+31+30+31 = 304 days; plus 26 days in November. Total 20,454 + 304 + 26 = 20,784 days. 20,784 × 86,400 = 1,795,737,600.

      "1795755600" is 5 hours later: midnight in New York (UTC−05:00 in November) instead of UTC. Unix time is always relative to UTC.

      "1795737627" adds the 27 leap seconds inserted since 1972; POSIX time does not count them.

      "1764201600" is 2025-11-27, one year (365 days) too early.
    options:
      - text: "1795755600"
      - text: "1795737600"
        correct: true
      - text: "1795737627"
      - text: "1764201600"
  - id: "millisecond-uptime-overflow"
    type: "single"
    prompt: "Firmware stores milliseconds since boot in a signed 32-bit integer. After how much uptime does it overflow?"
    explanation: |-
      2\^31 − 1 ms = 2,147,483,647 ms = 2,147,483.647 s. Divide by 86,400 s/day: ≈ 24.86 days. Devices that are "rebooted monthly" never see the bug; one that runs for 25 days does. Unsigned 32-bit milliseconds last twice as long, ≈ 49.7 days.

      "About 49.7 days" is the unsigned limit (2\^32 ms).

      "About 68 years" is the limit for *seconds* in a signed 32-bit integer.

      "About 2.5 days" is off by a factor of ten, as if the unit were 10 µs instead of 1 ms.
    options:
      - text: "About 49.7 days"
      - text: "About 68 years"
      - text: "About 24.9 days"
        correct: true
      - text: "About 2.5 days"
  - id: "mysql-timestamp-2038"
    type: "single"
    prompt: "In MySQL 8.4, which column type lets a table store a subscription expiry date of `2040-06-30 12:00:00` without hitting the 2038 limit?"
    explanation: |-
      The MySQL 8.4 Reference Manual gives `TIMESTAMP` the range '1970-01-01 00:00:01' UTC to '2038-01-19 03:14:07' UTC, the signed 32-bit epoch-seconds range. `DATETIME` stores calendar fields and supports '1000-01-01 00:00:00' to '9999-12-31 23:59:59', so it can hold 2040. (`DATETIME` does not convert time zones; the application must store a defined zone, typically UTC.)

      "`TIMESTAMP(6)`" adds fractional seconds but keeps the same 2038 upper bound.

      "`TIMESTAMP` with the session time zone set to UTC" changes conversion, not range.

      "`INT` holding epoch seconds" is a signed 32-bit column (max 2,147,483,647), the same limit in another form; `BIGINT` would work.

      This is the one question in this section that depends on a vendor's current documentation; the underlying limit is the 32-bit arithmetic above.
    options:
      - text: "`TIMESTAMP(6)`"
      - text: "`TIMESTAMP` with the session time zone set to UTC"
      - text: "`INT` holding epoch seconds"
      - text: "`DATETIME`"
        correct: true
  - id: "posix-time-properties"
    type: "multiple"
    prompt: "Which statements about POSIX (Unix) time are correct? Select all that apply."
    explanation: |-
      POSIX defines "seconds since the Epoch" by a formula over UTC calendar fields: tm_sec + tm_min × 60 + tm_hour × 3600 + tm_yday × 86400 + (year-based day count) × 86400. Every day therefore contributes exactly 86,400.

      Correct:

      - Every day is exactly 86,400 Unix seconds long.
      - Across a leap second, the difference of two Unix timestamps is one less than the SI seconds that elapsed, because the inserted second has no Unix time of its own.
      - A Unix timestamp identifies the same instant everywhere; it has no time zone. Zones only matter when converting it to a local date and time.

      Wrong:

      - "Unix time counts every SI second since 1970, including leap seconds" is the TAI-like interpretation; POSIX time is UTC-based and skips them.
      - "Negative Unix times only occur on 32-bit systems" confuses wraparound with representation: any instant before 1970 has a negative Unix time on every system.
    options:
      - text: "Every day is exactly 86,400 Unix seconds long."
        correct: true
      - text: "Across a leap second, Unix time advances one second less than real elapsed time."
        correct: true
      - text: "Unix time counts every SI second since 1970, including leap seconds."
      - text: "Negative Unix times only occur on 32-bit systems."
      - text: "A Unix timestamp names the same instant in every time zone."
        correct: true
---

## Learning objectives

- Convert between Unix timestamps and UTC calendar dates using the POSIX formula.
- Compute the limits of signed and unsigned 32-bit second and millisecond counters.
- Predict what a 32-bit `time_t` does when it overflows.
- Identify where 32-bit time still lurks in schemas, protocols and devices.

## The invariant

POSIX ("Seconds Since the Epoch", in the Base Definitions of IEEE Std 1003.1) defines Unix time as a count derived from UTC calendar fields, with the Epoch at 1970-01-01T00:00:00Z and **every day exactly 86,400 seconds**. Leap seconds are not counted; Unix time is a deterministic encoding of UTC date and time-of-day, not a count of SI seconds.

Its limits follow from the integer used to store it:

| Representation | Range | Overflows at |
|---|---|---|
| signed 32-bit seconds | −2\^31 … 2\^31 − 1 | 2038-01-19T03:14:08Z (wraps to 1901-12-13T20:45:52Z) |
| unsigned 32-bit seconds | 0 … 2\^32 − 1 | 2106-02-07T06:28:16Z (last valid 06:28:15) |
| signed 64-bit seconds | ±9.2 × 10\^18 | about 292 billion years away |
| signed 32-bit milliseconds | ±2\^31 ms | after ≈ 24.86 days |
| unsigned 32-bit milliseconds | 0 … 2\^32 − 1 ms | after ≈ 49.71 days |
| signed 64-bit milliseconds (JavaScript-style epoch ms) | far beyond any calendar | — |

Two's-complement overflow does not stop the clock; it teleports it to 1901. Code that compares timestamps, computes expiries ("now + 30 days") or sorts events then fails silently long before 2038, as soon as any computed future instant crosses the boundary.

The fix is always the same: a wider integer (64-bit `time_t`, `long` milliseconds, `BIGINT` columns) or a calendar type with a larger range. The hard part is finding every place the narrow type survives: on-disk formats, network protocols, database columns, file-system timestamps and embedded firmware.

## Worked example

Convert 2,147,483,647 to a date:

| Step | Arithmetic |
|---|---|
| Days and remainder | 2,147,483,647 = 24,855 × 86,400 + 11,647 |
| Time of day | 11,647 s = 3 × 3600 + 14 × 60 + 7 → 03:14:07 |
| Years from 1970 | 1970–2037 is 68 years with 17 leap days: 68 × 365 + 17 = 24,837 days |
| Remaining days | 24,855 − 24,837 = 18 → January 19 (day 0 = January 1) |
| Result | 2038-01-19T03:14:07Z |

```python
from datetime import datetime, timezone
for t in (2**31 - 1, -2**31, 2**32 - 1, 1_795_737_600):
    print(t, datetime.fromtimestamp(t, timezone.utc).isoformat())
# 2147483647 2038-01-19T03:14:07+00:00
# -2147483648 1901-12-13T20:45:52+00:00
# 4294967295 2106-02-07T06:28:15+00:00
# 1795737600 2026-11-27T00:00:00+00:00
```

```csharp
int t = int.MaxValue;
t = unchecked(t + 1);
Console.WriteLine(DateTimeOffset.FromUnixTimeSeconds(t).ToString("u")); // 1901-12-13 20:45:52Z
```

## Where this shows up today

As of November 2026, 64-bit platforms use 64-bit `time_t`, and glibc (since 2.34) lets 32-bit Linux programs opt into a 64-bit `time_t` with `_TIME_BITS=64`, but 32-bit microcontrollers, older embedded Linux images, binary file formats and database columns such as MySQL 8.4's `TIMESTAMP` still carry the 2038 limit. Systems that compute dates years ahead, such as certificates, mortgages and subscriptions, hit it first.

## Pitfalls checklist

- Storing epoch seconds in a signed 32-bit column, field or struct member.
- Keeping uptime or tick counters in 32-bit milliseconds on long-running devices.
- Assuming the 2038 problem starts in 2038; future-dated computations hit it today.
- Adding leap seconds to Unix-time arithmetic, or expecting Unix time to count them.
- Converting a Unix timestamp to local time without stating the zone.

## Key takeaways

- Unix time = days × 86,400 + seconds of day, UTC-based, leap seconds ignored.
- Signed 32-bit seconds end at 2038-01-19T03:14:07Z and wrap to 1901-12-13T20:45:52Z.
- 32-bit millisecond counters overflow after about 24.9 days (signed) or 49.7 days (unsigned).
- The fix is wider types everywhere the value travels: memory, disk, wire and database.
