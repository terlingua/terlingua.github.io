---
title: "ECDHE, Forward Secrecy and AEAD Record Protection"
questions:
  - id: "forward-secrecy-recorded-traffic"
    type: "single"
    prompt: "An attacker recorded a server's encrypted traffic during 2025 and steals the server's certificate private key in 2026. Some recorded sessions used TLS 1.3 with ECDHE; others used TLS 1.2 with static RSA key transport. Which recordings can the attacker decrypt?"
    explanation: |-
      With RSA key transport, the client encrypted the premaster secret under the server's long-term RSA public key. Anyone who later obtains the private key can decrypt that recorded value and derive every key of the session.

      With ECDHE, each handshake generates fresh ephemeral key pairs; the shared secret is computed from them and the private halves are deleted afterwards. The certificate key only *signs* the handshake (CertificateVerify) to authenticate the server; it never encrypts key material. Stealing it later reveals nothing about past sessions. That property is **forward secrecy**, and it is why TLS 1.3 removed RSA key transport entirely.

      "Both kinds" assumes the certificate key protects session keys in every handshake; in ECDHE it only authenticates.

      "Neither kind" overlooks that, with RSA key transport, the hello randoms are public and the premaster secret is the only secret, encrypted to the stolen key.

      "Only TLS 1.3" reverses the outcome.
    options:
      - text: "Both kinds, because the stolen private key is used to protect every TLS session key"
      - text: "Only the RSA key-transport sessions; the ECDHE sessions' ephemeral secrets were discarded"
        correct: true
      - text: "Neither kind, because each session's keys were derived from random values in the hellos"
      - text: "Only the TLS 1.3 sessions, because TLS 1.2 RSA sessions were protected by a session MAC"
  - id: "stolen-key-impact"
    type: "multiple"
    prompt: "A server that offers only TLS 1.3 with ECDHE has its certificate private key stolen. What can the attacker now do? Select all that apply."
    explanation: |-
      The certificate key's job in TLS 1.3 is authentication: it signs the handshake transcript in CertificateVerify. With it, an attacker can complete handshakes *as* the server, so it can impersonate the server or sit in the middle of new connections (answering the client as the server and connecting onward itself). This is why compromised keys must be revoked and why short certificate lifetimes limit the damage window.

      "Decrypt recorded sessions" fails because of forward secrecy: the ECDHE secrets are gone.

      "Other servers" have their own keys; one key's compromise does not transfer.

      "Passively read sessions in progress" fails for the same reason as recorded ones: their traffic keys came from ephemeral ECDHE secrets the attacker never saw. Only active interception of *new* handshakes helps.
    options:
      - text: "Decrypt sessions that it recorded before the theft"
      - text: "Decrypt traffic of other servers that hold different certificate keys"
      - text: "Impersonate the server to clients until the certificate is revoked or expires"
        correct: true
      - text: "Intercept new connections as an active man-in-the-middle"
        correct: true
      - text: "Read sessions in progress by passively observing their encrypted records on the wire"
  - id: "tampered-record"
    type: "single"
    prompt: "An on-path attacker flips one bit inside an encrypted TLS 1.3 application-data record and fixes up the TCP checksum. What does the receiving TLS stack do?"
    explanation: |-
      TLS 1.3 protects every record with an AEAD cipher (AES-GCM, ChaCha20-Poly1305), which encrypts and authenticates in one operation. Decryption verifies the 16-byte authentication tag over the ciphertext and associated data before returning any plaintext. A single flipped bit makes the check fail. RFC 9846 requires the receiver to terminate the connection with a `bad_record_mac` alert; there is no partial or garbled output.

      "Garbled plaintext" is how unauthenticated encryption (or old CBC-without-MAC designs) behaves, and it is what AEAD exists to prevent.

      "Asks for a retransmission" is not part of TLS; it has no retransmission mechanism, and an integrity failure is treated as an attack.

      "Accepts it because of the TCP checksum" confuses error detection with integrity: anyone can recompute a checksum, but nobody without the key can recompute an AEAD tag.
    options:
      - text: "It decrypts the record into slightly garbled plaintext and hands that to the application to reject"
      - text: "It discards the record and asks the peer to retransmit it, so the connection carries on normally"
      - text: "It accepts the record, because a corrected TCP checksum shows the bytes arrived as they were sent"
      - text: "The AEAD tag check fails, so it sends a `bad_record_mac` alert and closes without releasing the plaintext"
        correct: true
  - id: "per-record-nonce"
    type: "single"
    prompt: "A TLS 1.3 connection's client write IV is `1a2b3c4d5e6f708192a3b4c5` (12 bytes, hex). What nonce protects the record with sequence number 1?"
    explanation: |-
      RFC 9846 §5.3: the 64-bit record sequence number is left-padded with zeros to the IV length and XORed with the static write IV. Sequence number 1 padded to 12 bytes is `000000000000000000000001`. XOR changes only the last byte: `c5` XOR `01` = `c4` (1100 0101 XOR 0000 0001 = 1100 0100). Nonce: `1a2b3c4d5e6f708192a3b4c4`.

      Each record thus gets a unique nonce without sending one on the wire, and the sequence number (never transmitted, but implicit) also makes reordered, dropped or replayed records fail authentication.

      "`...c5`" is the bare IV, which would reuse one nonce for every record, catastrophic for AES-GCM.

      "`...c6`" adds 1 instead of XORing; XOR and addition coincide only when no bits overlap.

      "`...c501`" appends the sequence number, producing a 13-byte value that is not the required nonce length.
    options:
      - text: "`1a2b3c4d5e6f708192a3b4c4`"
        correct: true
      - text: "`1a2b3c4d5e6f708192a3b4c5`"
      - text: "`1a2b3c4d5e6f708192a3b4c6`"
      - text: "`1a2b3c4d5e6f708192a3b4c501`"
  - id: "record-count-overhead"
    type: "single"
    prompt: "A server sends 1,000,000 bytes of application data over TLS 1.3 in maximum-size records (16,384 bytes of plaintext each, no padding). Each record adds a 5-byte header, a 1-byte inner content type and a 16-byte AEAD tag. How many records are needed, and how many bytes of TLS overhead do they add?"
    explanation: |-
      The maximum TLSPlaintext fragment is 2^14 = 16,384 bytes.

      - Records: 1,000,000 / 16,384 = 61.04, so 62 (61 full records carry 999,424 bytes; one more carries 576).
      - Overhead per record: 5 + 1 + 16 = 22 bytes; 62 × 22 = 1,364 bytes.

      "61 records" rounds down and drops the last 576 bytes.

      "992 bytes" counts only the 16-byte tags (62 × 16).

      "977 records" divides by 1,024, a common buffer size, instead of the 16,384-byte record limit.
    options:
      - text: "61 records and 1,342 bytes"
      - text: "62 records and 1,364 bytes"
        correct: true
      - text: "62 records and 992 bytes"
      - text: "977 records and 21,494 bytes"
  - id: "why-hybrid-kex"
    type: "single"
    prompt: "TLS clients that adopt post-quantum key exchange combine X25519 with ML-KEM-768 (the `X25519MLKEM768` group) instead of using ML-KEM-768 alone. Why?"
    explanation: |-
      A hybrid key exchange (RFC 10024 defines `X25519MLKEM768` for TLS 1.3) runs both algorithms and feeds both shared secrets into the key schedule. An attacker must break both to recover the keys. ML-KEM addresses "harvest now, decrypt later": traffic recorded today could be decrypted if a large quantum computer later breaks elliptic-curve Diffie-Hellman. X25519 is kept because ML-KEM is much newer, and a classical attack on it or an implementation flaw would otherwise leave no protection.

      "ML-KEM cannot produce a shared secret on its own" is false; it is a complete key-encapsulation mechanism.

      "X25519 authenticates the server" confuses key exchange with authentication, which comes from the certificate signature.

      "ML-KEM shares are smaller" is backwards: the client's hybrid share is 1,216 bytes (1,184 for ML-KEM-768 plus 32 for X25519), much larger than X25519's 32, which can push the ClientHello beyond one packet.
    options:
      - text: "ML-KEM cannot produce a shared secret on its own, so it needs X25519 to finish the key agreement"
      - text: "X25519 authenticates the server while ML-KEM encrypts the records, so both parts are required"
      - text: "Keys stay safe if either part holds: ML-KEM against quantum attacks, X25519 against ML-KEM flaws"
        correct: true
      - text: "ML-KEM key shares are smaller than X25519 shares, so combining them shrinks the ClientHello"
