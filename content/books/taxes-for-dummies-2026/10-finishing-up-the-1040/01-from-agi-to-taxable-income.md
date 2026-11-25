---
title: From AGI to taxable income
questions:
- id: agi-flow
  prompt: Which sequence correctly describes Form 1040's path to taxable income?
  explanation: 'The book walks through lines 10 to 15: adjustments from Schedule 1 on line 10, AGI on line 11, the standard or itemized deduction on line 12, the QBI deduction on line 13, the total on line 14, and taxable income on line 15 (pp. 183–186). The 2025 form inserted the Schedule 1-A deductions alongside QBI. Credits come later, against tax, not income. Withholding is a payment, near the end.'
  options:
  - text: AGI → plus adjustments → total income → taxable income
  - text: Gross wages → minus taxes withheld → taxable income
  - text: Total income → minus credits → AGI → minus deductions → taxable income
  - text: Total income → minus adjustments → AGI → minus standard or itemized deduction, QBI deduction and Schedule 1-A deductions → taxable income
    correct: true
- id: mfs-itemize-first
  prompt: A separated couple files separately for 2025 without talking to each other. One spouse files first and itemizes. What does that mean for the other?
  explanation: “Both of you must choose to use either itemized deductions or the standard deduction ... the first return filed will determine whether the second return filed uses the standard deduction or itemizes” (p. 185). A spouse who qualifies as head of household (living apart for the last six months with a qualifying child) is treated as unmarried and can take the HOH standard deduction. Otherwise, the second filer's standard deduction is zero, a costly surprise if he has few itemized deductions.
  options:
  - text: They must file jointly
  - text: The second spouse gets double the standard deduction
  - text: Nothing; each chooses independently
  - text: The other spouse must also itemize (the standard deduction becomes zero), unless he or she qualifies to file as head of household
    correct: true
- id: qbi-basics
  prompt: A sole proprietor (not a specified service business) has $80,000 of qualified business income and $100,000 of taxable income before the QBI deduction (no capital gains). What is her QBI deduction for 2025?
  explanation: 'Below the threshold ($197,300 single for 2025), the deduction is generally 20% of QBI, limited to 20% of taxable income (before the deduction) minus net capital gain: 20% × $80,000 = $16,000; 20% × $100,000 = $20,000; the lesser is $16,000. The wage and property limits mentioned in the book (p. 186) apply only above the threshold, so she doesn''t need employees. QBI is net profit reduced by the deductible part of SE tax, self-employed health insurance and retirement contributions attributable to the business, which the simple example ignores.'
  options:
  - text: $20,000
  - text: $8,000
  - text: $16,000
    correct: true
  - text: Zero, because she has no employees
- id: sec199a-dividends
  prompt: Your 1099-DIV shows $1,000 in box 5 (section 199A dividends) from a REIT fund. What does that mean?
  explanation: The book notes the deduction “is also available to shareholders of some dividend-paying corporations. If you receive a Form 1099-DIV that lists a number in box 5 for Section 199A dividends, put that box 5 number here” (p. 185). REIT ordinary dividends usually aren't qualified dividends (they're taxed at ordinary rates), so the 20% deduction gives an effective top rate of 29.6% instead of 37%. A holding period of more than 45 days applies.
  options:
  - text: They must be reported on Schedule C
  - text: They're qualified dividends taxed at 15%
  - text: They're tax-free
  - text: Those REIT dividends qualify for the 20% QBI deduction, so $200 is deductible on Form 8995 (if you held the shares more than 45 days)
    correct: true
---
## Learning objectives

- Trace lines 9 through 15: total income, adjustments, AGI, deductions, taxable income
- Know the MFS itemizing rule and age/blindness additions
- Explain the qualified business income deduction at a high level
- Place the new Schedule 1-A deductions correctly

## The ideas

**The flow.** Total income (line 9) − adjustments (Schedule 1 Part II → line 10) = **AGI** (line 11; lines 11a/11b on the final 2025 form). AGI − standard or itemized deduction − QBI deduction − Schedule 1-A deductions = **taxable income** (line 15), never below zero (pp. 183–186).

**Standard deduction 2025**: $15,750 single and MFS, $31,500 joint, $23,625 HOH, plus $2,000 (single/HOH) or $1,600 (married, per person) for being 65 or older or blind (p. 185). If one spouse filing separately itemizes, the other must too; whoever files first effectively decides. A married person who qualifies as HOH is treated as unmarried and can take the standard deduction regardless (p. 185).

**Qualified business income (QBI) deduction** (section 199A; Forms 8995/8995-A; pp. 185–186): generally 20% of qualified business income from sole proprietorships, partnerships and S corporations, plus 20% of REIT dividends (1099-DIV box 5) and publicly traded partnership income, limited to 20% of taxable income minus net capital gain. Above a threshold ($197,300 single, $394,600 joint for 2025), wage and property limits phase in, and specified service businesses (law, health, consulting, finance and others whose principal asset is the owners' skill or reputation) phase out. Losses net against profits and carry forward. OBBBA made the deduction permanent, widened the phase-in ranges from 2026 and added a $400 minimum deduction for taxpayers with at least $1,000 of QBI from active businesses.

**Schedule 1-A (new for 2025)**: the deductions for qualified tips, qualified overtime, car loan interest and the senior deduction are below AGI but available whether or not you itemize, on line 13b (after the book went to print; the book's line references predate it).

## Sources

- Tyson and Eck (2025), pp. 183–186.
- IRS, [Instructions for Form 1040 (2025)](https://www.irs.gov/pub/irs-pdf/i1040gi.pdf); IRS, [Qualified business income deduction](https://www.irs.gov/newsroom/qualified-business-income-deduction).
