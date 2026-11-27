---
title: "Review: Clocks, Calendars and Zones in Incidents"
questions:
  - id: "timeouts-after-clock-step"
    type: "single"
    prompt: "Incident: \"At 03:00 every in-flight request on three hosts timed out at once.\" The hosts' NTP daemons had just stepped their clocks **forward** by 30 s. The client code:\n\n```python\ndeadline = time.time() + 5.0\nwhile pending():\n    if time.time() > deadline:\n        raise TimeoutError\n    poll()\n```\n\nWhat is the root cause, and the minimal fix?"
    explanation: |-
      `time.time()` is the wall clock (Chapter 5). A forward step of 30 s makes `time.time()` jump past every deadline computed before the step, so all waits expire at once. A backward step would do the opposite and make waits last too long. Deadlines and timeouts measure durations, so they must use a monotonic clock: `deadline = time.monotonic() + 5.0` and compare against `time.monotonic()`.

      "The 5-second timeout is too short for the backend" misses that the requests failed because of the clock jump, not because of slowness.

      "The deadline must be converted to UTC first" is wrong; converting changes nothing: `time.time()` is already UTC-based; time zones are not involved.

      "NTP must be disabled on these hosts" trades one problem for drifting clocks and broken certificate and log timestamps; the bug is using the wall clock for a duration.
    options:
      - text: "The 5-second timeout is too short for the backend."
      - text: "Wall-clock deadline; use `time.monotonic()`."
        correct: true
      - text: "The deadline must be converted to UTC first."
      - text: "NTP must be disabled on these hosts."
  - id: "billing-job-ran-twice"
    type: "single"
    prompt: "Incident: \"Some customers were charged twice on 2026-11-01.\" The billing job is scheduled daily at 01:30 in `America/New_York`, and the scheduler fires once per matching local time. What happened, and what is the minimal robust fix?"
    explanation: |-
      On 2026-11-01 New York fell back from 02:00 EDT to 01:00 EST (Chapter 6), so local 01:30 occurred twice, at 05:30Z and 06:30Z. A scheduler that fires on every matching local time ran the job twice.

      The robust fix makes the business operation idempotent per billing date: record "billing for 2026-11-01 done" and skip if already done. Moving the schedule to a time outside the transition window (or to UTC) also avoids the repeat, but idempotency protects against retries, failovers and every other source of duplicates.

      "NTP stepped the server's clock back by an hour" confuses DST with clock correction; the UTC clock did not move, the local mapping did.

      "The scheduler has a race condition under heavy load at night" ignores that the two runs were exactly one hour apart, the signature of a DST overlap.

      "2026-11-01 is a leap day, which the job mishandles" is false; leap days are February 29.
    options:
      - text: "NTP stepped the server's clock back by an hour."
      - text: "The scheduler has a race condition under heavy load at night."
      - text: "01:30 occurred twice; make billing idempotent per date."
        correct: true
      - text: "2026-11-01 is a leap day, which the job mishandles."
  - id: "fifteen-year-license-expired"
    type: "single"
    prompt: "Incident: \"Newly issued 15-year licenses show as expired.\" On 2026-11-27 the C# service runs this with default (unchecked) integer arithmetic and stores `expires` as Unix seconds:\n\n```csharp\nint now = 1_795_737_600;                 // 2026-11-27T00:00:00Z\nint expires = now + 15 * 365 * 86400;\n```\n\nWhich date does `expires` represent?"
    explanation: |-
      Arithmetic (Chapter 5): 15 × 365 × 86,400 = 473,040,000. 1,795,737,600 + 473,040,000 = 2,268,777,600, which exceeds 2\^31 − 1 = 2,147,483,647. In 32-bit arithmetic it wraps: 2,268,777,600 − 2\^32 = −2,026,189,696, which is 1905-10-17T17:31:44Z. Every expiry check sees a date in 1905, so the license is "expired".

      The intended expiry is 2041-11-23 (2026-11-27 plus 5,475 days; 15 calendar years would be 2041-11-27 because of 4 leap days, a second bug in `15 * 365 * 86400`). Fix: 64-bit storage and calendar arithmetic for "15 years".

      "2041-11-23" is the intended value, which a 32-bit column cannot hold.

      "2038-01-19" assumes the arithmetic saturates at `int.MaxValue`; C# integer arithmetic wraps unless it runs in a `checked` context (which would throw `OverflowException` instead).

      "1970-01-01" assumes overflow resets to zero.
    options:
      - text: "2041-11-23"
      - text: "2038-01-19"
      - text: "1970-01-01"
      - text: "1905-10-17"
        correct: true
  - id: "leap-day-anniversary-crash"
    type: "single"
    prompt: "Incident: \"The renewal job crashes for a handful of accounts every few years.\" It computes the next anniversary like this:\n\n```python\nnext_renewal = signup.replace(year=signup.year + 1)\n```\n\nWhat is the root cause, and the correct fix?"
    explanation: |-
      Accounts created on February 29 (for example 2024-02-29) have no anniversary in a common year: `date(2024, 2, 29).replace(year=2025)` raises `ValueError: day is out of range for month` (Chapter 5, Gregorian leap years). The crash recurs whenever such accounts come up for renewal in a common year.

      There is no universally correct "one year after February 29"; the business must choose (February 28 or March 1), and the code must implement that choice explicitly, for example by catching the invalid date or using a library function with a documented clamping policy, plus a test for February 29.

      "Add `timedelta(days=365)` instead of replacing the year" shifts every renewal by a day after each leap year and still hides the policy question.

      "Store signup dates in UTC to avoid the problem" does not help: the problem is the calendar date, not the time zone.

      "Python's `replace` has a bug in leap years" is wrong; raising for an invalid date is the documented behavior.
    options:
      - text: "Feb 29 has no anniversary in common years; set a policy."
        correct: true
      - text: "Add `timedelta(days=365)` instead of replacing the year."
      - text: "Store signup dates in UTC to avoid the problem."
      - text: "Python's `replace` has a bug in leap years."
  - id: "time-incident-remediations"
    type: "multiple"
    prompt: "A postmortem lists candidate remediations for this quarter's time-related incidents. Which ones remove a whole class of bugs? Select all that apply."
    explanation: |-
      Correct:

      - Measuring durations and timeouts with monotonic clocks removes every failure caused by wall-clock steps and leap-second handling (Chapter 5).
      - Logging and exchanging instants as RFC 3339 UTC timestamps with `Z` removes ambiguous local times in DST overlaps and makes logs sortable (Chapter 6).
      - Storing epoch times in 64-bit integers removes the 2038 overflow and its early, future-dated variants (Chapter 5).

      Wrong:

      - "Set every server's time zone to the users' local zone" reintroduces DST gaps and overlaps into logs, schedulers and arithmetic; servers should run in UTC and convert at the edges.
      - "Disable NTP so the clock never jumps" leaves clocks drifting by seconds per day, breaking cross-host ordering, certificate validity checks and lease logic; monotonic clocks already solve the jump problem for durations.
    options:
      - text: "Measure durations and timeouts with monotonic clocks."
        correct: true
      - text: "Set every server's time zone to the users' local zone."
      - text: "Disable NTP so the clock never jumps."
      - text: "Log and exchange instants as RFC 3339 UTC with `Z`."
        correct: true
      - text: "Store epoch times in 64-bit integers."
        correct: true
  - id: "interleaved-local-time-logs"
    type: "single"
    prompt: "Incident review: the log aggregator sorts lines by their timestamp string. Around the 2026-11-01 transition in New York the merged log reads:\n\n```bash\n2026-11-01 01:29:58 payment accepted id=17\n2026-11-01 01:29:59 refund issued id=17\n2026-11-01 01:30:00 payment accepted id=17\n```\n\nThe refund cannot precede the payment. What is the root cause, and the fix?"
    explanation: |-
      The services log naive local time (no offset). Between 01:00 and 01:59 on 2026-11-01, every New York local time occurs twice, first in EDT (−04:00) and then in EST (−05:00) (Chapter 6). Lines from the two passes through that hour have identical-looking timestamps and interleave when sorted as strings; the first "01:29:58" may be in the second pass, an hour after the 01:30:00 payment. The fix is to log instants unambiguously: RFC 3339 with `Z` (or at least with the offset), which also sorts correctly as text when every line uses UTC.

      "The aggregator's sort algorithm is unstable" cannot reorder lines whose keys differ; these keys differ but no longer reflect time order.

      "Clock skew between the payment and refund hosts" can reorder lines by milliseconds, not by an hour, and the lines are from one service.

      "The refund service's clock lacks leap-second smearing" is irrelevant: no leap second occurred.
    options:
      - text: "The aggregator's sort algorithm is unstable."
      - text: "Clock skew between the payment and refund hosts."
      - text: "Naive local times repeat in the overlap; log UTC."
        correct: true
      - text: "The refund service's clock lacks leap-second smearing."