---
## Learning objectives

- Separate confidentiality, integrity and authenticity and name the TLS mechanism for each.
- Explain forward secrecy and why TLS 1.3 requires (EC)DHE.
- Describe AEAD record protection, including nonce construction and record limits.
- Explain why post-quantum key exchange is deployed as a hybrid.

## The invariant

A secure channel needs three properties, and TLS 1.3 assigns each a mechanism:

| Property | Question it answers | TLS 1.3 mechanism |
|---|---|---|
| Confidentiality | Can others read it? | AEAD encryption with keys from (EC)DHE |
| Integrity | Was it modified? | AEAD authentication tag on every record |
| Authenticity | Am I talking to the right server? | Certificate chain plus CertificateVerify signature over the handshake |

**Key exchange.** Each side generates an ephemeral key pair, sends the public half in `key_share`, and computes the same shared secret. The key schedule (RFC 9846 §7.1) derives handshake and traffic keys from it. The ephemeral private keys are discarded, so even a later theft of the server's long-term signing key cannot decrypt recorded traffic: **forward secrecy**. TLS 1.2 also allowed static RSA key transport, where the session secret was encrypted to the certificate key, and a stolen key decrypts every recorded session; TLS 1.3 removed that option. The long-term key now only *authenticates*.

**Record protection** (RFC 9846 §5.2–5.3). Application data is split into records of at most 2^14 = 16,384 bytes of plaintext. Each record is encrypted with an AEAD algorithm that also authenticates it. The nonce is the per-direction write IV XORed with the record's 64-bit sequence number, so it is unique per record, and sequence numbers are implicit: a dropped, reordered or replayed record fails authentication. Any failure ends the connection with a `bad_record_mac` alert. On the wire a record costs 5 bytes of header, 1 byte of inner content type and a 16-byte tag. Long connections switch to fresh keys with KeyUpdate before the AEAD's usage limits are reached.

