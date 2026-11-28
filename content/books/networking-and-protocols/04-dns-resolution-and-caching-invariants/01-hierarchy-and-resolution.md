---
title: "The DNS Hierarchy: Recursive and Iterative Resolution"
questions:
  - id: "rd-ra-flags"
    type: "single"
    prompt: "A laptop's stub resolver asks a recursive resolver for `www.example.com`, and the recursive resolver then queries the root, `.com` and `example.com` servers. How are the RD (Recursion Desired) and RA (Recursion Available) bits used?"
    explanation: |-
      RFC 1034 describes two modes. In *recursive* mode the server does all the work and returns a final answer; a client asks for it with RD = 1, and a server that offers it sets RA = 1 in replies. In *iterative* mode a server answers from its own data or returns a referral. Stub resolvers on hosts use recursive mode toward their configured resolver; recursive resolvers walk the hierarchy iteratively, sending queries with RD clear, and authoritative servers answer or refer.

      "Each authoritative server recurses onward" is wrong: authoritative servers for the root and TLDs do not offer recursion; they return referrals.

      "The stub clears RD" inverts the flag's meaning; a stub that clears RD would get referrals it cannot follow.

      "Set only by authoritative servers" confuses RD/RA with the AA (Authoritative Answer) bit, which marks answers from a server authoritative for the zone.
    options:
      - text: "The stub sets RD, the resolver's reply sets RA, and the resolver's own queries usually clear RD"
        correct: true
      - text: "Every query sets RD, and each authoritative server recurses onward on the resolver's behalf"
      - text: "The stub clears RD to ask for iterative service, and the resolver sets RA once it has the answer"
      - text: "RD and RA are set only by authoritative servers, to mark answers that came from their own zones"
  - id: "cold-cache-query-count"
    type: "single"
    prompt: "`example.com` delegates `shop.example.com` to its own name servers, and every referral includes glue. A recursive resolver with an empty cache (except the root hints) resolves `www.shop.example.com`. How many queries does it send?"
    explanation: |-
      Each zone cut is one referral. The resolver asks:

      1. A root server → referral to the `com` servers (with their addresses).
      2. A `com` server → referral to the `example.com` servers.
      3. An `example.com` server → referral to the `shop.example.com` servers (the delegated child zone).
      4. A `shop.example.com` server → the A record, with AA set.

      Glue in each referral means no side lookups are needed for name-server addresses, so the total is 4.

      "3" skips the extra delegation; labels do not matter, zone cuts do. Without the `shop` delegation the `example.com` servers would answer directly and 3 would be right.

      "5" counts one query per label including `www`, but `www` is a name inside the `shop` zone, not a zone.

      "1" describes a warm cache, not an empty one.
    options:
      - text: "3"
      - text: "4"
        correct: true
      - text: "5"
      - text: "1"
  - id: "glue-records"
    type: "single"
    prompt: "`example.com` is served by `ns1.example.com` and `ns2.example.com`. What must the `.com` servers include when they refer a resolver to `example.com`, and why?"
    explanation: |-
      A referral consists of NS records naming the child zone's servers. Here the server names are *inside* the zone they serve. To find `ns1.example.com` the resolver would have to ask the `example.com` servers, which it cannot reach without that address: a circular dependency. The parent therefore holds and returns **glue**, address records for in-zone name servers, in the additional section of the referral (RFC 1034's delegation rules).

      "CNAME records" are wrong twice: name servers must not be aliases (RFC 2181 §10.3), and the root zone does not hold addresses for second-level names.

      "The SOA record" belongs to the child zone and is served by the child's own servers; a referral from the parent carries NS records and glue, not the child's SOA.

      "Start again from the root" loops back to the same `.com` referral and still needs the address.
    options:
      - text: "CNAME records that point the two name server names at addresses held in the root zone"
      - text: "The SOA record of `example.com`, so that the resolver knows the zone's serial number and refresh timers"
      - text: "A and AAAA glue for the two name servers, because finding their addresses would require asking those very servers"
        correct: true
      - text: "Nothing extra, because the resolver can look up `ns1.example.com` by starting again from the root servers"
  - id: "warm-cache-query-count"
    type: "single"
    prompt: "A resolver looked up `www.example.com` five minutes ago and still has the `.com` and `example.com` NS records (and their addresses) in its cache, with time left on their TTLs. It now resolves `api.example.com`, which it has never seen. How many queries does it send?"
    explanation: |-
      Resolvers cache referrals (NS records and glue) as well as answers, with their TTLs. To resolve a new name, a resolver starts from the closest enclosing zone whose servers it already knows. It has valid `example.com` NS records, so it goes straight to one of those servers: one query.

      "Three, as for any name it has never seen" ignores referral caching, which is what keeps root and TLD load low.

      "Two: com and example.com" assumes only the root referral was cached, but the `example.com` delegation was cached too.

      "None" confuses the delegation with the answer; NS records say who to ask, not what `api.example.com` resolves to.
    options:
      - text: "Three: root, `com` and `example.com`, as for any name it has never seen"
      - text: "Two: `com` and then `example.com`, because only the root referral is cached"
      - text: "None, because it can derive the address from the cached NS records"
      - text: "One, to an `example.com` name server"
        correct: true
  - id: "stale-delegation-bug"
    type: "single"
    prompt: "A company moves `example.org` from provider A to provider B and creates the zone, with NS records listing B's servers, at provider B. It then deletes the zone at provider A. Hours later, many users still get `SERVFAIL`. `dig +trace` shows the `.org` servers referring resolvers to `ns1.provider-a.net`. What went wrong?"
    explanation: |-
      Resolvers find a zone through its **parent's** delegation, not through the NS records inside the zone. The `.org` servers still return NS records for provider A (registrars publish delegations to the TLD). Provider A no longer has the zone, so its servers refuse or fail, and resolvers report `SERVFAIL`. Editing NS records at provider B changes only what B's servers say about themselves, which resolvers never reach. The fix: update the NS records at the registrar, and keep the old provider serving the zone until the parent's NS TTL has passed.

      "TTLs too long" fails because nobody is getting stale data; they are getting no data.

      "SOA serial must be higher" applies to zone transfers between primary and secondary servers of one provider, not to independent providers.

      "Switch off DNSSEC" misattributes the error; a DNSSEC move needs the DS record handled carefully, but the trace shows a plain delegation problem.
    options:
      - text: "The registrar still delegates to provider A's servers, which no longer serve the zone"
        correct: true
      - text: "The A records' TTLs at provider B are too long, so resolvers keep using addresses cached from provider A"
      - text: "Provider B's SOA serial number must be higher than provider A's, or resolvers ignore the new zone's data"
      - text: "DNSSEC must be switched off during a move, because provider B's servers cannot answer for a signed zone"
  - id: "root-and-tld-roles"
    type: "multiple"
    prompt: "Which statements about the root and TLD servers are true? Select all that apply."
    explanation: |-
      The root zone contains delegations for TLDs only. Asked for `www.example.com`, a root server returns NS and glue for `com`: a referral. TLD delegations have long TTLs (two days for `com` in practice), so a busy resolver rarely needs the root at all.

      "Root servers hold every domain's addresses" contradicts the delegation design; the root does not know about `example.com` at all.

      "TLD servers forward and relay" describes recursion; TLD servers are authoritative-only and return referrals.

      "Exactly 13 machines" confuses the 13 root server *identities* (a through m.root-servers.net) with servers. Each identity is an anycast address served by many instances around the world.
    options:
      - text: "Root servers hold the address records of every registered second-level domain"
      - text: "A root server answers a query for `www.example.com` with a referral to the `com` servers"
        correct: true
      - text: "Resolvers cache TLD delegations, so most lookups never contact a root server"
        correct: true
      - text: "TLD servers forward each query to the domain's servers and relay the answer back"
      - text: "The root is served by exactly 13 machines, one for each root server letter"
