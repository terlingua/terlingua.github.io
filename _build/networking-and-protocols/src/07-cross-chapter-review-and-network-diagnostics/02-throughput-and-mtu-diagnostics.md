---
title: "Throughput Caps and MTU Black Holes"
body_checks:
  - "assert round(2 * 1024 * 1024 * 8 / 0.06 / 1e6) == 280 and 1e9 * 0.06 / 8 == 7_500_000"
  - "assert round(1460 * 8 / 0.06 * math.sqrt(1.5) / math.sqrt(1e-5) / 1e6, 1) == 75.4"
  - "assert 1500 - 20 - 8 - 32 == 1440 and 1440 - 40 == 1400"
questions:
  - id: "fixed-buffer-cap"
    type: "single"
    prompt: "A replication job copies data between two regions with an RTT of 120 ms over a 10 Gbit/s link with no loss. The receiver's socket buffer is fixed at 4 MiB (4,194,304 bytes). What throughput does the job reach, and how large a window would fill the link?"
    correct:
      - "About 280 Mbit/s; about 150 MB"
    distractors:
      - "About 35 Mbit/s; about 18.75 MB"
      - "About 280 Mbit/s; about 1.2 GB"
      - "10 Gbit/s; the buffer size does not matter without loss"
      - "About 33.5 Mbit/s; about 150 MB"
    explanation: |-
      The receive window cannot exceed the socket buffer, and throughput ≤ window / RTT.

      - Cap: 4,194,304 bytes × 8 / 0.120 s ≈ 279.6 Mbit/s.
      - Needed window = BDP = 10 × 10^9 bit/s × 0.120 s / 8 = 150,000,000 bytes ≈ 150 MB.

      "About 35 Mbit/s; about 18.75 MB" divides both results by 8 a second time.

      "About 1.2 GB" forgets to convert the BDP from bits to bytes.

      "10 Gbit/s" is the misconception that loss-free paths run at link speed; the window is the limit, loss or not.

      "About 33.5 Mbit/s" divides the window by one second instead of by the 0.12-second RTT.
    checks:
      - "assert round(4194304 * 8 / 0.12 / 1e6, 1) == 279.6 and 10e9 * 0.12 / 8 == 150e6 and 10e9 * 0.12 == 1.2e9"
  - id: "loss-limited-throughput"
    type: "single"
    prompt: "A long Reno-style TCP flow with MSS 1,460 bytes runs over a path with an RTT of 100 ms and a random loss rate of 0.01% (one packet in 10,000). Using the steady-state estimate throughput ≈ (MSS / RTT) × 1.22 / √p, about how fast can it go?"
    correct:
      - "About 14 Mbit/s"
    distractors:
      - "About 1.4 Mbit/s"
      - "About 143 Mbit/s"
      - "About 1.8 Mbit/s"
    explanation: |-
      The Mathis et al. (1997) estimate follows from the AIMD sawtooth: with loss probability p, the window peaks around √(8 / 3p) segments, and the average rate works out to (MSS / RTT) × √(3/2) / √p, with √(3/2) ≈ 1.22.

      - MSS / RTT = 1,460 × 8 bits / 0.1 s = 116,800 bit/s.
      - √p = √0.0001 = 0.01, so 1.22 / 0.01 = 122.
      - 116,800 × 122 ≈ 14.2 Mbit/s (14.3 with the exact constant √1.5 ≈ 1.2247), about 14 Mbit/s.

      "About 1.4 Mbit/s" uses p = 1% (√p = 0.1).

      "About 143 Mbit/s" uses p = 0.0001% (√p = 0.001).

      "About 1.8 Mbit/s" is the right answer in megabytes per second, mislabeled as megabits.

      The formula shows why tiny loss rates cripple long paths: throughput falls with RTT and with the square root of loss, whatever the link capacity.
    checks:
      - "assert round(1460 * 8 / 0.1 * 1.22 / 0.01 / 1e6) == 14"
      - "base = 1460 * 8 / 0.1; c = math.sqrt(1.5); assert round(base * c / 0.01 / 1e6, 1) == 14.3 and round(base * c / 0.1 / 1e6, 1) == 1.4 and round(base * c / 0.001 / 1e6) == 143 and round(base * c / 0.01 / 8 / 1e6, 1) == 1.8"
  - id: "flat-5-mbit-checks"
    type: "multiple"
    prompt: "A bulk transfer on a 1 Gbit/s path with a 100 ms RTT and no loss levels off at almost exactly 5.2 Mbit/s. Which checks are most likely to find the cause? Select all that apply."
    correct:
      - "Whether the window scale option appears in both the SYN and the SYN-ACK"
      - "Whether the receiver's advertised window stays near 65,535 bytes in a capture"
    distractors:
      - "Whether the path MTU is smaller than 1,500 bytes somewhere along the route"
      - "Which congestion control algorithm the sender's kernel is configured to use by default"
      - "Whether the DNS record's TTL is too short and forces repeated lookups"
    explanation: |-
      65,535 bytes / 0.1 s × 8 = 5.24 Mbit/s. A loss-free flow that sits at exactly that rate is limited by an unscaled 16-bit receive window. RFC 7323 window scaling is negotiated only in the SYN and SYN-ACK; if either side (or a middlebox that strips options) leaves it out, the window can never exceed 65,535 bytes. A capture shows both facts directly.

      "A smaller path MTU" would lower efficiency by a few percent or cause stalls (black holes), not a precise ceiling at 64 KB per RTT.

      "Congestion control" matters with loss; with no loss and a full receive window, cwnd is not the binding limit.

      "DNS TTL" affects connection setup, not the rate of an established transfer.
    checks:
      - "assert round(65535 / 0.1 * 8 / 1e6, 2) == 5.24"
  - id: "tunnel-clamp-value"
    type: "single"
    prompt: "A VPN tunnel over IPv4 adds 60 bytes per packet (20 outer IPv4, 8 UDP, 32 tunnel header) on a path whose MTU is 1,500. Firewalls on the path drop ICMP. What MSS should the tunnel endpoints clamp inner IPv4 TCP connections to, and what happens without the clamp?"
    correct:
      - "1,400; without it, small exchanges work but full-size segments vanish"
    distractors:
      - "1,440; without it, the tunnel fragments large packets and only throughput drops slightly"
      - "1,460; the tunnel's overhead is absorbed by the outer packet, so no clamp is needed"
      - "1,380; without it, every connection fails during the TCP handshake itself"
    explanation: |-
      Tunnel MTU = 1,500 − 60 = 1,440 bytes for inner packets. Inner IPv4 TCP MSS = 1,440 − 20 − 20 = 1,400.

      Endpoints on ordinary 1,500-byte LANs otherwise negotiate MSS 1,460 and send 1,500-byte packets with DF set. They do not fit into the tunnel; the tunnel endpoint drops them and its ICMP "fragmentation needed" (or the remote PTB) never reaches the sender, because the firewalls drop ICMP. Small packets (handshakes, short requests) fit, so the failure shows up only when data gets large: the classic black hole.

      "1,440" is the tunnel MTU, not the MSS; it forgets the inner IPv4 and TCP headers.

      "1,460" ignores the encapsulation overhead entirely.

      "1,380" is the clamp for inner *IPv6* TCP (1,440 − 60); and the handshake itself uses small segments, so it succeeds.
    checks:
      - "mtu = 1500 - 60; assert mtu == 1440 and mtu - 40 == 1400 and mtu - 60 == 1380"
  - id: "slow-start-to-bdp"
    type: "single"
    prompt: "A new connection on a 1 Gbit/s, 80 ms RTT path starts with a congestion window of 10 segments of 1,460 bytes and doubles it every RTT with no loss. About how many round trips does it take before cwnd covers the path's bandwidth-delay product?"
    correct:
      - "10 round trips, about 800 ms"
    distractors:
      - "1 round trip, about 80 ms"
      - "7 round trips, about 560 ms"
      - "685 round trips, about 55 s"
    explanation: |-
      BDP = 10^9 × 0.080 / 8 = 10,000,000 bytes = 10,000,000 / 1,460 ≈ 6,849 segments.

      Slow start: cwnd = 10 × 2^k after k RTTs. 10 × 2^9 = 5,120 < 6,849 ≤ 10 × 2^10 = 10,240, so k = 10 round trips = 800 ms. Any transfer that completes in less time than this never reaches full speed, which is why short transfers on long, fast paths are latency-bound.

      "1 round trip" assumes the window starts at the BDP.

      "7 round trips" stops at 10 × 2^7 = 1,280 segments.

      "685 round trips" grows the window linearly by 10 segments per RTT, as if in congestion avoidance from the start.
    checks:
      - "b = 1e9 * 0.08 / 8 / 1460; k = next(i for i in range(30) if 10 * 2 ** i >= b); assert round(b) == 6849 and k == 10 and 10 * 2 ** 9 == 5120 and math.ceil(b / 10) == 685"
  - id: "parallel-streams-clue"
    type: "single"
    prompt: "Between two data centers, a single TCP stream measures 280 Mbit/s, while 8 parallel streams together reach about 2.2 Gbit/s on the same 10 Gbit/s link, with no loss. What does this most strongly suggest?"
    correct:
      - "A per-connection limit such as a receive window or socket buffer, since 8 × 280 Mbit/s ≈ 2.24 Gbit/s"
    distractors:
      - "The link's real capacity is 2.2 Gbit/s, and the single-stream result was distorted by slow start"
      - "Parallel streams lower the loss rate seen by each stream, so each of them can grow a larger window"
      - "The server's CPU is the bottleneck, because its interrupt load drops when it handles more streams"
      - "Other traffic on the 10 Gbit/s link leaves exactly 280 Mbit/s of spare capacity for each new stream"
    explanation: |-
      Each stream got the same 280 Mbit/s whether alone or with seven others, and the aggregate scaled linearly: 8 × 280 = 2,240 Mbit/s. Shared bottlenecks (the link, a CPU, a loss-limited path) would make streams compete and the per-stream rate fall. Linear scaling points to a limit that applies to each connection separately, typically window / RTT. Parallel streams are a workaround; raising buffer limits (or enabling auto-tuning) is the fix.

      "Real capacity is 2.2 Gbit/s" would have shown the 8 streams sharing it, not each at 280.

      "Lower loss per stream" contradicts the stated absence of loss.

      "CPU is the bottleneck" does not fit: more streams would load the CPU more, not less.

      "Other traffic leaves 280 Mbit/s per stream" would mean spare capacity grows with the number of streams, which shared capacity cannot do.
    checks:
      - "assert 8 * 280 == 2240"
