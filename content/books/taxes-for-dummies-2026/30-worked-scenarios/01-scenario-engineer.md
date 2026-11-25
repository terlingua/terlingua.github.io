---
title: 'Scenario 1: a single software engineer'
questions:
- id: s1-agi
  prompt: What's Priya's 2025 AGI?
  explanation: '401(k) deferrals and cafeteria-plan HSA contributions are excluded from box 1 wages (the HSA also escapes FICA). Investment income is added: $152,200 + $3,000 + $5,000 = $160,200. If she had funded the HSA directly instead, she''d deduct it on Schedule 1, reaching the same AGI but paying FICA on it.'
  options:
  - text: $180,000
  - text: $156,500
  - text: $188,000
  - text: '$160,200: W-2 wages of $152,200 ($180,000 − $23,500 − $4,300) plus $3,000 of dividends and $5,000 of gain'
    correct: true
- id: s1-deduction
  prompt: Should Priya itemize?
  explanation: Itemized deductions must exceed the standard deduction to help. $12,500 < $15,750. Taxable income = $160,200 − $15,750 = $144,450. From 2026 she could also take up to $1,000 of cash gifts as a non-itemizer charitable deduction.
  options:
  - text: Yes, because state tax is large
  - text: 'No: SALT ($12,000) plus charity ($500) is $12,500, less than the $15,750 standard deduction, so taxable income is $144,450'
    correct: true
  - text: She can take both
  - text: Yes, itemizing is always better for high earners
- id: s1-tax
  prompt: What's her regular income tax?
  explanation: 'Ordinary taxable income = $144,450 − $8,000 = $136,450. Tax: 10% × $11,925 = $1,192.50; 12% × $36,550 = $4,386; 22% × $54,875 = $12,072.50; 24% × $33,100 = $7,944; total $25,595. The $8,000 of qualified dividends and long-term gains sits above the 0% band ($48,350), so it''s taxed at 15%: $1,200. Preferential rates saved her $720 versus ordinary treatment.'
  options:
  - text: $27,515, taxing everything at ordinary rates
  - text: $25,595
  - text: $34,668 (24% of $144,450)
  - text: '$26,795: $25,595 on $136,450 of ordinary income plus $1,200 (15%) on $8,000 of dividends and gains'
    correct: true
- id: s1-niit-marginal
  prompt: What are Priya's marginal rate on another dollar of salary and her NIIT exposure?
  explanation: Her ordinary taxable income ($136,450) is in the 24% bracket (which runs to $197,300). NIIT (3.8%) applies only to the lesser of net investment income or MAGI over $200,000. Another $1,000 to her 401(k) or HSA would save $240 of federal income tax, plus state tax (and FICA for the HSA via payroll).
  options:
  - text: 24% on salary (plus 7.65% FICA up to the wage base, and state tax); no NIIT because her MAGI is below $200,000
    correct: true
  - text: 37%, and NIIT applies
  - text: 15%
  - text: 24%, and NIIT applies to her dividends
---
## Learning objectives

- Go from gross pay to AGI with pre-tax deductions
- Choose the standard deduction and compute taxable income
- Apply the qualified dividend and capital gain worksheet
- Check NIIT and marginal rates

## The facts

Priya is single, 34, a renter. In 2025 she earns $180,000 of salary and RSU vesting (all on her W-2). She defers $23,500 to her 401(k) (pre-tax) and puts $4,300 into her HSA through her employer's cafeteria plan. Her taxable brokerage account paid $3,000 of qualified dividends, and she sold index fund shares held three years for a $5,000 gain. She paid $12,000 of state income tax and gave $500 to charity.

## How the pieces fit

Gross pay − pre-tax 401(k) and cafeteria-plan HSA = W-2 box 1 wages (Chapter 5). Add investment income → AGI. Compare itemized deductions (SALT + charity + interest) with the $15,750 standard deduction (Chapters 4, 11). Tax ordinary income with the brackets, then tax qualified dividends and long-term gains with the 0/15/20% worksheet (Chapter 10). NIIT starts at $200,000 MAGI (Chapter 8).

## Sources

- IRS, Rev. Proc. 2024-40 (2025 brackets and gain thresholds) as modified by Rev. Proc. 2025-32; IRS, [Instructions for Form 1040 (2025)](https://www.irs.gov/instructions/i1040gi), Qualified Dividends and Capital Gain Tax Worksheet.
