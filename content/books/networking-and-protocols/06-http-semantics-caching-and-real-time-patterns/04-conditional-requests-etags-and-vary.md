---
title: "Conditional Requests, ETags and Vary"
questions:
  - id: "if-none-match-changed"
    type: "single"
    prompt: "A browser's stale cached copy of `/app.js` has `ETag: \"a1\"`. It sends `GET /app.js` with `If-None-Match: \"a1\"`. The server's current version has `ETag: \"a2\"`. What does the server return?"
    explanation: |-
      `If-None-Match` (RFC 9110 §13.1.2) means "send the representation only if its current ETag matches none of these". For GET and HEAD:

      - Condition false (the current ETag *is* listed): respond `304 Not Modified` with no body; the cache refreshes its stored metadata and reuses its copy.
      - Condition true (the current ETag is not listed): process the request normally, so `200 OK` with the full new representation.

      Here the current ETag `"a2"` is not `"a1"`, so the condition is true and the new body is returned.

      "`304 Not Modified`" would be the answer if the version were still `"a1"`.

      "`412 Precondition Failed`" is what a *failed* `If-None-Match` produces for methods other than GET and HEAD (for example `PUT` with `If-None-Match: *` to prevent overwriting); for GET a failed condition yields 304.

      "`409 Conflict`" is unrelated to validators.

      "`200 OK` with the body of version `"a1"`" misreads the header: `If-None-Match` names versions the client already has; the server always serves its current version.
    options:
      - text: "`304 Not Modified` with no body"
      - text: "`412 Precondition Failed` with no body"
      - text: "`200 OK` with the new body and `ETag: \"a2\"`"
        correct: true
      - text: "`409 Conflict` describing the newer version"
      - text: "`200 OK` with the body of version `\"a1\"`, since that is the version the client named"
  - id: "weak-validators"
    type: "multiple"
    prompt: "A server sends `ETag: W/\"v3\"`. Which statements about this weak validator are true? Select all that apply."
    explanation: |-
      RFC 9110 §8.8.1 and §8.8.3: a *strong* ETag changes whenever the representation's bytes change; a *weak* ETag (prefixed `W/`) promises only semantic equivalence, so a server can keep it across, say, different compression or insignificant whitespace.

      - **Weak comparison** ignores the `W/` flag and compares the opaque tags; `If-None-Match` uses it, so `W/"v3"` matches `W/"v3"` and `"v3"`.
      - **Strong comparison** requires both tags to be strong and identical. `If-Match` and `If-Range` use it, because byte ranges or a lost-update check need byte-identical content. A weak ETag therefore never satisfies `If-Range`.

      "Changes rarely, cache longer" invents a freshness meaning; validators say nothing about lifetime.

      "Works with `If-Match`" is wrong: `If-Match` uses strong comparison, so weak ETags never match.

      "Expires after 24 hours" is invented; validators have no lifetime of their own.
    options:
      - text: "The `W/` prefix tells caches that the resource changes rarely and can be cached for longer"
      - text: "It works with `If-Match` for lost-update prevention, because `If-Match` uses weak comparison"
      - text: "It expires after 24 hours, after which the server must issue a new strong validator"
      - text: "Under the weak comparison used by `If-None-Match`, it matches both `W/\"v3\"` and `\"v3\"`"
        correct: true
      - text: "It cannot validate byte-range requests with `If-Range`, which require a strong validator"
        correct: true
  - id: "missing-vary-bug"
    type: "single"
    prompt: "An origin gzips JSON responses when the request has `Accept-Encoding: gzip` and sends them uncompressed otherwise, with `Cache-Control: public, max-age=300` and no `Vary` header. Some clients behind the CDN, which send no `Accept-Encoding`, receive gzip bytes they cannot decode. What is the fix?"
    explanation: |-
      A cache stores responses keyed by method and URL. If the representation also depends on a request header, the origin must say so with `Vary` (RFC 9110 §12.5.5); the cache then reuses a stored response only for requests whose listed header values match those of the original request (RFC 9111 §4.1). Without `Vary`, the CDN stored whichever variant it fetched first, the gzip one, and handed it to every client for 300 seconds.

      "Add `Content-Length`" describes size, not encoding; clients still receive bytes they cannot use.

      "The same strong ETag" makes things worse: strong ETags must differ when bytes differ, and sharing one tells caches the variants are interchangeable.

      "Require `Accept: application/json`" varies nothing; the variants differ by `Accept-Encoding`.
    options:
      - text: "Add `Content-Length` to every response, so that clients can detect the compressed body"
      - text: "Add `Vary: Accept-Encoding`, so the CDN keys stored responses on that request header"
        correct: true
      - text: "Give compressed and uncompressed variants the same strong ETag, so the CDN treats them as one"
      - text: "Require clients to send `Accept: application/json`, so the CDN can serve the right variant"
  - id: "vary-star"
    type: "single"
    prompt: "A response includes `Vary: *`. How can a cache use it for later requests?"
    explanation: |-
      `Vary: *` says the response depends on aspects of the request beyond its header fields, such as the client's address. RFC 9111 §4.1 states that a stored response whose `Vary` contains `*` always fails to match a new request. The cache cannot reuse it directly; at most it can use the stored validator to send a conditional request and receive a 304.

      "One copy per header combination" describes a `Vary` that lists specific headers; `*` cannot be satisfied by any combination.

      "Disregards `Vary`" would violate the origin's statement and serve wrong variants.

      "Keys on `Cookie` only" invents a meaning for `*`.
    options:
      - text: "The cache keeps one stored copy for every distinct combination of request header values it sees"
      - text: "The cache disregards the `Vary` header and serves the stored copy for every request to the URL"
      - text: "No later request matches it, so each request is forwarded, perhaps as a conditional request"
        correct: true
      - text: "The cache keys stored copies on the `Cookie` header only, since `*` stands for credentials"
  - id: "revalidation-bandwidth"
    type: "single"
    prompt: "Browsers revalidate a 250,000-byte JavaScript bundle 1,000 times per minute, and it almost never changes. A `304` response is 350 bytes; a `200` response is 250,000 bytes of body plus the same 350 bytes of headers. On average, how much bandwidth do ETag-based 304s save compared with always returning 200?"
    explanation: |-
      Each revalidation that ends in 304 saves the body: 250,000 bytes.

      - Per minute: 1,000 × 250,000 = 250,000,000 bytes.
      - Per second: 250,000,000 / 60 ≈ 4,166,667 bytes.
      - In bits: × 8 ≈ 33,333,333 bit/s ≈ 33.3 Mbit/s.

      "About 250 Mbit/s" treats the per-minute byte count as megabits per second.

      "About 4.2 Mbit/s" is the per-second figure in *megabytes* (4.2 MB/s), mislabeled as bits.

      "About 2 Gbit/s" converts the per-minute count to bits (250,000,000 × 8 = 2,000,000,000) but never divides by the 60 seconds in a minute.
    options:
      - text: "About 250 Mbit/s"
      - text: "About 4.2 Mbit/s"
      - text: "About 2 Gbit/s"
      - text: "About 33.3 Mbit/s"
        correct: true
  - id: "last-modified-granularity"
    type: "single"
    prompt: "A configuration endpoint uses only `Last-Modified` for validation. It is updated twice within the same second, and some clients that revalidated between the two updates keep the first version. Why would an `ETag` avoid this?"
    explanation: |-
      HTTP dates have a resolution of one second (RFC 9110 §5.6.7), and `Last-Modified` is such a date. A client that fetched the first version during that second holds `Last-Modified: 12:00:05`; after the second update the server still reports 12:00:05, so `If-Modified-Since: 12:00:05` yields 304, and the client keeps the outdated version until the next change. An ETag derived from a version number or content hash changes on every update, so `If-None-Match` sees the difference. RFC 9110 §8.8.3 notes that ETags exist partly for this reason.

      "Compared with the client's clock" is wrong: the server compares the date the client echoes back with its own `Last-Modified`.

      "Not permitted on HTTP/2" is false; HTTP semantics are the same in every version.

      "A 304 cannot be cached" is false; 304 responses update stored responses regardless of the validator used.
    options:
      - text: "`Last-Modified` has one-second resolution, so both versions share a date; an ETag can change with every version"
        correct: true
      - text: "`If-Modified-Since` is compared with the client's local clock, which is often behind the server's clock"
      - text: "Conditional requests based on `Last-Modified` are not permitted on HTTP/2, while ETag-based ones are"
      - text: "A `304` produced by a date comparison cannot be cached, while a `304` produced from an ETag can be"
