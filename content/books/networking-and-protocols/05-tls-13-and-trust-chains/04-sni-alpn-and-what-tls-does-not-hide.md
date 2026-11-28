---
title: "SNI, ALPN, Encrypted Client Hello and What TLS Does Not Hide"
questions:
  - id: "missing-sni"
    type: "single"
    prompt: "One IP address hosts `a.example`, `b.example` and `c.example`, each with its own certificate. An old client connects to `b.example` without sending SNI. What happens?"
    explanation: |-
      The certificate is chosen during the handshake, before any HTTP bytes flow. The Server Name Indication extension (RFC 6066) in the ClientHello is how the client tells a multi-site server which name it wants. Without it, the server falls back to a default certificate. The client then checks that certificate against `b.example` and fails unless the default happens to include that name (for example in a SAN list covering all three sites).

      "Sends all three certificates" is not how TLS works: the Certificate message carries one chain for one identity.

      "Reads the Host header" is impossible: the HTTP request is sent only after the handshake, encrypted with keys that depend on the certificate already chosen.

      "An alert listing the host names" does not exist; the protocol has no such message.
    options:
      - text: "The server sends all three certificates in its Certificate message and lets the client pick the one that matches its host name"
      - text: "The server reads the HTTP `Host` header from the request, picks the `b.example` certificate, and finishes the handshake"
      - text: "The server closes the connection with an alert that lists the three host names, and the client retries with the right one"
      - text: "The server sends its default certificate, and the client's name check fails unless that one covers `b.example`"
        correct: true
  - id: "alpn-mismatch-bug"
    type: "single"
    prompt: "A load balancer terminates TLS, advertises ALPN protocols `h2` and `http/1.1`, and forwards the decrypted bytes unchanged over TCP to backends that speak only HTTP/1.1. Browsers fail with protocol errors while `curl --http1.1` works. What is wrong?"
    explanation: |-
      ALPN (RFC 7301) lets the client offer protocols in the ClientHello and the server pick one; in TLS 1.3 the choice is returned in EncryptedExtensions. Whatever is selected is a promise about the bytes that follow. Browsers prefer `h2`, so the load balancer agrees to HTTP/2, the browser sends the HTTP/2 connection preface and binary frames, and the forwarded bytes arrive at a server expecting `GET / HTTP/1.1`. curl with `--http1.1` offers only `http/1.1`, so it works. The fix: advertise only what the next hop really speaks, or have the load balancer terminate HTTP and translate.

      "SNI the backend does not recognize" concerns certificate selection at the load balancer, not request parsing at the backend.

      "TLS 1.3 encrypts ALPN" confuses encryption on the wire with knowledge at the endpoint: the load balancer decrypts it, and the backend never sees TLS at all.

      "HTTP/2 needs a client certificate" is false.
    options:
      - text: "Browsers negotiate `h2` via ALPN and send HTTP/2 frames, which the HTTP/1.1 backend cannot parse"
        correct: true
      - text: "Browsers send an SNI value that the backends do not recognize, so they reject the forwarded requests"
      - text: "TLS 1.3 encrypts the ALPN extension, so the backends never learn the protocol and drop every request"
      - text: "HTTP/2 needs a client certificate, which browsers send but curl does not, so the backends refuse it"
  - id: "visible-metadata"
    type: "multiple"
    prompt: "A browser connects to a site with TLS 1.3 and no Encrypted Client Hello. Which can a passive observer on the network path learn? Select all that apply."
    explanation: |-
      TLS protects application data and, in TLS 1.3, most of the handshake. It does not hide:

      - **Addresses and ports**, which are in the IP and TCP headers that routers need.
      - **SNI**, which travels in the cleartext ClientHello (unless ECH is used).
      - **Traffic shape**: record lengths, counts and timing, which often identify pages or actions.

      Distractors:

      - The **certificate chain** is sent in an encrypted handshake message in TLS 1.3 (in TLS 1.2 it was visible).
      - The **URL path and query** are part of the HTTP request, which is application data and fully encrypted. Only the host name leaks, through SNI and DNS.
    options:
      - text: "The server's IP address and port"
        correct: true
      - text: "The server name sent in the SNI extension"
        correct: true
      - text: "The sizes and timing of the encrypted records"
        correct: true
      - text: "The server's certificate chain"
      - text: "The URL path and query string of each request"
  - id: "ech-observer-view"
    type: "single"
    prompt: "A client uses Encrypted Client Hello (RFC 9849) to reach `private.example` through a provider whose ECH configuration names the public name `cover.provider.net`. What does a passive observer see?"
    explanation: |-
      ECH splits the ClientHello in two. The **ClientHelloInner**, holding the real SNI and other sensitive extensions, is encrypted with HPKE to the public key in the provider's ECH configuration. It travels inside a **ClientHelloOuter**, which is in clear and carries the configuration's *public name* as its SNI. The client-facing server decrypts the inner hello and completes the handshake for the real name. Observers see `cover.provider.net`, which is shared by many sites, and the IP addresses, which may still be revealing if few sites sit behind them.

      "Encrypts the whole ClientHello" is impossible: the outer hello has to be readable to start the handshake and route the connection.

      "A hash of the name" would be trivially reversible by hashing candidate names; ECH uses real public-key encryption.

      "Only ALPN and extensions" understates ECH; hiding the SNI is its primary purpose.
    options:
      - text: "Only IP addresses, because ECH encrypts the whole ClientHello including the outer SNI"
      - text: "A hash of `private.example` in the outer ClientHello, which it can match to a known list"
      - text: "The outer ClientHello's SNI `cover.provider.net` and the IP addresses, but not `private.example`"
        correct: true
      - text: "The real SNI `private.example`, because ECH encrypts only the ALPN list and extensions"
  - id: "size-identifies-page"
    type: "single"
    prompt: "A site serves four pages whose response bodies are 12,288, 48,128, 201,000 and 47,900 bytes. Each is sent in maximum-size TLS 1.3 records (16,384 bytes of plaintext), and each record adds 22 bytes (5 header, 1 content type, 16 tag). An observer sees one response totalling 48,194 bytes of TLS records. Which page was fetched?"
    explanation: |-
      TLS encrypts content but not length. For each page, records = ⌈size / 16,384⌉ and observed bytes = size + 22 × records:

      | Page | Records | Observed bytes |
      |---|---|---|
      | 12,288 | 1 | 12,310 |
      | 48,128 | 3 | 48,194 |
      | 201,000 | 13 | 201,286 |
      | 47,900 | 3 | 47,966 |

      48,194 matches the 48,128-byte page exactly.

      "The 47,900-byte page" is the nearest in size but gives 47,966; the observer's figure is exact.

      "The 201,000-byte page" is off by a factor of four.

      "Cannot be known" is the misconception the table disproves. TLS 1.3 allows record padding to hide lengths, but applications must ask for it, and it costs bandwidth.
    options:
      - text: "The 47,900-byte page"
      - text: "The 201,000-byte page"
      - text: "It cannot be known, because TLS hides the length of the content"
      - text: "The 48,128-byte page"
        correct: true
  - id: "ech-config-source"
    type: "single"
    prompt: "Where does a client normally get the public key it uses to encrypt the ClientHelloInner?"
    explanation: |-
      The client needs the key *before* it sends its first message, so it cannot come from the TLS handshake. RFC 9848 defines how an `ech` parameter in an SVCB or HTTPS DNS record (RFC 9460) carries the ECHConfigList: the HPKE public key, the public name and related parameters. If the server rejects the configuration (for example after a key rotation), it can return fresh *retry configs* in an authenticated way so the client can try again.

      "From the certificate" fails on ordering: the certificate comes after the ClientHello, and in TLS 1.3 it is encrypted anyway.

      "Only from a session ticket" is wrong; tickets serve resumption, not ECH configuration.

      "Deriving it from the public name" would let anyone derive the key, which would provide no secrecy.
    options:
      - text: "From the `ech` parameter of the site's DNS HTTPS record, as RFC 9848 specifies"
        correct: true
      - text: "From the site's certificate, which the server sends before the ClientHello"
      - text: "From a session ticket, which is the only way to obtain the key"
      - text: "By deriving it from the public name, which any client can do"
