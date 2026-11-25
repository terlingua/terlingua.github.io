---
title: The traditional IRA deduction, spousal IRAs and the backdoor Roth
questions:
- id: ira-compensation
  prompt: Which people can make a 2025 IRA contribution based on their own income? Select all that apply.
  type: multiple
  explanation: You need “taxable earned income from wages, commissions, tips, self-employment, nontaxable combat pay, or the receipt of alimony that is taxable to you” (p. 142). Unemployment “is considered unearned income”, and pensions and Social Security aren't compensation. Since 2020 there's no maximum age (p. 143). The contribution can't exceed compensation, so the freelancer's limit is effectively her net SE earnings after the SE tax deduction. A retiree with a working spouse could use a spousal IRA instead.
  options:
  - text: A retiree whose only income is a $40,000 pension and Social Security
  - text: A person whose only income is $20,000 of unemployment benefits
  - text: A freelancer with $5,000 of net self-employment income (limited to her compensation)
    correct: true
  - text: A 75-year-old working part-time who earned $9,000 in wages
    correct: true
  - text: A recipient of taxable alimony under a 2016 decree
    correct: true
- id: ira-phaseout
  prompt: Single, covered by a 401(k) at work, MAGI of $84,000 in 2025, age 40. How much of a $7,000 traditional IRA contribution is deductible?
  explanation: 'The 2025 phase-out range for a covered single filer is $79,000 to $89,000 (p. 146). Deduction = $7,000 × ($89,000 − $84,000) ÷ $10,000 = $7,000 × 50% = $3,500. The remaining $3,500 is a nondeductible contribution recorded on Form 8606. The book''s worksheet multiplies the $5,000 remainder by 30%, giving $1,500 (p. 147); that factor belongs to an era of smaller contribution limits. A quick way to check: halfway through the range, half the limit is deductible.'
  options:
  - text: $7,000
  - text: $0
  - text: $1,500
  - text: $3,500
    correct: true
- id: spouse-covered
  prompt: Jordan has no workplace plan; Jordan's spouse is covered by a 401(k). They file jointly with MAGI of $200,000 in 2025. Can Jordan deduct a $7,000 IRA contribution?
  explanation: 'Table 7-2: with one spouse covered, the contribution is fully deductible “Up to $236,000 ... for spouse whose employer doesn''t have a plan” (p. 143). The covered spouse''s own deduction uses the much lower $126,000–$146,000 range, so at $200,000 the covered spouse can''t deduct an IRA contribution, but Jordan can. Jordan needn''t have earnings personally: joint filers can base a spousal IRA on the couple''s combined compensation (p. 147).'
  options:
  - text: Yes, fully, because the phase-out for a non-covered spouse of a covered person is $236,000–$246,000 of joint MAGI
    correct: true
  - text: No, because the spouse is covered
  - text: Only $3,500
  - text: Only if Jordan earns at least $7,000 personally
- id: covered-definition
  prompt: When are you an "active participant" in a workplace retirement plan for IRA deduction purposes?
  explanation: “You're considered covered, or an active participant, even if you haven't earned the full right (known as vesting) to the benefits under your plan” (p. 144). For 401(k)-type plans, if “you and your employer didn't add any money to your account and no forfeitures were allocated during the tax year, you're not considered covered”. Being covered for even part of the year (for example, one of two employers had a plan) makes you covered for the year. Self-employed people with their own SEP, SIMPLE or Keogh are covered too.
  options:
  - text: Only if you are fully vested
  - text: Only if you contributed at least $5,000
  - text: Whenever your employer offers any plan, even if you never joined and nothing was added
  - text: When the W-2 "Retirement plan" box is checked — for a 401(k), generally when contributions or forfeitures were added to your account for the year; for a defined benefit plan, when you're eligible, even if not vested
    correct: true