---

## Learning objectives

- Diagnose window-limited, loss-limited and slow-start-limited transfers from numbers.
- Recognize the signature of a missing window scale option.
- Compute MSS clamps for tunnels and recognize MTU black holes.
- Use parallel-stream behavior as a diagnostic.

## The invariant

A TCP flow's throughput is capped by whichever limit binds first:

| Limit | Formula | Signature |
|---|---|---|
| Receive window | rwnd / RTT | flat, loss-free ceiling; scales linearly with parallel streams |
| Congestion window with loss | ≈ (MSS / RTT) × 1.22 / √p (Mathis et al., for Reno-style AIMD) | sawtooth; falls with RTT and loss rate |
| Slow start | 10, 20, 40, … segments per RTT | short transfers never reach full speed |
| Link | capacity | parallel streams share it and slow each other |

Comparing these limits with each other is the whole diagnosis: compute each one from measured values and see which one matches the observed rate. Three numbers decide most cases: the **BDP** (bandwidth × RTT), the **window** actually advertised (watch for 65,535 bytes, the unscaled maximum, which means window scaling is missing), and the **loss rate**. A loss-free transfer far below link speed is window-limited; a lossy one is congestion-limited; a short one is slow-start-limited.

**Packet size problems** look different. An MTU black hole does not slow a transfer; it stops it. Handshakes and small requests succeed, then the first full-size segment never arrives. Every encapsulation (VPN, GRE, VXLAN) shrinks the path MTU, and without working ICMP the endpoints never find out. Clamping the MSS at the tunnel, or packetization-layer probing, fixes it.

