# NOTES — networking-and-protocols

Running log of decisions, rule sources and conflicts. Unattended run, 2026-11-27.

## Environment

- No local checkout existed. `dump.txt` (project doc, export 2026-11-27T22:09Z) was read first, then the
  public repository was cloned (`git clone --depth 1`, HEAD e3d5f37, 2026-11-27 17:10 -0500) to read the
  existing books that the dump only inventories.
- No .NET SDK in the sandbox; the egress proxy refuses builds.dotnet.microsoft.com and api.nuget.org
  (HTTP 403 on CONNECT), so the real ContentProcessor cannot be built or run. As in the
  relational-sql-and-transactions and data-representation-traps runs, the compiler's rules are ported
  into `validate.py` and checked with PyYAML 6.0.3. Logged as unresolved in REPORT.md.
- curl to www.rfc-editor.org is refused by the proxy, but the WebFetch/WebSearch tools reach
  rfc-editor.org, datatracker.ietf.org and vendor sites. RFC status was checked there (see "RFC audit").
- The PAT in the user's preferences was not needed (anonymous clone worked; no GitHub API use).

## Compiler rules (source wins)

Re-read from `tools/ObserverMagazine.ContentProcessor` in the dump; unchanged from the previous runs:

- Book slug `^[a-z0-9]+(?:-[a-z0-9]+)*$`; chapter folders and section files `^(\d+)[-_](slug)$`;
  names starting with `.` or `_` are hidden.
- `book.yml`: title (required), author, summary, date, updated, tags, image, draft. Future dates skipped.
- `chapter.yml`: title (required), summary, draft.
- Section front matter starts at byte 0 with `---` and ends at the first `\n---`; YamlDotNet with
  camelCase and IgnoreUnmatchedProperties (unknown keys silently ignored, so validate.py rejects them).
- Question: id optional (slug regex, unique), prompt required, type null/single/multiple (single with
  more than one correct is an error), options >= 2, at least one `correct` (YAML bool), option text required.
- SyntaxHighlighter: csharp, javascript, typescript, python, sql, bash, json, yaml, html (xml), css plus
  aliases. Intersection with the prompt's allow-list: csharp, python, javascript, sql, bash, json, yaml, html.
  This book uses `bash` (raw HTTP, DNS output, packet traces), `python` and `csharp`.
- Block scalars (`explanation: |-`): standard YAML and supported by YamlDotNet. The
  relational-sql-and-transactions book is merged on main (deploy.yml and pr-check.yml both run the content
  processor, which fails the build on bad YAML) and uses `|-` in every question. The CI result itself could
  not be inspected from the sandbox (GitHub API use was denied), so acceptance is inferred, not observed.
  Decision: use `|-`.

## Style decisions copied from existing books

- Author id `terlingua-team` (exists in content/authors; most common in book.yml files).
- Date: unquoted `2026-11-27` (DateTime). `draft: false` and `correct: true` unquoted (bools).
- Title/summary/tags double-quoted like relational-sql-and-transactions.
- Section bodies end with a `## Sources` heading after the six required headings (house style of the two
  most recent books). Conflict with "exact six headings" logged below.

## Authoring pipeline

- Sources in `_build/networking-and-protocols/src/NN-chapter/NN-section.md` with per-question `correct:`
  and `distractors:` lists and `checks:` (Python statements, run by check_math.py). `chapter.yml` and
  `book.yml` are written directly into content/.
- `build.py` emits the content files: every string double-quoted (JSON escaping), explanations as `|-`,
  options placed by the position rule: first correct at (k + s) mod n, further correct options at the
  next indexes modulo n, distractors in authored order in the remaining slots.

## RFC audit (status checked 2026-11-27 on rfc-editor.org / datatracker)

- RFC 8446 (TLS 1.3) is OBSOLETED by RFC 9846 (July 2026). The book cites RFC 9846 as the TLS 1.3
  specification and mentions RFC 8446 as the original 2018 version. This is a conflict with the prompt's
  source list; the RFC Editor wins.
