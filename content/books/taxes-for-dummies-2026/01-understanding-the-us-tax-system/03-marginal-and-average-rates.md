---
title: Taxable income, marginal and average rates
questions:
- id: define-taxable-income
  prompt: Which is the best definition of taxable income?
  explanation: '“Taxable income is defined as the amount of income on which you actually pay income taxes” (p. 12). Two things make it smaller than what you earned: some income is excluded entirely (municipal bond interest, most gifts and inheritances, employer health premiums), and deductions are subtracted. On Form 1040 the path is total income → minus adjustments (Schedule 1) → AGI → minus standard or itemized deduction, the qualified business income deduction and the new Schedule 1-A deductions → taxable income. The brackets are applied to that last number, never to gross pay.'
  options:
  - text: Everything you earned during the year from all sources
  - text: Your gross wages before payroll deductions
  - text: Income after income tax has been paid
  - text: 'The amount on which income tax is actually computed: taxable income sources minus adjustments and minus the standard or itemized deduction (and certain other deductions)'
    correct: true
- id: compute-60k
  prompt: Using the 2025 single brackets (10% to $11,925; 12% to $48,475; 22% to $103,350), what is the federal income tax on taxable income of exactly $60,000?
  explanation: 'Tax is computed slice by slice. 10% × $11,925 = $1,192.50. The 12% slice runs from $11,925 to $48,475, which is $36,550, so 12% × $36,550 = $4,386. The 22% slice is $60,000 − $48,475 = $11,525, so 22% × $11,525 = $2,535.50. Total: $1,192.50 + $4,386 + $2,535.50 = $8,114. Applying 22% to everything ($13,200) is the classic mistake: the top rate applies only to dollars in the top slice. (In practice, for taxable income under $100,000 the IRS Tax Table gives the tax for $50 bands, so a table figure differs from this exact computation by a few dollars.)'
  options:
  - text: $7,200
  - text: $13,200 (22% of everything)
  - text: $8,114
    correct: true
  - text: $9,600
- id: average-vs-marginal
  prompt: For that single filer with $60,000 of taxable income and $8,114 of tax, what are the marginal and average rates?
  explanation: 'The marginal rate is the rate on the next (last) dollar: $60,000 is inside the 22% bracket. The average (or effective) rate is total tax divided by taxable income: $8,114 ÷ $60,000 ≈ 13.5%. The book stresses that the marginal rate “should be higher than your average tax rate” and is the one you need for decisions (p. 14), because it tells you what another dollar of income costs or another dollar of deduction saves. People sometimes quote "effective rate" against total income (before deductions), which gives an even lower number; always ask what the denominator is.'
  options:
  - text: Marginal 22%, average about 13.5%
    correct: true
  - text: Both 22%
  - text: Marginal 12%, average 10%
  - text: Marginal 13.5%, average 22%
- id: value-of-deduction
  prompt: You are in the 24% federal bracket and your state taxes the same income at 5%. You make an extra $1,000 deductible contribution to a traditional 401(k). Ignoring payroll tax, roughly how much income tax do you save this year?
  explanation: 'A deduction (or pre-tax exclusion) saves tax at your marginal rate, because it removes dollars from the top slice. Federal: $1,000 × 24% = $240, exactly the book''s example (p. 14). The book adds that “your total marginal rate includes your federal and state income tax rates” (p. 14), so with a 5% state rate you save another $50 — about $290 in all. (Strictly, state tax paid can itself be an itemized federal deduction, which slightly changes the combined figure for itemizers.) Note what you don''t save: 401(k) deferrals are still subject to Social Security and Medicare tax.'
  options:
  - text: $50
  - text: About $290 (24% federal plus 5% state)
    correct: true
  - text: $1,000
  - text: $240
- id: raise-myth
  prompt: A colleague refuses a $2,000 raise because it would "push her into the next bracket and she'd take home less." Using the 2025 brackets, what is wrong with that reasoning?
  explanation: A graduated system taxes each slice at its own rate. If she crosses from 12% to 22%, only the part of the $2,000 above the threshold is taxed at 22%; the rest is still at 12%. She keeps at least 78% of the raise (less payroll and state taxes). The bracket myth is common enough that the book spends pages on how marginal rates work (pp. 12–14). The real exceptions are cliffs and phase-outs in other benefits (for example, a means-tested credit), which can make an effective marginal rate very high over a narrow range — but brackets alone never do that.
  options:
  - text: Only the dollars above the bracket threshold are taxed at the higher rate, so her take-home pay still rises
    correct: true
  - text: Brackets apply to gross income, not taxable income
  - text: Raises are never taxable
  - text: Nothing; crossing a bracket raises the rate on all income
- id: phase-outs-hidden-rate
  prompt: Why can a phase-out of a tax break act like a higher marginal tax rate?
  explanation: Many breaks shrink as income rises. The book lists education breaks, the saver's credit (in 2025, no credit above AGI of $39,500 single or $79,000 joint), the $25,000 rental real estate loss allowance (begins to shrink above $100,000 of AGI) and Roth IRA eligibility (phases out from $150,000 single, $236,000 joint for 2025) (p. 14). Inside a phase-out range, one more dollar of income costs you your bracket rate plus the lost benefit. For example, the OBBBA senior deduction falls by 6% of income above $75,000 (single); for someone in the 22% bracket that adds about 22% × 6% ≈ 1.3 points to the effective marginal rate.
  options:
  - text: Because the IRS doubles your rate when you exceed the limit
  - text: They don't; phase-outs only affect average rates
  - text: Because each extra dollar of income both is taxed at your bracket rate and reduces a benefit you were receiving, so you lose more than the bracket rate on that dollar
    correct: true
  - text: Because phase-outs are added to the tax tables
