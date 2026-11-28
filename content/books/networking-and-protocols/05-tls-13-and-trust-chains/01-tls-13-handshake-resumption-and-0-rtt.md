---
title: "The TLS 1.3 Handshake, Resumption and 0-RTT Replay"
questions:
  - id: "full-handshake-first-byte"
    type: "single"
    prompt: "Over an already-established TCP connection with an RTT of 40 ms, a client sends a TLS 1.3 ClientHello at t = 0 (full handshake, no HelloRetryRequest) and then an HTTP request as soon as it can. When does the first response byte arrive?"
    explanation: |-
      TLS 1.3 needs one round trip before the client may send application data:

      - t = 0: ClientHello with a key share.
      - t = 40 ms: ServerHello (with the server's key share) and the encrypted EncryptedExtensions, Certificate, CertificateVerify and Finished arrive. The client now has all keys; it sends its Finished and the request together.
      - t = 80 ms: the response arrives.

      "40 ms" is what 0-RTT early data can achieve on resumption, not a full handshake.

      "120 ms" assumes TLS 1.2's two round trips before application data.

      "160 ms" adds a TCP handshake, which the question says already happened, on top of TLS 1.2.
    options:
      - text: "At 80 ms"
        correct: true
      - text: "At 40 ms"
      - text: "At 120 ms"
      - text: "At 160 ms"
  - id: "hello-retry-request"
    type: "single"
    prompt: "A client's ClientHello offers a key share only for X25519. The server is configured to accept only P-256. What happens?"
    explanation: |-
      In TLS 1.3 the client guesses which group the server will accept and sends a key share for it in the first flight; that guess is what makes the 1-RTT handshake possible. If the server supports a group the client listed in `supported_groups` but no share was sent for it, the server answers with a **HelloRetryRequest** naming the group. The client sends a second ClientHello with a share for that group, and the handshake proceeds: one extra round trip.

      "Falls back to RSA key transport" is impossible: TLS 1.3 removed RSA key transport entirely; every handshake uses (EC)DHE or a PSK.

      "Generates the share for both sides" breaks Diffie-Hellman: each side must generate its own private value.

      "Fails and needs TLS 1.2" is wrong because a mutually supported group exists; failure happens only if there is no common group.
    options:
      - text: "The server falls back to RSA key transport, so the handshake still completes in a single round trip"
      - text: "A HelloRetryRequest asks for a P-256 share; the client resends, costing one extra RTT"
        correct: true
      - text: "The server generates the P-256 share for both sides and continues, because shares are chosen by the server"
      - text: "The handshake fails with `handshake_failure`, and the client has to reconnect using TLS 1.2 instead"
  - id: "encrypted-handshake-messages"
    type: "multiple"
    prompt: "In a TLS 1.3 full handshake, which of these are sent encrypted? Select all that apply."
    explanation: |-
      As soon as the ServerHello delivers the server's key share, both sides can compute the handshake traffic keys, and every later handshake message is encrypted with them (RFC 9846 §2): EncryptedExtensions, CertificateRequest if any, Certificate, CertificateVerify and Finished. Hiding the certificate is a change from TLS 1.2, where it travelled in clear.

      "The ClientHello" must be readable, because no keys exist yet; that is why SNI leaks unless Encrypted Client Hello is used.

      "The ServerHello" must also be in clear, because it carries the server's key share that the client needs to derive the keys.
    options:
      - text: "The ClientHello, including the server name (SNI)"
      - text: "The ServerHello, including the server's key share"
      - text: "The server's Certificate message"
        correct: true
      - text: "EncryptedExtensions, which carries the negotiated ALPN protocol"
        correct: true
      - text: "The client's and the server's Finished messages"
        correct: true
  - id: "zero-rtt-post-replay"
    type: "single"
    prompt: "A mobile banking client resumes a TLS 1.3 session and sends `POST /transfers` as 0-RTT early data. An attacker records the first flight and replays it to another server in the same cluster, and the transfer is executed twice. What is the correct design rule?"
    explanation: |-
      0-RTT data is encrypted under a key derived only from the resumption PSK and the ClientHello. The server contributes no fresh randomness before processing it, so a byte-identical copy decrypts correctly on any server that holds the same ticket key. RFC 9846 §8 therefore says TLS does not provide replay protection for 0-RTT data across connections, and describes mitigations (single-use tickets, ClientHello recording, freshness checks) that are hard to make airtight in a distributed cluster. Applications must send only requests that are safe to execute more than once. For HTTP, RFC 8470 lets a server respond `425 Too Early`, which makes the client retry after the handshake completes.

      "Cannot be decrypted" is exactly wrong: decryptability by the legitimate server is what makes the replay work.

      "Record sequence numbers" are per connection; a replay is a *new* connection starting again at zero.

      "Finished authenticates the early data" is too late: the server may already have processed the request when Finished arrives, and the attacker can simply never complete the replayed handshake.
    options:
      - text: "None is needed: early data is encrypted with the session key, so a replayed copy cannot be decrypted by any server"
      - text: "None is needed: TLS record sequence numbers let every server detect and drop records it has seen before"
      - text: "None is needed: the Finished message authenticates the early data, so a replay fails before the request runs"
      - text: "Send only replay-safe requests as early data; reject others, for example with `425 Too Early`"
        correct: true
  - id: "psk-resumption"
    type: "single"
    prompt: "A client resumes a TLS 1.3 session with a ticket from an earlier connection, using the `psk_dhe_ke` mode and no early data. Compared with a full handshake, what does the resumed handshake leave out?"
    explanation: |-
      A NewSessionTicket from the earlier connection gives the client a pre-shared key. In the resumed handshake the ClientHello offers it in `pre_shared_key`; the server accepts it in its ServerHello. Possession of the PSK authenticates the server, so the expensive Certificate and CertificateVerify messages (and signature operations) are skipped. With `psk_dhe_ke`, both sides also exchange fresh ECDHE shares, and the traffic keys depend on both the PSK and the new Diffie-Hellman secret, so compromise of the ticket key later does not expose this session's traffic. The handshake still takes one round trip.

      "Data in clear until Finished" never happens in TLS 1.3.

      "Previous keys reused unchanged" is false: every connection derives fresh keys through the key schedule (RFC 9846 §7.1).

      "Skips the ServerHello" is false: the server must confirm it accepted the PSK and send its key share.
    options:
      - text: "The Certificate and CertificateVerify messages; a fresh ECDHE exchange still gives forward secrecy"
        correct: true
      - text: "The whole key exchange; data is sent in clear until both Finished messages have been exchanged"
      - text: "Deriving new traffic keys; the previous session's traffic keys are reused unchanged"
      - text: "The ServerHello; the client derives all the keys by itself from the ticket it already holds"
  - id: "zero-rtt-new-tcp"
    type: "single"
    prompt: "A client that holds a valid ticket opens a *new* TCP connection (RTT 40 ms) and sends its request as TLS 1.3 0-RTT early data as soon as TCP is established. When does the first response byte arrive, counting from the TCP SYN?"
    explanation: |-
      - t = 0 → 40 ms: TCP handshake (SYN, SYN-ACK, then the ACK goes out with the first data).
      - t = 40 ms: ClientHello plus early data carrying the request.
      - t = 80 ms: the server's flight arrives, and it can include the response to the 0-RTT request.

      Without 0-RTT the request would wait for the TLS round trip: 40 (TCP) + 40 (TLS) + 40 (request) = 120 ms. 0-RTT removes TLS's round trip, not TCP's.

      "40 ms" counts only TLS and forgets the TCP handshake on a new connection.

      "120 ms" is the timing without early data.

      "0 ms" is not achievable: some handshake round trip is always paid on a new TCP connection (TCP Fast Open aside).
    options:
      - text: "At 40 ms"
      - text: "At 80 ms"
        correct: true
      - text: "At 120 ms"
      - text: "At 0 ms"
---
## Learning objectives

- Trace the TLS 1.3 full handshake and count its round trips.
- Explain HelloRetryRequest, PSK resumption and which messages are encrypted.
- Explain why 0-RTT early data can be replayed and how applications must respond.
- Compute time to first byte with and without early data.

## The invariant

TLS 1.3 was first published as RFC 8446 (2018) and is now specified by **RFC 9846** (July 2026), a backward-compatible revision that keeps the version number. Its full handshake takes **one round trip** before the client sends application data:

```bash
Client                                               Server
ClientHello + key_share + SNI + ALPN offer   ---->
                                                     ServerHello + key_share
                                                     {EncryptedExtensions}   (selected ALPN)
                                                     {Certificate}
                                                     {CertificateVerify}
                                             <----   {Finished}  [application data allowed]
{Finished} + [application data]              ---->
```

Braces mark messages encrypted with handshake keys, which both sides can derive once they have each other's key shares. Only the ClientHello and ServerHello are in clear.

The 1-RTT design depends on the client **guessing** a key-exchange group and sending a share in its first flight. If the server wants a group the client supports but sent no share for, it answers with a **HelloRetryRequest**, and the handshake costs one extra round trip.

After a handshake the server may send **NewSessionTicket** messages. A later connection can present the ticket as a **pre-shared key** (PSK). Possession of the PSK authenticates the server, so the certificate is skipped; with the `psk_dhe_ke` mode a fresh ECDHE exchange keeps forward secrecy.

With a PSK the client may also send **0-RTT early data** in its first flight (RFC 9846 §2.3). Its keys depend only on the PSK and the ClientHello, with no fresh server input, so an attacker who captures the first flight can **replay** it, to the same server later or to another server sharing the ticket keys. RFC 9846 §8 says TLS does not provide replay protection for 0-RTT across connections and lists partial mitigations: single-use tickets (§8.1), ClientHello recording (§8.2) and freshness checks (§8.3). The application rule is simple: only send requests in early data that are safe to process twice. HTTP servers can refuse with **425 Too Early** (RFC 8470), and the client resends after the handshake.

## Worked example

RTT 40 ms; times are when the first response byte arrives, counted from the first packet the client sends.

| Scenario | Round trips | First response byte |
|---|---|---|
| Existing TCP connection, full TLS 1.3 | TLS 1 + request 1 | 80 ms |
| Existing TCP connection, HelloRetryRequest | TLS 2 + request 1 | 120 ms |
| New TCP connection, full TLS 1.3 | TCP 1 + TLS 1 + request 1 | 120 ms |
| New TCP connection, resumption with 0-RTT | TCP 1 + request in first TLS flight 1 | 80 ms |

`openssl s_client` shows the negotiated parameters and whether resumption and early data were used (illustrative output against a server that accepts early data):

```bash
$ openssl s_client -connect example.com:443 -tls1_3 -sess_out t.pem < /dev/null 2>/dev/null | grep -E 'Protocol|Cipher|Server Temp Key'
New, TLSv1.3, Cipher is TLS_AES_128_GCM_SHA256
Server Temp Key: X25519, 253 bits
$ openssl s_client -connect example.com:443 -tls1_3 -sess_in t.pem -early_data req.txt < /dev/null 2>/dev/null | grep -E 'Reused|Early data'
Reused, TLSv1.3, Cipher is TLS_AES_128_GCM_SHA256
Early data was accepted
```

## Where this shows up today

As of November 2026, RFC 9846 is the TLS 1.3 specification; it tightens rules from RFC 8446 (for example, forbidding reuse of key shares and negotiation of TLS 1.0 and 1.1) while staying interoperable with existing TLS 1.3 implementations. A server or CDN that enables 0-RTT needs exactly the gate described above: accept early data only for replay-safe requests and answer `425 Too Early` otherwise.

## Pitfalls checklist

- Allowing non-idempotent requests in 0-RTT early data.
- Assuming the TLS layer deduplicates replayed early data across servers.
- Offering key shares for groups the server rarely accepts and paying a HelloRetryRequest.
- Counting TLS 1.3 as two round trips, or forgetting the TCP handshake on new connections.
- Treating session tickets as harmless; their encryption keys must be rotated and protected.

## Key takeaways

- TLS 1.3: one round trip, with everything after ServerHello encrypted.
- A wrong key-share guess costs a HelloRetryRequest round trip.
- PSK resumption skips the certificate; `psk_dhe_ke` keeps forward secrecy.
- 0-RTT data is replayable; send only replay-safe requests in it.

## Sources

- RFC 9846, The Transport Layer Security (TLS) Protocol Version 1.3 (§1.2, §2.3, §7.1, §8); RFC 8446 (original TLS 1.3, obsoleted).
- RFC 8470, Using Early Data in HTTP (425 Too Early, Early-Data header).