---
## Learning objectives

- Trace a resolution through root, TLD and authoritative servers.
- Distinguish recursive service from iterative referrals, and the RD, RA and AA flags.
- Explain glue and why delegations live in the parent zone.
- Count queries with cold and warm caches.

## The invariant

DNS (RFC 1034, RFC 1035) is a tree of names cut into **zones**. A parent zone **delegates** a child by publishing NS records for it, plus **glue** (address records) when the child's name servers have names inside the child itself. The data a resolver follows to find a zone is always the parent's delegation; the child's own NS records are what its servers say about themselves.

Two roles do the work:

- A **recursive resolver** accepts queries with RD (Recursion Desired) set, does whatever lookups are needed, caches everything it learns with its TTL, and returns a final answer with RA (Recursion Available) set.
- **Authoritative servers** answer for their zones (setting AA) or return a **referral** to a child zone. Root and TLD servers are authoritative-only.

A host's **stub resolver** sends one recursive query; the recursive resolver performs **iterative** resolution:

1. Start at the deepest zone whose servers are cached (the root, from built-in root hints, if nothing else).
2. Ask one of its servers. A referral names the next zone down; an answer ends the walk.
3. Repeat until an answer (or NXDOMAIN, or a failure).

The number of queries on a cold cache equals the number of **zone cuts** on the path, plus any side lookups for name-server addresses that come without glue. On a warm cache, the resolver skips straight to the deepest cached zone, which is why most lookups touch only the final authoritative server.

