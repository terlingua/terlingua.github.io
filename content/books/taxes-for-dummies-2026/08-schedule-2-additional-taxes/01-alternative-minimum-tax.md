---
title: The alternative minimum tax
questions:
- id: amt-concept
  prompt: How does the AMT interact with the regular income tax?
  explanation: “Under the rules set by Congress, you must figure both methods and then pay the higher tax” (p. 158). Strictly, the AMT is only the excess of tentative minimum tax over regular tax; that excess appears on Schedule 2 line 1 and flows to Form 1040 line 17. Software performs the comparison automatically, but the book warns that missing an AMT item from a K-1, Form 3921 or 1099 “is a sure way to mess up your AMT calculation” (p. 159).
  options:
  - text: AMT is a separate tax paid in addition to the full regular tax with no comparison
  - text: AMT replaces regular tax for everyone above $100,000
  - text: You choose whichever system gives the lower tax
  - text: You compute tentative minimum tax under AMT rules and regular tax under normal rules; if the AMT figure is higher, the excess is added as AMT on Schedule 2
    correct: true
- id: amt-calc
  prompt: A married couple's 2025 AMTI after the $137,000 exemption is $251,200. What is their tentative minimum tax (ignoring capital gains)?
  explanation: '26% applies to the first $239,100 of the excess over the exemption and 28% to the rest: 26% × $239,100 = $62,166; 28% × ($251,200 − $239,100) = 28% × $12,100 = $3,388; total $65,554 (p. 160). If their regular tax is $45,694 (the 2025 tax on $250,000 of joint taxable income), the AMT added on Schedule 2 is $19,860. The book''s sidebar uses $46,800 for regular tax, giving $18,754; recomputing with the 2025 brackets gives the slightly larger figure.'
  options:
  - text: $62,166
  - text: $65,312
  - text: $70,336
  - text: $65,554
    correct: true
- id: amt-triggers
  prompt: Which items commonly push people into the AMT? Select all that apply.
  type: multiple
  explanation: The book lists ISOs, private activity bond interest, investment interest, depreciation, K-1 adjustments and state tax refunds (p. 159), and notes that people with “a bunch of deductions from state income taxes, real estate taxes ... or passive investments” can be caught (p. 15). SALT is added back for AMT, so the larger SALT cap for 2025–2029 may bring some high-tax-state itemizers back into AMT range. Qualified dividends and long-term gains get the same preferential rates under the AMT, so by themselves they don't create AMT (though they raise AMTI and can erode the exemption).
  options:
  - text: Exercising incentive stock options and holding the shares past year-end
    correct: true
  - text: Large state and local tax deductions
    correct: true
  - text: Interest from specified private activity municipal bonds
    correct: true
  - text: Wages from a single W-2 job with the standard deduction and no other items
  - text: Qualified dividends taxed at 15%
- id: amt-iso-planning
  prompt: An engineer exercises ISOs in March 2026, creating a $300,000 spread, and the stock later falls. What planning point does the AMT raise?
  explanation: 'The ISO spread at exercise is an AMT adjustment even though no regular tax is due (p. 159; Chapter 6). If the stock then drops and she holds into the next year, she can owe AMT on a gain that no longer exists, recoverable only slowly through the minimum tax credit. A same-year disqualifying sale converts the spread to ordinary income (capped at the actual gain on the sale) and removes the AMT item. The book''s general advice: “Defer those deductions and tax incentives that trigger the AMT or accelerate income” — check the AMT before year-end.'
  options:
  - text: She can deduct the paper loss against the AMT immediately
  - text: If she sells the shares in the same calendar year (a disqualifying disposition), the spread becomes ordinary income for regular tax and the AMT adjustment disappears, which can be better than owing AMT on value that has since evaporated
    correct: true
  - text: She must hold the shares at least ten years
  - text: Nothing; ISOs are never taxed
- id: amt-2026
  prompt: What did OBBBA change about the AMT starting in 2026?
  explanation: Under the TCJA, exemptions phased out starting around $626,350 (single) and $1,252,700 (joint) for 2025, at 25 cents per dollar (p. 160). OBBBA made the higher exemptions permanent ($90,100 single and $140,200 joint for 2026) but reset the phase-out start to $500,000/$1,000,000 (indexed) with a 50% rate. A single filer's 2026 exemption is gone at $500,000 + 2 × $90,100 = $680,200 of AMTI. More upper-middle and high earners with ISOs or high SALT will need to run Form 6251.
  options:
  - text: It made state taxes deductible for AMT
  - text: It repealed the AMT for individuals
  - text: It reduced the AMT rates to 20% and 22%
  - text: It kept the higher exemptions permanently but lowered the phase-out thresholds (to $500,000 single and $1,000,000 joint) and doubled the phase-out rate to 50%, so the exemption disappears faster for high earners
    correct: true
---
## Learning objectives

- Explain the AMT as a parallel tax computation and when it bites
- Compute AMT from AMTI, exemption and the 26%/28% rates
- Identify common AMT adjustments and preferences, especially ISOs
- Know the 2025 and 2026 exemption and phase-out amounts

## The ideas

**A parallel universe.** The AMT is “a parallel tax system with its own set of rules”; you compute tax both ways and pay the higher (p. 158). Mechanically: start with taxable income, add back the standard deduction (or the SALT itemized deduction), add preference items, subtract the AMT exemption, apply 26% up to $239,100 of excess (2025; $119,550 MFS) and 28% above, and compare with regular tax. Long-term gains and qualified dividends keep their 0/15/20% rates inside the AMT.

**Common adjustments and preferences** (pp. 158–159): state and local taxes (not deductible for AMT), the standard deduction, incentive stock option spreads at exercise (Form 3921), private activity bond interest, depreciation differences, certain K-1 items (partnership box 17, S corporation box 15, trust box 12), and depletion.

**2025 exemptions** (p. 160): $137,000 joint (phasing out from $1,252,700), $88,100 single and head of household (from $626,350), $68,500 MFS (from $626,350). The exemption shrinks by 25 cents per dollar above the threshold. The TCJA's high exemptions shrank the AMT population dramatically; OBBBA made them permanent but, from 2026, lowered the phase-out thresholds to $500,000 single / $1,000,000 joint (indexed) and doubled the phase-out rate to 50%, so more high earners will feel it.

**The book's example** (p. 160), married filing jointly: taxable income $250,000; AMTI = $250,000 + $31,500 (standard deduction) + $50,000 (estate preference items) + $56,700 (ISO spread) = $388,200; minus $137,000 exemption = $251,200. AMT = 26% × $239,100 + 28% × $12,100 = $62,166 + $3,388 = **$65,554**. The book puts regular tax at $46,800; with the 2025 joint brackets, tax on $250,000 is **$45,694**, so the AMT line would be $65,554 − $45,694 = $19,860 (not $18,754).

**AMT credit.** AMT caused by timing items such as ISOs creates a minimum tax credit (Form 8801) usable in later years when regular tax exceeds tentative minimum tax (p. 178).

## Sources

- Tyson and Eck (2025), pp. 157–160, 178.
- IRS, [Instructions for Form 6251 (2025)](https://www.irs.gov/forms-pubs/about-form-6251); IRS, [Rev. Proc. 2025-32](https://www.irs.gov/pub/irs-drop/rp-25-32.pdf) (2026 AMT).
