---
title: "X.509 Chain Validation, Name Matching and Revocation"
body_checks:
  - "assert 100 * 86400 == 8_640_000"
questions:
  - id: "chain-link-check"
    type: "single"
    prompt: "A client validates the chain leaf → intermediate → root. Which checks link the leaf certificate to the intermediate?"
    correct:
      - "The leaf's signature verifies with the intermediate's key, and its issuer equals the intermediate's subject"
    distractors:
      - "The intermediate's signature verifies with the leaf's public key, and both certificates list the same subject"
      - "The leaf's serial number appears in the intermediate's extensions as a certificate that it has approved"
      - "Both certificates carry the same subject alternative names, and their validity periods start together"
    explanation: |-
      RFC 5280 path validation (§6) processes the chain from the trust anchor down. For each certificate it checks that the signature verifies with the *issuer's* public key and that the certificate's issuer name matches the subject name of the certificate above it. Here the intermediate issued the leaf, so the leaf's signature is checked with the intermediate's key, and the leaf's `issuer` must equal the intermediate's `subject`. The same check links the intermediate to the root.

      "Verifies with the leaf's public key" reverses the direction of trust: issuers sign subjects, not the other way round.

      "Serial number in the intermediate's extensions" invents a whitelist; issuers do not list the certificates they sign.

      "The same SANs" is wrong: an intermediate normally has no SANs, and names are matched issuer-to-subject.
  - id: "missing-intermediate"
    type: "single"
    prompt: "A new site works in a desktop browser, but `curl` and a Java client fail with \"unable to get local issuer certificate\". The server is configured with only its leaf certificate. What is the cause?"
    correct:
      - "The server omits the intermediate; some browsers fetch it themselves, other clients need the full chain"
    distractors:
      - "The server omits the root certificate, which every client needs to receive from the server during the handshake"
      - "The client machines' clocks are wrong, so the leaf certificate appears not yet valid to curl and Java"
      - "The certificate uses an ECDSA key, which curl and Java reject unless explicitly configured to accept it"
    explanation: |-
      Clients trust *roots* from their own trust store. To build a path from the leaf to a root, they need every intermediate, and the TLS server is expected to send them in its Certificate message. Some browsers paper over a missing intermediate by using intermediates they have cached or by fetching the issuer's certificate from the URL in the leaf's Authority Information Access extension. Many other clients (curl with OpenSSL, Java, most HTTP libraries) do not, so the path cannot be built and validation fails. The fix is to configure the server with the full chain (leaf followed by intermediates).

      "The server omits the root" is not a problem: the root must come from the client's trust store anyway, and servers usually leave it out.

      "Clocks are wrong" would produce a "not yet valid" or "expired" error, not a missing-issuer error.

      "ECDSA keys" are supported by all mainstream TLS clients.
  - id: "san-wildcard-matching"
    type: "multiple"
    prompt: "A certificate has subject `CN=www.example.com` and subject alternative names `DNS:example.com` and `DNS:*.api.example.com`. Which host names does a client following RFC 9525 accept? Select all that apply."
    correct:
      - "`example.com`"
      - "`v1.api.example.com`"
    distractors:
      - "`www.example.com`"
      - "`a.b.api.example.com`"
      - "`api.example.com`"
    explanation: |-
      RFC 9525 (which obsoleted RFC 6125) says clients match DNS names against the subject alternative name extension and that the Common Name MUST NOT be used to identify a service. A wildcard must be the entire left-most label and matches exactly one label (§6.3).

      - `example.com` matches `DNS:example.com` exactly.
      - `v1.api.example.com`: `*` matches the single label `v1`.

      Distractors:

      - `www.example.com` appears only in the CN, which is ignored.
      - `a.b.api.example.com` needs the wildcard to cover two labels (`a.b`).
      - `api.example.com` has no label for the wildcard to match; `*.api.example.com` does not cover `api.example.com` itself.
    checks:
      - "def match(p, h):\n    pl, hl = p.split('.'), h.split('.')\n    return len(pl) == len(hl) and all(a == '*' or a == b for a, b in zip(pl, hl))\nsans = ['example.com', '*.api.example.com']\nok = [h for h in ['example.com', 'v1.api.example.com', 'www.example.com', 'a.b.api.example.com', 'api.example.com'] if any(match(p, h) for p in sans)]\nassert ok == ['example.com', 'v1.api.example.com']"
  - id: "leaf-cannot-issue"
    type: "single"
    prompt: "An attacker holds a valid certificate and private key for `attacker.example`, issued by a public CA. It uses that key to sign a certificate for `bank.example` and presents the chain `bank.example` → `attacker.example` → intermediate → root. Why does validation fail?"
    correct:
      - "The `attacker.example` certificate is not a CA: it lacks `basicConstraints` with `cA` true and the `keyCertSign` usage"
    distractors:
      - "The forged certificate's names do not match, because `bank.example` is not listed in the attacker's own SANs"
      - "The chain is one link too long, because browsers accept no more than three certificates in a path"
      - "The forged certificate's signature cannot verify, because the attacker's key is shorter than a CA key"
    explanation: |-
      RFC 5280 §6 requires every certificate used as an issuer in the path to be a CA certificate: its `basicConstraints` extension must have `cA` set to true, and if `keyUsage` is present it must include `keyCertSign`. Ordinary server certificates have `cA` false. So although the forged certificate's signature *does* verify with the attacker's public key, the attacker's certificate is not allowed to issue, and the path is rejected. (`pathLenConstraint` on CA certificates further limits how many CA certificates may follow.)

      "Names do not match" mixes two checks: the issuing certificate's SANs are irrelevant to what it signs; name matching applies to the leaf against the host name.

      "One link too long" invents a fixed length; paths with two intermediates are common.

      "The key is shorter" is irrelevant; the signature is mathematically valid, which is why the CA flag matters.
  - id: "max-validity-date"
    type: "single"
    prompt: "Under CA/Browser Forum ballot SC-081, publicly trusted TLS certificates issued on or after 2027-03-15 may have a validity period of at most 100 days, where the validity period runs from `notBefore` through `notAfter` inclusive. For `notBefore` = 2027-06-01 00:00:00 UTC, what is the latest permitted `notAfter`?"
    correct:
      - "2027-09-08 23:59:59 UTC"
    distractors:
      - "2027-12-17 23:59:59 UTC"
      - "2027-07-17 23:59:59 UTC"
      - "2028-07-02 23:59:59 UTC"
    explanation: |-
      RFC 5280 §4.1.2.5 and the CA/Browser Forum Baseline Requirements both treat the validity period as inclusive of both endpoints. 100 days = 8,640,000 seconds, so the last valid second is `notBefore` + 8,640,000 − 1 s.

      2027-06-01 00:00:00 + 100 days = 2027-09-09 00:00:00; one second earlier is 2027-09-08 23:59:59 UTC.

      "2027-12-17" uses the 200-day limit, which applies to certificates issued from 2026-03-15 until 2027-03-15.

      "2027-07-17" uses the 47-day limit, which starts on 2029-03-15.

      "2028-07-02" uses the old 398-day maximum.
    checks:
      - "import datetime as d\nnb = d.datetime(2027, 6, 1)\nassert nb + d.timedelta(seconds=100 * 86400 - 1) == d.datetime(2027, 9, 8, 23, 59, 59)\nassert nb + d.timedelta(seconds=200 * 86400 - 1) == d.datetime(2027, 12, 17, 23, 59, 59)\nassert nb + d.timedelta(seconds=47 * 86400 - 1) == d.datetime(2027, 7, 17, 23, 59, 59)\nassert nb + d.timedelta(seconds=398 * 86400 - 1) == d.datetime(2028, 7, 2, 23, 59, 59)"
  - id: "ocsp-stapling"
    type: "single"
    prompt: "What does OCSP stapling change about revocation checking?"
    correct:
      - "The server fetches a CA-signed, time-limited OCSP response and sends it in the handshake, so clients need not contact the CA"
    distractors:
      - "The client downloads the CA's complete revocation list during every handshake and checks the serial number against it"
      - "The server signs a statement of its own revocation status with its certificate key and sends it with the certificate"
      - "The CA pushes revocation notices to every client, so revoked certificates are rejected the moment the CA revokes them"
    explanation: |-
      With plain OCSP, the client asks the CA's responder about each certificate, which adds latency, fails when the responder is unreachable, and tells the CA which sites the user visits. With stapling, the *server* periodically fetches the OCSP response (signed by the CA and valid for a limited time) and includes it in the handshake, so the client gets fresh, CA-signed status without contacting anyone.

      "Downloads the complete list every handshake" describes naive CRL use, which stapling avoids.

      "Signs its own revocation status" would be useless: a compromised key could vouch for itself. The response must be signed by the CA (or its delegated responder).

      "The CA pushes notices" describes no standard mechanism; clients learn about revocation only through CRLs, OCSP responses or browser-curated revocation lists, all with delays.
