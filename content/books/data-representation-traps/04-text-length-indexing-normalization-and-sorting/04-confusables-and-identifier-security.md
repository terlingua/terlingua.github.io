---
title: "Confusables and Identifier Security"
questions:
  - id: "cyrillic-lookalike-normalization"
    type: "single"
    prompt: "A username is `p`, U+0430 CYRILLIC SMALL LETTER A, `ypal`. Does applying NFKC and then case folding make it equal to the ASCII string `paypal`?"
    explanation: |-
      No. NFKC unifies canonical and compatibility equivalents, and case folding unifies case variants. U+0430 is neither: it is a distinct letter of a different script that merely looks like Latin `a` in most fonts. Its normalized, folded form is still U+0430, so the key stays different from `paypal` (and the string is 7 UTF-8 bytes, not 6, because U+0430 takes 2).

      Detecting such look-alikes is a separate job, done with UTS #39 confusable "skeletons" and mixed-script checks.

      "Yes, because NFKC maps homoglyphs to ASCII" overstates NFKC; it maps compatibility variants such as full-width letters, not other scripts.

      "Yes, because case folding maps Cyrillic letters to Latin" confuses folding (a case operation within a script) with transliteration.

      "Only if the string is first converted to NFD" is wrong: U+0430 has no decomposition in any form.
    options:
      - text: "Yes, because NFKC maps homoglyphs to ASCII."
      - text: "Yes, because case folding maps Cyrillic letters to Latin."
      - text: "No; U+0430 is a different letter, so the keys differ."
        correct: true
      - text: "Only if the string is first converted to NFD."
  - id: "fullwidth-admin-key"
    type: "single"
    prompt: "A sign-up form receives `ＡＤＭＩＮ` written in full-width letters (U+FF21 U+FF24 U+FF2D U+FF29 U+FF2E). What comparison key does NFKC followed by case folding produce?"
    explanation: |-
      Trace:

      1. NFKC: full-width Latin letters U+FF21–U+FF3A have compatibility decompositions `<wide>` to U+0041–U+005A, so the string becomes `ADMIN`.
      2. Case folding: `ADMIN` → `admin`.

      Key: `admin`. A system that reserves `admin` must compare against this key, or an attacker can register a visually similar full-width name.

      "`ＡＤＭＩＮ` unchanged" is what NFC does; only the compatibility forms touch full-width characters.

      "`ADMIN`" stops after NFKC and forgets the folding step.

      "`ａｄｍｉｎ`" applies folding without NFKC: the full-width letters have full-width lowercase forms, so the result is still not ASCII.
    options:
      - text: "`ＡＤＭＩＮ` unchanged"
      - text: "`ADMIN`"
      - text: "`ａｄｍｉｎ`"
      - text: "`admin`"
        correct: true
  - id: "unique-index-on-raw-names"
    type: "single"
    prompt: "A database has a unique index on the raw `username` column. The login code looks users up by `NFKC(casefold(input))`, but registration stores the input as typed. What goes wrong when one person registers `Admin` and another registers `ａｄｍｉｎ` (full-width)?"
    explanation: |-
      The unique index compares raw strings, and `Admin` and `ａｄｍｉｎ` differ, so both registrations succeed. Login normalizes the input to the key `admin`, but the stored rows were never normalized, so the lookup either finds neither row (if it compares the key to raw values) or, if the code normalizes stored values at query time, finds two rows for one key, and whichever row the query returns gets logged in. Either way, identity is ambiguous and one user may reach another's account.

      The fix is to compute the canonical key once, at registration, store it in its own column and put the unique constraint on that column.

      "Registration of the second name fails because the unique index sees a duplicate" assumes the index normalizes; database unique indexes compare stored values exactly (or by collation, which is not NFKC).

      "Nothing goes wrong, because login normalizes the input" ignores that uniqueness was never enforced on the normalized form.

      "The full-width name is rejected as invalid UTF-8" is wrong: full-width letters are ordinary scalar values with valid 3-byte encodings.
    options:
      - text: "Both registrations succeed, and two accounts share one login key."
        correct: true
      - text: "Registration of the second name fails because the unique index sees a duplicate."
      - text: "Nothing goes wrong, because login normalizes the input."
      - text: "The full-width name is rejected as invalid UTF-8."
  - id: "bidi-override-in-source"
    type: "single"
    prompt: "A pull request adds a comment containing U+202E RIGHT-TO-LEFT OVERRIDE. Why do code-review tools flag it?"
    explanation: |-
      Bidirectional controls such as U+202E (RLO), U+2066–U+2069 (isolates) and U+202A–U+202D change the *display* order of characters under the Unicode Bidirectional Algorithm (UAX #9) without changing their logical order. A compiler reads logical order. Placed in comments or string literals, they can make code look like it does one thing while it compiles to another; this was published in 2021 as "Trojan Source" (CVE-2021-42574).

      "It makes compilers crash when they parse the comment" is not the risk; compilers accept the characters as part of comments and strings.

      "It silently switches the file's encoding to UTF-16" is wrong: U+202E is an ordinary character encoded in the file's encoding (3 bytes in UTF-8).

      "It only affects how Arabic and Hebrew text is shown" understates it: the override reorders any characters, including ASCII code.
    options:
      - text: "It makes compilers crash when they parse the comment."
      - text: "Displayed order can differ from the order the compiler reads."
        correct: true
      - text: "It silently switches the file's encoding to UTF-16."
      - text: "It only affects how Arabic and Hebrew text is shown."
  - id: "idna-label-for-munchen"
    type: "single"
    prompt: "Internationalized domain names are carried in DNS as ASCII. What ASCII label does IDNA produce for `münchen`?"
    explanation: |-
      IDNA encodes each non-ASCII label with Punycode (RFC 3492) and adds the ACE prefix `xn--` (RFC 5890 and related IDNA2008 documents). Punycode copies the basic (ASCII) code points in order, `mnchen`, adds a hyphen delimiter, and then encodes the insertion of `ü` (U+00FC) and its position as base-36 digits, here `3ya`. Result: `xn--mnchen-3ya`.

      "`xn--münchen`" keeps a non-ASCII character, which defeats the purpose: DNS labels must be ASCII.

      "`munchen`" is a transliteration that loses information and is a different domain.

      "`mnchen-3ya`" is the Punycode string without the `xn--` prefix; without the prefix resolvers cannot tell it is encoded.

      Look-alike attacks use the same mechanism: a label such as `xn--pypal-4ve` decodes to a Cyrillic-containing look-alike of `paypal`, which is why browsers apply confusable checks before showing the Unicode form.
    options:
      - text: "`xn--münchen`"
      - text: "`munchen`"
      - text: "`xn--mnchen-3ya`"
        correct: true
      - text: "`mnchen-3ya`"
  - id: "confusable-detection-facts"
    type: "multiple"
    prompt: "Which statements about UTS #39 (Unicode Security Mechanisms) are correct? Select all that apply."
    explanation: |-
      Correct:

      - UTS #39 defines a *skeleton* transform that maps each character to a prototype using its confusables data; two strings are confusable when their skeletons are equal.
      - It also defines mixed-script detection and restriction levels; `p` + Cyrillic а + `ypal` mixes Latin and Cyrillic in one identifier and fails the stricter levels.
      - Skeletons are for detection only. UTS #39 says they are not intended for display or storage, because they may not even be valid text in any script.

      Wrong:

      - "NFKC alone removes all confusables" ignores cross-script look-alikes, which normalization deliberately preserves.
      - "Unicode security guidance requires rejecting every non-ASCII identifier" misstates it: UTS #39 and UAX #31 provide profiles for safe internationalized identifiers precisely so that non-ASCII names can be allowed.
    options:
      - text: "Two strings are confusable when their UTS #39 skeletons are equal."
        correct: true
      - text: "NFKC alone removes all confusables."
      - text: "Unicode security guidance requires rejecting every non-ASCII identifier."
      - text: "Mixed-script detection flags Latin mixed with Cyrillic in one identifier."
        correct: true
      - text: "Skeletons are meant for detection, not for storage or display."
        correct: true
---

## Learning objectives

- Explain why visually identical identifiers can be different strings, and why normalization does not fix that.
- Build a canonical comparison key for usernames and enforce uniqueness on it.
- Recognize bidirectional-control and look-alike attacks in code and domain names.
- Apply UTS #39 skeletons and mixed-script checks for confusable detection.

## The invariant

Identifiers (usernames, domain names, package names, variable names) are compared by machines but recognized by people. Unicode gives several distinct tools, each with a precise scope:

| Problem | Example | Tool |
|---|---|---|
| Same character, different encoding | é vs e + U+0301 | NFC (UAX #15) |
| Formatting variants | ＡＤＭＩＮ vs ADMIN | NFKC (UAX #15) |
| Case variants | Admin vs admin | case folding |
| Different characters that look alike | Latin a vs Cyrillic а | UTS #39 confusables, mixed-script checks |
| Characters that change display order | U+202E inside code | UAX #9 awareness, reject or flag |
| Which characters may appear at all | letters, digits, joiners | UAX #31 identifier syntax, RFC 8264/8265 (PRECIS) for usernames |

The first three are *equivalences*: you can define a canonical key, typically NFKC followed by case folding (Unicode publishes the combined `NFKC_Casefold` mapping), and two identifiers are "the same" exactly when their keys are equal. The fourth is a *similarity*: no key makes Cyrillic а equal to Latin a without destroying legitimate text, so the right response is detection — reject, warn or require review — based on skeletons and script mixing.

Two engineering rules follow:

1. **Compute the canonical key once, store it, and enforce uniqueness on it.** Normalizing at lookup time only, or in one service but not another, creates duplicate identities.
2. **Show users what the machine sees.** Display identifiers in a way that exposes confusables (punycode for suspicious domains, warnings for bidi controls), because people make the trust decision.

## Worked example

Key derivation for three sign-up attempts, with a reserved name `admin`:

| Input | After NFKC | After case folding | Decision |
|---|---|---|---|
| `Admin` | Admin | admin | reject: reserved |
| `ＡＤＭＩＮ` (full-width) | ADMIN | admin | reject: reserved |
| `аdmin` (Cyrillic а) | аdmin | аdmin | key differs; reject by mixed-script check |

```python
import unicodedata as ud

def username_key(raw: str) -> str:
    return ud.normalize("NFKC", ud.normalize("NFKC", raw).casefold())

full_width = "".join(chr(0xFF21 + ord(c) - ord("A")) for c in "ADMIN")
cyrillic = chr(0x430) + "dmin"
print(username_key("Admin"), username_key(full_width))   # admin admin
print(username_key(cyrillic) == "admin")                  # False
print("münchen".encode("idna"))                           # b'xn--mnchen-3ya'
```

```sql
CREATE TABLE users (
    id           BIGINT PRIMARY KEY,
    display_name VARCHAR(64) NOT NULL,
    name_key     VARCHAR(64) NOT NULL UNIQUE
);
```

The application writes `name_key` from `username_key`, and every lookup uses the key column.

## Where this shows up today

As of November 2026, major browsers apply confusable and mixed-script policies to internationalized domain names and fall back to displaying the `xn--` form when a label looks deceptive, and popular code-hosting and review tools warn about bidirectional control characters in source files, a practice that spread after the 2021 Trojan Source disclosure.

## Pitfalls checklist

- Enforcing uniqueness on raw input while comparing on normalized keys elsewhere.
- Believing NFKC or case folding defends against cross-script homoglyphs.
- Storing or displaying UTS #39 skeletons instead of using them only for detection.
- Allowing bidi controls and zero-width characters in identifiers, code and file names without review.
- Rendering internationalized domains in Unicode without any confusable policy.

## Key takeaways

- Normalization and case folding define equivalence; confusables are similarity and need detection.
- Use one canonical key (NFKC + case folding), stored and unique-indexed.
- Bidi controls change display order, not logical order; flag them in code.
- IDNA turns non-ASCII labels into `xn--` Punycode; look-alike domains exploit the Unicode display form.
