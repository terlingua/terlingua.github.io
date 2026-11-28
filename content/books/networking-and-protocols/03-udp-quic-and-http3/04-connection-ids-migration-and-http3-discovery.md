---
title: "Connection IDs, Migration and Discovering HTTP/3"
questions:
  - id: "nat-rebinding-survives"
    type: "single"
    prompt: "A phone's QUIC connection sits idle until its NAT mapping expires. The next packet leaves the NAT from a new public port. What happens to the QUIC connection, and what would happen to a TCP connection on the same path?"
    explanation: |-
      TCP identifies a connection by its 4-tuple (addresses and ports). A segment from a new source port matches no connection, so the server answers with RST or drops it, and the connection is lost.

      QUIC (RFC 9000) carries a **Destination Connection ID** in every packet, chosen by the receiver. The server looks the connection up by that ID, so a change of address or port, whether from NAT rebinding or a deliberate move from Wi-Fi to cellular, does not lose the connection. The server then validates the new path before sending much to it.

      "Both break" assumes QUIC uses the 4-tuple like TCP; connection IDs exist to avoid that.

      "The NAT restores the old port" is not how NATs work: an expired mapping is gone, and a new one is created with whatever port the NAT picks.

      "QUIC breaks; TCP continues" reverses the two transports.
    options:
      - text: "Both connections break, because every transport identifies a connection by its source address and port"
      - text: "Both connections continue, because the NAT restores the old public port as soon as it sees the packet"
      - text: "The QUIC connection breaks until a new handshake runs; the TCP connection continues on the new port"
      - text: "QUIC continues, since the server finds it by connection ID; TCP breaks, since its 4-tuple changed"
        correct: true
  - id: "path-validation"
    type: "single"
    prompt: "After a client migrates to a new address, packets with a known connection ID start arriving from that address. What must the server do before it sends large amounts of data there?"
    explanation: |-
      A connection ID proves that the sender knows the ID, not that the new source address is genuine: an attacker who observed the ID could forge packets from a victim's address to make the server flood it. RFC 9000 §8.2 therefore has the endpoint send a PATH_CHALLENGE frame with unpredictable data to the new address and wait for a matching PATH_RESPONSE. Until the path is validated, the anti-amplification limit (3× the bytes received) applies, and the congestion controller is reset for the new path, because its capacity is unknown.

      "Repeat the TLS handshake" is unnecessary: the connection's keys are not tied to addresses, which is why migration is cheap.

      "A valid connection ID proves reachability" is the spoofing hole path validation closes.

      "Resend unacknowledged data" confuses path validation with loss recovery; retransmission is driven by acknowledgments, not by migration.
    options:
      - text: "Validate the new path with PATH_CHALLENGE and PATH_RESPONSE, sending at most three times what it received until then"
        correct: true
      - text: "Repeat the whole TLS handshake on the new path so that fresh keys are bound to the new client address"
      - text: "Nothing, because a packet carrying a valid connection ID proves that the client is reachable there"
      - text: "Ask the client to resend all unacknowledged stream data so the server can verify it on the new path"
  - id: "alt-svc-first-visit"
    type: "single"
    prompt: "A browser with an empty cache visits `https://shop.example/` for the first time. The server supports HTTP/2 and HTTP/3, has no HTTPS DNS record, and sends `Alt-Svc: h3=\":443\"; ma=86400`. Which protocols carry the first request and a request one hour later?"
    explanation: |-
      Without other information, a client starts with TCP, because it cannot know that a UDP endpoint exists. RFC 9114 says a server advertises HTTP/3 with the Alt-Svc header field (RFC 7838) using the ALPN token `h3`. `ma=86400` says the advertisement stays fresh for 86,400 seconds (24 hours), so later requests within that time may go over QUIC to port 443.

      "Learns from the TLS handshake" is wrong: ALPN on a TCP connection chooses between protocols that run over TCP; it cannot announce a UDP service.

      "First goes over HTTP/3" requires prior knowledge, such as an HTTPS DNS record advertising `alpn=h3`, which this site does not publish.

      "Only after a restart" is invented; Alt-Svc applies to subsequent requests as soon as it is cached.
    options:
      - text: "Both go over HTTP/3, because the browser learns that HTTP/3 is available from the TLS handshake of the first connection"
      - text: "The first request goes over TCP (HTTP/2); the later one can use HTTP/3, because the alternative is cached for 86,400 seconds"
        correct: true
      - text: "The first goes over HTTP/3 and falls back to HTTP/2 only if the QUIC handshake fails within its timeout"
      - text: "Both go over HTTP/2, because an Alt-Svc advertisement only takes effect after the browser is restarted"
  - id: "alt-svc-max-age"
    type: "single"
    prompt: "A client receives `Alt-Svc: h3=\":443\"; ma=3600` at 09:00. Is the alternative still fresh at 09:45 and at 10:30?"
    explanation: |-
      `ma` (max-age) is the number of seconds the advertisement stays fresh after it was received (RFC 7838; the default is 24 hours when `ma` is absent).

      - 09:45 is 45 × 60 = 2,700 seconds later: 2,700 < 3,600, so fresh.
      - 10:30 is 90 × 60 = 5,400 seconds later: 5,400 > 3,600, so expired.

      "Fresh at both times" ignores the 3,600-second limit, perhaps assuming the 24-hour default.

      "Expired at both times" reads `ma=3600` as 3,600 milliseconds, which would make the entry stale almost at once; the unit is seconds.

      "Fresh again at 10:30" invents a renewal; only a new Alt-Svc header refreshes the entry.
    options:
      - text: "Fresh at both times"
      - text: "Expired at both times"
      - text: "Fresh at 09:45, expired at 10:30"
        correct: true
      - text: "Expired at 09:45, fresh again at 10:30"
  - id: "load-balancer-five-tuple-bug"
    type: "single"
    prompt: "A fleet of QUIC servers sits behind a layer-4 load balancer that picks a backend by hashing each UDP datagram's 5-tuple. Mobile users report connections that die when they switch networks or after idle periods. What is the fix?"
    explanation: |-
      Hashing the 5-tuple works for TCP because a TCP connection's 4-tuple never changes. A QUIC connection's 4-tuple *can* change (NAT rebinding, migration). After the change, the hash sends the datagrams to a different backend, which has no state for that connection ID and drops them or answers with a stateless reset. The connection only survives if every backend-selection step keys on something stable: the connection ID. Servers choose the IDs they ask peers to use, so they can embed a routable backend identifier (the approach of the IETF QUIC-LB work), and the load balancer routes on it.

      "Disable UDP checksums" changes nothing about which backend is chosen.

      "Raise the idle timeout" does not control the NAT, whose own mapping timeout causes rebinding, and does nothing for deliberate network switches.

      "Pad Initial packets" affects the anti-amplification budget, not routing.
    options:
      - text: "Disable UDP checksums so the load balancer can forward datagrams without inspecting their headers"
      - text: "Raise the servers' idle timeout so that NAT mappings never change while connections are idle"
      - text: "Pad every client Initial packet to the full path MTU so that the hash spreads clients more evenly"
      - text: "Route by connection ID, for example by encoding a backend identifier in the IDs the servers issue"
        correct: true
  - id: "udp-blocked-fallback"
    type: "single"
    prompt: "A corporate firewall drops all outbound UDP to port 443. An employee's browser has a cached `h3` Alt-Svc entry for a site. What happens?"
    explanation: |-
      RFC 9114 expects clients to fall back to TCP-based HTTP when QUIC cannot be established, for example because UDP is blocked. Alternative services are optional: the origin remains reachable over TCP, and RFC 7838 lets a client ignore an alternative that does not work. Browsers typically race QUIC against TCP or fall back after a short timeout and remember that QUIC failed on this network, so users see at most a small delay.

      "Fails until the entry expires" would make any UDP block a site outage; clients are not required to use an alternative.

      "Tunnels QUIC inside TCP" is not something browsers do; HTTP/2 over TCP is the fallback.

      "The firewall rewrites QUIC into TCP" is impossible: QUIC packets are encrypted, and the two transports are not translatable.
    options:
      - text: "The QUIC attempt gets no answer, so the browser uses or races a TCP connection and loads the site over HTTP/2"
        correct: true
      - text: "The page fails to load until the Alt-Svc entry expires, because the browser must honor the cached alternative"
      - text: "The browser tunnels QUIC packets inside the TCP connection so the site still loads over HTTP/3"
      - text: "The firewall rewrites the QUIC packets into TCP segments, so the HTTP/3 connection completes over TCP"
