---
title: "Slow Start, AIMD and Fast Recovery"
questions:
  - id: "slow-start-rounds"
    type: "single"
    prompt: "A connection starts with an initial window of 10 segments, `ssthresh` is very large, and no loss occurs. How many round trips does slow start take to reach a congestion window of at least 300 segments?"
    explanation: |-
      In slow start (RFC 5681) the sender adds up to one SMSS to cwnd for each ACK of new data. Since a full window of segments is acknowledged per round trip, cwnd roughly doubles every RTT:

      | RTT | cwnd at start (segments) |
      |---|---|
      | 0 | 10 |
      | 1 | 20 |
      | 2 | 40 |
      | 3 | 80 |
      | 4 | 160 |
      | 5 | 320 |

      The first value ≥ 300 comes after 5 round trips (log2(300 / 10) ≈ 4.9, rounded up).

      "4" stops at 160 segments.

      "29" grows linearly by 10 segments per RTT, which is not how slow start behaves.

      "8" counts doublings from 1 segment (2^8 = 256 would still fall short anyway) instead of from the initial window of 10.

      "3" stops at 80 segments, the window at the start of the fourth round trip.
    options:
      - text: "5"
        correct: true
      - text: "4"
      - text: "29"
      - text: "8"
      - text: "3"
  - id: "ssthresh-after-dupacks"
    type: "single"
    prompt: "A Reno-style sender following RFC 5681 has 120 segments in flight when it receives the third duplicate ACK. After fast retransmit and fast recovery complete, what are `ssthresh` and `cwnd`?"
    explanation: |-
      RFC 5681 on the third duplicate ACK: set ssthresh = max(FlightSize / 2, 2 × SMSS) = max(120 / 2, 2) = 60 segments; retransmit the missing segment; set cwnd = ssthresh + 3 segments and inflate it by one segment per further duplicate ACK while recovering. When the ACK for new data arrives, cwnd *deflates* to ssthresh. The sender exits with cwnd = ssthresh = 60 and continues in congestion avoidance.

      "cwnd is 1 segment" is the reaction to a retransmission *timeout*, not to duplicate ACKs.

      "ssthresh 120" leaves the threshold unchanged; the point of the reaction is to remember half the flight as the new safe level.

      "84" applies CUBIC's multiplicative factor of 0.7 (RFC 9438); the question specifies RFC 5681 Reno behavior, which halves.
    options:
      - text: "`ssthresh` is 60 segments and `cwnd` is 1 segment"
      - text: "`ssthresh` is 120 segments and `cwnd` is 60 segments"
      - text: "Both are 84 segments"
      - text: "Both are 60 segments"
        correct: true
  - id: "rto-versus-dupacks"
    type: "multiple"
    prompt: "Which statements correctly contrast a retransmission timeout with the arrival of three duplicate ACKs under RFC 5681? Select all that apply."
    explanation: |-
      Both events set ssthresh to max(FlightSize / 2, 2 × SMSS); they differ in what they say about the path and therefore in cwnd.

      - Duplicate ACKs mean segments after the missing one are reaching the receiver, so the path still delivers: one segment was lost, not the whole flight. The sender retransmits it and keeps going with cwnd ≈ ssthresh (fast recovery).
      - A timeout means no ACKs came back for a whole RTO, so the sender knows nothing about the current state of the path. RFC 5681 sets cwnd to the *loss window* of one segment and re-runs slow start up to the new ssthresh.

      "Reset cwnd to the initial window" on duplicate ACKs throws away the information those ACKs carry.

      "A timeout halves cwnd" is the dupACK reaction misattributed to timeouts.

      "Neither changes ssthresh" is false; recording the reduced level in ssthresh is what makes slow start stop earlier next time.
    options:
      - text: "Three duplicate ACKs reset cwnd to the initial window before the retransmission is sent"
      - text: "A timeout halves cwnd and the sender carries on directly in congestion avoidance"
      - text: "After a timeout, cwnd drops to one segment and slow start begins again"
        correct: true
      - text: "Three duplicate ACKs show later segments still arrive, so sending continues at a reduced window"
        correct: true
      - text: "Neither event changes ssthresh; only cwnd is adjusted in each of the two cases"
  - id: "aimd-average-throughput"
    type: "single"
    prompt: "A long-lived Reno flow loses one packet each time its window reaches 100 segments and recovers by halving, producing a regular sawtooth. MSS is 1460 bytes and RTT is 50 ms. Approximately what average throughput does it achieve?"
    explanation: |-
      Additive increase, multiplicative decrease: the window climbs linearly from 50 to 100 segments (one segment per RTT) and halves on loss. Its average over a sawtooth is the midpoint, (50 + 100) / 2 = 75 segments.

      Throughput ≈ 75 × 1460 bytes / 0.050 s = 2,190,000 bytes/s = 17.52 Mbit/s.

      "About 23.4 Mbit/s" uses the peak window of 100 segments, as if the flow never backed off.

      "About 11.7 Mbit/s" uses the trough of 50 segments.

      "About 2.2 Mbit/s" is the correct number of *megabytes* per second (2.19 MB/s) mislabeled as megabits.
    options:
      - text: "About 23.4 Mbit/s"
      - text: "About 17.5 Mbit/s"
        correct: true
      - text: "About 11.7 Mbit/s"
      - text: "About 2.2 Mbit/s"
  - id: "short-flow-benchmark-bug"
    type: "single"
    prompt: "A team benchmarks a new 1 Gbit/s link by opening a fresh TCP connection for each 100 KB download (RTT 100 ms, initial window 10 segments of 1460 bytes, no loss). Each download takes about 400 ms, so they conclude the link delivers only about 2 Mbit/s. What is wrong with the conclusion?"
    explanation: |-
      100 KB = 100,000 / 1460 ≈ 69 segments. Slow start sends 10, then 20, then 40 segments in successive round trips: 70 segments after three RTTs. Add one RTT for the handshake and the download takes about 4 × 100 ms = 400 ms whatever the link speed. 100 KB / 0.4 s = 2 Mbit/s is a measure of round trips, not capacity; a long transfer (or many parallel ones) is needed to measure bandwidth.

      "Window scaling disabled" would cap a long flow at 65,535 / 0.1 s ≈ 5.2 Mbit/s, and a 100 KB transfer never grows its window that far anyway.

      "A 100 KB receive window" would allow 100 KB per RTT, about 8 Mbit/s, and the transfer would still need slow-start rounds to reach it.

      "Must be dropping packets" assumes transfer time is size / bandwidth; for short flows, round trips dominate.
    options:
      - text: "Window scaling must be disabled, which caps every connection at a 64 KB window and therefore at about 2 Mbit/s"
      - text: "The receive window of 100 KB limits throughput to 100 KB per RTT, which is about 2 Mbit/s on this path"
      - text: "Each download ends during slow start: a handshake RTT plus about three RTTs of window growth, so it measures latency"
        correct: true
      - text: "The link must be dropping packets, because a loss-free 1 Gbit/s path would finish 100 KB in under a millisecond"
  - id: "loss-based-vs-model-based"
    type: "single"
    prompt: "What is the essential mechanism difference between a loss-based congestion controller such as CUBIC (RFC 9438) and a model-based one such as BBR?"
    explanation: |-
      Loss-based controllers (Reno, and CUBIC per RFC 9438) treat packet loss as the congestion signal: they increase the window (CUBIC along a cubic curve of time since the last reduction) until a buffer somewhere overflows, then reduce multiplicatively (CUBIC by a factor of 0.7). They therefore tend to fill bottleneck buffers before backing off.

      BBR builds an explicit model of the path: the bottleneck bandwidth (from the delivery rate of ACKed data) and the minimum RTT. It paces sending at about the estimated bandwidth and keeps in-flight data near the estimated BDP, probing periodically for more bandwidth, so it does not need loss to find the operating point.

      "CUBIC estimates bandwidth" swaps the two designs.

      "Both react only to loss" ignores BBR's defining feature, its model.

      "Only for QUIC / only for TCP" is false: both algorithms are transport-agnostic and are used for TCP and QUIC (RFC 9438 itself says CUBIC can serve QUIC and SCTP).

      "CUBIC paces at a fixed rate; BBR halves on loss" inverts both designs: CUBIC is window-based and loss-driven, BBR is the one that paces.
    options:
      - text: "CUBIC grows its window until loss and then cuts it; BBR paces at its bandwidth and minimum-RTT estimates"
        correct: true
      - text: "CUBIC estimates bandwidth from the ACK arrival rate; BBR reacts only when packets are lost in the network"
      - text: "Both react only to loss; BBR differs by starting with a larger initial window and a faster slow start"
      - text: "CUBIC is defined only for QUIC connections, and BBR is defined only for TCP connections on Linux servers"
      - text: "CUBIC paces at a fixed rate derived from link speed; BBR halves its window on every loss event"
