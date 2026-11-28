---
title: "Retransmission Timers, Nagle, Delayed ACKs and Head-of-Line Blocking"
body_checks:
  - "srtt, rttvar = 100, 50; assert srtt + 4 * rttvar == 300"
  - "s, v, rto = rto_update(100, 50, 200, min_rto=0); assert close(v, 62.5) and close(s, 112.5) and close(rto, 362.5)"
  - "assert sum(2 ** i for i in range(5)) == 31"
questions:
  - id: "rto-update"
    type: "single"
    prompt: "A sender has `SRTT` = 80 ms and `RTTVAR` = 20 ms. A new RTT sample of 120 ms arrives. Using RFC 6298 with α = 1/8, β = 1/4, negligible clock granularity and no minimum applied, what are the new `SRTT` and `RTO`?"
    correct:
      - "`SRTT` = 85 ms, `RTO` = 185 ms"
    distractors:
      - "`SRTT` = 85 ms, `RTO` = 180 ms"
      - "`SRTT` = 85 ms, `RTO` = 165 ms"
      - "`SRTT` = 120 ms, `RTO` = 200 ms"
    explanation: |-
      RFC 6298 updates the variance first, using the *old* SRTT, then the smoothed RTT:

      - RTTVAR = (1 − 1/4) × 20 + 1/4 × |80 − 120| = 15 + 10 = 25 ms
      - SRTT = (1 − 1/8) × 80 + 1/8 × 120 = 70 + 15 = 85 ms
      - RTO = SRTT + max(G, 4 × RTTVAR) = 85 + 100 = 185 ms

      "RTO = 180 ms" updates SRTT first and then measures the deviation from the new value: |85 − 120| = 35 gives RTTVAR = 23.75 and RTO = 85 + 95 = 180. RFC 6298 specifies the opposite order.

      "RTO = 165 ms" adds 4 × the *old* RTTVAR (80) to the new SRTT.

      "SRTT = 120 ms" replaces the average with the latest sample, which is what smoothing exists to avoid.

      In a real stack the result would then be raised to the minimum RTO: RFC 6298 says SHOULD round up to 1 second; Linux uses 200 ms.
    checks:
      - "s, v, rto = rto_update(80, 20, 120, min_rto=0); assert close(s, 85) and close(v, 25) and close(rto, 185)"
      - "s2 = 7 / 8 * 80 + 1 / 8 * 120; v2 = 3 / 4 * 20 + 1 / 4 * abs(s2 - 120); assert close(s2 + 4 * v2, 180) and close(85 + 4 * 20, 165)"
  - id: "karn-ambiguous-sample"
    type: "single"
    prompt: "A segment's timer expires, the segment is retransmitted, and an ACK covering it arrives 30 ms after the retransmission. What should the RTT estimator do with this ACK?"
    correct:
      - "Take no RTT sample from it and keep the backed-off RTO until a non-retransmitted segment is timed"
    distractors:
      - "Record a 30 ms sample, since the ACK most likely answers the retransmitted copy"
      - "Record the time since the first transmission, since that copy was sent earlier"
      - "Reset SRTT to the 1-second initial value, since the path has clearly changed"
    explanation: |-
      The ACK is ambiguous: it may answer the original transmission (delayed) or the retransmission. Using either interpretation biases the estimate. Karn's algorithm, required by RFC 6298, says RTT samples MUST NOT be taken from retransmitted segments, and the backed-off RTO is kept until an ACK arrives for data that was sent only once.

      "Record 30 ms" would shrink the RTO if the ACK actually belonged to the slow original, causing more spurious timeouts.

      "Time since the first transmission" would inflate the RTO if the original was lost.

      "Reset SRTT to 1 second" is invented; the initial RTO applies only before any sample exists.
  - id: "backoff-timeline"
    type: "single"
    prompt: "A segment is sent with `RTO` = 1 s. Every copy is lost, and the RTO doubles after each timeout. How long after the original transmission is the fifth retransmission sent?"
    correct:
      - "31 seconds"
    distractors:
      - "16 seconds"
      - "5 seconds"
      - "63 seconds"
    explanation: |-
      RFC 6298 doubles the RTO on each expiry (exponential backoff). The waits before each retransmission are 1, 2, 4, 8 and 16 seconds:

      | Retransmission | Wait before it | Time since original |
      |---|---|---|
      | 1 | 1 s | 1 s |
      | 2 | 2 s | 3 s |
      | 3 | 4 s | 7 s |
      | 4 | 8 s | 15 s |
      | 5 | 16 s | 31 s |

      "16 seconds" is the length of the last wait, not the cumulative time.

      "5 seconds" assumes no backoff, a fixed 1-second timer.

      "63 seconds" is when the *sixth* retransmission would go out (31 + 32).
    checks:
      - "t = 0; w = 1; times = []\nfor _ in range(6):\n    t += w; times.append(t); w *= 2\nassert times[4] == 31 and times[5] == 63"
  - id: "nagle-delayed-ack-stall"
    type: "single"
    prompt: "A client sends each request with two `write()` calls, a 40-byte header and then a 60-byte body, and waits for the response. The server answers only after it has the whole request. The RTT is 1 ms, yet every request takes about 40 ms. What is the cause?"
    correct:
      - "Nagle holds the body until the header is ACKed, and the server delays that ACK hoping to piggyback it"
    distractors:
      - "Slow start restarts on every request because the connection is idle between requests for over an RTO"
      - "A retransmission timeout fires for each request because the small segments are lost in the network"
      - "The server's receive window is too small for two segments, so the second write waits for buffer space"
    explanation: |-
      Two standard mechanisms interact:

      1. **Nagle's algorithm** (RFC 896, RFC 1122): while any data is unacknowledged, a sender holds back further *small* segments until an ACK arrives or a full MSS accumulates. The header goes out at once; the 60-byte body waits.
      2. **Delayed ACK** (RFC 1122, RFC 5681): a receiver may wait (Linux about 40 ms, specification limit 500 ms) to combine the ACK with data or with a second segment. The server has only the header, has nothing to send yet, and so delays the ACK.

      Each side waits for the other until the delayed-ACK timer fires. The stall therefore equals the delayed-ACK timeout, about 40 ms, regardless of the 1 ms RTT. Fixes: send the request in one write, or set `TCP_NODELAY` on the client.

      "Slow start restarts" could cost a round trip or two, which is 1–2 ms here, not 40 ms.

      "A retransmission timeout fires" would cost at least the minimum RTO (200 ms to 1 s) and would appear as retransmissions in a capture.

      "The receive window is too small" is implausible for 100 bytes; even a tiny buffer holds both segments.
    checks:
      - "assert 40 + 60 == 100 and 100 < 1460"
  - id: "tcp-head-of-line"
    type: "single"
    prompt: "An HTTP/2 connection multiplexes ten streams over one TCP connection. A segment carrying part of stream 3 is lost; segments carrying data for streams 1, 2 and 4–10 arrive right after it. What happens to that later data?"
    correct:
      - "It waits in the receive buffer; TCP delivers nothing past the gap, so all streams stall until the retransmission"
    distractors:
      - "It is delivered immediately; only stream 3 waits, because HTTP/2 frames are independent of each other"
      - "HTTP/2 notices the loss and re-requests stream 3 on a new connection while the others carry on"
      - "TCP discards it, and the sender later resends everything from the lost segment onward"
    explanation: |-
      TCP offers one ordered byte stream (RFC 9293). The receiver may buffer out-of-order segments, but it can only hand bytes to the application in sequence. Until the missing segment is retransmitted (at least one RTT, longer if a timeout is needed), every byte after the hole waits, whichever HTTP/2 stream it belongs to. This is **transport-level head-of-line blocking**: HTTP/2 removed HTTP/1.1's request-level blocking, but one lost packet still stalls all streams.

      "Delivered immediately; only stream 3 waits" describes QUIC, which delivers each stream independently, not HTTP/2 over TCP.

      "Re-requests on a new connection" is not something HTTP/2 does; it cannot even see the loss, which TCP hides.

      "TCP discards it" describes go-back-N; TCP receivers keep out-of-order data, and with SACK the sender retransmits only what is missing.
  - id: "delayed-ack-rules"
    type: "multiple"
    prompt: "Which statements about delayed acknowledgments match the TCP specifications? Select all that apply."
    correct:
      - "An ACK must not be delayed by more than 0.5 seconds"
      - "A receiver should ACK at least every second full-sized segment"
    distractors:
      - "Delayed ACKs are turned off automatically whenever Nagle's algorithm is enabled"
      - "A receiver may delay an ACK until its receive buffer is completely full"
      - "An out-of-order segment should be ACKed only after the delay timer expires"
    explanation: |-
      RFC 1122 §4.2.3.2 allows delaying ACKs to save packets but limits the delay to less than 0.5 seconds; RFC 5681 adds that an ACK SHOULD be generated for at least every second full-sized segment, and that out-of-order segments SHOULD be ACKed immediately (those immediate ACKs are the duplicate ACKs used by fast retransmit).

      "Turned off whenever Nagle is enabled" is false; Nagle is a sender rule and delayed ACK a receiver rule, and their *combination* is what causes stalls.

      "Until the buffer is full" would stall senders indefinitely and contradicts both limits.

      "Out-of-order ACKed only after the timer" contradicts RFC 5681's immediate-ACK rule and would slow loss detection.
