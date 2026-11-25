---
title: 'Dependents: qualifying children and qualifying relatives'
questions:
- id: qc-age
  prompt: Which children meet the qualifying child age test for dependency in 2025? Select all that apply.
  type: multiple
  explanation: 'The child must be under 19 at the end of the year, or under 24 and a full-time student during some part of each of five months, and younger than the taxpayer; “No age test applies for a child who is permanently and totally disabled” (pp. 61–62). The 21-year-old worker fails the age test; he might still be a qualifying relative if his gross income is under $5,200 and you provide more than half his support. Remember the credit-specific ages: under 17 for the $2,200 child tax credit (a 17- or 18-year-old gets the $500 credit for other dependents) and under 13 for the dependent care credit.'
  options:
  - text: A 21-year-old who works full time and is not a student
  - text: An 18-year-old who is not a student
    correct: true
  - text: A 22-year-old who was a full-time student for five months of 2025
    correct: true
  - text: A 30-year-old who is permanently and totally disabled
    correct: true
- id: qc-residency
  prompt: A baby is born on December 30, 2025 and comes home on January 1, 2026. Is the baby a qualifying child for 2025?
  explanation: “An exception is made for children who are born during the tax year, provided that they lived with you for the entire period they were alive. So, a child who is born on December 30, 2025, is your qualifying child” (p. 62). The family gets the full $2,200 child tax credit for 2025 if the baby gets an SSN by the return's due date (including extensions). Similar rules apply to a child who dies during the year. A child born and dying in the same year can qualify; a stillborn child cannot (p. 57).
  options:
  - text: No; a child must be born before July 1
  - text: No; the child must live with you 183 nights
  - text: Only for the dependent care credit
  - text: Yes; a child born during the year counts as living with you for the year if he or she lived with you for the entire time alive (hospital time counts as temporary absence)
    correct: true
- id: qc-support-test
  prompt: For a qualifying child, what is the support test?
  explanation: 'For a qualifying child the test runs the other way from the qualifying relative test: what matters is that the child didn''t self-support (p. 62). A teenager who earns $20,000 and saves it still passes if he didn''t *spend* more than half of his total support himself; money banked isn''t support. Scholarships received by a full-time student are not counted as the child''s own support. For a qualifying relative, by contrast, *you* must provide more than half.'
  options:
  - text: You must provide more than half of the child's support
  - text: The child must not have provided more than half of his or her own support
    correct: true
  - text: The child must have no income
  - text: Both parents must provide equal support
- id: qr-gross-income
  prompt: Your 25-year-old unemployed brother lives with you. In 2025 he received $6,000 of taxable unemployment compensation and you paid for nearly everything else. Can you claim him as a qualifying relative?
  explanation: 'He isn''t a qualifying child (25 and not a disabled student), so the qualifying relative tests apply. He passes relationship and support, but fails the gross income test: the dependent''s gross income must be less than $5,200 for 2025, counting all taxable income (p. 57). Unemployment compensation is taxable, so it counts. Nontaxable income (such as Social Security that isn''t taxable) does not. The book lists exceptions for people under 19 or students under 24 (p. 58), but those apply to the qualifying child category; for a qualifying relative there is no age exception to the income test.'
  options:
  - text: No, because his gross income ($6,000) is not under the $5,200 limit for 2025
    correct: true
  - text: No, because brothers can't be dependents
  - text: Yes, because you paid more than half his support
  - text: Yes, because siblings are always dependents
- id: support-calc
  prompt: Your mother received $2,400 of Social Security and $400 of interest. She spent $2,000 on rent and $400 on recreation, and saved the rest. You spent $2,600 on her support. Who provided more than half of her support?
  explanation: 'The book''s example: “Even though her income was $2,800, she spent only $2,400 for her own support. If you spent more than $2,400 for her support, you can claim her as a dependent” (p. 59). Support is measured by what is actually spent, not what is received; savings don''t count. Total support = $2,400 + $2,600 = $5,000; your share is 52%, more than half. She also passes the gross income test, because her taxable gross income ($400 of interest plus any taxable Social Security) is under $5,200.'
  options:
  - text: Nobody; support must be computed on income
  - text: She did, because her income was $2,800
  - text: You did only if you paid her rent
  - text: 'You did: she spent $2,400 of her own money on her support, you spent $2,600, so your share is $2,600 ÷ $5,000 = 52%'
    correct: true
- id: multiple-support
  prompt: Three siblings each pay 30% of their father's support and the father pays 10%. Nobody pays more than half. Can anyone claim him?
  explanation: The multiple support rules require that you and other eligible persons together paid more than half, that you paid more than 10%, that no one paid more than half, that the other tests are met, and that each other contributor of more than 10% signs a statement (Form 2120) not to claim the person that year (pp. 59–60). The siblings can rotate the claim year to year. A dependent claimed this way does not, however, make the claimant eligible for head of household status (p. 52).
  options:
  - text: Yes; under a multiple support agreement the siblings, who together pay more than half and each more than 10%, can agree that one of them claims him (the others sign Form 2120 statements)
    correct: true
  - text: Only the oldest sibling can
  - text: All three can claim him
  - text: No; someone must pay more than half