## Worked example

Cold-cache resolution of `www.example.com`, as `dig +trace` shows it (abridged):

```bash
$ dig +trace www.example.com A
.                 518400 IN NS a.root-servers.net.
;; Received 239 bytes from 127.0.0.53#53(127.0.0.53)
com.              172800 IN NS a.gtld-servers.net.
;; Received 1170 bytes from 198.41.0.4#53(a.root-servers.net)
example.com.      172800 IN NS ns1.dnshost.net.
;; Received 220 bytes from 192.5.6.30#53(a.gtld-servers.net)
www.example.com.  300    IN A  192.0.2.80
;; Received 61 bytes from 203.0.113.53#53(ns1.dnshost.net)
```

Three queries: root, `com`, `example.com`. But the `example.com` servers here are named in `.net`, *outside* the zone, so the `com` referral carries no glue for them. If `ns1.dnshost.net`'s address is not cached, the resolver first resolves it, asking a `net` server and a `dnshost.net` server (the root referral for `net` may also be needed on a truly cold cache). In-zone name servers with glue save those side trips; out-of-zone name servers avoid the operational coupling of glue. Both are legitimate choices.

| Cache state | Queries for a new name in `example.com` |
|---|---|
| Empty (root hints only), in-zone glue | 3 |
| `com` delegation cached | 2 |
| `example.com` delegation cached | 1 |

## Where this shows up today

As of November 2026, root-servers.org lists 2,046 operational instances behind the 13 root server identities, run by 12 independent operators. Because resolvers cache TLD delegations for days, the root handles a tiny fraction of lookups; most resolver traffic goes to TLD and authoritative servers, and most user queries are answered from cache.

## Pitfalls checklist

- Changing NS records inside the zone and expecting the world to follow; the parent's delegation rules.
- Deleting a zone at the old provider before the parent's NS TTL has expired.
- Pointing a stub at an authoritative-only server and getting referrals or refusals.
- Forgetting glue for in-zone name servers.
- Counting labels instead of zone cuts when estimating lookup cost.

## Key takeaways

- Stubs ask recursively; resolvers walk the tree iteratively from cached referrals.
- Delegations (NS and glue) live in the parent zone.
- Cold-cache cost is one query per zone cut plus side lookups for unglued name servers.
- Warm caches send most lookups directly to the final authoritative server.

## Sources

- RFC 1034, Domain Names — Concepts and Facilities; RFC 1035, Domain Names — Implementation and Specification.
- RFC 2181, Clarifications to the DNS Specification (§10.3, NS and MX targets).
- Root Server Technical Operations Association, root-servers.org, read 27 November 2026.
