---
title: "End-to-End Latency Counting"
body_checks:
  - "assert math.ceil(30_000 / 1460) == 21 and 21 - 10 == 11"
  - "assert 5 + 3 * 40 + 15 == 140 and 5 + 2 * 40 + 15 == 100 and 40 + 15 == 55"
questions:
  - id: "cold-https-tcp"
    type: "single"
    prompt: "A browser loads `https://api.example/` on a fresh start. Its resolver is 10 ms away and has the name cached. The server is 60 ms away (RTT), uses HTTP/1.1 over TLS 1.3 on TCP, and spends 20 ms producing a small response. When does the first response byte arrive?"
    correct:
      - "210 ms"
    distractors:
      - "270 ms"
      - "150 ms"
      - "200 ms"
      - "230 ms"
    explanation: |-
      Add the sequential steps:

      | Step | Time |
      |---|---|
      | DNS query to the resolver (cache hit) | 10 ms |
      | TCP handshake (1 RTT) | 60 ms |
      | TLS 1.3 handshake (1 RTT) | 60 ms |
      | Request and response (1 RTT) | 60 ms |
      | Server processing | 20 ms |
      | **Total** | **210 ms** |

      "270 ms" uses TLS 1.2's two round trips.

      "150 ms" is the HTTP/3 figure, where the transport and TLS handshakes are a single round trip.

      "200 ms" leaves out the DNS lookup.

      "230 ms" counts the 20 ms of server processing twice.
    checks:
      - "assert 10 + 60 + 60 + 60 + 20 == 210 and 10 + 60 + 120 + 60 + 20 == 270 and 10 + 60 + 60 + 20 == 150 and 60 * 3 + 20 == 200"
  - id: "cold-http3"
    type: "single"
    prompt: "Same conditions as before (resolver 10 ms away with the name cached, server RTT 60 ms, 20 ms of server time), but the browser already knows the server supports HTTP/3 and opens a new QUIC connection. When does the first response byte arrive?"
    correct:
      - "150 ms"
    distractors:
      - "210 ms"
      - "90 ms"
      - "140 ms"
    explanation: |-
      QUIC combines the transport and TLS 1.3 handshakes into one round trip (RFC 9001), after which the request goes out with the client's Finished.

      10 (DNS) + 60 (QUIC handshake) + 60 (request and response) + 20 (server) = 150 ms.

      "210 ms" is the TCP + TLS 1.3 figure, with separate transport and TLS round trips.

      "90 ms" assumes 0-RTT, which requires a session ticket from an earlier connection.

      "140 ms" leaves out the DNS lookup.
    checks:
      - "assert 10 + 60 + 60 + 20 == 150 and 10 + 60 + 20 == 90 and 60 + 60 + 20 == 140"
  - id: "warm-and-resumed"
    type: "multiple"
    prompt: "The name is in the browser's own DNS cache (0 ms), the server RTT is 60 ms, server time is 20 ms, and the browser holds a valid session ticket. Which timings to the first response byte are correct? Select all that apply."
    correct:
      - "A new QUIC connection sending the request as 0-RTT data: 80 ms"
      - "A new TCP connection with TLS 1.3 sending the request as early data: 140 ms"
    distractors:
      - "A new QUIC connection sending the request as 0-RTT data: 20 ms"
      - "Reusing an existing HTTP/2 connection to the server: 140 ms"
      - "A new TCP connection with a full TLS 1.3 handshake and no early data: 140 ms"
    explanation: |-
      - QUIC 0-RTT: the request rides in the first flight; the response comes back one RTT later: 60 + 20 = 80 ms.
      - TCP + TLS 1.3 early data: the TCP handshake still costs a round trip, then the ClientHello carries the request: 60 + 60 + 20 = 140 ms.

      Distractors:

      - "QUIC 0-RTT at 20 ms" forgets that the response still needs a round trip to travel.
      - "Reusing an existing HTTP/2 connection" needs no handshake at all: 60 + 20 = 80 ms, not 140 ms.
      - "Full TLS 1.3 handshake on a new TCP connection" costs TCP + TLS + request: 60 + 60 + 60 + 20 = 200 ms.
    checks:
      - "assert 60 + 20 == 80 and 60 + 60 + 20 == 140 and 3 * 60 + 20 == 200"
  - id: "redirect-chain-cost"
    type: "single"
    prompt: "A user types `example.com`. The browser requests `http://example.com/`, which redirects (301) to `https://example.com/`, which redirects (301) to `https://www.example.com/`. Each new name costs 20 ms of DNS (resolver hit), the RTT is 50 ms for all hosts, TLS 1.3 over TCP is used, each scheme change or host change needs a new connection, and server time is negligible. When does the first byte of the final page arrive?"
    correct:
      - "440 ms"
    distractors:
      - "170 ms"
      - "420 ms"
      - "340 ms"
    explanation: |-
      | Request | DNS | TCP | TLS | HTTP | Subtotal |
      |---|---|---|---|---|---|
      | `http://example.com/` | 20 | 50 | — | 50 | 120 |
      | `https://example.com/` (port 443, name cached) | 0 | 50 | 50 | 50 | 150 |
      | `https://www.example.com/` (new name) | 20 | 50 | 50 | 50 | 170 |
      | **Total** | | | | | **440 ms** |

      Linking directly to `https://www.example.com/` would cost 170 ms, the last row alone. (HSTS lets browsers skip the plain-HTTP hop on later visits.)

      "170 ms" is the cost without redirects.

      "420 ms" forgets the DNS lookup for `www.example.com`.

      "340 ms" forgets both TLS handshakes.
    checks:
      - "assert (20 + 50 + 50) + (50 + 50 + 50) + (20 + 50 + 50 + 50) == 440 and 440 - 20 == 420 and 440 - 100 == 340"
  - id: "dns-cold-versus-warm"
    type: "single"
    prompt: "A stub resolver is 5 ms from its recursive resolver. The resolver reaches a root server in 10 ms, a `com` server in 25 ms and the `example.com` authoritative server in 40 ms (RTTs; glue everywhere). What does DNS add to a lookup of `api.example.com` when the resolver's cache holds only the root hints, and later when it also holds the `com` and `example.com` delegations but not the answer?"
    correct:
      - "80 ms, then 45 ms"
    distractors:
      - "75 ms, then 40 ms"
      - "80 ms, then 5 ms"
      - "105 ms, then 45 ms"
    explanation: |-
      Cold cache: the stub's round trip to the resolver plus one iterative query per zone cut: 5 + 10 (root) + 25 (`com`) + 40 (`example.com`) = 80 ms.

      With the delegations cached, the resolver goes straight to the `example.com` server: 5 + 40 = 45 ms.

      "75 ms, then 40 ms" omits the stub-to-resolver round trip.

      "80 ms, then 5 ms" treats cached delegations as a cached answer; the A record itself was not cached.

      "105 ms, then 45 ms" counts the `com` query twice (or adds an extra referral).
    checks:
      - "assert 5 + 10 + 25 + 40 == 80 and 5 + 40 == 45"
  - id: "slow-start-transfer-time"
    type: "single"
    prompt: "On an already-open TCP connection with an RTT of 60 ms, a server answers instantly with a 300,000-byte response. The congestion window starts at 10 segments of 1,460 bytes, doubles every RTT, and there is no loss and no receive-window limit. About how long after sending the request does the client receive the last byte?"
    correct:
      - "About 300 ms"
    distractors:
      - "About 60 ms"
      - "About 240 ms"
      - "About 360 ms"
      - "About 480 ms"
    explanation: |-
      300,000 / 1,460 = 205.5, so 206 segments. Slow start sends flights of 10, 20, 40, 80, 160 segments; flight k arrives k RTTs after the request:

      | Flight | Segments | Cumulative | Arrives at |
      |---|---|---|---|
      | 1 | 10 | 10 | 60 ms |
      | 2 | 20 | 30 | 120 ms |
      | 3 | 40 | 70 | 180 ms |
      | 4 | 80 | 150 | 240 ms |
      | 5 | 56 of 160 allowed | 206 | 300 ms |

      "About 60 ms" assumes the whole response fits in the first window, as if bandwidth were the only limit.

      "About 240 ms" stops after four flights, which deliver only 150 segments.

      "About 360 ms" adds a sixth round trip that is not needed.

      "About 480 ms" starts slow start from one segment (cumulative 1, 3, 7, …, 255 segments), needing eight round trips instead of five.
    checks:
      - "segs = math.ceil(300_000 / 1460); cw, sent, rtts = 10, 0, 0\nwhile sent < segs:\n    sent += cw; cw *= 2; rtts += 1\nassert segs == 206 and rtts == 5 and rtts * 60 == 300"
