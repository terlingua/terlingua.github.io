---
title: "DNSSEC and Encrypted DNS Transports"
body_checks:
  - "assert 3 * 20 == 60"
questions:
  - id: "dnssec-guarantees"
    type: "multiple"
    prompt: "What does DNSSEC give a validating resolver? Select all that apply."
    correct:
      - "Proof that records come from the zone's owner, unmodified"
      - "Authenticated proof that a requested name or record type does not exist"
    distractors:
      - "Encryption of queries, so on-path observers cannot see which names are looked up"
      - "Protection of the stub-to-resolver hop when the stub does not itself validate"
      - "Hiding the client's IP address from the authoritative servers it contacts"
    explanation: |-
      DNSSEC (RFC 4033–4035) adds digital signatures (RRSIG records) to RRsets and a chain of trust from the root. A validating resolver can check that each answer was signed with the zone's key, which gives **data origin authentication and integrity**. Signed NSEC or NSEC3 records give **authenticated denial of existence**, so an attacker cannot forge NXDOMAIN or NODATA either.

      "Encryption of queries" is explicitly out of scope: RFC 4033 states that DNSSEC provides no confidentiality. Queries and answers stay readable.

      "Protects the stub-to-resolver hop" is false: a non-validating stub only sees the AD bit, which an on-path attacker could set. That hop needs its own protection (a trusted channel such as DoT or DoH, or validation on the host).

      "Hides the client's address" is not a DNSSEC feature; resolvers already sit between clients and authoritative servers, and DNSSEC does not change that.
  - id: "ds-links-chain"
    type: "single"
    prompt: "In a DNSSEC chain of trust, what in the `com` zone links it to the keys of `example.com`?"
    correct:
      - "A DS record in `com` holding a digest of `example.com`'s key-signing key"
    distractors:
      - "An RRSIG record in the `example.com` zone, created with the private key of the root zone itself"
      - "A copy of `example.com`'s DNSKEY RRset, published in the `com` zone and refreshed by zone transfer"
      - "A TXT record at `example.com` in the `com` zone that contains the zone's public key fingerprint"
    explanation: |-
      Each delegation in a signed tree carries a **DS** (Delegation Signer) record, published by the *parent* at the child's name. It contains a hash of the child's key-signing key. The parent signs the DS RRset with its own zone key, so a resolver that trusts `com`'s keys can trust the DS, and therefore the child's DNSKEY whose hash matches it. The child's DNSKEY RRset is self-signed by that key, and the zone's data is signed by the keys in it. Starting from the root's trust anchor, the chain is: root DNSKEY → DS for `com` → `com` DNSKEY → DS for `example.com` → `example.com` DNSKEY → RRSIG over the answer.

      "An RRSIG created with the root's key" is wrong: each zone signs only its own data; the root never signs `example.com` records.

      "A copy of the DNSKEY in the parent" is not how it works; only the digest (DS) lives in the parent, so the child can sign with its own keys.

      "A TXT record" is not part of DNSSEC.
  - id: "expired-signatures"
    type: "single"
    prompt: "A zone's signing job stopped running, and the RRSIG records over its A records passed their expiration time an hour ago. What do clients see?"
    correct:
      - "Validating resolvers return SERVFAIL; non-validating resolvers still return the address"
    distractors:
      - "Validating resolvers return the address with the AD bit clear, so clients carry on unaffected"
      - "Every resolver returns NXDOMAIN, because a record with an expired signature is treated as absent"
      - "Validating resolvers fall back to treating the zone as unsigned until the signatures are renewed"
    explanation: |-
      A validating resolver that finds a signature outside its validity window cannot authenticate the data. If the chain of trust says the zone is signed (a DS exists in the parent), the result is **bogus**, and RFC 4035 has the resolver withhold it, returning SERVFAIL. Resolvers that do not validate ignore the signatures and keep answering. That split, "works on some networks and not others", is the classic signature of a DNSSEC failure.

      "Returned with AD clear" describes an *insecure* (unsigned, no DS) zone; a signed zone with bad signatures is bogus, not insecure.

      "NXDOMAIN" would be a false statement about the name's existence; validation failure is a server-failure condition.

      "Fall back to unsigned" would let an attacker strip signatures to bypass DNSSEC, so validators never downgrade a zone whose parent says it is signed.
  - id: "encrypted-transport-ports"
    type: "multiple"
    prompt: "Which pairings of encrypted DNS protocol and transport are correct? Select all that apply."
    correct:
      - "DNS over TLS: TLS over TCP, port 853"
      - "DNS over HTTPS: HTTPS requests, usually to port 443"
      - "DNS over QUIC: QUIC over UDP, port 853"
    distractors:
      - "DNS over HTTPS: plain HTTP requests to port 80"
      - "DNS over TLS: DTLS over UDP, port 53"
    explanation: |-
      - **DoT** (RFC 7858): a TLS session over TCP to port 853, carrying DNS messages with the usual two-byte length prefix used for DNS over TCP.
      - **DoH** (RFC 8484): DNS messages as HTTP requests (`application/dns-message`) over HTTPS, normally port 443, which makes it indistinguishable by port from web traffic.
      - **DoQ** (RFC 9250): DNS over a dedicated QUIC connection on UDP port 853.

      "Plain HTTP on port 80" would expose and allow tampering with everything; RFC 8484 requires HTTPS.

      "DTLS over UDP port 53" mixes up an experimental proposal (DNS over DTLS) with DoT, and port 53 is classic unencrypted DNS.
  - id: "what-doh-hides"
    type: "single"
    prompt: "A laptop sends all its lookups with DoH to a public resolver. Which parties can still learn the names it looks up?"
    correct:
      - "The DoH resolver's operator, and anyone who sees that resolver's queries to authoritative servers"
    distractors:
      - "Nobody, because DoH encrypts each name end to end all the way to the authoritative servers"
      - "Only the local network operator, which can read the names in the DoH requests on the wire"
      - "Only the authoritative servers, because DoH also hides the queries from the resolver itself"
    explanation: |-
      DoH encrypts one hop: stub to recursive resolver. The resolver decrypts every query in order to answer it, so its operator sees all names. When it has to resolve a name, it usually queries authoritative servers over classic, unencrypted DNS, so observers on those paths, and the authoritative servers themselves, see the names it asks about (though not which client asked). The local network operator loses visibility into the names, but can still see the IP addresses the laptop connects to afterwards, and the TLS SNI unless Encrypted Client Hello is used.

      "End to end to the authoritative servers" overstates DoH; it is a hop-by-hop protection.

      "Only the local network operator" is backwards: that is exactly the party DoH hides the names from.

      "Hides the queries from the resolver" is impossible for a resolver that must answer them.
  - id: "truncation-fallback-time"
    type: "single"
    prompt: "A client without EDNS asks a resolver 20 ms away (RTT) for a record whose response is 700 bytes. The response is truncated with the TC bit set, and the client retries over TCP. How long after the first query does the client have the full answer?"
    correct:
      - "60 ms"
    distractors:
      - "20 ms"
      - "40 ms"
      - "80 ms"
    explanation: |-
      RFC 1035 limits classic DNS messages over UDP to 512 bytes. A larger answer is cut short and the TC (truncated) bit is set; the client must repeat the query over TCP. Timeline with a 20 ms RTT:

      | Step | Round trips | Elapsed |
      |---|---|---|
      | UDP query and truncated reply | 1 | 20 ms |
      | TCP handshake | 1 | 40 ms |
      | Query and full answer over TCP | 1 | 60 ms |

      "20 ms" assumes the UDP answer was usable.

      "40 ms" forgets the TCP handshake (or the initial UDP exchange).

      "80 ms" adds a round trip that does not exist; the query can be sent right after the handshake.

      EDNS(0) (RFC 6891) avoids this by letting the client advertise a larger UDP buffer, so a 700-byte answer fits in one datagram.
    checks:
      - "assert 700 > 512 and 3 * 20 == 60"
