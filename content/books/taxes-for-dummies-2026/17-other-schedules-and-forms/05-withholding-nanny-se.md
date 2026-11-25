---
title: W-4, household employment and self-employment tax
questions:
- id: nanny-threshold
  prompt: You pay a nanny $400 a week all of 2025 ($20,800). What federal employment taxes apply?
  explanation: Cash wages of $2,800 or more in 2025 trigger FICA, and $1,000 in any quarter triggers FUTA (pp. 387–388). Schedule H replaces quarterly federal payroll forms; state filings may still be quarterly. Paying under the table forfeits the dependent care credit and the nanny's Social Security credits, and risks back taxes and penalties.
  options:
  - text: Only income tax withholding
  - text: None, because household workers are exempt
  - text: Social Security and Medicare (15.3% total, half of which you can withhold from the nanny) and FUTA, reported on Schedule H with your Form 1040, plus a W-2 by January 31, 2026
    correct: true
  - text: Quarterly Form 941 filings are mandatory
- id: nanny-teen
  prompt: Which household workers' 2025 wages are exempt from Social Security and Medicare tax, however much you pay?
  explanation: 'The book says the exemption covers employees “under the age of 21” (p. 388); IRS Publication 926 says wages to an employee under 18 at any time during the year are exempt unless household work is their principal occupation (being a student isn''t household work). Separately exempt: wages to your spouse, your child under 21, and (usually) your parent. A 19- or 20-year-old babysitter paid $2,800+ is covered. The $2,700 housekeeper is under the 2025 threshold.'
  options:
  - text: A 20-year-old full-time nanny
  - text: A 16-year-old neighbour babysitting occasionally while in school
    correct: true
  - text: A 19-year-old part-time babysitter who's a college student
  - text: A 40-year-old housekeeper paid $2,700
- id: se-tax-calc
  prompt: A freelancer has $100,000 of 2025 net profit and no wages. What's the self-employment tax and its deduction?
  explanation: Net SE earnings = 92.35% × $100,000 = $92,350 (mimicking the employer-half deduction an employee gets). × 15.3% = $14,129.55. Half ($7,064.78) goes on Schedule 1 line 15 (the book says line 14, p. 389). Above the wage base ($176,100 for 2025), only the 2.9% Medicare part continues. The deduction reduces income tax, not SE tax.
  options:
  - text: $15,300 of tax, $7,650 deductible
  - text: $12,400
  - text: About $14,130 of SE tax ($100,000 × 92.35% × 15.3%), with about $7,065 deductible as an adjustment
    correct: true
  - text: $15,300, nothing deductible
- id: se-with-wages
  prompt: In 2025 you earn $150,000 of W-2 wages and have $60,000 of net self-employment profit. How does the Social Security wage base affect your SE tax?
  explanation: Wages subject to Social Security count against the base first. Net SE earnings = $60,000 × 92.35% = $55,410; Social Security applies to $176,100 − $150,000 = $26,100 → $3,236; Medicare 2.9% × $55,410 = $1,607. Total about $4,843. Combined earnings over $200,000 also trigger the 0.9% additional Medicare tax on Form 8959.
  options:
  - text: None, because wages exceed $100,000
  - text: All $60,000 is taxed at 15.3%
  - text: Only $26,100 of net SE earnings (the $176,100 base minus $150,000 of wages) bears the 12.4% Social Security part; all $55,410 of net SE earnings bears 2.9% Medicare
    correct: true
  - text: Wages don't interact with SE tax
---
## Learning objectives

- Keep withholding right with the W-4 and the IRS estimator
- Apply the 2025 nanny tax thresholds (with a correction on the age exemption)
- Compute self-employment tax and its deduction

## The ideas

**Form W-4** (p. 387): revise it after marriage, divorce, a child, a home purchase or income changes; use the IRS Tax Withholding Estimator. Since 2020 the W-4 has no "allowances"; you enter dependents, other income, deductions and extra withholding. The IRS can send a **lock-in letter** fixing your withholding; a W-4 without reasonable basis carries a $500 civil penalty; willful falsification is a crime (p. 386).

**Household employees** (Schedule H, pp. 387–389): if you pay a household worker (nanny, housekeeper, gardener employed by you, not an agency) **$2,800 or more** in 2025 ($3,000 in 2026), you owe Social Security and Medicare (7.65% employer + 7.65% employee withheld or paid by you). **FUTA** applies if you paid $1,000+ in any quarter this or last year. Income tax withholding is optional. Pay with your 1040 via Schedule H, but increase your own withholding or estimates to avoid underpayment penalties. Issue a W-2 and file W-2/W-3 with SSA by January 31; check state unemployment and workers' compensation. *Correction:* the book says workers under **21** are exempt from Social Security tax (p. 388); the exemption is for workers **under 18** at any time in the year whose principal occupation isn't household work (wages paid to your own child under 21, your spouse, or your parent are separately exempt).

**Self-employment tax** (Schedule SE, p. 389): 15.3% (12.4% Social Security up to the wage base — $176,100 for 2025, $184,500 for 2026 — plus 2.9% Medicare on all) on **92.35%** of net SE earnings, if those are $400 or more. Wages count toward the wage base first. Plus 0.9% additional Medicare tax above $200,000 single/$250,000 joint. Half of SE tax (excluding the 0.9%) is an adjustment — *correction:* on Schedule 1 **line 15**, not line 14 as the book says on p. 389.

## Sources

- Tyson and Eck (2025), pp. 386–390.
- IRS, [Publication 926 (2025), Household Employer's Tax Guide](https://www.irs.gov/publications/p926); IRS, [Instructions for Schedule SE (2025)](https://www.irs.gov/instructions/i1040sse); IRS, [Tax Withholding Estimator](https://www.irs.gov/individuals/tax-withholding-estimator).
