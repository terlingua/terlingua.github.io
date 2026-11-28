---
title: "CIDR Arithmetic, Summarization and Longest-Prefix Match"
body_checks:
  - "n = net('10.20.37.200/21'); assert str(n.network_address) == '10.20.32.0' and str(n.broadcast_address) == '10.20.39.255' and usable_hosts('10.20.32.0/21') == 2046"
  - "assert lpm('10.20.37.200', ['0.0.0.0/0', '10.0.0.0/8', '10.20.0.0/16', '10.20.32.0/20']) == '10.20.32.0/20'"
  - "assert summarize(['192.168.4.0/24', '192.168.5.0/24', '192.168.6.0/24', '192.168.7.0/24']) == ['192.168.4.0/22']"
  - "assert summarize(['192.168.5.0/24', '192.168.6.0/24']) == ['192.168.5.0/24', '192.168.6.0/24']"
  - "assert 37 // 8 * 8 == 32 and 32 + 8 - 1 == 39"
  - "assert 2 ** (64 - 48) == 65536"
  - "assert ipaddress.ip_network('10.0.0.0/28').num_addresses - 5 == 11"
questions:
  - id: "network-and-broadcast"
    type: "single"
    prompt: "What are the network address and the broadcast address of `172.16.45.130/26`?"
    correct:
      - "`172.16.45.128` and `172.16.45.191`"
    distractors:
      - "`172.16.45.128` and `172.16.45.255`"
      - "`172.16.45.64` and `172.16.45.127`"
      - "`172.16.45.128` and `172.16.45.159`"
      - "`172.16.45.130` and `172.16.45.191`"
    explanation: |-
      A /26 leaves 32 − 26 = 6 host bits, so blocks are 2^6 = 64 addresses long and start at multiples of 64 in the last octet: 0, 64, 128, 192. 130 falls in the block starting at 128, so the network address is `172.16.45.128` and the broadcast address (all host bits set) is 128 + 64 − 1 = 191.

      "`.128` and `.255`" is the /25 block (128 addresses), which uses one host bit too many.

      "`.64` and `.127`" is the block *before* the address; 130 is greater than 127.

      "`.128` and `.159`" is the /27 block (32 addresses), one bit too few.

      "`.130` and `.191`" treats the given address as the network address; a network address has all host bits zero, and 130 = 128 + 2 has a host bit set.
    checks:
      - "n = net('172.16.45.130/26'); assert (str(n.network_address), str(n.broadcast_address)) == ('172.16.45.128', '172.16.45.191')"
      - "assert str(net('172.16.45.130/25').broadcast_address) == '172.16.45.255' and str(net('172.16.45.130/27').broadcast_address) == '172.16.45.159'"
  - id: "usable-host-counts"
    type: "multiple"
    prompt: "Which statements about usable IPv4 host addresses are true? Select all that apply."
    correct:
      - "A /22 subnet has 1022 usable host addresses"
      - "A /31 on a point-to-point link can use both of its addresses"
    distractors:
      - "A /30 subnet has 4 usable host addresses"
      - "A /24 subnet has 256 usable host addresses"
      - "A /32 cannot be configured on any interface, not even a loopback interface"
    explanation: |-
      On an ordinary multi-access subnet the all-zeros host part names the network and the all-ones host part is the broadcast address, so usable = 2^(32 − prefix) − 2.

      - /22: 2^10 − 2 = 1022. True.
      - /31: RFC 3021 allows 31-bit prefixes on point-to-point links, where there is no need for network or broadcast addresses, so both addresses are usable. True.

      Distractors:

      - "/30 has 4 usable" forgets the two reserved addresses: 2^2 − 2 = 2, the classic point-to-point size before RFC 3021.
      - "/24 has 256 usable" makes the same mistake: 2^8 − 2 = 254.
      - "/32 cannot be configured" is false: /32 host routes are routinely configured on loopback interfaces and for anycast service addresses; a /32 just has no on-link neighbors.
    checks:
      - "assert usable_hosts('10.0.0.0/22') == 1022 and usable_hosts('10.0.0.0/30') == 2 and usable_hosts('10.0.0.0/24') == 254"
      - "assert ipaddress.ip_network('10.0.0.0/31').num_addresses == 2"
  - id: "summarize-four-24s"
    type: "single"
    prompt: "Which single prefix covers exactly `10.1.8.0/24`, `10.1.9.0/24`, `10.1.10.0/24` and `10.1.11.0/24` and nothing else?"
    correct:
      - "`10.1.8.0/22`"
    distractors:
      - "`10.1.8.0/21`"
      - "`10.1.8.0/23`"
      - "`10.1.0.0/20`"
    explanation: |-
      Four contiguous /24s can be summarized by a prefix two bits shorter (2^2 = 4), a /22, provided the first one starts on a /22 boundary. In the third octet, 8 is a multiple of 4, and 8, 9, 10, 11 in binary are `000010 00`, `000010 01`, `000010 10`, `000010 11`: they share the first 6 bits of the third octet, and with the 16 bits of `10.1` that gives a 22-bit prefix. Result: `10.1.8.0/22`, which spans `10.1.8.0` to `10.1.11.255`.

      "`10.1.8.0/21`" covers 8 through 15, twice the space; it would attract traffic for `10.1.12.0`–`10.1.15.255`, which these networks do not own.

      "`10.1.8.0/23`" covers only 8 and 9.

      "`10.1.0.0/20`" covers 0 through 15, four times the space.
    checks:
      - "assert summarize(['10.1.8.0/24', '10.1.9.0/24', '10.1.10.0/24', '10.1.11.0/24']) == ['10.1.8.0/22']"
      - "assert str(net('10.1.8.0/22').broadcast_address) == '10.1.11.255' and net('10.1.8.0/21').num_addresses == 2048"
  - id: "longest-prefix-pick"
    type: "single"
    prompt: "A router has these routes: `0.0.0.0/0` via A, `10.0.0.0/8` via B, `10.20.0.0/16` via C, `10.20.32.0/20` via D, `10.20.48.0/20` via E. Where does it send a packet for `10.20.47.9`?"
    correct:
      - "D: `10.20.32.0/20` contains the address and is the longest match"
    distractors:
      - "E, because `10.20.48.0/20` is the numerically closest prefix to the address"
      - "C, because `10.20.0.0/16` was the first specific route to match in table order"
      - "B, because shorter prefixes are preferred as more stable summaries"
    explanation: |-
      RFC 4632 §5.1: forwarding in the Internet is done on a longest-match basis. Among all routes whose prefix contains the destination, the one with the most prefix bits wins, regardless of table order.

      Which routes contain `10.20.47.9`?

      - `0.0.0.0/0`: yes (matches everything).
      - `10.0.0.0/8`: yes.
      - `10.20.0.0/16`: yes.
      - `10.20.32.0/20`: spans `10.20.32.0`–`10.20.47.255` (16 values of the third octet starting at 32), so yes.
      - `10.20.48.0/20`: spans `10.20.48.0`–`10.20.63.255`, so no.

      The longest match is the /20 via D.

      "E" picks by numeric closeness; containment is what counts, and 47 is outside 48–63.

      "C" applies first-match order, which is how firewall rule lists work, not routing tables.

      "B" inverts the rule; shorter prefixes are fallbacks used only when nothing more specific matches.
    checks:
      - "assert lpm('10.20.47.9', ['0.0.0.0/0', '10.0.0.0/8', '10.20.0.0/16', '10.20.32.0/20', '10.20.48.0/20']) == '10.20.32.0/20'"
  - id: "string-prefix-bug"
    type: "single"
    prompt: "An allow-list check is meant to accept only addresses in `10.1.1.0/24`:\n\n```python\ndef allowed(ip: str) -> bool:\n    return ip.startswith(\"10.1.1\")\n```\n\nWhat is wrong with it?"
    correct:
      - "It also accepts addresses outside the subnet, such as `10.1.100.5` and `10.1.15.3`"
    distractors:
      - "It rejects `10.1.1.255`, which is part of the /24 but is the broadcast address"
      - "It works for a /24 and only breaks for prefixes that do not end on an octet"
      - "It rejects `10.1.1.0`, because the network address never matches a string prefix"
    explanation: |-
      CIDR membership is a bitwise comparison of the first *prefix-length* bits, not a text comparison. The string `"10.1.1"` is also a prefix of `"10.1.10.x"` through `"10.1.19.x"` and `"10.1.100.x"` through `"10.1.199.x"`, so the check accepts 111 /24s' worth of addresses instead of one. A correct version parses and compares numerically:

      ```python
      import ipaddress
      ALLOWED = ipaddress.ip_network("10.1.1.0/24")
      def allowed(ip: str) -> bool:
          return ipaddress.ip_address(ip) in ALLOWED
      ```

      "Rejects `10.1.1.255`" is false; that string starts with `"10.1.1"` and is accepted.

      "Works for a /24" is the misconception that makes this bug common: even with an octet-aligned prefix, the text check fails because the next character is not a delimiter.

      "Rejects `10.1.1.0`" is also false; `"10.1.1.0"` starts with `"10.1.1"`.
    checks:
      - "a = ipaddress.ip_network('10.1.1.0/24'); assert '10.1.100.5'.startswith('10.1.1') and ipaddress.ip_address('10.1.100.5') not in a and ipaddress.ip_address('10.1.15.3') not in a"
      - "assert len([o for o in range(256) if str(o).startswith('1')]) == 111"
  - id: "ipv6-subnets-in-48"
    type: "single"
    prompt: "A site is assigned `2001:db8:abcd::/48` and builds every LAN as a /64. How many /64 subnets does the assignment contain?"
    correct:
      - "65,536"
    distractors:
      - "256"
      - "4,096"
      - "2^64"
    explanation: |-
      The number of /64 subnets inside a /48 is 2^(64 − 48) = 2^16 = 65,536: the 16 bits between the end of the site prefix and the start of the 64-bit interface identifier. In text form these are the fourth group, `2001:db8:abcd:0000::/64` through `2001:db8:abcd:ffff::/64`.

      "256" is 2^8, the count for a /56, a common size for a single home.

      "4,096" is 2^12, the count for a /52.

      "2^64" is the number of *addresses* in one /64, not the number of /64s.

      LANs are /64 because stateless address autoconfiguration (RFC 4862) builds a 64-bit interface identifier; that is a convention of address configuration, while routing itself works on any prefix length.
    checks:
      - "assert 2 ** (64 - 48) == 65536 and 2 ** (64 - 56) == 256 and 2 ** (64 - 52) == 4096"
      - "assert len(list(ipaddress.ip_network('2001:db8:abcd::/48').subnets(new_prefix=56))) == 256"