---
## Learning objectives

- Explain why QUIC connections survive address changes and TCP connections do not.
- Describe path validation and its anti-spoofing role.
- Trace how clients discover HTTP/3 with Alt-Svc or HTTPS records, and fall back when UDP is blocked.
- Route QUIC through load balancers without breaking migration.

## The invariant

A TCP connection *is* its 4-tuple: source address, source port, destination address, destination port. If any of them changes, the connection is gone.

QUIC (RFC 9000) decouples connections from addresses with **connection IDs**. Each endpoint issues IDs that its peer puts in the Destination Connection ID field of every packet, and the receiver finds the connection by that ID. When the client's address or port changes, through NAT rebinding or a deliberate move from Wi-Fi to cellular (**connection migration**, §9), packets still reach the right connection. Endpoints switch to fresh, unlinkable connection IDs when they change paths so that observers cannot correlate the two paths.

An ID is not proof of address ownership. Before trusting a new path, an endpoint performs **path validation** (§8.2): it sends PATH_CHALLENGE with unpredictable data and waits for the matching PATH_RESPONSE. Until then it obeys the 3× anti-amplification limit, and congestion state restarts for the new path.

Anything that steers packets by 4-tuple breaks this. A load balancer hashing the 5-tuple sends post-migration packets to the wrong backend; QUIC deployments route on connection IDs, typically by encoding a backend identifier in the IDs the servers issue.

