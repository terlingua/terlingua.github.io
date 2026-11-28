---
title: "Layers, Encapsulation and the Hourglass"
body_checks:
  - "assert 1460 + 20 + 20 == 1500"
  - "assert 14 + 1500 + 4 == 1518 and 1518 + 8 + 12 == 1538"
  - "assert round(1460 / 1538 * 100, 1) == 94.9"
  - "assert round(1448 / 1538 * 100, 1) == 94.1"
  - "assert round(1440 / 1538 * 100, 1) == 93.6"
questions:
  - id: "narrow-waist"
    type: "single"
    prompt: "In the \"hourglass\" picture of the Internet architecture, which protocol sits at the narrow waist, and what does that position buy?"
    correct:
      - "IP: applications run over it and it runs over any link, so new links and new applications only need to work with IP"
    distractors:
      - "TCP: every application needs reliable delivery, so all traffic must pass through TCP before reaching IP on every host"
      - "Ethernet: every network ends in Ethernet, so all higher protocols are designed around its frame format"
      - "HTTP: nearly all traffic is web traffic, so lower layers are optimized for HTTP semantics"
    explanation: |-
      The Internet architecture (RFC 1122 describes its link, internet, transport and application layers) has exactly one network-layer protocol family that every host and router must speak: IP (RFC 791 for IPv4, RFC 8200 for IPv6). Many link technologies sit below it and many transports and applications sit above it. A new link type only needs a way to carry IP packets, and a new application only needs to run over IP, so the two sides evolve independently.

      "TCP" is wrong because TCP is one of several transports over IP; UDP, QUIC (which runs over UDP) and ICMP all bypass it. Routers forward IP packets without running TCP at all.

      "Ethernet" is wrong because links vary along a path (Wi-Fi, cellular, fiber, tunnels). IP hides those differences; nothing above IP depends on Ethernet framing.

      "HTTP" is wrong because the waist is defined by what every node must implement, not by traffic share. Routers never parse HTTP.
  - id: "wire-efficiency"
    type: "single"
    prompt: "A full-size TCP segment over IPv4 on Ethernet carries 1460 bytes of application data with no TCP or IP options. Counting the Ethernet header (14 bytes), frame check sequence (4), preamble and start delimiter (8) and inter-frame gap (12), what share of wire time carries application data?"
    correct:
      - "About 94.9% (1460 of 1538 byte times)"
    distractors:
      - "About 97.3% (1460 of 1500 bytes)"
      - "About 96.2% (1460 of 1518 bytes)"
      - "About 98.7% (1480 of 1500 bytes)"
    explanation: |-
      Each layer wraps the one above. Build the frame from the inside out:

      - Application data: 1460 bytes.
      - TCP header without options: 20 bytes, giving a 1480-byte segment.
      - IPv4 header without options: 20 bytes, giving a 1500-byte packet (the Ethernet MTU).
      - Ethernet header 14 + FCS 4: a 1518-byte frame.
      - Preamble/SFD 8 + inter-frame gap 12: 1538 byte times on the wire.

      Efficiency = 1460 / 1538 = 0.9493, about 94.9%.

      "1460 of 1500" measures only against the IP packet and ignores everything Ethernet adds.

      "1460 of 1518" counts the frame but forgets the preamble and gap, which also occupy the wire.

      "1480 of 1500" treats the TCP header as payload; 1480 is the TCP segment size, not the application data.
    checks:
      - "assert round(1460 / (1460 + 20 + 20 + 14 + 4 + 8 + 12) * 100, 1) == 94.9"
      - "assert round(1460 / 1500 * 100, 1) == 97.3"
      - "assert round(1460 / 1518 * 100, 1) == 96.2"
      - "assert round(1480 / 1500 * 100, 1) == 98.7"
  - id: "who-parses-ports"
    type: "multiple"
    prompt: "Which of these components must read the TCP header (the port numbers) to do their normal job? Select all that apply."
    correct:
      - "A NAT that lets many private hosts share one public IPv4 address"
      - "A layer-4 load balancer that picks a backend by hashing the 5-tuple"
    distractors:
      - "An Ethernet switch that learns which MAC address is on which port"
      - "An IP router doing longest-prefix-match forwarding"
      - "A Wi-Fi access point bridging frames onto a wired LAN"
    explanation: |-
      Each device works at the layer whose header it needs. Port numbers live in the transport header, so only devices whose decision depends on the connection identity must parse it.

      - A many-to-one NAT (NAPT) tells connections apart by rewriting and tracking source ports, so it must read and rewrite the TCP or UDP header.
      - A layer-4 load balancer keys on the 5-tuple (source IP, source port, destination IP, destination port, protocol) so all packets of one connection reach the same backend.

      The distractors operate below the transport layer:

      - "An Ethernet switch" learns and forwards on MAC addresses in the frame header; it never needs the IP header, let alone ports.
      - "An IP router" forwards on the destination IP address alone (longest-prefix match). Treating a router as port-aware is the common confusion between routing and firewalling.
      - "A Wi-Fi access point" bridges link-layer frames between 802.11 and Ethernet; like a switch, it is a layer-2 device.
  - id: "header-order"
    type: "single"
    prompt: "An HTTP/1.1 request is sent over TLS over TCP over IPv6 on Ethernet. In what order do the headers appear in the bytes on the wire, first to last?"
    correct:
      - "Ethernet, IPv6, TCP, TLS record header, HTTP request line and headers"
    distractors:
      - "Ethernet, TCP, IPv6, TLS record header, HTTP request line and headers"
      - "IPv6, Ethernet, TCP, TLS record header, HTTP request line and headers"
      - "Ethernet, IPv6, TLS record header, TCP, HTTP request line and headers"
    explanation: |-
      Encapsulation puts the lowest layer's header first, because each layer treats everything above it as opaque payload and prepends its own header. The link layer must come first so the receiving NIC can deliver the frame, then IP so routers can forward it, then TCP so the receiving host can find the connection, then TLS records, whose encrypted contents carry the HTTP request.

      "Ethernet, TCP, IPv6" puts the transport header outside the network header, but routers must find the IP header right after the link header.

      "IPv6, Ethernet" inverts the outermost pair; the Ethernet header is what the NIC reads first.

      "TLS record header, TCP" puts TLS below TCP; TLS runs over TCP's byte stream, so its records are inside TCP segments. (Over QUIC the picture changes, which later chapters cover.)
  - id: "segments-for-response"
    type: "single"
    prompt: "A server sends a 1,000,000-byte response over a TCP connection whose MSS is 1460 bytes. Ignoring the handshake and any retransmissions, what is the minimum number of data-carrying segments?"
    correct:
      - "685"
    distractors:
      - "684"
      - "667"
      - "977"
    explanation: |-
      Each segment carries at most MSS = 1460 bytes of payload, so the count is the ceiling of the division:

      1,000,000 / 1460 = 684.93, and the ceiling is 685 (684 full segments carry 998,640 bytes; one more segment carries the remaining 1,360 bytes).

      "684" truncates instead of rounding up and loses the final partial segment.

      "667" divides by 1500, the MTU, but the MTU includes the 40 bytes of IPv4 and TCP headers.

      "977" divides by 1024, a buffer-size habit that has nothing to do with the MSS.
    checks:
      - "assert math.ceil(1_000_000 / 1460) == 685 and 1_000_000 // 1460 == 684"
      - "assert 684 * 1460 == 998_640 and 1_000_000 - 998_640 == 1_360"
      - "assert math.ceil(1_000_000 / 1500) == 667 and math.ceil(1_000_000 / 1024) == 977"
  - id: "send-returned-bug"
    type: "single"
    prompt: "An order service writes a record to a TCP socket and marks the order as \"stored\" as soon as `send()` returns successfully. Applying the end-to-end argument, what is wrong with this design?"
    correct:
      - "A successful `send()` only means the kernel buffered the bytes; only the peer application's reply proves storage"
    distractors:
      - "Nothing; TCP retransmits until the bytes arrive, so a successful `send()` implies the remote application stored them"
      - "`send()` is only reliable on IPv6; on IPv4 the service must enable checksums before it can trust delivery of each record"
      - "The design is correct only when Nagle's algorithm is disabled, because then `send()` waits for the remote acknowledgment"
    explanation: |-
      The end-to-end argument (Saltzer, Reed and Clark, 1984) says a function such as "the record is durably stored" can only be implemented completely by the endpoints that care about it. TCP's reliability covers delivery of bytes into the peer's kernel receive buffer. A successful `send()` means even less: the bytes were copied into the *local* send buffer. The peer process may crash before reading them, or read them and fail to write to disk, and TCP will never report either event to the sender.

      "Nothing; TCP retransmits" confuses transport reliability with application success. Even a TCP ACK only says the peer's kernel received the bytes.

      "only reliable on IPv6" is invented; both IP versions carry TCP the same way, and the TCP checksum is mandatory in both.

      "only when Nagle's algorithm is disabled" misreads Nagle: it only delays sending small segments. `send()` never waits for the peer's acknowledgment either way.