- RFC 8312 (CUBIC) obsoleted by RFC 9438 (Proposed Standard, 2023), which updates RFC 5681.
- Current (no "obsoleted by"): 791 (STD 5), 8200 (STD 86, updated by 9673), 4632 (BCP 122), 826 (STD 37),
  4861 (updated by many), 1191, 8201 (STD 87), 9293 (STD 7), 5681 (updated by 9438), 6298,
  9000/9001/9002, 9114, 9110 (STD 97), 9111 (STD 98), 9112 (STD 99, updated by 9931), 9113,
  1034/1035 (STD 13), 2181, 2308, 5280 (updated by several), 6455 (updated by 7936, 8307, 8441),
  9460 (SVCB/HTTPS), 8484 (DoH).
- New RFCs found: RFC 9849 (ECH, March 2026), RFC 9848 (ECH bootstrapping via SVCB), RFC 10024 (hybrid
  ECDHE-MLKEM for TLS 1.3; X25519MLKEM768 codepoint 0x11EC, client share 1216 bytes).
- Section numbers are cited only where the fetched text confirmed them: RFC 2308 §5 (negative TTL =
  min(SOA MINIMUM, SOA TTL)), RFC 2181 §10.1 (CNAME and other data), §10.3 (MX/NS not aliases), §8
  (TTL range), RFC 9111 §5.2.2.4 (no-cache), §5.2.2.2 (must-revalidate), RFC 4632 §5.1 (longest match),
  RFC 9112 §9.3.2 (pipelining order), RFC 8200 §5 (1280 minimum MTU), RFC 9113 §5.3.2 (RFC 7540 priorities
  deprecated). Everything else is cited at RFC-number level.

## Dated claims verified (sources)

- Google IPv6 statistics first exceeded 50% on 28 March 2026 (Internet Society Pulse; APNIC blog
  2026-04-28 also gives APNIC Labs ~42% capability).
- CUBIC default in Linux, Windows and Apple stacks: stated in RFC 9438 itself. BBR: draft-ietf-ccwg-bbr-06
  (July 2026) specifies BBRv3, intended status Experimental, not an RFC.
- HTTP/3 enabled by default: Chrome/Edge 87, Firefox 88, Safari 18 (web-platform-dx web-features data,
  Baseline low date 2024-09-16).
- CA/Browser Forum ballot SC-081v3 (April 2025): max TLS cert validity 200 days from 2026-03-15, 100 days
  from 2027-03-15, 47 days from 2029-03-15; DCV reuse down to 10 days in 2029.
- Let's Encrypt: 90-day default, 6-day short-lived profile optional; 64 days from 2027-02-10, 45 days from
  2028-02-16 (letsencrypt.org upcoming features / cert-lifetimes pages, updated July 2026); its
  certificates carry a CRL distribution point instead of an OCSP URL.
- X25519MLKEM768 default: Chrome 131+, Firefox 132+ (desktop), Safari 26+ (Cloudflare PQC support doc).
- Idempotency-Key header: draft-ietf-httpapi-idempotency-key-header-09 (21 November 2026), WG document,
  not an RFC.

## Conflicts and resolutions

1. Prompt "run the repo's real build" vs no SDK/NuGet access -> ported validation (above).
2. Prompt "double-quote every string" vs YAML dates/bools -> dates and bools unquoted.
3. Six required headings vs house `## Sources` -> Sources appended last.
4. RFC 8446 in the prompt's source list is obsolete -> RFC 9846 cited (see RFC audit).
5. "Next valid index(es)" for multiple questions interpreted as wrapping modulo n (same as previous runs).
6. RFC 5861 (stale-while-revalidate) is Informational, not part of RFC 9111; described as such.
7. TIME_WAIT on Linux (60 s), Linux minimum RTO (200 ms), delayed-ACK minimum (~40 ms) and Linux's initial
   window of 10 are stated as long-standing implementation values, not as "As of" claims; the TIME_WAIT
   constant was checked against kernel-source discussions found by search, the others were not re-verified.
