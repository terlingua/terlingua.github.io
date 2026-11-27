---
title: "Gregorian Leap Years"
questions:
  - id: "mod-four-bug"
    type: "single"
    prompt: "This helper ships in a billing system. Among the years 1900, 2000, 2024 and 2100, which does it classify **wrongly**?\n\n```python\ndef is_leap(year: int) -> bool:\n    return year % 4 == 0\n```"
    explanation: |-
      The Gregorian rule: a year is a leap year if it is divisible by 4, except years divisible by 100, which are leap years only if also divisible by 400.

      - 1900: divisible by 100, not by 400 → **not** leap; the helper says leap. Wrong.
      - 2000: divisible by 400 → leap; the helper says leap. Right (by luck).
      - 2024: divisible by 4, not by 100 → leap. Right.
      - 2100: divisible by 100, not by 400 → **not** leap; the helper says leap. Wrong.

      "2000 only" assumes the century exception always applies; 2000 is rescued by the 400 rule.

      "2024 only" doubts the ordinary 4-year rule.

      "None of them" is what testing with 2000 and 2024 suggests; the bug only shows in century years not divisible by 400.
    options:
      - text: "1900 and 2100"
        correct: true
      - text: "2000 only"
      - text: "2024 only"
      - text: "None of them"
  - id: "which-are-leap-years"
    type: "multiple"
    prompt: "Which of these are Gregorian leap years? Select all that apply."
    explanation: |-
      Apply the rule in order: divisible by 400 → leap; else divisible by 100 → common; else divisible by 4 → leap; else common.

      Correct:

      - 2000: 2000 / 400 = 5 → leap.
      - 2024: not divisible by 100; 2024 / 4 = 506 → leap.
      - 2400: 2400 / 400 = 6 → leap.

      Wrong:

      - 1900: 1900 / 100 = 19 but 1900 / 400 = 4.75 → common year.
      - 2100: 2100 / 100 = 21 but 2100 / 400 = 5.25 → common year. Many systems will meet this case for the first time on 2100-02-28.
    options:
      - text: "1900"
      - text: "2000"
        correct: true
      - text: "2024"
        correct: true
      - text: "2400"
        correct: true
      - text: "2100"
  - id: "days-in-400-years"
    type: "single"
    prompt: "How many days are in one full 400-year Gregorian cycle?"
    explanation: |-
      400 years contain 100 multiples of 4, minus the 4 century years, plus the 1 century divisible by 400: 100 − 4 + 1 = 97 leap years. Days = 400 × 365 + 97 = 146,000 + 97 = 146,097.

      146,097 = 7 × 20,871 exactly, so the cycle is a whole number of weeks: the Gregorian calendar, weekdays included, repeats every 400 years.

      "146,100" uses the Julian rule (every 4th year, 100 leap days).

      "146,096" drops the 400-year exception (96 leap days), as if 2000 were not a leap year.

      "146,000" ignores leap years entirely.
    options:
      - text: "146,100"
      - text: "146,096"
      - text: "146,097"
        correct: true
      - text: "146,000"
  - id: "leap-years-this-century"
    type: "single"
    prompt: "How many leap years are there from 2001 through 2100 inclusive?"
    explanation: |-
      Multiples of 4 in 2001–2100: 2004, 2008, …, 2100 → (2100 − 2004) / 4 + 1 = 25. Remove 2100, which is divisible by 100 but not 400: 24 leap years.

      "25" forgets the century exception for 2100.

      "26" counts 2000 as well, which is outside the range.

      "23" removes 2100 twice or skips 2004.
    options:
      - text: "25"
      - text: "26"
      - text: "23"
      - text: "24"
        correct: true
  - id: "mean-gregorian-year"
    type: "single"
    prompt: "What is the mean length of a Gregorian calendar year, in days?"
    explanation: |-
      146,097 days / 400 years = 365.2425 days, exactly. The rule was designed so that the calendar year approximates the tropical year (about 365.2422 days), drifting by roughly one day in about 3,000 years.

      "365.25" is the Julian calendar's mean year (one leap day every 4 years), which drifted about 3 days every 400 years and forced the 1582 reform.

      "365.2422" is the approximate tropical (solar) year, the astronomical quantity the calendar approximates; it is not the calendar's exact mean.

      "365.24" is a rounded value that corresponds to no calendar rule.
    options:
      - text: "365.2425"
        correct: true
      - text: "365.25"
      - text: "365.2422"
      - text: "365.24"
  - id: "february-29-plus-one-year"
    type: "single"
    prompt: "What does this C# line print?\n\n```csharp\nConsole.WriteLine(new DateTime(2024, 2, 29).AddYears(1).ToString(\"yyyy-MM-dd\"));\n```"
    explanation: |-
      2025 is not a leap year, so 2025-02-29 does not exist. .NET's `AddYears` documents that if the resulting day is invalid for the resulting month, the last valid day of the month is used: 2025-02-28.

      This is a library policy, not a calendar law. Python's `date(2024, 2, 29).replace(year=2025)` raises `ValueError: day is out of range for month`, and other libraries roll forward to March 1. "One year after February 29" has no single correct answer, so business rules (renewals, birthdays, interest periods) must state their own.

      "2025-03-01" is the roll-forward policy some libraries and databases use.

      "It throws `ArgumentOutOfRangeException`" is what constructing `new DateTime(2025, 2, 29)` directly would do; `AddYears` clamps instead.

      "2025-02-29" is not a valid date.
    options:
      - text: "2025-03-01"
      - text: "2025-02-28"
        correct: true
      - text: "It throws `ArgumentOutOfRangeException`."
      - text: "2025-02-29"