---

## Learning objectives

- Validate a certificate chain the way RFC 5280 does.
- Match host names against SANs and wildcards per RFC 9525.
- Diagnose missing-intermediate and CA-flag failures.
- Compare CRLs, OCSP, stapling and short-lived certificates.

## The invariant

A TLS client authenticates a server by building a **certification path** from the server's leaf certificate to a **trust anchor** (a root certificate in the client's trust store) and validating it (RFC 5280 §6). For every certificate in the path:

1. **Signature**: it verifies with the public key of the certificate above it (its issuer).
2. **Names chain**: its issuer name equals that issuer's subject name.
3. **Validity**: the current time is within `notBefore` through `notAfter` (inclusive).
4. **CA rights**: every issuing certificate has `basicConstraints` `cA` = true and, if `keyUsage` is present, `keyCertSign`; `pathLenConstraint` and name constraints are respected.
5. **Status**: it has not been revoked, as far as the client can tell.

Then the leaf must match the host name. RFC 9525 (obsoleting RFC 6125) matches DNS names only against the **subject alternative name** extension; the Common Name MUST NOT be used. A wildcard must be the whole left-most label and matches exactly one label.

The server sends the leaf and the intermediates; the client supplies the root. A missing intermediate breaks clients that do not fetch or cache intermediates themselves.

