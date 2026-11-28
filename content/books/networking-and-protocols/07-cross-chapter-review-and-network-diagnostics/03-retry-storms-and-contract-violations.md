---
title: "Retry Storms and Protocol-Contract Violations"
questions:
  - id: "layered-retry-amplification"
    type: "single"
    prompt: "A mobile app calls an API gateway, which calls an order service, which calls a database. Each of the three callers makes up to 3 attempts (1 try + 2 retries) when its call fails. The database is down. How many database attempts can one user action cause?"
    explanation: |-
      Retries multiply across layers. Each app attempt reaches the gateway; each gateway attempt reaches the service; each service attempt reaches the database:

      3 (app) × 3 (gateway) × 3 (service) = 27 database attempts.

      During an outage this turns a fixed user load into 27 times the load on the component that is already failing, which is how retry storms delay recovery. Retry at one layer only (usually the one closest to the user, or the one that owns the idempotency key), use retry budgets, and fail fast below it.

      "9" counts only two layers of retries.

      "3" counts only the service's own attempts.

      "81" adds a fourth layer that does not exist.
    options:
      - text: "9"
      - text: "3"
      - text: "81"
      - text: "27"
        correct: true
  - id: "capped-backoff-total"
    type: "single"
    prompt: "A client retries up to 5 times with exponential backoff: the k-th wait (k = 0, 1, 2, …) is 100 ms × 2^k, capped at 1 second, with no jitter. How long does it spend waiting in total if every attempt fails?"
    explanation: |-
      The waits are 100 × 2^k for k = 0…4, each capped at 1,000 ms:

      | Retry | Uncapped | Capped |
      |---|---|---|
      | 1 | 100 | 100 |
      | 2 | 200 | 200 |
      | 3 | 400 | 400 |
      | 4 | 800 | 800 |
      | 5 | 1,600 | 1,000 |
      | **Total** | 3,100 | **2,500 ms** |

      "3,100 ms" ignores the cap.

      "1,500 ms" stops after four waits (100 + 200 + 400 + 800).

      "500 ms" uses a constant 100 ms wait, which is not backoff at all.

      In practice each wait should also be randomized ("full jitter": a uniform random delay between 0 and the capped value), which halves the expected wait and, more importantly, keeps clients that failed together from retrying together.

      "5,000 ms" assumes every one of the five waits hits the 1-second cap.
    options:
      - text: "3,100 ms"
      - text: "1,500 ms"
      - text: "2,500 ms"
        correct: true
      - text: "500 ms"
      - text: "5,000 ms"
  - id: "synchronized-retries-429"
    type: "single"
    prompt: "An API starts rejecting traffic: 1,000 clients receive `429 Too Many Requests` with `Retry-After: 30` within the same second. The SDK ignores the header and retries every request after a fixed 1 second. What happens next, and what should the SDK do?"
    explanation: |-
      Clients that failed at the same time and wait the same fixed delay retry at the same time. The server sees 1,000 requests at t + 1 s, rejects them, then again at t + 2 s, and so on: a *thundering herd* that keeps the overload in place. RFC 6585 §4 lets the server say how long to wait with `Retry-After` (seconds or a date, RFC 9110 §10.2.3); honoring it, and adding random jitter on top, spreads the returning load over time.

      "Spread out naturally" overestimates network variation: delays differ by milliseconds while the retry interval is a second, so the waves stay sharp.

      "Queues the rejected requests" is false; a 429 response is a rejection, and nothing is held for the client.

      "Malformed request" confuses 429 with 400; 429 says "too many, try later", which is a retryable condition.
    options:
      - text: "The retries spread out naturally because each client's network delay differs; the SDK only needs a longer fixed delay"
      - text: "All clients retry together and are rejected again in waves; wait the `Retry-After` time plus random jitter"
        correct: true
      - text: "The server queues the rejected requests and answers them in order; the SDK should keep retrying to hold its place"
      - text: "A 429 means the request was malformed, so retrying is pointless; the SDK should drop the request and report an error"
  - id: "safe-retries-without-key"
    type: "multiple"
    prompt: "A client has no idempotency keys. Which of these automatic retries are safe? Select all that apply."
    explanation: |-
      A retry is safe when repeating the request has the same effect as sending it once (idempotency, RFC 9110 §9.2.2), whatever happened to the first attempt.

      - `GET` is safe and idempotent; repeating it cannot change state.
      - `PUT` of a full representation is idempotent: applying it twice leaves the same state. (A 503 also usually means the work was never started.)

      Distractors:

      - `POST /orders` after a 504: the gateway stopped waiting, but the upstream may have created the order; a retry can create a second one.
      - `POST /payments` after a client timeout: the outcome is unknown, so a blind retry risks a double charge.
      - `PATCH` that appends: not idempotent, and a 502 does not prove the upstream did nothing before the gateway saw an invalid response.
    options:
      - text: "Retrying `PUT /users/7` with the same full body after a `503` that carried `Retry-After`"
        correct: true
      - text: "Retrying `POST /orders` after the gateway answered `504 Gateway Timeout` to the first attempt"
      - text: "Retrying `POST /payments` after the client's own read timeout expired on a slow network"
      - text: "Retrying `PATCH /carts/3` that appends an item after a `502 Bad Gateway`"
      - text: "Retrying `GET /orders/7` after the connection was reset before any response arrived"
        correct: true
  - id: "timeout-ordering-bug"
    type: "single"
    prompt: "A client calls a gateway with a 2-second timeout and retries once on timeout. The gateway calls a service with a 5-second timeout. Under load the service takes 3 seconds per request, and its load doubles as soon as latency rises. Why?"
    explanation: |-
      When an upstream timeout is shorter than a downstream one, the upstream abandons calls that the downstream keeps working on. Here the client stops waiting at 2 s and retries, but the gateway (5 s budget) and the service (3 s per request) carry on with the first request. Every slow request is now processed twice, which slows the service further: a feedback loop. Remedies: make each layer's timeout *shorter* than its caller's (or propagate a deadline so downstream work is cancelled when the caller gives up), and retry only idempotent work, with backoff.

      "The gateway retries internally" invents behavior not in the scenario; the doubling comes from the client's retry alone.

      "TCP retransmits the request" confuses layers: TCP retransmissions are deduplicated by sequence numbers and never reach the application as a new request.

      "The service's timeout is too short" reverses the problem; at 3 s per request the service finishes within 5 s.
    options:
      - text: "The gateway retries internally on every client timeout, so the service sees three copies of each request"
      - text: "TCP retransmits the client's request after 2 seconds, and the service treats the duplicate as a new request"
      - text: "The service's 5-second timeout is too short, so it cancels work and the client sees errors that it retries"
      - text: "The client gives up before the gateway does, so each retry starts a second copy of work that is still running"
        correct: true
  - id: "pooling-latency-and-ports"
    type: "single"
    prompt: "A service makes 600 requests per second to one upstream `10.0.0.9:443` with an RTT of 20 ms. It currently opens a new TCP + TLS 1.3 connection per request and closes it first (TIME_WAIT 60 s). After switching to a pool of keep-alive connections, what happens to setup latency per request and to the number of client sockets in TIME_WAIT?"
    explanation: |-
      - Per-request connections cost a TCP round trip, a TLS 1.3 round trip, and the request round trip: 3 × 20 = 60 ms. A pooled connection needs only the request: 20 ms.
      - Each closed connection leaves a client socket in TIME_WAIT for 60 s: 600 × 60 = 36,000. That exceeds a default Linux ephemeral range of 28,232 ports for this destination, so the per-request design is not just slower but heading for connect failures. A pool closes connections rarely, so TIME_WAIT sockets nearly vanish.

      "40 ms to 20 ms; about 600" forgets the TLS round trip and treats TIME_WAIT as lasting one second.

      "Stays at 20 ms" ignores handshake costs.

      "TIME_WAIT stays at 36,000" assumes pooled connections are still closed after each request, which is what pooling avoids.
    options:
      - text: "Request time drops from 60 ms to 20 ms; TIME_WAIT sockets drop from about 36,000 to nearly zero"
        correct: true
      - text: "Request time drops from 40 ms to 20 ms; TIME_WAIT sockets drop from about 600 to nearly zero"
      - text: "Request time stays at 20 ms; TIME_WAIT sockets drop from about 36,000 to nearly zero"
      - text: "Request time drops from 60 ms to 20 ms; TIME_WAIT sockets stay at about 36,000 with pooling"