---

## Learning objectives

- Use the OSI model as vocabulary while reasoning with the four-layer Internet architecture.
- Explain why IP is the narrow waist of the hourglass and what that buys.
- Build a frame from the inside out and compute encapsulation overhead.
- Say which header each kind of network device must read.

## The invariant

The OSI model (ISO/IEC 7498-1) has seven layers: physical, data link, network, transport, session, presentation, application. It is a useful *vocabulary*: "layer 2" means link-layer addressing and framing, "layer 3" means IP routing, "layer 4" means ports and connections, and "layer 7" means the application protocol. It is not how the Internet was specified. RFC 1122, the host requirements, describes four layers: link, internet (IP), transport (TCP, UDP) and application. OSI's session and presentation layers have no separate protocols in the Internet; TLS and HTTP fold those jobs into the application.

The defining feature of the Internet architecture is its shape. Many link technologies sit at the bottom, many applications at the top, and in the middle exactly one protocol that every host and router must speak: IP (RFC 791 for IPv4, RFC 8200 for IPv6). This is the *narrow waist*. A new link only has to carry IP packets; a new application only has to run over IP. Routers forward packets by looking at the IP header and nothing above it.

Two rules follow from the layering:

1. **Encapsulation.** Each layer treats what it receives from above as opaque payload and prepends its own header (and, for Ethernet, appends a trailer). On the wire, the lowest layer's header comes first.
2. **Each device works at its layer.** A switch reads link-layer addresses. A router reads the IP header, decrements the TTL or hop limit, and builds a new link-layer frame for the next hop. Devices that must tell connections apart, such as NATs and layer-4 load balancers, read transport headers too. A layer-7 proxy terminates the TCP connection (and usually TLS) and speaks the application protocol on both sides.

