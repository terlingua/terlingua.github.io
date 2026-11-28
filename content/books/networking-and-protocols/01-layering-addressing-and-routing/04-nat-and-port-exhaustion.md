---
title: "NAT, Port Mappings and Port Exhaustion"
questions:
  - id: "reply-translation"
    type: "single"
    prompt: "Host `10.0.0.5` connects from port `51000` to `198.51.100.9:443`. The NAT, whose public address is `203.0.113.1`, maps the connection to public port `40001`. How does the server's reply look when it reaches the NAT, and what does the NAT do with it?"
    explanation: |-
      A NAPT (traditional NAT with port translation, RFC 3022) keeps a mapping table. Outbound, it rewrites the *source* of `10.0.0.5:51000` to `203.0.113.1:40001` and recomputes the IP and TCP checksums. The server only ever sees the public pair, so it replies to `203.0.113.1:40001`. Inbound, the NAT looks that pair up and rewrites the *destination* back to `10.0.0.5:51000`.

      | Direction | Before NAT | After NAT |
      |---|---|---|
      | Out | `10.0.0.5:51000 → 198.51.100.9:443` | `203.0.113.1:40001 → 198.51.100.9:443` |
      | In | `198.51.100.9:443 → 203.0.113.1:40001` | `198.51.100.9:443 → 10.0.0.5:51000` |

      "The server learned the private address" is wrong: the NAT rewrote the SYN's source before it left, and private addresses are not routable on the Internet anyway.

      "Rewrites only the address" describes one-to-one NAT without ports; with many hosts behind one address, the port is what tells connections apart, so the server sees port 40001, not 51000.

      "Forwards it unchanged" fails because `10.0.0.5`'s TCP stack has no connection addressed to `203.0.113.1:40001`; without reverse translation the segment would match nothing.
    options:
      - text: "It arrives as `198.51.100.9:443 → 10.0.0.5:51000`, because the server learned the private address from the SYN"
      - text: "It arrives as `198.51.100.9:443 → 203.0.113.1:40001`; the NAT rewrites the destination to `10.0.0.5:51000`"
        correct: true
      - text: "It arrives as `198.51.100.9:443 → 203.0.113.1:51000`; the NAT rewrites only the destination address, not the port"
      - text: "It arrives as `198.51.100.9:443 → 203.0.113.1:40001`; the NAT forwards it unchanged and the host accepts it"
  - id: "unsolicited-inbound"
    type: "single"
    prompt: "Behind a home NAT, a developer runs a server on `192.168.1.20:8080` and gives a colleague the URL `http://203.0.113.1:8080/` (the NAT's public address). No port forwarding is configured. What happens to the colleague's SYN?"
    explanation: |-
      NAT state is created by outbound packets. An inbound packet is translated only if it matches an existing mapping (or a static port-forwarding rule). A fresh SYN to `203.0.113.1:8080` matches nothing, so the NAT has no internal destination to rewrite to and discards it (or answers with a RST or ICMP, depending on the device). This is why NAT behaves like a stateful firewall for inbound connections, and why peer-to-peer applications need hole punching or relays.

      "Most recently active host" is not a NAT behavior; guessing would send traffic to the wrong machine.

      "Broadcasts it on the LAN" invents a discovery step; NATs translate unicast packets via a table.

      "Translates only outbound packets" misses half of the mechanism: replies are translated inbound too, but only for mappings that already exist.
    options:
      - text: "The NAT forwards it to the internal host that most recently sent traffic"
      - text: "The NAT broadcasts it on the LAN and the host listening on 8080 answers"
      - text: "The NAT finds no mapping for port 8080 and drops or rejects it"
        correct: true
      - text: "It reaches `192.168.1.20`, since NAT translates only outbound packets"
  - id: "port-exhaustion-rate"
    type: "single"
    prompt: "A service behind a NAT with one public IPv4 address opens a new TCP connection for every call to one API endpoint (`203.0.113.80:443`). The NAT allocates source ports from 1024–65535 and holds each mapping for 240 seconds in total. What is the highest sustained rate of new connections before ports to that endpoint run out?"
    explanation: |-
      Every simultaneous connection to the same destination (IP, port, protocol) from the same public address must use a distinct public source port, or the 5-tuples would collide. So the NAT can hold at most 65535 − 1024 + 1 = 64,512 mappings to that destination. By Little's law, mappings in use = arrival rate × holding time. Setting 64,512 = rate × 240 s gives rate = 64,512 / 240 ≈ 268.8 per second.

      "64,512 per second" treats a port as instantly reusable; each one stays allocated for the whole 240 s.

      "117 per second" divides the Linux default ephemeral range (32768–60999, 28,232 ports) by 240; that range limits a host's own source ports, but here the NAT chooses the public ports from 1024–65535.

      "537 per second" uses a 120-second hold time instead of the stated 240 seconds.
    options:
      - text: "About 64,512 new connections per second"
      - text: "About 117 new connections per second"
      - text: "About 537 new connections per second"
      - text: "About 268 new connections per second"
        correct: true
  - id: "nat-port-mitigations"
    type: "multiple"
    prompt: "The service above hits NAT port allocation failures at peak. Which changes increase its headroom? Select all that apply."
    explanation: |-
      Port demand = new connections per second × holding time, per public address and destination. The fixes reduce the first factor or add capacity.

      - Connection reuse turns thousands of short connections into a few long ones, cutting the new-connection rate by orders of magnitude.
      - Each extra public address brings another 64,512 ports per destination.

      Distractors:

      - "Over UDP" fails because UDP mappings also consume a public port per (internal address, internal port), with their own timeouts (RFC 4787 asks for at least two minutes).
      - "Lower the MSS" changes segment size, not the number of connections or their lifetime.
      - "Raise the receive window" might shorten a long transfer slightly, but the mapping is held well after the connection closes; it does not address the per-call connection pattern.
    options:
      - text: "Send the same calls over UDP, since UDP traffic does not need NAT port mappings"
      - text: "Lower the MSS so that each connection carries fewer bytes per segment"
      - text: "Raise the TCP receive window so that connections finish sooner"
      - text: "Reuse connections with HTTP keep-alive or a client connection pool"
        correct: true
      - text: "Attach additional public IPv4 addresses to the NAT"
        correct: true
  - id: "cgnat-shared-space"
    type: "single"
    prompt: "Which IPv4 block is reserved as shared address space between a provider's carrier-grade NAT and its customers' edge routers?"
    explanation: |-
      RFC 6598 reserves `100.64.0.0/10` (4,194,304 addresses) for exactly this purpose. A carrier could not safely number the link between its CGN and customer routers from RFC 1918 space, because customers already use RFC 1918 ranges inside their homes; overlapping prefixes would make routing ambiguous. A separate block avoids the collision.

      "`172.16.0.0/12`" is one of the three RFC 1918 private ranges (with `10.0.0.0/8` and `192.168.0.0/16`) used inside customer networks.

      "`169.254.0.0/16`" is IPv4 link-local (RFC 3927), used when no address is configured; it is never routed.

      "`192.0.2.0/24`" is TEST-NET-1, reserved for documentation (RFC 5737) and used in examples like this book's.
    options:
      - text: "`172.16.0.0/12`"
      - text: "`100.64.0.0/10`"
        correct: true
      - text: "`169.254.0.0/16`"
      - text: "`192.0.2.0/24`"
  - id: "ipv6-bypasses-cgn"
    type: "single"
    prompt: "A mobile carrier gives each subscriber an IPv6 prefix and puts the subscriber's IPv4 traffic behind a carrier-grade NAT. Why does traffic to IPv6-enabled services reduce pressure on the CGN's port pool?"
    explanation: |-
      Carrier-grade NAT exists because there are not enough public IPv4 addresses: many subscribers share one public IPv4 address, and their connections compete for that address's ports. IPv6 gives every subscriber globally routable addresses, so IPv6 flows go around the NAT44 entirely. Every connection that goes over IPv6 is one fewer port mapping on the CGN.

      "Fewer IPv6 packets per second" confuses packet rate with mapping count; port exhaustion depends on concurrent mappings, not packet size.

      "IPv6 NAT reuses ports" assumes the carrier translates IPv6; in this design there is no IPv6 NAT at all.

      "IPv6 connections are shorter" is invented; the IP version does not change how long applications keep connections open.
    options:
      - text: "IPv6 headers are larger, so fewer IPv6 packets per second reach the CGN for each connection it translates"
      - text: "IPv6 NAT reuses the same public port for many subscribers, unlike NAT44, which needs one port per connection"
      - text: "IPv6 connections are routed natively without IPv4 translation, so they use none of the shared public IPv4 ports"
        correct: true
      - text: "IPv6 connections are shorter on average, so each CGN mapping is released sooner after the connection closes"
