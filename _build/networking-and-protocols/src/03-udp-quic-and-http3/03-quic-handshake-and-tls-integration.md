---
title: "The QUIC Handshake and Its Integration with TLS 1.3"
body_checks:
  - "rtt = 50; assert (2 * rtt, 3 * rtt) == (100, 150) and (1 * rtt, 2 * rtt) == (50, 100)"
  - "assert 3 * 1200 == 3600"
questions:
  - id: "first-byte-tcp-vs-quic"
    type: "single"
    prompt: "A client with no cached state fetches one small resource from a server 80 ms away (RTT), with DNS already resolved. When does the first response byte arrive with HTTPS over TCP and TLS 1.3, and with HTTP/3 over a new QUIC connection?"
    correct:
      - "240 ms over TCP and TLS 1.3; 160 ms over QUIC"
    distractors:
      - "320 ms over TCP and TLS 1.3; 160 ms over QUIC"
      - "160 ms over TCP and TLS 1.3; 80 ms over QUIC"
      - "240 ms over TCP and TLS 1.3; 240 ms over QUIC"
    explanation: |-
      Count round trips before the request can be sent, then add one for the request and response.

      - TCP + TLS 1.3: TCP handshake (1 RTT), then the TLS 1.3 handshake (1 RTT); the request goes with or right after the client's Finished; response after one more RTT. 3 × 80 = 240 ms.
      - QUIC: the transport and TLS 1.3 handshakes are the same exchange (RFC 9001). The client's Initial carries the ClientHello; after 1 RTT it has the server's handshake messages and sends its Finished and the request together; response after one more RTT. 2 × 80 = 160 ms.

      "320 ms" counts TLS 1.2's two round trips, not TLS 1.3's one.

      "160 ms / 80 ms" forgets the request/response round trip itself (or assumes 0-RTT, which needs earlier state the client does not have).

      "240 ms for QUIC too" stacks QUIC's handshake and TLS sequentially, as if QUIC were TCP.
    checks:
      - "rtt = 80; assert 3 * rtt == 240 and 2 * rtt == 160 and 4 * rtt == 320"
  - id: "crypto-frames"
    type: "single"
    prompt: "How does QUIC carry and use the TLS 1.3 handshake?"
    correct:
      - "TLS messages travel in CRYPTO frames; QUIC protects packets with keys exported from TLS"
    distractors:
      - "QUIC wraps complete TLS records, record headers included, inside STREAM frames on stream 0"
      - "QUIC first completes TLS over a short-lived TCP connection and then switches over to UDP"
      - "QUIC sends DTLS 1.3 records over UDP and puts its own frames inside the DTLS payloads"
    explanation: |-
      RFC 9001 uses TLS 1.3 only for its handshake and key schedule. TLS handshake messages (ClientHello, ServerHello, EncryptedExtensions, Certificate, CertificateVerify, Finished) are carried in CRYPTO frames, which have their own offsets per encryption level. The TLS record layer is not used at all; instead TLS hands QUIC the secrets for each level (Initial, Handshake, 0-RTT, 1-RTT), and QUIC uses them for its own packet protection, including header protection.

      "Complete TLS records inside STREAM frames on stream 0" resembles Google's pre-standard QUIC; the IETF design keeps the handshake out of application streams and drops TLS records.

      "Completes TLS over TCP first" would cost the extra round trips QUIC was designed to remove.

      "DTLS 1.3 records" describes a different protocol (RFC 9147); QUIC does not use DTLS.
  - id: "anti-amplification"
    type: "single"
    prompt: "A client's first QUIC datagram is 1,200 bytes. The server's complete first flight (ServerHello, certificate chain and so on) is 6,000 bytes. Under RFC 9000's anti-amplification rule, what happens before the client's address is validated?"
    correct:
      - "The server sends 3,600 bytes and waits for more client datagrams, which adds about one RTT"
    distractors:
      - "The server sends 1,200 bytes, then waits for an ACK from the client after every datagram"
      - "The server sends all 6,000 bytes at once, because it has already received a valid ClientHello"
      - "The server sends 3,600 bytes and trims the certificate chain so its first flight fits the limit"
    explanation: |-
      Until a server has validated that the client really receives packets at its claimed address, RFC 9000 §8.1 limits it to sending at most three times the bytes it has received. This stops a spoofed Initial from turning the server into a reflection amplifier. The client must pad its first Initial to at least 1,200 bytes, which allows 3 × 1,200 = 3,600 bytes in reply.

      A 6,000-byte flight does not fit: the server sends 3,600 bytes and stops. When the client's next datagrams (its ACKs, or its own Handshake packets) arrive, the budget grows and the server sends the remaining 2,400 bytes, one round trip later. This is why large certificate chains cost latency in QUIC, and why certificate compression helps.

      "1,200 bytes, then waits after every datagram" uses a 1× limit; the rule is 3×.

      "Sends all 6,000 bytes" ignores that a valid ClientHello proves nothing about the source address.

      "Trims the certificate chain" would make validation fail; the server must send the whole chain eventually.
    checks:
      - "assert 3 * 1200 == 3600 and 6000 - 3600 == 2400"
  - id: "initial-keys-public"
    type: "single"
    prompt: "Who can decrypt QUIC Initial packets (the ones carrying the ClientHello and ServerHello)?"
    correct:
      - "Anyone who observes them, because their keys come from the client's Destination Connection ID and a published salt"
    distractors:
      - "Only the server, because the client encrypts Initial packets to the public key in the server's certificate"
      - "Only the two endpoints, because Initial keys come from the ECDHE exchange in the ClientHello and ServerHello"
      - "Nobody needs to decrypt them, because Initial packets are sent in plaintext with no protection of any kind"
    explanation: |-
      RFC 9001 derives Initial secrets from a version-specific salt published in the RFC and the Destination Connection ID chosen by the client, which travels in the clear. Any observer can therefore compute the keys and read Initial packets. Their protection exists to stop off-path tampering and to keep middleboxes from treating the handshake as stable plaintext; it is not confidentiality. Real secrecy starts with Handshake packets, protected with keys from the ECDHE exchange.

      "Encrypts to the server's certificate key" describes RSA key transport, which neither TLS 1.3 nor QUIC uses, and the client does not yet have the certificate.

      "Keys come from ECDHE" is true of the *Handshake* and 1-RTT levels; the ECDHE shares are still in flight while Initial packets are being exchanged.

      "Plaintext with no protection" is false; Initial packets are encrypted and authenticated, just with keys anyone can derive.
  - id: "retry-cost"
    type: "single"
    prompt: "A QUIC server under load answers a client's Initial with a Retry packet containing a token. What does this cost the client?"
    correct:
      - "One extra round trip: the client resends its Initial with the token, proving it receives at its address"
    distractors:
      - "Nothing extra, because the Retry token is delivered together with the ServerHello in the first flight"
      - "Two extra round trips, because after a Retry the TLS handshake must restart over a TCP connection"
      - "One extra round trip, but only for clients that were attempting 0-RTT with a session ticket"
    explanation: |-
      Retry (RFC 9000 §8.1.2) is an optional address-validation step: before committing any state or crypto work, the server sends a token; the client must echo it in a new Initial. Receiving the token proves the client can receive packets at its address, and the server can then answer without the 3× limit. The price is one full round trip before the handshake proper begins. Servers can avoid repeating it by issuing tokens in NEW_TOKEN frames for future connections.

      "Delivered with the ServerHello" is wrong: Retry replaces the server's first flight; no ServerHello is sent until the token comes back.

      "Restart over TCP" confuses Retry with fallback to TCP when UDP is blocked.

      "Only for 0-RTT clients" is wrong: Retry applies to any new connection the server chooses to validate.
  - id: "quic-zero-rtt-availability"
    type: "multiple"
    prompt: "Which statements about QUIC 0-RTT are true? Select all that apply."
    correct:
      - "It requires a resumption ticket from an earlier connection to the same server"
      - "The client can send its request in its first flight, before hearing from the server"
    distractors:
      - "A client contacting the server for the first time can use it if the server supports it"
      - "Its data has the same replay protection as data sent after the handshake completes"
      - "It makes the server's Finished message unnecessary for the rest of the connection"
    explanation: |-
      QUIC's 0-RTT is TLS 1.3 early data (RFC 9001). A client that holds a session ticket (a pre-shared key) from a previous connection can derive 0-RTT keys immediately and send application data, such as an HTTP request, alongside its first Initial. The response can then arrive after one RTT instead of two.

      "First contact" fails because the keys come from the earlier session; without a ticket there is nothing to derive them from.

      "Same replay protection" is false: 0-RTT data can be captured and replayed to the server, because it is sent before the server contributes any fresh randomness. Applications must only send replay-safe requests this way (the TLS chapter covers this).

      "Makes Finished unnecessary" is false: the full handshake still completes and authenticates everything; 0-RTT only lets some data travel early.
