---
title: 'Self-employed adjustments: SE tax, retirement plans and health insurance'
questions:
- id: half-se
  prompt: Why can self-employed people deduct half of their self-employment tax?
  explanation: Employees pay 7.65% and employers pay 7.65%, which the employer deducts. The self-employed pay both (15.3%) “or a whopping 15.3 percent of all earnings from self-employment up to $176,100, and 2.9 percent on all earnings above that” (p. 133), and “You're allowed to deduct one half of your self-employment tax” (p. 133). SE tax is computed on 92.35% of net earnings (which mirrors excluding the employer's half from the base). The deduction reduces income tax, not SE tax itself.
  options:
  - text: Because SE tax is refundable
  - text: It is an itemized deduction available to all taxpayers
  - text: Because Congress forgot to apply it to the self-employed
  - text: Because SE tax includes both the employee and employer shares of FICA, and employers deduct their share as a business expense; the adjustment puts the self-employed on a similar footing
    correct: true
- id: sep-calc
  prompt: A sole proprietor has $100,000 of net profit in 2025. Her SE tax is about $14,130, so half is about $7,065. What is the maximum SEP-IRA contribution?
  explanation: 'For the self-employed, "compensation" is net profit minus half of SE tax minus the SEP contribution itself. Solving the circular definition turns a 25% plan rate into an effective 20%: the book''s Table 7-1 maps 25% to .200000 (pp. 136–137). Compute: $100,000 − $7,065 = $92,935; × 20% = $18,587. Using 25% of the gross profit ($25,000) is the classic overcontribution, which triggers a 6% excise tax each year it remains. The cap of $70,000 (2025) applies only above roughly $350,000 of profit, which is also the compensation limit (p. 136).'
  options:
  - text: About $18,587 (20% of $100,000 − $7,065)
    correct: true
  - text: $25,000 (25% of profit)
  - text: $70,000
  - text: $23,500
- id: solo401k
  prompt: A 52-year-old consultant with no employees has $180,000 of net profit in 2025 (half SE tax about $12,717). With a solo 401(k), roughly how much can she contribute in total?
  explanation: 'Employer share = 20% × ($180,000 − $12,717) ≈ $33,457. Employee deferral = $23,500, plus the $7,500 catch-up at 50+ (outside the $70,000 limit). Total ≈ $64,457; the non-catch-up part ($56,957) is under $70,000. A SEP alone would allow only the $33,457. The book''s version uses round numbers that ignore the SE tax adjustment ($36,000 employer share) and adds $23,500 + $7,500 + $36,000 to “$66,000” (p. 135), which is an arithmetic slip: it sums to $67,000. The idea is right: the solo 401(k) lets a modest-income owner shelter far more than a SEP, because the employee deferral isn''t tied to a percentage of profit.'
  options:
  - text: $77,500
  - text: $56,957
  - text: About $64,457 ($23,500 deferral + $7,500 catch-up + about $33,457 employer contribution)
    correct: true
  - text: $36,000
- id: shared-deferral
  prompt: Ana has a W-2 job with a 401(k) where she defers $23,500 in 2025, and a side business with a solo 401(k). How much can she defer as an *employee* into the solo 401(k)?
  explanation: “If you're both an employee and a sole proprietor, the most you can contribute to either your employer's 401(k) or the 401(k) feature of your Solo (k) is $23,500. Some taxpayers mistakenly believe they can contribute $23,500 to each 401(k)” (p. 135). The employer (profit-sharing) side of the solo 401(k) has its own $70,000-per-plan limit (for unrelated employers), so Ana can still contribute 20% of her net self-employment earnings. Excess deferrals must be withdrawn by April 15 to avoid double taxation.
  options:
  - text: Another $23,500
  - text: $7,000
  - text: $70,000
  - text: Nothing more; the $23,500 elective deferral limit is per person across all 401(k)s, though her business can still make an employer profit-sharing contribution
    correct: true
- id: sep-deadline
  prompt: It's March 2026. You're a sole proprietor with no retirement plan. Can you still make a deductible 2025 contribution?
  explanation: “You can set up your plan and make this contribution for 2025 up to the day that you file your income tax return in 2026, including extensions” (p. 134). Since the SECURE Act (2019), new profit-sharing plans can also be adopted after year-end up to the extended due date, so the book's statement that an HR10 (Keogh) plan “had to be set up by December 31, 2025” (p. 134) reflects the pre-2020 rule. Defined benefit plans have their own deadlines (minimum funding is due 8½ months after year-end, September 15 for calendar years, p. 137).
  options:
  - text: No; plans must be established by December 31
  - text: Yes; a SEP-IRA can be established and funded for 2025 up to the due date of your 2025 return, including extensions (October 15, 2026 if extended)
    correct: true
  - text: Only if you had employees
  - text: Only an IRA, not a SEP