- id: no-income-tax-states
  prompt: Which states does the book list as having no state income tax?
  explanation: 'The book lists those nine (p. 14). Two footnotes help: New Hampshire''s tax on interest and dividends was fully repealed from 2025, so it now taxes neither wages nor investment income; and Washington taxes certain large long-term capital gains (the book''s map omits “capital gains taxes”, p. 14). Living in a no-income-tax state doesn''t mean low taxes overall: such states often rely more on sales and property taxes. For federal itemizers, state income or sales tax and property tax feed the SALT deduction (Chapter 11).'
  options:
  - text: Alaska, Florida, Nevada, New Hampshire, South Dakota, Tennessee, Texas, Washington and Wyoming
    correct: true
  - text: All states have income tax
  - text: Only Alaska and Texas
  - text: California, New York, Florida and Texas
- id: amt-concept
  prompt: What is the Alternative Minimum Tax, in one sentence?
  explanation: 'Congress created a minimum tax in 1969 (the modern AMT dates from later reforms) “to ensure that higher-income earners with relatively high amounts of itemized deductions pay at least a minimum amount of taxes” (p. 14). The AMT disallows some deductions (notably state and local taxes) and adds back certain preferences (such as the bargain element of incentive stock options), applies an exemption and rates of 26% and 28%, and you owe the excess over regular tax. The book: “you must figure the tax you owe under the AMT system and under the other system and then pay whichever amount is higher” (p. 15). Chapter 8 covers Form 6251.'
  options:
  - text: A flat 10% tax everyone pays in addition to regular tax
  - text: A tax on minimum-wage workers
  - text: A second tax computation, with fewer deductions and its own exemption and rates, created to ensure higher-income people with many tax breaks pay at least a minimum; you pay whichever of regular tax or AMT is higher
    correct: true
  - text: A penalty for filing the wrong form
- id: graduated-meaning
  prompt: What does it mean that the US has a "graduated" income tax?
  explanation: “The government charges you different income tax rates for different portions of your annual income” (p. 12). The marginal rate is the rate on the last dollars. The book notes graduated taxes are ancient (it mentions Greece) and that top federal rates have ranged from 7% in 1913 to around 90% in the 1950s; today's top rate is 37%. Graduated rates mean a salary that feels uniform per paycheck is actually taxed unevenly across the year's income.
  options:
  - text: Rates rise with age
  - text: Everyone pays the same rate but deductions vary
  - text: Different portions of taxable income are taxed at rising rates, so the first dollars bear lower rates than the last dollars
    correct: true
  - text: Tax is due in instalments
---
## Learning objectives

- Define taxable income and explain why it is less than what you earn
- Compute tax across brackets, and the marginal and average rate
- Use your marginal rate to value a deduction or extra income
- Explain how phase-outs, state taxes and the AMT raise effective marginal rates

## The ideas

**Taxable income.** You do not pay income tax on everything you earn, for two reasons: some income is not taxable (municipal bond interest, for example), and you subtract deductions. For 2025 the standard deduction is $15,750 single, $31,500 married filing jointly and $23,625 head of household (p. 12), with extra amounts for people 65 or older or blind. You itemize instead only if your itemized deductions are larger.

**Brackets.** The US has a graduated income tax: each rate applies only to the slice of income inside its bracket. The 2025 brackets (p. 13, matching IRS Rev. Proc. 2024-40 as amended):

| Rate | Single | Married filing jointly |
|---|---|---|
| 10% | up to $11,925 | up to $23,850 |
| 12% | to $48,475 | to $96,950 |
| 22% | to $103,350 | to $206,700 |
| 24% | to $197,300 | to $394,600 |
| 32% | to $250,525 | to $501,050 |
| 35% | to $626,350 | to $751,600 |
| 37% | above | above |

**Worked example (the book's).** Single, taxable income $60,000:

- 10% × $11,925 = $1,192.50
- 12% × ($48,475 − $11,925) = 12% × $36,550 = $4,386.00
- 22% × ($60,000 − $48,475) = 22% × $11,525 = $2,535.50
- Total = **$8,114**

The **marginal rate** is 22% (the rate on the last dollar). The **average rate** is $8,114 ÷ $60,000 ≈ 13.5%. The marginal rate tells you what a change is worth: “if you're in the federal 24 percent tax bracket, for every $1,000 that you can reduce your taxable income, you shave $240 off your federal income tax bill” (p. 14).

**Myth: a raise can lower your take-home pay.** Moving into a higher bracket only taxes the *new* dollars at the higher rate. Earlier dollars stay taxed at the lower rates.

**Hidden marginal rates.** Many breaks phase out as income rises (education credits, the saver's credit, the $25,000 rental loss allowance, Roth IRA eligibility, p. 14). Losing a benefit as you earn more acts like an extra tax on those dollars.

**State taxes.** Your total marginal rate includes state (and sometimes local) income tax. Nine states have no broad wage income tax: Alaska, Florida, Nevada, New Hampshire, South Dakota, Tennessee, Texas, Washington and Wyoming (p. 14). (Washington does tax certain long-term capital gains, one of the "other taxes" the book's map excludes.)

**The AMT.** A parallel system created in 1969 to make sure high-income people with many deductions pay a minimum amount. You compute tax both ways and pay the higher (p. 15). Chapter 8 explains it.

## Sources

- Tyson and Eck (2025), pp. 11–15.
- IRS, [Rev. Proc. 2024-40](https://www.irs.gov/pub/irs-drop/rp-24-40.pdf) (2025 brackets) and [IR-2025-103](https://www.irs.gov/newsroom/irs-releases-tax-inflation-adjustments-for-tax-year-2026-including-amendments-from-the-one-big-beautiful-bill) (OBBBA standard deduction for 2025).
