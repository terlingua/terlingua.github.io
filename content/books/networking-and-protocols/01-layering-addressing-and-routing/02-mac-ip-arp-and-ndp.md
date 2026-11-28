---
title: "MAC Addresses, IP Addresses, ARP and Neighbor Discovery"
questions:
  - id: "frame-after-router"
    type: "single"
    prompt: "Host A (`192.0.2.10/24`, MAC `aa:aa:aa:00:00:01`) sends a packet to `203.0.113.5`. Its gateway R has LAN MAC `00:00:5e:00:01:01` and WAN MAC `02:00:00:00:00:02`; R's next hop N has MAC `02:00:00:00:00:03`. There is no NAT. Which addresses does the frame leaving R toward N carry?"
    explanation: |-
      A MAC address is only meaningful on one link; an IP address identifies the endpoints for the whole path. A router receives the frame, discards the link-layer header, decrements the TTL, chooses the next hop, and builds a new frame with *its own outgoing interface* as source MAC and the *next hop* as destination MAC. The IP addresses are untouched.

      Trace:

      | Link | Source MAC | Destination MAC | Source IP | Destination IP |
      |---|---|---|---|---|
      | A to R | `aa:aa:aa:00:00:01` | `00:00:5e:00:01:01` | `192.0.2.10` | `203.0.113.5` |
      | R to N | `02:00:00:00:00:02` | `02:00:00:00:00:03` | `192.0.2.10` | `203.0.113.5` |

      Keeping A's MAC as the source is wrong because A's MAC means nothing on R's WAN link; frames are rebuilt per hop.

      Keeping the first frame and putting N's IP as the destination confuses the two address types: the next hop is reached by MAC, while the destination IP never changes.

      Rewriting the source IP to R's WAN address is what a NAT does; the question rules NAT out, and plain routing never changes IP addresses.
    options:
      - text: "Source MAC `aa:aa:aa:00:00:01`, destination MAC `02:00:00:00:00:03`; source IP `192.0.2.10`, destination IP `203.0.113.5`"
      - text: "Source MAC `aa:aa:aa:00:00:01`, destination MAC `00:00:5e:00:01:01`; source IP `192.0.2.10`, destination IP N's address"
      - text: "Source MAC `02:00:00:00:00:02`, destination MAC `02:00:00:00:00:03`; source IP R's WAN address, destination IP `203.0.113.5`"
      - text: "Source MAC `02:00:00:00:00:02`, destination MAC `02:00:00:00:00:03`; source IP `192.0.2.10`, destination IP `203.0.113.5`"
        correct: true
  - id: "arp-for-gateway"
    type: "single"
    prompt: "Host `192.0.2.10/24` with default gateway `192.0.2.1` sends its first packet to `203.0.113.5`. Its ARP cache is empty. Which IPv4 address does it put in the ARP request?"
    explanation: |-
      Before sending, a host compares the destination with its own prefix. `203.0.113.5` is not in `192.0.2.0/24`, so the packet is off-link and goes to the next hop from the routing table, the default gateway. ARP (RFC 826) only resolves addresses on the local link, so the host asks for the gateway's MAC. The IP header still carries `203.0.113.5` as the destination.

      "ARP always resolves the final destination" is the classic misconception; the remote host is not on this link, so nobody would answer (unless a router were doing proxy ARP, which is a special configuration).

      "`192.0.2.255`" mixes up the *link-layer* broadcast used to send the request (`ff:ff:ff:ff:ff:ff`) with the IP broadcast address; the request asks about the gateway's address.

      "Its own address before every new destination" describes duplicate-address probing (RFC 5227), which happens when an address is configured, not per destination.
    options:
      - text: "`192.0.2.1`, because the destination is off-link and the frame goes to the gateway"
        correct: true
      - text: "`203.0.113.5`, because ARP always resolves the MAC of the final destination"
      - text: "`192.0.2.255`, because the request is sent to the subnet's broadcast address"
      - text: "`192.0.2.10`, because a host checks for its own address before every new destination"
  - id: "solicited-node-address"
    type: "single"
    prompt: "An IPv6 host wants the link-layer address of neighbor `2001:db8::5:a1b2:c3d4`. To which IPv6 destination does it send the Neighbor Solicitation?"
    explanation: |-
      IPv6 has no broadcast. RFC 4861 Neighbor Discovery sends Neighbor Solicitations for address resolution to the target's *solicited-node multicast address*: the prefix `ff02::1:ff00:0/104` followed by the low-order 24 bits of the target address.

      The target ends in `...:a1b2:c3d4`. Its low 24 bits are `b2:c3d4` (the last three bytes: `b2`, `c3`, `d4`). Result: `ff02::1:ffb2:c3d4`. On Ethernet this maps to MAC `33:33:ff:b2:c3:d4`, so only hosts whose addresses share those 24 bits receive it.

      "`ff02::1:ffa1:b2c3`" takes the wrong three bytes (it skips the last byte and includes `a1`).

      "`ff02::1`" is the all-nodes address; using it would be broadcast in all but name, which is what solicited-node multicast avoids.

      "`ff02::2`" is all-routers, used for Router Solicitations, not for resolving a neighbor.
    options:
      - text: "`ff02::1:ffa1:b2c3`"
      - text: "`ff02::1:ffb2:c3d4`"
        correct: true
      - text: "`ff02::1`"
      - text: "`ff02::2`"
  - id: "arp-exchange-facts"
    type: "multiple"
    prompt: "Which statements about an ordinary ARP request and reply on Ethernet are true? Select all that apply."
    explanation: |-
      RFC 826 ARP is a link-layer protocol with its own EtherType (`0x0806`). The requester does not know the target's MAC, so it broadcasts the request; only the owner of the target IPv4 address replies, and it knows the requester's MAC from the request, so the reply is unicast. RFC 826's packet-reception algorithm also has every receiver update an *existing* table entry for the sender's protocol address with the sender's hardware address, which is why caches self-correct when a request is seen.

      "Carried inside IPv4 packets" is wrong: ARP sits directly in the Ethernet frame, which is exactly why it cannot leave the link.

      "A router forwards an ARP request" is wrong for the same reason. A router answers only for its own addresses (or, with proxy ARP configured, on behalf of others), and never relays ARP.
    options:
      - text: "ARP messages are carried inside IPv4 packets, so they can cross routers"
      - text: "The request is sent to the link-layer broadcast address `ff:ff:ff:ff:ff:ff`"
        correct: true
      - text: "The reply is normally sent unicast to the requester's MAC address"
        correct: true
      - text: "A host that already has a cache entry for the requester may refresh it from the request"
        correct: true
      - text: "A router forwards an ARP request toward the subnet that owns the target address"
  - id: "failover-stale-arp"
    type: "single"
    prompt: "Two servers share virtual IP `192.0.2.50` in an active/passive pair. After failover, the passive server configures `192.0.2.50` on its own NIC (a different MAC). Clients on the same subnet and traffic arriving through the gateway keep failing for several minutes, then recover on their own. What is the most likely cause and fix?"
    explanation: |-
      The IP address did not change; only the MAC behind it did. Every device on the link that talked to `192.0.2.50` (the gateway above all) holds an ARP cache entry pointing at the old MAC, and keeps sending frames there until the entry ages out. That matches "fails for minutes, then recovers on its own".

      The fix is for the new owner to announce the binding: a gratuitous ARP (an ARP request or reply for its own address). RFC 826's rule that receivers update existing entries for the sender makes every neighbor overwrite the stale entry at once. VRRP and keepalived-style failover do this automatically.

      "DNS still maps the name to the old server" fails because DNS maps names to IP addresses, and the IP address is the same before and after failover.

      "TTL no longer matches" misreads TTL: it is a hop counter in each packet, not a per-server value that routers compare.

      "TCP sequence numbers" describes a real consequence for *existing* connections (the new server will reset them), but it does not explain why *new* connections fail for minutes and then recover.
    options:
      - text: "DNS still maps the service name to the old server; lower the record's TTL so clients re-resolve faster"
      - text: "Routers drop packets whose TTL no longer matches the new server; clients should send with a higher TTL"
      - text: "TCP sequence numbers no longer match the new server; clients should shorten their retransmission timeout"
      - text: "Neighbors and the gateway still cache `192.0.2.50` with the old MAC; the new server should send gratuitous ARP announcements on takeover"
        correct: true
  - id: "ttl-after-three-routers"
    type: "single"
    prompt: "A host sends an IPv4 packet with TTL 64 to a destination three routers away. What TTL does the destination see, and what else did each router change in the IPv4 header?"
    explanation: |-
      RFC 791 requires every router that forwards a packet to decrement the TTL (in practice by one per hop) and to discard the packet when it reaches zero. Three routers give 64 − 3 = 61. Because the TTL is covered by the IPv4 header checksum, each router must also update the checksum. (IPv6, RFC 8200, has a Hop Limit with the same per-hop decrement but no header checksum at all.)

      "64" confuses TTL with fragmentation; decrementing is unconditional for every forwarded packet.

      "replaced the source address" describes NAT, not routing.

      "60" adds a decrement at the destination; hosts deliver packets that arrive with TTL ≥ 1 without decrementing, because they are not forwarding.
    options:
      - text: "61, and each router recomputed the IPv4 header checksum"
        correct: true
      - text: "64, because TTL is only decremented when a router fragments the packet"
      - text: "61, and each router replaced the source address with its own address"
      - text: "60, because the destination also decrements TTL before delivering the packet"
