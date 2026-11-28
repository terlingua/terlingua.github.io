---
title: "Method Safety, Idempotency and Retry Safety"
body_checks:
  - "assert round(1 + 0.1 + 0.01, 2) == 1.11 and round(0.1 ** 3, 6) == 0.001"
questions:
  - id: "idempotent-methods"
    type: "multiple"
    prompt: "Which of these HTTP methods are idempotent as defined by RFC 9110? Select all that apply."
    correct:
      - "`PUT`"
      - "`DELETE`"
      - "`HEAD`"
    distractors:
      - "`POST`"
      - "`PATCH`"
    explanation: |-
      RFC 9110 §9.2.2: a method is idempotent if the intended effect on the server of several identical requests is the same as the effect of one. The idempotent methods are PUT, DELETE and all safe methods (GET, HEAD, OPTIONS, TRACE).

      - `PUT` replaces the target's state with the enclosed representation; doing it twice leaves the same state.
      - `DELETE` removes the target; after the first success there is nothing more to remove.
      - `HEAD` is safe (§9.2.1), so it is idempotent too.

      `POST` is not: its semantics are defined by the resource, and typically each request creates something new or triggers an action again.

      `PATCH` (RFC 5789) is not idempotent in general: a patch such as "append item" or "increment by 1" changes the result each time. Specific patch documents can be idempotent, but the method does not promise it.
  - id: "delete-different-status"
    type: "single"
    prompt: "A client sends `DELETE /orders/7` and gets `204 No Content`. Its retry of the same request, sent because the first response was slow, gets `404 Not Found`. Does this contradict DELETE being idempotent?"
    correct:
      - "No; idempotency concerns the effect on server state, which is the same after both requests"
    distractors:
      - "Yes; the second response must be `204` as well, or the server is violating the idempotency of the method"
      - "Yes; only GET and HEAD are idempotent, so a retried DELETE is never guaranteed to behave the same way"
      - "No, but only because both responses had empty bodies, which is the condition that idempotency requires"
    explanation: |-
      RFC 9110 §9.2.2 defines idempotency by the *intended effect on the server*, not by the response. After the first DELETE the order is gone; after the second it is still gone. The state is the same, so the method behaved idempotently, and a client may safely retry it. The status code reports what this particular request found, and a 404 on the retry is normal.

      "Must be 204 as well" confuses identical responses with identical effects.

      "Only GET and HEAD are idempotent" omits PUT, DELETE, OPTIONS and TRACE.

      "Because both bodies were empty" invents a condition; bodies are irrelevant to the definition.
  - id: "post-timeout-retry-bug"
    type: "single"
    prompt: "A client sends `POST /payments`, waits 10 seconds with no response, and its HTTP library automatically retries the request. The customer is charged twice. What is the underlying mistake?"
    correct:
      - "POST is not idempotent and a timed-out request's outcome is unknown, so a retry can charge twice"
    distractors:
      - "TCP should have suppressed the duplicate request, so the server's network stack must be misconfigured"
      - "The server should have answered `202 Accepted` at once, which would make blind retries of the POST safe"
      - "The client should send payments with GET, because GET requests are safe to retry after any timeout"
    explanation: |-
      A timeout means the client does not know whether the server received and executed the request: the request could have been lost, or the response could have been lost after the charge was made. Retrying is only safe if repeating the request has the same effect as sending it once. POST has no such guarantee, and RFC 9110 §9.2.2 says clients should not automatically retry non-idempotent requests unless they have some means of knowing it is safe. The fix is to make the operation idempotent at the application level: an idempotency key (the client sends a unique key; the server stores the outcome per key and returns it for repeats), or a client-generated resource ID with PUT.

      "TCP should have suppressed it" fails: the retry is a new HTTP request, often on a new connection; TCP only removes duplicate segments within one connection.

      "Answer 202" changes nothing about duplicate submission; a second POST would be accepted again.

      "Use GET" would violate GET's safety, making crawlers and prefetchers able to trigger payments.
  - id: "retry-expected-attempts"
    type: "single"
    prompt: "A client retries an idempotent `GET` until it succeeds, up to 3 attempts in total. Each attempt fails independently with probability 0.1. How many attempts does an average request use, and what fraction of requests fail after all retries?"
    correct:
      - "1.11 attempts on average; 0.1% fail"
    distractors:
      - "1.3 attempts on average; 0.1% fail"
      - "1.11 attempts on average; 1% fail"
      - "3 attempts on average; 0.1% fail"
    explanation: |-
      - Attempt 1 always happens; attempt 2 happens if attempt 1 failed (probability 0.1); attempt 3 if the first two failed (0.1 × 0.1 = 0.01). Expected attempts = 1 + 0.1 + 0.01 = 1.11.
      - The request fails overall only if all three attempts fail: 0.1^3 = 0.001 = 0.1%.

      "1.3 attempts" counts 0.1 extra attempts for each of the three tries, as if every request made all three.

      "1% fail" uses two failures (0.1^2) instead of three.

      "3 attempts on average" assumes every request uses the maximum.

      The same arithmetic shows why retries are dangerous during an outage: at a failure probability of 1, every request uses all 3 attempts, tripling load exactly when the server is struggling.
    checks:
      - "p = 0.1; assert round(1 + p + p * p, 2) == 1.11 and round(p ** 3, 4) == 0.001 and round(p ** 2, 3) == 0.01"
  - id: "idempotency-key-replay"
    type: "single"
    prompt: "A payment API implements the `Idempotency-Key` request header. A client sends `POST /payments` with `Idempotency-Key: 7f3c…`; the server charges the card, stores the `201 Created` response under that key, but the response is lost. The client retries with the same key and the same body. What should the server do?"
    correct:
      - "Return the stored `201 Created` response without charging the card a second time"
    distractors:
      - "Charge the card again, because POST is not idempotent and each request has to be executed"
      - "Return `409 Conflict`, because a key may never be used in more than one request"
      - "Return `304 Not Modified`, because the payment resource has not changed since the first request"
    explanation: |-
      The idempotency-key pattern makes a non-idempotent operation safe to retry. The server records each key with the outcome of the first request carrying it. A repeat with the same key and the same payload is answered from that record, so the effect on server state is the same as a single request. The IETF draft for the header also covers the edge cases: a repeat while the first request is still being processed gets `409 Conflict`, and a repeat with a different payload is rejected (the draft suggests `422`).

      "Charge the card again" ignores the key, which is the whole point of sending it.

      "409 for any reuse" would make retries impossible; 409 is only for concurrent in-flight duplicates.

      "304 Not Modified" belongs to conditional GETs with validators, not to POST replay.
  - id: "get-delete-link-bug"
    type: "single"
    prompt: "A webmail app deletes a message when the user follows a link to `GET /messages/42/delete`. After a browser extension starts prefetching links, users find messages disappearing unopened. What principle does the app violate?"
    correct:
      - "GET is safe, so agents send it freely; state changes need an unsafe method such as POST or DELETE"
    distractors:
      - "Prefetchers must send credentials only on POST, so the app must reject GET requests that carry session cookies"
      - "The app must check the `Referer` header on every GET, since that is how HTTP marks requests that users intended"
      - "HTTP allows only one GET per resource and session, so the extension broke the protocol by fetching links in advance"
    explanation: |-
      RFC 9110 §9.2.1 defines GET as **safe**: the client does not request, and does not expect, any state change. Because of that, browsers, prefetchers, crawlers, link previewers and caches send GET requests freely, without user intent. Binding a destructive action to a GET URL lets any of them trigger it. The fix is to use POST or DELETE for the action (and, for browser forms, CSRF protection).

      "Credentials only on POST" is not a rule; browsers attach cookies to GET requests too.

      "Check Referer" is not how HTTP expresses intent, and Referer is often absent or stripped.

      "Only one GET per resource" is invented; GET may be repeated any number of times.