---

## Learning objectives

- Compute SRTT, RTTVAR and RTO with RFC 6298 and apply Karn's algorithm.
- Compute exponential backoff timelines.
- Diagnose the Nagle plus delayed-ACK stall and fix it.
- Explain transport-level head-of-line blocking.

## The invariant

**Retransmission timer.** TCP retransmits when an ACK does not arrive within the retransmission timeout. RFC 6298 derives the RTO from measured round trips:

- First sample R: SRTT = R, RTTVAR = R / 2.
- Later samples R′: first RTTVAR = (1 − β) × RTTVAR + β × |SRTT − R′|, then SRTT = (1 − α) × SRTT + α × R′, with α = 1/8 and β = 1/4.
- RTO = SRTT + max(G, 4 × RTTVAR), where G is the clock granularity. Before any sample the RTO is 1 second; RFC 6298 says the RTO SHOULD be rounded up to at least 1 second (Linux uses 200 ms).
- Each expiry doubles the RTO (exponential backoff).
- **Karn's algorithm**: never take an RTT sample from a retransmitted segment, because its ACK is ambiguous.

**Nagle's algorithm** (RFC 896, required as a SHOULD by RFC 1122): while unacknowledged data is outstanding, hold back new small segments until an ACK arrives or a full MSS is ready. It prevents floods of tiny packets from chatty writers.

