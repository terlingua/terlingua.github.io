---
title: "MTU, MSS, Fragmentation and Path MTU Discovery"
body_checks:
  - "assert 1500 - 20 - 20 == 1460 and 1500 - 40 - 20 == 1440 and 1492 - 40 == 1452"
  - "assert 1500 - 24 == 1476 and 1476 - 40 == 1436"
  - "data = 3980; per = (1500 - 20) // 8 * 8; frags = [min(per, data - i) for i in range(0, data, per)]; assert frags == [1480, 1480, 1020] and [i // 8 for i in range(0, data, per)] == [0, 185, 370] and [f + 20 for f in frags] == [1500, 1500, 1040]"
questions:
  - id: "mss-over-pppoe"
    type: "single"
    prompt: "A home connection uses PPPoE, which leaves an IP MTU of 1492 bytes. What MSS should a host advertise for IPv4 TCP connections over it, with no IP or TCP options?"
    correct:
      - "1452"
    distractors:
      - "1460"
      - "1472"
      - "1432"
    explanation: |-
      The MSS is the largest TCP payload a host wants to receive: MTU minus the IP header minus the TCP header (RFC 9293 defines the MSS option as the maximum segment data, without headers). For IPv4 without options: 1492 − 20 − 20 = 1452.

      "1460" is the Ethernet value (1500 − 40) and ignores the 8 bytes PPPoE takes out of the 1500-byte Ethernet payload.

      "1472" subtracts only the IPv4 header; 1472 is the largest ICMP echo payload plus ICMP header that fits (1492 − 20), a figure people remember from `ping -s` tests.

      "1432" is the IPv6 value over the same link (1492 − 40 − 20), not the IPv4 one.
    checks:
      - "assert 1492 - 20 - 20 == 1452 and 1500 - 40 == 1460 and 1492 - 20 == 1472 and 1492 - 60 == 1432"
  - id: "fragment-offsets"
    type: "single"
    prompt: "A router must forward an IPv4 packet with total length 4000 bytes (20-byte header, 3980 bytes of data, DF clear) onto a link with MTU 1500. What Fragment Offset values appear in the fragments, in order?"
    correct:
      - "0, 185, 370"
    distractors:
      - "0, 1480, 2960"
      - "0, 187, 374"
      - "0, 185, 370, 555"
    explanation: |-
      RFC 791: each fragment carries its own 20-byte header, the data in every fragment but the last must be a multiple of 8 bytes, and the Fragment Offset field counts in 8-byte units.

      - Data per full fragment: 1500 − 20 = 1480 bytes, already a multiple of 8.
      - 3980 bytes of data split into 1480 + 1480 + 1020.
      - Offsets in bytes: 0, 1480, 2960; in 8-byte units: 0, 185, 370.
      - Total lengths: 1500, 1500, 1040. More Fragments is set on the first two and clear on the last.

      "0, 1480, 2960" gives the byte offsets; the header field stores them divided by 8.

      "0, 187, 374" divides the MTU (1500) by 8 and rounds, forgetting that each fragment repeats the 20-byte header.

      "0, 185, 370, 555" adds a fourth fragment; 3980 − 2960 = 1020 bytes fit in the third.
    checks:
      - "data = 3980; per = 1480; offs = list(range(0, data, per)); assert [o // 8 for o in offs] == [0, 185, 370] and offs == [0, 1480, 2960] and data - offs[-1] == 1020"
  - id: "df-set-too-big"
    type: "single"
    prompt: "An IPv4 packet of 1500 bytes with the Don't Fragment bit set reaches a router whose next-hop link has MTU 1400. What does a router that follows RFC 1191 do?"
    correct:
      - "Drops it and returns ICMP Destination Unreachable, code 4, with next-hop MTU 1400"
    distractors:
      - "Fragments it anyway, because delivering a packet takes priority over the DF bit"
      - "Truncates it to 1400 bytes and sets a flag telling the receiver data was cut"
      - "Drops it silently, because DF means the sender will detect loss by timeout and retransmit"
    explanation: |-
      Path MTU Discovery for IPv4 (RFC 1191) works by setting DF on every packet. A router that cannot forward a DF packet without fragmenting it must discard it and return ICMP type 3 (Destination Unreachable), code 4 ("fragmentation needed and DF set"). RFC 1191 added the next-hop MTU to that message, so the sender can lower its path MTU estimate to 1400 at once and resend smaller packets.

      "Fragments it anyway" ignores what DF means; fragmenting a DF packet is forbidden.

      "Truncates it" is not an IP behavior; IP has no partial-delivery flag and would corrupt the transport payload.

      "Drops it silently" is what happens when the ICMP is filtered or never generated, which is precisely the failure called a PMTUD black hole, not the specified behavior.
  - id: "ipv6-size-rules"
    type: "multiple"
    prompt: "Which statements about packet size in IPv6 are true? Select all that apply."
    correct:
      - "Routers never fragment IPv6 packets in transit"
      - "Every link must carry IPv6 packets of at least 1280 bytes"
      - "A router that cannot forward a too-large packet returns ICMPv6 Packet Too Big"
    distractors:
      - "Every IPv6 header carries a DF bit that the sender must set"
      - "The receiver can ask the sender to fragment by returning a Fragment header"
    explanation: |-
      RFC 8200 moved fragmentation entirely to the source: routers never fragment (§4.5), and the source uses a Fragment extension header if it must. §5 sets the minimum link MTU at 1280 bytes; links that cannot carry that must fragment and reassemble below IPv6. When a packet is too big for the next hop, the router drops it and sends ICMPv6 Packet Too Big with the MTU, which Path MTU Discovery for IPv6 (RFC 8201) relies on.

      "A DF bit" does not exist in IPv6; since routers never fragment, every packet behaves as if DF were set.

      "The receiver returns a Fragment header" inverts the roles: the Fragment header is added by the sending source, and no receiver-side request mechanism exists.
  - id: "tunnel-black-hole"
    type: "single"
    prompt: "Users reach an internal app over a site-to-site tunnel whose MTU is 1400. TCP handshakes complete and small API calls work, but downloads of large reports stall until timeout. The firewall drops all ICMP. What is happening, and what fixes it?"
    correct:
      - "A PMTUD black hole: full-size DF packets die at the tunnel and the ICMP error never returns; allow it or clamp the MSS"
    distractors:
      - "TCP window scaling is mangled by the tunnel; disabling window scaling on both ends lets large transfers finish"
      - "The TLS certificate chain is too large to encrypt; a smaller ECDSA certificate lets the handshake finish"
      - "The tunnel reorders packets; raising the duplicate-ACK threshold stops spurious retransmissions of large replies"
    explanation: |-
      Handshakes and small requests use small packets, which fit through the 1400-byte tunnel. The server's large response is sent in 1500-byte packets with DF set (MSS 1460 negotiated on 1500-byte LANs). The tunnel endpoint drops them and sends ICMP "fragmentation needed", but the firewall discards that ICMP, so the server never learns the smaller MTU and retransmits the same oversized packets until it gives up. That is a Path MTU Discovery black hole.

      Fixes: permit ICMP type 3 code 4 (and ICMPv6 Packet Too Big) through firewalls; clamp the TCP MSS on the tunnel routers so endpoints never send packets larger than the tunnel allows (1400 − 40 = 1360 for IPv4); or use packetization-layer PMTUD (RFC 4821), which probes sizes without depending on ICMP.

      "Window scaling is mangled" is a real but different failure; it shows up as low throughput, not as a stall exactly when packets become full-size.

      "The certificate chain is too large" misplaces the stall: the handshake completed, and certificate size has no effect on encryption.

      "The tunnel reorders packets" would cause spurious retransmissions and slowdowns, not a complete stall of every large response.
    checks:
      - "assert 1400 - 40 == 1360 and 1500 - 40 == 1460"
  - id: "gre-mss-clamp"
    type: "single"
    prompt: "A GRE tunnel over IPv4 runs on a path with a 1500-byte MTU. GRE adds a 4-byte header and the outer IPv4 header adds 20 bytes. What MSS should the tunnel router clamp IPv4 TCP SYNs to, assuming no options?"
    correct:
      - "1436"
    distractors:
      - "1460"
      - "1456"
      - "1416"
    explanation: |-
      Work from the outer path inwards:

      - Tunnel MTU: 1500 − 20 (outer IPv4) − 4 (GRE) = 1476 bytes for the inner packet.
      - Inner IPv4 header 20 + TCP header 20: MSS = 1476 − 40 = 1436.

      Clamping rewrites the MSS option in SYNs passing through the router, so both ends agree to send segments that fit inside the tunnel without relying on ICMP.

      "1460" is the MSS for a plain 1500-byte link and ignores the 24 bytes of encapsulation.

      "1456" subtracts the inner TCP header but not the inner IPv4 header (1476 − 20).

      "1416" is the right clamp for *IPv6* TCP inside the same tunnel (1476 − 60), not IPv4.
    checks:
      - "mtu = 1500 - 20 - 4; assert mtu == 1476 and mtu - 40 == 1436 and mtu - 20 == 1456 and mtu - 60 == 1416"
