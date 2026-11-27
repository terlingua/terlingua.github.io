---
title: "Wall Clocks Versus Monotonic Clocks"
questions:
  - id: "negative-elapsed-time"
    type: "single"
    prompt: "An operation takes 0.5 s of real time. While it runs, the NTP daemon **steps** the system clock back by 2 s. What does `elapsed` hold?\n\n```python\nimport time\nstart = time.time()\ndo_work()            # 0.5 s of real time\nelapsed = time.time() - start\n```"
    explanation: |-
      `time.time()` reads the wall clock (`CLOCK_REALTIME` on Linux), which follows every adjustment made to the system time. The end reading is start + 0.5 s − 2 s, so `elapsed` ≈ 0.5 − 2 = −1.5 s. Code that uses this value as a timeout, a rate denominator or a histogram bucket misbehaves: negative durations, divide-by-near-zero rates, or timeouts that never fire.

      "About 0.5 s" is what `time.monotonic()` would report; a monotonic clock is never stepped.

      "About 2.5 s" applies the step in the wrong direction (a forward step of 2 s would give that).

      "Exactly 0, because Python detects the step" is wrong: nothing detects or compensates for wall-clock steps.
    options:
      - text: "About 0.5 s"
      - text: "About 2.5 s"
      - text: "About −1.5 s"
        correct: true
      - text: "Exactly 0, because Python detects the step"
  - id: "monotonic-time-sources"
    type: "multiple"
    prompt: "Which of these are monotonic time sources suitable for measuring elapsed time? Select all that apply."
    explanation: |-
      A monotonic clock never goes backward and is not affected by wall-clock adjustments; its absolute value has no calendar meaning.

      Correct:

      - Python `time.monotonic()` (and `time.perf_counter()`).
      - .NET `Stopwatch` (backed by `Stopwatch.GetTimestamp()`, a high-resolution monotonic counter).
      - JavaScript `performance.now()`, which measures from the page or process time origin and is monotonic.

      Wrong:

      - JavaScript `Date.now()` returns wall-clock epoch milliseconds and moves when the system clock is changed.
      - .NET `DateTime.UtcNow` is wall-clock time too; UTC removes time-zone effects, not clock adjustments.
    options:
      - text: "Python `time.monotonic()`"
        correct: true
      - text: "JavaScript `Date.now()`"
      - text: ".NET `DateTime.UtcNow`"
      - text: ".NET `System.Diagnostics.Stopwatch`"
        correct: true
      - text: "JavaScript `performance.now()`"
        correct: true
  - id: "slew-duration"
    type: "single"
    prompt: "A clock is 1 s ahead. The time daemon corrects it by slewing at the maximum rate of 500 ppm (it runs the clock 0.05% slower) instead of stepping. How long does the correction take?"
    explanation: |-
      At 500 ppm the clock loses 500 × 10\^−6 s per real second = 0.0005 s/s. Removing 1 s takes 1 / 0.0005 = 2,000 s ≈ 33 minutes. During that time the clock stays monotonic and every second is 0.05% short, which is harmless for almost every application.

      "500 s" divides by the ppm number without converting it to a fraction (1 / 0.002).

      "About 1 s" confuses slewing with stepping.

      "86,400 s" is the time a 1-second correction takes at about 11.6 ppm, the rate of a 24-hour leap smear.
    options:
      - text: "2,000 s"
        correct: true
      - text: "500 s"
      - text: "About 1 s"
      - text: "86,400 s"
  - id: "why-slew-small-offsets"
    type: "single"
    prompt: "Why do time daemons such as ntpd and chrony prefer to slew small offsets rather than step the clock?"
    explanation: |-
      Slewing (adjusting the clock's rate slightly) keeps wall-clock time continuous and never makes it go backward, so timestamps stay ordered and naive elapsed-time calculations stay positive. Stepping is reserved for large offsets (for example at boot) where slewing would take too long.

      "Slewing ends closer to true time than stepping does" is not the reason; both end at the same corrected time, and slewing is slower to converge.

      "POSIX forbids stepping the system clock while running" is false: `clock_settime` and `settimeofday` exist precisely to step it.

      "A slewed correction is remembered across later reboots" confuses the adjustment with the hardware clock; neither method changes what the RTC holds after a reboot by itself.
    options:
      - text: "Slewing ends closer to true time than stepping does."
      - text: "It keeps wall-clock time continuous and never moving backward."
        correct: true
      - text: "POSIX forbids stepping the system clock while running."
      - text: "A slewed correction is remembered across later reboots."
  - id: "skewed-event-ordering"
    type: "single"
    prompt: "Server A's clock is 50 ms ahead of server B's. A writes event X stamped `10:00:00.040` (A's clock) and sends a message; 10 ms later B handles it and writes event Y stamped `10:00:00.000` (B's clock). An audit job sorts events by timestamp. What does it show, and what fixes it?"
    explanation: |-
      Trace, taking B's clock as true time: X happens at 09:59:59.990, but A reads 50 ms ahead and stamps it `10:00:00.040`. Y happens 10 ms later, at 10:00:00.000, and B stamps it `10:00:00.000`. Sorting by timestamp puts Y (…00.000) before X (…00.040), although X caused Y. Wall clocks on different machines are only approximately synchronized, and here the skew (50 ms) exceeds the causal gap (10 ms).

      The fix is to order causally related events with logical information: Lamport or vector clocks, hybrid logical clocks (physical time plus a logical counter that never goes backward on message receipt), or explicit sequence numbers from a single authority.

      "X before Y; NTP-synced clocks always preserve causality" ignores skew.

      "Y before X; fix it by switching both servers to UTC" misdiagnoses: the timestamps are already in one time scale; the problem is clock skew, not time zones.

      "Y before X; fix it by using monotonic clocks on both servers" fails because monotonic clocks on different machines have unrelated origins and cannot be compared at all.
    options:
      - text: "X before Y; NTP-synced clocks always preserve causality."
      - text: "Y before X; fix it by switching both servers to UTC."
      - text: "Y before X; fix it with logical or hybrid logical clocks."
        correct: true
      - text: "Y before X; fix it by using monotonic clocks on both servers."
  - id: "persisted-monotonic-value"
    type: "single"
    prompt: "A lease service stores `time.monotonic() + 30` in a shared database as the lease expiry, and other hosts compare it with their own `time.monotonic()`. What is wrong?"
    explanation: |-
      Monotonic clocks have an unspecified reference point, typically boot time, chosen independently on each machine and reset on reboot. A value from one host is not comparable with a reading on another host, or with the same host after a reboot. One host may see the lease as expired for days, another as valid for years.

      For cross-host expiries, store a wall-clock instant (UTC) and allow for skew with a safety margin, or keep lease timing on the lease holder only and renew well before the deadline.

      "Nothing, because monotonic time is the same on every host" is the misconception.

      "Only that 30 should be 30,000, because the unit is milliseconds" is wrong: Python's `time.monotonic()` returns seconds as a float.

      "Monotonic clocks overflow after 24.9 days" confuses this API with 32-bit millisecond tick counters.
    options:
      - text: "Nothing, because monotonic time is the same on every host."
      - text: "Only that 30 should be 30,000, because the unit is milliseconds."
      - text: "Monotonic clocks overflow after 24.9 days."
      - text: "Its origin differs per host and per boot."
        correct: true
---

## Learning objectives

- Distinguish wall-clock time from monotonic time and choose the right one.
- Explain how NTP corrections (steps and slews) affect timestamps and durations.
- Compute slew durations from offset and rate.
- Order events across machines without trusting wall-clock timestamps.

## The invariant

An operating system provides at least two clocks with different contracts:

| | Wall clock (real time) | Monotonic clock |
|---|---|---|
| Meaning | current date and time (UTC) | time since an arbitrary origin |
| Can jump backward | yes (manual change, NTP step, VM restore) | no |
| Comparable across machines | approximately, within the sync error | no |
| Survives reboot | yes (RTC + sync) | no (origin resets) |
| Use for | timestamps, calendars, expiries shared between hosts | durations, timeouts, rate limits, latency |
| Examples | `CLOCK_REALTIME`, `time.time()`, `DateTime.UtcNow`, `Date.now()` | `CLOCK_MONOTONIC`, `time.monotonic()`, `Stopwatch`, `performance.now()` |

POSIX specifies `CLOCK_REALTIME` and `CLOCK_MONOTONIC` for `clock_gettime`; every mainstream runtime exposes both. The rule is short: **measure durations with a monotonic clock; record instants with a wall clock.**

Wall clocks are disciplined by time daemons using NTP or PTP. Corrections come in two forms:

- **Step**: set the clock to the new value instantly. Fast, but time can move backward or skip forward.
- **Slew**: change the clock's rate slightly until the offset is gone. Continuous and monotonic, but slow: at the common 500 ppm limit, correcting 1 s takes 2,000 s.

Across machines, wall clocks agree only to within their synchronization error, from microseconds (PTP on good hardware) to many milliseconds (NTP over the internet), and errors grow when synchronization fails. Any design that infers ordering or causality from timestamps of different machines must either bound that uncertainty explicitly or use logical clocks (Lamport, vector, hybrid logical clocks).

## Worked example

Measure the same 0.5 s operation while the clock is stepped back 2 s:

| Clock | Start | End | Result |
|---|---|---|---|
| wall (`time.time()`) | 1795737600.000 | 1795737598.500 | −1.5 s |
| monotonic (`time.monotonic()`) | 1234.000 | 1234.500 | 0.5 s |

```python
import time
t0 = time.monotonic()
time.sleep(0.5)
print(round(time.monotonic() - t0, 1))   # 0.5, whatever the wall clock does meanwhile
```

```csharp
var sw = System.Diagnostics.Stopwatch.StartNew();
await Task.Delay(500);
Console.WriteLine(sw.Elapsed.TotalMilliseconds >= 500);  // True
var expiresAtUtc = DateTimeOffset.UtcNow.AddSeconds(30); // an instant to share with other hosts
```

Slew arithmetic: offset / rate = 1 s / (500 × 10\^−6) = 2,000 s.

## Where this shows up today

As of November 2026, distributed databases commonly pair physical clocks with logical counters (hybrid logical clocks) or expose explicit uncertainty bounds in the style of Google Spanner's TrueTime, rather than trusting raw wall-clock order. Public NTP services differ in whether they smear leap seconds, so fleets that mix sources can disagree by up to a second around leap events.

## Pitfalls checklist

- Computing timeouts, retry delays or latencies from `time.time()`, `Date.now()` or `DateTime.UtcNow`.
- Persisting or sending monotonic readings to other processes or hosts.
- Sorting events from different machines by wall-clock timestamp and calling it causal order.
- Assuming NTP keeps clocks within a few milliseconds without measuring it.
- Using `DateTime.Now` (local time) for anything but display.

## Key takeaways

- Wall clocks tell the date and can jump; monotonic clocks measure intervals and cannot.
- NTP steps break naive elapsed-time math; slews are continuous but slow (2,000 s per second at 500 ppm).
- Monotonic values are meaningless outside the process and boot that produced them.
- Cross-machine ordering needs logical or hybrid clocks, or explicit uncertainty bounds.
