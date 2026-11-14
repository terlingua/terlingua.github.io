---
title: 'Describing and Predicting: Observational and Correlational Methods'
questions:
- id: four-methods
  prompt: 'A team asks: "If we add mandatory code-review checklists, will escaped defects go down?" Which method in Table 2–1 is built to answer exactly that kind of question?'
  explanation: 'Table 2–1 (p. 26) pairs each method with a question. Observational: what is the phenomenon like? Correlational: from knowing X, can we predict Y? Experimental: is X a cause of Y? Intervention: “If we do X, will Y improve?” An intervention study applies knowledge to a real problem and tests whether the change helps; ideally it still uses random assignment (as an experiment does) so the improvement can be attributed to the intervention. The checklist question is about usefulness in the real setting, which is the intervention method''s focus.'
  options:
  - text: Observational
  - text: Correlational
  - text: Experimental
  - text: Intervention
    correct: true
- id: ethnography
  prompt: A researcher embeds with an open source community for a year, attending calls and reading mailing lists, trying to understand its norms from the members' own point of view without testing a prior hypothesis. Which method is this?
  explanation: Ethnography is “The method by which researchers attempt to understand a group or culture by observing it from the inside, without imposing any preconceived notions they might have” (p. 26, glossary p. 500). It is the chief method of cultural anthropology and is useful for describing a phenomenon and generating hypotheses. If the researcher instead counted specific, predefined behaviors (say, how often maintainers reject first-time contributors), that would be systematic observation, which requires clear definitions and checks that independent observers agree (interjudge reliability, p. 27).
  options:
  - text: Ethnography
    correct: true
  - text: Archival analysis
  - text: A meta-analysis
  - text: A field experiment
- id: archival
  prompt: A researcher analyzes ten years of a company's public postmortems and commit messages to see whether blame language decreased after a culture change. What kind of method is this?
  explanation: Archival analysis is “A form of the observational method in which the researcher examines the accumulated documents, or archives, of a culture (e.g., diaries, novels, magazines, and newspapers)” (p. 27, glossary p. 499). The book's example analyzed millions of tweets to track mood over time. Archives are cheap and unobtrusive, but they only describe; a drop in blame language after a culture change would be a correlation in time, not proof the change caused it.
  options:
  - text: Ethnography
  - text: Archival analysis
    correct: true
  - text: An intervention tournament
  - text: A laboratory experiment
- id: correlation-coefficient
  prompt: Across services, the number of on-call pages per week has a correlation of −0.6 with engineer satisfaction scores. What does this mean?
  explanation: 'The correlation coefficient is “A statistic that estimates how well you can predict one variable from another” (p. 28, glossary p. 500). It ranges from −1 to +1. A positive value means higher values of one go with higher values of the other; a negative value means higher values of one go with lower values of the other; zero means no linear predictive relationship. −0.6 is a moderately strong negative relationship. It does not tell you the direction of causation: unhappy teams might also neglect alert tuning, or a third variable (an under-resourced service) might cause both.'
  options:
  - text: Higher page counts go with lower satisfaction
    correct: true
  - text: Satisfaction is 60% lower for services with pages
  - text: There is no relationship between the two
  - text: More pages cause lower satisfaction
- id: three-explanations
  prompt: Teams that use pair programming have fewer production bugs. Which of the following are logically possible explanations of this correlation? Select all that apply.
  type: multiple
  explanation: 'When two variables correlate, “there are at least three possible causal relationships” (p. 30): A causes B, B causes A (reverse causation), or a third variable causes both. The book''s example: children who watch more violent TV are more aggressive, but aggressive children may seek violent TV, or neglectful parenting may cause both. The fourth option is the classic error: “Correlation does not prove causation. Unfortunately, forgetting this adage is one of the most common methodological errors in the social sciences” (p. 30). To find out which explanation is true you need an experiment that randomly assigns teams to pair or not.'
  options:
  - text: Pairing causes fewer bugs
    correct: true
  - text: Teams with fewer bugs have more slack time, which makes pairing affordable
    correct: true
  - text: A third variable, such as experienced leadership, leads both to pairing and to fewer bugs
    correct: true
  - text: Correlations of this kind always mean the first variable causes the second