---

## Learning objectives

- Explain what DNSSEC authenticates and what it does not.
- Trace a DNSSEC chain of trust through DS and DNSKEY records.
- Predict how validating and non-validating resolvers react to signature failures.
- Compare DoT, DoH and DoQ and what each hides, and from whom.

## The invariant

**DNSSEC** (RFC 4033, 4034, 4035) answers one question: did this data really come from the zone's owner, unmodified? Each zone signs its RRsets; signatures travel as **RRSIG** records with an inception and expiration time. The zone's public keys are published as **DNSKEY** records. The parent publishes a **DS** record, a digest of the child's key-signing key, and signs it with its own keys. A validating resolver starts from the root's trust anchor and walks DS → DNSKEY → RRSIG down the delegation chain. Signed **NSEC/NSEC3** records prove that a name or type does not exist.

Validation has three outcomes that matter operationally:

| Result | Example | What clients get |
|---|---|---|
| Secure | Chain verifies | Answer, AD bit set |
| Insecure | No DS at the parent (unsigned zone) | Answer, AD bit clear |
| Bogus | Expired or invalid signatures in a signed zone | SERVFAIL |

DNSSEC provides **no confidentiality** (RFC 4033). Names travel in clear text unless the transport is encrypted.

**Encrypted transports** protect a hop:

