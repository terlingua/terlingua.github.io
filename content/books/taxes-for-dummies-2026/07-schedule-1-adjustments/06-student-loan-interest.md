---
title: Student loan interest
questions:
- id: sli-joint
  prompt: A married couple filing jointly has modified AGI of $190,000 in 2025 and paid $3,000 of student loan interest. What is their deduction?
  explanation: 'Start with the $2,500 cap (not the $3,000 paid). The joint phase-out runs from $170,000 to $200,000, a $30,000 range. They are $20,000 into it: reduction = $2,500 × $20,000 ÷ $30,000 ≈ $1,667; deduction ≈ $833. The book''s table for the married example lists $20,000 on line 3 but then divides $15,000 by $30,000 to get $1,250 (pp. 150–151); its single example ($91,000 → $1,500) is correct.'
  options:
  - text: $1,250
  - text: $2,500
  - text: $0
  - text: About $833
    correct: true
- id: sli-who
  prompt: Which of these people can deduct student loan interest for 2025?
  explanation: 'You must be legally obligated on the loan, not file MFS, and not be claimable as a dependent (p. 150). The book notes the twist: if a nondependent student is the borrower and parents make the payments, the IRS treats the money as a gift to the student who then paid the interest, “the student may be allowed to take the interest deduction”. Parents who aren''t co-borrowers can''t deduct. A dependent student can''t deduct even if obligated.'
  options:
  - text: A 24-year-old graduate, not anyone's dependent, who is the borrower on her loans, even though her parents made the payments
    correct: true
  - text: A student who is claimed as a dependent on his parents' return
  - text: A married borrower filing separately
  - text: Parents who make payments on a loan in their child's name only
- id: sli-qualified
  prompt: Which loans can generate deductible student loan interest? Select all that apply.
  type: multiple
  explanation: Qualified expenses include “tuition, fees, room and board, books, supplies, and other necessary expenses” for a degree candidate carrying at least half the normal load (p. 150). Private loans qualify as well as federal ones (p. 151), and refinanced or consolidated qualified loans keep their status. “Loans from related family members don't qualify” and revolving credit doesn't unless used only for education (p. 150). Home equity interest isn't student loan interest (and HELOC interest is deductible as mortgage interest only if the money improved the home).
  options:
  - text: A federal Direct Loan used for tuition at an accredited university, where the student was enrolled at least half-time
    correct: true
  - text: A private bank loan used only for room and board while at least half-time
    correct: true
  - text: A home equity line used for anything, including tuition
  - text: A loan from your grandmother used for tuition
  - text: A refinanced or consolidated qualified education loan
    correct: true
---
## Learning objectives

- State the eligibility rules and $2,500 cap
- Compute the phase-out correctly
- Know who may claim the deduction when parents pay

## The ideas

Up to **$2,500** of interest paid on a qualified education loan, deductible above the line (p. 149). Requirements (p. 150): you're legally obligated on the loan; not MFS; not someone else's dependent; the loan paid qualified higher education expenses (tuition, fees, room and board, books, supplies) for you, your spouse or your dependent at the time, at an eligible institution, at least half-time; not from a relative or a qualified employer plan. Lenders issue Form 1098-E when interest is $600 or more.

**Phase-out (2025)**: modified AGI $85,000–$100,000 single, $170,000–$200,000 joint (p. 149). Reduction = $2,500 × (MAGI − start) ÷ range. Single at $91,000: $2,500 × $6,000 ÷ $15,000 = $1,000 reduction, $1,500 deductible. Joint at $190,000: $2,500 × $20,000 ÷ $30,000 = $1,667 reduction, **$833** deductible. *Correction:* the book's joint column divides $15,000 by $30,000 and arrives at $1,250 (pp. 150–151), which doesn't follow from its own line 3 ($20,000).

**Who deducts when parents pay?** Only someone obligated on the loan. If the student is obligated and is not anyone's dependent, payments by parents are treated as gifts to the student and paid by the student, so the student can deduct (p. 150). If the student is a dependent, nobody deducts that year.

## Sources

- Tyson and Eck (2025), pp. 149–151.
- IRS, [Publication 970](https://www.irs.gov/publications/p970) (tax benefits for education); IRS, [Topic 456](https://www.irs.gov/taxtopics/tc456).
