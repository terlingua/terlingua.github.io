---
title: Schedule B, 1099-INT and 1099-OID, and matching
questions:
- id: schedule-b-required
  prompt: In 2025 you have $900 of bank interest, $1,200 of dividends and a $3,000 account at a bank in Canada. Must you file Schedule B?
  explanation: Schedule B is required if taxable interest or ordinary dividends exceed $1,500, “And if you have a foreign bank account or are involved in a foreign trust, Schedule B has to be filed” (p. 241). Part III asks whether you had an interest in or authority over a foreign account. With a $3,000 maximum balance you answer "yes" to having an account but don't need to file an FBAR (FinCEN Form 114), because the aggregate didn't exceed $10,000 (p. 258).
  options:
  - text: Only if the Canadian bank sends a 1099
  - text: No, because neither interest nor dividends exceed $1,500
  - text: Yes, because you have a foreign financial account, even though interest and dividends are each under $1,500
    correct: true
  - text: No, because the foreign account is under $10,000
- id: cp2000
  prompt: What usually triggers a CP2000 notice?
  explanation: '“About a year after filing your taxes, you''ll receive a nasty notice called a CP-2000 listing the interest and dividends that you didn''t report” (p. 243). The IRS''s automated underreporter program compares information returns with your return. A CP2000 is a proposal, not a bill: you can agree, partially agree or disagree with documentation (Chapter 18). Listing every payer on Schedule B makes it easier to trace mismatches.'
  options:
  - text: Filing an extension
  - text: Filing on paper
  - text: Claiming the standard deduction
  - text: A mismatch between income reported to the IRS on information returns (W-2s, 1099s) and what you reported on your return
    correct: true
- id: backup-withholding
  prompt: A bank begins withholding 24% from your interest because you never certified your SSN. What does that withholding mean for your tax?
  explanation: 'Backup withholding is 24% of interest or dividends when you fail to provide a correct taxpayer identification number or underreport such income. The book: the amounts “simply represent a forced payment of your expected tax ... You get ‘credit’ for it on line 25b” (p. 243). You still report the income in full. Give the payer a signed W-9 with your SSN; if the IRS imposed it for underreporting, you need IRS certification to stop it.'
  options:
  - text: It's a penalty you can never recover
  - text: It replaces the tax on the interest, so you don't report the interest
  - text: It's refundable only if you're under 65
  - text: It's a prepayment credited to you on Form 1040 line 25b, just like wage withholding; certify your TIN (Form W-9) to stop it
    correct: true
- id: nominee
  prompt: A $1,200 interest 1099-INT is issued under your SSN for a joint account with your brother, who owns half the money. How do you report it?
  explanation: Report the full $1,200 on Schedule B, and on the line below “subtract the $600 belonging to the other person. In the space for the name of the payer, write ‘Nominee Distribution’” (p. 249). Showing the gross figure lets the IRS's matching system reconcile the 1099. Technically you become a nominee payer who must file a 1099 for the other owner. Spouses filing jointly don't need this.
  options:
  - text: Ask the bank to split the 1099
  - text: Report $1,200 and let your brother report $600 too
  - text: Report only $600 with no explanation
  - text: Report $1,200 on Schedule B, then subtract $600 on a separate line labelled "Nominee Distribution"; you should also issue your brother a 1099-INT for $600
    correct: true
- id: accrued-interest
  prompt: On April 30 you buy a bond that pays $600 of interest on June 30, paying the seller $400 of accrued interest. How do you report the June coupon?
  explanation: “You report the $600 in interest that you received for the bond on Schedule B. On the line below, subtract the $400 of interest you paid the seller ... write ‘Accrued Interest Paid’” (p. 248). The seller reports the $400 as interest (and excludes it from sale proceeds). The rule matches interest income to whoever owned the bond as it accrued.
  options:
  - text: Report $600; the $400 is added to basis
  - text: Report the $600 on Schedule B, then subtract $400 on a separate line labelled "Accrued Interest Paid", so $200 is taxable to you
    correct: true
  - text: Report $200 only, with no adjustment line
  - text: Report nothing until you sell the bond
- id: bank-toaster
  prompt: Your bank gives you a blender worth $30 for opening an account with $3,000. Is it taxable?
  explanation: Premiums for opening an account are treated as interest. “A gift valued at $10 or less for a deposit of less than $5,000 or $20 for a deposit of $5,000 or more isn't taxable” (p. 246). The bank reports cash bonuses (often hundreds of dollars for new accounts) on Form 1099-INT; they're fully taxable. The $600 1099 threshold is about the payer's reporting, not taxability.
  options:
  - text: Yes, $30 of interest, because it exceeds the $10 de minimis amount for deposits under $5,000
    correct: true
  - text: Only if you sell the blender
  - text: No, because it's under $600
  - text: No; gifts from banks are never taxable
---
## Learning objectives

- Know when Schedule B is required and why filing it voluntarily helps
- Read Forms 1099-INT and 1099-OID
- Handle nominee interest, accrued interest on bonds bought between payment dates, and small bank gifts
- Understand backup withholding and CP2000 notices

## The ideas

**When required** (p. 241): taxable interest or ordinary dividends over $1,500, or any foreign financial account or foreign trust. Filing it anyway is a useful checklist against last year's sources.

**Matching.** Payers send 1099s by January 31 (often only through online portals, p. 242), and the IRS matches them to your return. Omitted income produces a **CP2000** proposed-adjustment notice, about a year later, with tax, interest and possibly penalties (p. 243). Failing to give payers your SSN (or underreporting) can trigger **24% backup withholding**, which you claim as tax withheld (line 25b). Check 1099s against statements and request corrections; corrected 1099s can arrive late, so very early filers sometimes have to amend (p. 244).

**Form 1099-INT** boxes: 1 (taxable interest), 2 (early withdrawal penalty), 3 (US savings bond and Treasury interest — federally taxable, state-exempt), 4 (backup withholding), 8 (tax-exempt interest), 9 (private activity bond interest, an AMT item), 10–13 (market discount and bond premium) (pp. 243–245). **Form 1099-OID** reports original issue discount on zero-coupon and other discount bonds, taxable each year although no cash is paid (p. 244).

**Adjustments shown on Schedule B** (pp. 247–249), each as a separate negative line so the IRS can match the 1099 total:

- **Nominee interest**: interest reported under your SSN that belongs to someone else (subtract it, and issue that person a 1099).
- **Accrued interest paid**: when you buy a bond between payment dates you pay the seller interest earned so far; subtract it from the first coupon you receive. (Example: you pay $400 of accrued interest and receive a $600 coupon: report $600, subtract $400.)
- **Savings bond interest previously reported**.
- **Amortized bond premium**.

**Bank gifts** for opening an account are interest unless de minimis: $10 for deposits under $5,000, $20 for $5,000 or more (p. 246). Interest on tax refunds is taxable (p. 250).

## Sources

- Tyson and Eck (2025), pp. 241–250.
- IRS, [Instructions for Schedule B](https://www.irs.gov/forms-pubs/about-schedule-b-form-1040); IRS, [Understanding your CP2000 notice](https://www.irs.gov/individuals/understanding-your-cp2000-series-notice); IRS, [Topic 307, Backup withholding](https://www.irs.gov/taxtopics/tc307).