8. Prompt rule "dated facts must never be the only reason an answer is correct": the one question built on
   a dated schedule (s21 `max-validity-date`) states the SC-081 schedule in its prompt, so the answer is
   derivable from the prompt plus arithmetic.
9. Ch7 (review) has 3 sections; total 30 sections (within 25–32).

## Additional section numbers confirmed during writing

- RFC 9000 §2.1 (stream types), §8.1 and §8.1.2 (address validation, Retry), §8.2 (path validation),
  §9 (migration), §14 (datagram size).
- RFC 9846 §1.2 (relationship to RFC 8446: backward-compatible revision, tightened rules), §2.3 (0-RTT),
  §5.2–5.3 (record protection, per-record nonce), §7.1 (key schedule), §8 / 8.1–8.3 (anti-replay).
- RFC 9110 §5.6.7, §8.8.1–8.8.3, §9.2.1–9.2.3, §10.2.3, §12.5.5, §13.1.1–13.1.5, §15.4.2–15.4.9,
  §15.5.2, §15.5.4, §15.5.10, §15.5.13, §15.6.3–15.6.5.
- RFC 9111 §4.1 (Vary; "*" always fails to match), §4.2.1–4.2.4, §5.1, §5.2.2.1, §5.2.2.2 (504 when
  disconnected with must-revalidate), §5.2.2.4, §5.2.2.5, §5.2.2.7, §5.2.2.9, §5.2.2.10.
- RFC 6455 §1.3, §4.2.2 (GUID 258EAFA5-E914-47DA-95CA-C5AB0DC85B11; example accept value), §5.1 (servers
  MUST NOT mask), §5.3. Note: a first draft of check_math.py had a mistyped GUID; the check failed against
  the RFC's own example value, which is exactly what the audit is for, and was corrected.
- RFC 9525 §2 (CN MUST NOT be used), §6.3 (wildcard matches one label). RFC 9213 (CDN-Cache-Control).
- RFC 6585 §4 (429) and RFC 5280 §4.1.2.5 / §6 are cited from memory of the documents' structure; their
  status (current) was not re-fetched. RFC 7838 (Alt-Svc, `ma` default 24 h) cited at RFC level.
- draft-ietf-httpapi-idempotency-key-header-09 §2.7: 400 missing key, 409 concurrent, 422 changed payload.
- draft-ietf-dnsop-aname: expired (last revision 2019-07-08).
- RFC 9369 (QUIC v2): purpose is to combat ossification and exercise version negotiation.
- root-servers.org (27 Nov 2026): 2,046 instances, 13 identities, 12 operators.
- AWS VPC docs: 5 reserved addresses per subnet; /28 smallest; NAT gateway 55,000 simultaneous connections
  per unique destination per IPv4 address, up to 8 addresses.

## Code examples executed

- UDP vs TCP boundary example (s11) and Linux `MSG_TRUNC` truncation: run with Python 3 in the sandbox;
  output matches the text.
- AES-GCM record example (s20): run with the `cryptography` package; prints `22` and `InvalidTag`.
- ETag `respond()` (s26): exercised for 200, weak-match 304 and list-match 304.
- WebSocket accept computation (s27): matches RFC 6455's example.
- C# snippets (s10 TCP_NODELAY, s23 idempotency-key client, s30 backoff) could not be compiled (no SDK);
  they use only BCL APIs (System.Net.Sockets, System.Net.Http.Json, Random.Shared) and C# 12 collection
  expressions.
- Command-line outputs (tcpdump, ss, dig, openssl, curl) are illustrative, abridged, with documentation
  addresses; the DNSSEC and early-data ones are explicitly labeled as illustrative in the text.

## Balancing passes

- Position rule: 13 single-answer questions received a fifth, misconception-based distractor (addopt.py)
  so that no position holds more than 40% of a section's correct answers.
- Option length: correct option strictly longest in 51 of 180 questions (28%), at most 2 per section,
  after shortening 29 correct options and lengthening 12 distractors (relen.py, relen2.py). Explanations
  quote distractors by prefix, so appended words did not break them.