---

## Learning objectives

- Count round trips for TCP + TLS 1.3 versus QUIC, with and without resumption.
- Explain how QUIC uses TLS 1.3 without the TLS record layer.
- Apply the anti-amplification limit and the cost of Retry.
- Explain who can read Initial packets and what 0-RTT requires.

## The invariant

QUIC merges the transport handshake and the TLS 1.3 handshake into one exchange (RFC 9001):

1. **Client Initial**: a ClientHello in a CRYPTO frame, padded so the datagram is at least 1,200 bytes.
2. **Server Initial + Handshake**: ServerHello in an Initial packet, then EncryptedExtensions, Certificate, CertificateVerify and Finished in Handshake packets. The server may already send 1-RTT data.
3. **Client Handshake + 1-RTT**: the client's Finished and, in the same flight, its first request.

A new connection therefore needs **one round trip before the request**, against two for TCP followed by TLS 1.3. With a session ticket from an earlier connection, the client can send **0-RTT** data in step 1.

The TLS record layer is not used. TLS supplies handshake messages, which QUIC carries in **CRYPTO frames**, and secrets for four encryption levels (Initial, 0-RTT, Handshake, 1-RTT), which QUIC uses for its own packet and header protection. **Initial keys** are derived from a published, version-specific salt and the client-chosen Destination Connection ID, so anyone can derive them; they protect against tampering and ossification, not eavesdropping.

