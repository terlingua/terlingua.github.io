---
title: "Cache-Control and Freshness Arithmetic"
body_checks:
  - "assert max(0, 0) + 100 + 400 == 500 and 600 - 500 == 100"
  - "assert round(0.1 * 10 * 86400) == 86400"
questions:
  - id: "remaining-freshness"
    type: "single"
    prompt: "A CDN receives a response with `Cache-Control: max-age=600` and `Age: 100` (it came through another cache), with negligible network delay. The CDN has now held it for 400 seconds. How much longer is it fresh?"
    correct:
      - "100 seconds"
    distractors:
      - "200 seconds"
      - "500 seconds"
      - "None; it is already stale"
    explanation: |-
      RFC 9111 compares the *freshness lifetime* (here max-age = 600 s) with the response's *current age* (§4.2.3). The current age includes the age the response already had when it arrived, carried in the `Age` header, plus the time it has been resident in this cache:

      current_age = 100 (Age on arrival) + 400 (resident time) = 500 s.

      Remaining freshness = 600 − 500 = 100 s.

      "200 seconds" ignores the `Age` header and counts only the CDN's own 400 s; caches that do this serve content longer than the origin allowed.

      "500 seconds" reports the current age instead of the time remaining.

      "Already stale" would require the current age to exceed 600 s.
    checks:
      - "age = 100 + 400; assert 600 - age == 100 and 600 - 400 == 200"
  - id: "s-maxage-split"
    type: "single"
    prompt: "A response carries `Cache-Control: public, max-age=60, s-maxage=3600`. For how long may a browser's private cache and a CDN each treat it as fresh?"
    correct:
      - "Browser 60 seconds; CDN 3,600 seconds"
    distractors:
      - "Browser 3,600 seconds; CDN 60 seconds"
      - "Both 60 seconds, the stricter of the two values"
      - "Both 3,600 seconds, the larger of the two values"
    explanation: |-
      `s-maxage` applies only to shared caches (RFC 9111 §5.2.2.10) and overrides `max-age` there. Private caches, such as the browser's, ignore it and use `max-age`. So the CDN keeps the response for an hour while each browser rechecks after a minute, a common pattern that lets the origin purge or update the CDN while browsers stay close to current.

      "Browser 3,600; CDN 60" swaps the audiences.

      "Both 60, the stricter" invents a "strictest wins" rule.

      "Both 3,600" applies `s-maxage` to private caches, which it does not address.
  - id: "no-cache-vs-no-store"
    type: "multiple"
    prompt: "Which statements about response directives are true under RFC 9111? Select all that apply."
    correct:
      - "`no-cache` lets a cache store the response but requires validation with the origin before each reuse"
      - "`no-store` forbids caches from storing any part of the request or the response"
    distractors:
      - "`no-cache` forbids every cache from storing the response at all"
      - "`no-store` allows storage provided the cache revalidates before every reuse"
      - "`private` forbids the user's own browser cache from storing the response"
    explanation: |-
      The two names are misleading, so the definitions matter:

      - `no-cache` (§5.2.2.4): the response may be stored, but must not be used to satisfy a later request without successful validation on the origin server. Combined with an ETag, revalidation is cheap (a 304 has no body).
      - `no-store` (§5.2.2.5): a cache must not store the request or response at all; use it for sensitive data.

      "`no-cache` forbids storing" is the common misreading; that is `no-store`.

      "`no-store` allows storage with revalidation" describes `no-cache`.

      "`private` forbids the browser cache" inverts it: `private` (§5.2.2.7) forbids *shared* caches from storing the response, while the user's private cache may.
  - id: "must-revalidate-origin-down"
    type: "single"
    prompt: "A shared cache holds a response with `Cache-Control: max-age=60, must-revalidate`. It is now 90 seconds old, and the origin server is unreachable. What does the cache do with a new request?"
    correct:
      - "It must not serve the stale copy; it returns an error, normally `504 Gateway Timeout`"
    distractors:
      - "It serves the stale copy, since serving something is better than failing while the origin is down"
      - "It serves the stale copy with a warning, because `must-revalidate` applies only while the response is fresh"
      - "It holds the request open until the origin comes back, then validates the copy and responds"
    explanation: |-
      Normally RFC 9111 §4.2.4 lets a cache that cannot reach the origin serve a stale response, unless an explicit directive prohibits it. `must-revalidate` (§5.2.2.2) is such a directive: once the response is stale, the cache must not reuse it without successful validation, and if it is disconnected it must generate an error, using 504 unless another error status is more appropriate. Origins use it for data where a stale answer is worse than none, such as balances.

      "Serves the stale copy" is the default without `must-revalidate`, not with it.

      "Applies only while fresh" inverts the directive; it constrains the stale period.

      "Holds the request open" is not specified behavior; the cache answers with an error.
  - id: "personalized-page-leak"
    type: "single"
    prompt: "An account page that shows the signed-in user's name and address is served with `Cache-Control: max-age=300` and sits behind a CDN. Users report seeing other people's details. What fixes it?"
    correct:
      - "Mark the response `private` (or `no-store`) so that shared caches such as the CDN do not store it"
    distractors:
      - "Add an `ETag` so that the CDN can tell apart the copies of the page that belong to different users"
      - "Shorten the lifetime to `max-age=30` so that any page served to the wrong user expires sooner"
      - "Add `must-revalidate` so that the CDN checks with the origin before giving the page to anyone"
    explanation: |-
      `max-age` without `private` permits *any* cache, shared ones included, to store and reuse the response for other requests to the same URL. The CDN cached the first user's page and served it to everyone else who requested `/account` within 300 seconds. `private` (RFC 9111 §5.2.2.7) restricts storage to the user's own cache; `no-store` forbids storage entirely.

      "Add an ETag" helps validation, not separation; the CDN still serves its stored copy without asking while it is fresh.

      "max-age=30" shrinks the leak window but keeps the leak.

      "must-revalidate" applies only once the response is stale; while fresh, the CDN serves it without contacting the origin.
  - id: "stale-while-revalidate-window"
    type: "single"
    prompt: "A cache that implements RFC 5861 holds a response with `Cache-Control: max-age=60, stale-while-revalidate=30`. Requests arrive when the stored response is 50, 75 and 95 seconds old (no revalidation has completed in between). How is each one handled?"
    correct:
      - "Served fresh; served stale while a background revalidation starts; held until revalidation completes"
    distractors:
      - "Served fresh; held until revalidation completes; served stale while a background revalidation starts"
      - "Served fresh; served fresh; served stale while a background revalidation starts"
      - "Served stale while a background revalidation starts in all three cases"
      - "Served fresh; served stale while a background revalidation starts; served stale while a background revalidation starts"
    explanation: |-
      `stale-while-revalidate=N` (RFC 5861, an Informational extension, not part of RFC 9111) lets a cache serve a stale response for up to N seconds *after it becomes stale* while it revalidates in the background, hiding origin latency from the user.

      | Age | Status | Handling |
      |---|---|---|
      | 50 s | fresh (≤ 60) | served from cache |
      | 75 s | stale by 15 s (within 30) | served stale; background revalidation |
      | 95 s | stale by 35 s (beyond 30) | must validate first; the request waits |

      "Fresh; held until revalidation; served stale" swaps the last two cases: the grace period covers ages 60 to 90 s, so 75 s is inside it and 95 s is outside it.

      "Fresh; fresh; served stale" treats 75 s as fresh, but freshness ended at 60 s.

      "Served stale in all three cases" ignores that at 50 s the response is still fresh and needs no revalidation at all.

      "Served stale at 95 s as well" treats the window as open-ended; it ends 30 seconds after the response became stale, at an age of 90 s.
    checks:
      - "fl, swr = 60, 30; kinds = ['fresh' if a <= fl else 'swr' if a <= fl + swr else 'block' for a in (50, 75, 95)]; assert kinds == ['fresh', 'swr', 'block']"
