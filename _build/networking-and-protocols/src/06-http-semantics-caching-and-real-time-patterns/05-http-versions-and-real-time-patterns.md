---
title: "HTTP/1.1, HTTP/2, HTTP/3 and Real-Time Patterns"
body_checks:
  - "assert math.ceil(60 / 6) * 50 == 500"
  - "assert ws_accept('dGhlIHNhbXBsZSBub25jZQ==') == 's3pPLMBiTxaQ9kYGzzhZRbK+xOo='"
  - "assert round(10_000 / 30) == 333"
questions:
  - id: "pipelining-order"
    type: "single"
    prompt: "A client pipelines three requests on one HTTP/1.1 connection: A, which the server takes 2 seconds to answer, then B and C, which take 10 ms each. When can the client receive C's response?"
    correct:
      - "Only after A's response, at about 2 seconds, because responses must return in request order"
    distractors:
      - "After about 10 ms, because C is independent of A and the server answers it as soon as it is ready"
      - "Never, because servers reject a second request until the first response has been sent"
      - "After about 6 seconds, because the server processes pipelined requests one after another and each waits 2 s"
    explanation: |-
      HTTP/1.1 has no way to label which response belongs to which request; responses are matched to requests purely by order. RFC 9112 §9.3.2 therefore requires a server to send responses to pipelined requests in the order the requests were received. Even if B and C are ready after 10 ms, they wait behind A's 2-second response: head-of-line blocking at the HTTP layer. This is why browsers never enabled pipelining by default and opened several parallel connections instead.

      "After about 10 ms" describes HTTP/2 or HTTP/3, where streams are labeled and can complete out of order.

      "Servers reject a second request" is false; pipelining is permitted, only the ordering rule constrains it.

      "About 6 seconds" multiplies A's delay by three; B and C take 10 ms each, not 2 s.
  - id: "connections-versus-multiplexing"
    type: "single"
    prompt: "A page needs 60 small resources from one origin. The RTT is 50 ms, connections are already open, and each resource needs one round trip; bandwidth is not a constraint. The browser uses at most 6 HTTP/1.1 connections per origin without pipelining, or one HTTP/2 connection with all requests in flight at once. Roughly how long does fetching all 60 take in each case?"
    correct:
      - "About 500 ms over HTTP/1.1; about 50 ms over HTTP/2"
    distractors:
      - "About 3,000 ms over HTTP/1.1; about 50 ms over HTTP/2"
      - "About 500 ms over HTTP/1.1; about 500 ms over HTTP/2"
      - "About 50 ms over HTTP/1.1; about 50 ms over HTTP/2"
    explanation: |-
      - HTTP/1.1 without pipelining: each connection carries one request per round trip. Six connections give 6 requests per RTT, so 60 requests need ⌈60 / 6⌉ = 10 round trips = 500 ms.
      - HTTP/2 (RFC 9113): each request is a stream on one connection, and all 60 can be outstanding at once, so about one round trip, 50 ms.

      "3,000 ms" assumes a single HTTP/1.1 connection (60 sequential round trips).

      "500 ms over HTTP/2" ignores multiplexing, the main point of HTTP/2.

      "50 ms over HTTP/1.1" assumes unlimited parallel connections.
    checks:
      - "assert math.ceil(60 / 6) * 50 == 500 and 60 * 50 == 3000"
  - id: "sse-for-push-only"
    type: "single"
    prompt: "A monitoring dashboard needs the server to push status updates to browsers. Clients send nothing after subscribing, the updates must pass through proxies that handle only ordinary HTTP responses, and clients should resume from the last event after a disconnect. Which mechanism fits best?"
    correct:
      - "Server-sent events: one long-lived `text/event-stream` response, with reconnection that sends `Last-Event-ID`"
    distractors:
      - "WebSockets, because they are the only mechanism with which a server can send data without a request"
      - "Polling with `GET` every 100 ms, because frequent short requests pass through every kind of proxy"
      - "A WebSocket per update, opened by the server toward each browser whenever a new status is ready"
    explanation: |-
      Server-sent events (the HTML standard's `EventSource`) are a plain HTTP response with content type `text/event-stream` that never ends; the server writes events as lines of text. The traffic is one-directional (server to client), which matches the need exactly, and it is an ordinary HTTP response, so HTTP proxies can pass it as long as they do not buffer it. Each event may carry an `id`; on reconnect the browser sends `Last-Event-ID`, so the server can resume.

      "WebSockets are the only way" is wrong: SSE and long polling both push data over HTTP. WebSockets fit when the client must also send a stream of messages.

      "Poll every 100 ms" works but multiplies requests by orders of magnitude and still adds up to 100 ms of latency.

      "Opened by the server toward each browser" is impossible: browsers do not accept inbound connections; every mechanism here is client-initiated.
  - id: "websocket-accept"
    type: "single"
    prompt: "A WebSocket client sends `Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==`. What `Sec-WebSocket-Accept` value must the server return?"
    correct:
      - "`s3pPLMBiTxaQ9kYGzzhZRbK+xOo=`"
    distractors:
      - "`dGhlIHNhbXBsZSBub25jZQ==`"
      - "`b37a4f2cc0624f1690f64606cf385945b2bec4ea`"
      - "`hHLtf2V1k8aDQZfNjw3Ia1hCwt0=`"
    explanation: |-
      RFC 6455 §4.2.2: concatenate the key with the fixed GUID `258EAFA5-E914-47DA-95CA-C5AB0DC85B11`, take the SHA-1 hash, and base64-encode the 20-byte digest. For this key (the RFC's own example) the result is `s3pPLMBiTxaQ9kYGzzhZRbK+xOo=`. The exchange proves that the server understood the WebSocket handshake rather than being an HTTP server that echoes headers; it is not a security mechanism.

      "`dGhlIHNhbXBsZSBub25jZQ==`" echoes the key, which is exactly the behavior the transformation is designed to distinguish from a real WebSocket server.

      "`b37a4f2c…`" is the correct SHA-1 digest written in hex instead of base64.

      "`hHLtf2V1…`" hashes the key without appending the GUID.
    checks:
      - "assert ws_accept('dGhlIHNhbXBsZSBub25jZQ==') == 's3pPLMBiTxaQ9kYGzzhZRbK+xOo='"
      - "g = '258EAFA5-E914-47DA-95CA-C5AB0DC85B11'; k = 'dGhlIHNhbXBsZSBub25jZQ=='; assert hashlib.sha1((k + g).encode()).hexdigest() == 'b37a4f2cc0624f1690f64606cf385945b2bec4ea' and base64.b64encode(hashlib.sha1(k.encode()).digest()).decode() == 'hHLtf2V1k8aDQZfNjw3Ia1hCwt0='"
  - id: "long-poll-request-rate"
    type: "single"
    prompt: "10,000 browsers use long polling: each request is held open until an event arrives or 30 seconds pass, and the client immediately sends a new request when one ends. Events are rare. About how many requests per second does the server handle at steady state?"
    correct:
      - "About 333"
    distractors:
      - "About 10,000"
      - "About 33"
      - "About 300,000"
    explanation: |-
      With rare events nearly every request ends by timeout, so each client sends one request every 30 seconds. By Little's law, 10,000 outstanding requests each lasting 30 seconds means 10,000 / 30 ≈ 333 requests per second arrive. Each of them still carries full HTTP headers and passes through the whole stack, which is the overhead SSE and WebSockets avoid by keeping one stream open.

      "About 10,000" counts open requests, not arrivals per second.

      "About 33" divides by 300 instead of 30.

      "About 300,000" multiplies by 30 instead of dividing.
    checks:
      - "assert round(10_000 / 30) == 333 and round(10_000 / 300) == 33 and 10_000 * 30 == 300_000"
  - id: "websocket-protocol-facts"
    type: "multiple"
    prompt: "Which statements about the WebSocket protocol (RFC 6455) are true? Select all that apply."
    correct:
      - "The connection starts as an HTTP/1.1 request with `Upgrade: websocket` and switches with `101 Switching Protocols`"
      - "Every frame sent from the client to the server must be masked"
    distractors:
      - "After the upgrade, data flows as an unframed byte stream with no message boundaries"
      - "Frames from the server to the client must be masked with a key that the client chose"
      - "WebSocket connections run over UDP so that a lost packet does not block later messages"
    explanation: |-
      RFC 6455 opens with an HTTP/1.1 GET carrying `Upgrade: websocket`, `Connection: Upgrade`, `Sec-WebSocket-Key` and `Sec-WebSocket-Version: 13`; the server answers `101 Switching Protocols` with `Sec-WebSocket-Accept`, and the same TCP connection then carries WebSocket frames. Clients must mask every frame they send with a fresh 32-bit key, which prevents attacker-controlled bytes from looking like HTTP to intermediaries that do not understand WebSockets (cache-poisoning attacks).

      "An unframed byte stream" is wrong: WebSockets add framing and message boundaries on top of TCP; that is a large part of their value.

      "Server frames masked with the client's key" inverts the rule: servers must *not* mask their frames.

      "Over UDP" is wrong: WebSockets run over TCP (or, via later RFCs, as streams of HTTP/2 or HTTP/3 connections), so a loss on TCP still blocks later messages.
---

## Learning objectives

- Explain how HTTP/1.1, HTTP/2 and HTTP/3 differ in concurrency and head-of-line blocking.
- Estimate page fetch times from connection limits and RTT.
- Choose between long polling, server-sent events and WebSockets.
- Compute the WebSocket handshake value and state its framing rules.

## The invariant

HTTP's semantics (methods, status codes, headers) are the same in every version (RFC 9110). What changes is how messages share connections:

| Version | Framing | Concurrency per connection | Head-of-line blocking |
|---|---|---|---|
| HTTP/1.1 (RFC 9112) | text | one request at a time; pipelined responses must keep request order (§9.3.2) | at the HTTP layer and in TCP |
| HTTP/2 (RFC 9113) | binary frames on numbered streams, HPACK | many concurrent streams | in TCP only |
| HTTP/3 (RFC 9114) | frames on QUIC streams, QPACK | many concurrent streams | none across streams |

Because HTTP/1.1 responses are matched to requests by order, one slow response delays every later one on the connection, so browsers open several connections per origin instead of pipelining. HTTP/2 labels every frame with a stream ID, so responses interleave and complete out of order, but all streams share one TCP byte stream and stall together on a lost packet. HTTP/3 moves the streams into QUIC, where each is delivered independently.

**Server-to-client delivery** has three common patterns:

- **Long polling**: the client sends a request that the server holds until an event or a timeout, then immediately sends another. Works everywhere; costs one request per event or timeout.
- **Server-sent events**: one long-lived HTTP response of type `text/event-stream`; one-directional; built-in reconnection with `Last-Event-ID`.
- **WebSockets** (RFC 6455): an HTTP/1.1 Upgrade handshake answered with `101 Switching Protocols`, then a full-duplex stream of framed messages on the same TCP connection. The server proves it speaks the protocol by returning base64(SHA-1(key + `258EAFA5-E914-47DA-95CA-C5AB0DC85B11`)); clients mask every frame they send.

## Worked example

A WebSocket handshake, using RFC 6455's example key:

```bash
GET /chat HTTP/1.1
Host: server.example.com
Upgrade: websocket
Connection: Upgrade
Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==
Sec-WebSocket-Version: 13

HTTP/1.1 101 Switching Protocols
Upgrade: websocket
Connection: Upgrade
Sec-WebSocket-Accept: s3pPLMBiTxaQ9kYGzzhZRbK+xOo=
```

```python
import base64, hashlib
key = "dGhlIHNhbXBsZSBub25jZQ=="
guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
print(base64.b64encode(hashlib.sha1((key + guid).encode()).digest()).decode())
```

Output:

```bash
s3pPLMBiTxaQ9kYGzzhZRbK+xOo=
```

Concurrency arithmetic for 60 one-round-trip resources at 50 ms RTT: six HTTP/1.1 connections need ⌈60 / 6⌉ = 10 round trips (500 ms); one HTTP/2 connection needs about one (50 ms). Long polling 10,000 idle clients with a 30-second timeout costs about 10,000 / 30 ≈ 333 requests per second even when nothing happens.

## Where this shows up today

As of November 2026, RFC 9113 deprecates the stream-priority signaling of the original HTTP/2 specification (§5.3.2), and RFC 6455 has been updated by RFC 8441, which carries WebSockets over HTTP/2 streams with an extended CONNECT method instead of a separate TCP connection. The WebSocket message model is unchanged; only the transport underneath it varies.

## Pitfalls checklist

- Expecting HTTP/2 to remove head-of-line blocking on lossy networks; TCP still serializes.
- Choosing WebSockets for one-way push that SSE would handle with plain HTTP.
- Proxies that buffer `text/event-stream` responses and deliver events in bursts.
- Long-polling designs whose timeouts are shorter than intermediaries' idle timeouts, or much longer.
- Treating `Sec-WebSocket-Accept` as authentication; it only proves protocol awareness.

## Key takeaways

- HTTP/1.1 orders responses per connection; HTTP/2 multiplexes on one TCP connection; HTTP/3 makes streams independent.
- Six connections × one request per RTT bounds HTTP/1.1 page loads.
- SSE for one-way push over HTTP, WebSockets for full-duplex messaging, long polling as a fallback.
- `Sec-WebSocket-Accept` = base64(SHA-1(key + GUID)); clients mask, servers do not.

## Sources

- RFC 9110 (semantics); RFC 9112 §9.3.2 (pipelining order); RFC 9113 §5.3.2 (HTTP/2, priority deprecation); RFC 9114 (HTTP/3).
- RFC 6455, The WebSocket Protocol (§4.2.2 handshake, §5.3 masking); RFC 8441, Bootstrapping WebSockets with HTTP/2.
- WHATWG HTML Living Standard, "Server-sent events".