---
## Learning objectives

- Explain why MAC addresses change at every hop while IP addresses stay the same end to end.
- Decide whether a destination is on-link and which address a host resolves.
- Describe ARP's request/reply exchange and IPv6 Neighbor Discovery's solicited-node multicast.
- Diagnose stale neighbor caches after an address moves.

## The invariant

Two kinds of address do two different jobs. A **link-layer address** (an Ethernet MAC) names an interface on *one link*; it is used to get a frame across that link and nowhere else. A **network-layer address** (IPv4 or IPv6) names the endpoints for the *whole path*. Routers forward by IP address; switches forward by MAC address.

Sending a packet follows the same steps on every host and router:

1. **On-link or off-link?** Compare the destination with the prefixes of the local interfaces. If it falls inside one, the next hop is the destination itself; otherwise look up the routing table, usually ending at the default gateway.
2. **Resolve the next hop's link-layer address.** IPv4 uses ARP (RFC 826): a request broadcast to `ff:ff:ff:ff:ff:ff` asks "who has 192.0.2.1?", and the owner unicasts a reply. IPv6 uses Neighbor Discovery (RFC 4861): a Neighbor Solicitation goes to the target's *solicited-node multicast address* (`ff02::1:ff` plus the target's low 24 bits) and the owner answers with a Neighbor Advertisement. IPv6 has no broadcast at all.
3. **Build a frame** with the local interface as source MAC and the next hop as destination MAC, and send it.

A router repeats steps 1–3 for each packet it forwards, after discarding the incoming frame and decrementing the TTL (IPv4) or Hop Limit (IPv6). The IP header keeps the original source and destination (unless a NAT, covered later, rewrites them).

Results are cached. ARP caches and the IPv6 neighbor cache keep entries for minutes; RFC 4861 defines reachability states (INCOMPLETE, REACHABLE, STALE, DELAY, PROBE) and probes neighbors that stop confirming reachability. RFC 826 has every receiver refresh an existing entry for the sender of a request, which is what makes a *gratuitous ARP*, an announcement of your own binding, repair stale caches across a link.

## Worked example

Host A (`192.0.2.10/24`) sends to `203.0.113.5` through gateway R and then router N.

| Link | Source MAC | Destination MAC | Source IP | Destination IP | TTL |
|---|---|---|---|---|---|
| A to R | A | R (LAN side) | `192.0.2.10` | `203.0.113.5` | 64 |
| R to N | R (WAN side) | N | `192.0.2.10` | `203.0.113.5` | 63 |
| N to next | N (outgoing) | next hop | `192.0.2.10` | `203.0.113.5` | 62 |

Three routers later the destination sees TTL 64 − 3 = 61. The ARP exchange A performs first is for the gateway, not for `203.0.113.5`:

```bash
$ tcpdump -e -nn arp
aa:aa:aa:00:00:01 > ff:ff:ff:ff:ff:ff, ARP, Request who-has 192.0.2.1 tell 192.0.2.10
00:00:5e:00:01:01 > aa:aa:aa:00:00:01, ARP, Reply 192.0.2.1 is-at 00:00:5e:00:01:01
```

For IPv6 neighbor `2001:db8::5:a1b2:c3d4`, the solicitation goes to `ff02::1:ffb2:c3d4` (Ethernet `33:33:ff:b2:c3:d4`), so only hosts sharing those 24 bits are interrupted.

## Where this shows up today

As of November 2026, RFC 4861 is still the Neighbor Discovery specification, updated by a dozen later RFCs but never replaced. Virtualized networks often answer ARP and neighbor solicitations inside the hypervisor's virtual switch rather than flooding them, but hosts still run the same resolve-then-frame steps, so stale-cache failures after an address moves look the same.

## Pitfalls checklist

- Expecting to see the remote server's MAC in a capture; you only ever see the next hop's.
- Moving a virtual IP between machines without sending gratuitous ARP or unsolicited Neighbor Advertisements.
- Blaming DNS for failures after failover when the IP address never changed.
- Blocking all ICMPv6 on IPv6 links; Neighbor Discovery runs over ICMPv6.
- Assuming routers relay ARP; ARP never leaves its link.

## Key takeaways

- MAC addresses are rebuilt at every hop; IP addresses are end to end (absent NAT).
- Off-link traffic is framed to the gateway's MAC after resolving the gateway, not the destination.
- ARP broadcasts requests and unicasts replies; IPv6 replaces broadcast with solicited-node multicast.
- Neighbor caches go stale when an address moves; announcements fix them immediately.

## Sources

- RFC 826, An Ethernet Address Resolution Protocol; RFC 5227, IPv4 Address Conflict Detection.
- RFC 4861, Neighbor Discovery for IP version 6.
- RFC 791 (IPv4); RFC 8200 (IPv6).