---

## Learning objectives

- Classify methods as safe, idempotent and cacheable per RFC 9110.
- Decide when a client may retry a request automatically.
- Compute the load and failure effects of a retry policy.
- Use idempotency keys to make POST retryable.

## The invariant

RFC 9110 gives each method properties that intermediaries and clients rely on:

| Method | Safe (§9.2.1) | Idempotent (§9.2.2) | Cacheable (§9.2.3) |
|---|---|---|---|
| GET, HEAD | yes | yes | yes |
| OPTIONS, TRACE | yes | yes | no |
| PUT, DELETE | no | yes | no |
| POST | no | no | only with explicit freshness information |
| PATCH (RFC 5789) | no | not in general | no |

**Safe** means the client requests no state change, so any agent may send it at any time. **Idempotent** means several identical requests have the same intended effect on the server as one; responses may differ. Both are promises about semantics: a server that deletes data on GET is still wrong, even though it can.

The properties matter most when something fails. After a timeout or a dropped connection, a client cannot tell whether the server executed the request. For an idempotent request, repeating it is harmless, so RFC 9110 lets clients retry automatically after a connection failure. For a non-idempotent request, a blind retry may execute the action twice; the client must not retry automatically unless it has a way to know the repeat is safe.

That way is usually an **idempotency key**: the client generates a unique key per logical operation and sends it with every attempt; the server stores the result of the first attempt per key and replays it for repeats. An alternative is to let the client choose the resource identifier and use PUT, which is idempotent by definition.

