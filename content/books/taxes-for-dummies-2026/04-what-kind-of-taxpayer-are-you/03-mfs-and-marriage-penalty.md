---
title: Married filing separately, the marriage penalty and community property
questions:
- id: mfs-why
  prompt: In which situation is married filing separately most likely to save a couple money?
  explanation: 'Deductions with a floor based on AGI work better against a smaller AGI. Example: combined AGI $150,000; one spouse''s AGI $50,000; that spouse''s medical bills $12,000. Jointly, only $12,000 − 7.5% × $150,000 = $750 is deductible. Separately, $12,000 − 7.5% × $50,000 = $8,250. The book''s Table 4-1 makes the same point with a casualty loss and its 10%-of-AGI floor (pp. 50–51), but under current law personal casualty losses are deductible only in declared disasters, so medical expenses are the more common case. MFS must beat the loss of joint brackets and the many MFS restrictions, so compute both ways.'
  options:
  - text: Both spouses have income, and one has very large medical expenses that exceed 7.5% of his or her own AGI but not 7.5% of their combined AGI
    correct: true
  - text: Both spouses have equal wages and no deductions
  - text: The couple wants to claim the earned income credit
  - text: One spouse earns all the income and the other has none
- id: mfs-restrictions
  prompt: Which of these are true for married couples filing separately for 2025? Select all that apply.
  type: multiple
  explanation: 'All but the last are on the book''s list (pp. 49–50). The Roth conversion item is out of date: the old rule barring MFS filers (and anyone with modified AGI over $100,000) from converting was repealed for 2010 and later, so anyone may convert regardless of filing status. Other MFS restrictions include the child and dependent care credit (generally unavailable), the earned income credit (generally unavailable, with a 2021-onward exception for separated spouses with a qualifying child), reduced IRA deductibility if either spouse is covered by a workplace plan, and Social Security taxation from the first dollar if the spouses lived together during the year.'
  options:
  - text: They cannot claim the American opportunity or lifetime learning credits
    correct: true
  - text: If one spouse itemizes, the other must itemize too
    correct: true
  - text: The capital loss deduction is limited to $1,500 each
    correct: true
  - text: They cannot convert a traditional IRA to a Roth IRA
  - text: They cannot deduct student loan interest
    correct: true
- id: mfs-protect
  prompt: Besides saving tax, what is a non-tax reason to file separately?
  explanation: “Another reason you may choose to file separately is to avoid being responsible for your spouse's share of the joint tax bill whenever you suspect some kind of monkey business” (p. 49). On a joint return both spouses are jointly and severally liable; on separate returns each is liable only for his or her own. “This is a case where paying a little more upfront can save you a whole lot down the road.” Innocent spouse relief exists but is harder to get than simply not signing a joint return.
  options:
  - text: To get a larger standard deduction
  - text: To qualify for the earned income credit
  - text: To avoid being liable for tax arising from a spouse's income or questionable return, for example when you suspect underreporting or the marriage is ending
    correct: true
  - text: To speed up a refund
- id: marriage-penalty-calc
  prompt: Two people each have taxable income of $700,000 in 2025. If they marry, their combined tax rises. Using the 2025 brackets (single 37% above $626,350; joint 37% above $751,600, all lower joint brackets exactly double), how large is the marriage penalty from the brackets?
  explanation: 'Up to the 35% bracket, the joint thresholds are exactly double the single ones, so marriage changes nothing there. At the top they diverge: two singles each reach 37% only above $626,350 (combined $1,252,700), but a joint return reaches 37% above $751,600. So $1,252,700 − $751,600 = $501,100 of their combined income is taxed at 37% instead of 35%: 2% × $501,100 = $10,022. The book''s sidebar explains the mechanism — “the highest income bracket is not double that of filing single” — but cites the older $693,750 threshold (p. 53). Many non-bracket items (capital loss limit, Social Security thresholds, some phase-outs) also create penalties at lower incomes.'
  options:
  - text: About $10,000
    correct: true
  - text: About $50,000
  - text: Zero, because joint brackets are double
  - text: $2,000
- id: marriage-bonus
  prompt: Pat has $100,000 of taxable income and Chris has none. Using 2025 brackets, what happens to their income tax if they marry and file jointly?
  explanation: 'Single: 10% × $11,925 + 12% × $36,550 + 22% × $51,525 = $1,192.50 + $4,386 + $11,335.50 = $16,914. Joint: 10% × $23,850 + 12% × $73,100 + 22% × $3,050 = $2,385 + $8,772 + $671 = $11,828. Bonus ≈ $5,086 (and in reality taxable income would also change, because the joint standard deduction is $31,500 versus $15,750). The book: most couples find their joint bill is less, “when one spouse doesn''t earn any income or when both partners have a modest income” (p. 53). Two equal earners at moderate incomes see no change at all: two singles at $80,000 pay the same as a couple at $160,000.'
  options:
  - text: It is unchanged because brackets are double
  - text: It falls to zero
  - text: It falls from about $16,914 (Pat single) to about $11,828 (joint), a marriage bonus of roughly $5,000
    correct: true
  - text: It rises because of the marriage penalty
