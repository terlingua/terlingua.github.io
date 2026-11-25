---
title: Reconstructing missing records and the Cohan rule
questions:
- id: basis-definition
  prompt: What is "tax basis"?
  explanation: '“Tax basis is an IRS term for cost. Your basis usually is what you paid for something” (p. 37). It is then adjusted: improvements and some purchase costs increase it; depreciation and casualty-loss deductions reduce it. Gain = amount realised − adjusted basis. The book stresses that “property” includes not just real estate but stocks, bonds, cars, boats and computers. Without basis records you risk paying tax on the whole sale price.'
  options:
  - text: The current market value of an asset
  - text: Essentially your cost (adjusted for things like improvements and depreciation), which is subtracted from the sale price to determine gain or loss
    correct: true
  - text: The tax rate applied to a sale
  - text: The assessed value used for property tax
- id: inherited-basis
  prompt: You inherit stock your father bought for $10,000; it was worth $60,000 when he died. You sell it a year later for $65,000. What is your taxable gain?
  explanation: 'Inherited property takes a basis equal to its fair market value on the date of death (Code section 1014), the "stepped-up basis": “the tax basis ... is the fair market value of the property on the date of the titleholder''s death” (p. 37). An estate can elect the alternate valuation date six months later if that lowers estate tax (p. 39). So the $50,000 of appreciation during your father''s life is never income-taxed. Your gain is $65,000 − $60,000 = $5,000, and inherited property is automatically treated as long-term regardless of how long you held it.'
  options:
  - text: Zero, because inheritances are tax-free forever
  - text: $65,000
  - text: $55,000
  - text: $5,000, because your basis is the $60,000 date-of-death value
    correct: true
- id: gift-basis
  prompt: Your aunt gives you shares she bought for $8,000, worth $20,000 at the time of the gift. You sell them for $22,000. What is your gain?
  explanation: 'Gifts get a carryover basis: you step into the donor''s shoes (Code section 1015). Basis = $8,000; gain = $22,000 − $8,000 = $14,000. The donor''s holding period also carries over, so the gain is long-term if the aunt''s plus your holding exceeds a year — which is why, for gifted property, the book notes you need the date the donor acquired it (p. 38). The special rule: if the value at the gift was *less* than the donor''s basis, then for computing a *loss* you use that lower value (Chapter 14). Gifting appreciated assets passes the built-in gain to the recipient; bequeathing them erases it.'
  options:
  - text: Zero, because gifts are not taxable
  - text: $22,000
  - text: $2,000
  - text: $14,000, because you generally take over the donor's basis
    correct: true
- id: farm-estimate
  prompt: 'Using the book''s newspaper-ad method: in 1990 a house with 50 acres sold for $75,000 and one with 60 acres for $85,000. About what was a comparable house with 100 acres worth?'
  explanation: 'The ten extra acres cost $10,000, so an acre ≈ $1,000. A 100-acre farm has 40 more acres than the $85,000 farm: $85,000 + 40 × $1,000 = $125,000 (p. 38). It''s a linear interpolation from comparables, the same logic appraisers use. The book''s warning: IRS agents act like they''re from Missouri, the "Show Me" state, so keep copies of the ads and data you relied on.'
  options:
  - text: $150,000
  - text: $170,000
  - text: $100,000
  - text: $125,000
    correct: true
- id: assessed-value
  prompt: A property was assessed at $27,000 in the year you inherited it, and that county assessed property at 30% of market value. What was the approximate market value?
  explanation: Assessed value = market value × assessment ratio, so market value = assessed value ÷ ratio = $27,000 ÷ 0.30 = $90,000 (p. 38). The book calls this possibly the most accurate method because assessments were made for tax purposes at the time. Get copies from the county office that collects property taxes. A common slip is multiplying instead of dividing ($27,000 × 30% = $8,100).
  options:
  - text: $90,000
    correct: true
  - text: $35,100
  - text: $270,000
  - text: $8,100