- DNS over TLS (RFC 7858): TCP port 853.
- DNS over HTTPS (RFC 8484): HTTPS, usually port 443.
- DNS over QUIC (RFC 9250): UDP port 853.

They hide names from observers *between the stub and its resolver*. The resolver sees everything, and its own queries to authoritative servers are usually still classic DNS. Encryption and DNSSEC are complementary: one hides the hop, the other authenticates the data wherever it came from.

Classic DNS over UDP carries at most 512 bytes (RFC 1035). Larger answers are truncated with the **TC** bit and retried over TCP, which costs extra round trips; EDNS(0) (RFC 6891) lets clients advertise larger UDP buffers.

## Worked example

Checking a chain with `dig` against a validating resolver (illustrative, abridged output; the key tag and digest are invented):

```bash
$ dig +dnssec +multi example.com DS @a.gtld-servers.net | grep -E 'DS|RRSIG'
example.com.  86400 IN DS 370 13 2 (BE74359954660069D5C63D200C39F5603827D7DD02B56F120EE9F3A8 6764247C )
example.com.  86400 IN RRSIG DS 13 2 86400 ( 20261204000000 20261127000000 ... com. ... )
$ dig +dnssec example.com A @1.1.1.1 | grep -E 'flags|RRSIG'
;; flags: qr rd ra ad; QUERY: 1, ANSWER: 2, AUTHORITY: 0, ADDITIONAL: 1
example.com.  300 IN RRSIG A 13 2 300 20261211000000 20261127000000 370 example.com. ...
```

The DS is signed by `com.` (signer field of its RRSIG); the A record is signed by `example.com.` with key tag 370, here a combined signing key, which is the key the DS digests. (Zones that split key-signing and zone-signing keys sign data with a different key, itself vouched for by the DNSKEY RRset's signature.) The `ad` flag shows the resolver validated the chain. If the A record's RRSIG had expired (its first timestamp is the expiration), the same query would return `status: SERVFAIL` from this resolver and a normal answer from a non-validating one.

## Where this shows up today

As of November 2026, the SVCB and HTTPS record types (RFC 9460, 2023) carry connection parameters such as supported ALPN protocols, and RFC 9848 (March 2026) specifies how they deliver Encrypted Client Hello configurations. That makes the DNS answer itself privacy-sensitive input to the TLS handshake, which is one reason encrypted transports and DNSSEC validation matter together.

## Pitfalls checklist

- Letting signature-refresh automation fail and turning a signed zone bogus.
- Changing DNS providers or keys without updating the DS at the parent.
- Assuming DNSSEC hides queries, or that DoH authenticates the data.
- Trusting the AD bit from a resolver reached over an untrusted network.
- Blocking DNS over TCP and breaking every answer larger than the UDP limit.

## Key takeaways

- DNSSEC authenticates data and denial of existence; it does not encrypt.
- The chain runs DS (in the parent) → DNSKEY (in the child) → RRSIG (over the data).
- Bogus data produces SERVFAIL on validating resolvers only.
- DoT, DoH and DoQ protect the stub-to-resolver hop; the resolver still sees every name.

## Sources

- RFC 4033, 4034, 4035 (DNSSEC); RFC 1035 (512-byte UDP limit, TC bit); RFC 6891 (EDNS(0)).
- RFC 7858 (DNS over TLS); RFC 8484 (DNS over HTTPS); RFC 9250 (DNS over QUIC).
- RFC 9460 (SVCB and HTTPS RRs); RFC 9848 (Bootstrapping TLS Encrypted ClientHello with DNS Service Bindings).
