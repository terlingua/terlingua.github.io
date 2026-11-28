---
title: "Status-Code Contracts: Redirects, Auth, Conflicts, Throttling and Gateways"
questions:
  - id: "post-redirect-303"
    type: "single"
    prompt: "A browser submits a form with `POST /orders`. The server creates order 9 and responds `303 See Other` with `Location: /orders/9`. What request does the browser send next?"
    explanation: |-
      RFC 9110 §15.4.4: a 303 response directs the client to retrieve a *different* resource with GET (or HEAD). That is the standard "POST/redirect/GET" pattern: after the POST, the browser loads a page it can bookmark and refresh without resubmitting the form.

      "`POST /orders/9` with the original body" is what 307 or 308 would require, and it would submit the order a second time.

      "`PUT`" is never chosen by a redirect; methods are either preserved or changed to GET.

      "Shows its cached copy" ignores that the redirect asks for a new request; a cache may satisfy that GET, but a GET is what is issued.

      "`GET /orders`, the original URL" ignores the `Location` header, which names the resource to fetch next.
    options:
      - text: "`GET /orders/9`"
        correct: true
      - text: "`POST /orders/9` with the original form body"
      - text: "`PUT /orders/9` with the original form body"
      - text: "None; it shows its cached copy of `/orders/9`"
      - text: "`GET /orders`, the URL of the original form"
  - id: "method-preserving-redirects"
    type: "multiple"
    prompt: "An API moves `POST /v1/upload` to `/v2/upload`. Which redirect responses guarantee that a compliant client repeats the POST with the same body at the new URI? Select all that apply."
    explanation: |-
      RFC 9110 §15.4.8 (307) and §15.4.9 (308) forbid the user agent to change the request method when following the redirect, so the POST and its body are repeated at the new URI. 308 also says the move is permanent, so clients may update stored links, and it is cacheable by default.

      "`301`" and "`302`": for historical reasons RFC 9110 allows user agents to change POST to GET when following them (browsers do), so neither guarantees the method. 307 and 308 were created to fill that gap.

      "`303`" changes the method to GET by definition.
    options:
      - text: "`301 Moved Permanently`"
      - text: "`307 Temporary Redirect`"
        correct: true
      - text: "`308 Permanent Redirect`"
        correct: true
      - text: "`302 Found`"
      - text: "`303 See Other`"
  - id: "401-versus-403-bug"
    type: "single"
    prompt: "An API returns `401 Unauthorized` with no `WWW-Authenticate` header when a correctly signed-in user requests a report they lack permission for. Client SDKs respond by refreshing the token and retrying in a loop. What should the API return?"
    explanation: |-
      RFC 9110 §15.5.2: 401 means the request lacks valid authentication credentials, and the server MUST send a `WWW-Authenticate` challenge saying how to authenticate. Clients reasonably react by (re)authenticating. §15.5.4: 403 means the server understood the request and refuses it; if credentials were provided, they are not sufficient, and repeating the request with the same credentials will not help. A signed-in user without permission is the textbook 403.

      "401 is correct" ignores both the meaning and the missing mandatory header.

      "400" describes malformed syntax, not authorization.

      "404 in every case" is a legitimate *option* (RFC 9110 permits hiding a forbidden resource's existence with 404), but it is a policy choice, not what correct signalling requires, and it would not be the fix for this bug.
    options:
      - text: "`401 Unauthorized` is correct; the SDKs are wrong to treat it as a request to obtain fresh credentials"
      - text: "`400 Bad Request`, because a request for a resource the user may not see is malformed by definition"
      - text: "`404 Not Found` in every case, because any other status reveals that the report exists at all"
      - text: "`403 Forbidden`: the credentials are valid but insufficient, so re-authenticating cannot help"
        correct: true
  - id: "409-versus-412"
    type: "single"
    prompt: "A client sends `PUT /documents/5` with `If-Match: \"v7\"`, but the document's current ETag is `\"v8\"`. Which status code does RFC 9110 call for?"
    explanation: |-
      The client made the request conditional: "apply this only if the current version is v7". The condition evaluated false, and RFC 9110 §13.1.1 and §15.5.13 say the server must not perform the method and responds 412. This is how a client avoids overwriting someone else's update.

      "`409 Conflict`" (§15.5.10) is for requests that conflict with the current state of the resource when no precondition was involved, for example a business rule that rejects an edit. The precondition mechanism has its own code.

      "`428 Precondition Required`" (RFC 6585) is for the opposite case: the server *requires* a conditional request and the client sent none.

      "`304 Not Modified`" answers a successful conditional GET or HEAD (`If-None-Match` matched), never a failed PUT.
    options:
      - text: "`412 Precondition Failed`"
        correct: true
      - text: "`409 Conflict`"
      - text: "`428 Precondition Required`"
      - text: "`304 Not Modified`"
  - id: "retry-after-seconds"
    type: "single"
    prompt: "At 14:05:30 a client receives `429 Too Many Requests` with `Retry-After: 90`. When is the earliest time it should retry?"
    explanation: |-
      RFC 6585 §4 defines 429 and allows a `Retry-After` header. RFC 9110 §10.2.3 defines `Retry-After` as either an HTTP-date or a number of seconds (`delay-seconds`). 90 is seconds: 14:05:30 + 90 s = 14:07:00.

      "14:05:31" ignores the header and retries almost immediately, which is exactly what makes throttling escalate.

      "14:06:30" adds only 60 seconds.

      "15:35:30" reads 90 as minutes.
    options:
      - text: "14:05:31"
      - text: "14:07:00"
        correct: true
      - text: "14:06:30"
      - text: "15:35:30"
  - id: "gateway-504"
    type: "single"
    prompt: "An API gateway forwards a request to an upstream service with a 60-second upstream timeout. The upstream is still working when the 60 seconds pass. What status should the gateway return, and what does it tell the client?"
    explanation: |-
      RFC 9110 assigns distinct meanings to the gateway errors:

      - 504 (§15.6.5): a gateway or proxy did not receive a timely response from an upstream server. That is this case.
      - 502 (§15.6.3): the gateway received an *invalid* response (a reset connection, garbage, a protocol error).
      - 503 (§15.6.4): the server is temporarily unable to handle the request (overload, maintenance), possibly with `Retry-After`.

      "408 Request Timeout" is about the *client* being slow to send its request, not the upstream being slow to answer.

      The distinction matters for retries: after a 504 the upstream may still complete the work, so retrying a non-idempotent request risks duplication, while a 503 usually means the work was not started.
    options:
      - text: "`502 Bad Gateway`: the upstream sent the gateway a malformed or invalid response"
      - text: "`503 Service Unavailable`: the gateway itself is overloaded and is shedding load"
      - text: "`504 Gateway Timeout`: the gateway did not receive a timely response from the upstream"
        correct: true
      - text: "`408 Request Timeout`: the client took too long to send its complete request"
---
## Learning objectives

- Choose redirects by whether the method must be preserved and whether the move is permanent.
- Separate 401 from 403 and 409 from 412.
- Honor 429 and 503 with `Retry-After`.
- Attribute 502, 503 and 504 to the right component.

## The invariant

A status code is a contract between server and client: it tells generic software (browsers, SDKs, proxies, caches, retry logic) what happened without reading the body. RFC 9110 §15 defines the codes; the ones that cause real bugs come in pairs.

**Redirects** differ on two axes:

| Code | Permanent | Method on the new request |
|---|---|---|
| 301 Moved Permanently | yes | may change POST to GET (historical) |
| 302 Found | no | may change POST to GET (historical) |
| 303 See Other | no | always GET |
| 307 Temporary Redirect | no | preserved, body included |
| 308 Permanent Redirect | yes | preserved, body included |

**Authentication vs authorization.** 401 means "no valid credentials" and must carry a `WWW-Authenticate` challenge; clients respond by authenticating. 403 means "I know who you are, and the answer is no"; re-authenticating will not help.

**Conflicts.** 412 Precondition Failed answers a request whose explicit precondition (`If-Match`, `If-Unmodified-Since`) was false. 409 Conflict reports any other clash with the resource's current state. 428 Precondition Required (RFC 6585) says the server insists on a conditional request.

**Throttling and availability.** 429 Too Many Requests (RFC 6585 §4) and 503 Service Unavailable may carry `Retry-After`, either seconds or an HTTP-date (RFC 9110 §10.2.3). Clients should wait at least that long.

**Gateways.** 502: the upstream answered with something invalid. 504: the upstream did not answer in time. 503: the server (or gateway) is temporarily unable to serve. Only the last says work was probably not started.

## Worked example

A POST-redirect-GET exchange followed by throttling:

```bash
POST /orders HTTP/1.1
Host: shop.example
Content-Type: application/x-www-form-urlencoded

item=42&qty=1

HTTP/1.1 303 See Other
Location: /orders/9

GET /orders/9 HTTP/1.1
Host: shop.example

HTTP/1.1 429 Too Many Requests
Date: Fri, 27 Nov 2026 14:05:30 GMT
Retry-After: 90
```

The browser follows the 303 with a GET; the order is not resubmitted on refresh. The 429 arrives at 14:05:30, so the earliest retry is 14:07:00. Had the server used `Retry-After: Fri, 27 Nov 2026 14:07:00 GMT`, the meaning would be the same; the date form is robust against network delay, the seconds form against clock skew.

## Where this shows up today

As of November 2026, RFC 9110 (STD 97) defines the core status codes, while IANA's HTTP Status Code Registry adds codes from other RFCs, such as 429 and 428 (RFC 6585) and 425 Too Early (RFC 8470). Generic client libraries and API gateways key their retry and redirect behavior on these registered meanings, so a misused code changes behavior far from the server that chose it.

## Pitfalls checklist

- Redirecting API POSTs with 301 or 302 and silently turning them into GETs.
- Returning 401 for authorization failures, or omitting `WWW-Authenticate`.
- Using 409 where a failed `If-Match` should produce 412.
- Ignoring `Retry-After` and retrying immediately.
- Treating 504 as "nothing happened" and retrying non-idempotent requests.

## Key takeaways

- 303 turns the next request into GET; 307 and 308 preserve the method and body.
- 401 asks for credentials; 403 refuses despite them.
- 412 is for failed preconditions; 409 for other state conflicts.
- 502 invalid upstream response, 503 temporarily unavailable, 504 upstream too slow.

## Sources

- RFC 9110, HTTP Semantics, §10.2.3 (Retry-After), §13.1.1 (If-Match), §15.4.2–15.4.9 (redirects), §15.5.2, §15.5.4, §15.5.10, §15.5.13, §15.6.3–15.6.5.
- RFC 6585, Additional HTTP Status Codes (428, 429); RFC 8470 (425).
- IANA, Hypertext Transfer Protocol (HTTP) Status Code Registry.