Retries also multiply load. With independent failure probability p and up to n attempts, the expected number of attempts per request is 1 + p + p² + … + p^(n−1), and the residual failure rate is pⁿ. When p approaches 1 during an outage, load approaches n times normal.

## Worked example

A client calls a payment API with a deduplication key and retries idempotently:

```csharp
using System.Net.Http.Json;

var http = new HttpClient { BaseAddress = new Uri("https://pay.example/") };
var key = Guid.NewGuid().ToString();            // one key per logical payment

for (var attempt = 1; attempt <= 3; attempt++)
{
    using var request = new HttpRequestMessage(HttpMethod.Post, "payments")
    {
        Content = JsonContent.Create(new { amount = 1250, currency = "USD" })
    };
    request.Headers.Add("Idempotency-Key", key);   // same key on every attempt
    try
    {
        using var response = await http.SendAsync(request);
        if ((int)response.StatusCode < 500) { Console.WriteLine(response.StatusCode); break; }
    }
    catch (HttpRequestException) { }
    await Task.Delay(TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)));
}
```

The same key on every attempt is what makes the retry safe; generating a new key per attempt would defeat it. Retry arithmetic for p = 0.1 and 3 attempts: 1 + 0.1 + 0.01 = 1.11 attempts on average, and 0.1³ = 0.1% of requests still fail.

## Where this shows up today

As of November 2026, the `Idempotency-Key` header is specified by draft-ietf-httpapi-idempotency-key-header-09 (21 November 2026), an HTTPAPI working-group draft that is not yet an RFC. Payment and cloud APIs have used the pattern under this and similar header names for years, which is why the draft documents existing practice rather than inventing it.

## Pitfalls checklist

- Letting an HTTP client library automatically retry POST after timeouts.
- Generating a new idempotency key on each retry instead of once per operation.
- Performing state changes on GET, where crawlers and prefetchers will trigger them.
- Treating a different status code on a retried DELETE as an error.
- Retrying without limits or backoff and multiplying load during outages.

## Key takeaways

- Safe implies idempotent; PUT and DELETE are idempotent but not safe; POST is neither.
- Idempotency concerns the effect on server state, not the response.
- After a timeout, only idempotent requests may be retried blindly.
- Idempotency keys turn POST into a retryable operation.

## Sources

- RFC 9110, HTTP Semantics, §9.2.1 (safe), §9.2.2 (idempotent), §9.2.3 (methods and caching).
- RFC 5789, PATCH Method for HTTP.
- draft-ietf-httpapi-idempotency-key-header-09, The Idempotency-Key HTTP Header Field (work in progress).