- id: divorced-release
  prompt: After a divorce, the child lives with Mom 250 nights and with Dad 115 nights. Mom signs Form 8332 releasing the claim to Dad. What can Dad claim?
  explanation: The custodial parent is the one with more nights (p. 58). Form 8332 transfers the dependency claim and the child-related tax credits that follow it (the child tax credit/credit for other dependents). It does not transfer head of household status, the earned income credit or the child and dependent care credit, which stay with the custodial parent based on where the child lived. The book notes that the custodial parent keeps the dependent care credit and that either parent can deduct medical expenses he or she pays for the child (p. 58).
  options:
  - text: The child as a dependent and the child tax credit
    correct: true
  - text: Everything Mom could have claimed
  - text: The child tax credit, head of household status and the earned income credit
  - text: Nothing; Form 8332 is only for medical expenses
- id: tiebreaker
  prompt: Grandma, Mom and her 6-year-old all live together all year, and the child is a qualifying child of both Mom and Grandma. Grandma has higher AGI. If Mom chooses not to claim the child, who may?
  explanation: 'When a child is the qualifying child of more than one person, tie-breaker rules apply (Publication 501): a parent wins over a non-parent; between parents, the one with whom the child lived longer, then the higher AGI. If the parent doesn''t claim the child, another eligible person may, but only if that person''s AGI is higher than any parent''s AGI. The book''s general point is that each dependent “can put money into your pocket” and nontraditional families have to work through the rules (p. 56). The IRS Interactive Tax Assistant automates the decision.'
  options:
  - text: Both of them
  - text: Nobody, because Mom is the parent
  - text: Grandma, but only if her AGI is higher than Mom's (which it is)
    correct: true
  - text: Whichever filed first
- id: dependent-ssn
  prompt: Your dependent was born and died in 2025 and never got an SSN. What does the book say to do?
  explanation: '“If your dependent was born and died in 2025 and didn''t have a Social Security number, write ‘Died’ in column (2)” (p. 60); the IRS instructions add that you attach a birth certificate, death certificate or hospital records. Otherwise every dependent needs an identifying number: an SSN (required for the child tax credit), an ITIN, or an ATIN for a child in a pending US adoption (Form W-7A). “No Social Security number, no deduction, and no right to claim head of household status” (p. 60).'
  options:
  - text: Use your own SSN
  - text: You cannot claim the child at all
  - text: Apply for an ATIN
  - text: Write "Died" in the SSN column and attach a copy of the birth certificate, death certificate or hospital records
    correct: true
---
## Learning objectives

- Apply the qualifying child tests (relationship, age, residency, support, joint return)
- Apply the qualifying relative tests (household or relationship, joint return, citizenship, gross income, support)
- Handle divorced parents, multiple support agreements and identification numbers

## Qualifying child

A uniform definition used for dependency, HOH, the earned income credit, the child tax credit and the dependent care credit (p. 61):

- **Relationship**: child, stepchild, eligible foster child, adopted child (or one placed for adoption), sibling, step-sibling, or a descendant of any of these (a grandchild, niece or nephew) (p. 62).
- **Age**: under 19 at year end, or under 24 and a full-time student in some part of five months; any age if permanently and totally disabled. The child must also be younger than you (or your spouse). Credits add their own limits: under 17 for the child tax credit, under 13 for the dependent care credit (p. 62).
- **Residency**: lived with you more than half the year, counting nights; temporary absences (school, medical care, juvenile detention, military service) count as time at home; a child born or dying during the year counts if at home for the time alive (p. 62).
- **Support**: the child did not provide more than half of his or her own support (p. 62).
- **Joint return**: the child didn't file a joint return except only to claim a refund.

## Qualifying relative

For anyone else, five tests (pp. 56–60):

1. **Member of household or relative**: either lives with you all year, or is related (parents, grandparents, siblings, aunts and uncles, in-laws...). Cousins must live with you all year. A relationship that violates local law fails. Live-in employees fail.
2. **Not filing jointly** (except for a refund).
3. **Citizen, national or resident** of the US, Canada or Mexico.
4. **Gross income** under $5,200 for 2025 (nontaxable income such as untaxed Social Security doesn't count). *Correction:* the book lists exceptions for people under 19 or students under 24 (p. 58); those exceptions belong to the qualifying child test. A qualifying relative has no age exception to the gross income limit.
5. **Support**: you provided more than half of total support. Support includes food, lodging at fair rental value, clothing, medical care, education, and items for the person's own use. The person's own money counts only if actually spent on support (p. 59). A **multiple support agreement** (Form 2120) lets one member of a group that together provides more than half — each over 10% — claim the person.

**Divorced or separated parents.** The custodial parent (more nights) claims the child, unless he or she releases the claim on Form 8332 (or, for pre-2009 decrees, a qualifying decree page) to the noncustodial parent (p. 58). The release transfers the child tax credit and dependency, but not HOH, the earned income credit or the dependent care credit, which stay with the custodial parent.

**Identification numbers.** Each dependent needs an SSN, ITIN or (pending adoption) ATIN; the child tax credit requires an SSN (p. 60).

## Sources

- Tyson and Eck (2025), pp. 55–62.
- IRS, [Publication 501](https://www.irs.gov/publications/p501); IRS, [Interactive Tax Assistant: Whom may I claim as a dependent?](https://www.irs.gov/help/ita/whom-may-i-claim-as-a-dependent)