**Hybrid post-quantum key exchange** (RFC 10024) combines a classical ECDH share with an ML-KEM encapsulation in one `key_share`, so the session is protected as long as either algorithm holds.

## Worked example

A 100,000-byte response in maximum-size records:

| Item | Value |
|---|---|
| Records | ⌈100,000 / 16,384⌉ = 7 |
| Overhead | 7 × (5 + 1 + 16) = 154 bytes |
| Nonce for record n | write_iv XOR pad64(n) |

Seeing these parts in Python with the `cryptography` package:

```python
from cryptography.hazmat.primitives.ciphers.aead import AESGCM
import os

key, iv = AESGCM.generate_key(bit_length=128), os.urandom(12)
aead = AESGCM(key)

def nonce(seq: int) -> bytes:
    return bytes(a ^ b for a, b in zip(iv, seq.to_bytes(12, "big")))

header = bytes([23, 3, 3, 0, 22])          # opaque_type, legacy_version, length
record = aead.encrypt(nonce(0), b"hello" + bytes([23]), header)
print(len(record))                          # 5 + 1 + 16: data, content type, tag
tampered = bytes([record[0] ^ 1]) + record[1:]
try:
    aead.decrypt(nonce(0), tampered, header)
except Exception as e:
    print(type(e).__name__)
```

Output:

```bash
22
InvalidTag
```

The record body is the 6-byte inner plaintext plus the tag; flipping one bit makes decryption fail outright rather than return altered data.

## Where this shows up today

As of November 2026, RFC 10024 standardizes the `X25519MLKEM768` hybrid group (codepoint 0x11EC). Cloudflare's post-quantum support page lists it as the default key agreement in Chrome 131+, Firefox 132+ on desktop and Safari 26+. Signatures and certificates remain classical, because recorded traffic is at risk from future decryption but past authentication is not.

## Pitfalls checklist

- Keeping TLS 1.2 static-RSA cipher suites enabled on servers that also speak TLS 1.3.
- Treating the certificate key as an encryption key; in TLS 1.3 it only signs.
- Reusing an AEAD nonce, for example by building custom framing on raw AES-GCM.
- Middleboxes that assume a ClientHello fits in one packet, which large hybrid key shares break.
- Confusing a TCP checksum with an integrity check.

## Key takeaways

- ECDHE gives forward secrecy; the certificate key only authenticates.
- Every record is AEAD-protected with a nonce derived from the IV and sequence number.
- One flipped bit ends the connection with `bad_record_mac`; nothing is released.
- Hybrid key exchange stays secure while either component is unbroken.

## Sources

- RFC 9846, TLS 1.3 (§5.2 record payload protection, §5.3 per-record nonce, §7.1 key schedule).
- RFC 10024, Post-quantum hybrid ECDHE-MLKEM Key Agreement for TLSv1.3.
- Cloudflare Docs, "Post-quantum cryptography: support", read November 2026.
