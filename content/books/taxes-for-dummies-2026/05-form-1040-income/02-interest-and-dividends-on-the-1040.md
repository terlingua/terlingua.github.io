---
title: Interest and dividends on the 1040
questions:
- id: muni-reporting
  prompt: Why do you report tax-exempt municipal bond interest on Form 1040 line 2a if it isn't taxed?
  explanation: “Even though you may not have to pay tax on tax-exempt interest, these numbers are important in figuring out how much of your Social Security benefits may be subject to tax” (p. 82). Tax-exempt interest is added to "provisional income" in the Social Security formula, so muni interest can indirectly make up to 85% of benefits taxable. Interest from private activity bonds (1099-INT box 9) is added back for the AMT. Tax-exempt interest also counts in modified AGI for the premium tax credit and Medicare IRMAA premiums.
  options:
  - text: Because the IRS charges a reporting fee
  - text: Because it is used in other calculations, such as how much of your Social Security is taxable, and private activity bond interest is a preference item for the AMT
    correct: true
  - text: Because it becomes taxable above $1,500
  - text: It doesn't need to be reported
- id: qualified-dividend-holding
  prompt: You buy a stock two days before its ex-dividend date, collect the dividend and sell it 20 days later. The company reports the dividend as qualified in box 1b. How is it taxed to you?
  explanation: 'The company''s box 1b says the *corporate* requirements are met. You must still meet the holding period: “a dividend is only qualified if you owned the shares in that corporation for more than 60 days during a 121-day period beginning 60 days before the ex-dividend date” (p. 82). Holding 22 days fails, so the dividend is taxed at ordinary rates (and the stock sale is a short-term gain or loss). The rule stops traders from buying just before a dividend to capture the low rate. For preferred stock dividends attributable to more than 366 days, the period is 90 days in a 181-day window.'
  options:
  - text: As a short-term capital gain
  - text: At 0%/15%/20% because the company reported it as qualified
  - text: At ordinary rates, because you didn't hold the shares more than 60 days in the 121-day window around the ex-dividend date
    correct: true
  - text: It is tax-free because you sold within 30 days
- id: schedule-b-threshold
  prompt: When must you file Schedule B?
  explanation: 'The book: if taxable interest is more than $1,500 “you must complete Schedule B” (p. 82), and the same applies to dividends over $1,500 (p. 83). Part III of Schedule B also asks about foreign financial accounts and foreign trusts, and must be completed if you had such accounts even with small income (Chapter 12 covers FBAR reporting). The authors suggest filing Schedule B even when not required if you have many 1099s, because detail helps resolve IRS matching notices.'
  options:
  - text: When taxable interest or ordinary dividends exceed $1,500, among other triggers (such as foreign accounts)
    correct: true
  - text: When you have more than five brokerage accounts
  - text: Only when you itemize
  - text: Whenever you have any interest income
- id: line-3a-3b
  prompt: Your 1099-DIV shows ordinary dividends of $4,000 in box 1a and qualified dividends of $3,000 in box 1b. What goes on lines 3a and 3b, and how much is taxed at ordinary rates?
  explanation: Box 1a (ordinary dividends) is the total of all dividends and includes the qualified ones; box 1b is the qualified subset. Line 3b gets the total ($4,000) and line 3a the qualified portion ($3,000). The tax computation (Qualified Dividends and Capital Gain Tax Worksheet) taxes $3,000 at 0/15/20% and the remaining $1,000 at ordinary rates. The book warns that if you have qualified dividends, using the regular tax table instead of the worksheet “is a sure way to overpay your tax” (p. 83).
  options:
  - text: Line 3a $3,000 and line 3b $4,000; $1,000 is taxed at ordinary rates and $3,000 at capital gain rates
    correct: true
  - text: Line 3a $3,000 and line 3b $1,000; $1,000 at ordinary rates
  - text: Line 3a $3,000 and line 3b $7,000
  - text: Both lines $4,000
- id: tey
  prompt: You are in the 32% federal bracket and your state doesn't tax its own munis but taxes other bond interest at 5%. An in-state muni yields 3.2%. What taxable bond yield would match it?
  explanation: Taxable-equivalent yield = tax-free yield ÷ (1 − marginal rate). Combined marginal rate ≈ 32% + 5% = 37% (ignoring the small federal benefit of deducting state tax, which most people don't get once SALT caps or the standard deduction apply). 3.2% ÷ 0.63 ≈ 5.08%. So the muni wins unless a taxable bond of similar quality yields over about 5.1%. The book's rule of thumb (p. 82) uses outdated income thresholds; the formula is the timeless version, and Chapter 24 returns to it. At a 12% bracket, the same muni equals only 3.2% ÷ 0.83 ≈ 3.9% (no state tax), so taxable bonds often win for lower brackets.
  options:
  - text: About 5.1%
    correct: true
  - text: 3.2%
  - text: About 4.2%
  - text: About 6.4%
---
## Learning objectives

- Distinguish tax-exempt interest, taxable interest, ordinary dividends and qualified dividends
- Apply the qualified-dividend holding period
- Know when Schedule B and Schedule D are required
- Evaluate tax-exempt bonds by taxable-equivalent yield

## The ideas

**Line 2a — tax-exempt interest** (Form 1099-INT box 8) isn't taxed, but it is reported because it counts in the Social Security taxation formula, and private activity bond interest (box 9) is an AMT preference (pp. 81–82).

**Line 2b — taxable interest.** Bank, bond, Treasury, and original-issue-discount interest. Schedule B is required if taxable interest *or* ordinary dividends exceed $1,500 (p. 82). Interest inside an IRA or 401(k) is not reported until withdrawn.

**Line 3a — qualified dividends** (1099-DIV box 1b) are taxed at the long-term capital gain rates of 0%, 15% or 20%. In your hands they qualify only if you held the stock “for more than 60 days during a 121-day period beginning 60 days before the ex-dividend date” (p. 82). **Line 3b — ordinary dividends** (box 1a) includes qualified dividends; the difference is taxed at ordinary rates (for example, REIT and money-market fund dividends). Don't add 3a and 3b together.

**Capital gain distributions** (box 2a) can go straight to line 7 without Schedule D if you have no other capital transactions (p. 83).

**Is muni interest worth it?** The book gives rule-of-thumb income thresholds (p. 82) that are outdated: the figures are from earlier years and its mention of capital gain rates of 5% and 10% refers to the pre-2003 era. The right test is the **taxable-equivalent yield**: tax-free yield ÷ (1 − your marginal rate, including state tax if the bond is exempt from it). A 3% muni equals a 3.85% taxable bond at a 22% rate but a 4.76% bond at 37%.

## Sources

- Tyson and Eck (2025), pp. 81–83.
- IRS, [Publication 550](https://www.irs.gov/publications/p550); IRS, [Instructions for Schedule B](https://www.irs.gov/forms-pubs/about-schedule-b-form-1040).
