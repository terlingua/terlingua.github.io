---
title: The evidence hierarchy
questions:
  - id: confounding
    prompt: A cohort study finds that people who eat more blueberries live longer. What is the most important reason this does not prove that blueberries extend life?
    options:
      - text: Cohort studies are always fraudulent
      - text: Blueberries contain no nutrients
      - id: confounders
        text: Blueberry eaters may differ in income, exercise, smoking and many other factors that also affect lifespan
        correct: true
      - text: Cohort studies cannot measure death
    explanation: This is **confounding**. Statistical adjustment helps but can never fully remove differences that were measured badly or not measured at all ("residual confounding").
  - id: rct-strength
    prompt: Why is a large randomised controlled trial (RCT) the strongest single design for testing whether a diet causes an outcome?
    options:
      - id: balances
        text: Random allocation balances known and unknown confounders between groups on average
        correct: true
      - text: RCTs never have dropouts
      - text: RCTs always last for decades
      - text: RCTs do not need statistics
    explanation: Randomisation is the only method that balances factors nobody has thought of. RCTs still have weaknesses — short duration, poor adherence, and unrepresentative participants.
  - id: rct-limits
    prompt: Which are genuine limitations of diet RCTs?
    type: multiple
    options:
      - text: Chronic diseases take decades to develop, but most diet trials last months to a few years
        correct: true
      - text: Participants often drift back to their usual diet, shrinking the difference between groups
        correct: true
      - text: Blinding is usually impossible because people know what they are eating
        correct: true
      - text: Randomisation makes the results apply to every population on Earth automatically
    explanation: Randomisation buys internal validity, not universal generalisability. Adherence and duration are the chronic weak points of nutrition RCTs.
  - id: self-report
    prompt: What have doubly labelled water studies revealed about self-reported food intake?
    options:
      - text: People report their intake almost perfectly
      - text: People systematically over-report how much they eat
      - id: under
        text: People systematically under-report energy intake, and the error is larger in people with higher body weight
        correct: true
    explanation: Doubly labelled water measures true energy expenditure. Comparisons show substantial, non-random under-reporting, which is one reason food-frequency questionnaires are a weak measurement tool.
  - id: mechanistic
    prompt: A compound kills cancer cells in a Petri dish. What can you conclude about eating foods that contain it?
    options:
      - text: The food prevents cancer
      - text: The food cures cancer
      - id: almost-nothing
        text: Almost nothing yet — dose, absorption, metabolism and whole-body effects are untested
        correct: true
      - text: The food is dangerous
    explanation: Bleach also kills cancer cells in a dish. In-vitro findings generate hypotheses; they are the bottom of the evidence pyramid for human health claims.
  - id: mr
    prompt: What does Mendelian randomisation use as a natural experiment?
    options:
      - text: Random selection of foods in supermarkets
      - id: genes
        text: Genetic variants, randomly assigned at conception, that affect exposure to a nutrient or risk factor
        correct: true
      - text: Random weather patterns that affect crop yields
    explanation: Because genes are allocated randomly at conception, they are less confounded by lifestyle. The method has its own assumptions (for example, that the gene affects the outcome only through the exposure).
  - id: meta
    prompt: A meta-analysis pools twenty small, biased trials. What is the result?
    options:
      - text: A highly reliable answer, because twenty is a large number
      - id: precise-biased
        text: A precise estimate of a biased answer — pooling does not remove shared bias
        correct: true
      - text: An answer that is always null
    explanation: Garbage in, garbage out. Good meta-analyses assess risk of bias in each study and check for publication bias.
---
## Learning objectives

- Rank study designs by the strength of causal evidence they provide
- Explain confounding, reverse causation and measurement error
- Recognise why diet research is unusually hard

## Why nutrition science seems to change its mind

Headlines flip between "coffee kills" and "coffee saves lives" because most nutrition headlines come
from **observational** studies, which find associations, not causes. The underlying science moves far
more slowly than the headlines. Some conclusions — that vitamin C prevents scurvy, that folic acid
prevents neural tube defects, that trans fats raise heart disease risk, that excess energy intake causes
weight gain — are as solid as anything in biology. Others — the ideal ratio of fats, the health effect
of a single food — remain open.

## The hierarchy, from weakest to strongest

| Design | What it can show | Main weakness |
|--------|------------------|---------------|
| Anecdote, testimonial, tradition | That something happened to someone | No comparison, no control for chance or bias |
| In-vitro (cell) and animal studies | Plausible mechanisms | Doses and biology often do not translate to humans |
| Cross-sectional survey | Associations at one point in time | Cannot tell which came first |
| Case-control study | Associations with rare diseases | Recall bias: sick people remember diets differently |
| Prospective cohort | Associations over years, with exposure measured before disease | Confounding and measurement error |
| Mendelian randomisation | Causal estimates using genetic "natural experiments" | Assumptions about pleiotropy can fail |
| Randomised controlled trial | Cause and effect | Short, small, poor adherence, often unblinded |
| Systematic review and meta-analysis of RCTs | The overall weight of trial evidence | Only as good as the trials it pools |

Evidence is strongest when **different designs with different weaknesses agree**. The case that smoking
causes lung cancer, and that LDL cholesterol causes atherosclerosis, rests on exactly this
convergence: cohorts, trials, genetics and mechanisms all point the same way.

## The three classic traps

**Confounding.** People who eat more vegetables also tend to smoke less, exercise more and earn more.
Adjusting for measured factors helps; unmeasured ones remain.

**Reverse causation.** People who have just been diagnosed with heart disease often change their diet.
A study might then wrongly conclude that the "healthy" diet is linked with heart disease.

**Measurement error.** Most cohort studies ask people what they ate over the past year using a
food-frequency questionnaire. Doubly labelled water studies show that people under-report energy
intake, typically by hundreds of calories per day, and not randomly.

## Why diet is harder than drugs

- You cannot give a placebo steak; blinding is rarely possible
- Changing one nutrient changes others (cutting fat raises carbohydrate or protein)
- Effects are usually small and take decades to show
- Food is eaten in patterns, not as isolated nutrients
- Funding is limited compared with drug trials, so many trials are small and short

## The multiple-comparisons problem

Schoenfeld and Ioannidis took 50 common ingredients from a cookbook and searched the literature:
40 of them had been studied for cancer risk, and most of those had published associations — some
claiming increased risk, some decreased, often for the same food. Most single studies reporting a
single food–cancer association show implausibly large effects that shrink in meta-analyses.

> **Rule of thumb:** a single observational study about a single food is a hypothesis, not a finding.

## Sources

- Schoenfeld JD, Ioannidis JPA. Is everything we eat associated with cancer? *Am J Clin Nutr* 2013. [doi:10.3945/ajcn.112.047142](https://doi.org/10.3945/ajcn.112.047142)
- Ioannidis JPA. The challenge of reforming nutritional epidemiologic research. *JAMA* 2018.
- Satija A, Yu E, Willett WC, Hu FB. Understanding nutritional epidemiology and its role in policy. *Adv Nutr* 2015.
- Ference BA et al. LDL causes atherosclerotic cardiovascular disease (EAS consensus). *Eur Heart J* 2017. [doi:10.1093/eurheartj/ehx144](https://doi.org/10.1093/eurheartj/ehx144)