- id: cpi-method
  prompt: You sell inherited land for $300,000. Lacking other data, you note that the Consumer Price Index has tripled since the date of death. What basis does the CPI method suggest, and what is the risk?
  explanation: 'Deflating the sale price by the CPI''s growth gives an estimate of the earlier value: $300,000 ÷ 3 = $100,000 (p. 38). The book offers this as a last resort and warns that the IRS may adjust it, “because historically real estate prices have risen at higher rates than the CPI in many major metropolitan areas” (p. 39). Note the direction of the risk: if land really outpaced inflation, the true value at death was *lower* than $100,000, so your gain would be *larger*. A retrospective appraisal by a qualified appraiser is stronger evidence.'
  options:
  - text: $900,000; there is no risk
  - text: $300,000; inheritances always have basis equal to sale price
  - text: About $100,000; the IRS may not accept it without adjustment because real estate often rose faster than the CPI
    correct: true
  - text: $33,333; the IRS always accepts CPI estimates
- id: cohan-rule
  prompt: What did the Cohan case establish, and what did Congress later change?
  explanation: George M. Cohan claimed $55,000 of travel and entertainment for 1921–22 without receipts. In 1930 the Second Circuit told the government to “make as close an approximation as it can, bearing heavily, if it chooses, upon the taxpayer whose inexactitude is of his own making” (p. 41). In the early 1960s Congress enacted what is now section 274(d), which bars estimates for travel, gifts and listed property (and entertainment, which since 2018 is not deductible at all). Cohan still applies to other expenses, such as petty cash, freight, cleaning, small tools and taxi fares (p. 42).
  options:
  - text: Entertainment expenses are fully deductible without records
  - text: The IRS must accept any estimate a taxpayer offers
  - text: Taxpayers never need receipts for any expense
  - text: Courts may estimate deductible expenses when the taxpayer clearly incurred them but lacks exact records, "bearing heavily" on the taxpayer; Congress later required strict substantiation for travel, entertainment, gifts and listed property such as cars
    correct: true
- id: cohan-limits
  prompt: Which statements about relying on the Cohan rule are true? Select all that apply.
  type: multiple
  explanation: “The Cohan Rule doesn't mean that you can stop keeping receipts and simply use estimates. You must have a valid reason for relying on the Cohan Rule, such as impracticability or lost or destroyed records” (p. 42). Taxpayers have been penalised for not seeking duplicates of records lost in a move, and for routinely destroying records after filing; one court held that “the unexplained loss of corporate records carries a strong presumption that the records would have prejudiced the taxpayer's position”. And because estimates "bear heavily" on the taxpayer, courts usually allow less than claimed.
  options:
  - text: You need a valid reason, such as records lost in a fire, or impracticability, as with petty cash
    correct: true
  - text: It lets you stop keeping receipts and estimate everything
  - text: Unexplained loss of records can create a presumption that they would have hurt you
    correct: true
  - text: Courts have penalised taxpayers who made no attempt to obtain duplicate records
    correct: true
