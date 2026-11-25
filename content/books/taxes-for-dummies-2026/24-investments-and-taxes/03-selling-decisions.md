---
title: 'Selling decisions: lots, gains, losses and options'
questions:
- id: spec-id
  prompt: You own 100 shares bought ten years ago at $50 and 100 shares bought two years ago at $100. The stock is $150. You sell 100 shares without telling your broker which. What's your gain?
  explanation: '“If you don''t specify before the sale which shares are to be sold — the first-in-first-out (FIFO) method” applies (p. 522). FIFO lot: ($150 − $50) × 100 = $10,000. Specific ID of the $100 lot: $5,000, both long-term. The deferred gain doesn''t vanish; you''ll realise it when you sell the remaining low-basis shares — or never, if they''re held until death and stepped up.'
  options:
  - text: $15,000
  - text: $7,500 (average cost)
  - text: $5,000
  - text: $10,000, because FIFO applies and the $50 shares are deemed sold; specifying the $100 lot beforehand would have made it $5,000
    correct: true
- id: split-basis
  prompt: Your parents gave you 1,000 shares bought at $10 ($10,000) ten years ago; after a 2-for-1 split you own 2,000 shares worth $20 each. What's your gain if you sell them all?
  explanation: 'The book''s example: “if you sell XYZ stock for $40,000 today, you''d have a capital gain of $30,000” (p. 523). Gifted shares carry the donor''s basis and holding period; splits spread basis over more shares without changing total basis. Had you inherited them, basis would be the value at death.'
  options:
  - text: $20,000
  - text: Nothing, because they were a gift
  - text: $30,000 ($40,000 proceeds − $10,000 carryover basis); the split just halves the per-share basis to $5
    correct: true
  - text: $10,000
- id: wash-before
  prompt: On November 20 you buy 100 more shares of a stock you already own at a loss. On December 5 you sell your original (higher-cost) 100 shares at a loss. Is the loss allowed?
  explanation: The book frames the rule as buying back “within 30 days” after selling (p. 524), but the 61-day window runs 30 days before through 30 days after the sale (Chapter 14). The disallowed loss is added to the basis of the November shares. Wait 31 days after buying before selling the old lot, or swap into a similar but different fund.
  options:
  - text: Yes, because you didn't buy back within 30 days after
  - text: Yes, because the shares were bought at different prices
  - text: Only half is disallowed
  - text: No, the loss is disallowed under the wash sale rule because you bought substantially identical shares within 30 days before the sale
    correct: true
- id: loss-harvest
  prompt: You have $12,000 of realised long-term gains in 2025 and a fund showing a $9,000 loss you'd be happy to replace with a similar fund. What's the effect of harvesting the loss?
  explanation: “When you sell other stocks during the year at a profit, tax-loss selling to offset these taxable gains makes more sense” (p. 524). Harvesting lowers your basis in the replacement, so it defers rather than eliminates tax (p. 524), but the deferral can be long and the gain may be stepped up at death. Use another index fund tracking a different index.
  options:
  - text: The loss is wasted because gains are long-term
  - text: You can deduct $3,000 only
  - text: You must wait a year to buy anything
  - text: The $9,000 loss offsets $9,000 of the gains, so only $3,000 of gain is taxed this year; buying a similar (not substantially identical) fund keeps you invested
    correct: true
- id: iso-holding
  prompt: You exercise ISOs on March 1, 2026 (granted June 1, 2024). When is the earliest sale that qualifies for full long-term capital gain treatment?
  explanation: A sale “within a year of having exercised the options, or within two years after the grant of the option (whichever is later)” is a disqualifying disposition taxed as ordinary income on the spread (p. 525). Both tests must be met; here the exercise test is the later one. The spread at exercise is an AMT preference in the year of exercise if you hold the shares past year-end.
  options:
  - text: Immediately, because ISOs are tax-favoured
  - text: March 2, 2027 — more than one year after exercise (the two-years-from-grant test is met on June 2, 2026)
    correct: true
  - text: June 2, 2026
  - text: March 1, 2028
---
## Learning objectives

- Choose specific identification, FIFO or average cost
- Decide whether to sell appreciated holdings and how to prioritise
- Harvest losses without tripping the wash sale rule (with a correction)
- Weigh ISO and NSO selling choices and find lost basis

## The ideas

**Lot selection** (pp. 521–522): **specific identification** must be made at or before the sale (confirmations should reflect it); otherwise **FIFO** applies, which usually sells your cheapest, oldest shares. For mutual fund shares, the **average cost** method is also available (p. 524); you can generally revoke an average cost election only within a year of making it and before selling, and changes apply prospectively.

**Selling winners** (pp. 522–523): taxes shouldn't stop you from selling when you need the money or to diversify; prefer selling large holdings with small gains and pair gains with losses.

**Selling losers** (pp. 523–524): losses offset gains, then up to $3,000 of ordinary income ($1,500 MFS), with carryforwards. Don't hold losers out of pride. *Correction:* the book says you just need to wait 31 days to buy back; the **wash sale** window is 30 days **before or after** the sale, so purchases (including reinvested dividends) in the 30 days before can trigger it too. Buying a similar but not substantially identical fund avoids it.

**Stock options** (pp. 524–525): ISOs — hold one year after exercise and two years after grant for long-term treatment (and watch the AMT); NSOs — ordinary income at exercise on the spread, then capital gain or loss from that basis. Concentration risk in employer stock matters as much as tax.

**Unknown basis** (pp. 525–526): ask the broker or fund company for old statements; brokers must report basis for stocks acquired from 2011, funds from 2012, bonds and options from 2014. Gifts take the donor's basis; inheritances get the date-of-death value.

## Sources

- Tyson and Eck (2025), pp. 521–526.
- IRS, [Publication 550](https://www.irs.gov/publications/p550) (identifying shares, wash sales); IRS, [Publication 551](https://www.irs.gov/publications/p551); Treas. Reg. § 1.1012-1(c), (e).