---

## Learning objectives

- Compute MSS from an MTU for IPv4, IPv6 and common encapsulations.
- Compute IPv4 fragment sizes and offsets.
- Explain Path MTU Discovery for IPv4 and IPv6 and why it fails when ICMP is filtered.
- Choose between allowing ICMP, MSS clamping and packetization-layer probing.

## The invariant

The **MTU** of a link is the largest IP packet it carries in one frame (1500 bytes on standard Ethernet). The **path MTU** is the smallest MTU along a path. TCP's **MSS** option (RFC 9293) advertises the largest segment *payload* a host will accept, so MSS = MTU − IP header − TCP header: 1460 for IPv4 and 1440 for IPv6 on Ethernet without options.

When a packet is larger than the next link's MTU, the protocols differ:

- **IPv4, DF clear.** The router fragments (RFC 791). Each fragment gets its own header; every fragment's data except the last is a multiple of 8 bytes, and the Fragment Offset field counts in 8-byte units. The destination reassembles; losing any fragment loses the whole packet.
- **IPv4, DF set.** The router drops the packet and returns ICMP Destination Unreachable, code 4, "fragmentation needed". RFC 1191 Path MTU Discovery sets DF on all packets and uses the next-hop MTU in that message to lower its estimate (never below 68 bytes, the IPv4 minimum).
- **IPv6.** Routers never fragment (RFC 8200 §4.5). A too-big packet is dropped and the router returns ICMPv6 Packet Too Big; RFC 8201 is the IPv6 Path MTU Discovery procedure. Every link must carry at least 1280 bytes (RFC 8200 §5).