**Discovering HTTP/3.** A client cannot know a server speaks QUIC on UDP until something tells it:

- **Alt-Svc** (RFC 7838, used by RFC 9114): an HTTP/1.1 or HTTP/2 response carries `Alt-Svc: h3=":443"; ma=86400`. Later requests may use HTTP/3 for `ma` seconds (default 24 hours). The first visit uses TCP.
- **HTTPS DNS records** (RFC 9460): `alpn="h3,h2"` in the record lets a client try QUIC on its very first connection.

If QUIC fails (UDP blocked, handshake timeout), clients fall back to TCP; alternatives are optional by design.

## Worked example

A phone downloads a large file over HTTP/3 and walks out of Wi-Fi range.

| Time | Event | TCP equivalent |
|---|---|---|
| t | Packets from `198.51.100.7:50000`, DCID `c4a1…` | 4-tuple fixed |
| t + 1 s | Packets now from cellular `203.0.113.40:61002`, same connection, new DCID `9e07…` | Connection lost; new handshake, HTTP range request |
| t + 1 s + 1 RTT | Server's PATH_CHALLENGE answered; path validated | — |
| afterwards | Transfer resumes from the current stream offset; congestion window rebuilt on the new path | — |

Alt-Svc freshness arithmetic: received at 09:00 with `ma=3600`, the alternative is fresh at 09:45 (2,700 s elapsed) and expired at 10:30 (5,400 s).

What a first visit looks like from the command line (with a QUIC-capable curl):

```bash
$ curl -sI https://shop.example/ | grep -i -E '^(HTTP|alt-svc)'
HTTP/2 200
alt-svc: h3=":443"; ma=86400
$ curl -sI --http3-only https://shop.example/ | head -1
HTTP/3 200
```

## Where this shows up today

As of November 2026, HTTP/3 is enabled by default in Chrome and Edge (since version 87), Firefox (since 88) and Safari (since 18), according to the web-platform-dx web-features data. Enterprise networks that block outbound UDP 443 remain common enough that clients keep TCP fallback; the user-visible cost is the extra time before falling back, which is why browsers race the two.

## Pitfalls checklist

- Load balancing QUIC by 5-tuple and losing connections on every NAT rebinding.
- Assuming a connection ID authenticates an address; validate the path.
- Expecting HTTP/3 on a first visit without an HTTPS DNS record.
- Treating "UDP blocked" as an outage; clients fall back to TCP.
- Setting a tiny Alt-Svc `ma` and sending every visit back to TCP first.

## Key takeaways

- TCP connections are their 4-tuple; QUIC connections are found by connection ID.
- New paths are validated with PATH_CHALLENGE/PATH_RESPONSE before full use.
- Clients learn about HTTP/3 from Alt-Svc (second visit) or HTTPS records (first visit).
- If QUIC cannot connect, clients use HTTP/2 or HTTP/1.1 over TCP.

## Sources

- RFC 9000 §2.1 (stream types), §8.2 (path validation), §9 (connection migration).
- RFC 9114, HTTP/3 (discovery via Alt-Svc, `h3` ALPN token, fallback).
- RFC 7838, HTTP Alternative Services; RFC 9460, SVCB and HTTPS Resource Records.
- web-platform-dx web-features, "HTTP/3" (browser support data), read November 2026.
