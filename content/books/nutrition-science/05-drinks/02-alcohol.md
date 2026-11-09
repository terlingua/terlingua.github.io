---
title: Alcohol
questions:
  - id: iarc
    prompt: How does IARC classify alcoholic beverages?
    options:
      - text: Group 3, not classifiable
      - text: Group 2B, possibly carcinogenic
      - id: group1
        text: Group 1, carcinogenic to humans
        correct: true
    explanation: Alcohol causes cancers of the mouth, pharynx, larynx, oesophagus, liver, colorectum and female breast. Acetaldehyde, its metabolite, damages DNA.
  - id: breast
    prompt: Is there a threshold below which alcohol does not increase breast cancer risk?
    options:
      - text: Yes, below three drinks a day
      - id: no-threshold
        text: No clear threshold has been identified; risk rises from low intakes, about 7–10% per daily drink
        correct: true
      - text: Alcohol lowers breast cancer risk
    explanation: The US Surgeon General's January 2025 advisory highlighted that alcohol is the third leading preventable cause of cancer in the US after tobacco and obesity.
  - id: j-curve
    prompt: Why is the "moderate drinking protects the heart" J-curve now doubted?
    type: multiple
    options:
      - text: Non-drinker groups often include former drinkers who quit because of illness ("sick quitter" bias)
        correct: true
      - text: Moderate drinkers tend to be wealthier and healthier in other ways
        correct: true
      - text: Mendelian randomisation studies generally do not show a protective effect
        correct: true
      - text: Large RCTs have proven that wine prevents heart attacks
    explanation: No long-term RCT has tested moderate drinking on hard outcomes; the NIH-funded MACH15 trial was cancelled in 2018 after revelations of industry funding solicitation. Grade for cardioprotection - **Uncertain**, leaning **Not supported**.
  - id: standard-drink
    prompt: How much pure alcohol is in a US "standard drink"?
    options:
      - text: 5 g
      - id: fourteen
        text: 14 g
        correct: true
      - text: 50 g
    explanation: That is roughly 355 ml of 5% beer, 150 ml of 12% wine or 45 ml of 40% spirits. A UK unit is 8 g; definitions differ by country.
  - id: pregnancy
    prompt: What is the guidance on alcohol in pregnancy?
    options:
      - text: One glass of wine a day is beneficial
      - id: none-safe
        text: No safe amount has been established, so avoiding alcohol entirely is recommended
        correct: true
      - text: Only spirits are harmful
    explanation: Fetal alcohol spectrum disorders are entirely preventable. Low-level exposure effects are harder to measure, which is exactly why the advice is abstention.
  - id: resveratrol
    prompt: Red wine contains resveratrol. Is that a reason to drink it for health?
    options:
      - text: Yes, the resveratrol outweighs the alcohol
      - id: tiny-dose
        text: No — the amounts are tiny compared with doses used in animal studies, human trials are disappointing, and alcohol's harms are established
        correct: true
    explanation: You would need to drink hundreds of litres of wine to match some animal-study doses.
  - id: gbd
    prompt: The Global Burden of Disease alcohol analysis (Lancet 2018) concluded that the level of drinking that minimised overall health loss was what?
    options:
      - id: zero
        text: Zero standard drinks per week
        correct: true
      - text: Two drinks per day
      - text: Seven drinks per day
    explanation: A 2022 GBD update found small net benefits for some older adults with high cardiovascular risk, but recommended zero for people aged 15–39. Estimates of very-low-dose effects remain debated; harms at higher intakes are not.
  - id: dga-alcohol
    prompt: How did the 2025–2030 US Dietary Guidelines change alcohol advice compared with 2020–2025?
    options:
      - text: They recommended a daily glass of wine
      - id: vaguer
        text: They dropped the numeric limits (up to 2 drinks a day for men, 1 for women) in favour of a general "drink less" message
        correct: true
      - text: They banned alcohol
    explanation: Critics noted that removing numbers made the advice vaguer, while the cancer evidence had become stronger.
---
## Learning objectives

- Understand alcohol's established harms
- Evaluate the "moderate drinking is healthy" claim
- Know standard drink sizes

## What is established

| Effect | Grade |
|--------|-------|
| Causes at least seven cancers | **Established** |
| Liver disease (fatty liver, hepatitis, cirrhosis) | **Established** |
| Raises blood pressure and causes atrial fibrillation with dose | **Established** |
| Causes fetal alcohol spectrum disorders | **Established** |
| Injury, violence and road deaths | **Established** |
| Addiction (alcohol use disorder) | **Established** |
| Light drinking protects the heart | **Uncertain** — likely overstated by confounding |
| Any amount is safe for cancer | **Not supported** |
| Red wine is a health food | **Not supported** |

## Harm rises with dose

There is a broad consensus that **less is better**. In 2023 the WHO Regional Office for Europe stated that no
safe level of alcohol consumption for cancer has been identified. That is a statement about cancer risk;
total health effects at very low intakes are smaller and harder to measure, and scientists continue to argue
about them. The direction of advice is not in doubt.

## Sources

- IARC Monographs volume 100E. Alcohol consumption and ethyl carbamate. 2012.
- GBD 2016 Alcohol Collaborators. Alcohol use and burden for 195 countries and territories. *Lancet* 2018. [doi:10.1016/S0140-6736(18)31310-2](https://doi.org/10.1016/S0140-6736(18)31310-2)
- GBD 2020 Alcohol Collaborators. Population-level risks of alcohol consumption by amount, geography, age, sex, and year. *Lancet* 2022.
- US Surgeon General. Alcohol and Cancer Risk advisory. January 2025.
- Stockwell T et al. Do "moderate" drinkers have reduced mortality risk? A systematic review and meta-analysis. *J Stud Alcohol Drugs* 2016.
- Millwood IY et al. Conventional and genetic evidence on alcohol and vascular disease aetiology (China Kadoorie Biobank). *Lancet* 2019.