---

## Learning objectives

- Diagnose time incidents by separating clock problems, calendar problems and zone problems.
- Recognize the signatures of NTP steps, DST overlaps, 32-bit overflow and leap days.
- Choose remediations that remove a class of bugs rather than one occurrence.

## The invariant

Time bugs fall into three layers, and each earlier chapter supplies the invariant for one of them:

| Layer | Question | Invariant | Chapter |
|---|---|---|---|
| Clock | How much time passed? | Wall clocks step; monotonic clocks do not | 5 |
| Representation | Can the value be stored? | 32-bit seconds end in 2038; floats quantize | 1, 2, 5 |
| Calendar | Which date is it? | Gregorian rules; February 29; months differ in length | 5 |
| Zone | Which local time is it, and which instant is a local time? | Gaps, overlaps and rule changes; offsets are per instant | 6 |

An incident report usually contains a signature that points to one layer:

| Signature | Likely layer and cause |
|---|---|
| Many timeouts at once, or negative durations | clock: wall-clock step used for a duration |
| Exactly one hour off, twice a year | zone: DST handled with offsets or naive local time |
| Duplicate or missing runs on transition Sundays | zone: scheduler gap/overlap behavior |
| Dates in 1901–1905 or values that turn negative | representation: 32-bit overflow |
| Failure only for February 29 or on day 366 | calendar: leap-year arithmetic |
| Sub-microsecond values quantized | representation: binary64 seconds since epoch |