---

## Learning objectives

- Add DNS, transport, TLS and HTTP round trips into a time to first byte.
- Compare cold, warm and resumed connections over TCP and QUIC.
- Price redirect chains and DNS cache states.
- Include slow start in transfer time.

## The invariant

The latency of a request is a sum of **sequential** waits. Most of them are round trips, so the RTT is the unit of account:

| Component | Cold cost | Warm cost |
|---|---|---|
| DNS | stub→resolver RTT, plus one query per uncached zone cut | 0 if the client cached it, else one stub→resolver RTT |
| TCP handshake | 1 RTT | 0 on a reused connection |
| TLS 1.3 | 1 RTT (2 with HelloRetryRequest; TLS 1.2 costs 2) | 0 on reuse; resumption still 1 RTT, or 0 for early data |
| QUIC + TLS 1.3 | 1 RTT combined | 0 with 0-RTT data |
| HTTP request/response | 1 RTT + server time | same |
| Body transfer | slow-start rounds: flights of 10, 20, 40, … segments | fewer if the connection's window is already large |

Things that change the count:

- **Connection reuse** removes the handshake rows entirely, which is why connection pools and HTTP/2 multiplexing matter more than any micro-optimization.
- **Redirects** to another scheme or host repeat DNS and handshakes; each one can cost more than the final request.
- **Discovery**: HTTP/3 needs an Alt-Svc from an earlier response or an HTTPS DNS record, so a first visit without the record uses TCP.
- **0-RTT** sends a replay-safe request in the first flight, but a new TCP connection still pays its own handshake.

