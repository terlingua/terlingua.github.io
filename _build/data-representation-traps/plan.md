# Plan — Data Representation Traps: Floating-Point, Unicode, and Time

Slug `data-representation-traps`, author `terlingua-team`, date 2026-11-27. 7 chapters, 28 sections (s = global section number used by the answer-position formula).

## 01-binary-floating-point-anatomy

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 1 | 01-three-fields-and-the-bias.md | Read binary32/binary64 as sign, biased exponent, fraction | binary32 field widths; binary64 field widths; bias formula 2^(k−1)−1; stored exponent of 32.0f; bit pattern of 1.0f; effect of XOR with the sign bit; why biased exponents make positive floats sort as integers |
| 2 | 02-hidden-bit-and-exact-decoding.md | Encode and decode normal numbers exactly | precision p = 24/53 from the hidden bit; encode −6.5f; decode 0x3E200000; exact value of 0x40490FDB; largest finite binary32; spot-the-bug exponent extraction mask/shift |
| 3 | 03-subnormals-and-gradual-underflow.md | Explain the zero-exponent encoding and why gradual underflow matters | smallest normal 2^−126; smallest subnormal 2^−149 and its pattern; effective exponent of subnormals is 1−bias; x−y==0 ⇔ x==y guarantee; what `5e-324 / 2` prints; flush-to-zero consequences; count of positive subnormals |
| 4 | 04-special-encodings-and-small-formats.md | Recognize inf/NaN/−0 encodings and the same layout in binary16, bfloat16, FP8 | inf vs NaN by fraction field; quiet-NaN bit; number of NaN patterns in binary32; binary16 layout and max 65504; bfloat16 as upper half of binary32; FP8 E4M3 vs E5M2 trade-off |