- id: community-property
  prompt: A married couple in California files separately. One spouse earned $120,000 of wages; the other earned nothing. Generally, how is the wage income reported?
  explanation: 'In the nine community property states (Arizona, California, Idaho, Louisiana, Nevada, New Mexico, Texas, Washington and Wisconsin), income earned during the marriage is generally owned half by each spouse, so “each spouse, as a general rule, must report one-half of the joint income” on separate returns (p. 51). Exceptions: spouses living apart all year who don''t transfer earnings between them, and income one spouse concealed (the other needn''t report it). Withholding credits follow the same split. For community property states MFS rarely saves federal tax for this reason. IRS Publication 555 covers the details.'
  options:
  - text: All $120,000 on the non-earner's return
  - text: Generally $60,000 on each return, because wages earned during marriage are community income split equally
    correct: true
  - text: Wages are excluded in community property states
  - text: All $120,000 on the earner's return
- id: divorce-december
  prompt: A couple divorces in December purely to file as singles, then remarries in January. What does the book say?
  explanation: '“You can''t divorce in December just to save on your taxes and then remarry the next year. Taxpayers who''ve tried this scam in the past have been slapped with penalties in addition to the extra taxes” (p. 54). The IRS disregards year-end divorces followed by remarriage as shams (Revenue Ruling 76-255). The book also points out a non-tax cost of not marrying: an unmarried partner has no right to spousal or survivor Social Security benefits based on the other''s record.'
  options:
  - text: It works if they live apart for one night
  - text: The IRS treats such sham divorces as ineffective; taxpayers have been assessed the extra tax plus penalties
    correct: true
  - text: It is required to avoid the marriage penalty
  - text: It is a legitimate planning strategy
---
## Learning objectives

- Explain when MFS can pay and why it usually doesn't
- List the main restrictions on MFS filers, and correct the book's list
- Compute a marriage penalty and a marriage bonus
- Explain community property reporting

## The ideas

**Why MFS?** Usually it costs more. It can help when one spouse has large deductions limited by a percentage of AGI (medical expenses above 7.5% of AGI are the main current example), when you want to avoid liability for a spouse's dubious return, or for state tax reasons (pp. 48–49). Compute it both ways.

**MFS restrictions** (pp. 49–50): if one spouse itemizes, the other must too; generally no child and dependent care credit (dependent care FSA exclusion halved to $2,500 for 2025); no adoption credit unless living apart; no earned income credit (the book says flatly “You can't”; since 2021 a separated spouse living apart from the other for the last six months who has a qualifying child can claim it); no education credits or student loan interest deduction; no savings-bond education exclusion; reduced or no IRA deduction if the other spouse has a workplace plan; Social Security benefits taxable from the first dollar if you lived together at any time in the year; no $25,000 rental loss allowance if you lived together; capital loss limit $1,500. *Correction:* the book says MFS filers “can't transfer funds from a traditional IRA to a Roth IRA” (p. 50). That restriction ended in 2010; anyone may convert regardless of filing status or income.

**Marriage penalty and bonus.** A penalty arises when spouses pay more together than they would as two singles; a bonus when they pay less (p. 53). Bonuses are common when one spouse earns most of the income. For 2025 all brackets up to 35% are exactly double the single brackets for joint filers, so penalties from the brackets arise only at the top: the joint 37% bracket starts at $751,600, not double the single $626,350. Many other provisions also don't double (capital loss limit, Social Security thresholds, some phase-outs). (The book's sidebar uses older figures — a $693,750 threshold and a $10,000 SALT cap — both superseded for 2025.)

**Community property states** — Arizona, California, Idaho, Louisiana, Nevada, New Mexico, Texas, Washington and Wisconsin — generally require each spouse filing separately to report half the community income (p. 51; IRS Publication 555). Exceptions exist for spouses living apart all year and for income a spouse hid.

## Sources

- Tyson and Eck (2025), pp. 48–54.
- IRS, [Publication 504](https://www.irs.gov/publications/p504) (divorced or separated) and [Publication 555](https://www.irs.gov/publications/p555) (community property); IRS, [Roth IRA conversions](https://www.irs.gov/retirement-plans/plan-participant-employee/rollovers-of-retirement-plan-and-ira-distributions).