The *end-to-end argument* (Saltzer, Reed and Clark, 1984) explains why the middle stays simple: a guarantee such as "the record was stored" can only be completed by the endpoints, so lower layers provide best effort and the endpoints add what they need. TCP's reliability ends at the peer's kernel buffer, not at the peer's database.

## Worked example

A full-size segment on IPv4 over Ethernet, built inside out:

| Layer | Adds | Running size |
|---|---|---|
| Application data | 1460 | 1460 |
| TCP header (no options) | 20 | 1480 |
| IPv4 header (no options) | 20 | 1500 (the Ethernet MTU) |
| Ethernet header + FCS | 14 + 4 | 1518 (the frame) |
| Preamble/SFD + inter-frame gap | 8 + 12 | 1538 byte times on the wire |

Wire efficiency is 1460 / 1538 = 94.9%. Real stacks usually enable TCP timestamps (RFC 7323), a 12-byte option that leaves 1448 bytes of data: 1448 / 1538 = 94.1%. Over IPv6 (a 40-byte header) without options, data is 1440 bytes: 93.6%. These few percent are why link speed never equals goodput, even on a perfect network.

A packet capture shows the same order, outermost first:

```bash
$ tcpdump -e -nn -c 1 'tcp port 443'
12:00:00.000001 3c:22:fb:00:00:01 > 00:1a:2b:00:00:02, ethertype IPv4 (0x0800), length 1514:
    192.0.2.10.51514 > 198.51.100.7.443: Flags [.], seq 1:1449, ack 1, win 501,
    options [nop,nop,TS val 1 ecr 1], length 1448
```

`length 1514` is the frame without the FCS (14 + 1500), and `length 1448` is the TCP payload after the timestamp option.

## Where this shows up today

As of November 2026, the newest standard transport in wide use, QUIC (RFC 9000, 2021), travels inside UDP datagrams instead of having its own IP protocol number. That is the hourglass in practice: firewalls and NATs along real paths pass TCP and UDP but often drop anything else, so new transports build on top of UDP rather than beside it.

## Pitfalls checklist

- Treating OSI layer numbers as protocol boundaries in the Internet; TLS has no single OSI layer.
- Assuming link speed equals throughput; headers, gaps and options take several percent before any loss.
- Believing routers read ports; only NATs, firewalls and load balancers that track connections do.
- Treating a successful `send()` or a TCP ACK as proof that the peer application processed the data.
- Dividing by the MTU instead of the MSS when counting segments.

## Key takeaways

- OSI is vocabulary; the Internet is specified as four layers around a single narrow waist, IP.
- Encapsulation prepends headers, so the lowest layer's header is first on the wire.
- A 1500-byte MTU carries 1460 bytes of TCP data at most over IPv4, 1440 over IPv6, fewer with options.
- Guarantees that matter to the application must be confirmed end to end by the application.

## Sources

- RFC 1122, Requirements for Internet Hosts — Communication Layers.
- RFC 791 (IPv4); RFC 8200 (IPv6); RFC 9293 (TCP); RFC 7323 (TCP timestamps and window scaling).
- J. H. Saltzer, D. P. Reed and D. D. Clark, "End-to-End Arguments in System Design", *ACM TOCS* 2(4), 1984.
- ISO/IEC 7498-1, Basic Reference Model.
