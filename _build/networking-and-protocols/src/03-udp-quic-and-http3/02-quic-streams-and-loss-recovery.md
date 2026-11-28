---
title: "QUIC Streams, Packet Numbers and Loss Recovery"
body_checks:
  - "assert [4 * n + 0 for n in range(3)] == [0, 4, 8] and [4 * n + 1 for n in range(2)] == [1, 5] and [4 * n + 2 for n in range(2)] == [2, 6] and [4 * n + 3 for n in range(2)] == [3, 7]"
  - "assert 14 - 11 >= 3 and 14 - 13 < 3"
  - "assert 9 / 8 * max(40, 48) == 54"
questions:
  - id: "quic-per-stream-delivery"
    type: "single"
    prompt: "A QUIC connection carries ten streams. One packet, holding only data for stream 3, is lost; packets carrying data for the other nine streams arrive just after it. What happens to the other streams' data?"
    correct:
      - "It is delivered to those streams at once; only stream 3 waits for its missing range to be retransmitted"
    distractors:
      - "It waits in the receive buffer until the lost packet is retransmitted, because QUIC orders all packets"
      - "It is delivered at once, and stream 3 skips the missing range, since QUIC streams accept data loss"
      - "It is discarded, and the sender retransmits every packet sent after the lost one, as go-back-N does"
    explanation: |-
      RFC 9000 orders data *per stream*: each STREAM frame carries a stream ID and a byte offset, and a receiver delivers a stream's bytes in offset order. There is no ordering across streams and no connection-wide byte sequence. A gap in stream 3 blocks only stream 3; the other streams' bytes are complete and are delivered immediately. This removes the transport-level head-of-line blocking that TCP imposes on multiplexed protocols.

      "QUIC orders all packets" describes TCP's single byte stream; QUIC packets are processed as they arrive.

      "Stream 3 skips the missing range" is wrong: streams are reliable, and the lost frame's data is retransmitted. (Unreliable data needs the separate DATAGRAM extension.)

      "Go-back-N" is wrong: QUIC acknowledges ranges of packets and retransmits only lost frames.
  - id: "retransmission-new-packet-number"
    type: "single"
    prompt: "QUIC packet number 41 carried a STREAM frame for stream 0 at offset 1000 and was declared lost. How is that data retransmitted?"
    correct:
      - "Its frame goes into a new packet with a new, higher packet number, keeping stream offset 1000"
    distractors:
      - "Packet 41 is sent again byte for byte, with the same packet number, so the peer can match it"
      - "The data is sent at a new stream offset after the highest offset sent so far on stream 0"
      - "The connection rewinds to packet 41 and resends every packet after it, numbers unchanged"
    explanation: |-
      In QUIC, packet numbers are never reused within a packet number space (RFC 9000; RFC 9002 states they never repeat for the lifetime of a connection). Frames, not packets, are retransmitted: lost frames' contents are placed in a new packet with a new packet number. The stream offset stays 1000, because offsets identify the data's position in the stream.

      A consequence is that every ACK is unambiguous: an acknowledgment of packet 57 refers to exactly one transmission, so QUIC can take RTT samples from retransmitted data. TCP needs Karn's algorithm precisely because its retransmissions reuse sequence numbers.

      "Sent again with the same packet number" would recreate TCP's ambiguity and violates RFC 9000.

      "A new stream offset" would place the bytes at the wrong position in the stream.

      "Rewinds and resends everything" describes go-back-N, which QUIC does not use.
  - id: "packet-threshold-loss"
    type: "single"
    prompt: "A QUIC sender has sent packets 20 through 27. An ACK arrives acknowledging 20, 21, 23, 24 and 25. Using only RFC 9002's packet threshold of 3, which unacknowledged packets are declared lost?"
    correct:
      - "Only packet 22"
    distractors:
      - "Packets 22, 26 and 27"
      - "Packets 22 and 26"
      - "None until two more ACKs arrive"
    explanation: |-
      RFC 9002 declares an unacknowledged packet lost if it was sent *before* an acknowledged packet and either its packet number is at least kPacketThreshold (3) below the largest acknowledged, or it was sent long enough ago (time threshold). Here the largest acknowledged is 25.

      - Packet 22: 25 − 22 = 3 ≥ 3, so lost.
      - Packets 26 and 27: newer than the largest acknowledged, so there is no evidence about them yet; they are simply in flight.

      "22, 26 and 27" treats every unacknowledged packet as lost.

      "22 and 26" also condemns 26, which was sent after the largest acknowledged packet.

      "None until two more ACKs" carries over TCP's habit of counting duplicate ACKs; QUIC counts packet-number distance, which the single ACK already shows.
    checks:
      - "acked = {20, 21, 23, 24, 25}; largest = max(acked); lost = [p for p in range(20, 28) if p not in acked and p < largest and largest - p >= 3]; assert lost == [22]"
  - id: "time-threshold"
    type: "single"
    prompt: "A QUIC sender's `smoothed_rtt` is 40 ms and its `latest_rtt` is 48 ms. Under RFC 9002, how long after it was sent is an unacknowledged packet older than the largest acknowledged packet declared lost by the time threshold?"
    correct:
      - "54 ms"
    distractors:
      - "45 ms"
      - "48 ms"
      - "120 ms"
    explanation: |-
      RFC 9002's time threshold is kTimeThreshold × max(smoothed_rtt, latest_rtt), with kTimeThreshold = 9/8 (and a floor of kGranularity, 1 ms). Using the larger of the two RTTs guards against declaring losses too early when the RTT has just risen.

      max(40, 48) = 48 ms; 9/8 × 48 = 54 ms.

      "45 ms" applies 9/8 to the smoothed RTT only.

      "48 ms" omits the 9/8 margin that allows for reordering.

      "120 ms" multiplies by 3, mixing the packet threshold into the time threshold.
    checks:
      - "assert 9 / 8 * max(40, 48) == 54 and 9 / 8 * 40 == 45 and 3 * 40 == 120"
  - id: "stream-id-encoding"
    type: "single"
    prompt: "QUIC stream IDs encode the initiator and direction in their two lowest bits. What stream ID does a client use for its *third* client-initiated bidirectional stream?"
    correct:
      - "8"
    distractors:
      - "2"
      - "3"
      - "5"
    explanation: |-
      RFC 9000: the least significant bit says who opened the stream (0 client, 1 server) and the next bit says whether it is bidirectional (0) or unidirectional (1). Stream IDs of each type count up by 4.

      | Type (low bits) | IDs |
      |---|---|
      | Client-initiated bidirectional (0x0) | 0, 4, 8, … |
      | Server-initiated bidirectional (0x1) | 1, 5, 9, … |
      | Client-initiated unidirectional (0x2) | 2, 6, 10, … |
      | Server-initiated unidirectional (0x3) | 3, 7, 11, … |

      The third client bidirectional stream is 4 × 2 + 0 = 8. HTTP/3 uses one such stream per request.

      "2" is the client's first *unidirectional* stream (HTTP/3 clients typically open their control stream there).

      "3" is the server's first unidirectional stream.

      "5" is the server's second bidirectional stream; servers cannot open request streams in HTTP/3.
    checks:
      - "assert 4 * 2 + 0x0 == 8 and 4 * 0 + 0x2 == 2 and 4 * 0 + 0x3 == 3 and 4 * 1 + 0x1 == 5"
  - id: "why-quic-over-udp"
    type: "multiple"
    prompt: "Why was QUIC built on top of UDP rather than as a new IP protocol or as a set of TCP extensions? Select all that apply."
    correct:
      - "Many paths pass TCP and UDP but drop packets carrying unfamiliar IP protocol numbers"
      - "Middleboxes strip or block unfamiliar TCP options, so TCP changes deploy very slowly"
      - "Over UDP the transport can ship in user-space libraries and update with the application"
    distractors:
      - "UDP provides congestion control that QUIC reuses instead of defining its own"
      - "UDP's checksum also encrypts the payload, so QUIC gets confidentiality for free on every path"
    explanation: |-
      This is *ossification*: network devices that inspect transport headers come to depend on what they have seen, and break anything new. Firewalls and NATs commonly pass only TCP and UDP, so a new IP protocol number is dead on arrival on many paths. TCP itself is hard to change because middleboxes normalize or drop unknown options and kernels update slowly. UDP gives QUIC a header that middleboxes already pass, and QUIC encrypts and authenticates almost everything above it, including most of its own header, so middleboxes cannot come to depend on its internals. Living above UDP also means QUIC can be implemented in libraries and updated with each application release.

      "UDP provides congestion control" is false; QUIC defines its own (RFC 9002), as it must.

      "UDP's checksum encrypts the payload" is false; a checksum detects corruption and provides no secrecy. QUIC's confidentiality comes from TLS 1.3 keys (RFC 9001).
