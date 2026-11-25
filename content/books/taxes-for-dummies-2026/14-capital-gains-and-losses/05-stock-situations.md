---
title: Worthless stock, wash sales, small business stock, options and employer shares
questions:
- id: wash-sale
  prompt: You paid $15,000 for stock and sell it on December 10 for $6,000. On December 24 you buy the same stock back for $7,000. What happens?
  explanation: “If you sell the stock and buy back the shares within 30 days of the sale, you can't deduct the loss ... You add your $9,000 loss to the $7,000 that you paid for the new shares ... The cost ... is $16,000” (p. 329). Report it on Form 8949 with code W. The window is 30 days *before or after* the sale; buying more shares before selling a losing lot can trigger it too. To harvest a loss while staying invested, buy a similar but not substantially identical fund (a different index).
  options:
  - text: You deduct the $9,000 loss this year
  - text: The rule only applies if you rebuy within 7 days
  - text: The loss is lost forever
  - text: The $9,000 loss is disallowed for now and added to the new shares' basis, making it $16,000
    correct: true
- id: wash-sale-ira
  prompt: You sell a stock at a $5,000 loss in your taxable account and, two weeks later, buy the same stock in your Roth IRA. What happens to the loss?
  explanation: The wash sale rule looks at purchases by you or related accounts you control, including IRAs; in Revenue Ruling 2008-5 the IRS held that the disallowed loss doesn't increase basis in the IRA, so it's gone for good. Purchases by your spouse also count. Dividend reinvestment of the same stock within the window can create small partial wash sales.
  options:
  - text: It's disallowed, and because basis can't be added to an IRA, the loss is permanently lost
    correct: true
  - text: It's deductible because IRAs are separate
  - text: It's added to the IRA's basis
  - text: It becomes a Roth conversion
- id: worthless-stock
  prompt: You bought shares in 2019 for $15,000. The company's bankruptcy plan, confirmed in 2025, cancels the shares with nothing for shareholders. How do you report it?
  explanation: A security that becomes completely worthless is treated as sold on the last day of the tax year in which it became worthless (p. 328). Because the holding period runs from 2019 to December 31, 2025, the loss is long-term; the book's statement that such losses are reported in the short-term section is incorrect. Partial worthlessness doesn't count, and a sham sale for a penny is disregarded (p. 328). Because pinpointing the year is hard, the refund deadline for worthless securities is extended to seven years.
  options:
  - text: As a short-term loss in 2025
  - text: As an ordinary loss
  - text: As a $15,000 long-term capital loss in 2025, treated as a sale for $0 on December 31, 2025
    correct: true
  - text: Not until you sell the certificate to someone
- id: sec1244
  prompt: A married couple filing jointly loses $150,000 on qualifying section 1244 stock in a small corporation they founded. How is the loss treated?
  explanation: “You can deduct up to $50,000 ($100,000 if filing jointly) of a loss from the sale of or from worthless small business stock ... So if you're married and sustained a $150,000 1244 loss, you can deduct $100,000 from your income” (p. 329). The ordinary portion goes on Form 4797; the rest on Form 8949 and Schedule D. Only original-issue stock held by the original individual owner qualifies.
  options:
  - text: All $150,000 is a capital loss
  - text: $100,000 is an ordinary loss (deductible against any income) and $50,000 is a capital loss subject to the $3,000 limit
    correct: true
  - text: All $150,000 is ordinary
  - text: $50,000 ordinary and $100,000 capital
- id: iso-disqualifying
  prompt: You exercise ISOs for $15,000 when the shares are worth $20,000, and sell them six months later for $25,000. How is the $10,000 gain taxed?
  explanation: 'Selling before one year after exercise (and two after grant) is a disqualifying disposition: “The difference between the exercise price and the value on the day that you exercised the option is taxed as ordinary income”; the rest is capital gain (p. 330). The book describes the remaining $5,000 as long-term in its example; with a six-month holding period after exercise it''s short-term. Meeting both holding periods would make the whole $10,000 long-term (and the exercise spread an AMT item).'
  options:
  - text: $10,000 ordinary income
  - text: $5,000 ordinary and $5,000 long-term gain
  - text: $10,000 long-term capital gain
  - text: $5,000 ordinary income (the spread at exercise) and $5,000 short-term capital gain
    correct: true
