---
title: "The Three-Way Handshake and Sequence/ACK Arithmetic"
body_checks:
  - "c, s = 1000, 5000; assert (c + 1, s + 1) == (1001, 5001)"
  - "assert 1001 + 120 == 1121 and 5001 + 900 == 5901"
  - "assert 1121 + 1 == 1122 and 5901 + 1 == 5902"
questions:
  - id: "synack-numbers"
    type: "single"
    prompt: "A client sends a SYN with sequence number 4000. The server chooses initial sequence number 9000. What sequence and acknowledgment numbers does the server's SYN-ACK carry?"
    correct:
      - "`seq=9000`, `ack=4001`"
    distractors:
      - "`seq=9001`, `ack=4001`"
      - "`seq=9000`, `ack=4000`"
      - "`seq=4001`, `ack=9000`"
    explanation: |-
      RFC 9293: a segment's sequence number is the number of its first sequence-space octet, the SYN flag occupies one sequence number, and the acknowledgment field holds the *next* sequence number the sender expects to receive.

      - The SYN-ACK is the server's first segment, so its sequence number is its ISN: 9000.
      - The client's SYN used number 4000, so the next number the server expects is 4001.

      "`seq=9001`" advances the server's sequence number before its own SYN has been sent; the SYN flag on this very segment consumes 9000.

      "`ack=4000`" acknowledges the SYN's own number instead of the next expected one; TCP acknowledgments always name the next byte wanted.

      "`seq=4001`, `ack=9000`" swaps the two sequence spaces. Each direction has its own; the server numbers its bytes from 9000.
    checks:
      - "assert 4000 + 1 == 4001"
  - id: "ack-after-two-writes"
    type: "single"
    prompt: "After a handshake in which the client's ISN was 4000, the client sends 300 bytes and then 200 bytes, and both arrive in order. What acknowledgment number does the server send after receiving the second segment?"
    correct:
      - "4501"
    distractors:
      - "4500"
      - "4301"
      - "4502"
    explanation: |-
      The SYN consumed 4000, so the first data byte is 4001.

      - First segment: bytes 4001–4300 (300 bytes), so the next expected byte is 4301.
      - Second segment: bytes 4301–4500 (200 bytes), so the next expected byte is 4501.

      ACKs are cumulative: one ACK of 4501 acknowledges everything up to and including byte 4500.

      "4500" names the last byte received rather than the next byte expected.

      "4301" acknowledges only the first segment.

      "4502" counts an extra sequence number as if a FIN had been sent; no FIN was sent.
    checks:
      - "first = 4000 + 1; assert first + 300 == 4301 and first + 300 + 200 == 4501"
  - id: "duplicate-acks-on-gap"
    type: "single"
    prompt: "A sender transmits four 100-byte segments starting at sequence numbers 1001, 1101, 1201 and 1301. The segment starting at 1101 is lost; the others arrive in order. Without SACK, which acknowledgment numbers does the receiver send in response to the three segments that arrive?"
    correct:
      - "1101, 1101, 1101"
    distractors:
      - "1101, 1301, 1401"
      - "1101, 1201, 1301"
      - "1101, and then nothing until the gap is filled"
    explanation: |-
      The acknowledgment number is cumulative: it can only advance past bytes that have all arrived. RFC 9293 and RFC 5681 have a receiver send an immediate ACK when an out-of-order segment arrives, and that ACK repeats the next expected sequence number.

      | Arrives | Bytes held | Next expected | ACK sent |
      |---|---|---|---|
      | 1001 | 1001–1100 | 1101 | 1101 |
      | 1201 | 1001–1100, 1201–1300 | 1101 | 1101 (duplicate) |
      | 1301 | 1001–1100, 1201–1400 | 1101 | 1101 (duplicate) |

      These duplicate ACKs are the signal fast retransmit uses. When 1101–1200 finally arrives, the ACK jumps to 1401.

      "1101, 1301, 1401" acknowledges bytes past the hole, which a cumulative ACK cannot do. (SACK, RFC 2018, reports the received blocks separately, without moving the ACK field.)

      "1101, 1201, 1301" echoes the starting number of each arriving segment.

      "Nothing until the gap is filled" withholds the duplicate ACKs that out-of-order arrivals are supposed to trigger.
    checks:
      - "got = {1001, 1201, 1301}; nxt = 1001; acks = []\nfor seg in [1001, 1201, 1301]:\n    while nxt in got:\n        nxt += 100\n    acks.append(nxt)\nassert acks == [1101, 1101, 1101]"
  - id: "why-random-isn"
    type: "multiple"
    prompt: "Why do TCP implementations choose initial sequence numbers that vary over time and are hard to predict instead of always starting at 0? Select all that apply."
    correct:
      - "So that delayed segments from an earlier connection with the same 4-tuple are unlikely to fit in the new connection's window"
      - "So that an off-path attacker cannot easily guess sequence numbers to inject data or resets"
    distractors:
      - "So that the receiver can compute how many bytes the sender has sent since it booted"
      - "Because sequence number 0 is reserved for SYN segments and cannot carry data"
      - "So that both sides pick the same ISN and can skip the third handshake segment"
    explanation: |-
      RFC 9293 requires ISNs that advance with a clock so that sequence numbers of a new connection do not overlap with old segments still in the network from a previous incarnation of the same (address, port) pair. It also incorporates the RFC 6528 recommendation to add a keyed hash of the connection identifiers, because a predictable ISN lets an attacker who cannot see the traffic forge segments with acceptable sequence numbers (injected data, or a RST that kills the connection).

      "Bytes sent since boot" is not a property of the ISN; the clock component is coarse and per-connection hashing deliberately hides any global count.

      "Sequence number 0 is reserved" is false; 0 is an ordinary value and sequence numbers wrap around modulo 2^32.

      "Both sides pick the same ISN" misunderstands the handshake: each direction has its own sequence space, and the third segment is what confirms the server's ISN to the server.
  - id: "ack-of-fin"
    type: "single"
    prompt: "The client has sent all of its data; its next sequence number is 4501. It now sends a FIN that carries no data. What acknowledgment number does the server send for the FIN?"
    correct:
      - "4502"
    distractors:
      - "4501"
      - "4500"
      - "9001"
    explanation: |-
      Like SYN, the FIN flag occupies one sequence number (RFC 9293). The FIN is sent with `seq=4501`, uses that number, and the next expected number becomes 4502, so the server acknowledges 4502.

      "4501" treats FIN as consuming no sequence space; then the sender could never tell whether its FIN had been acknowledged.

      "4500" acknowledges the last data byte instead of naming the next expected number.

      "9001" mixes in the server's own sequence space.
    checks:
      - "assert 4501 + 1 == 4502"
  - id: "flow-byte-count-bug"
    type: "single"
    prompt: "A flow monitor computes \"bytes sent by the client\" as the server's final ACK number minus the client's ISN. For a connection in which the client sent 500 bytes of data and then closed normally, it reports 502. Why?"
    correct:
      - "SYN and FIN each consume one sequence number, so the range includes two numbers that carried no data"
    distractors:
      - "The receiver acknowledges one byte beyond what it received in order to signal that its window is open"
      - "Retransmitted segments were counted twice because each retransmission moves the ACK number forward"
      - "The 20-byte TCP header is counted in sequence space, and the monitor rounds to the nearest byte"
      - "Delayed ACKs acknowledge two extra sequence numbers so that the sender can detect the end of the stream"
    explanation: |-
      Sequence numbers cover data bytes plus the SYN and FIN control flags. With client ISN x:

      - SYN uses x; data uses x+1 through x+500; FIN uses x+501.
      - The server's final ACK is x+502, so final ACK − ISN = 502 = 500 data bytes + SYN + FIN.

      Correct accounting subtracts the two control flags (or counts payload lengths of unique segments).

      "Acknowledges one byte beyond" misstates the rule; the ACK is exactly the next expected number, and windows are advertised in a separate field.

      "Retransmissions counted twice" is wrong because duplicate data does not advance the cumulative ACK.

      "TCP header in sequence space" is false; headers are never numbered, and if they were the error would be 40 or more, not 2.

      "Delayed ACKs acknowledge two extra numbers" invents a rule: delaying an ACK changes when it is sent, never the number it carries.
    checks:
      - "x = 777; final_ack = x + 1 + 500 + 1; assert final_ack - x == 502 and final_ack - x - 2 == 500"
