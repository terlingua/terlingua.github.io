---
title: "TTLs, Negative Caching and Anycast"
body_checks:
  - "assert 12 * 3600 + 3600 == 13 * 3600"
  - "assert 9 * 3600 + 3600 <= 12 * 3600 and 12 * 3600 + 300 == 12 * 3600 + 5 * 60"
  - "assert min(3600, 300) == 300"
  - "assert 2 ** 31 - 1 == 2147483647"
questions:
  - id: "worst-case-staleness"
    type: "single"
    prompt: "An A record with TTL 86400 is changed on the authoritative servers at 10:00 on Monday, with no preparation. Ignoring serve-stale and assuming caches honor TTLs, what is the latest time a resolver may still hand out the old address?"
    correct:
      - "10:00 on Tuesday"
    distractors:
      - "10:00 on Monday, because the authoritative change is visible at once"
      - "11:00 on Monday, because resolvers re-check records every hour"
      - "10:00 on Wednesday, because the browser cache adds a second TTL"
    explanation: |-
      A resolver that fetched the record just before 10:00 on Monday may cache it for its full TTL of 86,400 seconds (24 hours) and answer from cache until 10:00 on Tuesday. DNS has no invalidation message; the TTL is the only bound.

      "Visible at once" is true only for resolvers that did not have the record cached.

      "Re-check every hour" invents a refresh interval; resolvers re-query only when the TTL runs out.

      "A second TTL from the browser" assumes downstream caches restart the clock. A compliant resolver hands out the *remaining* TTL, so a stub or browser cache that stores the answer expires it at the same moment the resolver would; the bound stays one TTL from when the authoritative data changed.
    checks:
      - "assert 86400 == 24 * 3600"
  - id: "lower-ttl-before-cutover"
    type: "single"
    prompt: "A record currently has TTL 3600. The team wants any client to see a new address within 60 seconds of a cutover at 15:00. What is the latest time they can lower the TTL to 60?"
    correct:
      - "14:00"
    distractors:
      - "14:59"
      - "15:00"
      - "14:30"
    explanation: |-
      Lowering the TTL only affects caches that fetch the record *after* the change. A resolver that cached the record with TTL 3600 just before the change keeps it for up to 3,600 more seconds. So the new, short TTL must be published at least one old TTL before the cutover: 15:00 − 3,600 s = 14:00. From 15:00, every cache holds the record with TTL ≤ 60, and the cutover is visible everywhere by 15:01.

      "14:59" leaves caches that refreshed at 14:58 holding a 3,600-second copy until 15:58.

      "15:00" changes TTL and address together, so the old address may live until 16:00.

      "14:30" covers only half of the old TTL.
    checks:
      - "assert 15 * 3600 - 3600 == 14 * 3600 and 15 * 3600 + 60 == 15 * 3600 + 60"
  - id: "negative-ttl"
    type: "single"
    prompt: "A zone's SOA record has TTL 3600 and its MINIMUM field is 900. For how long may a resolver cache an NXDOMAIN answer from this zone?"
    correct:
      - "900 seconds"
    distractors:
      - "3600 seconds"
      - "4500 seconds"
      - "0 seconds, because negative answers are never cached"
    explanation: |-
      RFC 2308 §5: a negative answer carries the zone's SOA in the authority section, and its cache lifetime is the minimum of the SOA record's own TTL and the SOA MINIMUM field. min(3600, 900) = 900 seconds. (RFC 2308 redefined MINIMUM for exactly this purpose; it was originally a default TTL for the zone's records.)

      "3600 seconds" uses only the SOA record's TTL.

      "4500 seconds" adds the two values.

      "Never cached" was the behavior RFC 2308 replaced; without negative caching, typos and missing names would hammer authoritative servers.
    checks:
      - "assert min(3600, 900) == 900 and 3600 + 900 == 4500"
  - id: "nodata-versus-nxdomain"
    type: "single"
    prompt: "`api.example.com` has an A record and no AAAA record. A resolver asks the authoritative server for its AAAA record. What comes back?"
    correct:
      - "NOERROR with an empty answer and the SOA in the authority section: NODATA, cached for AAAA at that name only"
    distractors:
      - "NXDOMAIN with the SOA in the authority section, so A lookups for the name then fail from the cache as well"
      - "SERVFAIL, because the server holds no data of the requested type for that name and cannot answer"
      - "A referral back to the parent `example.com` servers, because the name has no data of the requested type"
    explanation: |-
      RFC 2308 distinguishes two negative answers.

      - **NXDOMAIN** (RCODE 3): the name does not exist at all, for any type.
      - **NODATA**: the name exists but has no records of the requested type. RCODE is NOERROR (0), the answer section is empty, and the SOA is included so the result can be cached, keyed by name *and type*.

      Here the name exists (it has an A record), so the answer is NODATA for AAAA. Dual-stack clients see this constantly, and caching it per type means it never affects A lookups.

      "NXDOMAIN" would claim the name does not exist, which would wrongly poison A lookups.

      "SERVFAIL" signals a server or validation failure, not an absence of data.

      "A referral" happens only for names below a zone cut; this server is authoritative for the name.
  - id: "premature-query-bug"
    type: "single"
    prompt: "A deployment script resolves `new-svc.example.com` as a health check *before* the step that creates its record. The zone's SOA has TTL 3600 and MINIMUM 900; the new record has TTL 300. After the record is created, some clients cannot resolve the name for up to 15 minutes. Why?"
    correct:
      - "Resolvers cached the NXDOMAIN from the early query for min(3600, 900) = 900 seconds"
    distractors:
      - "The new record's TTL of 300 seconds must expire before resolvers will fetch it"
      - "The zone serial was not incremented, so resolvers do not know the zone changed"
      - "Anycast keeps routing these clients to a server site that has not been updated"
    explanation: |-
      The premature lookup produced NXDOMAIN, and the resolvers that handled it cached that negative answer for min(SOA TTL, MINIMUM) = 900 seconds (RFC 2308 §5), which is 15 minutes. Until the negative entry expires, they answer "no such name" without asking again. Fix: create records before anything looks them up, or lower the SOA MINIMUM for zones with frequent new names.

      "The new record's TTL must expire" cannot apply: no resolver has the new record yet, and its TTL starts only when it is fetched.

      "The zone serial was not incremented" matters for zone transfers to secondary servers, not for resolver caches, and would not produce a 15-minute window tied to MINIMUM.

      "Anycast" could cause inconsistency between sites during propagation, but not a delay that matches the negative-caching TTL exactly.
    checks:
      - "assert min(3600, 900) == 900 == 15 * 60"
  - id: "anycast-facts"
    type: "multiple"
    prompt: "Which statements about anycast DNS service are true? Select all that apply."
    correct:
      - "Many sites announce the same prefix via BGP, and each packet goes to the routing-closest one"
      - "A failed site can withdraw its announcement so routing shifts traffic to other sites"
    distractors:
      - "Each query is sent to every site at once, and the client uses whichever answer arrives first"
      - "Clients must be configured with every site's unicast address so that they can choose among them"
      - "Routers send each query to the site whose servers report the lowest load at that moment"
    explanation: |-
      Anycast is a routing technique: several sites announce the same prefix, and ordinary BGP path selection delivers each packet to one of them, the "closest" in routing terms (fewest AS hops or local preference, not necessarily lowest latency). DNS suits it well because most exchanges are a single UDP request and reply. Withdrawing a route is the failover mechanism. Root servers and large DNS providers use anycast for capacity and resilience against denial of service.

      "Sent to every site" describes multicast-like fan-out, which anycast is not; one packet goes to one site.

      "Configured with every site's address" is the opposite of anycast's point: clients see one address.

      "Lowest load" is not something BGP knows about; anycast is load-unaware unless operators adjust announcements.
---

## Learning objectives

- Bound how long a changed record can remain in caches, and plan TTL reductions.
- Compute negative-caching lifetimes from the SOA record.
- Distinguish NXDOMAIN from NODATA.
- Explain anycast and why DNS uses it.

## The invariant

Every record carries a **TTL**: the number of seconds a cache may keep it (an unsigned 31-bit value; RFC 2181 §8 says a value with the top bit set is treated as zero). A resolver counts the TTL down and returns the *remaining* TTL to its clients, so downstream caches expire the record at the same moment. DNS has no invalidation: after a change at the authoritative servers, caches may serve the old data for up to one full TTL.

Two practical rules follow:

- **Worst-case staleness = the TTL that was in effect when the data was last fetched.**
- **To make a change fast, lower the TTL at least one old TTL in advance.** Lowering it at the moment of the change does nothing for copies already cached.

**Negative answers** are cached too (RFC 2308). An authoritative server that has no answer includes the zone's SOA in the authority section, and the result may be cached for min(SOA TTL, SOA MINIMUM) (§5). There are two kinds:

| Answer | Meaning | RCODE | Cached per |
|---|---|---|---|
| NXDOMAIN | the name does not exist | 3 | name |
| NODATA | the name exists, but not with this type | 0 (NOERROR), empty answer | name and type |

A premature lookup of a name that is about to be created therefore causes a delay as long as the negative TTL. RFC 8767 (**serve-stale**) lets resolvers answer with expired data when every authoritative server is unreachable, trading freshness for availability during outages.

**Anycast** announces one prefix from many sites; BGP delivers each packet to the routing-closest site. Single-packet UDP exchanges make DNS a natural fit, and route withdrawal is the failover mechanism.

## Worked example

A cutover of `app.example.com` (TTL 3600) planned for 12:00:

| Time | Action | Effect |
|---|---|---|
| 09:00 | Lower the TTL to 300 | Copies fetched before 09:00 live until 10:00 at the latest |
| 10:00 | Every cached copy now has TTL ≤ 300 | |
| 12:00 | Change the address | Old address gone from all caches by 12:05 |
| 13:00 | Raise the TTL back to 3600 | |

Without the 09:00 step the old address could survive until 13:00.

Negative caching with `SOA ... 3600 ... MINIMUM 300`: a query for a non-existent name is cached for min(3600, 300) = 300 seconds. What the answers look like:

```bash
$ dig +noall +comments +authority nosuch.example.com A | grep -E 'status|SOA'
;; ->>HEADER<<- opcode: QUERY, status: NXDOMAIN, id: 4113
example.com.  300  IN  SOA  ns1.example.com. hostmaster.example.com. 2026112701 7200 900 1209600 300
$ dig +noall +comments +authority api.example.com AAAA | grep -E 'status|SOA'
;; ->>HEADER<<- opcode: QUERY, status: NOERROR, id: 5120
example.com.  300  IN  SOA  ns1.example.com. hostmaster.example.com. 2026112701 7200 900 1209600 300
```

The second response is NODATA: NOERROR with no answer records, only the SOA. (Authoritative servers usually send the SOA with its TTL already capped at the negative TTL, which is why both lines show 300.)

## Where this shows up today

As of November 2026, RFC 8767 is listed among the documents updating both RFC 1034 and RFC 2181, so serving stale answers during authoritative outages is part of the standard behavior a resolver may adopt. It changes the worst case during outages only: when authoritative servers are reachable, TTLs still bound staleness exactly as above.

## Pitfalls checklist

- Lowering a TTL at the same moment as the change it was meant to speed up.
- Querying names before creating them in automation and caching NXDOMAIN.
- Setting very large SOA MINIMUM values on zones that add names often.
- Treating NODATA as NXDOMAIN in client code and failing dual-stack lookups.
- Assuming anycast routes by latency or load.

## Key takeaways

- A change can take one full TTL to reach every cache; prepare by lowering TTLs one old TTL ahead.
- Negative answers live for min(SOA TTL, SOA MINIMUM).
- NXDOMAIN means no such name; NODATA means no such type at an existing name.
- Anycast gives one address many sites, chosen by BGP routing.

## Sources

- RFC 1034/1035 (TTL and caching); RFC 2181 §8 (TTL range).
- RFC 2308, Negative Caching of DNS Queries (DNS NCACHE), §5.
- RFC 8767, Serving Stale Data to Improve DNS Resiliency.
- RFC 4786, Operation of Anycast Services.