---

## Learning objectives

- Apply the Gregorian leap-year rule, including both century exceptions.
- Count leap years and days over arbitrary ranges.
- Explain the 400-year cycle and the mean year length.
- Recognize that date arithmetic around February 29 is a policy choice.

## The invariant

The Gregorian calendar (introduced 1582; the calendar used by ISO 8601 and essentially every computing system) defines leap years as:

1. divisible by 400 → leap year;
2. otherwise divisible by 100 → common year;
3. otherwise divisible by 4 → leap year;
4. otherwise → common year.

In code: `y % 4 == 0 and (y % 100 != 0 or y % 400 == 0)`.

Consequences:

- **97 leap years per 400 years**, so a cycle has 400 × 365 + 97 = 146,097 days and a mean year of exactly 365.2425 days.
- **146,097 is divisible by 7** (20,871 weeks), so the calendar, including weekdays, repeats exactly every 400 years. Date libraries exploit this to reduce any date into a single cycle.
- **Century years are the dangerous cases.** 2000 was a leap year (divisible by 400), so code with the naive `% 4` rule passed the year 2000. The next century year, 2100, is not a leap year, and the same code fails there.
- **ISO 8601 uses the proleptic Gregorian calendar**: the same rules extended backwards before 1582, even though many countries adopted the calendar much later. Historical dates therefore need care, but computing systems are consistent.

Leap years also make "add one year" and "add one month" ill-defined for some dates: February 29 plus a year, or January 31 plus a month. Libraries choose a policy (clamp to the end of the month, roll over, or throw), and the policy belongs in the requirements, not in the library default.

## Worked example

Days between 2000-03-01 and 2100-03-01:

| Step | Value |
|---|---|
| Years | 100 |
| Leap days counted (Feb 29 of 2004 … 2096; 2100 excluded) | 24 |
| Days | 100 × 365 + 24 = 36,524 |

```python
import calendar
from datetime import date

print([y for y in (1900, 2000, 2024, 2100, 2400) if calendar.isleap(y)])  # [2000, 2024, 2400]
print(calendar.leapdays(2001, 2101))                                     # 24
print((date(2100, 3, 1) - date(2000, 3, 1)).days)                         # 36524
```

```csharp
Console.WriteLine(DateTime.IsLeapYear(2100));                             // False
Console.WriteLine(new DateTime(2024, 1, 31).AddMonths(1).ToString("d", CultureInfo.InvariantCulture)); // 02/29/2024
```

`calendar.leapdays(y1, y2)` counts leap years in `[y1, y2)`, so the upper bound is exclusive.

## Where this shows up today

As of November 2026, the most recent leap day was 2024-02-29 and the next is 2028-02-29. Leap-day incidents recur every four years in payment terminals, licensing checks and scheduled jobs, usually from hand-written date arithmetic or "day 366" assumptions rather than from library bugs.

## Pitfalls checklist

- Implementing leap years as `year % 4 == 0`.
- Assuming every year has 365 days, or that day-of-year 366 never occurs.
- Adding 365 × 86,400 seconds for "one year" instead of using calendar arithmetic.
- Not defining what one year after February 29 (or one month after January 31) means for the business.
- Testing only with 2000 and an ordinary leap year, which hides the century bug.

## Key takeaways

- Leap year: divisible by 4, except centuries, except centuries divisible by 400.
- 400 years = 146,097 days = 20,871 weeks; mean year 365.2425 days.
- 2100 is not a leap year; naive code that survived 2000 will fail then.
- Month- and year-addition across February 29 is policy; make it explicit.
