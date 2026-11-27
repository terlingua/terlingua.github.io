---
title: "Offsets Versus Time Zones"
questions:
  - id: "etc-gmt-plus-five"
    type: "single"
    prompt: "What UTC offset does the IANA zone `Etc/GMT+5` have?"
    explanation: |-
      The `Etc/GMT±N` zones follow the POSIX TZ string convention, in which the number is how far the zone is *behind* UTC, so the sign is inverted relative to ISO 8601: `Etc/GMT+5` is UTC−05:00. The tz database documentation keeps these names only for compatibility and warns about the inverted sign.

      "UTC+05:00" reads the name with ISO 8601 sign conventions, which is exactly the trap.

      "It follows US Eastern time, including DST" confuses a fixed-offset zone with a region: `Etc/` zones never observe DST.

      "UTC+00:00, because it is a GMT zone" ignores the offset in the name.
    options:
      - text: "UTC+05:00"
      - text: "It follows US Eastern time, including DST"
      - text: "UTC+00:00, because it is a GMT zone"
      - text: "UTC−05:00"
        correct: true
  - id: "ambiguous-abbreviation"
    type: "single"
    prompt: "A partner API sends timestamps such as `2026-03-02 09:00 CST`. What is the problem with `CST`?"
    explanation: |-
      Time-zone abbreviations are not unique identifiers. `CST` is used for US Central Standard Time (UTC−06:00), China Standard Time (UTC+08:00) and Cuba Standard Time (UTC−05:00), among others, and abbreviations do not encode DST rules either. A parser has to guess, and guesses differ between libraries. Interchange formats should use a numeric offset (RFC 3339) and, where future local rules matter, an IANA zone ID.

      "Nothing; `CST` always means UTC−06:00" is a US-centric assumption.

      "It is fine as long as the receiver uses UTC internally" misses the point: converting to UTC requires knowing the offset first.

      "`CST` is ambiguous only during DST" is wrong: the collision between US, China and Cuba exists year-round.
    options:
      - text: "`CST` names several different offsets."
        correct: true
      - text: "Nothing; `CST` always means UTC−06:00."
      - text: "It is fine as long as the receiver uses UTC internally."
      - text: "`CST` is ambiguous only during DST."
  - id: "offset-to-utc"
    type: "single"
    prompt: "Convert `2026-07-01T12:00:00-04:00` to UTC."
    explanation: |-
      An RFC 3339 / ISO 8601 offset is local time minus UTC: local = UTC + offset. So UTC = local − offset = 12:00 − (−04:00) = 16:00. Result: `2026-07-01T16:00:00Z`.

      "2026-07-01T08:00:00Z" adds the offset instead of subtracting it.

      "2026-07-01T12:00:00Z" ignores the offset.

      "2026-07-02T16:00:00Z" gets the hour right but moves the date; no date change occurs because 16:00 is still on July 1.
    options:
      - text: "2026-07-01T08:00:00Z"
      - text: "2026-07-01T16:00:00Z"
        correct: true
      - text: "2026-07-01T12:00:00Z"
      - text: "2026-07-02T16:00:00Z"
  - id: "store-zone-not-offset"
    type: "single"
    prompt: "A calendar app stores each user's time zone as a fixed offset captured at sign-up. A New York user signed up in January, so the app stored `-05:00`. What goes wrong?"
    explanation: |-
      An offset is a property of an *instant in a zone*, not of the zone. America/New_York is UTC−05:00 in winter and UTC−04:00 during daylight saving time, and governments can change the rules. A stored `-05:00` makes every summer event display one hour off, and it cannot absorb future rule changes. The app should store the IANA zone ID `America/New_York` and compute the offset for each instant.

      "Nothing, because New York is always UTC−05:00" ignores DST.

      "Only events created in other users' zones are shown at the wrong time" is wrong: the user's own local times are off for half the year.

      "Offsets cannot represent New York because it uses `EST`" confuses abbreviations with offsets; `-05:00` is a perfectly valid offset, just not a zone.
    options:
      - text: "Nothing, because New York is always UTC−05:00."
      - text: "Only events created in other users' zones are shown at the wrong time."
      - text: "Summer times are an hour off; store `America/New_York` instead."
        correct: true
      - text: "Offsets cannot represent New York because it uses `EST`."
  - id: "kathmandu-local-time"
    type: "multiple"
    prompt: "At `2026-01-01T09:00:00Z`, which of these local times are correct? Select all that apply."
    explanation: |-
      Local time = UTC + offset, and offsets need not be whole hours.

      Correct:

      - Asia/Kathmandu is UTC+05:45: 09:00 + 5:45 = 14:45.
      - Asia/Kolkata is UTC+05:30: 09:00 + 5:30 = 14:30.
      - Pacific/Chatham is UTC+13:45 in January (its daylight time; +12:45 in standard time): 09:00 + 13:45 = 22:45 on the same date.

      Wrong:

      - "Asia/Kathmandu: 15:00" rounds the offset to whole hours.
      - "America/New_York: 14:00" adds 5 hours instead of subtracting them; New York is UTC−05:00 in January, so the local time is 04:00.
    options:
      - text: "Asia/Kathmandu: 15:00"
      - text: "America/New_York: 14:00"
      - text: "Asia/Kathmandu: 14:45"
        correct: true
      - text: "Asia/Kolkata: 14:30"
        correct: true
      - text: "Pacific/Chatham: 22:45"
        correct: true
  - id: "zone-model-facts"
    type: "single"
    prompt: "Which statement about the IANA time zone database (tzdb) model is correct?"
    explanation: |-
      A tzdb zone such as `Europe/Berlin` stands for a region whose clocks have agreed since 1970, and it records the complete history of that region's offsets and DST rules. Converting an instant to local time looks up the rule in force at that instant, which is why the same zone has different offsets at different times.

      "Each UTC offset corresponds to exactly one zone ID" is false: many zones share an offset at any moment (UTC+01:00 covers dozens) while following different DST rules.

      "Zone IDs are named after countries" is false: they are named after a representative city (`America/New_York`, `Asia/Kolkata`), and a country can have many zones.

      "A zone's rules never change once published" is false: tzdb releases update rules whenever governments change them, sometimes with only weeks of notice.
    options:
      - text: "A zone ID records a region's history of offset rules."
        correct: true
      - text: "Each UTC offset corresponds to exactly one zone ID."
      - text: "Zone IDs are named after countries."
      - text: "A zone's rules never change once published."