---
## Learning objectives

- Use `If-None-Match` and `If-Modified-Since` to revalidate, and predict 200 versus 304.
- Distinguish strong and weak ETags and where each may be used.
- Use `Vary` so caches keep variants apart, and know what `Vary: *` does.
- Quantify what revalidation saves.

## The invariant

A **validator** identifies a version of a representation so that a client can ask "has it changed?" cheaply. RFC 9110 defines two:

- **ETag** (§8.8.3): an opaque tag chosen by the server. A *strong* ETag changes whenever the bytes change; a *weak* one (`W/"..."`) promises only semantic equivalence (§8.8.1).
- **Last-Modified** (§8.8.2): a date with one-second resolution.

**Conditional requests** (§13) attach a precondition:

| Header | Comparison | When the condition is false |
|---|---|---|
| `If-None-Match` (GET/HEAD) | weak | `304 Not Modified`, no body |
| `If-Modified-Since` (GET/HEAD) | date | `304 Not Modified` |
| `If-Match` | strong | `412 Precondition Failed` (lost-update protection) |
| `If-Range` | strong | send the whole representation instead of the range |

A 304 carries no body; the cache combines the new headers (fresh `Cache-Control`, the same ETag) with the body it already has.

**Variants.** When a response depends on request headers, the origin lists them in **Vary** (§12.5.5). A cache may reuse a stored response only for requests whose listed headers match (RFC 9111 §4.1), so `Vary: Accept-Encoding` keeps gzip and identity copies apart. `Vary: *` matches no request, so the response can only be revalidated, never reused directly. Each listed header multiplies the number of variants a cache must store; varying on high-cardinality headers such as `User-Agent` or `Cookie` makes a shared cache nearly useless.