## Worked example

Three symptoms on paths with 60 ms RTT and 1 Gbit/s links:

| Symptom | Arithmetic | Diagnosis |
|---|---|---|
| One stream flat at 280 Mbit/s, no loss | 2 MiB × 8 / 0.06 s ≈ 280 Mbit/s; BDP = 7.5 MB | receive buffer capped at 2 MiB |
| Sawtooth near 75 Mbit/s with 0.001% loss | 1,460 × 8 / 0.06 × 1.22 / √0.00001 ≈ 75.4 Mbit/s | loss-limited; find the loss |
| Small API calls fine, report downloads hang | tunnel adds 60 bytes: MTU 1,440, MSS 1,400 | MTU black hole; clamp MSS to 1,400 |

Gathering the evidence on Linux:

```bash
$ ss -tin dst 198.51.100.20 | grep -oE 'wscale:[0-9,]+|rtt:[0-9.]+|cwnd:[0-9]+|retrans:[0-9/]+'
wscale:7,7
rtt:60.4
cwnd:1450
retrans:0/0
$ tcpdump -nn -c 2 'tcp[tcpflags] & tcp-syn != 0 and host 198.51.100.20'
IP 10.0.0.4.43122 > 198.51.100.20.443: Flags [S], seq 1, win 64240, options [mss 1460,sackOK,TS val 1 ecr 0,nop,wscale 7], length 0
IP 198.51.100.20.443 > 10.0.0.4.43122: Flags [S.], seq 9, ack 2, win 65160, options [mss 1400,sackOK,TS val 9 ecr 1,nop,wscale 7], length 0
```

