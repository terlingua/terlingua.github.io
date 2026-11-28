---
title: "Record Types and the CNAME Rule"
questions:
  - id: "ptr-name"
    type: "single"
    prompt: "Under which owner name is the PTR record for the IPv4 address `203.0.113.25` published?"
    explanation: |-
      Reverse lookups map addresses into the DNS tree under `in-addr.arpa` (RFC 1035). DNS names are most specific on the left, while IPv4 addresses are most specific on the right, so the octets are reversed: `203.0.113.25` becomes `25.113.0.203.in-addr.arpa`. Reversing also lets the owner of `203.0.113.0/24` be delegated the zone `113.0.203.in-addr.arpa`.

      "`203.0.113.25.in-addr.arpa`" keeps the address order, which would put the least significant octet at the top of the tree.

      "`ip6.arpa`" is the IPv6 reverse tree, where each of the 32 hex nibbles becomes a label.

      "`113.0.203.in-addr.arpa`" is the reverse *zone* for the /24, not the name of an individual address's PTR record.
    options:
      - text: "`203.0.113.25.in-addr.arpa`"
      - text: "`25.113.0.203.in-addr.arpa`"
        correct: true
      - text: "`25.113.0.203.ip6.arpa`"
      - text: "`113.0.203.in-addr.arpa`"
  - id: "mx-preference"
    type: "single"
    prompt: "`example.com` has three MX records: `MX 20 mx2.example.com.`, `MX 10 mx1.example.com.` and `MX 30 mx3.example.com.` Which server does a sending mail server try first?"
    explanation: |-
      An MX record holds a preference and a mail exchanger name; lower preference values are more preferred (RFC 1035, with SMTP's use defined in RFC 5321). Senders try the lowest value first and fall back to higher values. Equal values share load randomly.

      "The highest preference value" reverses the ordering; the field is a cost, not a priority score.

      "Listed first in the zone file" assumes order matters; record order in a set is not significant, and servers may return them in any order.

      "At random with weights" describes SRV weights, not MX preferences.
    options:
      - text: "`mx3.example.com`, because it has the highest preference value"
      - text: "`mx2.example.com`, because it is listed first in the zone file"
      - text: "`mx1.example.com`, because it has the lowest preference value"
        correct: true
      - text: "Any of the three, chosen at random with weights by preference"
  - id: "srv-weights"
    type: "single"
    prompt: "`_sip._tcp.example.com` has three SRV records: `10 60 5060 big.example.com.`, `10 20 5060 small.example.com.` and `20 0 5060 backup.example.com.` (priority, weight, port, target). While all servers are up, what share of new connections should go to `big.example.com`?"
    explanation: |-
      RFC 2782: clients use the records with the lowest *priority* first, and within that priority choose targets randomly in proportion to their *weight*. `backup` has priority 20 and is only used if both priority-10 targets fail.

      Share for `big` = 60 / (60 + 20) = 0.75 = 75%.

      "60%" reads the weight as a percentage; weights are relative.

      "100%" sends everything to the heaviest target, ignoring proportional selection.

      "50%" splits evenly, as if weights did not matter.
    options:
      - text: "60%"
      - text: "100%"
      - text: "50%"
      - text: "75%"
        correct: true
  - id: "cname-plus-txt-bug"
    type: "single"
    prompt: "The zone has `www.example.com. CNAME lb.cdn.example.net.`. To verify domain ownership, an engineer adds `www.example.com. TXT \"verify=abc\"`. What is the problem?"
    explanation: |-
      RFC 2181 §10.1: a name either has one CNAME record (plus DNSSEC records) or other data, never both. A CNAME means "this name is an alias; ask the canonical name instead", for every type. If the alias also had a TXT record, a resolver could not know whether to answer with that TXT or follow the alias to the canonical name's TXT. Most authoritative servers refuse to load or accept such a zone; where one slips through, behavior differs between resolvers.

      The fix is to put the verification record on a different name (many services offer a `_verify` label) or on the CNAME's target.

      "Nothing; TXT queries get the TXT" describes what the engineer hoped for, which the rule forbids.

      "TXT overrides the CNAME" invents a precedence rule.

      "Ignored after its TTL expires" confuses validity with caching.
    options:
      - text: "A name with a CNAME may hold no other data, so this zone is invalid"
        correct: true
      - text: "Nothing; TXT queries get the TXT record while A and AAAA queries follow the CNAME to the CDN"
      - text: "The TXT record overrides the CNAME for every query type, so the website stops resolving"
      - text: "The CNAME is ignored for TXT queries only after its TTL expires, so the change is delayed"
  - id: "apex-cname"
    type: "single"
    prompt: "Why can't `example.com` itself (the zone apex) be a CNAME to `lb.cdn.example.net`, and what is the standardized way to point the apex at the CDN for clients that support it?"
    explanation: |-
      Every zone apex has an SOA record and NS records. RFC 2181 §10.1 forbids any other data at a name with a CNAME, so the apex can never be a CNAME. RFC 9460 introduced the HTTPS record; in **AliasMode** (priority 0) it points a name at another name for HTTPS service, and RFC 9460 states that a main purpose of AliasMode is aliasing at the zone apex, where CNAME is not allowed: `example.com. 3600 IN HTTPS 0 lb.cdn.example.net.`

      "Legal at the apex, browsers ignore them" is wrong on both counts, and ALIAS is not defined in RFC 9460 or any RFC.

      "Only SOA and NS at the apex" is false; the apex can hold A, AAAA, MX, TXT and more. A TTL of 0 is legal but unrelated to the problem.

      "Allowed if its TTL equals the SOA minimum" invents a rule; TTL does not affect the coexistence ban.
    options:
      - text: "CNAMEs are legal at the apex, but browsers ignore them there; an ALIAS record, defined in RFC 9460, fixes that"
      - text: "The apex must hold SOA and NS records, which cannot coexist with a CNAME; an HTTPS record in AliasMode can alias the apex"
        correct: true
      - text: "The apex may hold only SOA and NS records, so the CDN's address goes in an A record with a TTL of 0"
      - text: "A CNAME at the apex is allowed if its TTL equals the SOA minimum; otherwise an MX record must be used"
  - id: "alias-flattening"
    type: "multiple"
    prompt: "Several DNS providers offer an \"ALIAS\" or \"ANAME\" record for the zone apex. Which statements describe how these work? Select all that apply."
    explanation: |-
      ALIAS/ANAME is *flattening* inside the authoritative server. The provider looks up the target's A and AAAA records (on a schedule or per query) and serves them as if they were the apex's own records. Resolvers see plain address records, so the CNAME rule is never touched. The IETF's ANAME draft (draft-ietf-dnsop-aname) expired in 2019 without becoming an RFC, so the feature is provider-specific: refresh intervals, TTLs, and whether geolocation-aware answers survive flattening all vary.

      "Resolvers follow the alias themselves" describes CNAME (or HTTPS AliasMode), not flattening.

      "A real CNAME if DNSSEC is off" is false; the coexistence rule has nothing to do with DNSSEC beyond allowing DNSSEC records next to a CNAME.

      "The exception RFC 2181 defines" is false; §10.1 allows only DNSSEC records alongside a CNAME.
    options:
      - text: "Resolvers receive a CNAME-like record from the provider and follow the alias themselves"
      - text: "They let the apex keep a real CNAME record as long as DNSSEC is not enabled for the zone"
      - text: "The provider's servers resolve the target name and answer with ordinary A and AAAA records"
        correct: true
      - text: "They are not a standard record type, so their behavior differs between DNS providers"
        correct: true
      - text: "They are the exception to the CNAME rule that RFC 2181 §10.1 itself defines for the apex"
---
## Learning objectives

- Read and write the common record types, including MX, SRV, SOA and PTR.
- Build reverse-lookup names for IPv4 and IPv6.
- State the CNAME coexistence rule and its consequence at the zone apex.
- Compare ALIAS/ANAME flattening with HTTPS AliasMode.

## The invariant

A DNS record is (owner name, type, class, TTL, data). The types used every day:

| Type | Data | Notes |
|---|---|---|
| A / AAAA | IPv4 / IPv6 address | AAAA from RFC 3596 |
| CNAME | canonical name | the owner is an alias; ask the target instead |
| MX | preference, mail exchanger | lower preference tried first |
| TXT | one or more strings | each string at most 255 bytes |
| SRV | priority, weight, port, target | owner `_service._proto.name` (RFC 2782) |
| NS | name server name | delegation; must not be an alias |
| PTR | name | reverse mapping under `in-addr.arpa` / `ip6.arpa` |
| SOA | primary server, contact, serial, refresh, retry, expire, minimum | one per zone, at the apex |

Reverse names put the most specific part on the left: IPv4 octets reversed under `in-addr.arpa`, IPv6 nibbles reversed under `ip6.arpa`.

**The CNAME rule** (RFC 2181 §10.1): a name has either a single CNAME (plus DNSSEC records) or other data, never both. A CNAME redirects lookups of *every* type, so any other record at the same name would be ambiguous. Two consequences follow:

- **No CNAME at the zone apex.** The apex always has SOA and NS records.
- **NS and MX targets must not be aliases** (RFC 2181 §10.3); they must name hosts with address records.

Two workarounds exist for pointing an apex at a service whose addresses you do not control:

- **ALIAS/ANAME flattening**, a provider feature: the authoritative server resolves the target and answers with A/AAAA records. Not standardized; behavior varies.
- **HTTPS/SVCB AliasMode** (RFC 9460): `example.com. HTTPS 0 target.` is a standard record that clients supporting it follow, alongside the apex's ordinary A/AAAA records for clients that do not.

## Worked example

A small zone that follows the rules:

```bash
$ORIGIN example.com.
@        3600  IN SOA   ns1.example.com. hostmaster.example.com. 2026112701 7200 900 1209600 300
@        3600  IN NS    ns1.example.com.
@        3600  IN NS    ns2.example.net.
@        300   IN A     192.0.2.80
@        300   IN HTTPS 0 lb.cdn.example.net.
@        3600  IN MX    10 mx1.example.com.
@        3600  IN MX    20 mx2.example.com.
@        3600  IN TXT   "v=spf1 mx -all"
www      300   IN CNAME lb.cdn.example.net.
_verify.www 300 IN TXT  "verify=abc"
mx1      3600  IN A     192.0.2.25
mx2      3600  IN A     198.51.100.25
ns1      3600  IN A     192.0.2.53
_sip._tcp 3600 IN SRV   10 60 5060 big.example.com.
_sip._tcp 3600 IN SRV   10 20 5060 small.example.com.
```

The apex has SOA, NS, A, HTTPS, MX and TXT, which is fine because none is a CNAME. `www` is a CNAME and has nothing else, so the ownership token lives at `_verify.www`. The reverse name for `192.0.2.80` is `80.2.0.192.in-addr.arpa`. Among the SRV records, `big` gets 60 / (60 + 20) = 75% of new connections.

## Where this shows up today

As of November 2026, the IETF's standardized ANAME proposal (draft-ietf-dnsop-aname) remains expired since 2019, so apex aliasing is either a provider's flattening feature or an RFC 9460 HTTPS record in AliasMode. Since HTTPS records are understood only by clients that query them, apex zones keep A/AAAA records as well.

## Pitfalls checklist

- Adding TXT or MX records to a name that is already a CNAME.
- Pointing MX or NS records at CNAMEs.
- Treating MX preference as priority where higher wins.
- Expecting an ALIAS record to behave the same at a different provider.
- Writing reverse names in forward order.

## Key takeaways

- A CNAME owns its name completely: no other data except DNSSEC records.
- The apex always holds SOA and NS, so it can never be a CNAME.
- MX: lowest preference first; SRV: lowest priority first, then proportional weights.
- Apex aliasing: provider flattening (non-standard) or HTTPS AliasMode (RFC 9460).

## Sources

- RFC 1035 (record formats, `in-addr.arpa`); RFC 3596 (AAAA, `ip6.arpa`); RFC 2782 (SRV).
- RFC 2181, Clarifications to the DNS Specification, §10.1 (CNAME and other data), §10.3 (MX and NS targets).
- RFC 9460, Service Binding and Parameter Specification via the DNS (SVCB and HTTPS RRs).
- draft-ietf-dnsop-aname-04 (expired), IETF datatracker, read November 2026.