## Worked example

```bash
GET /app.js HTTP/1.1
Host: static.example
If-None-Match: "a1"
Accept-Encoding: gzip, br

HTTP/1.1 304 Not Modified
ETag: "a1"
Cache-Control: max-age=300
Vary: Accept-Encoding
```

The browser keeps its stored body and treats it as fresh for another 300 seconds. Had the server's version been `"a2"`, it would have answered `200 OK` with the new body and `ETag: "a2"`.

Generating a strong ETag from content and answering conditional GETs:

```python
import hashlib

def etag_for(body: bytes) -> str:
    return '"' + hashlib.sha256(body).hexdigest()[:16] + '"'

def respond(body: bytes, if_none_match: str | None):
    tag = etag_for(body)
    if if_none_match and tag in [t.strip().removeprefix("W/") for t in if_none_match.split(",")]:
        return 304, {"ETag": tag}, b""
    return 200, {"ETag": tag}, body
```

The weak comparison step strips `W/` before comparing, as `If-None-Match` requires. Savings: a 180,000-byte bundle revalidated 2,000 times an hour moves 360 MB per hour as 200s (about 0.8 Mbit/s on average), and almost nothing as 304s.

## Where this shows up today

As of November 2026, RFC 9110 and RFC 9111 (STD 97 and STD 98, June 2022) remain the current definitions of validators, conditional requests and Vary, with no later RFCs updating them. Build tools that put a content hash in each file name (`app.3f9c1a.js`) sidestep revalidation for immutable assets entirely, leaving ETags to handle the URLs whose content does change.

## Pitfalls checklist

- Content negotiation (compression, language, format) without the matching `Vary`.
- `Vary: User-Agent` or `Vary: Cookie` on shared-cache content, fragmenting the cache.
- Weak ETags on resources that need `If-Match` or range requests.
- Relying on `Last-Modified` for resources that change more than once per second.
- ETags that differ between servers in a cluster for identical content (for example, derived from inode numbers).

## Key takeaways

- `If-None-Match` on GET gives 304 when nothing changed and 200 with the new body when it did.
- Weak ETags work for revalidation, not for `If-Match` or `If-Range`.
- `Vary` names the request headers that select a variant; `Vary: *` defeats reuse.
- 304s save the whole body on every revalidation.

## Sources

- RFC 9110, HTTP Semantics (§5.6.7 HTTP-date, §8.8.1–8.8.3 validators, §12.5.5 Vary, §13.1.1–13.1.2 preconditions).
- RFC 9111, HTTP Caching (§4.1, calculating cache keys with Vary; §4.3, validation).