## 02-floating-point-arithmetic-and-traps

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 5 | 01-why-0-1-plus-0-2-is-not-0-3.md | Trace decimal→binary rounding and round-to-nearest-even | which decimals are exact (power-of-two denominators); exact stored value of 0.1; what `0.1 + 0.2 == 0.3` prints; ties-to-even on 2^53+1 and 2^53+3; shortest round-trip printing; hex of 0.1 |
| 6 | 02-ulps-and-the-2-to-the-53-limit.md | Compute ULP size and the exact-integer limit | ulp(x)=2^(e−52); ulp at 1e16; 2^53 limit and 9007199254740993; binary32 limit 2^24; loop `x += 1` stalls; ulp of Unix-seconds-as-double; (dated) JSON big IDs in JavaScript |
| 7 | 03-cancellation-and-non-associativity.md | Predict cancellation and order-dependent sums | (1e16 + 1) − 1e16; (a+b)+c ≠ a+(b+c) trace; relative error after cancellation; stable quadratic root rewrite; Kahan compensation trace; which strategies reduce summation error |
| 8 | 04-nan-signed-zero-and-infinity.md | Apply IEEE special-value rules in comparisons | NaN ≠ NaN; NaN-safe check; sort comparator broken by NaN; −0.0 == 0.0 but 1/−0.0; inf − inf; language-specific division by zero (Python raises vs C#/JS) |
| 9 | 05-tolerances-decimal-and-money.md | Choose tolerances, decimal types or integer minor units | relative vs absolute tolerance; `math.isclose(x, 0)` trap; C# `decimal` 0.1m+0.2m; allocation of 100.00 into 3 shares; minor-unit exponents (JPY 0, BHD 3); ties-to-even vs half-up on money |

## 03-code-points-utf-8-and-utf-16

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 10 | 01-code-points-scalar-values-and-code-units.md | Separate code points, scalar values, code units | size of codespace 1,114,112; scalar values 1,112,064; plane of U+1F600; U+D800 not a scalar value; `len` vs `.Length` of an emoji; which runtimes index by UTF-16 units |
| 11 | 02-utf-8-byte-patterns.md | Encode/decode UTF-8 by hand | ranges per length; encode U+20AC; decode F0 9F 92 A9; byte count of mixed string; self-synchronization (find start from middle); spot-the-bug lead-byte test |
| 12 | 03-invalid-and-overlong-utf-8.md | Reject ill-formed UTF-8 and handle BOMs | overlong C0 AF; bytes never valid (C0, C1, F5–FF); encoded surrogate ED A0 80; RFC 3629 max 4 bytes; BOM EF BB BF decoded as U+FEFF; truncation mid-sequence → U+FFFD count |
| 13 | 04-utf-16-surrogate-pairs.md | Compute surrogate pairs and their failure modes | encode U+1F600 → D83D DE00; decode D83C DF89; lone surrogate from substring; UTF-16 vs UTF-8 byte counts; BOM bytes for UTF-16LE; (dated) MySQL utf8mb3 rejection of astral |

## 04-text-length-indexing-normalization-and-sorting

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 14 | 01-four-lengths-of-a-string.md | Compute the four lengths and truncate by grapheme | lengths of "e"+U+0301; thumbs-up+skin tone; flag regional indicators; ZWJ family; which API counts grapheme clusters; truncation splitting a cluster |
| 15 | 02-normalization-forms.md | Apply NFC/NFD/NFKC/NFKD correctly | canonical vs compatibility equivalence; NFC of e+U+0301; NFKC of U+FB01; ANGSTROM SIGN → U+00C5; byte length change after NFD; concatenation not closed |
| 16 | 03-case-mapping-and-collation.md | Avoid culture and case-mapping traps | Turkish dotless i; `"ß".upper()` length; casefold vs lower; ordinal sort of "Zebra","apple","Éclair"; locale-dependent order (Swedish ä); culture-sensitive vs ordinal API choice |
| 17 | 04-confusables-and-identifier-security.md | Compare identifiers safely | Cyrillic а vs Latin a; NFKC + casefold for usernames; unique index on raw vs normalized; bidi override (Trojan Source); punycode `xn--`; skeleton check per UTS #39 |

## 05-calendars-epochs-and-clocks

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 18 | 01-unix-time-and-2038.md | Compute epoch limits and overflow results | 2^31−1 → 2038-01-19T03:14:07Z; wrap to 1901-12-13T20:45:52Z; unsigned 32-bit → 2106; ms timestamps in int32; POSIX day = 86400 s; (dated) MySQL TIMESTAMP range |
| 19 | 02-gregorian-leap-years.md | Apply the 4/100/400 rule | 1900/2000/2100; buggy `% 4` only; days in 400 years 146097; leap years 2001–2100 = 24; average year 365.2425; Feb 29 + 1 year |
| 20 | 03-utc-tai-gps-and-leap-seconds.md | Convert between time scales | TAI−UTC = 37 s; GPS−UTC = 18 s; GPS epoch; 23:59:60 parsing; smear rate ppm; (dated) CGPM 2026 continuous UTC |
| 21 | 04-wall-clock-versus-monotonic.md | Measure durations correctly | negative elapsed with wall clock; which APIs are monotonic; slew duration at 500 ppm; step vs slew; event ordering across machines; monotonic clock meaningless across processes/reboots |

## 06-time-zones-dst-and-serialization

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 22 | 01-offsets-versus-zones.md | Distinguish offsets, zone IDs and abbreviations | `Etc/GMT+5` = UTC−05:00; ambiguity of "CST"; offset is not a zone; converting 2026-07-01T12:00−04:00 to UTC; non-hour offsets (+05:45); offset range −12..+14 |
| 23 | 02-dst-gaps-and-overlaps.md | Handle nonexistent and ambiguous local times | 2026-03-08 02:30 New York nonexistent; 2026-11-01 01:30 ambiguous; length of local day = 23 h; add 1 day vs 24 h; Python `fold`; scheduling daily jobs at 02:30 |
| 24 | 03-storing-past-and-future-times.md | Pick the right representation for past and future events | past = UTC instant; future civil = local + zone ID; stored offset goes stale; recurring meeting across DST; (dated) tzdata delivery in containers; recomputing after rule changes |
| 25 | 04-iso-8601-rfc-3339-and-database-types.md | Serialize timestamps unambiguously | RFC 3339 requires offset; `-00:00` meaning; JS date-only parsed as UTC vs date-time local; naive vs aware Python; PostgreSQL `timestamptz` stores an instant not a zone; (dated) Temporal Stage 4 |

## 07-cross-chapter-review-and-diagnostics

| s | File | Learning goal | Question concepts (each combines ≥2 chapters) |
|---|------|---------------|-------------------|
| 26 | 01-numbers-on-the-wire.md | Diagnose numeric bugs that cross serialization boundaries | JSON id round trip; float seconds timestamps; money summed as double then rounded; NaN in JSON; binary32 GPU coordinates; float32 epoch seconds |
| 27 | 02-text-in-fixed-size-boxes.md | Diagnose truncation and storage limits | byte-truncation mid UTF-8; UTF-16 `Substring` on emoji; `VARCHAR(N)` byte vs character semantics; utf8mb3 and astral; normalization before uniqueness; BOM in CSV header |
| 28 | 03-clocks-calendars-and-zones-in-incidents.md | Diagnose time incidents end to end | timeout measured with wall clock during NTP step; DST double-run of cron; 2038 in int32 expiry; leap day yearly renewal; offset stored for future meeting; float seconds since epoch precision |

No concept is reused across chapters 1–6; chapter 7 deliberately combines them.