- id: casualty-proof
  prompt: Your $10,000 watch was destroyed in a federally declared disaster and you have no receipt. What two things does the book say you must show?
  explanation: 'The book''s Rolex example lists exactly these two proofs (pp. 40–41). A police, fire or insurance report establishes that the event happened, but not what you lost or its value. Appraisals of similar items, photographs and third-party statements fill the gap, and a FEMA appraisal can serve if you got a disaster loan. Context the book doesn''t spell out here: since 2018 a personal casualty loss is deductible only if it is attributable to a federally declared disaster (OBBBA adds certain state-declared disasters from 2026), and only the amount above $100 per event and 10% of AGI (Chapter 11).'
  options:
  - text: That you could afford it (for example, from your income on your returns) and that you actually owned it (for example, a photo, a statement from the giver or a friend, or a jeweller's service record)
    correct: true
  - text: Only the police report
  - text: That the watch was insured
  - text: Only that the disaster occurred
- id: business-reconstruct
  prompt: Your business records were destroyed in a flood. Which reconstruction methods does the book suggest? Select all that apply.
  type: multiple
  explanation: 'Duplicate bills from major vendors are usually easy to get; bank statements show deposits from which income can be reconstructed; and a representative month of driving, if it approximates an average month, is “generally” accepted to estimate business use of a car (p. 41). Online account histories are typically free. The common thread: third-party records carry far more weight than your own after-the-fact estimates.'
  options:
  - text: Rebuild income from bank or brokerage deposit records
    correct: true
  - text: Request duplicate bills from vendors such as utilities, landlords and card issuers
    correct: true
  - text: Estimate everything at round numbers and sign an affidavit
  - text: Reconstruct a typical month of car use to estimate business mileage
    correct: true
---
## Learning objectives

- Define tax basis and the special basis rules for inherited and gifted property
- Use newspaper ads, broker data, assessed values and the CPI to estimate a historical value
- Reconstruct records for improvements, casualty losses and business expenses
- Explain the Cohan rule and its limits under section 274(d)

## The ideas

**Basis.** “Tax basis is an IRS term for cost” (p. 37). Gain = amount realised − adjusted basis. Inherited property gets a basis equal to fair market value at the date of death (or the alternate valuation date six months later, if the estate elects it) — the "step-up". Gifted property generally takes the donor's basis (carryover basis), with a special rule if the value at the gift was below the donor's basis (Chapter 14).

**Estimating a value** when no estate or gift tax return exists (pp. 37–39):

- *Newspaper ads*: in the book's example, ads show a house on 50 acres at $75,000 and one on 60 acres at $85,000 in 1990, so an acre was worth about $1,000 and Aunt Jessica's 100-acre farm about $85,000 + 40 × $1,000 = $125,000. Keep copies; the IRS wants documentation.
- *Broker and real estate board records*.
- *Assessed value*: divide the assessed value by the assessment ratio. $27,000 ÷ 30% = $90,000.
- *CPI*: if prices tripled since the acquisition, a $300,000 sale price implies about $100,000 then. The IRS may adjust this, because real estate often outpaced the CPI.

**Securities** are easier: historical prices are online; transfer agents and brokers can supply acquisition dates.

**Improvements**: bank and card statements, photos, the Certificate of Occupancy, building permits, assessment changes; or today's cost deflated by the CPI (p. 40).

**Casualty and theft losses**: police, fire and insurance reports prove the event; photos, appraisals of similar items, statements from people who saw you own the item, and repair-shop records prove ownership and value (pp. 40–41). (Today a personal casualty loss is deductible only if it results from a federally declared disaster — and, from 2026, certain state-declared disasters — so this mostly matters in disasters.)

**Business records**: duplicate bills from vendors; bank statements to rebuild income; a reconstructed typical month to estimate business mileage (p. 41).

**The Cohan rule.** George M. Cohan, the Broadway showman, deducted $55,000 of travel and entertainment for 1921–22 with no receipts. In 1930 the Second Circuit told the tax authorities to “make as close an approximation as it can, bearing heavily, if it chooses, upon the taxpayer whose inexactitude is of his own making” (p. 41). Congress overrode the rule in 1962 for travel, entertainment, gifts and listed property such as cars (now Code section 274(d)), which require strict substantiation. The rule still lets courts accept reasonable estimates for other expenses — petty cash, freight, cleaning, small tools, taxi fares — if you have a valid reason for the missing records (p. 42). (The book's list also includes “tips and business gifts”; business gifts are in fact under section 274(d)'s strict rules.)

## Sources

- Tyson and Eck (2025), pp. 36–42.
- *Cohan v. Commissioner*, 39 F.2d 540 (2d Cir. 1930); Internal Revenue Code §§ 274(d), 1014, 1015, 165(h).
