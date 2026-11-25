---
title: 'Computing the tax: rate schedules and the capital gain worksheet'
questions:
- id: rate-schedule-calc
  prompt: Single, 2025 taxable income of $118,800, no capital gains. Using the rate schedule ($17,651 plus 24% of the excess over $103,350), what is the tax?
  explanation: $118,800 − $103,350 = $15,450; 24% × $15,450 = $3,708; $17,651 + $3,708 = $21,359 (p. 189). The base $17,651 is simply the tax on the first $103,350 (10% and 12% and 22% slices). Rate schedules are the same marginal arithmetic packaged as "base amount + rate × excess". Applying 24% to the whole amount ($28,512) is the bracket myth again.
  options:
  - text: $21,359
    correct: true
  - text: $17,651
  - text: $28,512
  - text: $26,136
- id: zero-percent
  prompt: A married couple filing jointly has 2025 taxable income of $90,000, of which $20,000 is long-term capital gain and the rest is wages. What rate applies to the gain?
  explanation: 'Capital gains stack on top of ordinary income. Ordinary income = $70,000; adding $20,000 brings the total to $90,000, still under $96,700, the 2025 joint ceiling for the 0% rate. So the entire gain is taxed at 0%, and their tax is just the ordinary tax on $70,000 ($7,923). The book highlights this: below the threshold, these gains and dividends “are taxed at 0 percent. It just doesn''t get much better than that!” (p. 189). This is why low-income years (early retirement, sabbaticals) are good for "harvesting gains" to reset basis tax-free.'
  options:
  - text: 0% on half and 15% on half
  - text: 0%, because their taxable income ($90,000) is below the $96,700 top of the 0% capital gain range
    correct: true
  - text: 12%, their ordinary bracket
  - text: 15%, because all capital gains are taxed at 15%