---
## Learning objectives

- Trace how a NAPT rewrites addresses and ports in both directions.
- Explain why unsolicited inbound connections fail behind NAT.
- Compute the connection rate at which a NAT runs out of ports for one destination.
- Explain carrier-grade NAT and how IPv6 relieves it.

## The invariant

A NAT with port translation (NAPT, "traditional NAT", RFC 3022) lets many hosts share one public address by keeping a **mapping table**. When an internal host sends the first packet of a flow, the NAT picks a public port and records `(internal address, internal port, protocol) ↔ (public address, public port)`. It then:

- **Outbound:** rewrites the source address and port to the public pair and recomputes the IP and transport checksums.
- **Inbound:** looks up the destination (public address, public port); if a mapping exists, rewrites the destination back to the internal pair. If none exists, the packet has nowhere to go and is dropped or rejected.

Mappings expire after idle timeouts. RFC 4787 asks NATs to keep UDP mappings for at least two minutes; RFC 5382 asks for at least 2 hours 4 minutes for established TCP connections. Many devices also keep a mapping for a while after a TCP connection closes.

The scarce resource is ports. Two simultaneous connections from the same public address to the same destination (address, port, protocol) must use different public source ports, or their 5-tuples would collide. With public ports 1024–65535, that is at most 64,512 concurrent mappings per public address per destination. Concurrent mappings equal the new-connection rate multiplied by how long each mapping is held (Little's law), so short-lived connections opened at a high rate exhaust ports long before bandwidth runs out.

**Carrier-grade NAT** (CGN, also called NAT444) applies the same idea at an ISP: subscribers' home NATs sit behind the carrier's NAT. The link between them uses `100.64.0.0/10`, the shared address space of RFC 6598, so it cannot collide with the RFC 1918 ranges (`10.0.0.0/8`, `172.16.0.0/12`, `192.168.0.0/16`) inside homes. A CGN often hands each subscriber a block of ports: with 1024 ports per subscriber, one public address serves 64,512 / 1024 = 63 subscribers.

## Worked example

A worker pool calls one payment API at `203.0.113.80:443` through a NAT with one public address. It opens a new connection per call, 300 calls per second, and the NAT holds each mapping for 240 seconds in total.

| Quantity | Value |
|---|---|
| Ports available per destination | 65535 − 1024 + 1 = 64,512 |
| Mappings needed | 300 × 240 = 72,000 |
| Maximum sustainable rate | 64,512 / 240 ≈ 268.8 per second |

At 300 calls per second the NAT runs out of ports and new connection attempts fail, even though bandwidth and CPU are fine. Reusing connections (HTTP keep-alive, a pooled client) collapses the demand to a handful of long-lived mappings; adding public addresses multiplies capacity. A host's own ephemeral range matters as well: Linux defaults to 32768–60999, 28,232 ports per destination, which the same arithmetic applies to.

## Where this shows up today

As of November 2026, Google's IPv6 measurement first exceeded 50% of users on 28 March 2026 (Internet Society Pulse), while APNIC Labs measured about 42% IPv6 capability in April 2026; the gap reflects different methods. The rest of IPv4 traffic increasingly crosses CGNs. In cloud networks the same limit is documented: AWS NAT gateways support up to 55,000 simultaneous connections per unique destination per IPv4 address, with up to eight addresses per gateway.

## Pitfalls checklist

- Opening a new TCP connection per request through a NAT and discovering port exhaustion at peak traffic.
- Expecting inbound connections to reach a host behind NAT without forwarding, hole punching or a relay.
- Using RFC 1918 space for infrastructure that must coexist with customers' private networks.
- Assuming UDP needs no NAT state; it needs mappings and keepalives within the timeout.
- Treating "the client IP" in server logs as one user when many share a CGN address.

## Key takeaways

- NAPT rewrites source on the way out and destination on the way back, using a mapping table.
- No mapping, no inbound delivery.
- Ports per destination per public address cap concurrency: rate × hold time must stay below 64,512.
- Carrier-grade NAT stretches IPv4; IPv6 traffic bypasses it entirely.

## Sources

- RFC 3022, Traditional IP Network Address Translator; RFC 4787 (UDP) and RFC 5382 (TCP) NAT behavioral requirements.
- RFC 1918, Address Allocation for Private Internets; RFC 6598, Shared Address Space (100.64.0.0/10).
- Internet Society Pulse, "18 years later, IPv6 reaches majority" (2026); APNIC blog, "Google hits 50% IPv6" (28 April 2026).
- AWS, Amazon VPC User Guide, "NAT gateway basics", read November 2026.
