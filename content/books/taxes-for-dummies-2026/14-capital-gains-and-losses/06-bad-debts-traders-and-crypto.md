---
title: Bad debts, traders and cryptocurrency
questions:
- id: bad-debt
  prompt: You lent a friend $2,500 with a signed note; he declared bankruptcy in 2025 and the debt was discharged. How do you deduct it?
  explanation: Nonbusiness bad debts are deductible as short-term capital losses “regardless of how long someone has owed you the money” (p. 334), subject to the $3,000 net capital loss limit. You must show a valid debt (a right to sue; a note helps), worthlessness through an identifiable event such as bankruptcy, and that the money lent was already taxed (p. 333). A loan the IRS sees as a gift (common between relatives without paperwork) isn't deductible.
  options:
  - text: As an ordinary business loss
  - text: As a short-term capital loss on Form 8949, with a statement explaining the debt and why it's worthless
    correct: true
  - text: As an itemized deduction
  - text: You can't; personal loans aren't deductible
- id: trader-status
  prompt: Which statement about traders and mark-to-market is correct?
  explanation: “Traders are subject to the same $3,000 annual limit on trading losses that exceed gains. The wash sale rule also applies” (p. 334). The MTM election converts trading gains and losses to ordinary (Form 4797) and removes the loss limit and wash sales, but must be made by the due date of the prior year's return (p. 335). Trader status requires substantial, frequent, regular trading; the IRS resists it for people with full-time jobs (the book thinks part-time traders can qualify).
  options:
  - text: A trader can deduct trading expenses on Schedule C, but without a timely section 475(f) election gains and losses stay capital, subject to the $3,000 loss limit and wash sale rule
    correct: true
  - text: The mark-to-market election can be made any time before filing for the same year
  - text: Traders pay self-employment tax on gains
  - text: Anyone who trades online is a trader
- id: crypto-inherited
  prompt: You inherit bitcoin your father bought for $5,000; it was worth $60,000 when he died. You sell it for $62,000. What's the gain?
  explanation: 'The book says “the basis of inherited cryptocurrency is the same as the basis in the hands of the original owner” (p. 336). That''s incorrect: the IRS treats digital assets as property, and section 1014 gives property acquired from a decedent a basis equal to fair market value at death. So basis = $60,000, gain = $2,000, and it''s long-term because inherited. The practical problem is access: heirs need keys or exchange credentials, and estates should document holdings and values at death.'
  options:
  - text: $2,000, because inherited crypto gets a stepped-up basis equal to its value at death, like any other property
    correct: true
  - text: $57,000, because crypto keeps the decedent's basis
  - text: $62,000
  - text: Zero, because inheritances are tax-free
- id: crypto-taxable-events
  prompt: Which of these are taxable events in 2025? Select all that apply.
  type: multiple
  explanation: 'Crypto transactions are treated like property dispositions: “If you are buying, selling, trading, bartering, exchanging, using for the purchase of goods or services, or otherwise using cryptocurrency, you''d better be prepared to report your transactions” (p. 336). Each swap or purchase realises gain or loss; payments received are ordinary income at fair market value. Buying and holding, or transferring between your own wallets, isn''t a disposal (though it may still prompt a "yes" on the digital assets question if you received assets). The IRS has pursued nonreporting aggressively, with civil and criminal penalties (p. 336).'
  options:
  - text: Paying for a laptop with bitcoin that has appreciated since you bought it
    correct: true
  - text: Moving bitcoin between two wallets you own
  - text: Swapping ether for another token
    correct: true
  - text: Receiving stablecoins as payment for consulting work
    correct: true
  - text: Buying bitcoin with dollars and holding it
---
## Learning objectives

- Deduct a nonbusiness bad debt
- Distinguish investors, traders and mark-to-market traders
- Report crypto correctly, with two corrections

## The ideas

**Nonbusiness bad debts** (pp. 333–334): a loan to a friend or relative that becomes totally worthless is a **short-term capital loss**, regardless of how long it was outstanding. You need a bona fide debt (right to sue, note, ideally collateral and interest — family loans are often treated as gifts), an identifiable event showing worthlessness, and basis (you can't deduct wages you were never paid). Attach a statement describing the debt and collection efforts. Partial worthlessness only counts for business debts.

**Investors vs traders** (pp. 334–335). Investors report gains and losses on Schedule D and get no deduction for investment expenses (other than investment interest). **Traders** — frequent, substantial and continuous trading seeking short-term swings — deduct expenses on Schedule C (including a home office), but gains and losses remain capital (with the $3,000 limit and wash sales) unless they make a **section 475(f) mark-to-market election**. MTM traders treat open positions as sold at year-end, report ordinary gains and losses on Form 4797, escape the $3,000 limit and wash sale rule, but lose capital gain rates on trading positions. The election is due by the original due date of the *prior* year's return (for 2026, by April 15, 2026). Trading gains aren't subject to SE tax and don't support retirement contributions.

**Cryptocurrency** (p. 336): property, not currency. Selling, swapping one coin for another, or spending crypto are taxable disposals; receiving it as pay or rewards is income. Report on Form 8949 like stock, with the same holding-period rules. *Corrections:* (1) the book says crypto doesn't appear on 1099-B and to use box C or F; for 2025, brokers issue Form 1099-DA and Form 8949 has dedicated digital asset boxes (G–L); (2) the book says inherited crypto keeps the decedent's basis. That's wrong: crypto is property, and inherited property gets a basis equal to its fair market value at death under section 1014, like stock.

## Sources

- Tyson and Eck (2025), pp. 333–336.
- IRS, [Digital assets](https://www.irs.gov/filing/digital-assets) and [Frequently asked questions on virtual currency transactions](https://www.irs.gov/individuals/international-taxpayers/frequently-asked-questions-on-virtual-currency-transactions); IRS, [Topic 429, Traders in securities](https://www.irs.gov/taxtopics/tc429); Internal Revenue Code § 1014.