---

## Learning objectives

- Compute freshness lifetime and current age, including the `Age` header.
- Target browsers and shared caches separately with `max-age`, `s-maxage` and `private`.
- Choose correctly between `no-cache`, `no-store` and `must-revalidate`.
- Reason about `stale-while-revalidate` windows.

## The invariant

An HTTP cache (RFC 9111) may reuse a stored response without contacting the origin while it is **fresh**: while its *freshness lifetime* exceeds its *current age*.

**Freshness lifetime** (§4.2.1), first match wins:

1. `s-maxage` (shared caches only, §5.2.2.10)
2. `max-age` (§5.2.2.1)
3. `Expires` minus `Date`
4. a heuristic (§4.2.2), for responses that allow it, typically a fraction such as 10% of the time since `Last-Modified`

**Current age** (§4.2.3) is the age the response had on arrival (the larger of the `Age` header plus the request's transit time and the apparent age from `Date`) plus how long this cache has held it. Ignoring `Age` makes a downstream cache serve content longer than the origin allowed.

When a response is **stale**, the cache sends a conditional request (validation, covered next) or, if the origin is unreachable, may serve it stale unless a directive forbids that (§4.2.4).

| Directive | Effect |
|---|---|
| `no-store` (§5.2.2.5) | do not store at all |
| `no-cache` (§5.2.2.4) | store, but validate before every reuse |
| `private` (§5.2.2.7) | only the user's own cache may store it |
| `public` | shared caches may store it even if they normally would not (for example, with authentication) |
| `must-revalidate` (§5.2.2.2) | never serve stale without validation; error (usually 504) if disconnected |
| `stale-while-revalidate=N` (RFC 5861) | serve stale for N seconds past freshness while revalidating in the background |

RFC 9213 adds **targeted** fields such as `CDN-Cache-Control`: a CDN that recognizes one uses it and ignores `Cache-Control` for that response, so origins can give CDNs and browsers different policies explicitly.

## Worked example

A product image is served as:

```bash
HTTP/1.1 200 OK
Date: Fri, 27 Nov 2026 10:00:00 GMT
Cache-Control: public, max-age=600, s-maxage=86400, stale-while-revalidate=60
ETag: "img-v42"
Content-Type: image/webp
```

| Cache | Freshness lifetime | Serves stale while revalidating until age |
|---|---|---|
| Browser | 600 s (`max-age`) | 660 s |
| CDN | 86,400 s (`s-maxage`) | 86,460 s |

A CDN that received this through a regional cache with `Age: 100` and has held it for 400 s has current age 500 s against its own 86,400 s lifetime; a browser that received the same copy with `Age: 100` and held it 400 s has 100 s of freshness left (600 − 500). A response with only `Last-Modified` ten days ago and no explicit lifetime could be given a heuristic lifetime of 10% × 10 days = 1 day, which is why APIs should always state their caching policy.

## Where this shows up today

As of November 2026, RFC 9213 (2022, Proposed Standard) defines `CDN-Cache-Control`, and its precedence rule lets one response say "an hour at the CDN, a minute in browsers" without overloading `s-maxage`. Which targeted fields a particular CDN honors, and whether it implements `stale-while-revalidate`, is vendor-specific and has to be checked in that vendor's documentation.

## Pitfalls checklist

- Serving per-user responses with `max-age` and no `private`.
- Using `no-cache` when the intent was `no-store` (or the reverse).
- Ignoring the `Age` header in chained caches.
- Leaving out explicit freshness and getting heuristic caching of API responses.
- Expecting `must-revalidate` to have any effect while a response is still fresh.

## Key takeaways

- Fresh while lifetime > current age, and current age includes the incoming `Age`.
- `s-maxage` and targeted fields address shared caches; `private` keeps responses out of them.
- `no-cache` means "revalidate"; `no-store` means "do not keep".
- `must-revalidate` forbids stale reuse; `stale-while-revalidate` permits a bounded amount.

## Sources

- RFC 9111, HTTP Caching (§4.2.1–4.2.4, §5.1, §5.2.2.1, §5.2.2.2, §5.2.2.4, §5.2.2.5, §5.2.2.7, §5.2.2.10).
- RFC 5861, HTTP Cache-Control Extensions for Stale Content (Informational).
- RFC 9213, Targeted HTTP Cache Control.
