---
title: Military moving expenses, Form 8606 and Roth distributions
questions:
- id: pro-rata
  prompt: At the end of 2025 you have $10,000 of nondeductible basis and IRA balances that, with the $20,000 you converted to a Roth during 2025, total $100,000. How much of the conversion is taxable?
  explanation: 'Line 10''s ratio = basis ÷ (year-end IRA value + distributions + conversions): “divide $10,000 by $100,000 to get 0.1 (or 10 percent)” (p. 378). The conversion is 10% basis, 90% taxable. Basis can''t be cherry-picked: all your traditional, SEP and SIMPLE IRAs are aggregated. That''s why "backdoor Roth" users roll pre-tax IRAs into a 401(k) (which isn''t counted) before converting.'
  options:
  - text: $20,000
  - text: $0, because you converted only nondeductible money
  - text: '$18,000: 10% of the conversion ($2,000) is basis and 90% is taxable'
    correct: true
  - text: $10,000
- id: backdoor-roth
  prompt: A single engineer with $200,000 of income and no other IRAs contributes $7,000 nondeductibly to a traditional IRA in January 2026 and converts it to a Roth a week later, when it's worth $7,005. What's taxable, and which form is filed?
  explanation: Income above the Roth limit ($165,000 single for 2025, p. 377) blocks direct Roth contributions but not conversions (income limits on conversions ended in 2010). With no pre-tax IRA balance on December 31, the pro-rata ratio is 100% basis, so only the $5 of earnings is taxable. A pre-tax IRA balance at year-end, even from a rollover after the conversion, would make most of it taxable.
  options:
  - text: The contribution is illegal because income exceeds the Roth limit
  - text: Nothing, and no form is needed
  - text: $5 is taxable; Form 8606 reports the $7,000 nondeductible contribution (Part I) and the conversion (Part II)
    correct: true
  - text: $7,005 is taxable
- id: roth-ordering
  prompt: You're 45, opened a Roth in 2015 and contributed a total of $30,000 (withdrawing $1,500 in 2024). The account is worth $60,000, with no conversions. You withdraw $35,000 in 2025. What's taxable?
  explanation: 'Nonqualified Roth distributions come out contributions first. Basis = $30,000 − $1,500 = $28,500 (the book''s example, p. 379). The excess $6,500 is earnings: taxable and penalized because you''re under 59½ and no exception applies. The five-year rule alone doesn''t make distributions qualified; you also need age 59½, death, disability or a first-time home purchase.'
  options:
  - text: All $35,000
  - text: Nothing, because the account is over five years old
  - text: $5,000
  - text: $6,500 of earnings is taxable and subject to the 10% penalty; the first $28,500 is a tax-free return of contributions
    correct: true
- id: military-move
  prompt: 'An active-duty soldier moves under permanent change of station orders from Texas to a base 40 miles away. Unreimbursed costs: $2,000 for movers, $400 for meals en route, 300 miles of driving. What can be deducted for 2025?'
  explanation: “Meals are never deductible” (p. 375). The 2025 IRS medical/moving mileage rate is 21 cents (the book says 22 cents, p. 375). IRS Publication 3 states that Armed Forces members moving under PCS orders don't have to meet the distance or time tests. Civilians can't deduct moving costs at all through at least 2025 and, under OBBBA, permanently; a few states still allow it.
  options:
  - text: The $2,000 moving cost and $63 of mileage (300 × 21 cents); meals aren't deductible, and the 50-mile test doesn't apply to PCS moves
    correct: true
  - text: Nothing, because the move is under 50 miles
  - text: $2,000 plus meals
  - text: $2,463
---
## Learning objectives

- Know who can still deduct moving expenses and which costs qualify
- Track nondeductible IRA basis and apply the pro-rata rule
- Compute the taxable part of a conversion and a Roth distribution

## The ideas

**Moving expenses** (Form 3903, pp. 374–376): deductible only for active-duty Armed Forces members moving under a permanent change of station order (OBBBA made this permanent and adds certain intelligence community members from 2026). *Correction:* the book applies the 50-mile and 39-week tests to military members (p. 374); those tests don't apply to Armed Forces members moving under PCS orders (IRS Publication 3). Deductible: moving and storing household goods, travel and lodging en route (not meals), driving at **21 cents a mile** for 2025 (the book says 22), utility connections, shipping cars and pets. Reimbursements shown in W-2 box 12 code P aren't taxable and reduce the deduction; reimbursements in box 1 wages don't.

**Form 8606** (pp. 376–379): Part I tracks **basis** (nondeductible contributions) in traditional IRAs. When you take money out or convert, the **pro-rata rule** applies: nontaxable fraction = total basis ÷ (year-end value of *all* traditional, SEP and SIMPLE IRAs + distributions + conversions). File 8606 every year you make a nondeductible contribution, even without a return to file ($50 penalty for failing to).

**Part II**: conversions — taxable amount = conversion − its basis share; flows to Form 1040 line 4b.

**Part III**: nonqualified Roth distributions come out in order: contributions (tax- and penalty-free), then conversions (tax-free; 10% penalty if within five years of that conversion and under 59½), then earnings (taxable, plus penalty unless excepted). A Roth distribution is **qualified** (entirely tax-free) after age 59½ (or death, disability, first home up to $10,000) and five years from the first Roth contribution.

## Sources

- Tyson and Eck (2025), pp. 374–380.
- IRS, [Publication 3 (2025), Armed Forces' Tax Guide](https://www.irs.gov/publications/p3); IRS, [Instructions for Form 8606 (2025)](https://www.irs.gov/instructions/i8606); IRS, [Publication 590-B](https://www.irs.gov/publications/p590b).
