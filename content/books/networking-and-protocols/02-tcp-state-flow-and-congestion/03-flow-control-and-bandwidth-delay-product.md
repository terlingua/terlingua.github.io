---
title: "Flow Control, Window Scaling and the Bandwidth-Delay Product"
questions:
  - id: "window-limited-throughput"
    type: "single"
    prompt: "A connection has no window scaling, so the receiver can advertise at most 65,535 bytes. The RTT is 100 ms and the link runs at 1 Gbit/s with no loss. What is the maximum throughput?"
    explanation: |-
      A sender may have at most one window of unacknowledged data in flight, and it takes one RTT for the ACKs that open the window again to return. So throughput ≤ window / RTT.

      65,535 bytes / 0.100 s = 655,350 bytes/s = 655,350 × 8 bits/s ≈ 5.24 Mbit/s.

      "About 52 Mbit/s" uses a 10 ms RTT, or slips a factor of ten.

      "About 655 Mbit/s" confuses bytes with bits and kilo with mega: 655,350 is *bytes* per second.

      "1 Gbit/s" is the misconception the formula corrects: ACKs do reopen the window, but only one window's worth per round trip, so link speed does not matter once the window is the limit.
    options:
      - text: "About 52 Mbit/s"
      - text: "About 5.2 Mbit/s"
        correct: true
      - text: "About 655 Mbit/s"
      - text: "1 Gbit/s, because each ACK reopens the window"
  - id: "bdp-ten-gig"
    type: "single"
    prompt: "What is the bandwidth-delay product of a 10 Gbit/s path with a 30 ms round-trip time?"
    explanation: |-
      BDP = bandwidth × RTT, the amount of data that must be in flight to keep the path full.

      10 × 10^9 bits/s × 0.030 s = 3 × 10^8 bits = 3 × 10^8 / 8 bytes = 37.5 × 10^6 bytes = 37.5 MB.

      "300 MB" forgets to divide bits by 8 (it is 300 megabits).

      "3.75 MB" uses 3 ms instead of 30 ms.

      "4.7 MB" divides by 8 twice (37.5 / 8 ≈ 4.69).
    options:
      - text: "300 MB"
      - text: "3.75 MB"
      - text: "37.5 MB"
        correct: true
      - text: "4.7 MB"
  - id: "window-scale-shift"
    type: "single"
    prompt: "A receiver needs a window of at least 10,000,000 bytes to fill its path. The 16-bit window field is multiplied by 2^shift, where the shift is negotiated by the window scale option. What is the smallest shift that works?"
    explanation: |-
      RFC 7323: the advertised window is the 16-bit field shifted left by the negotiated scale, so the largest window is 65,535 × 2^shift.

      - Shift 7: 65,535 × 128 = 8,388,480 bytes, too small.
      - Shift 8: 65,535 × 256 = 16,776,960 bytes, enough.

      So 8 is the smallest that works (10,000,000 / 65,535 ≈ 152.6, and the next power of two is 256).

      "7" falls short by about 1.6 MB.

      "14" works but is not the smallest; it is RFC 7323's maximum, giving about 1 GiB.

      "16" is not allowed: RFC 7323 caps the shift at 14 so the window stays below half the 32-bit sequence space.

      The option is only exchanged in SYN segments. If a middlebox strips it from the handshake, the connection is stuck at 65,535 bytes for its whole life.
    options:
      - text: "7"
      - text: "14"
      - text: "16"
      - text: "8"
        correct: true
  - id: "min-cwnd-rwnd"
    type: "single"
    prompt: "A sender's congestion window is 40 segments of 1460 bytes. The receiver advertises a window of 32,768 bytes. How many bytes may the sender have unacknowledged?"
    explanation: |-
      Two separate limits apply. Flow control (the receive window, RFC 9293) protects the *receiver's* buffer. Congestion control (cwnd, RFC 5681) protects the *network*. The sender must respect both, so the usable window is min(cwnd, rwnd).

      cwnd = 40 × 1460 = 58,400 bytes; rwnd = 32,768 bytes; min = 32,768.

      "58,400" ignores the receiver and would overrun its buffer.

      "91,168" adds the two limits, as if they were independent budgets.

      "65,535" is the largest unscaled window field, which has nothing to do with this receiver's advertisement.
    options:
      - text: "32,768"
        correct: true
      - text: "58,400"
      - text: "91,168"
      - text: "65,535"
  - id: "zero-window-next"
    type: "single"
    prompt: "A receiving application stops reading its socket. Its buffer fills and it advertises a window of 0. What does the sender's TCP do next?"
    explanation: |-
      A zero window is a flow-control statement: "my buffer is full". RFC 9293 requires the sender to stop sending new data and to run a *persist timer*, periodically sending a probe (a segment with one byte or a zero-length segment, depending on the implementation) to elicit an ACK carrying the current window. Probing protects against a lost window update: if the receiver's "window open" ACK disappeared, both sides would otherwise wait forever. The connection is not aborted however long the window stays closed.

      "Treats it as congestion" confuses flow control with congestion control; a full receiver buffer says nothing about the network.

      "Keeps sending" violates the window; well-behaved senders never transmit beyond it.

      "Aborts after one timeout" is wrong: a closed window can persist indefinitely while the peer keeps answering probes.
    options:
      - text: "Treats the zero window as congestion, halves cwnd, and keeps sending at the lower rate"
      - text: "Stops sending new data and sends periodic window probes until the window reopens"
        correct: true
      - text: "Keeps sending; the receiver drops the excess and the sender retransmits on timeout"
      - text: "Aborts the connection after one retransmission timeout with no window update"
  - id: "slow-transfer-causes"
    type: "multiple"
    prompt: "A bulk transfer across a 200 ms RTT path levels off at about 10 Mbit/s on a 1 Gbit/s link, with no packet loss. Which explanations are consistent with that number? Select all that apply."
    explanation: |-
      With no loss, the steady rate is window / RTT. Working backwards: 10 Mbit/s × 0.2 s = 2,000,000 bits = 250,000 bytes in flight. Anything that pins the effective window near 250 KB explains it: a small receive buffer, or an application that drains the buffer slowly so the advertised window stays small. (The BDP of the path is 1 Gbit/s × 0.2 s / 8 = 25 MB, a hundred times larger.)

      "MTU 1500 rather than 9000" changes header overhead by a few percent, not by a factor of 100.

      "Nagle's algorithm" only delays *small* segments while data is unacknowledged; a bulk transfer sends full-sized segments, which Nagle never holds.

      "cwnd far larger than the BDP" would not *lower* throughput; the sender uses min(cwnd, rwnd), and a large cwnd simply stops being the limit.
    options:
      - text: "The receiving application reads slowly, so the advertised window stays near 250 KB"
        correct: true
      - text: "The path MTU is 1500 rather than 9000, so per-packet overhead caps the rate"
      - text: "Nagle's algorithm is holding back full-sized segments until earlier data is ACKed"
      - text: "The congestion window is far larger than the BDP, so the sender is pacing itself"
      - text: "The receiver's socket buffer, and therefore its advertised window, is about 250 KB"
        correct: true