---
## Learning objectives

- Explain how SNI selects a certificate and what breaks without it.
- Explain ALPN and the consequences of advertising the wrong protocol.
- List the metadata TLS does not hide, and compute how sizes leak.
- Describe Encrypted Client Hello and where its keys come from.

## The invariant

The **ClientHello** is the only message a TLS client sends before keys exist, so everything in it is visible unless something extra is done. Two extensions in it matter for every HTTPS connection:

- **SNI** (Server Name Indication, RFC 6066) names the host the client wants. Servers hosting many names on one address use it to choose a certificate. It is plaintext.
- **ALPN** (RFC 7301) lists application protocols the client can speak (`h2`, `http/1.1`; `h3` for QUIC). The server picks one; in TLS 1.3 the choice comes back in the encrypted EncryptedExtensions. The pick binds the rest of the connection to that protocol.

TLS 1.3 hides application data and most of the handshake, including the certificate. It does **not** hide:

| Leak | Why |
|---|---|
| IP addresses and ports | Routers need them |
| SNI (without ECH) and the rest of the ClientHello | Sent before keys exist |
| Record sizes, counts and timing | Encryption preserves length; TLS 1.3 padding is optional |
| DNS lookups (without encrypted DNS) | Separate protocol |

**Encrypted Client Hello** (ECH, RFC 9849, March 2026) closes the SNI leak. A provider publishes an ECH configuration (an HPKE public key and a *public name*) in the `ech` parameter of its DNS HTTPS record (RFC 9848). The client puts the real ClientHello, the *inner* one, encrypted under that key inside an *outer* ClientHello whose SNI is the public name. The provider's client-facing server decrypts the inner hello and completes the handshake for the real site. Observers see only the shared public name. ECH's protection depends on many sites sharing a provider, and on DNS being fetched privately.

