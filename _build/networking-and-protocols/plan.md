# Plan — Networking & Internet Protocols: RFC Mechanics

7 chapters, 30 sections, 6 questions per section (180 total). s = book-wide section number.

## 01-layering-addressing-and-routing — "Layering, Addressing and Routing"

| s | File | Goal | Question concepts |
|---|---|---|---|
| 1 | 01-layers-and-the-hourglass.md | Use OSI as a vocabulary and TCP/IP as the real architecture; compute encapsulation overhead | narrow waist (IP) meaning; encapsulation order on the wire; header-overhead efficiency arithmetic; which layer a device examines (switch/router/L4 LB); end-to-end principle; frames per payload count |
| 2 | 02-mac-ip-arp-and-ndp.md | Trace which addresses change per hop; resolve next hops with ARP/NDP | MAC rewritten per hop, IP unchanged; host ARPs for gateway not remote IP; ARP request broadcast vs reply unicast; IPv6 solicited-node multicast address computation; stale ARP / duplicate IP after failover (gratuitous ARP); TTL/hop-limit decrement trace |
| 3 | 03-cidr-subnets-and-longest-prefix-match.md | Do CIDR arithmetic and pick routes by longest prefix | network/broadcast of an address; usable host count (/31 /32 special); summarization of contiguous blocks; longest-prefix match among routes; "is X in subnet Y" bug with string prefix; IPv6 /64 and /48 counts |
| 4 | 04-nat-and-port-exhaustion.md | Explain NAPT mappings and compute port exhaustion | NAPT rewrites src addr+port and reverse; unsolicited inbound dropped; ports per (dst ip, dst port) arithmetic; connection pool / many destinations to mitigate; RFC 1918 vs RFC 6598 shared space; IPv6 adoption (dated) |
| 5 | 05-mtu-mss-and-path-mtu-discovery.md | Compute MSS, explain PMTUD and black holes | MSS from MTU (v4/v6, PPPoE, tunnel); IPv4 fragment offsets in 8-byte units; DF + ICMP frag needed type 3 code 4; IPv6 routers never fragment, PTB; black hole symptom (handshake ok, large response hangs); MSS clamping/PLPMTUD fix |

## 02-tcp-state-flow-and-congestion — "TCP: State, Flow and Congestion"

| s | File | Goal | Question concepts |
|---|---|---|---|
| 6 | 01-handshake-sequence-and-ack-arithmetic.md | Trace SEQ/ACK through a handshake and data | SYN consumes one sequence number; ACK = next expected byte; trace of three segments; random ISN purpose; duplicate ACK on gap; FIN consumes one number |
| 7 | 02-teardown-and-time-wait.md | Explain FIN/ACK states and TIME_WAIT | active closer gets TIME_WAIT; 2*MSL purpose (two reasons); TIME_WAIT count arithmetic; client-side ephemeral exhaustion; RST vs FIN semantics; half-close / CLOSE_WAIT leak bug |
| 8 | 03-flow-control-and-bandwidth-delay-product.md | Compute BDP and window-limited throughput | throughput = window/RTT; BDP in bytes; window scale shift needed; rwnd vs cwnd effective window min; zero window and persist timer; buffer bloat / rwnd too big vs app read |
| 9 | 04-slow-start-aimd-and-fast-recovery.md | Trace cwnd through slow start, AIMD and recovery | RTTs to reach a window in slow start; ssthresh after loss (half flight); 3 dupACKs fast retransmit; timeout vs dupACK reaction; AIMD sawtooth average; loss-based vs model-based (dated) |
| 10 | 05-retransmission-timers-nagle-and-head-of-line-blocking.md | Compute RTO; diagnose Nagle/delayed-ACK stalls and HOL | SRTT/RTTVAR/RTO update arithmetic; Karn's algorithm; exponential backoff total wait; Nagle + delayed ACK write-write-read stall; TCP_NODELAY fix; in-order delivery HOL blocking |

## 03-udp-quic-and-http3 — "UDP, QUIC and HTTP/3"

| s | File | Goal | Question concepts |
|---|---|---|---|
| 11 | 01-udp-datagrams-and-message-boundaries.md | Contrast message framing with byte streams | recv on UDP returns one datagram; TCP coalescing bug; max UDP payload arithmetic; buffer too small truncation; no delivery/order guarantees; UDP reflection amplification factor |
| 12 | 02-quic-streams-and-loss-recovery.md | Explain per-stream delivery and QUIC loss detection | lost packet blocks only its streams; packet numbers never reused; packet threshold 3 trace; time threshold 9/8 arithmetic; stream offsets vs packet numbers; why over UDP (ossification) |
| 13 | 03-quic-handshake-and-tls-integration.md | Count QUIC handshake RTTs and explain TLS integration | QUIC 1-RTT vs TCP+TLS 2-RTT; TLS in CRYPTO frames not records; anti-amplification 3x arithmetic; Retry adds RTT; Initial keys derivable (not secret); 0-RTT only resumed |
| 14 | 04-connection-ids-migration-and-http3-discovery.md | Explain connection IDs, migration, and h3 discovery/fallback | 4-tuple vs connection ID after NAT rebinding; path validation before sending much; Alt-Svc discovery means first visit is TCP; HTTPS RR alpn; UDP blocked fallback; browser default support (dated) |

## 04-dns-resolution-and-caching-invariants — "DNS: Resolution and Caching Invariants"