Before a server has validated the client's address, RFC 9000 §8.1 limits it to **three times the bytes received**. Address validation happens implicitly when the client's Handshake packets arrive, or explicitly with a **Retry** packet that hands the client a token to echo, at the cost of one round trip. Large server first flights (big certificate chains) can hit the 3× limit and also cost a round trip.

## Worked example

RTT 50 ms, DNS already cached, small request and response:

| Transport | Request sent at | First response byte at |
|---|---|---|
| TCP + TLS 1.3 | 100 ms | 150 ms |
| QUIC, new connection | 50 ms | 100 ms |
| QUIC with Retry | 100 ms | 150 ms |
| QUIC 0-RTT (resumed) | 0 ms | 50 ms |

Amplification budget: the client's 1,200-byte Initial lets the server send 3 × 1,200 = 3,600 bytes. A first flight of 6,000 bytes sends 3,600 at once and the remainder one round trip later, which matters as much as the Retry row above.

A qlog-style summary of the new-connection case:

```bash
t=0ms    client  Initial[0]    CRYPTO(ClientHello)  PADDING  -> 1200 bytes
t=50ms   server  Initial[0]    CRYPTO(ServerHello)  ACK
         server  Handshake[0]  CRYPTO(EncryptedExtensions, Certificate, CertificateVerify, Finished)
t=50ms   client  Handshake[0]  CRYPTO(Finished)  ACK
         client  1-RTT[0]      STREAM(id=0, HEADERS: GET /)
t=100ms  server  1-RTT[0]      STREAM(id=0, HEADERS: 200, DATA)
```

## Where this shows up today

As of November 2026, the TLS 1.3 specification that QUIC's handshake relies on is RFC 9846 (July 2026), which obsoleted RFC 8446; RFC 9001 still cites RFC 8446, and the protocol on the wire is the same TLS 1.3. Servers that send large certificate chains over QUIC commonly use certificate compression to stay within the 3× budget.

## Pitfalls checklist

- Expecting 0-RTT on a first visit; it needs a ticket from an earlier connection.
- Treating Initial packets as confidential; anyone on the path can decrypt them.
- Serving long certificate chains over QUIC and paying an extra round trip to the amplification limit.
- Enabling Retry for every connection and adding a round trip to every handshake.
- Counting QUIC's handshake as transport RTT plus TLS RTT.

## Key takeaways

- QUIC: one round trip before the request (zero with resumption); TCP + TLS 1.3: two.
- TLS 1.3 handshake messages ride in CRYPTO frames; QUIC does its own packet protection.
- Before address validation a server may send at most 3× what it received.
- Initial packet keys are public by design; Handshake and 1-RTT keys come from ECDHE.

## Sources

- RFC 9000, QUIC Transport (§8.1 address validation and amplification, Retry, padding to 1,200 bytes).
- RFC 9001, Using TLS to Secure QUIC (CRYPTO frames, encryption levels, Initial secrets, 0-RTT).
- RFC 9846, The Transport Layer Security (TLS) Protocol Version 1.3 (obsoletes RFC 8446).
