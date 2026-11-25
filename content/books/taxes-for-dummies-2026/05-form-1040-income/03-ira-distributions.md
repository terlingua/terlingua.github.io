---
title: IRA distributions, required minimum distributions and early withdrawals
questions:
- id: pro-rata
  prompt: Over the years you made $20,000 of nondeductible IRA contributions. In 2025 you withdraw $50,000, and your traditional IRA balance on December 31, 2025 is $150,000. How much of the withdrawal is taxable?
  explanation: 'Form 8606 applies the pro-rata rule: nontaxable = basis × distributions ÷ (year-end value + distributions) = $20,000 × $50,000 ÷ ($150,000 + $50,000) = $20,000 × 25% = $5,000. Taxable = $45,000, and $15,000 of basis carries forward. The book: “if you simply enter the amount reported in box 1 ... as being fully taxable, you''ll overpay your tax if you made nondeductible contributions” (p. 85). The pro-rata rule aggregates all traditional, SEP and SIMPLE IRAs (not 401(k)s), which is why "backdoor Roth" conversions get messy when you have other pre-tax IRA money.'
  options:
  - text: $50,000
  - text: $30,000 (the withdrawal minus your basis)
  - text: $45,000
    correct: true
  - text: Zero, because you're withdrawing after-tax money first
- id: rmd-calc
  prompt: You turn 75 in 2025 and your IRA was worth $500,000 on December 31, 2024. The Uniform Lifetime Table factor at 75 is 24.6. What is your 2025 RMD?
  explanation: 'RMD = prior year-end balance ÷ distribution period = $500,000 ÷ 24.6 ≈ $20,325. The book''s example at 73: $200,000 ÷ 26.5 = $7,547 (p. 86). The factor shrinks each year (26.5 at 73, 25.5 at 74, 24.6 at 75), so the required percentage rises — about 3.8% at 73, 4.1% at 75, 6.5% at 85. Use the balance from December 31 of the *previous* year, and add up all your IRAs'' RMDs (you may take the total from any one IRA).'
  options:
  - text: $37,594
  - text: $12,500
  - text: About $20,325
    correct: true
  - text: $25,000
- id: first-rmd-trap
  prompt: You turn 73 in 2025 and delay your first RMD until March 2026. What is the consequence?
  explanation: You may wait until April 1 of the year after you reach RMD age for the first distribution, “however, if you choose to delay that first payment to the following year, you'll also have to take a second distribution in that same year, by December 31” (p. 84). Doubling up income in one year can raise your bracket, make more Social Security taxable and increase Medicare IRMAA surcharges two years later. Usually it is better to take the first RMD in the year you turn 73. After the first year, every RMD is due by December 31.
  options:
  - text: Nothing; the deadline is simply April 1
  - text: The 2025 RMD is forgiven
  - text: You owe a 25% penalty for 2025
  - text: You must also take your 2026 RMD by December 31, 2026, so two RMDs are taxed in 2026, possibly pushing you into a higher bracket and raising Medicare premiums
    correct: true
- id: missed-rmd-penalty
  prompt: You forgot a $4,000 RMD for 2025 and discover it in June 2026. What should you do?
  explanation: 'SECURE 2.0 reduced the penalty from 50% to 25% — the book''s example: a missed $4,000 RMD costs “a cool $1,000” (p. 93). It falls to 10% if you take the missed amount within the correction window (generally by the end of the second year after the year of the miss, or earlier if the IRS assesses). Better still, the IRS routinely waives it for reasonable error if you correct the shortfall and attach an explanation to Form 5329 (the instructions say to write "RC" and the amount to be waived). The book''s advice: “If you discover you should have been taking distributions, start now!” (p. 94).'
  options:
  - text: Recharacterize it as a Roth contribution
  - text: Nothing; the penalty is a flat $50
  - text: Pay a 50% penalty of $2,000
  - text: Take the missed distribution promptly and file Form 5329; the excise tax is 25% ($1,000), cut to 10% ($400) if corrected in time, and you can request a full waiver for reasonable error
    correct: true
- id: early-ira-exceptions
  prompt: Which IRA withdrawals before age 59½ avoid the 10% additional tax (though they may be taxable)? Select all that apply.
  type: multiple
  explanation: The book lists the first-home exception (up to $10,000 lifetime; "first" means no ownership interest in a main home in the two years before), higher education expenses (no dollar limit; tuition, fees, books, and room and board for at least half-time students), medical expenses above 7.5% of AGI, health insurance while unemployed, death, disability and substantially equal periodic payments (pp. 87–88). Paying debts and buying cars have no exception. SECURE 2.0 added a once-a-year $1,000 emergency withdrawal, birth/adoption ($5,000), terminal illness, domestic abuse (up to the lesser of $10,000 indexed or 50%) and federally declared disaster exceptions. Spouses each get their own $10,000 first-home allowance from their own IRAs (p. 88).
  options:
  - text: $10,000 used within 120 days to buy a first home (no home owned in the prior two years)
    correct: true
  - text: $15,000 used for a child's qualified college tuition and fees
    correct: true
  - text: $25,000 withdrawn to pay off credit cards
  - text: A $30,000 withdrawal to buy a car for a new job
  - text: Withdrawals equal to medical expenses above 7.5% of AGI
    correct: true
