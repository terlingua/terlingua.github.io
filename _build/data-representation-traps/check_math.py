import warnings; warnings.filterwarnings("ignore")
import struct, math, unicodedata, datetime, calendar
from decimal import Decimal
from fractions import Fraction

checks = 0
def ok(cond, label):
    global checks
    checks += 1
    if not cond:
        raise AssertionError(label)

def f32_bits(x): return struct.unpack(">I", struct.pack(">f", x))[0]
def f64_bits(x): return struct.unpack(">Q", struct.pack(">d", x))[0]
def bits_f32(b): return struct.unpack(">f", struct.pack(">I", b))[0]
def bits_f64(b): return struct.unpack(">d", struct.pack(">Q", b))[0]
def s32(u): return u - (1 << 32) if u & 0x80000000 else u

# s=1 three fields and the bias
ok(2**(8-1)-1 == 127 and 2**(11-1)-1 == 1023, "biases")
ok(f32_bits(32.0) == 0x42000000 and (0x42000000 >> 23) & 0xFF == 132, "32.0f")
ok(f32_bits(1.0) == 0x3F800000, "1.0f")
ok(f64_bits(1.0) == 0x3FF0000000000000, "1.0")
ok(f64_bits(2.0) == 0x4000000000000000, "2.0")
ok(f64_bits(32.0) == 0x4040000000000000 and (0x4040000000000000 >> 52) == 1028, "32.0 double")
ok(f32_bits(2.5) == 0x40200000 and bits_f32(0x40200000 ^ 0x80000000) == -2.5, "xor sign")
ok(s32(0xC0200000) == -1071644672, "reinterpreted int")
ok(s32(f32_bits(-1.0)) == -1082130432 and s32(f32_bits(-2.0)) == -1073741824, "neg sort")
ok(f32_bits(-2.5) == 0xC0200000 and f64_bits(-2.5) == 0xC004000000000000, "-2.5")
ok(bits_f64(1) == 2**-1074 and str(Decimal(2**-1074))[:4] == "4.94", "min subnormal double")

# s=2 hidden bit
ok(f32_bits(-6.5) == 0xC0D00000 and bits_f32(0xC0E80000) == -7.25 and bits_f32(0xC0500000) == -3.25, "-6.5")
ok(bits_f32(0x3E200000) == 0.15625 and bits_f32(0x3EA00000) == 0.3125, "0x3E200000")
ok(Decimal(bits_f32(0x40490FDB)) == Decimal("3.1415927410125732421875"), "float pi exact")
ok(Fraction(bits_f32(0x40490FDB)) == Fraction(13176795, 4194304), "float pi fraction")
ok(abs((bits_f32(0x40490FDB) - math.pi) - 8.74e-8) < 1e-9, "float pi error")
ok(repr(bits_f32(0x40490FDB)) == "3.1415927410125732", "repr widened")
ok(((0x40000000 >> 23) & 0x7F) - 127 == -127 and ((0x3F800000 >> 23) & 0x7F) - 127 == 0, "mask bug")
ok(bits_f32(0x7F7FFFFF) == (2 - 2**-23) * 2**127 == 2**128 - 2**104, "max float")
ok(bits_f32(0x7F800000) == math.inf, "inf")
ok(round(53 * math.log10(2), 2) == 15.95, "digits")

# s=3 subnormals
import numpy as np
ok(bits_f32(0x00800000) == 2.0**-126 and bits_f32(0x00000001) == 2.0**-149, "min normal/subnormal f32")
ok(bits_f32(0x00200000) == 2.0**-128, "2^-128 pattern")
ok(bits_f32(0x00000003) == 3 * 2.0**-149, "0x3 subnormal")
ok(5e-324 == 2.0**-1074 and 5e-324 / 2 == 0.0 and 5e-324 * 0.75 == 5e-324, "underflow print")
a32, b32 = np.float32(1.5e-38), np.float32(1.2e-38)
ok(a32 >= np.float32(2.0**-126) and b32 >= np.float32(2.0**-126), "inputs normal")
d32 = a32 - b32
ok(0 < d32 < np.float32(2.0**-126), "difference subnormal")
ok(abs(float(np.float32(1) / d32) - 3.33e38) / 3.33e38 < 0.01 and np.isfinite(np.float32(1) / d32), "reciprocal finite")
ok(2.0**-1022 == 2.2250738585072014e-308, "min normal f64")
ok(np.finfo(np.float32).smallest_subnormal == np.float32(2.0**-149), "float.Epsilon")

