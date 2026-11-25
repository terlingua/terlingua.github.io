---
title: 'Social Security benefits: how much is taxable'
questions:
- id: provisional-single
  prompt: Single, 70, with $30,000 of pension and interest income (no tax-exempt interest) and $20,000 of Social Security benefits. How much of the benefits is taxable?
  explanation: 'Provisional income = $30,000 + 50% × $20,000 = $40,000. Above $34,000, so both tiers apply. Tier 1: the smaller of 50% of benefits ($10,000), half of ($40,000 − $25,000) = $7,500, or the $4,500 single cap → $4,500. Tier 2: 85% × ($40,000 − $34,000) = $5,100. Sum = $9,600, which is less than 85% of benefits ($17,000). So $9,600 goes on line 6b and $20,000 on line 6a. The worksheet structure is the book''s Worksheet II (pp. 104–105). Note how each extra $1,000 of pension in this range makes $850 more of benefits taxable — an effective marginal rate well above the bracket rate.'
  options:
  - text: $17,000
  - text: $4,500
  - text: $9,600
    correct: true
  - text: $0
- id: book-ws1
  prompt: Using the book's first example — married filing jointly, $30,000 of other income, $16,000 of Social Security — how much is taxable?
  explanation: 'Base income = $30,000 + 50% × $16,000 = $38,000. That exceeds $32,000 by $6,000; half of that is $3,000. Compare with half the benefits ($8,000) and take the smaller: $3,000 (pp. 103–104). Because $38,000 is below $44,000, the 85% tier doesn''t apply. If the couple shifted $6,000 of taxable income into tax-free munis, it would not help: tax-exempt interest is added back into base income.'
  options:
  - text: $13,600
  - text: $3,000
    correct: true
  - text: $8,000
  - text: $0
- id: muni-not-shelter
  prompt: A retiree moves money from taxable bonds into municipal bonds to reduce the taxation of her Social Security. Does it work?
  explanation: 'The base income worksheet adds “Tax-exempt interest income” to the total (p. 103), and the book''s Worksheet II example includes $6,000 of tax-exempt interest. Switching to munis does avoid tax on the interest itself, which may still be worthwhile, but it doesn''t shrink provisional income. What does reduce it: Roth IRA withdrawals (excluded), qualified charitable distributions from IRAs (excluded from income), and timing of capital gains.'
  options:
  - text: No; tax-exempt interest is added to provisional income, so it still pushes benefits into the taxable range (though the interest itself remains untaxed)
    correct: true
  - text: Yes, but only above $44,000
  - text: No, because munis are taxable for retirees
  - text: Yes; muni interest is excluded from all calculations
- id: mfs-social-security
  prompt: A married couple lives together but files separately. One spouse receives $20,000 of Social Security and has $5,000 of other income. How much of that spouse's benefits can be taxable?
  explanation: 'For MFS filers who lived with their spouse at any time during the year, the base amount is zero (pp. 104, 107). Provisional income = $5,000 + $10,000 = $15,000, all above zero, so the formula yields 85% × $15,000 = $12,750, capped at 85% of benefits ($17,000): $12,750 taxable. Had they lived apart all year, the $25,000 single thresholds would apply and nothing would be taxable. The book: “living apart meant separate residences, not separate bedrooms” (p. 107).'
  options:
  - text: Up to 85%, because the base amount is $0 for a married person filing separately who lived with the spouse at any time during the year
    correct: true
  - text: None, because provisional income is under $25,000
  - text: None; MFS filers are exempt from Social Security tax
  - text: Exactly 50%