**Revocation** has three forms:

- **CRLs**: the CA publishes a signed list of revoked serial numbers at a URL in the certificate.
- **OCSP**: the client asks the CA's responder about one certificate. With **stapling**, the server fetches the signed response and sends it in the handshake.
- **Short-lived certificates**: if a certificate expires within days, revocation matters less, because its maximum exposure is short.

Clients often *soft-fail* when status is unavailable, which is why shorter lifetimes are the dependable control.

## Worked example

Inspecting the chain a server actually sends:

```bash
$ openssl s_client -connect shop.example:443 -servername shop.example -showcerts < /dev/null 2>/dev/null | grep -E '^ [0-9] s:|^   i:'
 0 s:CN = shop.example
   i:C = US, O = Example CA, CN = Example Issuing CA R2
 1 s:C = US, O = Example CA, CN = Example Issuing CA R2
   i:C = US, O = Example CA, CN = Example Root CA X1
$ openssl x509 -in leaf.pem -noout -ext subjectAltName,basicConstraints,keyUsage
X509v3 Subject Alternative Name:
    DNS:shop.example, DNS:*.cdn.shop.example
X509v3 Basic Constraints: critical
    CA:FALSE
X509v3 Key Usage: critical
    Digital Signature
```

Each certificate's issuer (`i:`) is the next one's subject (`s:`); the root is absent, as it should be. The leaf is not a CA, so it cannot sign other certificates, and it covers `shop.example` and any single label under `cdn.shop.example`.

Lifetime arithmetic: with a 100-day maximum and `notBefore` = 2027-06-01 00:00:00 UTC, the latest `notAfter` is 2027-09-08 23:59:59 UTC, because the period includes its last second.

## Where this shows up today

As of November 2026, CA/Browser Forum ballot SC-081v3 (April 2025) caps public TLS certificates at 200 days since 2026-03-15, falling to 100 days on 2027-03-15 and 47 days on 2029-03-15. Let's Encrypt still issues 90-day certificates by default, offers optional 6-day certificates, plans 64 days from 2027-02-10 and 45 days from 2028-02-16, and its certificates carry a CRL distribution point instead of an OCSP URL. Renewal must be automated (ACME, RFC 8555).

## Pitfalls checklist

- Configuring only the leaf certificate and testing only in a browser.
- Relying on the Common Name for host matching, or expecting `*.example.com` to cover `example.com` or two labels.
- Renewing certificates by hand when lifetimes are measured in weeks.
- Assuming revocation is enforced everywhere; many clients soft-fail.
- Pinning a specific intermediate and breaking when the CA rotates it.

## Key takeaways

- Each link: issuer's key verifies the signature, issuer name equals subject name, and the issuer is a CA.
- Servers send leaf plus intermediates; clients bring the root.
- Host names match SANs only; wildcards cover exactly one left-most label.
- Revocation is best-effort; short lifetimes and automation are the reliable control.

## Sources

- RFC 5280, Internet X.509 PKI Certificate and CRL Profile (§4.1.2.5 validity, §6 path validation).
- RFC 9525, Service Identity in TLS (§2, §6.3).
- RFC 6960 (OCSP); RFC 6066 (status_request, OCSP stapling); RFC 8555 (ACME).
- CA/Browser Forum, Ballot SC081v3 (2025); Let's Encrypt, "Upcoming Features" and "Certificate Lifetime Rationale and Plans", updated July 2026.