---

## Learning objectives

- Give the sequence and acknowledgment numbers of every segment in a handshake.
- Compute acknowledgment numbers for data, gaps and FIN.
- Explain why initial sequence numbers are clock-driven and unpredictable.
- Count application bytes from sequence numbers without off-by-one errors.

## The invariant

TCP (RFC 9293) numbers **every byte** in each direction of a connection. Each direction has its own 32-bit sequence space starting at an **initial sequence number** (ISN) chosen by the sender; arithmetic is modulo 2^32.

Four rules generate every number you will see in a trace:

1. A segment's **sequence number** is the number of its first byte (or of its SYN or FIN flag, if it carries no data).
2. **SYN and FIN each consume one sequence number.** That is how they can be acknowledged and retransmitted like data.
3. The **acknowledgment number** is the next sequence number the receiver expects. It is cumulative: it can only advance past bytes that have all arrived.
4. An out-of-order arrival triggers an immediate ACK that repeats the current acknowledgment number, a *duplicate ACK* (RFC 5681). The SACK option (RFC 2018) can additionally list received blocks above the hole.

The three-way handshake exchanges and confirms both ISNs:

| Segment | Flags | seq | ack |
|---|---|---|---|
| 1 | SYN | x | — |
| 2 | SYN, ACK | y | x + 1 |
| 3 | ACK | x + 1 | y + 1 |

