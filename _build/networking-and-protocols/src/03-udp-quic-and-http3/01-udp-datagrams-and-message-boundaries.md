---
title: "UDP: Datagrams, Message Boundaries and What You Must Add"
body_checks:
  - "assert 65535 - 20 - 8 == 65507 and 65535 - 8 == 65527"
  - "assert 1500 - 20 - 8 == 1472 and 1500 - 40 - 8 == 1452"
questions:
  - id: "udp-recv-one-datagram"
    type: "single"
    prompt: "A sender calls `sendto()` twice on a UDP socket, first with `b\"abc\"` and then with `b\"defg\"`. Both datagrams have arrived when the receiver calls `recv(1024)` once. What does that call return?"
    correct:
      - "`b\"abc\"`"
    distractors:
      - "`b\"abcdefg\"`"
      - "`b\"defg\"`"
      - "Either `b\"abc\"` or `b\"abcdefg\"`, depending on timing"
    explanation: |-
      UDP (RFC 768) is message-oriented: each `sendto()` produces one datagram, and each receive returns at most one whole datagram, in the order the socket received them. The first call returns `b"abc"`; the next returns `b"defg"`. Datagrams are never merged.

      "`b"abcdefg"`" is what a TCP receiver might see, because TCP is a byte stream that may coalesce writes.

      "`b"defg"`" skips the datagram that arrived first. UDP does not guarantee ordering across the network, but a socket hands out datagrams in the order it queued them.

      "Depending on timing" also describes TCP; UDP boundaries do not depend on timing.
  - id: "tcp-framing-bug"
    type: "single"
    prompt: "A service sends one JSON document per `send()` call over TCP. The receiver runs `json.loads(sock.recv(65536))` once per expected message. It passes tests on a laptop but raises `JSONDecodeError` under load across the network. Why?"
    correct:
      - "TCP is a byte stream, so one `recv()` can return part of a message or several; the protocol needs framing such as a length prefix"
    distractors:
      - "Under load TCP drops segments, and the bytes missing from the stream leave the JSON incomplete when it is parsed"
      - "A buffer of 65,536 bytes is larger than the MSS, so the kernel truncates each read to one segment's worth of data"
      - "TCP reorders segments under load, and `recv()` returns them in arrival order instead of the order they were sent"
    explanation: |-
      TCP (RFC 9293) promises an ordered, reliable stream of *bytes*, not messages. The sender's `send()` boundaries are not preserved: the stack may split one write across segments, Nagle or a busy receiver may combine several writes, and `recv()` returns whatever bytes are available, up to the buffer size. On a laptop each message often arrives alone, so the bug hides; under load, reads return half a document or two documents glued together.

      The fix is explicit framing: a length prefix (read exactly 4 bytes for the length, then read until that many bytes have arrived) or a delimiter that cannot occur inside a message, such as a newline for line-delimited JSON.

      "TCP drops segments" is wrong: TCP retransmits, and the application never sees a hole.

      "Truncates each read to one segment" is wrong; `recv()` can return many segments' worth of buffered bytes, and the MSS does not limit reads.

      "Returns them in arrival order" is wrong; TCP reorders out-of-order segments before delivery.
  - id: "max-udp-payload-v4"
    type: "single"
    prompt: "What is the largest UDP payload that can be carried in a single IPv4 datagram (no IP options, before any fragmentation is considered)?"
    correct:
      - "65,507 bytes"
    distractors:
      - "65,535 bytes"
      - "65,515 bytes"
      - "1,472 bytes"
    explanation: |-
      The IPv4 Total Length field is 16 bits, so the whole packet is at most 65,535 bytes. Subtract the 20-byte IPv4 header and the 8-byte UDP header: 65,535 − 20 − 8 = 65,507 bytes of payload. Such a datagram is fragmented into many IP fragments on a 1500-byte link, and losing any one of them loses the whole datagram.

      "65,535 bytes" is the maximum IP packet size, headers included.

      "65,515 bytes" subtracts only the IPv4 header and forgets the UDP header.

      "1,472 bytes" is the largest payload that fits one 1500-byte Ethernet frame without fragmentation (1500 − 20 − 8), a practical limit rather than the protocol maximum.
    checks:
      - "assert 65535 - 20 - 8 == 65507 and 65535 - 20 == 65515 and 1500 - 28 == 1472"
  - id: "udp-truncation"
    type: "single"
    prompt: "On Linux, a 1,200-byte datagram is queued on a UDP socket, and the application calls `recvfrom()` with a 512-byte buffer. What happens?"
    correct:
      - "The call returns the first 512 bytes and the other 688 bytes of that datagram are discarded"
    distractors:
      - "The call returns 512 bytes, and the next call returns the remaining 688 bytes of the datagram"
      - "The call fails with an error and the datagram stays queued until a large enough buffer is offered"
      - "The kernel splits the datagram into three datagrams of at most 512 bytes and returns the first"
    explanation: |-
      Because UDP preserves message boundaries, one receive consumes exactly one datagram. If the buffer is smaller than the datagram, the excess is lost: on Linux the call returns 512 bytes and sets `MSG_TRUNC` in the message flags (`recvmsg`), and the next call returns the *next* datagram. Applications must size receive buffers for the largest datagram they accept (DNS resolvers, for example, use the EDNS buffer size they advertised).

      "The next call returns the remaining 688 bytes" is stream behavior; a datagram socket does not keep the rest.

      "Fails and stays queued" does not match Linux. (Windows reports `WSAEMSGSIZE`, but the excess data is discarded there too.)

      "Splits into three datagrams" invents a re-segmentation step; the kernel never changes datagram boundaries.
    checks:
      - "assert 1200 - 512 == 688"
  - id: "what-udp-apps-add"
    type: "multiple"
    prompt: "An application sends its own messages over plain UDP. Which of these must the application implement itself if it needs them? Select all that apply."
    correct:
      - "Retransmission of messages that are lost"
      - "Congestion control so it does not overload the path"
      - "Restoring the order in which messages were sent"
    distractors:
      - "Keeping each message's boundaries intact on delivery"
      - "Delivering each datagram to the socket bound to its destination port"
    explanation: |-
      UDP adds only ports (demultiplexing), a length, and a checksum to IP's best-effort delivery (RFC 768). Loss, duplication and reordering pass straight through, and UDP has no congestion control. RFC 8085 (UDP usage guidelines) asks applications that send more than a little traffic to add congestion control themselves, and anything that needs reliability or order must add sequence numbers, acknowledgments and timers, which is exactly what QUIC does on top of UDP.

      "Keeping message boundaries" is something UDP already provides; each datagram is delivered whole or not at all.

      "Delivering to the socket bound to its port" is UDP's demultiplexing, its main job.
  - id: "reflection-amplification"
    type: "single"
    prompt: "A UDP service answers a 60-byte request with a 3,000-byte response and does not verify the source address. An attacker sends 200 Mbit/s of requests with the victim's address as the source. About how much traffic reaches the victim?"
    correct:
      - "About 10 Gbit/s"
    distractors:
      - "About 200 Mbit/s"
      - "About 4 Mbit/s"
      - "About 600 Gbit/s"
    explanation: |-
      Without a handshake, a UDP server cannot tell whether the source address is real, so it sends its response to whoever the request names. The amplification factor is response bytes / request bytes = 3,000 / 60 = 50. Traffic at the victim = 200 Mbit/s × 50 = 10 Gbit/s.

      "About 200 Mbit/s" assumes one byte out per byte in; the size difference is the whole attack.

      "About 4 Mbit/s" divides by 50 instead of multiplying.

      "About 600 Gbit/s" multiplies the rate by the response size in bytes rather than by the ratio.

      Defenses are protocol-level: return-path validation before sending large responses (QUIC's anti-amplification limit and TCP's handshake), response-size limits, and network ingress filtering of spoofed sources (BCP 38).
    checks:
      - "assert 3000 / 60 == 50 and 200e6 * 50 == 10e9 and 200e6 / 50 == 4e6 and 200e6 * 3000 == 600e9"
