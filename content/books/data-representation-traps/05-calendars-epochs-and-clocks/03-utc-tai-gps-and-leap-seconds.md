---
title: "UTC, TAI, GPS Time and Leap Seconds"
questions:
  - id: "convert-utc-to-tai"
    type: "single"
    prompt: "Since 2017-01-01, TAI − UTC has been 37 seconds. What is the TAI reading at 2026-11-27T12:00:00 UTC?"
    explanation: |-
      TAI and UTC tick at the same rate (both count SI seconds); UTC is offset from TAI by an integer number of seconds that changes only when a leap second is inserted. TAI − UTC was set to 10 s in 1972 and 27 positive leap seconds have been added since, the last at the end of 2016: 10 + 27 = 37 s. TAI = UTC + 37 s = 12:00:37.

      "12:00:27" uses only the count of leap seconds and forgets the initial 10-second offset of 1972.

      "12:00:18" is GPS time (TAI − 19 s), not TAI.

      "11:59:23" applies the offset with the wrong sign; TAI is ahead of UTC.
    options:
      - text: "12:00:27"
      - text: "12:00:37"
        correct: true
      - text: "12:00:18"
      - text: "11:59:23"
  - id: "gps-minus-utc"
    type: "single"
    prompt: "GPS time started equal to UTC at its epoch, 1980-01-06T00:00:00 UTC, when TAI − UTC was 19 s, and GPS time never inserts leap seconds. What is GPS − UTC today (TAI − UTC = 37 s)?"
    explanation: |-
      Because GPS time has no leap seconds, it keeps a constant offset from TAI: at the epoch GPS = UTC = TAI − 19 s, so GPS = TAI − 19 s forever. Today GPS − UTC = (TAI − 19) − (TAI − 37) = 37 − 19 = 18 s. GPS receivers broadcast this offset so they can display UTC.

      "19" is the constant TAI − GPS offset, not GPS − UTC.

      "37" is TAI − UTC.

      "0" assumes GPS time follows UTC's leap seconds.
    options:
      - text: "19"
      - text: "37"
      - text: "18"
        correct: true
      - text: "0"
  - id: "parse-leap-second-timestamp"
    type: "single"
    prompt: "What happens when this Python line runs?\n\n```python\nfrom datetime import datetime\ndatetime.fromisoformat(\"2016-12-31T23:59:60+00:00\")\n```"
    explanation: |-
      The last leap second was inserted as 2016-12-31T23:59:60Z, a valid UTC label that RFC 3339 explicitly allows. Python's `datetime` cannot represent second 60, so parsing raises `ValueError: second must be in 0..59`. JavaScript's `Date` parses it to `NaN` (an invalid date). Systems that log or exchange UTC timestamps produced by leap-second-aware clocks can therefore crash or drop records once every few years.

      "It returns `2017-01-01T00:00:00+00:00`" describes a normalizing parser some libraries use; Python's does not normalize.

      "It returns `2016-12-31T23:59:59+00:00`" describes clamping; Python does not clamp.

      "It returns a `datetime` whose `second` is 60" would require a type that models leap seconds; `datetime` does not.
    options:
      - text: "It returns `2017-01-01T00:00:00+00:00`."
      - text: "It returns `2016-12-31T23:59:59+00:00`."
      - text: "It returns a `datetime` whose `second` is 60."
      - text: "It raises `ValueError`."
        correct: true
  - id: "leap-smear-rate"
    type: "single"
    prompt: "A provider absorbs a positive leap second with a linear smear over 24 hours: its clocks run slightly slow so that they lose exactly 1 s against an unsmeared clock in that window. By how much is the clock rate changed?"
    explanation: |-
      1 s spread over 24 h = 86,400 s: 1 / 86,400 ≈ 1.157 × 10\^−5 = 11.6 parts per million. Halfway through the smear the clock is 0.5 s behind UTC as defined with a leap second. Clients synchronized to a smeared source disagree with clients synchronized to an unsmeared source by up to 0.5 s during the window, so mixing the two in one system creates its own errors.

      "1 ppm" would take about 11.6 days to absorb one second.

      "500 ppm" is the maximum slew rate typically allowed by NTP implementations, far faster than this smear.

      "0.1 ppm" would take about 116 days.
    options:
      - text: "About 11.6 ppm"
        correct: true
      - text: "1 ppm"
      - text: "500 ppm"
      - text: "0.1 ppm"
  - id: "time-scale-facts"
    type: "multiple"
    prompt: "Which statements about TAI, UTC and GPS time are correct? Select all that apply."
    explanation: |-
      Correct:

      - TAI (International Atomic Time) is a continuous count of SI seconds with no leap seconds.
      - GPS time is also continuous: it was aligned with UTC in 1980 and has stayed a fixed 19 s behind TAI ever since.
      - UTC ticks at the same rate as TAI. Since 1972 the two differ only by an integer number of seconds, changed by leap seconds.

      Wrong:

      - "Leap seconds follow a fixed schedule of one every 18 months" is false: the IERS decides based on Earth-rotation measurements and announces each decision in its Bulletin C about six months ahead. Intervals have ranged from 6 months to 7 years.
      - "Unix time represents 23:59:60 as its own distinct value" is false: POSIX time has exactly 86,400 values per day, so the inserted second shares a value with a neighbor or is skipped.
    options:
      - text: "TAI is continuous and has no leap seconds."
        correct: true
      - text: "GPS time stays a constant 19 s behind TAI."
        correct: true
      - text: "UTC and TAI tick at the same rate."
        correct: true
      - text: "Leap seconds follow a fixed schedule of one every 18 months."
      - text: "Unix time represents 23:59:60 as its own distinct value."
  - id: "continuous-utc-decision"
    type: "single"
    prompt: "What did the 28th General Conference on Weights and Measures (CGPM) decide in October 2026 about UTC?"
    explanation: |-
      Resolution 5 of the 28th CGPM, "On the technical actions needed to ensure the continuity of UTC", decided that continuous UTC becomes effective on 20 May 2027 and that the maximum value of |UT1 − UTC| will be 3,600 s (one hour). In effect, no further leap seconds will be inserted. It implements the 2022 CGPM Resolution 4, which had decided to raise the tolerance "in, or before, 2035".

      "Abolish UTC and switch civil time to TAI" is wrong: UTC remains the reference; it simply stops jumping.

      "Insert a final negative leap second in 2027" is wrong: the decision stops discontinuities; no negative leap second has ever been inserted.

      "Replace leap seconds with a leap minute every decade" misdescribes it: there is no scheduled replacement adjustment.

      This is the one question in this section about a recent decision rather than a timeless rule. The engineering consequence is unchanged either way: code must still handle the leap seconds already in historical UTC data.
    options:
      - text: "Abolish UTC and switch civil time to TAI."
      - text: "Insert a final negative leap second in 2027."
      - text: "Continuous UTC from 20 May 2027, with |UT1 − UTC| allowed up to one hour."
        correct: true
      - text: "Replace leap seconds with a leap minute every decade."
