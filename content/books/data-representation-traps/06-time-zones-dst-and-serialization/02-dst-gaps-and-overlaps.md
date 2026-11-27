---
title: "DST Gaps and Overlaps"
questions:
  - id: "nonexistent-local-time"
    type: "single"
    prompt: "On Sunday 2026-03-08, clocks in `America/New_York` jump from 02:00 EST to 03:00 EDT. What is true of the local time 2026-03-08 02:30?"
    explanation: |-
      At the transition the offset changes from −05:00 to −04:00, and local clocks go from 01:59:59 straight to 03:00:00. Local times from 02:00 up to (not including) 03:00 never occur that day: 02:30 is a *nonexistent* (gap) time. No instant maps to it, so any conversion must apply a policy: reject it, shift it forward by the gap (to 03:30 EDT, which is what Python's `zoneinfo` effectively does when you convert it to UTC and back), or use the offset before the gap.

      "It occurs twice" describes the autumn *overlap*, not the spring gap.

      "It occurs once, at 07:30Z" assumes standard time continues; that instant is 03:30 local.

      "It occurs once, at 06:30Z" assumes daylight time applied already; that instant is 01:30 local EST.
    options:
      - text: "It never occurs; local clocks skip from 02:00 to 03:00."
        correct: true
      - text: "It occurs twice."
      - text: "It occurs once, at 07:30Z."
      - text: "It occurs once, at 06:30Z."
  - id: "ambiguous-local-time"
    type: "single"
    prompt: "On Sunday 2026-11-01, `America/New_York` falls back from 02:00 EDT to 01:00 EST. Which UTC instants does the local time 01:30 that day correspond to?"
    explanation: |-
      Local times from 01:00 to 01:59:59 happen twice: first in EDT (−04:00), then again in EST (−05:00). UTC = local − offset:

      - First 01:30 (EDT): 01:30 + 4 h = 05:30Z.
      - Second 01:30 (EST): 01:30 + 5 h = 06:30Z.

      So 01:30 is *ambiguous* and maps to both 05:30Z and 06:30Z, one hour apart. A local timestamp without an offset or a disambiguation flag cannot say which.

      "06:30Z only" assumes standard time applies to the whole day.

      "05:30Z only" assumes daylight time applies to the whole day.

      "04:30Z and 05:30Z" shifts both answers an hour early, as if the offsets were −03:00 and −04:00.
    options:
      - text: "06:30Z only"
      - text: "05:30Z and 06:30Z"
        correct: true
      - text: "05:30Z only"
      - text: "04:30Z and 05:30Z"
  - id: "length-of-spring-day"
    type: "single"
    prompt: "How much real time elapses between local midnight 2026-03-08 and local midnight 2026-03-09 in `America/New_York`?"
    explanation: |-
      Midnight on March 8 is still EST: 00:00 −05:00 = 05:00Z. Midnight on March 9 is EDT: 00:00 −04:00 = 04:00Z on March 9. Elapsed: from 05:00Z March 8 to 04:00Z March 9 = 23 hours. The calendar day is 23 hours long because one local hour was skipped.

      "24 hours" assumes every calendar day has the same length; that is true only in fixed-offset zones.

      "25 hours" is the length of the autumn transition day (2026-11-01), when an hour repeats.

      "22 hours" double-counts the skipped hour.
    options:
      - text: "24 hours"
      - text: "25 hours"
      - text: "23 hours"
        correct: true
      - text: "22 hours"
  - id: "aware-datetime-arithmetic"
    type: "single"
    prompt: "What does this Python code print?\n\n```python\nfrom datetime import datetime, timedelta, timezone\nfrom zoneinfo import ZoneInfo\nny = ZoneInfo(\"America/New_York\")\nstart = datetime(2026, 3, 7, 12, 0, tzinfo=ny)\na = start + timedelta(hours=24)\nb = (start.astimezone(timezone.utc) + timedelta(hours=24)).astimezone(ny)\nprint(a.hour, b.hour)\n```"
    explanation: |-
      Python performs arithmetic on aware datetimes that share a `tzinfo` in *wall-clock* terms: `start + timedelta(hours=24)` moves the local fields to 2026-03-08 12:00 and then computes the offset for that local time (−04:00). So `a` is noon local, even though only 23 real hours have passed. Doing the arithmetic in UTC gives a true 24-hour duration: 17:00Z on March 7 + 24 h = 17:00Z on March 8 = 13:00 EDT. Output: `12 13`.

      This is language-specific behavior. Java's `ZonedDateTime.plusHours(24)` and .NET's `DateTimeOffset.AddHours(24)` add elapsed time instead, so libraries disagree about what "add 24 hours" means.

      "12 12" assumes both computations are wall-clock arithmetic.

      "13 13" assumes Python's `+` adds elapsed time.

      "13 12" reverses the two behaviors.
    options:
      - text: "12 12"
      - text: "13 13"
      - text: "13 12"
      - text: "12 13"
        correct: true
  - id: "daily-job-around-dst"
    type: "multiple"
    prompt: "A job is scheduled daily at a fixed local time in `America/New_York`. Which statements are correct? Select all that apply."
    explanation: |-
      Correct:

      - A job at 02:30 has no matching local time on the spring-forward day; depending on the scheduler it is skipped or shifted to 03:30. Either way, behavior must be defined.
      - A job at 01:30 matches twice on the fall-back day (05:30Z and 06:30Z) unless the scheduler deliberately runs it once.
      - Scheduling the job in UTC avoids gaps and repeats, but the local run time then shifts by an hour across DST changes (for example 07:00Z is 02:00 EST in winter and 03:00 EDT in summer).

      Wrong:

      - "A job at 02:30 runs twice on the November transition day" is false: the repeated hour is 01:00–01:59; at 02:00 EDT the clock goes back to 01:00, so 02:30 occurs once.
      - "Only jobs scheduled at exactly 02:00 are affected by DST" is false: every time in the skipped or repeated hour is affected.
    options:
      - text: "A job at 02:30 has no matching time on the March transition day."
        correct: true
      - text: "A job at 02:30 runs twice on the November transition day."
      - text: "Only jobs scheduled at exactly 02:00 are affected by DST."
      - text: "A job at 01:30 can match twice on the November transition day."
        correct: true
      - text: "Scheduling in UTC avoids gaps and repeats but shifts the local run time."
        correct: true
  - id: "python-fold-attribute"
    type: "single"
    prompt: "What does this Python code print?\n\n```python\nfrom datetime import datetime\nfrom zoneinfo import ZoneInfo\nny = ZoneInfo(\"America/New_York\")\nfirst = datetime(2026, 11, 1, 1, 30, tzinfo=ny)\nsecond = first.replace(fold=1)\nprint(first.strftime(\"%z\"), second.strftime(\"%z\"))\n```"
    explanation: |-
      PEP 495 added the `fold` attribute to disambiguate repeated local times. `fold=0` (the default) selects the earlier occurrence, `fold=1` the later one. At 01:30 on 2026-11-01 the earlier occurrence is EDT (−04:00) and the later is EST (−05:00). Output: `-0400 -0500`.

      "-0500 -0500" assumes `fold` is ignored and standard time is used.

      "-0400 -0400" assumes `fold` is ignored and daylight time is used.

      "-0500 -0400" reverses the meaning of `fold`: 0 is the first (earlier) occurrence.
    options:
      - text: "-0500 -0500"
      - text: "-0400 -0500"
        correct: true
      - text: "-0400 -0400"
      - text: "-0500 -0400"
---

## Learning objectives

- Identify nonexistent (gap) and ambiguous (overlap) local times at DST transitions.
- Convert ambiguous local times to both possible UTC instants.
- Compute the real length of a transition day and distinguish calendar arithmetic from elapsed-time arithmetic.
- Use disambiguation features such as Python's `fold` and define scheduler behavior around transitions.

## The invariant

A time zone maps **instants to local times** as a function: each instant has exactly one local time. The reverse mapping, local time to instant, is not a function. When a zone's offset changes by Δ:

| Transition | Local times affected | Count of instants per local time |
|---|---|---|
| Offset increases (spring forward) | a gap of length Δ | 0: nonexistent |
| Offset decreases (fall back) | an overlap of length Δ | 2: ambiguous |
| Elsewhere | all other times | 1 |

For America/New_York (US rules since 2007: DST from the second Sunday in March to the first Sunday in November, switching at 02:00 local):

- 2026-03-08: 02:00 EST → 03:00 EDT. Local 02:00–02:59 do not exist. The day lasts 23 hours.
- 2026-11-01: 02:00 EDT → 01:00 EST. Local 01:00–01:59 occur twice. The day lasts 25 hours.

Consequences that hold in every language:

- **Local date-times are not instants.** Converting one needs a zone and, around transitions, a rule for gaps and overlaps. APIs expose this explicitly: Python's `fold`, .NET's `TimeZoneInfo.IsInvalidTime`/`IsAmbiguousTime`, Java's `ZonedDateTime.ofStrict`/`withEarlierOffsetAtOverlap`.
- **"Add one day" and "add 24 hours" differ** on transition days. Calendar arithmetic (same local time tomorrow) and duration arithmetic (exactly 86,400 s later) are both legitimate; the requirement must say which.
- **Schedules defined in local time must define gap and overlap behavior.** A daily 02:30 job has no slot one day a year; a 01:30 job has two.

Not every zone shifts by one hour (Lord Howe Island shifts by 30 minutes), and transitions can occur at other local times; the rules come from the tz database, not from assumptions.

## Worked example

| Local time (America/New_York) | Status | Offset(s) | UTC |
|---|---|---|---|
| 2026-03-08 01:30 | normal | −05:00 | 06:30Z |
| 2026-03-08 02:30 | gap | none | none (often normalized to 03:30 EDT = 07:30Z) |
| 2026-03-08 03:30 | normal | −04:00 | 07:30Z |
| 2026-11-01 00:30 | normal | −04:00 | 04:30Z |
| 2026-11-01 01:30 | overlap | −04:00 or −05:00 | 05:30Z or 06:30Z |
| 2026-11-01 02:30 | normal | −05:00 | 07:30Z |

```csharp
var ny = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
Console.WriteLine(ny.IsInvalidTime(new DateTime(2026, 3, 8, 2, 30, 0)));     // True
Console.WriteLine(ny.IsAmbiguousTime(new DateTime(2026, 11, 1, 1, 30, 0)));  // True
Console.WriteLine(ny.GetAmbiguousTimeOffsets(new DateTime(2026, 11, 1, 1, 30, 0)).Length); // 2
```

## Where this shows up today

As of November 2026, the United States and the European Union both still change clocks twice a year (the US on 2026-03-08 and 2026-11-01, the EU on 2026-03-29 and 2026-10-25); proposals in both to stop the changes have stalled. Billing cycles, cron jobs and "same time tomorrow" reminders keep producing skipped and doubled runs on those dates.

## Pitfalls checklist

- Converting local times to UTC without handling nonexistent or ambiguous inputs.
- Assuming every day is 24 hours long, or every hour occurs exactly once.
- Using "add 24 hours" where the business means "same local time tomorrow", or the reverse.
- Scheduling recurring jobs in the 01:00–03:00 local window without defining DST behavior.
- Assuming DST shifts are always one hour at 02:00.

## Key takeaways

- Spring forward creates a gap (local times that never happen); fall back creates an overlap (local times that happen twice).
- Local time plus zone is not always one instant; APIs need a disambiguation rule.
- Transition days last 23 or 25 hours; calendar arithmetic and elapsed-time arithmetic diverge.
- Define scheduler behavior for gaps and overlaps explicitly.