- id: earnings-test
  prompt: You claimed Social Security at 63 and earn $33,400 from a part-time job in 2025. What happens under the earnings test?
  explanation: 'For 2025, beneficiaries under full retirement age lose $1 for every $2 of earnings above $23,400 (p. 102). ($33,400 − $23,400) ÷ 2 = $5,000. In the year you reach full retirement age, the rate is $1 for $3 above $62,160, counting only earnings before the month you reach it; from then on there is no limit. The book calls this a "giveback", but it is closer to a deferral: SSA recalculates your benefit at full retirement age to add back months in which benefits were withheld. Only earned income counts; pensions and investment income don''t.'
  options:
  - text: Your benefits become fully taxable
  - text: $5,000 of benefits is withheld ($1 for every $2 above $23,400), and your benefit is recalculated upward at full retirement age to credit the withheld months
    correct: true
  - text: Nothing; the earnings test was repealed
  - text: You lose $10,000 of benefits permanently
- id: lump-sum-ss
  prompt: In 2025 you receive a $20,000 lump-sum Social Security payment, $10,000 of which is for 2023, when your income was much lower. What does the lump-sum election allow?
  explanation: You can figure the taxable part of the prior-year portion using that year's income, then add it to the current year's taxable benefits; “you don't file an amended return for that year” (p. 106). In the book's example, reporting everything in 2025 makes $17,000 taxable (85%), but the election reduces it to $13,500 ($5,000 for the 2023 portion at 50%, plus $8,500 for the 2025 portion). IRS Publication 915 has the worksheets.
  options:
  - text: Amending the 2023 return to report it
  - text: Spreading the $20,000 over ten years
  - text: Excluding the back benefits entirely
  - text: Computing the taxable part of the $10,000 as if received in 2023 (using 2023 income), and adding that amount to 2025 income — without amending the 2023 return
    correct: true
---
## Learning objectives

- Compute provisional (base) income and apply the 50% and 85% tiers
- Work both worksheets with numbers
- Explain the MFS trap, lump-sum elections and the earnings test

## The ideas

**Provisional income** = other income (AGI without Social Security) + tax-exempt interest + certain exclusions + **half** of Social Security benefits (pp. 103–104).

**Tiers.** Below $25,000 (single) or $32,000 (joint), none is taxable. Between $25,000/$32,000 and $34,000/$44,000, up to 50% is taxable. Above $34,000/$44,000, up to 85%. Married filing separately and living together at any time: base $0, so up to 85% is taxable from the first dollar (p. 107). These thresholds have never been indexed for inflation since benefits were first taxed in 1984 (p. 54), so more retirees are taxed every year.

**The formula** (Worksheet II in the book): taxable = smaller of (a) 85% of benefits or (b) 85% × (provisional income − upper threshold) + smaller of [50% of benefits, half of (provisional − lower threshold), $4,500 single / $6,000 joint].

**Worked examples** (pp. 103–105):

- Joint, $30,000 other income, $16,000 benefits: provisional = $30,000 + $8,000 = $38,000. Over $32,000 by $6,000 → half = $3,000; that's less than half the benefits, so $3,000 is taxable.
- Joint, AGI $36,000, tax-exempt interest $6,000, benefits $16,000: provisional $50,000. Tier 1: smaller of $8,000, ($50,000 − $32,000)/2 = $9,000, or $6,000 → $6,000. Tier 2: 85% × ($50,000 − $44,000) = $5,100. Total $11,100, less than 85% of benefits ($13,600). Taxable $11,100.

**Lump-sum back benefits** can be attributed to earlier years when computing the taxable amount, without amending (p. 106).

**Earnings test** (before full retirement age; p. 102): for 2025, $1 of benefits is withheld per $2 earned above $23,400; in the year you reach full retirement age, $1 per $3 above $62,160 (earnings before the birthday month only). Withheld benefits aren't truly lost: at full retirement age the benefit is recalculated to credit the months withheld.

## Sources

- Tyson and Eck (2025), pp. 101–107.
- IRS, [Publication 915 (2025)](https://www.irs.gov/publications/p915); Social Security Administration, [Receiving benefits while working](https://www.ssa.gov/benefits/retirement/planner/whileworking.html).