# s=4 special encodings
ok(math.isinf(bits_f32(0xFF800000)) and bits_f32(0xFF800000) < 0, "-inf")
ok(bits_f32(0xFF7FFFFF) == -bits_f32(0x7F7FFFFF), "most negative finite")
b = 0x7FA00000
ok((b >> 23) & 0xFF == 0xFF and b & 0x7FFFFF != 0 and not (b >> 22) & 1, "signaling NaN")
ok((0x7FC00000 >> 22) & 1 == 1, "quiet NaN bit")
ok(2 * (2**23 - 1) == 2**24 - 2 == 16777214, "NaN count")
ok((2 - 2**-10) * 2**15 == 65504 and float(np.frombuffer(bytes.fromhex("7bff")[::-1], dtype=np.float16)[0]) == 65504, "binary16 max")
ok(np.isinf(np.frombuffer(bytes.fromhex("7c00")[::-1], dtype=np.float16)[0]), "binary16 inf")
ok(np.isinf(np.float16(65520.0)) and float(np.float16(65504.0)) == 65504.0, "65520 overflows")
ok(bits_f32(0x40490000) == 3.140625 and 0x0FDB < 0x8000, "bfloat16 pi")
ok(bits_f32(0x40480000) == 3.125, "3.125 distractor")
ok((2 - 2**-7) * 2**127 > 3.38e38 and (2 - 2**-7) * 2**127 < 3.40e38, "bfloat16 max")
ok(1.75 * 2**15 == 57344 and 1.75 * 2**8 == 448 and 1.875 * 2**7 == 240, "fp8 maxima")

# s=5 0.1 + 0.2
for v in (0.375, 0.5, 2.25):
    ok(Fraction(v) == Fraction(str(v)), f"exact {v}")
for v in (0.1, 0.3):
    ok(Fraction(v) != Fraction(str(v)), f"inexact {v}")
ok(Fraction(0.1) > Fraction(1, 10), "0.1 rounds up")
ok(f64_bits(0.1) == 0x3FB999999999999A and f64_bits(0.2) == 0x3FC999999999999A, "0.1/0.2 hex")
ok(f64_bits(0.3) == 0x3FD3333333333333 and f64_bits(0.1 + 0.2) == 0x3FD3333333333334, "0.3 hex")
s_exact = Fraction(0.1) + Fraction(0.2)
ok(s_exact - Fraction(0.3) == Fraction(0.1 + 0.2) - s_exact == Fraction(1, 2**55), "tie")
ok(repr(0.1 + 0.2) == "0.30000000000000004" and (0.1 + 0.2 == 0.3) is False, "what prints")
ok(float(np.float32(0.1)) != 0.1 and Decimal(float(np.float32(0.1))) == Decimal("0.100000001490116119384765625"), "0.1f")
ok(f32_bits(0.1) == 0x3DCCCCCD, "0.1f hex")

# s=6 ULPs
ok(math.ulp(1e16) == 2.0 and 2**53 < 1e16 < 2**54, "ulp 1e16")
ok(float(2**53) == 2**53 and float(2**53 + 1) == 2**53 and float(2**52 + 1) == 2**52 + 1, "first unrepresentable")
ok(int(float(2**53 + 3)) == 2**53 + 4 and float(2**64) == 2**64, "2^53+3 and 2^64")
ok(np.float32(16777216) + np.float32(1) == np.float32(16777216), "float counter stall")
ok(math.ulp(1.76e9) == 2**-22 and 2**30 < 1.76e9 < 2**31 and round(2**-22 * 1e9) == 238, "ulp unix seconds")
ok(float(np.spacing(np.float32(1.76e9))) == 128.0 and 2**-20 * 1e6 < 0.96, "binary32 unix seconds")
ok(math.ulp(2.0**24) == 2**-28 and float(np.spacing(np.float32(2.0**53))) == 2.0**30, "table")
for x in (1.0, 1.5, 1.999999, 3.7e10, 9.9e300):
    ok(2**-53 <= math.ulp(x) / x <= 2**-52, "relative spacing")
ok(float(9007199254740993) == 9007199254740992.0, "json id")