| s | File | Goal | Question concepts |
|---|---|---|---|
| 15 | 01-hierarchy-and-resolution.md | Trace recursive and iterative resolution | stub->recursive recursive, recursive->auth iterative; referral count cold cache; glue records needed; warm cache query count; delegation NS mismatch bug; RD/RA flags |
| 16 | 02-record-types-and-the-cname-rule.md | Use record types correctly; know why CNAME is exclusive | PTR name construction; MX preference ordering; SRV fields; CNAME exclusivity (RFC 2181 10.1); apex CNAME impossible; HTTPS AliasMode / ALIAS flattening |
| 17 | 03-ttl-negative-caching-and-anycast.md | Compute cache lifetimes, negative TTLs and anycast behavior | worst-case staleness after change; lowering TTL ahead of migration timing; negative TTL = min(SOA TTL, MINIMUM); NXDOMAIN vs NODATA; anycast routes to nearest by BGP; serve-stale (dated-ish mechanism) |
| 18 | 04-dnssec-and-encrypted-transports.md | Separate DNSSEC integrity from transport confidentiality | DNSSEC gives origin authentication not secrecy; chain DS->DNSKEY->RRSIG; validation failure = SERVFAIL; DoT 853/DoH 443/DoQ; what encrypted DNS hides and from whom; TC bit / UDP 512 and fallback to TCP |

## 05-tls-13-and-trust-chains — "TLS 1.3 and Trust Chains"

| s | File | Goal | Question concepts |
|---|---|---|---|
| 19 | 01-tls-13-handshake-resumption-and-0-rtt.md | Count TLS 1.3 handshake flights; reason about 0-RTT replay | 1-RTT full handshake, client sends app data after one RTT; HelloRetryRequest extra RTT; which messages are encrypted; PSK resumption; 0-RTT replay -> only idempotent; 425 Too Early |
| 20 | 02-ecdhe-forward-secrecy-and-aead.md | Explain forward secrecy and AEAD records | static RSA removed; FS meaning after key theft; AEAD integrity rejects modified record; nonce = IV xor sequence; record size max 2^14 count; hybrid PQ key exchange (dated) |
| 21 | 03-x509-chain-validation-and-revocation.md | Validate a chain and reason about revocation | leaf->intermediate->root order; missing intermediate bug; SAN not CN; wildcard one label; basicConstraints CA; validity window / lifetimes (dated) |
| 22 | 04-sni-alpn-and-what-tls-does-not-hide.md | Know what TLS reveals and how SNI/ALPN/ECH work | SNI selects certificate; ALPN picks h2/http1.1; visible metadata (IP, sizes, timing); ECH hides inner SNI but outer public name; size of response leaks; ALPN mismatch bug |

## 06-http-semantics-caching-and-real-time-patterns — "HTTP Semantics, Caching and Real-Time Patterns"

| s | File | Goal | Question concepts |
|---|---|---|---|
| 23 | 01-method-safety-idempotency-and-retries.md | Classify methods and decide retry safety | safe vs idempotent table; PUT idempotent POST not; DELETE idempotent though status differs; retry after timeout on POST; idempotency key mechanism (dated draft); PATCH not idempotent generally |
| 24 | 02-status-code-contracts.md | Choose and interpret status codes | 303 changes to GET; 307/308 preserve method; 401 needs WWW-Authenticate vs 403; 409 vs 412; 429 + Retry-After compute; 502/503/504 attribution |
| 25 | 03-cache-control-and-freshness.md | Compute freshness and choose directives | freshness lifetime vs Age; s-maxage for shared caches; no-cache vs no-store; must-revalidate when stale; private; stale-while-revalidate window arithmetic |
| 26 | 04-conditional-requests-etags-and-vary.md | Use validators and Vary correctly | If-None-Match -> 304; If-Match lost-update prevention -> 412; weak vs strong ETag; Vary key bug (Accept-Encoding); Vary: * effect; 304 carries no body bytes arithmetic |
| 27 | 05-http-versions-and-real-time-patterns.md | Compare HTTP/1.1, 2, 3 and SSE/WebSocket/long polling | pipelining in-order HOL; H2 one TCP connection HOL; connection count arithmetic; SSE vs WebSocket choice; WebSocket handshake accept computation; long polling request count |

## 07-cross-chapter-review-and-network-diagnostics — "Cross-Chapter Review and Network Diagnostics"

| s | File | Goal | Question concepts (combinations) |
|---|---|---|---|
| 28 | 01-end-to-end-latency-counting.md | Count round trips for whole page loads | cold HTTP/1.1+TLS1.3+DNS total ms; HTTP/3 cold; warm with resumption 0-RTT; Alt-Svc first-visit; redirect chain cost; DNS negative caching + retry latency |
| 29 | 02-throughput-and-mtu-diagnostics.md | Diagnose throughput caps and black holes | rwnd cap with no scaling; BDP for target; loss-limited throughput (Mathis) ; PMTUD black hole through tunnel; MSS clamp value; slow start time to fill BDP |
| 30 | 03-retry-storms-and-contract-violations.md | Debug client retry behavior across protocols | retry amplification across layers arithmetic; jittered exponential backoff total; 429 Retry-After ignored; POST retried without key duplicate; 503 vs 504 retry choice; TIME_WAIT/NAT port exhaustion from no pooling |