---
## Learning objectives

- Quantify retry amplification across layers and the cost of backoff policies.
- Recognize synchronized retries and how `Retry-After` and jitter break them up.
- Decide which retries are safe from method semantics and status codes.
- Order timeouts and reuse connections so that client behavior does not amplify failures.

## The invariant

Retries are the point where every earlier chapter meets: TCP connection costs, HTTP method semantics, status-code contracts and capacity arithmetic.

1. **Retries multiply.** If n layers each make up to a attempts, the bottom layer sees up to aⁿ attempts per user action. A failing dependency then receives a multiple of normal load at the worst moment. Retry at one layer, cap retries with a budget (for example at most 10% extra requests), and fail fast elsewhere.
2. **Backoff must be capped and jittered.** Wait base × 2^k, capped, and randomize each wait so clients that failed together do not return together. Honor `Retry-After` on 429 and 503 (RFC 6585 §4, RFC 9110 §10.2.3); it is the server telling you its schedule.
3. **Only retry what is safe to repeat.** Idempotent methods (GET, HEAD, PUT, DELETE) can be retried after any failure. Non-idempotent requests after an ambiguous failure (timeouts, 502, 504, connection resets after the request was sent) need an idempotency key or a state check first. A 503 or 429 usually means the work was not started.
4. **Timeouts must nest.** Each callee's timeout should be shorter than its caller's, ideally by propagating a deadline. Otherwise callers abandon work that keeps running and retry it, doubling the load.
5. **Connections are not free.** A new TCP + TLS 1.3 connection costs two extra round trips and leaves a TIME_WAIT socket on the side that closes first; through NATs it also consumes a port mapping. Pools amortize both.