---

## Learning objectives

- Compute network, broadcast and usable host counts for any IPv4 prefix.
- Summarize contiguous prefixes and recognize when a summary over-claims.
- Select a route by longest-prefix match.
- Size IPv6 assignments in /64 subnets.

## The invariant

Classless Inter-Domain Routing (RFC 4632, BCP 122) writes a network as `address/length`: the first *length* bits are the network prefix and the rest are host bits. Everything else is arithmetic on that split.

- **Block size** is 2^(32 − length) addresses for IPv4 and 2^(128 − length) for IPv6. Blocks are aligned: a prefix's network address has all host bits zero.
- **Network and broadcast.** Clear the host bits to get the network address; set them all to get the IPv4 broadcast address. On a multi-access IPv4 subnet both are reserved, so usable hosts = 2^(32 − length) − 2. RFC 3021 lets point-to-point links use both addresses of a /31. IPv6 has no broadcast address.
- **Summarization (aggregation).** 2^k aligned, contiguous prefixes of length L combine into one prefix of length L − k. If they are not aligned or not contiguous, no single prefix covers them exactly, and a shorter one would claim address space you do not hold.
- **Longest-prefix match.** RFC 4632 §5.1: forwarding is done on a longest-match basis. Of all routes whose prefix contains the destination, the router uses the one with the most prefix bits. The default route `0.0.0.0/0` (or `::/0`) matches everything and therefore loses to any other match.

