---
title: Self-employment tax, tips, misclassified wages and the nanny tax
questions:
- id: se-calc
  prompt: A freelancer has $50,000 of net profit for 2025 and no wages. What is the self-employment tax?
  explanation: 'SE tax applies to 92.35% of net profit: $50,000 × 0.9235 = $46,175. Rate 15.3% (12.4% Social Security + 2.9% Medicare): $46,175 × 0.153 ≈ $7,065. Half ($3,532) is deducted on Schedule 1. The 92.35% factor (= 1 − 7.65%) mimics the fact that an employee''s wages don''t include the employer''s FICA share. The book''s description of the rates is on p. 164; Chapter 17 covers Schedule SE.'
  options:
  - text: $6,200
  - text: $3,825
  - text: About $7,065
    correct: true
  - text: $7,650
- id: se-wage-base
  prompt: In 2025 Ravi earns $150,000 in W-2 wages and $60,000 of net self-employment profit. How is the Social Security part of his SE tax limited?
  explanation: The Social Security wage base ($176,100 for 2025) is shared between wages and self-employment earnings; W-2 Social Security wages count first. Net SE earnings = $60,000 × 0.9235 = $55,410. Remaining room = $176,100 − $150,000 = $26,100, so Social Security SE tax = 12.4% × $26,100 = $3,236; Medicare = 2.9% × $55,410 = $1,607; total ≈ $4,843. The book notes that above $176,100, “the rate is 2.9 percent” (p. 164). Additional Medicare Tax may also apply above $200,000 of combined earnings.
  options:
  - text: The 12.4% Social Security tax applies only to self-employment earnings up to the remaining room under the $176,100 wage base ($26,100), while the 2.9% Medicare tax applies to all $55,410 of net SE earnings
    correct: true
  - text: The wage base doesn't apply to self-employment
  - text: He pays no SE tax because he has wages
  - text: He pays 15.3% on all $60,000
- id: form-8919
  prompt: You worked full-time at a company that paid you on a 1099-NEC, but it controlled your hours, tools and methods, and you've filed Form SS-8. Which form lets you pay only the employee share of FICA?
  explanation: 'Form 8919 is for workers who believe they were employees but were treated as contractors. You report the pay as wages and pay 7.65%, not 15.3% (p. 165). The book: “you get yourself off the hook for the other half of the contribution”. You need a qualifying reason, such as having filed Form SS-8 (a determination request) or having received an IRS determination. The employer may face back payroll taxes. Factors that suggest employee status: behavioural control, financial control and the relationship''s permanence.'
  options:
  - text: Schedule SE
  - text: Form 4137
  - text: Form W-4
  - text: Form 8919, Uncollected Social Security and Medicare Tax on Wages
    correct: true
- id: nanny-tax
  prompt: In 2025 you paid a nanny $20,000 in cash wages. What do you owe?
  explanation: The household employment tax applies when cash wages to a household employee reach $2,800 in 2025; FUTA applies when you paid $1,000 or more in any calendar quarter (p. 166). You file Schedule H with your Form 1040, give her a W-2 and file W-3 with the Social Security Administration. Income tax withholding is optional unless she asks. You can pay through increased withholding or estimated taxes to avoid an underpayment penalty. Childcare paid this way can still count toward the dependent care credit or FSA.
  options:
  - text: Nothing, because household workers are contractors
  - text: Only income tax withholding
  - text: Social Security and Medicare on her wages (7.65% employer share plus her 7.65% share, which you may withhold), plus federal unemployment tax, reported on Schedule H
    correct: true
  - text: Nothing unless she earns more than $25,000
- id: unreported-tips
  prompt: A bartender received $6,000 of cash tips in 2025 but reported only $2,000 to his employer. What must he do?
  explanation: All tips are taxable income. Tips not reported to the employer escaped FICA withholding, so Form 4137 computes the employee share (7.65%) on them (p. 165). The OBBBA tips deduction (up to $25,000, 2025–2028) can then reduce income tax on qualified tips, but it doesn't remove FICA, and it requires that tips be properly reported. Tips of less than $20 a month at one job don't have to be reported to the employer, though they're still income.
  options:
  - text: Nothing; tips under $10,000 are exempt
  - text: Report all $6,000 as income and use Form 4137 to pay the employee's Social Security and Medicare tax on the $4,000 not reported to the employer
    correct: true
  - text: Pay 15.3% on $4,000 as self-employment tax
  - text: Report only $2,000, because that's what the W-2 shows
---
## Learning objectives

- Compute self-employment tax
- Know how unreported tips and uncollected FICA are reported
- Use Form 8919 when misclassified as a contractor
- Apply the household employment tax thresholds

## The ideas

**Self-employment tax** (Schedule SE; p. 164). Net earnings from self-employment = 92.35% of net profit. Social Security portion: 12.4% up to the wage base ($176,100 for 2025, reduced by any W-2 Social Security wages); Medicare: 2.9% on all of it. No SE tax if net earnings are under $400. Half is deductible (Chapter 7).

Example: $50,000 net profit → $50,000 × 0.9235 = $46,175 → × 15.3% = $7,065 of SE tax; deduction $3,532.

**Tips** (line 5; Form 4137): tips you didn't report to your employer (and allocated tips) owe the employee share of FICA. Uncollected FICA on reported tips (W-2 box 12 codes A and B) is added on Schedule 2 (p. 165).

**Misclassified workers** (line 6; Form 8919): if you were treated as a contractor but were really an employee, you pay only the employee half (7.65%) instead of 15.3% SE tax, and your wages are credited to your Social Security record (p. 165).

**Household employment ("nanny tax")** (line 9; Schedule H): if you paid a household employee $2,800 or more of cash wages in 2025, you owe Social Security and Medicare (both shares, unless you withheld the employee's share); if you paid $1,000 or more in any calendar quarter of 2024 or 2025, also federal unemployment tax (p. 166). The 2026 threshold is $3,000. Wages to your child under 21, your spouse, or your parent (generally) and to employees under 18 who aren't primarily household workers are exempt.

## Sources

- Tyson and Eck (2025), pp. 164–166.
- IRS, [Topic 554, Self-employment tax](https://www.irs.gov/taxtopics/tc554); IRS, [Publication 926 (2025)](https://www.irs.gov/publications/p926) (household employer's tax guide).