---

## Learning objectives

- Relate UTC, TAI, GPS time and UT1 and convert between them.
- Explain how leap seconds are inserted and why POSIX time cannot represent them.
- Evaluate leap-second smearing and its side effects.
- Describe the decision to make UTC continuous and what it does not change.

## The invariant

Several time scales coexist, defined by the BIPM, the IERS and ITU-R Recommendation TF.460:

| Scale | What it counts | Leap seconds | Relation (since 2017-01-01) |
|---|---|---|---|
| TAI | SI seconds from an ensemble of atomic clocks | none | reference |
| UTC | SI seconds, kept near Earth rotation | yes (so far) | UTC = TAI − 37 s |
| GPS time | SI seconds from GPS epoch 1980-01-06 | none | GPS = TAI − 19 s = UTC + 18 s |
| UT1 | Earth's rotation angle | n/a | irregular; UT1 − UTC was kept within ±0.9 s |
| POSIX time | UTC days × 86,400 + seconds of day | not representable | |

Since 1972, UTC has run at exactly the TAI rate, offset by a whole number of seconds. When Earth's rotation lagged, the IERS scheduled a **leap second** at the end of June or December, labeled 23:59:60, to keep |UT1 − UTC| below 0.9 s. Twenty-seven positive leap seconds were inserted between 1972 and 2016; a negative one has never been needed.

Software trouble comes from the representation, not the physics:

- **POSIX time has no slot for 23:59:60.** Kernels typically repeat a second or stop the clock, so for one second timestamps are ambiguous or non-monotonic.
- **Many parsers reject second 60.** RFC 3339 allows it, but common date types cannot hold it.
- **Smearing** (spreading the second over hours by slowing clocks slightly) avoids the 60th second but makes smeared and unsmeared clocks disagree by up to half a second.

Systems that need a uniform physical timescale (navigation, telecom, finance, scientific logging) use TAI or GPS time internally and convert to UTC only for display.

## Worked example

Convert 2016-12-31T23:59:60 UTC, the last leap second, to other scales:

| Scale | Value |
|---|---|
| UTC | 2016-12-31T23:59:60 |
| TAI (UTC + 36 s before the leap, + 37 s after) | 2017-01-01T00:00:36 |
| GPS (TAI − 19 s) | 2017-01-01T00:00:17 |
| Unix time | 1483228799 or 1483228800, depending on the kernel's handling |

The TAI and GPS values are unambiguous: each second, including the leap second, has its own label. Unix time does not.

Smear arithmetic for a 24-hour linear smear: rate change 1 / 86,400 = 11.6 ppm; offset at the midpoint 0.5 s.

```python
from datetime import datetime, timezone, timedelta
utc = datetime(2026, 11, 27, 12, 0, 0, tzinfo=timezone.utc)
tai_offset, gps_offset = timedelta(seconds=37), timedelta(seconds=18)
print((utc + tai_offset).time(), (utc + gps_offset).time())  # 12:00:37 12:00:18
```

## Where this shows up today

As of November 2026, the 28th CGPM has adopted Resolution 5 (October 2026), making UTC continuous from 20 May 2027 with |UT1 − UTC| allowed to reach 3,600 s, and the IERS has announced (Bulletin C 72, July 2026) that no leap second will be inserted at the end of December 2026. TAI − UTC therefore stays at 37 s, but historical data from 1972–2016 still contains leap seconds, and code that parses or stores UTC must keep handling them.

## Pitfalls checklist

- Converting UTC to TAI or GPS with a hard-coded offset without a table of when it changed.
- Rejecting or crashing on `23:59:60` in logs, feeds or certificates.
- Mixing smeared and unsmeared time sources in one cluster.
- Computing precise elapsed times across 1972–2016 from Unix timestamps without leap-second tables.
- Assuming GPS receivers report UTC directly; they report GPS time plus a broadcast offset.

## Key takeaways

- TAI and GPS time are continuous; UTC = TAI − 37 s and GPS = UTC + 18 s since 2017.
- Leap seconds are labeled 23:59:60, which POSIX time and many parsers cannot represent.
- Smearing trades a discontinuity for a rate error of about 11.6 ppm per second per day.
- UTC becomes continuous on 20 May 2027, but past leap seconds remain in historical data.