Longest match is what makes aggregation safe: a provider can announce one short prefix while a customer announces a more specific one, and traffic for the customer's range follows the more specific route.

IPv4 and IPv6 share these rules. Differences that matter for arithmetic: IPv4 addresses are 32 bits and the header is 20 to 60 bytes; IPv6 addresses are 128 bits, the base header is a fixed 40 bytes (RFC 8200), and LANs are conventionally /64 because stateless autoconfiguration (RFC 4862) uses a 64-bit interface identifier.

## Worked example

Address `10.20.37.200/21`. Host bits: 11. The third octet carries 3 of them, so third-octet blocks are 2^3 = 8 wide: 37 lies in 32–39.

| Item | Value |
|---|---|
| Network | `10.20.32.0` |
| Broadcast | `10.20.39.255` |
| Usable hosts | 2^11 − 2 = 2046 |

Route lookup for `10.20.37.200` with routes `0.0.0.0/0`, `10.0.0.0/8`, `10.20.0.0/16` and `10.20.32.0/20`: all four contain it, and `10.20.32.0/20` is longest, so it wins.

Summaries: `192.168.4.0/24` through `192.168.7.0/24` collapse to `192.168.4.0/22` (4 starts on a multiple of 4). `192.168.5.0/24` and `192.168.6.0/24` do not collapse to a /23, because 5 is odd; the /23 holding 5 is `192.168.4.0/23`.