# s=7 cancellation
ok(repr((0.1 + 0.2) + 0.3) == "0.6000000000000001" and repr(0.1 + (0.2 + 0.3)) == "0.6" and 0.2 + 0.3 == 0.5, "assoc")
ok((1e16 + 1.0) - 1e16 == 0.0 and (10**16 // 2) % 2 == 0, "absorb")
ok(abs((2 * 1000 * 1e-8) / 1e-4 - 0.2) < 1e-12, "cancellation 20%")
_a, _b, _c = 1.0, 1e8, 1.0
_d = math.sqrt(_b * _b - 4 * _a * _c)
ok(repr(_d) == "99999999.99999999" and (-_b + _d) / (2 * _a) == -7.450580596923828e-09 and 2 * _c / (-_b - _d) == -1e-08, "quadratic")
ok(-_b + _d == -2**-26 and -_b - _d == -2e8, "quadratic numerators")
def _kahan(xs):
    s = c = 0.0
    for x in xs:
        y = x - c; t = s + y; c = (t - s) - y; s = t
    return s
_n = 0.0
for _x in [1e16, 1.0, 1.0]:
    _n += _x
ok(_n == 1e16 and repr(_kahan([1e16, 1.0, 1.0])) == "1.0000000000000002e+16", "kahan")
ok(math.fsum([1e16, 1.0, 1.0]) == 1e16 + 2, "fsum")

# s=8 NaN / zero / inf
_nan = float("nan")
ok(sorted([3.0, _nan, 1.0, 2.0])[0] == 3.0 and sorted([3.0, _nan, 1.0, 2.0])[2:] == [1.0, 2.0], "sort nan")
ok(math.isnan(max(_nan, 1.0)) and max(1.0, _nan) == 1.0, "max nan")
ok(math.isnan(math.inf - math.inf) and math.copysign(1.0, -0.0) == -1.0 and 0.0 == -0.0, "inf/zero")
ok({_nan: 1}.get(float("nan")) is None, "dict nan")
ok(json.dumps(_nan) == "NaN" if (json := __import__("json")) else False, "json nan")
try:
    1.0 / 0.0
    ok(False, "py zero div")
except ZeroDivisionError:
    ok(True, "py zero div")
ok(np.float64(-1.0) / np.float64(np.inf) == 0 and math.copysign(1, np.float64(-1.0) / np.float64(np.inf)) == -1, "-1/inf")

# s=9 tolerances and money
from decimal import ROUND_HALF_UP, getcontext
ok(math.isclose(0.1 + 0.2, 0.3) and not math.isclose(1e-12, 0.0) and math.isclose(1e-12, 0.0, abs_tol=1e-9), "isclose")
ok(math.ulp(1e9) == 2**-23 and math.ulp(1e9) > 1e-9 and 2**29 < 1e9 < 2**30, "ulp 1e9")
ok(Decimal("0.1") + Decimal("0.2") == Decimal("0.3") and str(Decimal(1) / Decimal(3) * 3) == "0.9999999999999999999999999999", "decimal")
ok(divmod(10000, 3) == (3333, 1) and 3334 + 3333 + 3333 == 10000 and 3 * 3333 == 9999 and 3 * 3334 == 10002, "allocation")
ok(round(2.675, 2) == 2.67 and str(round(Decimal("2.675"), 2)) == "2.68", "round 2.675")
ok(str(Decimal(2.675)) == "2.67499999999999982236431605997495353221893310546875", "2.675 exact")
ok(str(Decimal("2.675").quantize(Decimal("0.01"), rounding=ROUND_HALF_UP)) == "2.68", "half up")
ok(round(2.5) == 2, "py half even")

# s=10 code points
ok(0x10FFFF + 1 == 1114112 == 17 * 65536 and 1114112 - 2048 == 1112064 and 2**21 == 2097152, "codespace")
ok(0x1F600 >> 16 == 1 and len("😀") == 1 and len("😀".encode("utf-16-le")) // 2 == 2 and len("😀".encode()) == 4, "plane/len")
try:
    "\ud83d".encode("utf-8"); ok(False, "lone surrogate")
except UnicodeEncodeError:
    ok(True, "lone surrogate")
ok("\ud83d".encode("utf-8", "surrogatepass") == b"\xed\xa0\xbd" and "\ud83d".encode("utf-8", "replace") == b"?", "surrogatepass")
_s = "a€😀"
ok((len(_s), len(_s.encode()), len(_s.encode("utf-16-le")) // 2) == (3, 8, 4), "a€😀")
ok(65536 + 2**20 == 1114112, "utf16 reach")

import subprocess
JS = []
def js(code, expected, label):
    JS.append((code, expected, label))
js('const x=NaN; console.log(x === x, x !== x, [NaN].includes(NaN), [NaN].indexOf(NaN))', "false true true -1", "nan js")
js('console.log(0 === -0, 1 / -0, Object.is(0, -0))', "true -Infinity false", "zero js")
js('console.log(0.1 + 0.2, 0.1 + 0.2 === 0.3); console.log((0.1).toString(2))', "0.30000000000000004 false\n0.0001100110011001100110011001100110011001100110011001101", "0.1 js")
js('console.log(JSON.parse(\'{"id": 9007199254740993}\').id)', "9007199254740992", "json id js")
js('const s = "a€😀"; console.log(s.length, [...s].length)', "4 3", "length js")
js('console.log(JSON.stringify([NaN, Infinity]))', "[null,null]", "json nan js")

# s=11 UTF-8 patterns
ok(chr(0x800).encode() == b"\xe0\xa0\x80" and chr(0x7FF).encode() == b"\xdf\xbf", "u0800")
ok("€".encode() == b"\xe2\x82\xac" and bytes.fromhex("c282").decode() == "\x82", "euro")
ok(ord(bytes.fromhex("f09f92a9").decode()) == 0x1F4A9 and "\U0001F4A9".encode("utf-16-be") == bytes.fromhex("d83ddca9"), "poo")
_g = "Grüße, 東京 👋"
ok(len(_g) == 11 and len(_g.encode()) == 20 and len(_g.encode("utf-16-le")) == 24, "byte count")
ok(sum(1 for b in "héllo".encode() if (b & 0x80) == 0) == 4 and sum(1 for b in "héllo".encode() if (b & 0xC0) != 0x80) == 5, "count bug")
ok("é".encode() == b"\xc3\xa9" and "😀".encode() == bytes.fromhex("f09f9880"), "examples")
_words = ["z", "é", "€", "😀", "a", "\x7f", "߿", "￿", "\U00010000"]
ok(sorted(_words) == sorted(_words, key=lambda w: w.encode()), "byte order = code point order")

# s=12 invalid UTF-8
def _bad(b):
    try:
        b.decode("utf-8"); return False
    except UnicodeDecodeError:
        return True
for _b in (0xC0, 0xC1, 0xF5, 0xFF):
    ok(all(_bad(bytes([_b]) + bytes([c]) + b"\x80\x80") for c in range(0x80, 0xC0)), f"never valid {_b:x}")
ok(bytes.fromhex("f09f9880").decode() == "😀" and bytes.fromhex("ed9fbf").decode() == "퟿", "0x80 / 0xED valid")
ok(((0xC0 & 0x1F) << 6 | (0xAF & 0x3F)) == 0x2F and b"\xc0\xaf".decode("utf-8", "replace") == "��", "overlong slash")
ok(((0xED & 0x0F) << 12 | (0xA0 & 0x3F) << 6 | (0x80 & 0x3F)) == 0xD800 and _bad(b"\xed\xa0\x80"), "encoded surrogate")
ok(((0xF8 & 0x07) << 24 | 0x08 << 18) == 0x200000 and _bad(b"\xf8\x88\x80\x80\x80"), "five byte")
ok(len(b"\xef\xbb\xbfid".decode("utf-8")) == 3 and len(b"\xef\xbb\xbfid".decode("utf-8-sig")) == 2, "bom")
ok(b"a\xe2\x82".decode("utf-8", "replace") == "a�" and b"\xf4\x90\x80\x80".decode("utf-8", "replace") == "�" * 4, "replacement counts")
js('console.log(JSON.stringify(new TextDecoder().decode(new Uint8Array([0x61,0xe2,0x82]))), new TextDecoder().decode(new Uint8Array([0xef,0xbb,0xbf,0x69,0x64])).length)', '"a�" 2', "textdecoder")

# s=13 UTF-16
ok("😀".encode("utf-16-be") == bytes.fromhex("d83dde00") and 0xD800 + (0x1F600 >> 10) == 0xD87D, "encode pair")
ok(((0xD83C - 0xD800) << 10 | (0xDF89 - 0xDC00)) + 0x10000 == 0x1F389 and bytes.fromhex("d83cdf89").decode("utf-16-be") == "\U0001F389", "decode pair")
_doc = "一" * 1000 + "😀" * 1000
ok(len(_doc.encode()) == 7000 and len(_doc.encode("utf-16-le")) == 6000, "sizes")
ok("A".encode("utf-16-le") == b"A\x00" and "A".encode("utf-16") == b"\xff\xfeA\x00", "bom le")
ok(bytes.fromhex("4100").decode("utf-16-be") == "䄀", "mixed endian")
ok(1024 * 1024 == 2**20 == 0x10FFFF - 0xFFFF, "pairs = supplementary")
js('try { encodeURIComponent("😀abc".slice(0, 1)) } catch (e) { console.log(e.name + ": " + e.message) }', "URIError: URI malformed", "lone surrogate js")

# s=14 four lengths
def _lens(s):
    return (len(s.encode()), len(s.encode("utf-16-le")) // 2, len(s))
_fam = "\U0001F468‍\U0001F469‍\U0001F467"
ok(_lens(_fam) == (18, 8, 5), "family")
ok(_lens("\U0001F44D\U0001F3FD") == (8, 4, 2) and _lens("é") == (3, 2, 2) and _lens("\U0001F1FA\U0001F1F8") == (8, 4, 2), "others")
_c = "cafe" + chr(0x301)
ok(_c[:4] == "cafe" and len(_c) == 5, "truncate")
js('const seg=new Intl.Segmenter("en",{granularity:"grapheme"}); const c=s=>[...seg.segment(s)].length; const fam=String.fromCodePoint(0x1F468,0x200D,0x1F469,0x200D,0x1F467); console.log(new TextEncoder().encode(fam).length, fam.length, [...fam].length, c(fam), "\\u{1F44D}\\u{1F3FD}".length, c(String.fromCodePoint(0x1F1FA,0x1F1F8,0x1F1E6)), c("e\\u0301"))', "18 8 5 1 4 2 1", "graphemes js")

# s=15 normalization
import unicodedata as ud
ok(ud.normalize("NFC", "é") == "é" and len("é".encode()) == 2 and len("é".encode()) == 3, "nfc e")
ok(ud.normalize("NFC", "Å") == "Å" and ud.normalize("NFC", "Ω") == "Ω", "singletons")
ok(ud.normalize("NFC", "ﬁ") == "ﬁ" and ud.normalize("NFKC", "ﬁle") == "file" and ud.normalize("NFKC", "①") == "1" and ud.normalize("NFC", "①") == "①", "compat")
ok(ud.normalize("NFD", "한") == "한" and len(ud.normalize("NFD", "한").encode()) == 9, "hangul")
ok(ud.normalize("NFC", "e") + ud.normalize("NFC", "́") != ud.normalize("NFC", "é"), "concat")
ok(ud.normalize("NFD", ud.normalize("NFC", "Ω")) == "Ω" and ud.normalize("NFKC", "x²") == "x2", "stability examples")
ok(ud.normalize("NFD", "Å") == "Å" and ud.normalize("NFD", "é") == "é", "table")
ok(ud.normalize("NFC", "q́") == "q́", "q acute stays")

# s=16 case mapping and collation
ok("straße".upper() == "STRASSE" and len("straße".upper()) == 7, "sharp s upper")
ok("Straße".lower() != "strasse" and "Straße".casefold() == "strasse", "casefold")
ok(sorted(["Zebra", "apple", "Éclair"]) == ["Zebra", "apple", "Éclair"] and sorted(["zebra", "äpple", "apple"]) == ["apple", "zebra", "äpple"], "ordinal sort")
ok("ŉ".upper() == "ʼN", "n apostrophe")
js('for (const l of ["sv","de"]) console.log(["zebra","äpple","apple"].sort(new Intl.Collator(l).compare).join(",")); console.log(["Zebra","apple","Éclair"].sort(new Intl.Collator("en").compare).join(","), "file".toLocaleUpperCase("tr-TR"), "I".toLocaleLowerCase("tr"), "ß".toUpperCase())', "apple,zebra,äpple\napple,äpple,zebra\napple,Éclair,Zebra FİLE ı SS", "collation js")

# s=17 confusables
def _key(raw):
    return ud.normalize("NFKC", ud.normalize("NFKC", raw).casefold())
_fw = "".join(chr(0xFF21 + ord(c) - 65) for c in "ADMIN")
ok(_key("Admin") == "admin" == _key(_fw) and ud.normalize("NFKC", _fw) == "ADMIN" and _fw.casefold() == "ａｄｍｉｎ", "fullwidth admin")
ok(_key("pаypal") != "paypal" and len("pаypal".encode()) == 7, "cyrillic")
ok("münchen".encode("idna") == b"xn--mnchen-3ya" and b"xn--pypal-4ve".decode("idna") == "pаypal", "idna")
ok(len("‮".encode()) == 3 and ud.name("‮") == "RIGHT-TO-LEFT OVERRIDE", "rlo")

# s=18 Unix time
from datetime import datetime as _dt, timezone as _tz, date as _date, timedelta as _td
_f = lambda t: _dt.fromtimestamp(t, _tz.utc).strftime("%Y-%m-%dT%H:%M:%SZ")
ok(_f(2**31 - 1) == "2038-01-19T03:14:07Z" and _f(-2**31) == "1901-12-13T20:45:52Z" and _f(2**32 - 1) == "2106-02-07T06:28:15Z", "epoch limits")
ok(divmod(2**31 - 1, 86400) == (24855, 11647) and 68 * 365 + 17 == 24837 and 24855 - 24837 == 18, "2038 arithmetic")
ok(24856 * 86400 - 2**31 == 74752 and _date(1970, 1, 1) - _td(days=24856) == _date(1901, 12, 13), "wrap arithmetic")
ok(calendar.timegm((2026, 11, 27, 0, 0, 0)) == 1795737600 == 20784 * 86400 and 56 * 365 + 14 + 304 + 26 == 20784, "unix time of date")
ok(1795737600 + 5 * 3600 == 1795755600 and 1795737600 - 365 * 86400 == calendar.timegm((2025, 11, 27, 0, 0, 0)), "distractors")
ok(round((2**31 - 1) / 1000 / 86400, 2) == 24.86 and round((2**32 - 1) / 1000 / 86400, 2) == 49.71, "ms overflow")
ok(int((2**63 - 1) / (365.2425 * 86400) / 1e9) == 292, "64-bit years")

# s=19 leap years
_L = lambda y: y % 4 == 0 and (y % 100 != 0 or y % 400 == 0)
ok([y for y in (1900, 2000, 2024, 2100, 2400) if _L(y)] == [2000, 2024, 2400] and all(_L(y) == calendar.isleap(y) for y in range(1, 5000)), "leap rule")
ok([y for y in (1900, 2000, 2024, 2100) if (y % 4 == 0) != _L(y)] == [1900, 2100], "mod four bug")
ok(sum(366 if _L(y) else 365 for y in range(2001, 2401)) == 146097 == 400 * 365 + 97 and 146097 % 7 == 0 and 146097 // 7 == 20871, "400 years")
ok(sum(_L(y) for y in range(2001, 2101)) == 24 == calendar.leapdays(2001, 2101) and (2100 - 2004) // 4 + 1 == 25, "leap count")
ok(146097 / 400 == 365.2425 and 400 * 365 + 100 == 146100 and 400 * 365 + 96 == 146096, "mean year")
ok((_date(2100, 3, 1) - _date(2000, 3, 1)).days == 36524, "century days")
try:
    _date(2024, 2, 29).replace(year=2025); ok(False, "feb29")
except ValueError:
    ok(True, "feb29")

# s=20 time scales
ok(10 + 27 == 37 and 37 - 19 == 18, "tai/gps offsets")
_u = _dt(2026, 11, 27, 12, 0, 0, tzinfo=_tz.utc)
ok((_u + _td(seconds=37)).strftime("%H:%M:%S") == "12:00:37" and (_u + _td(seconds=18)).strftime("%H:%M:%S") == "12:00:18" and (_u - _td(seconds=37)).strftime("%H:%M:%S") == "11:59:23", "tai convert")
ok(round(1 / 86400 * 1e6, 1) == 11.6 and round(1e6 / 86400, 1) == 11.6 and round(1e7 / 86400, 0) == 116, "smear ppm")
try:
    _dt.fromisoformat("2016-12-31T23:59:60+00:00"); ok(False, "leap parse")
except ValueError:
    ok(True, "leap parse")
ok(calendar.timegm((2017, 1, 1, 0, 0, 0)) == 1483228800, "unix 2017")
js('console.log(new Date("2016-12-31T23:59:60Z").getTime())', "NaN", "js leap parse")

# s=21 clocks
ok(0.5 - 2 == -1.5 and abs(1 / 500e-6 - 2000) < 1e-9 and abs(1 / 0.002 - 500) < 1e-9, "step/slew")
ok(abs(1 / (1 / 86400) - 86400) < 1e-6, "smear 86400")
_true_x = -0.010; _stamp_x = _true_x + 0.050; _true_y = 0.000; _stamp_y = _true_y
ok(abs(_stamp_x - 0.040) < 1e-12 and _stamp_y < _stamp_x and _true_x < _true_y, "skew ordering")

# s=22 offsets and zones
from zoneinfo import ZoneInfo as _Z
_i = _dt(2026, 1, 1, 9, 0, tzinfo=_tz.utc)
_loc = lambda z: _i.astimezone(_Z(z)).strftime("%H:%M")
ok(_loc("America/New_York") == "04:00" and _loc("Asia/Kolkata") == "14:30" and _loc("Asia/Kathmandu") == "14:45" and _loc("Pacific/Chatham") == "22:45" and _loc("Etc/GMT+5") == "04:00", "local times")
ok(_dt.fromisoformat("2026-07-01T12:00:00-04:00").astimezone(_tz.utc) == _dt(2026, 7, 1, 16, tzinfo=_tz.utc), "offset to utc")
ok(_dt(2026, 7, 1, 12, tzinfo=_Z("America/New_York")).utcoffset() == _td(hours=-4) and _dt(2026, 1, 15, tzinfo=_Z("America/New_York")).utcoffset() == _td(hours=-5), "ny dst")
ok(_dt(2026, 7, 1, tzinfo=_Z("Pacific/Kiritimati")).utcoffset() == _td(hours=14) and _dt(2026, 7, 1, tzinfo=_Z("Etc/GMT+12")).utcoffset() == _td(hours=-12), "offset range")
ok(_dt(2026, 7, 1, tzinfo=_Z("Pacific/Chatham")).utcoffset() == _td(hours=12, minutes=45), "chatham standard")

# s=23 DST
_ny = _Z("America/New_York")
_U = lambda d: d.astimezone(_tz.utc)
ok(_U(_dt(2026, 3, 9, tzinfo=_ny)) - _U(_dt(2026, 3, 8, tzinfo=_ny)) == _td(hours=23) and _U(_dt(2026, 11, 2, tzinfo=_ny)) - _U(_dt(2026, 11, 1, tzinfo=_ny)) == _td(hours=25), "day lengths")
_d0 = _dt(2026, 11, 1, 1, 30, tzinfo=_ny); _d1 = _d0.replace(fold=1)
ok(_d0.strftime("%z") == "-0400" and _d1.strftime("%z") == "-0500" and _U(_d0).strftime("%H:%M") == "05:30" and _U(_d1).strftime("%H:%M") == "06:30", "fold")
_g = _dt(2026, 3, 8, 2, 30, tzinfo=_ny)
ok(_U(_g).astimezone(_ny).strftime("%H:%M%z") == "03:30-0400" and _U(_g).strftime("%H:%M") == "07:30", "gap normalization")
_s = _dt(2026, 3, 7, 12, tzinfo=_ny)
ok((_s + _td(hours=24)).hour == 12 and (_U(_s) + _td(hours=24)).astimezone(_ny).hour == 13, "aware arithmetic")
ok(_U(_dt(2026, 3, 8, 1, 30, tzinfo=_ny)).strftime("%H:%M") == "06:30" and _U(_dt(2026, 11, 1, 0, 30, tzinfo=_ny)).strftime("%H:%M") == "04:30" and _U(_dt(2026, 11, 1, 2, 30, tzinfo=_ny)).strftime("%H:%M") == "07:30", "table")
_lon = _Z("Europe/Berlin")
ok(_dt(2026, 3, 29, 12, tzinfo=_lon).utcoffset() == _td(hours=2) and _dt(2026, 3, 28, 12, tzinfo=_lon).utcoffset() == _td(hours=1) and _dt(2026, 10, 25, 12, tzinfo=_lon).utcoffset() == _td(hours=1) and _dt(2026, 10, 24, 12, tzinfo=_lon).utcoffset() == _td(hours=2), "eu 2026 dates")

# s=24 storing times
ok((_dt(2026, 1, 5, 14, tzinfo=_tz.utc).astimezone(_ny).hour, _dt(2026, 7, 6, 14, tzinfo=_tz.utc).astimezone(_ny).hour) == (9, 10), "recurring standup")
ok((_dt(2027, 7, 6, 8, tzinfo=_tz.utc) + _td(hours=1)).hour == 9 and (_dt(2027, 7, 6, 10) - _td(hours=2)).hour == 8, "rule change arithmetic")
ok(_dt(2027, 7, 6, 10, tzinfo=_ny).astimezone(_tz.utc).isoformat() == "2027-07-06T14:00:00+00:00", "appointment example")

# s=25 serialization
ok(tuple(_date(2027, 1, 1).isocalendar()) == (2026, 53, 5) and tuple(_date(2026, 11, 27).isocalendar()) == (2026, 48, 5) and _date(2026, 11, 27).timetuple().tm_yday == 331, "iso week")
ok(_date(2026, 1, 1).weekday() == 3 and _date(2027, 1, 4).weekday() == 0 and tuple(_date(2026, 12, 28).isocalendar())[:2] == (2026, 53), "week year details")
ok(_dt.fromisoformat("2026-11-27T10:00:00-05:00").astimezone(_tz.utc) == _dt(2026, 11, 27, 15, tzinfo=_tz.utc), "offset parse")
try:
    _dt(2026, 11, 27, 10) < _dt(2026, 11, 27, 10, tzinfo=_tz.utc); ok(False, "naive aware")
except TypeError:
    ok(True, "naive aware")
ok((_dt(2026, 11, 27, 10) == _dt(2026, 11, 27, 10, tzinfo=_tz.utc)) is False, "naive aware equality")
ok(_dt.fromisoformat("2026-11-27T15:00:00-00:00") == _dt(2026, 11, 27, 15, tzinfo=_tz.utc), "-00:00 instant")
JS_TZ = []
js('process.env.TZ = "America/New_York"; console.log(new Date("2026-11-27").toISOString(), new Date("2026-11-27T00:00").toISOString())', "2026-11-27T00:00:00.000Z 2026-11-27T05:00:00.000Z", "js date-only")

# s=26 review: numbers on the wire
ok(math.ulp(1.8e9) == 2**-22 and float(np.spacing(np.float32(1.8e9))) == 128.0, "latency quantum")
ok(round(1.005 * 3, 2) == 3.01 and repr(1.005 * 3) == "3.0149999999999997" and str((Decimal("1.005") * 3).quantize(Decimal("0.01"), ROUND_HALF_UP)) == "3.02", "invoice")
ok(str(Decimal(1.005)) == "1.00499999999999989341858963598497211933135986328125" and str(round(Decimal("3.015"), 2)) == "3.02", "1.005 exact / half-even")
ok(float(np.spacing(np.float32(122.4))) == 2**-17 and abs(2**-17 * 111320 - 0.849) < 0.001 and abs(2**-17 * 111320 * math.cos(math.radians(37.77)) - 0.67) < 0.01, "longitude")
ok(float(np.spacing(np.float32(0.1))) == 2**-27 and abs(2**-27 * 111320 * 1000 - 0.83) < 0.01 and float(np.spacing(np.float32(4096.0))) * 111320 < 85 < float(np.spacing(np.float32(8192.0))) * 111320, "longitude distractors")
ok(float(np.spacing(np.float32(1e5))) == 2**-7, "binary32 durations")
ok(repr((0.1 + 0.2) + 0.3) == "0.6000000000000001" and math.isclose((0.1 + 0.2) + 0.3, 0.6), "parallel sum")
ok(float("1795737600.0000003") - float("1795737600.0000001") == 2**-22, "trace span")
js('try { JSON.parse(\'{"temp": NaN}\') } catch (e) { console.log(e.name) }', "SyntaxError", "json nan parse")

# s=27 review: text in boxes
_cafe = "Café ☕".encode()
ok(len(_cafe) == 9 and repr(_cafe[:7].decode("utf-8", "replace")) == "'Café �'" and _cafe.hex() == "436166c3a920e29895", "cafe truncation")
def _trunc(data, limit):
    if len(data) <= limit:
        return data
    cut = limit
    while cut > 0 and (data[cut] & 0xC0) == 0x80:
        cut -= 1
    return data[:cut]
ok(_trunc(_cafe, 7).decode() == "Café ", "safe truncation")
_g = "Great job \U0001F44D team"
ok(len(_g.encode("utf-16-le")) // 2 > 11 and _g.encode("utf-16-le")[20:24] == bytes.fromhex("3dd84ddc"), "surrogate at 10-11")
ok("\ud83d".encode("utf-8", "replace") == b"?" and "�".encode() == b"\xef\xbf\xbd", "replacement bytes")
ok("josé".encode() != "josé".encode() and "é".encode() == b"\xc3\xa9" and "é".encode() == b"e\xcc\x81" and ud.normalize("NFC", "josé") == "josé", "emails")
_hdr = b"\xef\xbb\xbfid,name".decode("utf-8").split(",")[0]
ok((len(_hdr), _hdr == "id") == (3, False) and b"\xef\xbb\xbfid,name".decode("utf-8-sig").split(",")[0] == "id", "bom header")
_w = "\U0001F44B" * 5
ok((len(_w), len(_w.encode("utf-16-le")) // 2, len(_w.encode())) == (5, 10, 20), "five emoji")
ok("Zoë".encode().decode("cp1252") == "ZoÃ«" and "ë".encode("latin-1") == b"\xeb" and b"\xeb".decode("utf-8", "replace") == "�", "mojibake")
ok("é€".encode().decode("cp1252") == "Ã©â‚¬", "mojibake table")

# s=28 review: time incidents
_v = 1795737600 + 15 * 365 * 86400
ok(15 * 365 * 86400 == 473040000 < 2**31 and _v == 2268777600 > 2**31 - 1 and _v - 2**32 == -2026189696 == (_v + 2**31) % 2**32 - 2**31, "license overflow")
ok(_f(-2026189696) == "1905-10-17T17:31:44Z" and _date(2026, 11, 27) + _td(days=5475) == _date(2041, 11, 23), "license dates")
ok(int(_dt(2041, 11, 27, tzinfo=_tz.utc).timestamp()) == 2269123200 == _v + 4 * 86400, "fixed license")
ok(_U(_dt(2026, 11, 1, 1, 30, tzinfo=_ny)).strftime("%H:%M") == "05:30" and _U(_dt(2026, 11, 1, 1, 30, fold=1, tzinfo=_ny)).strftime("%H:%M") == "06:30", "billing twice")

# late additions (s=4 python print, s=18 JS int32)
ok(repr(struct.unpack(">f", bytes.fromhex("ff800000"))[0]) == "-inf" and repr(struct.unpack(">f", bytes.fromhex("ff7fffff"))[0]) == "-3.4028234663852886e+38", "struct -inf")
ok(-24856 * 86400 + 74752 == -2**31, "int32 wrap arithmetic")
js('const ms = Date.UTC(2038, 0, 19, 3, 14, 8); const seconds = (ms / 1000) | 0; console.log(seconds, new Date(seconds * 1000).toISOString())', "-2147483648 1901-12-13T20:45:52.000Z", "js int32 wrap")

# run JavaScript checks with Node.js
import shutil
node = shutil.which("node")
if node:
    for code, expected, label in JS:
        out = subprocess.run([node, "-e", code], capture_output=True, text=True, timeout=30)
        ok(out.returncode == 0 and out.stdout.strip() == expected, f"{label}: got {out.stdout.strip()!r} {out.stderr.strip()[:200]}")
    print(f"{len(JS)} JavaScript checks run with {node}")
else:
    print("node not found: JavaScript checks skipped")
print(f"check_math: {checks} checks passed")