---

## Learning objectives

- Explain how QUIC streams avoid transport head-of-line blocking.
- Explain why QUIC packet numbers are never reused and what that buys.
- Apply RFC 9002's packet and time thresholds.
- Decode stream IDs and explain why QUIC runs over UDP.

## The invariant

A QUIC connection (RFC 9000) carries many **streams**. Each stream is an ordered, reliable byte sequence identified by a 62-bit **stream ID** whose two low bits give the initiator and direction (0 client bidirectional, 1 server bidirectional, 2 client unidirectional, 3 server unidirectional). Stream data travels in STREAM frames carrying (stream ID, offset, bytes). A receiver orders bytes *within each stream* by offset; nothing orders bytes across streams. A lost packet therefore delays only the streams whose frames it carried.

Packets and data are numbered separately:

- **Packet numbers** increase monotonically within each packet number space and are never reused. A lost packet is never resent. Instead its *frames* are put into new packets with new numbers. The stream offset of retransmitted data does not change.
- **ACK frames** list ranges of received packet numbers and report how long the receiver delayed the acknowledgment, so RTT samples are exact and unambiguous even for retransmitted data.

RFC 9002 declares a packet lost when a *later* packet has been acknowledged and either

- its packet number is at least **kPacketThreshold = 3** below the largest acknowledged packet, or
- it was sent more than **kTimeThreshold × max(smoothed_rtt, latest_rtt)** ago, with kTimeThreshold = 9/8.