Working an incident therefore starts with the timeline in UTC, not in anyone's local time: convert every timestamp in the report to an instant, note which host and which clock produced it, and only then look for the anomaly. A gap of exactly 3,600 s, a jump equal to a known NTP correction, or a value that crosses 2\^31 is far easier to spot on one consistent axis than in a mixture of local logs. If a timestamp cannot be converted unambiguously, because it is a naive local time inside a DST overlap, that ambiguity is itself a finding.

The durable remediations follow the same layering: monotonic clocks for durations; UTC instants in RFC 3339 for logs and interchange; local date-time plus zone ID for future civil events; 64-bit integers for epoch values; explicit policies for calendar arithmetic. Each one removes a category of incidents instead of the specific occurrence in the report.

## Worked example

Re-run the 15-year license computation with each fix applied:

| Step | Buggy | Fixed |
|---|---|---|
| "15 years" | 15 × 365 × 86,400 s | calendar addition: 2026-11-27 + 15 years = 2041-11-27 |
| Value | 2,268,777,600 | 2,269,123,200 (2041-11-27T00:00:00Z) |
| Storage | signed 32-bit: wraps to −2,026,189,696 (1905-10-17T17:31:44Z) | signed 64-bit: stored as is |

```python
from datetime import datetime, timezone
issued = datetime(2026, 11, 27, tzinfo=timezone.utc)
expires = issued.replace(year=issued.year + 15)        # safe here: issued is not Feb 29
print(int(expires.timestamp()), expires.isoformat())   # 2269123200 2041-11-27T00:00:00+00:00
wrapped = (2_268_777_600 + 2**31) % 2**32 - 2**31
print(wrapped)                                         # -2026189696
```

## Where this shows up today

As of November 2026, the leap-second era is ending (continuous UTC from 20 May 2027 under CGPM Resolution 5), but the other incident classes in this section remain: clocks are still stepped, DST transitions still happen twice a year in many countries, 32-bit time fields persist in devices and schemas, and February 29 returns in 2028.

## Pitfalls checklist

- Fixing the occurrence (a one-off data patch, a retry) instead of the layer that caused it.
- Treating a one-hour discrepancy as a clock problem rather than a zone problem.
- Running servers or log pipelines in local time.
- Using fixed-second arithmetic for calendar quantities such as years and months.
- Assuming the end of leap seconds ends time incidents.

## Key takeaways

- Classify time incidents by layer: clock, representation, calendar or zone.
- Monotonic clocks for durations; UTC RFC 3339 for instants; zone IDs for future local times.
- 64-bit epoch storage and explicit calendar policies remove whole bug classes.
- The signature in the report (all at once, one hour, 1901, February 29) usually names the layer.