---

## Learning objectives

- Explain how UDP's message boundaries differ from TCP's byte stream.
- Frame messages correctly over TCP and size receive buffers correctly over UDP.
- Compute UDP payload limits and reflection amplification.
- List what an application must build on top of UDP.

## The invariant

UDP (RFC 768) adds an 8-byte header to IP: source port, destination port, length and checksum. That is all. There is no connection, no handshake, no acknowledgment, no retransmission, no ordering and no congestion control. What UDP *does* guarantee is **message integrity of boundaries**: each send produces one datagram, and each receive returns at most one whole datagram. If the receive buffer is too small, the rest of that datagram is discarded.

TCP is the opposite: an ordered, reliable **byte stream** (RFC 9293). Write boundaries vanish. One `recv()` can return part of one write or the tail of one write plus the head of the next. Any message protocol over TCP needs explicit **framing**: a length prefix, a delimiter, or a self-describing format read incrementally.

The numbers follow from header sizes:

| Limit | Arithmetic | Bytes |
|---|---|---|
| Largest UDP payload over IPv4 | 65,535 − 20 − 8 | 65,507 |
| Largest UDP payload over IPv6 (no jumbograms) | 65,535 − 8 (IPv6 Payload Length excludes its 40-byte header) | 65,527 |
| Largest unfragmented payload on Ethernet, IPv4 | 1500 − 20 − 8 | 1,472 |
| Same over IPv6 | 1500 − 40 − 8 | 1,452 |