## Worked example

An order flow under partial outage. Three layers retry up to 3 times each (27 database attempts per user action), with a fixed 1-second retry delay and per-request connections. The same flow after fixing it:

| Change | Before | After |
|---|---|---|
| Retrying layers | app, gateway, service | app only, with an idempotency key on `POST /orders` |
| Attempts reaching the database per action | up to 3 × 3 × 3 = 27 | up to 3 |
| Retry delays | fixed 1 s | 100 ms × 2^k, capped at 1 s, full jitter; `Retry-After` honored |
| Total worst-case wait over 5 retries | 5 s, synchronized | at most 2,500 ms, spread out |
| Timeouts | client 2 s < gateway 5 s | deadline propagated: client 6 s, gateway 5 s, service 4 s |
| Connections | new TCP + TLS per call | pooled keep-alive |

A minimal jittered backoff in C#:

```csharp
static TimeSpan Backoff(int attempt, TimeSpan? retryAfter)
{
    var cap = TimeSpan.FromSeconds(1);
    var exp = TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt));
    var ceiling = exp < cap ? exp : cap;
    var jittered = TimeSpan.FromMilliseconds(Random.Shared.NextDouble() * ceiling.TotalMilliseconds);
    return retryAfter is { } ra && ra > jittered ? ra + jittered : jittered;
}
```

The server's `Retry-After` wins when it is longer, and jitter is added in both cases so clients spread out.

## Where this shows up today

As of November 2026, the IETF's `Idempotency-Key` header is still a working-group draft (draft-ietf-httpapi-idempotency-key-header-09), so retry-safety for POST depends on each API documenting its own key semantics. Retry budgets and deadline propagation are configured per client library or proxy, so the layer that retries has to be chosen deliberately rather than left to whatever each component does by default.

## Pitfalls checklist

- Retries configured independently at every layer of a call chain.
- Fixed retry intervals without jitter, or ignoring `Retry-After`.
- Retrying POST after timeouts, 502s or 504s without an idempotency key.
- Caller timeouts shorter than callee timeouts, with no cancellation.
- New connections per request, multiplying handshakes, TIME_WAIT sockets and NAT mappings.

## Key takeaways

- n layers × a attempts each = aⁿ attempts at the bottom; retry in one place.
- Back off exponentially, cap it, add jitter, and honor `Retry-After`.
- Method semantics and status codes decide whether a retry is safe.
- Nest timeouts and pool connections so failures do not feed themselves.

## Sources

- RFC 9110 (§9.2.2 idempotent methods, §10.2.3 Retry-After, §15.6 server errors); RFC 6585 §4 (429).
- RFC 9293 (TIME-WAIT); RFC 9846 (TLS 1.3 handshake cost).
- draft-ietf-httpapi-idempotency-key-header-09 (work in progress).
