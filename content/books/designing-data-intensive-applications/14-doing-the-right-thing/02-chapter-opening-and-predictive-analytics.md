---
title: "Chapter Opening and Predictive Analytics"
questions:
  - id: ethics-process
    prompt: "How does the chapter characterize ethics in practice?"
    options:
      - id: oafeaa6
        text: "A compliance checklist"
      - id: o11acf7
        text: "Something only lawyers handle"
      - id: o724155
        text: "A fixed set of rules"
      - id: o8061a2
        text: "A participatory, iterative process of reflection with the people involved and accountability for results"
        correct: true
    explanation: "“Ethics is not going through a checklist to confirm you comply; it’s a participatory and iterative process of reflection, in dialog with the people involved, with accountability for the results.”"
  - id: tech-neutral
    prompt: "What does the chapter say about whether technology is good or bad?"
    options:
      - id: o7774d9
        text: "Software is always good"
      - id: o74097e
        text: "Only weapons have ethical implications"
      - id: o0afbd4
        text: "It isn't in itself; what matters is how it's used and how it affects people, and engineers bear responsibility"
        correct: true
      - id: o3b2c9a
        text: "Software is always harmful"
    explanation: "“A technology is not good or bad in itself—what matters is how it is used and how it affects people ... it is not … sufficient for software engineers to focus exclusively on the technology and ignore its consequences.”"
  - id: say-no
    prompt: "Why do organizations using predictive analytics tend to say \"no\" when in doubt?"
    options:
      - id: o343437
        text: "Saying \"no\" improves accuracy"
      - id: o3f7607
        text: "Algorithms can only output \"no\""
      - id: o9675e8
        text: "Regulations require it"
      - id: obda196
        text: "A missed opportunity costs them little, while a bad loan or problematic hire costs much more"
        correct: true
    explanation: "“the cost of a missed business opportunity is low, but the cost of a bad loan or a problematic employee is much higher ... If in doubt, they are better off saying no.”"
  - id: algorithmic-prison
    prompt: "What is \"algorithmic prison\"?"
    options:
      - id: o6a82e7
        text: "A jail sentence for programmers"
      - id: o03d8cb
        text: "A sandbox for testing algorithms"
      - id: ob60012
        text: "A prison run by AI"
      - id: oa1ccc8
        text: "Being systematically excluded from jobs, travel, insurance, housing and finance after being labeled risky by algorithms"
        correct: true
    explanation: "“Systematically being excluded from jobs, air travel, insurance coverage, property rental, financial services, and other key aspects of society is such a large constraint of an individual’s freedom that it has been called “algorithmic prison””."
  - id: presumption
    prompt: "How do automated exclusion systems contrast with criminal justice in rights-respecting countries?"
    options:
      - id: od2ba03
        text: "Justice presumes innocence; automated systems can exclude people without proof and with little appeal"
        correct: true
      - id: oedaf98
        text: "They are identical"
      - id: oac0f1c
        text: "Both presume guilt"
      - id: of971c3
        text: "Automated systems always allow appeal"
    explanation: "“the criminal justice system presumes innocence until proven guilty; on the other hand, automated systems can systematically and arbitrarily exclude a person from participating in society without any proof of guilt, and with little chance of appeal.”"
  - id: inferred-rules
    prompt: "How does predictive analytics differ from automating a human decision with explicit rules?"
    options:
      - id: o0377f3
        text: "It doesn't use data"
      - id: oe8a370
        text: "It is always transparent"
      - id: o39b471
        text: "It always uses explicit if/then rules"
      - id: o927de8
        text: "The rules themselves are inferred from data and are opaque"
        correct: true
    explanation: "“we are not merely automating a human’s decision by using software to specify the rules for when to say yes or no; we are leaving the rules themselves to be inferred from data. However, the patterns learned by these systems are opaque”."
  - id: amplify
    prompt: "What happens if a model's input data carries a systematic bias?"
    options:
      - id: o103104
        text: "The bias cancels out"
      - id: o49d4b9
        text: "The system will most likely learn and amplify it"
        correct: true
      - id: o28e9b5
        text: "The bias disappears with more data"
      - id: of69a60
        text: "The model refuses to train"
    explanation: "“If the input to an algorithm carries a systematic bias, the system will most likely learn and amplify that bias in its output.”"
  - id: proxy-book
    prompt: "Which example of a proxy for a protected trait does the chapter give?"
    options:
      - id: o156e2a
        text: "Browser type predicting religion"
      - id: oe1c86d
        text: "Postal code or IP address in segregated neighbourhoods predicting race"
        correct: true
      - id: oa35a77
        text: "Screen size predicting disability"
      - id: oe6b47e
        text: "Favourite color predicting age"
    explanation: "“in racially segregated neighborhoods, a person’s postal code or even their IP address is a strong predictor of race.”"
  - id: money-laundering
    prompt: "What does the satirical phrase about money laundering criticize?"
    options:
      - id: oe26e8e
        text: "The belief that biased data can produce fair, impartial output through an algorithm"
        correct: true
      - id: oeaa166
        text: "Cloud billing"
      - id: ode6d7f
        text: "Cryptocurrency fraud"
      - id: oa6f7ef
        text: "Tax software"
    explanation: "The belief is satirized as ““machine learning is like money laundering for bias””. The chapter calls it “ridiculous to believe that an algorithm could somehow take biased data as input and produce fair and impartial output from it”."
  - id: moral-imagination
    prompt: "What does the chapter say is required for the future to be better than the past?"
    options:
      - id: o4582ee
        text: "Larger models"
      - id: o6f1937
        text: "Faster hardware"
      - id: o76b71a
        text: "More data"
      - id: ocd2fdd
        text: "Moral imagination, which only humans can provide"
        correct: true
    explanation: "“Predictive analytics systems merely extrapolate from the past; if the past is discriminatory, they codify and amplify that discrimination. If we want the future to be better than the past, moral imagination is required, and that’s something only humans can provide.”"
  - id: accountability
    prompt: "Which accountability questions does the chapter raise?"
    type: multiple
    options:
      - id: od61dea
        text: "Is there recourse if credit scoring discriminates?"
        correct: true
      - id: ob03f37
        text: "Which GPU trained the model?"
      - id: o1c030b
        text: "Who is responsible when a self-driving car causes an accident?"
        correct: true
      - id: oa451ff
        text: "Can you explain an ML decision to a judge?"
        correct: true
    explanation: "The chapter asks all three and concludes “People should not be able to evade their responsibility by blaming an algorithm.”"
  - id: credit-vs-ml
    prompt: "How do traditional credit scores differ from ML-based predictive analytics?"
    options:
      - id: o894d41
        text: "ML scores are more transparent"
      - id: o188682
        text: "They are identical"
      - id: o6e1fc9
        text: "Credit scores use more inputs"
      - id: occ094b
        text: "Credit scores are based on your own borrowing history and correctable; ML predictions rely on how similar people behaved and are opaque"
        correct: true
    explanation: "“A credit score summarizes “How did you behave in the past?,” whereas predictive analytics usually work on the basis of “Who is similar to you, and how did people like you behave in the past?””."
  - id: life-expectancy
    prompt: "What does the life-expectancy example illustrate?"
    options:
      - id: oa3270f
        text: "Averages predict individuals exactly"
      - id: o248fec
        text: "A correct overall distribution says little about any individual case"
        correct: true
      - id: o9245e8
        text: "Statistics are always wrong"
      - id: ocd6deb
        text: "People die on their expected date"
    explanation: "“if the average life expectancy in your country is 80 years, that doesn’t mean you’re expected to drop dead on your 80th birthday ... the output of a prediction system is probabilistic and may well be wrong in individual cases.”"
  - id: dual-use
    prompt: "What double-edged use of analytics does the chapter note?"
    options:
      - id: o11b2de
        text: "It can only help people"
      - id: o743d8a
        text: "It can only harm people"
      - id: o065589
        text: "It has no effect"
      - id: o1716b5
        text: "It could focus aid on those who need it, but is also used by predatory businesses to target vulnerable people"
        correct: true
    explanation: "“this power could be used to focus aid and support to help those who need it most. On the other hand, it is sometimes used by predatory businesses seeking to identify vulnerable people”."
  - id: echo
    prompt: "What risk do recommendation systems pose even though they seem low-stakes?"
    options:
      - id: o55a19b
        text: "Slower page loads"
      - id: oa7e7e5
        text: "Better diversity of views"
      - id: o8b1936
        text: "Echo chambers where stereotypes, misinformation and polarization breed"
        correct: true
      - id: o1dcd60
        text: "Higher storage costs"
    explanation: "Services may “end up showing people only opinions they already agree with, leading to echo chambers in which stereotypes, misinformation, and polarization can breed.”"
  - id: credit-loop
    prompt: "How can using credit scores in hiring create a downward spiral?"
    options:
      - id: o8222a3
        text: "Misfortune lowers your score, making it harder to find work, which worsens your finances and score further"
        correct: true
      - id: o85eb96
        text: "Employers ignore scores"
      - id: ofdfda7
        text: "It speeds up hiring fairly"
      - id: o46b4d6
        text: "Scores improve when unemployed"
    explanation: "“As you miss payments on your bills, your credit score suffers, and you will be less likely to find work. Joblessness pushes you toward poverty, which further worsens your score”."
  - id: collusion
    prompt: "What happened when German gas stations adopted algorithmic pricing?"
    options:
      - id: oc51909
        text: "Prices fell sharply"
      - id: o2ee4cc
        text: "Algorithms learned to collude, reducing competition and raising prices"
        correct: true
      - id: of3e361
        text: "Stations went out of business"
      - id: o392491
        text: "Nothing changed"
    explanation: "“when gas stations in Germany introduced algorithmic prices, competition was reduced and prices for consumers went up because the algorithms learned to collude.”"
  - id: systems-thinking
    prompt: "What approach helps anticipate feedback loops?"
    options:
      - id: o3087db
        text: "More data collection"
      - id: occ5751
        text: "Systems thinking — considering the whole system including the people interacting with it"
        correct: true
      - id: o3e2e4f
        text: "Unit testing"
      - id: o18492b
        text: "Faster deployment"
    explanation: "“many consequences can be predicted by thinking about the entire system (not just the computerized parts, but also the people interacting with it)—an approach known as systems thinking.”"
---
Every system has intended and unintended consequences, and many datasets are about people. Predictive
analytics can exclude people unfairly, launder bias, evade accountability and create feedback loops.

## Learning objectives

- Explain why ethics is a participatory process, not a checklist
- Explain "algorithmic prison" and why organizations err on the side of "no"
- Explain how ML can learn and amplify bias, including via proxies
- Discuss accountability, recourse and the limits of probabilistic predictions
- Recognize feedback loops and apply systems thinking

## In a nutshell

- Technology isn't good or bad in itself; how it's used matters.
- Predictions about people have direct effects on their lives.
- "Machine learning is like money laundering for bias."
- Credit scores ask "How did you behave?"; predictive analytics ask "How did people like you behave?"
- Feedback loops: credit scores in hiring, algorithmic price collusion.

## Connections to today

- **The EU AI Act** classifies credit scoring and hiring systems as high-risk, imposing requirements on
  data governance, transparency and human oversight.
- **LLM-based screening tools** inherit biases in training data — the same "past codified" problem.
- **Recommendation feedback loops** shape what news and videos people see.