## Worked example

Two connections to the same IP, with and without SNI:

```bash
$ openssl s_client -connect 192.0.2.80:443 -servername b.example < /dev/null 2>/dev/null | grep -E 'subject=|Verification'
subject=CN = b.example
Verification: OK
$ openssl s_client -connect 192.0.2.80:443 -noservername < /dev/null 2>/dev/null | grep -E 'subject=|Verification'
subject=CN = a.example
Verification: OK
```

The second connection succeeds cryptographically, but it presents `a.example`, the default certificate; a client that wanted `b.example` would then fail the host-name check. ALPN is visible the same way: `openssl s_client -alpn h2,http/1.1` prints `ALPN protocol: h2` if the server chose HTTP/2.

Length leakage: a 48,128-byte page needs ⌈48,128 / 16,384⌉ = 3 records and appears on the wire as 48,128 + 3 × 22 = 48,194 bytes of TLS records. A 47,900-byte page appears as 47,966. An observer who has crawled the site can tell them apart from encrypted traffic alone.

## Where this shows up today

As of November 2026, ECH is a Proposed Standard (RFC 9849, March 2026), published together with RFC 9848 for distributing its keys in DNS HTTPS records. Networks that filter by SNI see only provider public names for sites that use it, and must fall back to IP-based policy or endpoint controls.

## Pitfalls checklist

- Clients or health checks that omit SNI and receive the default certificate.
- Advertising `h2` in ALPN from a TLS terminator that cannot speak or translate HTTP/2.
- Assuming HTTPS hides which site a user visits; SNI, DNS and IPs reveal it without ECH and encrypted DNS.
- Treating encrypted responses as size-opaque.
- Deploying ECH without HTTPS records, so clients never learn the key.

## Key takeaways

- SNI picks the certificate before any HTTP is exchanged; it is plaintext without ECH.
- ALPN commits the connection to one application protocol.
- TLS leaks addresses, the ClientHello, and traffic sizes and timing.
- ECH encrypts the real ClientHello to a key published in DNS, leaving only a shared public name visible.

## Sources

- RFC 6066, TLS Extensions (Server Name Indication); RFC 7301, TLS ALPN Extension.
- RFC 9846, TLS 1.3 (EncryptedExtensions, record padding).
- RFC 9849, TLS Encrypted Client Hello; RFC 9848, Bootstrapping TLS Encrypted ClientHello with DNS Service Bindings; RFC 9460 (HTTPS RRs).