Window scaling is present on both sides and nothing was retransmitted, so a flat rate points at buffers; the SYN-ACK's `mss 1400` shows the tunnel clamp is in place.

## Where this shows up today

As of November 2026, RFC 9438 records that CUBIC is the default congestion control in Linux, Windows and Apple stacks. CUBIC grows its window as a function of time rather than per round trip, so on long, fast paths it tolerates loss better than the Reno-based Mathis estimate predicts; the estimate remains a useful lower bound and a way to see how strongly RTT and loss interact.

## Pitfalls checklist

- Benchmarking with one stream and short durations, then blaming the link.
- Fixing a window-limited flow by adding parallel connections instead of raising buffers.
- Ignoring a loss rate of a few hundredths of a percent on long-RTT paths.
- Deploying tunnels without MSS clamping while firewalls drop ICMP.
- Reading a stall as congestion when it is a black hole.

## Key takeaways

- Flat and loss-free below link speed means window-limited: compare rwnd with the BDP.
- Loss-limited throughput scales with MSS / RTT and 1 / √p.
- Slow start needs about log2(BDP / initial window) round trips to fill a path.
- "Small works, large hangs" means MTU; clamp the MSS to the tunnel MTU minus 40 (IPv4).

## Sources

- RFC 7323 (window scale); RFC 5681 (AIMD); RFC 9438 (CUBIC); RFC 6928 (initial window).
- M. Mathis, J. Semke, J. Mahdavi and T. Ott, "The Macroscopic Behavior of the TCP Congestion Avoidance Algorithm", *ACM SIGCOMM CCR* 27(3), 1997.
- RFC 1191, RFC 8201 (Path MTU Discovery); RFC 4821 (packetization-layer PMTUD).