Datagrams larger than the path MTU are fragmented at the IP layer, and the loss of any fragment loses the datagram, so practical UDP protocols keep datagrams below the path MTU. The UDP checksum is optional over IPv4 (zero means "none") and mandatory over IPv6 (RFC 8200).

Because there is no handshake, a UDP server cannot verify that a request's source address is genuine. A server whose responses are larger than requests becomes a **reflection amplifier**: attackers spoof the victim's address and the server multiplies their traffic. RFC 8085 sets out what well-behaved UDP applications add: congestion control, limits on response size before the peer is validated, and keepalives for NAT mappings.

## Worked example

The same two writes, over UDP and over TCP:

```python
import socket

u_rx = socket.socket(socket.AF_INET, socket.SOCK_DGRAM); u_rx.bind(("127.0.0.1", 0))
u_tx = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
u_tx.sendto(b"abc", u_rx.getsockname()); u_tx.sendto(b"defg", u_rx.getsockname())
print(u_rx.recv(1024), u_rx.recv(1024))

srv = socket.create_server(("127.0.0.1", 0))
c = socket.create_connection(srv.getsockname()); s, _ = srv.accept()
c.sendall(b"abc"); c.sendall(b"defg"); c.close()
data = b""
while chunk := s.recv(1024):
    data += chunk
print(data)
```

Output:

```bash
b'abc' b'defg'
b'abcdefg'
```

The UDP receiver gets two messages; the TCP receiver gets seven bytes with no boundary. (A single `s.recv(1024)` might have returned `b'abc'` or `b'abcdefg'`; the loop is how stream code has to read.) A length-prefixed reader for TCP:

```python
import struct

def read_exact(sock, n):
    buf = bytearray()
    while len(buf) < n:
        chunk = sock.recv(n - len(buf))
        if not chunk:
            raise EOFError("connection closed mid-message")
        buf += chunk
    return bytes(buf)

def read_message(sock):
    (length,) = struct.unpack("!I", read_exact(sock, 4))
    return read_exact(sock, length)
```

## Where this shows up today

As of November 2026, the transport carrying most new Internet protocol work, QUIC (RFC 9000), and with it HTTP/3 (RFC 9114), runs over UDP. QUIC adds back everything this section lists as missing (reliability, ordering per stream, congestion control, and address validation before large responses), which shows how much of TCP an application must rebuild to use UDP safely at scale.

## Pitfalls checklist

- Parsing one `recv()` from a TCP socket as one message.
- Using a UDP receive buffer smaller than the largest datagram and silently truncating.
- Sending datagrams larger than the path MTU and multiplying the loss rate through fragmentation.
- Building a UDP service whose responses are much larger than unauthenticated requests.
- Sending UDP at a fixed high rate with no congestion response.

## Key takeaways

- UDP preserves message boundaries and nothing else beyond ports and a checksum.
- TCP preserves bytes and order but not boundaries; frame your messages.
- Max UDP payload: 65,507 bytes over IPv4; practical size is below the path MTU.
- No handshake means spoofable sources; size responses with amplification in mind.

## Sources

- RFC 768, User Datagram Protocol; RFC 8085, UDP Usage Guidelines (BCP 145).
- RFC 9293 (TCP byte stream); RFC 8200 (IPv6 UDP checksum requirement).
- RFC 2827 (BCP 38), Network Ingress Filtering.