---

## Learning objectives

- Distinguish an offset, a time zone (IANA zone ID) and a zone abbreviation.
- Convert between local time with an offset and UTC.
- Recognize non-hour offsets and the inverted sign of `Etc/GMT±N` zones.
- Store user and event zones in a form that survives DST and rule changes.

## The invariant

Three different things are casually called "time zone":

| Concept | Example | What it determines |
|---|---|---|
| UTC offset | `-04:00`, `+05:45` | local − UTC for **one instant** |
| IANA zone ID | `America/New_York` | the rule history that yields the offset for **any instant** |
| Abbreviation | `EDT`, `CST`, `IST` | a display label; not unique, not a key |

The IANA Time Zone Database (tzdb, also called the Olson database, distributed as `tzdata`) is the shared source for zone rules in operating systems, language runtimes, browsers and databases. Each zone ID names a region whose civil clocks have agreed since 1970 and records its transitions: changes of standard offset and DST rules over time.

Offsets follow ISO 8601 and RFC 3339: local time = UTC + offset, so UTC = local − offset. Offsets are not restricted to whole hours (India +05:30, Nepal +05:45, Chatham Islands +12:45/+13:45), and in current use they span from −12:00 to +14:00.

The **POSIX TZ string** convention (`EST5EDT`, and the tzdb's `Etc/GMT+5`) uses the opposite sign: it gives how far the zone is *behind* UTC. This is the only place where "+5" means five hours behind.

Consequences:

- An offset alone cannot tell you the offset of another instant in the same place.
- A zone ID plus an instant determines the offset; a zone ID plus a local date-time almost always does (exceptions are DST gaps and overlaps, the next section).
- Abbreviations are for humans. Never parse them as keys.

## Worked example

The same instant, 2026-01-01T09:00:00Z, in several zones:

| Zone | Offset at that instant | Local time |
|---|---|---|
| America/New_York | −05:00 | 04:00 |
| Asia/Kolkata | +05:30 | 14:30 |
| Asia/Kathmandu | +05:45 | 14:45 |
| Pacific/Chatham | +13:45 (DST) | 22:45 |
| Etc/GMT+5 | −05:00 | 04:00 |

```python
from datetime import datetime, timezone
from zoneinfo import ZoneInfo

instant = datetime(2026, 1, 1, 9, 0, tzinfo=timezone.utc)
for zone in ("America/New_York", "Asia/Kathmandu", "Etc/GMT+5"):
    local = instant.astimezone(ZoneInfo(zone))
    print(zone, local.isoformat())
# America/New_York 2026-01-01T04:00:00-05:00
# Asia/Kathmandu 2026-01-01T14:45:00+05:45
# Etc/GMT+5 2026-01-01T04:00:00-05:00
```

```csharp
var ny = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
Console.WriteLine(ny.GetUtcOffset(new DateTimeOffset(2026, 7, 1, 12, 0, 0, TimeSpan.Zero))); // -04:00:00
```

## Where this shows up today

As of November 2026, the tzdb is still updated several times a year as governments change offsets and DST rules, and .NET (on all platforms since .NET 6), Python's `zoneinfo` (Python 3.9 and later), Java and browsers accept IANA zone IDs. Applications that store offsets or abbreviations instead of zone IDs still produce one-hour errors twice a year.

## Pitfalls checklist

- Storing a user's "time zone" as a fixed offset or as an abbreviation.
- Reading `Etc/GMT+5` as UTC+05:00.
- Assuming offsets are whole hours, or that they range only from −12 to +12.
- Converting with the offset sign reversed (adding instead of subtracting).
- Parsing abbreviations such as `CST` or `IST` from external input.

## Key takeaways

- An offset describes one instant; a zone ID describes a region's rule history.
- UTC = local − offset; `Etc/GMT±N` uses the inverted POSIX sign.
- Offsets can be fractional hours and span −12:00 to +14:00.
- Store IANA zone IDs; treat abbreviations as display text only.
