# REPORT — networking-and-protocols

Generated 2026-11-27 by an unattended run. See NOTES.md for rule sources, verified facts and decisions.

## Summary

| Item | Count |
|---|---|
| Chapters | 7 |
| Sections | 30 |
| Questions | 180 (150 single, 30 multiple) |
| Prose words (excluding code, tables, Sources) | 16,093 |
| Arithmetic checks run by check_math.py | 183 (all pass) |
| Correct option strictly the longest | 51 of 180 (28%), at most 2 per section |
| Correct-answer positions (book) | 0: 49, 1: 48, 2: 48, 3: 53, 4: 20 |

Delivered: `content/books/networking-and-protocols/book.yml`, 7 `chapter.yml`, 30 section files.
Tools and sources: `_build/networking-and-protocols/` (`src/`, `build.py`, `validate.py`, `check_math.py`,
`audit.py`, `balance.py`, `balance2.py`, `addopt.py`, `relen.py`, `relen2.py`, `longest.py`, `tick.py`,
`plan.md`, `progress.md`, `NOTES.md`, this report, and the captured outputs).

To regenerate after editing a source: `python3 _build/networking-and-protocols/build.py`, then `validate.py`
and `check_math.py`.

## Per-section question counts

| s | File | Questions | Multiple |
|---|---|---|---|
| 1 | 01/01-layers-and-the-hourglass.md | 6 | 1 |
| 2 | 01/02-mac-ip-arp-and-ndp.md | 6 | 1 |
| 3 | 01/03-cidr-subnets-and-longest-prefix-match.md | 6 | 1 |
| 4 | 01/04-nat-and-port-exhaustion.md | 6 | 1 |
| 5 | 01/05-mtu-mss-and-path-mtu-discovery.md | 6 | 1 |
| 6 | 02/01-handshake-sequence-and-ack-arithmetic.md | 6 | 1 |
| 7 | 02/02-teardown-and-time-wait.md | 6 | 1 |
| 8 | 02/03-flow-control-and-bandwidth-delay-product.md | 6 | 1 |
| 9 | 02/04-slow-start-aimd-and-fast-recovery.md | 6 | 1 |
| 10 | 02/05-retransmission-timers-nagle-and-head-of-line-blocking.md | 6 | 1 |
| 11 | 03/01-udp-datagrams-and-message-boundaries.md | 6 | 1 |
| 12 | 03/02-quic-streams-and-loss-recovery.md | 6 | 1 |
| 13 | 03/03-quic-handshake-and-tls-integration.md | 6 | 1 |
| 14 | 03/04-connection-ids-migration-and-http3-discovery.md | 6 | 0 |
| 15 | 04/01-hierarchy-and-resolution.md | 6 | 1 |
| 16 | 04/02-record-types-and-the-cname-rule.md | 6 | 1 |
| 17 | 04/03-ttl-negative-caching-and-anycast.md | 6 | 1 |
| 18 | 04/04-dnssec-and-encrypted-transports.md | 6 | 2 |
| 19 | 05/01-tls-13-handshake-resumption-and-0-rtt.md | 6 | 1 |
| 20 | 05/02-ecdhe-forward-secrecy-and-aead.md | 6 | 1 |
| 21 | 05/03-x509-chain-validation-and-revocation.md | 6 | 1 |
| 22 | 05/04-sni-alpn-and-what-tls-does-not-hide.md | 6 | 1 |
| 23 | 06/01-method-safety-idempotency-and-retries.md | 6 | 1 |
| 24 | 06/02-status-code-contracts.md | 6 | 1 |
| 25 | 06/03-cache-control-and-freshness.md | 6 | 1 |
| 26 | 06/04-conditional-requests-etags-and-vary.md | 6 | 1 |
| 27 | 06/05-http-versions-and-real-time-patterns.md | 6 | 1 |
| 28 | 07/01-end-to-end-latency-counting.md | 6 | 1 |
| 29 | 07/02-throughput-and-mtu-diagnostics.md | 6 | 1 |
| 30 | 07/03-retry-storms-and-contract-violations.md | 6 | 1 |

## Validation output (validate.py)

```
stats: {'chapters': 7, 'sections': 30, 'questions': 180, 'type_single': 150, 'type_multiple': 30, 'longest_correct': 51, 'prose_words': 16093}
correct-answer positions (book): {0: 49, 1: 48, 2: 48, 3: 53, 4: 20}
0 error(s), 0 warning(s)
```