- id: excess-ira
  prompt: In 2025 you contributed $8,000 to your IRA but were only allowed $7,000 (you're 45). It's March 2026. What are your options to avoid the 6% excise tax?
  explanation: 'The book''s options: apply the excess to next year''s limit (“This way works best”), or withdraw the excess and its earnings by the due date (including extensions); the earnings are taxable (and subject to the 10% tax if under 59½) (p. 145). Otherwise a 6% excise tax applies every year the excess stays in the account, reported on Form 5329. Excess contributions often happen when someone with only part-year earnings, or who was over the Roth income limit, contributes the full amount.'
  options:
  - text: Nothing; the 6% is unavoidable
  - text: Leave it; excess contributions are simply nondeductible
  - text: Withdraw only the $1,000, ignoring earnings, any time in 2027
  - text: Withdraw the $1,000 excess plus its earnings by the extended due date of your 2025 return, or treat the $1,000 as a 2026 contribution
    correct: true
- id: backdoor-roth
  prompt: Taylor, single, has $200,000 of MAGI in 2025 and no other traditional IRA money. She makes a $7,000 nondeductible traditional IRA contribution and converts it to a Roth a week later, when it's worth $7,005. What is taxable?
  explanation: 'Taylor is above the $165,000 single limit for direct Roth contributions, but conversions have no income limit. She has $7,000 of basis (Form 8606), so only the $5 of earnings is taxable. The book describes this "back-door" strategy (pp. 148–149), though it attributes the removal of conversion income limits to the SECURE Act of 2019; the income limit on conversions actually disappeared in 2010. The trap is the pro-rata rule: if she also had $93,000 of pre-tax IRA money, only 7% of the conversion would be tax-free.'
  options:
  - text: $7,000
  - text: $5 (the earnings); the $7,000 of basis converts tax-free
    correct: true
  - text: $7,005
  - text: Nothing, but the conversion is illegal because her income exceeds the Roth limit
- id: recharacterize
  prompt: You converted a traditional IRA to a Roth in 2025, then the market dropped. Can you undo (recharacterize) the conversion?
  explanation: “Recharacterizations were eliminated beginning on January 1, 2018” for conversions (p. 145). You can still recharacterize a regular annual *contribution* (for example, switch a Roth contribution to a traditional one) by the extended due date. Because conversions are now irrevocable, people often convert in tranches over several years or late in a year when income is known.
  options:
  - text: Only with IRS permission
  - text: No; recharacterizing Roth conversions has been prohibited since 2018
    correct: true
  - text: Yes, until October 15, 2026
  - text: Yes, but only if you're under 50
---
## Learning objectives

- Determine who can contribute (compensation) and how much
- Determine deductibility based on workplace plan coverage and MAGI, with correct phase-out arithmetic
- Handle spousal IRAs, nondeductible contributions, excess contributions and the backdoor Roth

## The ideas

**Who can contribute.** Anyone with taxable compensation (wages, self-employment income, taxable alimony, nontaxable combat pay), or a spouse of such a person filing jointly. There is no age limit since 2020 (p. 143). Unemployment, pensions and investment income don't count (p. 142).

**Limits for 2025**: $7,000, plus $1,000 at 50 or older, or 100% of compensation if less — combined across traditional and Roth IRAs (p. 144). Contributions for 2025 can be made until April 15, 2026. (2026: $7,500 plus $1,100.)

**Deductibility (2025)** depends on whether you or your spouse is an "active participant" in a workplace plan (W-2 box 13) (pp. 143–146):

| Situation | Full deduction up to | No deduction at |
|---|---|---|
| Single/HOH, covered | $79,000 | $89,000 |
| Joint, you covered | $126,000 | $146,000 |
| Joint, you not covered but spouse covered | $236,000 | $246,000 |
| MFS, covered (lived together) | — | $10,000 |
| Neither covered | always deductible | — |

**Phase-out arithmetic.** Deduction = limit × (top of range − MAGI) ÷ width of range, rounded up to the next $10 (minimum $200 if not zero). A single filer at $84,000: $7,000 × $5,000 ÷ $10,000 = $3,500. *Correction:* the book's worksheet multiplies the remaining amount by 30% (35% if 50+) (p. 147); with today's $7,000 limit and a $10,000 range the factor is 70% (80% at 50+), and for joint ranges of $20,000 it's 35% (40%).

**Spousal IRA.** A nonworking spouse filing jointly can contribute up to the limit based on the working spouse's compensation; the couple's total can't exceed their combined compensation (p. 148). (The book's Michael and Lisa example computes Lisa's deemed compensation as $28,000 − $7,000 = “$21,500”; it's $21,000, which doesn't change their result.)

**Nondeductible contributions** are reported on Form 8606, which tracks basis forever ($50 penalty for not filing it) (p. 144). **Excess contributions** cost 6% a year until fixed: withdraw the excess plus earnings by the extended due date, or apply it to next year's limit (p. 145).

**Backdoor Roth** (pp. 148–149). High earners above the Roth income limits (2025: $165,000 single, $246,000 joint) contribute nondeductibly to a traditional IRA and convert to a Roth. Conversions have had no income limit since 2010 (the book attributes the change to the SECURE Act of 2019, which instead removed the age limit for contributions). The pro-rata rule (Chapter 5) taxes part of the conversion if you hold other pre-tax IRA money. Recharacterizing a conversion has been banned since 2018 (p. 145).

## Sources

- Tyson and Eck (2025), pp. 141–149.
- IRS, [Publication 590-A (2025)](https://www.irs.gov/publications/p590a); IRS, [2025 IRA deduction limits](https://www.irs.gov/retirement-plans/plan-participant-employee/2025-ira-deduction-limits-effect-of-modified-agi-on-deduction-if-you-are-covered-by-a-retirement-plan-at-work).