When nothing is acknowledged at all, a **probe timeout** (PTO) sends probe packets rather than collapsing the window as TCP's RTO does. The initial RTT before any sample is 333 ms.

QUIC sits on UDP because of **ossification**: middleboxes pass TCP and UDP but often drop new IP protocols, and they mangle unfamiliar TCP options. QUIC encrypts nearly all of its header and payload, so middleboxes cannot come to depend on its details, and it can live in user-space libraries.

## Worked example

A sender sends packets 10 to 16; packet 11 is lost.

| ACK received | Ranges | Largest acked | Lost by packet threshold |
|---|---|---|---|
| after 13 arrives | 10, 12–13 | 13 | none (13 − 11 = 2) |
| after 14 arrives | 10, 12–14 | 14 | 11 (14 − 11 = 3) |

With smoothed_rtt = 40 ms and latest_rtt = 48 ms, the time threshold is 9/8 × 48 = 54 ms, so packet 11 would also be declared lost 54 ms after it was sent if 14 were the last packet in flight. The data from packet 11 goes into, say, packet 17 with its original stream offsets; the ACK for 17 yields a clean RTT sample.

A trace from a qlog-style log of a client opening three request streams:

```json
[
  {"name": "transport:packet_sent", "data": {"packet_number": 10, "frames": [{"frame_type": "stream", "stream_id": 0, "offset": 0, "length": 1200}]}},
  {"name": "transport:packet_sent", "data": {"packet_number": 11, "frames": [{"frame_type": "stream", "stream_id": 4, "offset": 0, "length": 1200}]}},
  {"name": "transport:packet_sent", "data": {"packet_number": 12, "frames": [{"frame_type": "stream", "stream_id": 8, "offset": 0, "length": 1200}]}},
  {"name": "recovery:packet_lost", "data": {"packet_number": 11}},
  {"name": "transport:packet_sent", "data": {"packet_number": 17, "frames": [{"frame_type": "stream", "stream_id": 4, "offset": 0, "length": 1200}]}}
]
```

Streams 0 and 8 complete while stream 4 waits for packet 17.

## Where this shows up today

As of November 2026, QUIC has a second version, RFC 9369 (2023), whose stated purpose is to combat ossification and exercise version negotiation: it changes the version number on the wire and the initial key derivation so that middleboxes cannot hard-code version 1. Encrypted headers plus deliberate variation are how QUIC tries to stay changeable.

## Pitfalls checklist

- Assuming QUIC removes all head-of-line blocking; data within one stream is still ordered.
- Putting unrelated messages on one stream and recreating TCP's blocking.
- Confusing packet numbers with stream offsets when reading traces.
- Porting TCP's duplicate-ACK counting into QUIC tooling.
- Assuming middleboxes can inspect QUIC the way they inspect TCP.

## Key takeaways

- QUIC orders bytes per stream, so one loss blocks only the affected streams.
- Packet numbers are never reused; frames are retransmitted in new packets.
- Loss: 3 packets behind the largest acknowledged, or 9/8 of the RTT late.
- Running over UDP and encrypting headers are defenses against ossification.

## Sources

- RFC 9000, QUIC: A UDP-Based Multiplexed and Secure Transport (streams, stream IDs, packet numbers, frames).
- RFC 9002, QUIC Loss Detection and Congestion Control (kPacketThreshold, kTimeThreshold, PTO, kInitialRtt).
- RFC 9369, QUIC Version 2.