validate.py ports the compiler's rules (naming regexes, front-matter split on `\n---`, YAML keys, question
and option rules) and checks the prompt's hard rules: every string double-quoted, `type` always present,
4–5 options (5 for multiple, 2–3 correct), "Select all that apply." placement, reserved ids, no option
references by letter or position, no all/none of the above, code-fence languages, heading order and bullet
counts, 400–1200 prose words, the size of the "today" block and its "As of November 2026" claim, the
(k + s) mod n position rule and the 40% per-position cap. All front matter also parses with ruamel.yaml
(YAML 1.2, pure Python) as a second parser.

## Math audit (check_math.py)

```
checks: 183 run, 0 failed, 30 source files
```

Every CIDR boundary, host count, summary and longest-prefix match (Python `ipaddress`), MTU/MSS and fragment
offset, sequence/ACK number, RTO update, slow-start round count, BDP, window, Mathis estimate, NAT and
TIME_WAIT port arithmetic, TLS record count and nonce, WebSocket accept value, cache age, date arithmetic
and latency total used in an option, explanation or worked example has a `checks:` or `body_checks:` entry.
The audit caught one real error during writing (a mistyped WebSocket GUID in the helper), fixed against the
RFC's own example.

## RFC audit

All cited RFCs were looked up on rfc-editor.org or datatracker (see NOTES.md). Findings that changed the book:

- RFC 8446 is obsolete: TLS 1.3 is now RFC 9846 (July 2026). The book cites RFC 9846 throughout and mentions
  RFC 8446 as the original version.
- RFC 8312 (CUBIC) is obsolete: RFC 9438 is cited.
- New RFCs used: RFC 9849 (ECH, March 2026), RFC 9848 (ECH via SVCB), RFC 10024 (hybrid ECDHE-MLKEM),
  RFC 9525 (service identity, obsoletes RFC 6125).
- Section numbers appear only where they were confirmed in the fetched RFC text (list in NOTES.md).

## Question audit

All 180 questions were re-read against their explanations. `audit.py` adds a structural check (one quoted
rebuttal per distractor); its 7 flags were reviewed by hand and are explanations that rebut distractors in
bullets or combined sentences without quoting them (for example, 301 and 302 handled together). Fixes made
during the audit: a misleading statement about where the child's SOA appears (s15), the stale-while-
revalidate explanation now rebuts each distractor separately (s25), a rounding inconsistency in the Mathis
question (s29), escaped quotes inside block scalars (s11, s26), and the option-length and position
rebalancing described in NOTES.md.

## Unresolved items and uncertainties

1. **Real compiler not run.** No .NET SDK in the sandbox, and dot.net / nuget.org are blocked by the egress
   proxy. Please run `dotnet run --project tools/ObserverMagazine.ContentProcessor` (or `dotnet test`) once
   before merging. Residual risk is low: the format matches the merged relational-sql-and-transactions book
   (block-scalar explanations, quoted strings), and two independent YAML parsers accept every file.
2. **C# snippets not compiled** (s10, s23, s30). Python snippets in s11, s20, s26 and s27 were executed.
3. **Illustrative command output.** tcpdump, ss, dig, openssl and curl outputs use documentation addresses and
   are abridged; the DNSSEC and 0-RTT outputs are labeled illustrative in the text. They show formats and
   relationships, not captures from real hosts.
4. **Implementation constants** stated without an "As of" date and not re-verified this run: Linux TIME_WAIT
   60 s (checked only against kernel-source discussions), Linux minimum RTO 200 ms, delayed-ACK ≈ 40 ms,
   initial window 10, Go and Kestrel enabling `TCP_NODELAY` by default.
5. **Dated claims** were each checked against a primary or vendor source on 2026-11-27 (list in NOTES.md).
   Two depend on secondary aggregation: browser HTTP/3 defaults (web-platform-dx web-features data) and
   browser hybrid post-quantum defaults (Cloudflare's PQC support page). No adoption percentage is given
   except Google's IPv6 crossing 50% on 28 March 2026 and APNIC's ~42% (April 2026), both attributed.
6. **RFC 6585 §4, RFC 5280 §4.1.2.5 and §6, RFC 7838 `ma` default** are cited from the documents' known
   structure; their current status was not re-fetched in this run.
7. **Timeless-question limit** (at most one dated question per section) was applied by hand. The only
   question resting on a dated schedule (s21) states the schedule in its prompt.
8. **House style addition:** each section ends with `## Sources` after the six required headings, as in the
   existing books; remove it if the exact six-heading structure is required.
