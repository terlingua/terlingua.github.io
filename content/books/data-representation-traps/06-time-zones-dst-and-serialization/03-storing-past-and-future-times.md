---
title: "Storing Past and Future Times"
questions:
  - id: "store-a-past-event"
    type: "single"
    prompt: "An e-commerce service records when each order was placed, for auditing and for \"orders in the last 24 hours\" queries. Which representation is correct?"
    explanation: |-
      An order placement is an event that has already happened: a single point on the global timeline. Store the **instant**, as a UTC timestamp (or a timestamp with its offset, which identifies the same instant). Duration queries ("last 24 hours") and cross-region ordering are then exact, and the local time for display can be derived with the viewer's zone.

      "The local date-time in the warehouse's zone, without an offset" loses the instant: during a fall-back overlap two different instants share the same local time, and readers must guess the zone.

      "The local date-time plus the IANA zone ID" works but is the shape for *future civil* times; for past events it adds an unnecessary conversion that can be ambiguous during overlaps.

      "Epoch milliseconds stored in a `double`" is an instant, but binary64 cannot represent every integer millisecond far from the epoch, and it mixes in the floating-point problems from Chapter 2 for no benefit; an integer type or a timestamp type is the right container.
    options:
      - text: "The local date-time in the warehouse's zone, without an offset"
      - text: "A UTC instant (or a timestamp with its offset)"
        correct: true
      - text: "The local date-time plus the IANA zone ID"
      - text: "Epoch milliseconds stored in a `double`"
  - id: "future-meeting-after-rule-change"
    type: "single"
    prompt: "A city's clocks were going to be at UTC+02:00 (summer time) next July, so an app converted a 10:00 local meeting there to `08:00Z` and stored only that instant. The government then abolishes summer time, so the city will stay on UTC+01:00, and a tzdata update ships the new rule. What local time does the app now show for the meeting?"
    explanation: |-
      The stored instant does not change: 08:00Z. With the new rule the offset in July is +01:00, so local = 08:00 + 1:00 = 09:00. The attendees still intend to meet at 10:00 local, so the app is now an hour early for everyone in that city.

      Future civil events are defined by local wall time in a place. Converting them to UTC at creation freezes the *old* rules into the data. Storing the local date-time with the IANA zone ID (and recomputing the instant when needed) keeps 10:00 at 10:00.

      "10:00, because UTC instants never need adjusting" is the misconception: the instant was correct under the old rules and wrong under the new ones.

      "11:00" applies the change with the wrong sign (local = UTC + offset, and the offset decreased).

      "08:00, because the app displays UTC" ignores that the app converts to the user's local time.
    options:
      - text: "10:00, because UTC instants never need adjusting"
      - text: "11:00"
      - text: "09:00"
        correct: true
      - text: "08:00, because the app displays UTC"
  - id: "future-civil-time-representations"
    type: "multiple"
    prompt: "Which representations keep a future in-person appointment at its intended local wall time even if the government changes DST rules before it happens? Select all that apply."
    explanation: |-
      Correct:

      - Local date-time plus IANA zone ID, with the UTC instant computed when needed: rule changes delivered through tzdata are picked up automatically.
      - Local date-time plus zone ID, with a cached UTC instant that is recomputed whenever the tzdata version changes: same truth, with a fast index for queries.

      Wrong:

      - "Only the UTC instant computed at booking time" freezes the old rules.
      - "Local date-time plus the UTC offset at booking time" also freezes the old rules, since the offset is what changes.
      - "Epoch seconds computed at booking time" is the same as storing the UTC instant.
    options:
      - text: "Only the UTC instant computed at booking time"
      - text: "Local date-time plus the UTC offset at booking time"
      - text: "Local date-time plus IANA zone ID, instant computed when needed"
        correct: true
      - text: "Local date-time plus zone ID, with a cached instant rebuilt on tzdata updates"
        correct: true
      - text: "Epoch seconds computed at booking time"
  - id: "recurring-standup-in-utc"
    type: "single"
    prompt: "A weekly 09:00 New York stand-up was created in January as \"every 7 days from `14:00Z`\". What local time does it show in July?"
    explanation: |-
      In January, New York is UTC−05:00: 14:00Z = 09:00 EST. The recurrence adds 7 × 24 h of elapsed time, so every occurrence stays at 14:00Z. In July, New York is UTC−04:00 (EDT): 14:00Z = 10:00 local. The meeting drifts an hour later for the summer.

      Recurring civil events need the recurrence rule in local time plus a zone (as iCalendar's RRULE with TZID does), so each occurrence is computed as 09:00 in America/New_York and then converted.

      "09:00" assumes the stored UTC time follows DST.

      "08:00" applies the DST shift in the wrong direction.

      "14:00" shows UTC instead of local time.
    options:
      - text: "10:00"
        correct: true
      - text: "09:00"
      - text: "08:00"
      - text: "14:00"
  - id: "missing-tzdata-in-container"
    type: "single"
    prompt: "A Python 3 service in a minimal container image raises `ZoneInfoNotFoundError: 'No time zone found with key America/New_York'`. What is the minimal correct fix?"
    explanation: |-
      Python's `zoneinfo` (standard since Python 3.9) reads compiled tz data from the system time-zone paths (such as `/usr/share/zoneinfo`) and, if none is found, from the first-party `tzdata` package on PyPI. Minimal images often omit the OS tzdata package, so no source exists. Installing the OS package (for example `tzdata` from the distribution) or adding the `tzdata` Python dependency provides the rules. Prefer a source that is updated regularly, because rule changes ship through it.

      "Set the `TZ` environment variable to `UTC`" changes the process's default zone but adds no data; looking up `America/New_York` still fails.

      "Replace the zone with a fixed `-05:00` offset" makes the error disappear and the data wrong for half of every year.

      "Upgrade to a newer Python version" does not help: CPython does not bundle the tz database in the standard library.

      This question depends on packaging practice rather than a timeless rule; the timeless point is that zone rules are data that must be deployed and kept current.
    options:
      - text: "Set the `TZ` environment variable to `UTC`."
      - text: "Install the OS `tzdata` package or the `tzdata` PyPI package."
        correct: true
      - text: "Replace the zone with a fixed `-05:00` offset."
      - text: "Upgrade to a newer Python version."
  - id: "tzdata-version-skew"
    type: "single"
    prompt: "Service A runs with a tzdata release that includes a recently announced DST change; service B runs with an older release. Both convert the same future local date-time and zone to UTC. Which statement is correct?"
    explanation: |-
      Conversions for instants after the rule change differ by the size of the change, and nothing fails: each service silently applies the rules it has. Results for past instants (before the change) agree. That is why tzdata must be deployed consistently across services, and why future civil times should travel between services as local date-time plus zone ID, so the receiver applies its own (ideally current) rules rather than trusting a precomputed instant.

      "tzdata only affects historical dates" is backwards: new releases mostly change future rules.

      "UTC conversions never depend on tzdata" ignores that the offset for a zone at a given instant comes from tzdata.

      "Service B fails loudly with an unknown-rule error" is wrong: old data is still valid data; it is just out of date.

      "Both agree as long as they use the same programming language" ignores that the data, not the language, determines the result.
    options:
      - text: "They can silently disagree for instants after the rule change."
        correct: true
      - text: "tzdata only affects historical dates."
      - text: "UTC conversions never depend on tzdata."
      - text: "Service B fails loudly with an unknown-rule error."
      - text: "Both agree as long as they use the same programming language."
---

## Learning objectives

- Choose between storing an instant and storing a local date-time with a zone.
- Explain why converting future civil times to UTC at creation is fragile.
- Represent recurring events so they keep their local wall time across DST.
- Treat tz rules as deployable data and keep services consistent.

## The invariant

Two kinds of time values look alike but have different identities:

| Kind | Examples | Identity | Store as |
|---|---|---|---|
| Instant (event that happened, or a deadline defined globally) | log entry, payment, sensor reading, token expiry | a point on the UTC timeline | UTC timestamp (or timestamp with offset) |
| Civil time (a wall-clock appointment in a place) | dentist at 10:00, store opening hours, "every Monday 09:00" | a local date-time in a zone | local date-time + IANA zone ID (+ recurrence rule) |

The mapping from civil time to instant is a function of the **tz rules in force**, and rules change: governments adopt or abolish DST, move transition dates and change standard offsets, sometimes with only weeks of notice. Therefore:

1. **Past events: store the instant.** The rules for the past are settled, and an instant answers every ordering and duration question exactly.
2. **Future civil times: store local date-time + zone ID.** Compute the instant when needed (to sort, notify, or show to someone in another zone). If you cache the instant, invalidate the cache when tzdata changes.
3. **Recurring events: store the rule in local terms** (for example iCalendar's RRULE with a TZID), not "every N × 86,400 seconds from an instant".
4. **tz rules are data.** They ship in OS packages, language runtimes, browsers and databases, each with its own update channel. Mixed versions disagree silently about the future.

## Worked example

A meeting at 10:00 local on a July date, in a city whose July offset changes from +02:00 to +01:00 after the meeting is booked:

| Stored representation | Before the rule change | After the tzdata update |
|---|---|---|
| UTC instant `08:00Z` | 10:00 local ✓ | 09:00 local ✗ |
| local `10:00` + offset `+02:00` | 10:00 local ✓ | still claims +02:00 ✗ |
| local `10:00` + zone ID | 10:00 local (08:00Z) ✓ | 10:00 local (09:00Z) ✓ |

```python
from dataclasses import dataclass
from datetime import datetime, timezone
from zoneinfo import ZoneInfo

@dataclass(frozen=True)
class Appointment:
    local: datetime     # naive local wall time
    zone: str           # IANA zone ID

    def instant(self) -> datetime:
        return self.local.replace(tzinfo=ZoneInfo(self.zone)).astimezone(timezone.utc)

appt = Appointment(datetime(2027, 7, 6, 10, 0), "America/New_York")
print(appt.instant().isoformat())   # 2027-07-06T14:00:00+00:00 under current rules
```

```sql
CREATE TABLE appointments (
    id            BIGINT PRIMARY KEY,
    local_start   TIMESTAMP WITHOUT TIME ZONE NOT NULL,  -- wall time
    zone_id       VARCHAR(64) NOT NULL,                  -- e.g. 'America/New_York'
    start_utc     TIMESTAMP WITH TIME ZONE NOT NULL,     -- derived, recomputed on tzdata change
    tzdata_version VARCHAR(16) NOT NULL
);
```

## Where this shows up today

As of November 2026, minimal container images frequently omit OS tz data, and runtimes differ in where they look for it (Python's `zoneinfo`, in Python 3.9 and later, falls back to the `tzdata` PyPI package; .NET on Linux reads the system database). Fleets built from images of different ages run different tzdata releases, so the same future appointment can resolve to different instants in different services.

## Pitfalls checklist

- Converting user-entered future appointments to UTC at creation and discarding the zone.
- Storing a fixed offset alongside local times and calling it a time zone.
- Implementing recurrence as repeated fixed durations from a UTC start.
- Shipping containers without tz data, or never updating it after build.
- Exchanging precomputed instants for future civil events between services with different tzdata.

## Key takeaways

- Instants (past events) → UTC; civil times (future appointments) → local date-time + zone ID.
- Rule changes move the instant of a future civil time; store what the user meant.
- Recurrence belongs in local terms with a zone.
- tz rules are versioned data: deploy and update them consistently.
