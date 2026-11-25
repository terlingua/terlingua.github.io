---
title: 'Dividends: 1099-DIV, qualified status, return of capital and splits'
questions:
- id: return-of-capital
  prompt: You bought fund shares for $10,000 and received $500 in box 3 (nondividend distributions) of Form 1099-DIV. What's the effect?
  explanation: 'Box 3 amounts are a return of your own investment: “These non-dividend distributions aren''t entered on your return ... What these distributions do is reduce the basis of your shares” — $10,000 becomes $9,500 (p. 255). Once basis reaches zero, further return-of-capital distributions are capital gains. Master limited partnerships and some REITs pay large return-of-capital distributions, so basis tracking matters.'
  options:
  - text: $500 of qualified dividends
  - text: No tax now; your basis falls to $9,500, increasing gain (or reducing loss) when you sell
    correct: true
  - text: A $500 capital loss
  - text: $500 of ordinary income
- id: not-qualified
  prompt: Which of these are NOT qualified dividends eligible for the 0/15/20% rates? Select all that apply.
  type: multiple
  explanation: Bank, credit union and money-market "dividends" are really interest (p. 257). REIT dividends generally aren't qualified (though they may get the 20% QBI deduction via box 5). The holding period requires more than 60 days in the 121-day window (p. 258). Dividends from US corporations and foreign corporations traded on US exchanges (or from treaty countries) qualify (p. 257). Money-market and bond fund distributions are taxed as ordinary interest-like dividends.
  options:
  - text: Dividends from a foreign company whose shares trade on the New York Stock Exchange
  - text: Dividends from a US blue-chip stock held for three years
  - text: Dividends on stock you held for only 20 days around the ex-dividend date
    correct: true
  - text: '"Dividends" from a credit union savings account'
    correct: true
  - text: Ordinary dividends from a REIT
    correct: true
- id: stock-split
  prompt: You own 100 shares with a total basis of $5,000. The company declares a 2-for-1 split. What happens for tax purposes?
  explanation: “Stock dividends (not the cash variety) and splits aren't taxable. You now own more shares!” (p. 256). The original basis is spread over the larger number of shares, and the new shares take the original holding period. (A stock dividend with a cash option, or that changes shareholders' proportional interests, can be taxable — an exception.)
  options:
  - text: The new shares have zero basis
  - text: Nothing is taxable; you now own 200 shares with the same $5,000 total basis ($25 a share), and the holding period carries over
    correct: true
  - text: You have $5,000 of dividend income
  - text: Your basis doubles to $10,000
- id: exempt-interest-dividends
  prompt: Your municipal bond fund's 1099-DIV shows $2,000 in box 12 (exempt-interest dividends), of which $300 is in box 13 (private activity bonds). How are they reported?
  explanation: Box 12 amounts carry to line 2a, and “if you're subject to the alternative minimum tax, the amount in box 13 will be added back to income” (p. 256). For state purposes you can usually exclude the portion earned on your own state's (and US territories') bonds, which funds list in a supplemental statement. Tax-exempt interest still counts in the Social Security and premium tax credit formulas.
  options:
  - text: $300 is taxable and $1,700 exempt
  - text: $2,000 goes on Form 1040 line 2a as tax-exempt interest; the $300 is an AMT preference; part may be exempt from your state's tax if earned on your own state's bonds
    correct: true
  - text: $2,000 on Schedule B as taxable dividends
  - text: Nothing needs to be reported
- id: iras-and-qualified
  prompt: You hold dividend-paying stocks in a traditional IRA. Do you get the 15% qualified dividend rate on them?
  explanation: '“As soon as you begin taking distributions, though, you don''t get the reduced rate; all distributions from IRAs (unless you made after-tax contributions), 401(k)s, and other retirement accounts are treated as ordinary income” (p. 258). This drives "asset location": hold tax-efficient assets (broad stock index funds with qualified dividends and deferred gains) in taxable accounts, and tax-inefficient ones (taxable bonds, REITs) in IRAs (Chapter 24). Roth accounts make everything tax-free if qualified.'
  options:
  - text: Yes, each year
  - text: Only after age 59½
  - text: Yes, when you withdraw them
  - text: No; dividends inside the IRA aren't taxed currently, and withdrawals are ordinary income regardless of the source
    correct: true
---
## Learning objectives

- Read every important box on Form 1099-DIV
- Determine qualified versus nonqualified dividends
- Compute basis after return-of-capital distributions and splits

## The ideas

**Form 1099-DIV** (pp. 253–256): 1a ordinary dividends (total, including qualified) → line 3b; 1b qualified dividends → line 3a; 2a capital gain distributions → Schedule D or line 7; 2b unrecaptured section 1250 gain (25% rate); 2c section 1202 small business stock gain; 2d collectibles gain (28%); 3 **nondividend distributions** (return of capital): not taxed now, but reduce basis; once basis reaches zero, further amounts are capital gain; 4 backup withholding; 5 section 199A (REIT) dividends eligible for the 20% QBI deduction; 7 foreign tax paid; 9–10 liquidation distributions; 12 exempt-interest dividends (from muni funds) → line 2a; 13 private activity bond dividends (AMT).

**Qualified dividends** (p. 257): from US corporations and qualified foreign corporations (traded on US exchanges or from treaty countries), meeting the holding period (more than 60 days in the 121-day window around the ex-dividend date). Not qualified: "dividends" from banks, credit unions and money-market funds (actually interest), most REIT dividends, dividends from tax-exempt organisations, substitute payments in lieu of dividends on shorted stock, and dividends you elected to treat as investment income (p. 258).

**Not dividends at all** (p. 256): stock dividends and splits (no income; spread basis over more shares); life insurance dividends until they exceed premiums paid.

**Retirement accounts**: qualified dividends inside an IRA or 401(k) lose their character; withdrawals are ordinary income (p. 258).

## Sources

- Tyson and Eck (2025), pp. 253–258.
- IRS, [Instructions for Form 1099-DIV](https://www.irs.gov/instructions/i1099div); IRS, [Publication 550](https://www.irs.gov/publications/p550).
