---
title: 'Scenario 4: a retired couple, and a capital netting exercise'
questions:
- id: s4-ss
  prompt: How much of Pat and Lee's Social Security is taxable?
  explanation: 'Provisional income = $40,000 + $25,000 = $65,000. Above the $44,000 second threshold: taxable = the lesser of 85% × benefits ($42,500) or 85% × ($65,000 − $44,000) + the lesser of $6,000 or half the benefits = $17,850 + $6,000 = $23,850. Each extra IRA dollar here makes 85 cents of benefits taxable — the "tax torpedo".'
  options:
  - text: '$23,850: provisional income is $65,000; 85% × ($65,000 − $44,000) + $6,000 = $23,850, less than 85% of benefits ($42,500)'
    correct: true
  - text: $0
  - text: $25,000
  - text: $42,500
- id: s4-tax
  prompt: What are their AGI, taxable income and federal tax?
  explanation: 'AGI = $40,000 + $23,850. Standard deduction $31,500 + 2 × $1,600 (both 65+) = $34,700; plus the 2025–2028 senior deduction of $6,000 each ($12,000; MAGI well below $150,000), claimed on Schedule 1-A. Taxable income $17,150, all in the 10% bracket: $1,715. Without the senior deduction it would be $2,915.'
  options:
  - text: AGI $63,850; taxable income $29,150; tax $2,915
  - text: AGI $90,000; taxable income $58,500; tax $6,400
  - text: No tax because Social Security is exempt
  - text: AGI $63,850; deductions $46,700 ($31,500 + $3,200 + $12,000 senior); taxable income $17,150; tax $1,715
    correct: true
- id: s4-conversion-room
  prompt: Pat and Lee have $6,700 of room left in the 10% bracket ($23,850 − $17,150). Why is a Roth conversion here less cheap than "10%" suggests?
  explanation: In the 85% phase-in range, $1 of IRA income raises taxable income by $1.85. At 10%, that's an 18.5% effective rate; at 12% it would be 22.2%. Once 85% of benefits are taxable ($42,500 here), the torpedo ends. Model several years together (including RMDs and IRMAA thresholds) before converting.
  options:
  - text: Conversions aren't allowed after RMD age
  - text: Conversions are tax-free for seniors
  - text: It's exactly 10%
  - text: Each converted dollar also makes 85 cents of Social Security taxable (and can reduce the senior deduction at higher incomes), so the effective marginal rate is about 18.5% (10% × 1.85) until benefits are maximally taxed
    correct: true
- id: s5-netting
  prompt: Morgan (single) has a $300,000 home sale gain qualifying for the $250,000 exclusion, a $10,000 short-term stock loss and a $5,000 long-term loss carryover. What's the 2025 result?
  explanation: 'Gain above the exclusion ($50,000) is long-term (owned more than a year). Long-term net: $50,000 − $5,000 = $45,000. Short-term net: −$10,000. Combined: $35,000 of net long-term gain. Losses first offset gains, so nothing remains for the $3,000 ordinary deduction. Report the sale on Form 8949 with code H for the exclusion (Chapter 14).'
  options:
  - text: No taxable gain because of the exclusion
  - text: 'Net long-term gain of $35,000 taxed at 0/15/20%: $50,000 taxable home gain − $5,000 carryover − $10,000 short-term loss'
    correct: true
  - text: $35,000 of short-term gain
  - text: $50,000 long-term gain, with a $3,000 deduction for losses
- id: s5-niit
  prompt: Would Morgan's home sale gain be subject to the 3.8% NIIT if her MAGI were $280,000?
  explanation: Excluded gain under section 121 isn't net investment income; the taxable $50,000 of gain (net of capital losses, $35,000 here) is. NIIT = 3.8% × the lesser of NII ($35,000) or MAGI over $200,000 ($80,000) = $1,330 (Chapter 8).
  options:
  - text: Yes, on the full $300,000
  - text: No, home sales are never subject to NIIT
  - text: Only if she owned the home less than five years
  - text: Only the taxable portion above the exclusion counts as net investment income; with MAGI $80,000 over the $200,000 threshold, NIIT applies to the lesser of NII or $80,000
    correct: true
---
## Learning objectives

- Compute taxable Social Security with the provisional income formula
- Apply the 65+ additional standard deduction and the new senior deduction
- Net short- and long-term gains and losses with a partial home sale exclusion

## The facts

**Retirees.** Pat and Lee, both 67, file jointly for 2025. Social Security benefits total $50,000; traditional IRA withdrawals total $40,000. No other income; they take the standard deduction.

**Netting.** Separately, Morgan (single, 2025) sells a home owned and lived in for 10 years at a $300,000 gain, has a $10,000 short-term loss on stock, and has a $5,000 long-term capital loss carryover.

## How the pieces fit

Provisional income = other income + half of benefits; above $44,000 (joint) up to 85% of benefits are taxable (Chapter 5). The 2025 standard deduction is $31,500 plus $1,600 per spouse aged 65+, and each spouse 65+ gets the new $6,000 senior deduction if MAGI is under $150,000 (phased out above) (Chapters 1, 4). Home sale gain above the exclusion is long-term gain (Chapter 14).

## Sources

- IRS, [Publication 915 (2025), Social Security and Equivalent Railroad Retirement Benefits](https://www.irs.gov/publications/p915); IRS, [Schedule 1-A](https://www.irs.gov/newsroom/schedule-1-a-additional-deductions-what-to-know-about-the-new-form); IRS, [Publication 523](https://www.irs.gov/publications/p523).