---
## Learning objectives

- Trace cwnd through slow start and congestion avoidance.
- Apply RFC 5681's reactions to duplicate ACKs and to timeouts.
- Estimate the average throughput of an AIMD sawtooth.
- Contrast loss-based and model-based congestion control.

## The invariant

A TCP sender keeps a **congestion window** (cwnd) and a **slow-start threshold** (ssthresh) and never has more than min(cwnd, rwnd) bytes outstanding. RFC 5681 defines four algorithms:

1. **Slow start** (cwnd < ssthresh): add up to one SMSS per ACK of new data. A whole window is ACKed each RTT, so cwnd roughly **doubles per RTT**. RFC 5681's formula gives an initial window of 3 segments for a 1460-byte SMSS; RFC 6928 (Experimental) raises it to 10, the value Linux uses.
2. **Congestion avoidance** (cwnd ≥ ssthresh): add about one SMSS **per RTT**. Together with the halving below this is AIMD, additive increase and multiplicative decrease, which converges competing flows toward fair shares.
3. **Fast retransmit**: on the third duplicate ACK, retransmit the missing segment without waiting for the timer, and set ssthresh = max(FlightSize / 2, 2 × SMSS).
4. **Fast recovery**: set cwnd = ssthresh + 3 SMSS, inflate by one SMSS per further duplicate ACK (each means a segment left the network), and deflate to ssthresh when new data is ACKed. Sending continues throughout.