- id: espp-qualifying
  prompt: You bought ESPP shares for $16,000 when they were worth $20,000; at grant they were worth $18,000. You sell more than two years after grant and one year after purchase for $25,000. Using the book's simplified approach, how is the $9,000 gain split?
  explanation: 'In a qualifying disposition, the ordinary portion is limited to the discount measured at the grant date (here $18,000 − $16,000 = $2,000, per the book''s example), or the actual gain if smaller; the rest is long-term capital gain (p. 330). A disqualifying (early) sale uses the purchase-date value: $20,000 − $16,000 = $4,000 ordinary, $5,000 capital gain (p. 331). Many employers report the ordinary part on the W-2; adjust basis on Form 8949 to avoid double tax.'
  options:
  - text: $2,000 ordinary income and $7,000 long-term capital gain
    correct: true
  - text: $9,000 long-term capital gain
  - text: $9,000 ordinary income
  - text: $4,000 ordinary and $5,000 capital gain
- id: eighty-three-b
  prompt: A startup grants you restricted shares worth $2,000 that vest over four years and might be worth $500,000 at vesting. What does an 83(b) election do, and what's the deadline?
  explanation: 'Without the election, restricted stock is taxed as ordinary income when the restrictions lapse — possibly at 37% on a large value. With an 83(b) election you''re taxed now on a small value and “the profit faces the maximum of the 20-percent capital gain rate” later (p. 332). The book doesn''t stress the deadline: the election must be made within 30 days after the transfer, with no extensions. The risk: if you forfeit or the company fails, the tax already paid isn''t refundable (only a capital loss for what you paid for the shares).'
  options:
  - text: It converts the shares to ISOs
  - text: It taxes the $2,000 as ordinary income now, so the growth to vesting isn't wage income and later gains are capital gains; it must be filed with the IRS within 30 days of the grant
    correct: true
  - text: It defers all tax until sale
  - text: It can be made with your tax return by April 15
---
## Learning objectives

- Deduct worthless securities correctly (with a correction on holding period)
- Apply the wash sale rule, including purchases before the sale and in IRAs
- Use section 1244 ordinary loss treatment
- Tax ISOs, ESPPs, NSOs and restricted stock (with the 83(b) deadline), and net unrealized appreciation

## The ideas

**Worthless securities** (p. 328): deductible only when *completely* worthless with an identifiable event; treated as sold on the last day of the year of worthlessness for $0. *Correction:* the book says to report them as short-term (p. 328); the holding period runs to December 31 of that year, so stock held over a year is a long-term loss. You have seven years to amend for a worthless security, because the year of worthlessness is often unclear. A sham sale to a friend for a penny doesn't work.

**Wash sales** (pp. 328–329): a loss on a sale is disallowed if you buy substantially identical stock within 30 days *before or after* the sale (a 61-day window); the disallowed loss is added to the new shares' basis and their holding period includes the old shares'. Example: cost $15,000, sold for $6,000, rebought for $7,000 → $9,000 loss disallowed, new basis $16,000. Buying in your IRA triggers the rule and the loss is lost permanently (Rev. Rul. 2008-5).

**Section 1244 small business stock** (p. 329): losses on original-issue stock of a small business corporation (capital received up to $1 million) are ordinary up to $50,000 a year ($100,000 joint); the rest is capital. (Section 1202 is the separate exclusion for *gains* on qualified small business stock, expanded by OBBBA for stock issued after July 4, 2025.)

**ISOs** (pp. 329–330): no regular tax at grant or exercise (AMT adjustment); long-term capital gain if held more than two years from grant and one year from exercise; otherwise the exercise spread is ordinary income. Options first exercisable on more than $100,000 of stock in a year are NSOs.

**ESPPs** (pp. 330–331): qualifying disposition — ordinary income equals the lesser of the actual gain or the discount measured at grant; disqualifying — the spread at purchase is ordinary income.

**NSOs** (p. 331): ordinary income at exercise (the spread); basis = value at exercise.

**Restricted stock and 83(b)** (p. 332): taxed when it vests, unless you elect under section 83(b) **within 30 days of the transfer** to be taxed on its current (often low) value, so later growth is capital gain. The gamble: if forfeited or worthless, the tax paid isn't refunded (only a capital loss on any amount paid).

**Expired options** you bought: a capital loss on expiry (p. 331). **Net unrealized appreciation** on employer stock from a plan (p. 333): Chapter 5.

## Sources

- Tyson and Eck (2025), pp. 327–333.
- IRS, [Publication 550](https://www.irs.gov/publications/p550); IRS, Rev. Rul. 2008-5 (wash sales and IRAs); Treas. Reg. § 1.83-2 (83(b) elections).
