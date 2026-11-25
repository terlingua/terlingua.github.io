---
title: Form 8949 and adjusted basis
questions:
- id: drip-basis
  prompt: You own 1,000 shares (basis $20,000) and reinvest a $1,000 dividend at $20 a share in 2024. You sell all 1,050 shares in early 2025 for $25 each. How is the sale reported?
  explanation: 'Reinvested dividends were taxed when paid, so they add to basis: the total becomes $21,000 (p. 312). “Your holding period on each batch of new shares purchased is the dividend payment date, not the date of your original investment”: the 50 new shares are short-term if sold within a year. Forgetting reinvested dividends in basis means paying tax twice on the same money.'
  options:
  - text: One long-term sale with $20,000 basis and a $6,250 gain
  - text: The reinvested shares have zero basis
  - text: One short-term sale
  - text: 'Two lots: 1,000 shares long-term with $20,000 basis ($5,000 gain) and 50 shares short-term with $1,000 basis ($250 gain)'
    correct: true
- id: gift-dual-basis
  prompt: Your uncle gives you stock he bought for $10,000; it was worth $7,000 at the gift. What's your gain or loss if you sell it for (a) $12,000, (b) $5,000, (c) $8,000?
  explanation: 'When the value at the gift is below the donor''s basis, you use the donor''s basis to compute a gain and the lower value to compute a loss (p. 312). A sale between the two ($8,000) produces neither: measured from $10,000 it''s a loss, from $7,000 it''s a gain, so neither applies. The rule prevents shifting built-in losses to someone who can use them. Donors with losing positions should usually sell and give cash instead.'
  options:
  - text: Always use $7,000
  - text: Always use $10,000
  - text: (a) $2,000 gain using the donor's $10,000 basis; (b) $2,000 loss using the $7,000 value; (c) no gain or loss
    correct: true
  - text: (a) $5,000 gain; (b) $5,000 loss; (c) $1,000 gain
- id: digital-asset-boxes
  prompt: You sold bitcoin on an exchange in 2025, held for two years, and received a Form 1099-DA showing gross proceeds but no basis. Where do you report it?
  explanation: 2025 is the first year of Form 1099-DA reporting by digital asset brokers (gross proceeds only for 2025; basis for many assets acquired from 2026). The 2025 Form 8949 added boxes G, H and I (short-term) and J, K and L (long-term) for digital assets; the instructions say not to use box C or F for them. The book, written before the final forms, says to tick box C or F for crypto (p. 336). You must supply basis from your own or a tracking tool's records; without it the IRS may treat the whole proceeds as gain.
  options:
  - text: Form 8949 Part II with box K (long-term, reported on 1099-DA, basis not reported to the IRS), entering your own basis records
    correct: true
  - text: Nowhere, because the exchange reported it
  - text: Form 8949 box F, as for any asset without a 1099-B
  - text: Form 1040 line 8 as other income
- id: broker-basis-wrong
  prompt: Your 1099-B reports $10,000 of basis for NSO-exercise shares you sold, but you already paid ordinary tax on a $15,000 spread at exercise (included on your W-2). What should you do?
  explanation: Basis for stock received as compensation includes the amount you were taxed on (p. 312). Brokers often report only the cash paid for option shares. Form 8949 lets you correct basis with code "B" in column (f) and the adjustment in column (g) (p. 311). Equity-compensation employees should reconcile the supplemental statement many brokers provide.
  options:
  - text: Don't report the sale
  - text: Amend your W-2
  - text: Use the broker's $10,000 basis because it was reported to the IRS
  - text: Report the sale on Form 8949 with the correct basis (cost plus the $15,000 already taxed), using the adjustment columns, so you don't pay tax twice
    correct: true
---
## Learning objectives

- Choose the right Form 8949 box, including the new 2025 digital asset boxes
- Correct broker-reported basis with adjustment codes
- Determine basis for purchased, gifted, inherited, divorce and compensation property
- Track basis through reinvested dividends, splits, returns of capital and partial sales

## The ideas

**Form 8949** lists each sale; totals flow to Schedule D (p. 308). Boxes: A/D — 1099-B with basis reported to the IRS; B/E — 1099-B without basis reported; C/F — no 1099-B. New for 2025: **G/J, H/K, I/L** for digital asset transactions reported on the new **Form 1099-DA** (with or without basis) or not reported at all; the IRS instructions say not to use C or F for digital assets. Transactions with reported basis and no adjustments can go directly on Schedule D line 1a/8a. Column (f) codes and column (g) amounts correct errors (wash sales "W", nondeductible losses, excluded home gain "H", wrong basis "B", and so on) (p. 311).

**Basis** (pp. 311–313):

- *Purchase*: price plus commissions and fees.
- *Inheritance*: fair market value at death (or alternate valuation date).
- *Gift*: donor's basis for gains; for losses, the lower of donor's basis or value at the gift (dual basis); between those two figures, no gain or loss.
- *Divorce*: carryover basis.
- *Compensation*: the amount you included in income (stock received for services; NSO spread).

**Adjustments**: reinvested dividends add basis (and each reinvestment starts its own holding period — a lot sold within a year is short-term); improvements add basis; depreciation and casualty losses reduce it; nondividend distributions reduce it; splits spread it over more shares; undistributed gains (Form 2439) increase it; bond premium and discount adjust it.

**Partial sales**: unless you specifically identify the lots (before or at the sale), the default is FIFO; mutual funds also allow average cost (p. 313; Chapter 24).

## Sources

- Tyson and Eck (2025), pp. 308–314.
- IRS, [Instructions for Form 8949 (2025)](https://www.irs.gov/instructions/i8949); IRS, [Publication 551](https://www.irs.gov/publications/p551) (basis of assets).