- id: sepp
  prompt: At 56 you begin substantially equal periodic payments (SEPP/72(t)) from an IRA. When can you change or stop them without retroactive penalties?
  explanation: 'The exception requires payments over your life (or joint lives) computed by an IRS-approved method, and you must not modify them until the later of five years from the first payment or age 59½. The book''s example: start at 56, and “At age 61, you can withdraw the remaining balance or any part of it if you want to” (p. 87). Modify early and the 10% tax applies retroactively, with interest, to all prior payments. You can apply SEPP to just one IRA, so splitting IRAs first lets you size the payments.'
  options:
  - text: At 59½ regardless of when you started
  - text: Never
  - text: After the later of five years or reaching age 59½ — so at 61 in this case
    correct: true
  - text: After one year
- id: ira-divorce
  prompt: Under your divorce decree you must give your ex-spouse $100,000 from your IRA. Which method is tax-free to both of you?
  explanation: 'A transfer of an IRA interest to a spouse or former spouse under a divorce or separation instrument is not taxable and not a distribution (section 408(d)(6)); the IRA then belongs to the recipient. The book warns of a taxpayer who withdrew and endorsed the check over: the Tax Court held “his interest in his IRA was extinguished when he withdrew the funds”, so he was taxed (p. 88). The book also recommends a QDRO; strictly, Qualified Domestic Relations Orders apply to employer plans (401(k), pensions), while IRAs follow the divorce decree itself. For a 401(k), a distribution to an ex-spouse under a QDRO is taxed to the ex and exempt from the 10% tax.'
  options:
  - text: Withdraw the money and let your ex deposit it within 60 days
  - text: Withdraw $100,000 and endorse the check to your ex
  - text: A direct trustee-to-trustee transfer from your IRA to an IRA in your ex's name (or retitling the whole IRA), made under the divorce or separation instrument
    correct: true
  - text: Any method; divorce transfers are always tax-free
---
## Learning objectives

- Report IRA distributions and compute the taxable part when there is basis (Form 8606)
- Compute a required minimum distribution and know the first-year timing trap
- List the exceptions to the 10% additional tax for IRAs
- Transfer an IRA in a divorce without tax

## The ideas

**Taxability.** Traditional IRA distributions are fully taxable unless you made nondeductible contributions (basis). With basis, the **pro-rata rule** on Form 8606 makes each distribution partly tax-free: nontaxable share = basis × distribution ÷ (year-end value of all your traditional, SEP and SIMPLE IRAs + distributions) (p. 85). You can't withdraw only the after-tax dollars.

**Form 1099-R box 7 codes** (p. 85): 1 = early, no known exception; 2 = early, exception applies; 3 = disability; 4 = death; 7 = normal; G = direct rollover. You can claim an exception on Form 5329 even if the payer coded 1.

**RMDs.** Required minimum distributions start for the year you reach 73 (75 if born 1960 or later). The first can be delayed to April 1 of the next year, but then you take two in that year (p. 84). The amount = prior December 31 balance ÷ the Uniform Lifetime Table factor for your age (26.5 at 73, 25.5 at 74, 24.6 at 75) (p. 86). A spouse more than ten years younger and the sole beneficiary lets you use the Joint Life table, which gives smaller RMDs. For IRAs you total the RMDs and may take them from any IRA; for 401(k)s each plan's RMD comes from that plan (p. 86).

**Missed RMD.** The excise tax was cut by SECURE 2.0 from 50% to **25%** (p. 93), and to **10%** if corrected within the correction window (generally by the end of the second year after the miss). The IRS can waive it for reasonable error if you correct promptly (explain on Form 5329).

**The 10% additional tax** applies to the taxable part of distributions before 59½, with exceptions (pp. 87–88): death; disability; substantially equal periodic payments (continue for the later of five years or age 59½); medical expenses above 7.5% of AGI; health insurance premiums while unemployed 12 weeks; higher education expenses (no dollar limit); first-time homebuyer (up to $10,000 lifetime, used within 120 days, no ownership in the prior two years); IRS levy; and SECURE 2.0 additions (birth/adoption $5,000, terminal illness, one $1,000 emergency withdrawal a year, domestic abuse victims, disasters).

**Divorce.** A transfer of an IRA to a former spouse under a divorce or separation instrument is tax-free (Code section 408(d)(6)) if done by trustee-to-trustee transfer or retitling. Never withdraw and endorse the check over: a taxpayer who did so was taxed on it (p. 88). (The book also says a QDRO is needed; QDROs are for employer plans such as 401(k)s; for an IRA, the divorce decree or separation agreement itself is the instrument.)

## Sources

- Tyson and Eck (2025), pp. 84–88, 92–94.
- IRS, [Publication 590-B (2025)](https://www.irs.gov/publications/p590b); IRS, [Retirement topics — exceptions to tax on early distributions](https://www.irs.gov/retirement-plans/plan-participant-employee/retirement-topics-exceptions-to-tax-on-early-distributions).