Both discovery procedures depend on ICMP getting back to the sender. When a firewall filters it, or a tunnel endpoint never generates it, large packets vanish without any signal: a **PMTUD black hole**. The symptom is distinctive: connection setup and small exchanges work, and transfers stall the moment segments reach full size.

Three remedies exist. Let the ICMP messages through. **Clamp the MSS** on routers at the narrow point, rewriting the MSS option in passing SYNs so endpoints never send oversized TCP segments. Or use **packetization-layer PMTUD** (RFC 4821 for TCP, RFC 8899 for datagram transports), which probes with larger packets and treats their loss as the signal, without trusting ICMP.

## Worked example

MSS values for common links:

| Link | IP MTU | IPv4 MSS | IPv6 MSS |
|---|---|---|---|
| Ethernet | 1500 | 1460 | 1440 |
| PPPoE (8 bytes of PPPoE header) | 1492 | 1452 | 1432 |
| GRE over IPv4 (20 + 4 bytes) | 1476 | 1436 | 1416 |

Fragmenting a 4000-byte IPv4 packet (3980 bytes of data) onto a 1500-byte link:

| Fragment | Data bytes | Byte offset | Offset field | Total length | MF |
|---|---|---|---|---|---|
| 1 | 1480 | 0 | 0 | 1500 | 1 |
| 2 | 1480 | 1480 | 185 | 1500 | 1 |
| 3 | 1020 | 2960 | 370 | 1040 | 0 |

Testing a path by hand with DF set: on Linux, `ping -M do -s 1472` sends 1472 bytes of ICMP data + 8 bytes of ICMP header + 20 bytes of IPv4 header = 1500 bytes.

```bash
$ ping -M do -s 1472 -c 1 203.0.113.9
ping: local error: message too long, mtu=1400
$ ping -M do -s 1372 -c 1 203.0.113.9
1380 bytes from 203.0.113.9: icmp_seq=1 ttl=57 time=21.4 ms
```

The first probe fails because the local tunnel interface knows its 1400-byte MTU; through a remote black hole, the same probe would simply time out.

## Where this shows up today

As of November 2026, QUIC (RFC 9000) requires every path it uses to carry UDP payloads of at least 1200 bytes and allows datagram PMTUD (RFC 8899) to probe upward from there, so a QUIC connection does not depend on ICMP to find its packet size. Tunnels and VPN overlays remain the usual source of black holes for TCP, which is why MSS clamping is a standard option on tunnel routers.

## Pitfalls checklist

- Blocking all ICMP or ICMPv6 "for security" and breaking Path MTU Discovery.
- Building a tunnel or overlay without clamping MSS or lowering the inner MTU.
- Computing MSS from the MTU without subtracting both headers, or using the IPv4 value for IPv6.
- Treating "small requests work, big ones hang" as an application bug.
- Relying on IPv4 fragmentation for performance; one lost fragment loses the packet.

## Key takeaways

- MSS = MTU − IP header − TCP header; every encapsulation shrinks it.
- IPv4 routers fragment only when DF is clear; IPv6 routers never fragment.
- Path MTU Discovery depends on ICMP "fragmentation needed" or "Packet Too Big" reaching the sender.
- Fix black holes by permitting that ICMP, clamping MSS, or probing at the packetization layer.

## Sources

- RFC 791 (IPv4 fragmentation); RFC 1191, Path MTU Discovery; RFC 8200 §4.5 and §5 (IPv6); RFC 8201, Path MTU Discovery for IPv6.
- RFC 9293 (TCP MSS option); RFC 4821, Packetization Layer Path MTU Discovery; RFC 8899, Datagram PLPMTUD.
- RFC 9000 §14 (QUIC datagram size).