- id: contraception-example
  prompt: In the Rosenberg et al. (1992) clinic study, women whose partners used condoms had more STIs than women who used diaphragms or sponges. Why can't we conclude that condoms caused the difference?
  explanation: 'The book uses this study as a warning (p. 30). The press concluded that diaphragms and sponges protected against disease, and some articles urged women to switch. But the data were correlational: “condom users were more likely to have had sex with multiple partners in the previous month,” and their partners may have been more likely to have STIs. “There is simply no way of knowing” which explanation is true from the correlation alone. The same trap appears in engineering telemetry: users of a feature churn less, but power users both adopt features and stay longer.'
  options:
  - text: Because the researchers used random assignment incorrectly
  - text: Because the sample was too large
  - text: The groups may differ in other ways, such as number of partners
    correct: true
  - text: Because sexually transmitted infections cannot be measured
- id: random-selection
  prompt: In 1936 the Literary Digest polled millions of people drawn from telephone directories and car registrations and predicted the wrong presidential winner. What went wrong?
  explanation: 'Random selection is “A way of ensuring that a sample of people is representative of a population by giving everyone in the population an equal chance of being selected for the sample” (p. 29, glossary p. 503). In the Depression, people with phones and cars were wealthier than average and more likely to vote Republican, so the huge sample was biased. Size does not fix bias. The engineering equivalent: surveying only users who opened your newsletter, or measuring latency only from clients in your own region, gives precise but biased estimates.'
  options:
  - text: People lied about how they would vote
  - text: The poll was taken too late
  - text: The sample was not randomly selected
    correct: true
  - text: The sample was too small
- id: self-report-limits
  prompt: Users in an interview say they abandoned onboarding because "the copy was confusing." Logging shows most abandoned at a slow-loading step. What does the book's discussion of self-report predict?
  explanation: The book warns that asking people to describe why they did something “is an invitation to inaccuracy. People’s reports about the causes of their responses often pertain more to their theories and beliefs about what should have influenced them than to what actually influenced them (Nisbett & Wilson, 1977)” (p. 30). Interviews are valuable for discovering what people think and feel, but behavioral data are better at revealing what caused the behavior. Chapter 4 returns to this as the limits of introspection and causal theories.
  options:
  - text: Logs are always wrong when they disagree with users
  - text: People often report what they think should have influenced them
    correct: true
  - text: Self-reports about causes are always accurate if people are anonymous
  - text: People never know anything about their own behavior
- id: observational-limits
  prompt: What is a key limitation of the observational method?
  explanation: The observational method is “The technique whereby a researcher observes people and systematically records measurements or impressions of their behavior” (p. 26, glossary p. 502). It excels at describing a phenomenon and generating hypotheses. But you cannot observe what is rare (emergencies), private, or happened in the past, and archives may not record what you need. Most importantly, observing that two things co-occur does not tell you whether one causes the other, which is why the book moves on to correlational and experimental methods.
  options:
  - text: It cannot describe behavior at all
  - text: It can only be used with animals
  - text: It cannot show causes and misses rare or private behavior
    correct: true
  - text: It requires random assignment, which is often unethical
---
## Learning objectives

- Name the four research methods and the question each answers
- Describe ethnography and archival analysis, and the limits of observation
- Explain the correlation coefficient and what positive, negative and zero correlations mean
- Explain random selection in surveys and why the Literary Digest poll failed
- Explain why correlation does not equal causation, including reverse causation and third variables

## Key ideas

| Method | Focus | Question |
|---|---|---|
| Observational | Description | What is the nature of the phenomenon? |
| Correlational | Prediction | From knowing X, can we predict Y? |
| Experimental | Causality | Is X a cause of Y? |
| Intervention | Usefulness | If we do X, will Y improve? |

(Table 2–1, p. 26)

**Observational method** (p. 26): ethnography observes a group from the inside without imposing preconceived notions; systematic observation defines behaviors in advance and checks interjudge reliability; **archival analysis** studies the documents of a culture (p. 27). Limits: some behavior is rare or private.

**Correlational method** (p. 27): measure two or more variables and assess how well one predicts the other. The **correlation coefficient** ranges from −1 to +1 (p. 28). **Surveys** need **random selection** (p. 29); the 1936 Literary Digest poll drew from phone books and car registrations and wrongly predicted Landon would beat Roosevelt. Self-reports can be inaccurate: people often report their theories about what influenced them (Nisbett & Wilson, 1977; p. 30).

**Correlation does not equal causation** (p. 30): if A and B correlate, A may cause B, B may cause A, or a third variable may cause both.

## Sources

- Aronson et al., *Social Psychology* 12e, ch. 2, pp. 25–31
- Nisbett, R. E., & Wilson, T. D. (1977). Telling more than we can know. *Psychological Review*, 84, 231–259
- Rosenberg, M. J., et al. (1992). Barrier contraceptives and sexually transmitted diseases in women. *American Journal of Public Health*, 82, 669–674
