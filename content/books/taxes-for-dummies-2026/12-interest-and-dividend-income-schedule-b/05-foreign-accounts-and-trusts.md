---
title: Foreign accounts, FBAR, Form 8938 and Form 3520
questions:
- id: fbar-threshold
  prompt: 'You have two foreign accounts: one peaked at $6,000 and the other at $5,000 during 2025 (at different times). Must you file an FBAR?'
  explanation: The FBAR threshold is an aggregate of the highest balances of all foreign accounts during the year exceeding $10,000. The book describes the rule in terms of “the highest aggregate balance in all of your foreign accounts” (p. 258). It's filed electronically with FinCEN by April 15 (automatic extension to October 15), separately from your tax return, listing each account's maximum value. Signature authority over an employer's foreign account can also trigger a filing.
  options:
  - text: No, because no single account exceeded $10,000
  - text: No, because the peaks were at different times
  - text: Only if the accounts earned interest
  - text: Yes, because the FBAR test adds the maximum values of all foreign accounts, and $11,000 exceeds $10,000
    correct: true
- id: fbar-vs-8938
  prompt: Which statements about FBAR and Form 8938 are correct? Select all that apply.
  type: multiple
  explanation: 'The two regimes overlap but are separate laws: the Bank Secrecy Act (FBAR, $10,000 aggregate) and FATCA (Form 8938, higher thresholds, broader asset types such as foreign stock held directly or interests in foreign entities). Filing one doesn''t satisfy the other. The book focuses on FBAR and stresses the stakes (p. 258); penalties for missing either can be severe, though non-wilful FBAR penalties are civil and there are streamlined procedures for coming into compliance.'
  options:
  - text: FBAR is filed with FinCEN; Form 8938 is attached to your income tax return
    correct: true
  - text: Form 8938's thresholds are higher (for a single US resident, $50,000 at year-end or $75,000 at any time)
    correct: true
  - text: You may have to file both for the same accounts
    correct: true
  - text: Filing Form 8938 eliminates the need for an FBAR
- id: foreign-inheritance
  prompt: You inherit $300,000 from your grandmother, a citizen and resident of Italy. Is it taxable in the US, and what must you file?
  explanation: Inheritances aren't income, but “If you receive a gift or inheritance from a foreign individual or estate above $100,000, you also have to provide the IRS with the foreigner's name and address. Report this information on Form 3520” (p. 259). Penalties for not filing are steep (up to 25% of the amount for foreign gifts). If the money lands in a foreign account, FBAR and possibly Form 8938 apply. Form 709 is for US donors making gifts, not recipients.
  options:
  - text: It's taxable as ordinary income
  - text: You must file Form 709
  - text: It isn't US income, but you must report it on Form 3520 because gifts and bequests from foreign individuals exceeded $100,000 in the year
    correct: true
  - text: Nothing needs to be filed
- id: rrsp
  prompt: A US citizen living in Seattle has a Canadian RRSP from years working in Toronto. Under current IRS rules, what reporting is needed?
  explanation: The book says RRSP holders must file Form 3520 or a treaty statement each year (p. 259). That was the pre-2014 rule (with Form 8891). Revenue Procedure 2014-55 granted automatic deferral to eligible US persons and removed the Form 3520 and 8891 requirements for Canadian RRSPs and RRIFs. Distributions are taxable when received. Foreign financial account reporting (FBAR, Form 8938) is unaffected.
  options:
  - text: Nothing at all
  - text: Form 3520 every year plus a treaty statement
  - text: Report the RRSP's annual earnings as income
  - text: The RRSP's growth is automatically tax-deferred under the treaty without Form 3520 (since Rev. Proc. 2014-55), but FBAR and possibly Form 8938 reporting still apply
    correct: true
---
## Learning objectives

- Know when FinCEN Form 114 (FBAR) is required, and how it's filed
- Distinguish FBAR from Form 8938 (FATCA)
- Know when Form 3520 is required for foreign gifts, inheritances and trusts

## The ideas

**FBAR** (FinCEN Form 114; p. 258): required if the *aggregate* maximum value of your foreign financial accounts exceeded $10,000 at any time during the year (including accounts over which you only have signature authority). Filed electronically with FinCEN (not the IRS), due April 15 with an automatic extension to October 15. Non-wilful violations bring civil penalties; wilful violations bring very large penalties and possible prosecution (the book's warning that failure to file is a felony with a five-year prison term describes the wilful case).

**Form 8938** (FATCA), filed with your return, covers "specified foreign financial assets" above higher thresholds — for a single person living in the US, more than $50,000 on the last day of the year or $75,000 at any time (double for joint filers; much higher for those living abroad). Many people must file both.

**Form 3520** (p. 259): report gifts or bequests from foreign individuals or estates over $100,000 in the year (not taxable, but reportable), transactions with and distributions from foreign trusts, and certain gifts from foreign entities. Penalties for not filing start at $10,000 (or a percentage of the amount). It's due with your return (extended with it) and filed separately.

**Canadian RRSPs and RRIFs.** The book says US holders must file Form 3520 or a treaty statement each year (p. 259). Since Revenue Procedure 2014-55, US persons get tax deferral automatically and no longer need Form 3520 (or the old Form 8891) for these plans; FBAR and Form 8938 reporting still apply.

## Sources

- Tyson and Eck (2025), pp. 258–259.
- FinCEN, [Report of Foreign Bank and Financial Accounts](https://www.fincen.gov/report-foreign-bank-and-financial-accounts); IRS, [Comparison of Form 8938 and FBAR requirements](https://www.irs.gov/businesses/comparison-of-form-8938-and-fbar-requirements); IRS, Rev. Proc. 2014-55.