The ISN is not arbitrary. RFC 9293 requires it to advance with a clock so that old duplicates from an earlier connection with the same 4-tuple are unlikely to land inside the new window, and incorporates RFC 6528's keyed hash so that off-path attackers cannot predict valid numbers and inject data or resets.

## Worked example

Client ISN 1000, server ISN 5000. The client sends a 120-byte request; the server sends a 900-byte response; then both close.

| # | Direction | Flags | seq | ack | Data |
|---|---|---|---|---|---|
| 1 | C→S | SYN | 1000 | — | 0 |
| 2 | S→C | SYN, ACK | 5000 | 1001 | 0 |
| 3 | C→S | ACK | 1001 | 5001 | 0 |
| 4 | C→S | ACK, PSH | 1001 | 5001 | 120 |
| 5 | S→C | ACK, PSH | 5001 | 1121 | 900 |
| 6 | C→S | FIN, ACK | 1121 | 5901 | 0 |
| 7 | S→C | FIN, ACK | 5901 | 1122 | 0 |
| 8 | C→S | ACK | 1122 | 5902 | 0 |

Check the arithmetic: the request covers 1001–1120, so the next expected byte is 1121; the response covers 5001–5900, so 5901; each FIN adds one. Packet analyzers usually display *relative* numbers (ISN shown as 0), which turns the table into 0, 1, 121, 122 for the client:

```bash
$ tcpdump -nn -S 'tcp port 8080'   # -S prints absolute sequence numbers
10.0.0.2.40000 > 10.0.0.9.8080: Flags [S], seq 1000, win 64240, length 0
10.0.0.9.8080 > 10.0.0.2.40000: Flags [S.], seq 5000, ack 1001, win 65160, length 0
10.0.0.2.40000 > 10.0.0.9.8080: Flags [.], ack 5001, win 502, length 0
10.0.0.2.40000 > 10.0.0.9.8080: Flags [P.], seq 1001:1121, ack 5001, win 502, length 120
```

`seq 1001:1121` is tcpdump's half-open range: first byte 1001, next byte 1121.

## Where this shows up today

As of November 2026, RFC 9293 (2022, STD 7) is the TCP specification. It replaced RFC 793 and folded in several later documents, including RFC 6528's ISN generation and RFC 879's MSS rules, so one document now defines the arithmetic above. Kernel implementations of SYN cookies and TCP Fast Open encode extra state in or around these same numbers without changing the rules.

## Pitfalls checklist

- Forgetting that SYN and FIN each take a sequence number, giving off-by-one byte counts.
- Reading an ACK number as "last byte received" instead of "next byte expected".
- Mixing up the two directions' sequence spaces in a trace.
- Assuming a duplicate ACK means data was duplicated; it means a gap.
- Comparing relative sequence numbers from one capture with absolute numbers from another.

## Key takeaways

- Sequence numbers count bytes plus SYN and FIN; ACKs name the next expected number.
- The handshake is SYN(x), SYN-ACK(y, x+1), ACK(x+1, y+1).
- Gaps produce duplicate ACKs, the raw material for fast retransmit.
- ISNs are clock-driven and keyed so old and forged segments rarely fit the window.

## Sources

- RFC 9293, Transmission Control Protocol (STD 7).
- RFC 5681, TCP Congestion Control (immediate ACK on out-of-order data); RFC 2018, TCP Selective Acknowledgment Options.
- RFC 6528, Defending against Sequence Number Attacks (obsoleted by and incorporated into RFC 9293).