- id: se-health
  prompt: Which conditions apply to the self-employed health insurance deduction? Select all that apply.
  type: multiple
  explanation: 'The deduction is 100% of premiums, taken on Schedule 1 line 17, “with one caveat: You may not deduct more than your net profit from your business” (p. 137). It covers you, your spouse and dependents (and children under 27), Medicare premiums, and qualified long-term care premiums within age-based limits. It''s disallowed for any month you could have joined an employer-subsidised plan (yours or your spouse''s), even if you didn''t. Premiums that don''t qualify can go to Schedule A as medical expenses. Form 7206 is used when the computation is complex.'
  options:
  - text: It can cover your spouse and dependents
    correct: true
  - text: It isn't allowed for months in which you were eligible for a subsidised employer health plan through your own job or your spouse's
    correct: true
  - text: It requires itemizing deductions
  - text: It can't exceed the net profit of the business under which the plan is established
    correct: true
- id: db-plan
  prompt: Why might a 58-year-old physician earning $500,000 choose a defined benefit plan over a SEP?
  explanation: 'With a defined benefit plan “you select an annual pension benefit that you want to receive at retirement”, and an actuary computes the annual contribution needed; for 2025 the benefit can be as high as $280,000 a year (p. 137). Because an older owner has fewer years to fund the benefit, the required (and deductible) contribution can be well above $100,000. Trade-offs: mandatory annual funding, actuarial and Form 5500 costs, and coverage of employees. Some owners pair a defined benefit plan with a solo 401(k).'
  options:
  - text: SEPs are illegal for physicians
  - text: Defined benefit plans have no administrative costs
  - text: A defined benefit plan's deductible contribution is set by an actuary to fund a target benefit, which can far exceed the $70,000 defined contribution limit for an older, high earner
    correct: true
  - text: Defined benefit contributions are optional every year
---
## Learning objectives

- Explain why half of self-employment tax is deductible
- Compute a SEP contribution correctly (the 20% effective rate)
- Compare SEP, SIMPLE, solo 401(k) and defined benefit plans
- Apply the self-employed health insurance deduction

## The ideas

**Half of SE tax** (line 15). The self-employed pay both halves of Social Security and Medicare (15.3% on 92.35% of net earnings up to the wage base, $176,100 for 2025; 2.9% above). Deducting half mirrors an employer's deduction for its share (p. 133).

**SEP-IRA** (p. 134). Contributions up to 25% of *compensation*; for a self-employed person, compensation is net profit minus half of SE tax minus the contribution itself, which makes the effective rate 20% of (net profit − half SE tax). Max $70,000 for 2025, $72,000 for 2026. Can be opened and funded until the extended due date. No catch-ups. Covered employees must get the same percentage.

**SIMPLE IRA / SIMPLE 401(k)**: employee deferrals of $16,500 for 2025 (+$3,500 at 50; $5,250 at 60–63; higher limits for employers with 25 or fewer employees), plus an employer match of up to 3% of pay or a 2% nonelective contribution (the book's “matching a like amount”, p. 134, overstates the employer's share).

**Solo 401(k)** (p. 135). For owners with no employees other than a spouse: an employee deferral ($23,500 for 2025, plus $7,500 catch-up at 50 or $11,250 at 60–63) *plus* the employer profit-sharing contribution (20% effective rate), up to $70,000 total excluding catch-ups. The deferral limit is shared with any other 401(k) you participate in as an employee.

**Defined benefit plans** (pp. 135–137). An actuary computes the contribution needed to fund a target benefit (up to $280,000 a year for 2025); older, high-income owners can deduct far more than $70,000, at the cost of administration and mandatory funding.

**Deadlines.** Under the SECURE Act a new SEP or profit-sharing plan (including a new solo 401(k)'s employer contribution) can be established for a year up to the return's due date including extensions. The book says a qualified plan must be set up by December 31 (p. 134), the old rule; since 2023, sole proprietors may even make first-year solo 401(k) employee deferrals up to the original filing date.

**Self-employed health insurance** (line 17, p. 137). 100% of premiums for you, your spouse and dependents (including Medicare and qualified long-term care premiums, within age-based limits), limited to net profit from the business, and not for any month you were eligible for a subsidised employer plan (yours or your spouse's). More-than-2% S corporation shareholders qualify if the premiums are run through their W-2.

## Sources

- Tyson and Eck (2025), pp. 133–137.
- IRS, [Publication 560](https://www.irs.gov/publications/p560) (retirement plans for small business); IRS, [Publication 535/Schedule 1 instructions on self-employed health insurance](https://www.irs.gov/forms-pubs/about-form-7206) (Form 7206).