**Delayed ACKs** (RFC 1122 §4.2.3.2, RFC 5681): a receiver may delay an ACK, by less than 0.5 seconds, to combine it with outgoing data, but should ACK at least every second full-sized segment and should ACK out-of-order data immediately.

Each rule is sensible alone; together they deadlock briefly. A client that writes a small header and then a small body sees Nagle hold the body until the header is ACKed, while the server, which cannot answer yet, delays that ACK. The stall lasts the delayed-ACK timer. Writing the whole message at once, or setting `TCP_NODELAY`, removes it.

**Head-of-line blocking.** TCP delivers an ordered byte stream. Data after a lost segment sits in the receive buffer until the gap is filled, even if it belongs to logically independent messages. Multiplexing many streams over one TCP connection shares that fate.

## Worked example

RTO arithmetic for samples of 100 ms and then 200 ms:

| Step | SRTT | RTTVAR | SRTT + 4 × RTTVAR |
|---|---|---|---|
| First sample 100 ms | 100 | 50 | 300 ms |
| Sample 200 ms: RTTVAR = 0.75 × 50 + 0.25 × 100 | 112.5 | 62.5 | 362.5 ms |

With RFC 6298's 1-second floor both RTOs become 1 s; with Linux's 200 ms floor they stay as computed. If every copy is lost, retransmissions go out at 1, 3, 7, 15 and 31 seconds after the original with a 1 s starting RTO.

The write-write-read stall, and its fix in C#:

```csharp
using System.Net.Sockets;
using System.Text;

byte[] body = Encoding.UTF8.GetBytes("""{"op":"get","key":"42"}""");
byte[] header = Encoding.ASCII.GetBytes($"LEN {body.Length}\n");

using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
socket.NoDelay = true;
await socket.ConnectAsync("10.0.0.9", 7000);

byte[] message = [.. header, .. body];
await socket.SendAsync(message);
```

`NoDelay` (`TCP_NODELAY`) or the single `SendAsync` of the combined buffer alone removes the 40 ms stall; sending one buffer is the better habit because it also halves the packet count.

## Where this shows up today

As of November 2026, RFC 6298 is still the standard RTO algorithm, with no updating RFCs. Many HTTP stacks enable `TCP_NODELAY` by default (Go's `net` package and ASP.NET Core's Kestrel, for example), because they already write whole messages, so the stall now appears mainly in hand-written protocols and database drivers that write headers and bodies separately. Head-of-line blocking over TCP is the main reason HTTP/3 moved to QUIC, which delivers each stream independently (next chapter).

## Pitfalls checklist

- Writing a message in several small `write()` calls on a Nagle-enabled socket.
- Disabling Nagle and then writing byte-by-byte, flooding the network with tiny packets.
- Sampling RTT from retransmissions, or ignoring backoff when estimating failure-detection time.
- Assuming a minimum RTO of 1 second everywhere; implementations differ.
- Multiplexing unrelated streams over one TCP connection on a lossy path.

## Key takeaways

- RTO = SRTT + 4 × RTTVAR, updated variance-first, doubled on every expiry.
- Karn: no RTT samples from retransmitted segments.
- Nagle plus delayed ACK stalls write-write-read patterns for the delayed-ACK time.
- TCP's in-order delivery makes one loss block every stream that shares the connection.

## Sources

- RFC 6298, Computing TCP's Retransmission Timer.
- RFC 896, Congestion Control in IP/TCP Internetworks (Nagle); RFC 1122 §4.2.3.2 (delayed ACKs) and §4.2.3.4 (Nagle).
- RFC 5681, TCP Congestion Control (ACK generation); RFC 9293 (in-order delivery).