A **retransmission timeout** is treated more severely: ssthresh = max(FlightSize / 2, 2 × SMSS) as before, but cwnd falls to the **loss window**, one segment, and slow start begins again. Duplicate ACKs prove the path still delivers; silence proves nothing.

For a long-lived flow that loses one packet at window W, cwnd oscillates between W/2 and W, averaging 3W/4, so average throughput ≈ 0.75 × W × MSS / RTT. Short transfers never leave slow start, so their duration is set by round trips, not bandwidth.

## Worked example

Initial window 10 segments, MSS 1460, RTT 50 ms, ssthresh initially unlimited.

| Event | cwnd (segments) | ssthresh |
|---|---|---|
| Start | 10 | ∞ |
| After 4 RTTs of slow start (10 + 20 + 40 + 80 = 150 segments sent) | 160 | ∞ |
| Third duplicate ACK with 160 in flight | 80 + 3 during recovery | 80 |
| Recovery ends | 80 | 80 |
| Each RTT afterwards | 81, 82, … | 80 |

If the loss had been detected by timeout instead, cwnd would restart at 1 and need 7 round trips (1, 2, 4, 8, 16, 32, 64, then capped at 80) to get back to ssthresh, about 350 ms of reduced sending.

On Linux, `ss -ti` shows the live values:

```bash
$ ss -ti dst 198.51.100.20
     cubic wscale:7,7 rto:252 rtt:50.3/2.1 mss:1448 cwnd:83 ssthresh:80
     bytes_sent:18874368 bytes_retrans:1448 ...
```

## Where this shows up today

As of November 2026, RFC 9438 states that CUBIC is the default congestion control in the Linux, Windows and Apple TCP stacks. BBR's current specification is draft-ietf-ccwg-bbr-06 (July 2026), which describes BBRv3 and is intended for Experimental status; it is not yet an RFC. Both are used for TCP and for QUIC, where RFC 9002 specifies a Reno-like default that implementations may replace.

## Pitfalls checklist

- Benchmarking bandwidth with short transfers that finish in slow start.
- Treating every loss the same; timeouts cost far more than duplicate-ACK recovery.
- Expecting a halved window to halve throughput; the average sits at three quarters of the peak.
- Disabling SACK or delaying ACKs so aggressively that duplicate ACKs never reach three.
- Comparing congestion controllers on one path and generalizing to all paths.

## Key takeaways

- Slow start doubles cwnd per RTT; congestion avoidance adds one segment per RTT.
- Three duplicate ACKs: halve to ssthresh and keep sending; timeout: back to one segment.
- An AIMD sawtooth averages three quarters of its peak window.
- Loss-based control fills buffers until loss; model-based control paces to estimates.

## Sources

- RFC 5681, TCP Congestion Control; RFC 6928, Increasing TCP's Initial Window.
- RFC 9438, CUBIC for Fast and Long-Distance Networks (obsoletes RFC 8312, updates RFC 5681).
- draft-ietf-ccwg-bbr-06, BBR Congestion Control (July 2026, work in progress).
- RFC 9002, QUIC Loss Detection and Congestion Control.
