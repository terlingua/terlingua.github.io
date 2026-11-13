---
title: "5.3 Categorical Variables"
questions:
  - id: categorical-definition
    prompt: "What makes a variable *categorical* in the book's sense?"
    options:
      - text: "It is stored as a string"
      - text: "It has fewer than ten values"
      - text: "It is the outcome of a classifier"
      - text: "It is discrete and unordered — no case can be half in one category, and no category is larger than another"
        correct: true
    explanation: "**p. 153:** “A category here means discrete and unordered.” Taxonomic group is categorical “because no species can be half-ape and half-monkey (discreteness), and there is no sense in which one is larger or smaller than the other (unordered).” Cloud region, browser family, HTTP method and team are categorical. HTTP status *class* (2xx < 4xx < 5xx) may look ordered, but the numbers are labels; treating them as quantities is a bug."
  - id: continuous-countries
    prompt: "The box “Continuous countries” describes a published finding that was reversed on reanalysis. What went wrong?"
    options:
      - text: "The country variable, which is categorical, was entered as if it were a continuous number"
        correct: true
      - text: "The sample was too small"
      - text: "The priors were too narrow"
      - text: "The outcome was misreported"
    explanation: "A 2015 study of children in six countries reported a negative association between religiosity and generosity. **p. 153:** “Upon reanalysis, it was found that the country variable, which is categorical, was entered as a continuous variable instead. This made Canada (value 2) twice as much “country” as the United States (value 1).” After reanalysis with country as a category, the result vanished and the paper was retracted. In code, this is the bug of feeding an enum's integer value into arithmetic. Always check that categorical IDs (region IDs, tenant IDs) never become numeric features."
  - id: indicator-variable
    prompt: "In `μᵢ = α + βm mᵢ` with mᵢ = 1 for males and 0 otherwise, what does βm represent?"
    options:
      - text: "The average male height"
      - text: "The expected difference in height between males and females"
        correct: true
      - text: "The average female height"
      - text: "The probability of being male"
    explanation: "**p. 154:** when mᵢ = 1 the linear model is µᵢ = α + βm, and when mᵢ = 0 it is simply µᵢ = α. “Using this approach means that βm represents the expected difference between males and females in height.” The indicator turns a parameter on for one category and off for the other — a boolean flag multiplying a coefficient."
  - id: indicator-prior-problem
    prompt: "What problem do indicator variables create for priors?"
    options:
      - text: "They make the model unidentifiable"
      - text: "They require flat priors"
      - text: "The category coded 1 uses two parameters (α + βm), so its prior is wider than the other category's, implying more prior uncertainty for one category for no reason"
        correct: true
      - text: "They prevent prior predictive simulation"
    explanation: "**p. 155:** simulating priors gives a standard deviation of about 20 for females and about 22.4 for males: “The prior for males is wider, because it uses both parameters ... We aren’t actually more unsure about male height than female height, a priori.” Asymmetric prior uncertainty is an accidental assumption introduced by the encoding."
  - id: index-variable
    prompt: "How does an *index variable* model work?"
    options:
      - text: "It creates one indicator column per category"
      - text: "It replaces categories with their frequency"
      - text: "It orders the categories by their mean"
      - text: "It assigns each category an integer that selects its own parameter, μᵢ = α[sex[i]], so each category gets the same prior"
        correct: true
    explanation: "**p. 155:** “An index variable contains integers that correspond to different categories. The integers are just names, but they also let us reference a list of corresponding parameters, one for each category.” Then “the same prior can be assigned to each, corresponding to the notion that all the categories are the same, prior to the data.” Programmers will recognize it as an array lookup: `alpha[sexIndex[i]]`. It extends to any number of categories with no change to the model code."
  - id: k-minus-one
    prompt: "With k categories, how many indicator variables does the indicator approach need, and how does the index approach compare?"
    options:
      - text: "k − 1 indicators plus an intercept; the index approach just has one parameter per category and the specification does not change when categories are added"
        correct: true
      - text: "k indicators; the index approach needs k² parameters"
      - text: "One indicator; the index approach needs k − 1"
      - text: "The two approaches cannot represent more than two categories"
    explanation: "**p. 156:** “when there are more than two categories, the indicator variable approach explodes ... If you have k unique categories, you need k − 1 indicator variables.” The index approach “does not change at all when you add more categories.” It is the difference between `if/else` chains per category and a dictionary lookup."
  - id: contrast
    prompt: "You fit separate means for two categories. How do you get the posterior of their difference?"
    options:
      - text: "Subtract the posterior means and report that number"
      - text: "Compute the difference for each posterior sample, producing a contrast with its own posterior distribution"
        correct: true
      - text: "Check whether their intervals overlap"
      - text: "Refit the model with only one category"
    explanation: "**p. 156:** `post$diff_fm <- post$a[,1] - post$a[,2]` gives the female–male difference, about −7.7 cm with an 89% interval of roughly −11.4 to −4.0. “This kind of calculation is called a contrast. No matter how many categories you have, you can use samples from the posterior to compute the contrast between any two.” For an A/B/C test, compute B − A and C − A per sample; never eyeball overlapping intervals."
  - id: significance-difference
    prompt: "βf = 0.15 ± 0.02 (clearly above zero) and βm = 0.02 ± 0.10 (overlapping zero). Can you conclude the two slopes differ?"
    options:
      - text: "Yes, because one is significant and the other is not"
      - text: "Yes, because their means differ by 0.13"
      - text: "No — the difference is about 0.13 ± 0.10, which overlaps zero substantially; you must compute the contrast"
        correct: true
      - text: "No, because both are positive"
    explanation: "From the box “Differences and statistical significance” (p. 158): “you can be confident that βf is far from zero, but you cannot be sure that the difference between βf and βm is far from zero.” The difference of “significant” and “not significant” is not itself necessarily significant (Gelman and Stern 2006). In experiment reviews: 'the change helped on iOS (significant) but not Android (not significant)' does not show the effect differs by platform."
  - id: multiple-categoricals
    prompt: "How do you add a second categorical variable, such as a randomly assigned 'house', to the clade model?"
    options:
      - text: "Multiply the two index variables"
      - text: "Create one index for every combination"
      - text: "It is not possible with index variables"
      - text: "Add another indexed parameter vector: μᵢ = α[clade[i]] + h[house[i]]"
        correct: true
    explanation: "**p. 158:** `mu <- a[clade_id] + h[house]` with `h[house] ~ dnorm(0, 0.5)`. Each categorical gets its own parameter vector. The book's tongue-in-cheek houses were assigned at random, yet one stands out — a reminder that random noise can produce an apparently 'interesting' category, especially with small groups. Chapter 13 shows how partial pooling tames this."
  - id: track-indices
    prompt: "What practical warning does the book give about index values and R factors?"
    options:
      - text: "Be very careful to keep track of which index values go with which categories; don't trust automatic factor conversion"
        correct: true
      - text: "Index values must start at zero"
      - text: "Factors are always correct"
      - text: "Always use alphabetical order"
    explanation: "**p. 157:** “In practice, you have to be very careful to keep track of which index values go with which categories. Don’t trust R’s factor variable type to necessarily do things right.” The same bug appears whenever a mapping from strings to integer IDs is implicit — e.g. enum reordering in a new release that silently remaps stored values. Keep the mapping explicit and label outputs."
---

How to put unordered categories into a model — and why the book prefers index variables to dummy variables.

## Learning objectives

- Identify categorical variables and the danger of treating them as numbers
- Build models with indicator and index variables and compare their priors
- Compute contrasts between categories from posterior samples
- Avoid the 'significant versus not significant' fallacy

## In a nutshell

| Approach | Model | Priors | Scales to k categories |
|---|---|---|---|
| Indicator (dummy) | μ = α + β·m | Unequal across categories | k − 1 indicators |
| Index | μ = α[category] | Same for every category | One vector, unchanged code |

```csharp
var alpha = new double[] { 134.9, 142.6 };
double Mu(int sexIndex) => alpha[sexIndex];
var contrast = posterior.Select(s => s.Alpha[0] - s.Alpha[1]);
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 5.3, pp. 153–158.
- Gelman A, Stern H. The difference between “significant” and “not significant” is not itself statistically significant. *The American Statistician* 2006. [doi:10.1198/000313006X152649](https://doi.org/10.1198/000313006X152649)
- Shariff AF et al. Religiosity is not negatively associated with generosity. *Current Biology* 2016 — the reanalysis behind “Continuous countries”.
