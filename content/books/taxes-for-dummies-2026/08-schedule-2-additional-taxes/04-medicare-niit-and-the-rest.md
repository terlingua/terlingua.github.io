---
title: Additional Medicare Tax, the net investment income tax and the rest of Schedule 2
questions:
- id: addl-medicare-couple
  prompt: Spouses each earn $150,000 in W-2 wages in 2025 and file jointly. How much Additional Medicare Tax do they owe, and why might it come as a surprise?
  explanation: 'Additional Medicare Tax is 0.9% of combined wages above $250,000 for joint filers (p. 167). Employers withhold only on wages above $200,000 that they pay to an individual, without regard to filing status, so “for people who work multiple jobs and get to these limits only by combining wages from all their jobs, none of this additional tax will have been withheld” (p. 167). The same happens to dual-income couples. Fix it with extra W-4 withholding. The reverse also occurs: a single earner paid $230,000 has withholding on $30,000 but, if married to a non-earner, owes nothing, so the excess is credited back on Form 8959.'
  options:
  - text: $450 (0.9% of $50,000 over the $250,000 joint threshold); it wasn't withheld because neither employer paid either spouse more than $200,000
    correct: true
  - text: Zero, because neither spouse exceeded $200,000
  - text: $900, because each spouse is over $125,000
  - text: $2,700 (0.9% of $300,000)
- id: niit-calc
  prompt: A single filer has $230,000 of wages and $40,000 of capital gains and dividends (MAGI $270,000). What is her net investment income tax?
  explanation: NIIT = 3.8% × the lesser of net investment income ($40,000) or MAGI over the threshold ($270,000 − $200,000 = $70,000) (p. 167). Lesser = $40,000; tax = $1,520. If her wages were $170,000 instead (MAGI $210,000), the excess would be only $10,000, and NIIT would be $380. Wages are not investment income, but they push MAGI over the threshold, which exposes investment income to the tax.
  options:
  - text: $1,520, because 3.8% applies to the lesser of $40,000 of investment income or $70,000 of MAGI over $200,000
    correct: true
  - text: $10,260 (3.8% × $270,000)
  - text: $1,140
  - text: $2,660 (3.8% × $70,000)
- id: niit-exclusions
  prompt: Which items are NOT subject to the 3.8% net investment income tax? Select all that apply.
  type: multiple
  explanation: NIIT covers interest, dividends, capital gains, rents, royalties, passive business income and nonqualified annuities (p. 167). It excludes retirement plan distributions, tax-exempt interest, earned income (which bears Medicare tax instead) and excluded home-sale gain; “only the taxable portion is subject to the NIIT” (p. 168). Retirement distributions do raise MAGI, though, so a big IRA withdrawal can push investment income over the threshold. The thresholds ($200,000/$250,000/$125,000) aren't indexed, so more people pay it each year.
  options:
  - text: Municipal bond interest
    correct: true
  - text: Wages and self-employment income
    correct: true
  - text: Long-term capital gains on a brokerage account
  - text: Gain on a home sale that is excluded under the $250,000/$500,000 exclusion
    correct: true
  - text: Distributions from a 401(k) or IRA
    correct: true
  - text: Rental income from a property the owner doesn't actively manage as a business
- id: form-5329-items
  prompt: Which mistakes produce an "additional tax" computed on Form 5329? Select all that apply.
  type: multiple
  explanation: Form 5329 covers additional taxes on IRAs, other retirement plans, Coverdell ESAs, 529 plans, Archer MSAs and HSAs (p. 166). The book notes the form avoids the word "penalty" and says “additional tax” instead. (The HSA's 20% is technically computed on Form 8889, then carried to Schedule 2 alongside Form 5329's total.) Late payment is a separate penalty assessed under section 6651, not a Form 5329 tax.
  options:
  - text: Paying tax late (that's the failure-to-pay penalty)
  - text: A non-qualified HSA withdrawal at 50 (20%)
    correct: true
  - text: An excess Roth IRA contribution left in the account (6% a year)
    correct: true
  - text: A missed required minimum distribution (25%, or 10% if corrected promptly)
    correct: true
  - text: An IRA withdrawal at 45 with no exception (10%)
    correct: true
- id: installment-interest
  prompt: Why does Schedule 2 include interest on tax deferred by certain installment sales?
  explanation: In an installment sale the gain is fixed at sale but taxed as you collect. “Since you're taking your time paying the total tax, you're also going to pay interest on the tax” for dealer sales of residential lots and timeshares that elect installment treatment (line 14), and for sales over $150,000 when your outstanding installment obligations exceed $5 million (line 15) (pp. 168–169). The interest uses the underpayment rate (7% for most of 2025) and is not deductible.
  options:
  - text: It's a refund of interest the buyer paid
  - text: Because all installment sales owe a 25% penalty
  - text: Because installment sales are illegal
  - text: Because installment reporting lets you pay tax on the gain as payments arrive; for timeshares, residential lots and very large installment obligations, Congress charges interest on that deferral
    correct: true
---
## Learning objectives

- Compute the 0.9% Additional Medicare Tax and explain why withholding can fall short
- Compute the 3.8% net investment income tax
- Recognise Form 5329 additional taxes, installment-sale interest and recapture lines

## The ideas

**Additional Medicare Tax** (Form 8959; p. 167): 0.9% on wages and self-employment income above $200,000 (single, HOH), $250,000 (joint) or $125,000 (MFS). Employers must withhold it once wages paid to you exceed $200,000, regardless of your filing status. Two spouses each earning $150,000 have no extra withholding but owe 0.9% × ($300,000 − $250,000) = $450 on their joint return. The thresholds are not indexed.

**Net investment income tax** (Form 8960; pp. 167–168): 3.8% of the *lesser* of net investment income or modified AGI above the same thresholds. Investment income includes interest, dividends, capital gains, rents and royalties (unless from an active business or a real estate professional's rentals), passive business income and nonqualified annuities. It excludes wages, self-employment income, retirement plan distributions, tax-exempt interest and the excluded gain on a home sale (only the taxable portion counts).

**Other Schedule 2 items** (pp. 166–170): Form 5329 additional taxes (early distributions, excess contributions, missed RMDs, non-qualified HSA and 529 withdrawals); recapture of the 2008 first-time homebuyer credit; interest on deferred tax for certain installment sales (timeshares and residential lots; large installment obligations over $5 million); recapture of the low-income housing credit and other credits; and section 965 installments.

## Sources

- Tyson and Eck (2025), pp. 166–170.
- IRS, [Questions and answers for the Additional Medicare Tax](https://www.irs.gov/businesses/small-businesses-self-employed/questions-and-answers-for-the-additional-medicare-tax); IRS, [Topic 559, Net investment income tax](https://www.irs.gov/taxtopics/tc559).
