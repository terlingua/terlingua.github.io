---
title: Credits, payments, the refund and the amount you owe
questions:
- id: full-return
  prompt: 'Single filer, 2025: wages (W-2 box 1) $80,000, interest $1,000, ordinary dividends $2,500 including $2,000 qualified, long-term capital gain $3,000, no adjustments, standard deduction. Federal withholding $9,500. Using 2025 rates, about what is the refund or balance due?'
  explanation: Total income = $80,000 + $1,000 + $2,500 + $3,000 = $86,500 = AGI. Taxable income = $86,500 − $15,750 = $70,750. Preferential income = $2,000 qualified dividends + $3,000 gain = $5,000; ordinary part = $65,750. Ordinary tax on $65,750 = $1,192.50 + $4,386 + 22% × ($65,750 − $48,475) = $9,379. Since $65,750 is above the 0% capital gain range, the $5,000 is taxed at 15% = $750. Total tax = $10,129. Withholding $9,500 → **balance due ≈ $629**. Taxing everything at ordinary rates would give $10,479 (a $979 balance due), so the worksheet saves $350. Under $100,000 the official Tax Table would shift these figures by a few dollars.
  options:
  - text: A refund of about $371
  - text: A balance due of about $979
  - text: A refund of about $9,500
  - text: A balance due of about $629 (total tax about $10,129)
    correct: true
- id: eitc-basics
  prompt: Which statements about the 2025 earned income credit are correct? Select all that apply.
  type: multiple
  explanation: 'The EIC is refundable: if it exceeds your tax “the IRS will send you a check” (p. 195). Maximum for 2025 is $8,046 with three or more children; childless workers must be 25–64 and not someone''s dependent (p. 195). Investment income must not exceed $11,950. Since 2021, a separated spouse living apart for the last six months of the year who has a qualifying child can claim the EIC even when filing separately. The IRS scrutinises EIC claims and may hold refunds that include the EIC or ACTC until mid-February.'
  options:
  - text: It is refundable
    correct: true
  - text: Married couples filing separately can never claim it
  - text: A worker with no qualifying children must generally be 25 to 64
    correct: true
  - text: Investment income above $11,950 disqualifies you
    correct: true
  - text: It can be as large as $15,000