---
## Learning objectives

- Distinguish flow control (receive window) from congestion control (congestion window).
- Compute throughput from window and RTT, and the bandwidth-delay product of a path.
- Choose a window scale shift and explain why it must be negotiated in the SYN.
- Predict sender behavior when the receive window closes.

## The invariant

Every TCP segment carries a **receive window**: how many bytes beyond the acknowledged point the receiver is willing to buffer (RFC 9293). This is **flow control**, and it protects the receiver. The sender also keeps a **congestion window** (cwnd, RFC 5681), which protects the network. The sender may have at most min(cwnd, rwnd) bytes outstanding.

Because the window can only reopen when ACKs return, at most one window of data moves per round trip:

**throughput ≤ window / RTT**

Turned around, keeping a path busy requires a window at least as large as its **bandwidth-delay product**:

**BDP = bandwidth × RTT**

The window field is 16 bits, so without help the window tops out at 65,535 bytes, which caps a 100 ms path at about 5.2 Mbit/s whatever the link speed. RFC 7323's **window scale** option fixes this: each side states a shift count (0 to 14) in its SYN, and later window fields are multiplied by 2^shift, up to 65,535 × 2^14 = 1,073,725,440 bytes. The shift is only exchanged during the handshake, so a middlebox that strips the option leaves the connection at 64 KB for its whole life.

When the receiving application does not read, its buffer fills and the advertised window shrinks to **zero**. The sender stops sending new data and runs a **persist timer**, probing until a non-zero window is advertised. Probing matters because window updates are ordinary ACKs that may be lost.

## Worked example

A 1 Gbit/s path with 80 ms RTT:

| Quantity | Computation | Result |
|---|---|---|
| BDP | 10^9 bit/s × 0.080 s ÷ 8 | 10,000,000 bytes (10 MB) |
| Throughput with a 65,535-byte window | 65,535 ÷ 0.080 × 8 | 6.55 Mbit/s |
| Smallest scale shift | 65,535 × 2^7 = 8.39 MB < 10 MB ≤ 65,535 × 2^8 | shift 8 (16.78 MB) |

The receive window comes from the receiver's socket buffer, so the buffer has to be at least the BDP too. A quick check on Linux:

```bash
$ ss -tin dst 203.0.113.50
ESTAB 0 0 10.0.0.4:43122 203.0.113.50:443
     cubic wscale:7,7 rto:284 rtt:80.2/1.1 mss:1448 cwnd:5800 rcv_space:65483
     snd_wnd:8388480 ...
```

`wscale:7,7` shows the negotiated shifts. `snd_wnd:8388480` is 65,535 × 2^7: the peer's advertised window is the cap, about 8.4 MB, so this flow cannot exceed 8,388,480 × 8 / 0.0802 ≈ 837 Mbit/s, below the link rate.

## Where this shows up today

As of November 2026, RFC 7323 (2014), which replaced RFC 1323, is still the specification for window scaling. Modern kernels auto-tune socket buffers, growing them as a connection's measured RTT and throughput demand, but every auto-tuner has a maximum, and applications that set a fixed buffer size switch auto-tuning off for that socket. Long, fast paths between cloud regions are where a forgotten fixed buffer or a configured maximum below the BDP shows up as a flat, loss-free throughput ceiling.

## Pitfalls checklist

- Setting a small fixed socket buffer and capping every long-distance flow.
- Middleboxes stripping the window scale option from SYNs.
- Mixing bits and bytes in BDP arithmetic.
- Treating a zero window as a network problem; it is a slow reader.
- Expecting a faster link to help when the window, not the link, is the limit.

## Key takeaways

- Usable window = min(cwnd, rwnd); throughput ≤ window / RTT.
- A path needs a window of at least bandwidth × RTT to run at full speed.
- Window scaling (shift ≤ 14) is negotiated once, in the SYNs.
- A zero window stops the sender, which probes until the receiver reads.

## Sources

- RFC 9293, Transmission Control Protocol (window, persist timer, zero-window probing).
- RFC 7323, TCP Extensions for High Performance (window scale).
- RFC 5681, TCP Congestion Control (cwnd and the sender's window).