```python
import ipaddress
n = ipaddress.ip_network("10.20.37.200/21", strict=False)
print(n, n.broadcast_address, n.num_addresses - 2)
print(list(ipaddress.collapse_addresses(
    ipaddress.ip_network(f"192.168.{i}.0/24") for i in range(4, 8))))
```

Output:

```bash
10.20.32.0/21 10.20.39.255 2046
[IPv4Network('192.168.4.0/22')]
```

An IPv6 site with `2001:db8:abcd::/48` has 2^(64 − 48) = 65,536 /64 LANs.

## Where this shows up today

As of November 2026, AWS documents that each VPC subnet reserves five IPv4 addresses (network, VPC router, DNS, one for future use, and broadcast), so a /28, its smallest subnet, leaves 11 usable addresses rather than 14. Cloud subnet sizing is CIDR arithmetic with a provider-specific reservation on top.

## Pitfalls checklist

- Comparing addresses as strings; `10.1.1` is a text prefix of `10.1.100.5`.
- Forgetting the two reserved IPv4 addresses (and any provider reservations) when sizing a subnet.
- Announcing or configuring a summary that is not aligned, which claims someone else's space.
- Reading a route table top-down as if first match won.
- Sizing IPv6 LANs smaller than /64 and breaking autoconfiguration.

## Key takeaways

- A prefix length splits the address; block size, network and broadcast follow from the host bits.
- 2^k aligned, contiguous prefixes summarize to a prefix k bits shorter, and nothing else does exactly.
- Routers pick the longest matching prefix; the default route is the last resort.
- Use a real address library for membership tests.

## Sources

- RFC 4632, Classless Inter-domain Routing (CIDR) (BCP 122), §5.1.
- RFC 3021, Using 31-Bit Prefixes on IPv4 Point-to-Point Links.
- RFC 8200 (IPv6); RFC 4862, IPv6 Stateless Address Autoconfiguration.
- AWS, Amazon VPC User Guide, "Subnet CIDR blocks" (reserved addresses), read November 2026.