- id: estimated-safe-harbor
  prompt: Last year your total tax was $20,000 and your AGI was $120,000. This year, thanks to a big bonus without enough withholding, your tax will be $35,000. How much must you have paid through withholding and estimates to avoid the underpayment penalty?
  explanation: The penalty doesn't apply if payments cover the smaller of 90% of this year's tax or 100% of last year's tax (110% if last year's AGI was over $150,000). Here 100% × $20,000 = $20,000 is far less than 90% × $35,000 = $31,500. The book mentions the $1,000 and 90% tests (pp. 195, 199); the prior-year safe harbor is what lets people with volatile income plan safely. You still owe the $15,000 balance by April 15, but without penalty. Form 2210's annualized method helps when income came late in the year.
  options:
  - text: $20,000 (100% of last year's tax), paid in timely instalments — the prior-year safe harbor
    correct: true
  - text: $31,500 (90% of this year's tax), no other option
  - text: $35,000
  - text: $1,000
- id: refund-options
  prompt: Which statements about receiving a 2025 refund are correct? Select all that apply.
  type: multiple
  explanation: The book explains Form 8888's split deposits and warns that to count as a 2025 IRA contribution, “your bank must receive the cash prior to the original filing deadline” (p. 198). An overpayment applied to 2026 can't be taken back; it becomes a credit on the 2026 return (p. 198). Separately, Executive Order 14247 (March 2025) directed Treasury to stop issuing paper checks, and the IRS began phasing out paper refund checks for individuals on September 30, 2025, so a return without bank details may face delays.
  options:
  - text: Applying an overpayment to next year's estimated tax can't be reversed after filing
    correct: true
  - text: A refund deposited to an IRA in May 2026 counts as a 2025 contribution
  - text: The IRS began phasing out paper refund checks for individuals on September 30, 2025
    correct: true
  - text: You can split a refund among up to three accounts with Form 8888, including an IRA or HSA
    correct: true
- id: ctc-phaseout
  prompt: A married couple filing jointly with two children under 17 has modified AGI of $430,000 in 2025. What is their child tax credit?
  explanation: The child tax credit is $2,200 per qualifying child for 2025 (p. 192), so $4,400. It phases out by $50 per $1,000 (or fraction) of modified AGI above $400,000 joint ($200,000 others). $30,000 over → 30 × $50 = $1,500 reduction → $2,900. The refundable portion (additional child tax credit) is up to $1,700 per child, limited to 15% of earned income above $2,500, but refundability matters only if tax is less than the credit.
  options:
  - text: $4,000
  - text: $4,400
  - text: $0
  - text: '$2,900 ($4,400 reduced by $50 for each $1,000 over $400,000: $1,500)'
    correct: true
- id: pay-electronically
  prompt: Which payment approach does the book suggest first for a balance due?
  explanation: The book lists, “in the order in which we tend to use them”, direct debit through the return (you choose the date, no later than April 15), IRS online payment, EFTPS (allow about two weeks to enroll), the IRS2Go app, check or money order with Form 1040-V, card (fees apply, not recommended) and cash (not recommended) (pp. 198–199). If you can't pay in full, file on time anyway and request an installment agreement (Chapter 18).
  options:
  - text: Paying by credit card for the rewards
  - text: Mailing cash
  - text: Waiting for a bill
  - text: Entering bank information in your tax software and scheduling an electronic withdrawal for any date up to the April 15 deadline
    correct: true
---
## Learning objectives

- Compute total tax (lines 16–24) and total payments (lines 25–33)
- Understand the earned income credit thresholds and refundable credits on lines 27–29
- Choose refund options, know about the end of paper refund checks, and pay a balance cheaply
- Apply the estimated tax penalty safe harbors

## The ideas

**Total tax** (pp. 192–193): line 16 tax + line 17 (AMT and excess APTC from Schedule 2) = line 18; minus line 19 (child tax credit and credit for other dependents from Schedule 8812: $2,200 per child under 17, $500 per other dependent, phasing out by $50 per $1,000 of MAGI above $200,000 / $400,000 joint) and line 20 (Schedule 3 nonrefundable credits) = line 22; plus line 23 (other taxes, Schedule 2 Part II) = **line 24, total tax**.

**Payments** (pp. 193–197): withholding by source (25a W-2s, 25b 1099s, 25c other); estimated payments and last year's overpayment applied (26); **earned income credit** (27; up to $8,046 with three children for 2025; income limits from $19,104 with no children up to $68,675 joint with three; investment income must not exceed $11,950); **additional child tax credit** (28; refundable part, up to $1,700 per child); refundable AOTC (29, up to $1,000); Schedule 3 Part II (31).

**Refund** (pp. 197–198): direct deposit (up to three accounts via Form 8888, including an IRA, HSA or Coverdell), or apply to next year's estimated tax (irrevocable). Under Executive Order 14247, the IRS began phasing out paper refund checks for individuals on September 30, 2025, so direct deposit information matters more than ever (this postdates the book).

**Balance due** (pp. 198–199): pay by electronic withdrawal when e-filing (you can schedule it up to the due date), Direct Pay, EFTPS, IRS2Go, card (fees), check with Form 1040-V, or cash at retail partners.

**Estimated tax penalty** (Form 2210; p. 199): you generally avoid it if you owe less than $1,000 after withholding and credits, or if withholding and timely estimates covered at least 90% of this year's tax or 100% of last year's tax (110% if last year's AGI exceeded $150,000, $75,000 MFS). The book states only the 90% test; the prior-year safe harbor is often the easier one to plan around. Withholding counts as paid evenly through the year, which makes a year-end W-4 change a useful fix.

## Sources

- Tyson and Eck (2025), pp. 192–200.
- IRS, [Topic 306, Penalty for underpayment of estimated tax](https://www.irs.gov/taxtopics/tc306); IRS, [Modernizing payments to and from America's bank account](https://www.irs.gov/ModernPayments); IRS, [EITC tables (2025)](https://www.irs.gov/credits-deductions/individuals/earned-income-tax-credit/earned-income-and-earned-income-tax-credit-eitc-tables).