- id: gain-straddle
  prompt: A married couple has $80,000 of ordinary taxable income and $30,000 of long-term gain in 2025 (total $110,000). How is the gain taxed?
  explanation: The 0% band ends at $96,700 of taxable income for joint filers in 2025. Ordinary income uses $80,000 of it, leaving $16,700 of room at 0%; the remaining $13,300 of gain is taxed at 15% = $1,995. Total tax = tax on $80,000 ordinary ($9,123) + $1,995 = $11,118. (If you use the book's $96,950 threshold, you get $16,950 at 0% and a slightly smaller tax; the official 2025 figure is $96,700.) The worksheet automates exactly this layering (pp. 190–191).
  options:
  - text: All at the 22% ordinary rate
  - text: $16,700 at 0% (filling the room up to $96,700) and $13,300 at 15%
    correct: true
  - text: All $30,000 at 0%
  - text: All $30,000 at 15%
- id: worksheet-saving
  prompt: In the book's worksheet example, why does using the capital gain worksheet save $1,350?
  explanation: The single filer has $125,000 of taxable income, with $15,000 of preferential income. Ordinary tax on the $110,000 remainder is $19,247; 15% on $15,000 is $2,250; total $21,497, against $22,847 if computed entirely at ordinary rates (pp. 190–191). At $110,000–$125,000 her marginal ordinary rate is 24%, so the saving is 24% − 15% = 9% of $15,000. The book warns that using the Tax Table when you have qualified dividends “is a sure way to overpay your tax” (p. 83).
  options:
  - text: Because the worksheet doubles the standard deduction
  - text: 'Because $15,000 of qualified dividends and long-term gains is taxed at 15% instead of the 24% ordinary rate that would otherwise apply: 9% × $15,000 = $1,350'
    correct: true
  - text: Because qualified dividends are tax-free
  - text: Because the worksheet uses the 0% rate on everything
- id: special-rates
  prompt: Which long-term gains can be taxed above 20% (before NIIT)? Select all that apply.
  type: multiple
  explanation: “The maximum rate on the gain on the sale of collectibles is 28 percent. The rate for depreciable real estate is 25 percent on depreciation that's recaptured” (p. 190). These are maximum rates; if your ordinary bracket is lower, the lower rate applies. Gains on ordinary stocks and funds held more than a year and qualified dividends are capped at 20% (plus 3.8% NIIT for high earners). Physically backed precious-metal ETFs are treated as collectibles. These cases require the Schedule D Tax Worksheet, not the simpler worksheet (p. 191).
  options:
  - text: The portion of a rental building's gain attributable to prior depreciation (unrecaptured section 1250 gain, up to 25%)
    correct: true
  - text: Qualified dividends
  - text: Gain on selling an S&P 500 index fund held three years
  - text: Gain on selling collectibles such as coins, art or stamps (up to 28%)
    correct: true
- id: form-8814
  prompt: When can parents report a child's investment income on their own return using Form 8814 for 2025?
  explanation: Form 8814 is the “Parents' Election to Report Child's Interest and Dividends” (p. 192). The income limit is ten times the $1,350 kiddie threshold, $13,500 for 2025 (the book cites $13,000, the 2024 figure). It saves a separate return but can raise the parents' AGI, which may reduce their credits or deductions and increase state tax. Otherwise the child files Form 8615, which taxes unearned income above $2,700 at the parent's rate.
  options:
  - text: Only if the child has wages
  - text: Never; children always file separately
  - text: Whenever the child is under 24
  - text: When the child's only income is interest, dividends and capital gain distributions totalling less than $13,500, no estimated tax was paid in the child's name and no withholding applied
    correct: true
---
## Learning objectives

- Use the tax table or rate schedules
- Apply the 0%, 15% and 20% rates to qualified dividends and long-term gains with the worksheet
- Know the special 25% and 28% rates and the kiddie tax boxes

## The ideas

**Rate schedules** (pp. 187–189). Under $100,000 of taxable income you may use the Tax Table ($50 bands); at $100,000 or more you must use the Tax Computation Worksheet (the rate schedules). The book's example: single, $118,800 → $17,651 + 24% × ($118,800 − $103,350) = $17,651 + $3,708 = **$21,359**.

**Qualified dividends and long-term capital gains** (pp. 189–191) stack on top of ordinary income and are taxed at:

- **0%** up to taxable income of $48,350 single and MFS, $96,700 joint, $64,750 HOH (2025);
- **15%** up to $533,400 single, $600,050 joint, $566,700 HOH, $300,000 MFS;
- **20%** above.

The book gives the 0% ceilings as $48,475 / $96,950 / $64,850 (p. 189), which are the tops of the 12% ordinary brackets; for 2025 the capital gain thresholds are slightly lower ($48,350 / $96,700 / $64,750), a difference that matters only near the boundary. Plus 3.8% NIIT at high incomes. Unrecaptured section 1250 gain (depreciation on real estate) is taxed at up to 25%, collectibles at up to 28% (p. 190).

**The book's worksheet example** (pp. 190–191): single, taxable income $125,000 including $3,000 of qualified dividends and $12,000 of long-term gain. Ordinary part = $110,000, taxed at $19,247; the $15,000 is above the 0% range, so 15% × $15,000 = $2,250; total $21,497, versus $22,847 if everything were taxed at ordinary rates — a $1,350 saving (= 9% × $15,000, the difference between 24% and 15%). (The book's worksheet still mentions a 5% rate on one line, a holdover from before 2008; that rate is now 0%.)

**Kiddie tax and Form 8814** (pp. 191–192): a child's unearned income above $2,700 (2025; the book's p. 191 says $2,600, the 2024 figure) is taxed at the parent's rate (Form 8615). Parents may instead report a child's interest and dividends on their own return (Form 8814) if the child's income is only interest, dividends and capital gain distributions and is under $13,500 for 2025 (10 × $1,350; the book cites $13,000). Form 4972 covers old lump-sum averaging.

## Sources

- Tyson and Eck (2025), pp. 186–192.
- IRS, [Rev. Proc. 2024-40](https://www.irs.gov/pub/irs-drop/rp-24-40.pdf) (2025 capital gain thresholds); IRS, [Topic 409, Capital gains and losses](https://www.irs.gov/taxtopics/tc409).