## Worked example

RTT 40 ms, resolver 5 ms away with the name cached, server time 15 ms, a 30 KB response (21 segments: 10 + 11, so two slow-start flights).

| Scenario | Setup | First byte | Last byte |
|---|---|---|---|
| Cold, TCP + TLS 1.3 | 5 + 40 + 40 | 5 + 3 × 40 + 15 = 140 ms | 180 ms |
| Cold, QUIC | 5 + 40 | 5 + 2 × 40 + 15 = 100 ms | 140 ms |
| Reused connection | 0 | 40 + 15 = 55 ms | 95 ms (window may already be larger) |
| QUIC 0-RTT, DNS cached locally | 0 | 40 + 15 = 55 ms | 95 ms |

The second flight of 11 segments adds one RTT after the first byte. Measured with curl, the same decomposition appears directly:

```bash
$ curl -so /dev/null -w 'dns=%{time_namelookup} tcp=%{time_connect} tls=%{time_appconnect} ttfb=%{time_starttransfer} total=%{time_total}\n' https://api.example/
dns=0.005 tcp=0.045 tls=0.085 ttfb=0.140 total=0.180
```

Each curl value is cumulative from the start: TCP ends at 45 ms (5 + 40), TLS at 85 ms, the first byte at 140 ms and the last at 180 ms.

## Where this shows up today

As of November 2026, Chrome, Edge, Firefox and Safari enable HTTP/3 by default, so whether a cold page load pays two round trips of setup (TCP + TLS 1.3) or one (QUIC) depends mostly on whether the site advertises HTTP/3 (Alt-Svc or an HTTPS record) and whether UDP is open on the path. Redirect chains and cold DNS still cost the same either way.

## Pitfalls checklist

- Quoting server processing time as "latency" and ignoring three or more round trips of setup.
- Chains of scheme and host redirects on landing pages.
- Disabling connection reuse in clients and paying handshakes on every request.
- Assuming 0-RTT removes the TCP handshake.
- Measuring with large RTT-insensitive downloads and missing slow-start costs on small ones.

## Key takeaways

- Cold HTTPS over TCP + TLS 1.3 is DNS + 3 RTT + server time to first byte; QUIC saves one RTT.
- Reused connections and 0-RTT remove setup round trips; nothing removes the request's own round trip.
- Every redirect to a new scheme or host repeats DNS and handshakes.
- Body size adds slow-start rounds: 10, 20, 40, 80 segments per RTT.

## Sources

- RFC 9293 (TCP); RFC 9846 (TLS 1.3); RFC 9000/9001 (QUIC handshake); RFC 9114 and RFC 7838 (HTTP/3 discovery).
- RFC 1034 (resolution); RFC 5681 and RFC 6928 (slow start, initial window).
- web-platform-dx web-features, "HTTP/3", read November 2026.
