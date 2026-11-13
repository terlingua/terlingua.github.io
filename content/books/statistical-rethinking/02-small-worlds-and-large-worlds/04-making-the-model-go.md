---
title: "2.4 Making the Model Go"
questions:
  - id: posterior-definition
    prompt: "What is the posterior distribution in the globe model?"
    options:
      - text: "Pr(W, L | p), the probability of the data given p"
      - text: "Pr(p), the plausibility of p before seeing data"
      - text: "The single best value of p"
      - text: "Pr(p | W, L), the probability of each possible value of p conditional on the observed data and the model"
        correct: true
    explanation: "**p. 36:** “The posterior distribution takes the form of the probability of the parameters, conditional on the data. In this case, it would be Pr(p|W, L), the probability of each possible value of p, conditional on the specific W and L that we observed.” It is a whole distribution, not a number. Bayesian results are therefore like latency histograms rather than single averages: they carry the full shape of what you know."
  - id: bayes-words
    prompt: "In words, Bayes' theorem says the posterior equals what?"
    options:
      - text: "Probability of the data × prior, divided by the average probability of the data"
        correct: true
      - text: "Likelihood plus prior"
      - text: "Probability of the data under the null hypothesis"
      - text: "Prior divided by likelihood"
    explanation: "**p. 37:** `Posterior = Probability of the data × Prior / Average probability of the data`. The denominator, Pr(W, L), is the likelihood averaged over the prior; the book notes it is often called the 'evidence' or 'average likelihood' and its job is to standardize the posterior so it sums to 1. The book's key lesson: “the posterior is proportional to the product of the prior and the probability of the data.” That is why grid approximation can skip the denominator and normalize at the end — just divide by the sum."
  - id: not-about-bayes-theorem
    prompt: "What distinguishes Bayesian data analysis, according to the box “Bayesian data analysis isn’t about Bayes’ theorem”?"
    options:
      - text: "Using Bayes' theorem at all"
      - text: "Using Bayes' theorem more generally, to quantify uncertainty about unobservable things such as parameters and models"
        correct: true
      - text: "Using only uniform priors"
      - text: "Avoiding probability theory"
    explanation: "**p. 38:** “This is a mistake. Inference under any probability concept will eventually make use of Bayes’ theorem.” The medical-test examples (rare disease, positive test) are frequency calculations that any statistician would accept. Instead, “Bayesian approaches get to use Bayes’ theorem more generally, to quantify uncertainty about theoretical entities that cannot be observed, like parameters and models” (p. 39). Computing P(outage | alert) from historical counts is not especially Bayesian; putting a distribution on an unobservable fault rate is."
  - id: prior-shapes
    prompt: "Figure 2.6 multiplies the same likelihood by three different priors: flat, a step that rules out p < 0.5, and a sharp peak at 0.5. What does it show?"
    options:
      - text: "The posterior is the same for all three priors"
      - text: "Priors only matter when the likelihood is flat"
      - text: "Different priors with the same likelihood produce different posteriors, and the step prior makes p < 0.5 impossible in the posterior"
        correct: true
      - text: "The prior is ignored when there is any data"
    explanation: "The posterior is prior × likelihood, pointwise. Where the prior is zero, the posterior is zero, regardless of data. With modest data (9 tosses), the prior visibly reshapes the posterior; with much more data, the likelihood dominates and the posteriors converge. The engineering lesson is to make the prior's influence visible: plot prior, likelihood and posterior together, the way you would plot input, transform and output when debugging a pipeline."
  - id: three-engines
    prompt: "Which three *conditioning engines* does the book use to compute posteriors?"
    options:
      - text: "Exact integration, symbolic algebra and lookup tables"
      - text: "Gradient descent, Newton's method and bisection"
      - text: "Bootstrap, jackknife and permutation"
      - text: "Grid approximation, quadratic approximation and Markov chain Monte Carlo"
        correct: true
    explanation: "**p. 39:** “(1) Grid approximation (2) Quadratic approximation (3) Markov chain Monte Carlo (MCMC)”. Real models rarely have closed-form posteriors, so you need numerical engines. The engine is an implementation detail behind one interface — 'give me the posterior' — and the book teaches all three so you understand the trade-offs, the way you might understand a hash map, a B-tree and a skip list as implementations of a lookup."
  - id: grid-approx
    prompt: "How does grid approximation compute the posterior for one parameter p?"
    options:
      - text: "It evaluates prior × likelihood at a finite list of p values, then divides each product by their sum"
        correct: true
      - text: "It fits a Gaussian at the peak"
      - text: "It solves Bayes' theorem symbolically"
      - text: "It samples random values of p until convergence"
    explanation: "The recipe (p. 40): define the grid, compute the prior at each point, compute the likelihood at each point, multiply, standardize. In C# terms: `var grid = Enumerable.Range(0, 20).Select(i => i / 19.0); var unstd = grid.Select(p => Prior(p) * Binomial(6, 9, p)); var post = unstd.Select(u => u / unstd.Sum());`. More grid points give a better approximation. It is exact counting, discretized."
  - id: grid-scaling
    prompt: "Why does grid approximation fade away after the early chapters?"
    options:
      - text: "It gives wrong answers"
      - text: "It scales very poorly: with 100 grid points per parameter, 10 parameters need 100^10 evaluations"
        correct: true
      - text: "It only works with uniform priors"
      - text: "R does not support it"
    explanation: "**p. 40:** “The reason is that it scales very poorly, as the number of parameters increases.” The number of grid points is exponential in the number of parameters — the curse of dimensionality. It is like brute-forcing a configuration space: fine for two flags, hopeless for twenty. **p. 42:** “it’s routine to have models with hundreds or thousands of parameters.”"
  - id: quadratic-approx
    prompt: "Why is it called *quadratic* approximation?"
    options:
      - text: "Because it uses four parameters"
      - text: "Because it runs in quadratic time"
      - text: "Because it approximates the posterior with a Gaussian, and the logarithm of a Gaussian is a parabola — a quadratic function"
        correct: true
      - text: "Because it squares the data"
    explanation: "**p. 42:** “Under quite general conditions, the region near the peak of the posterior distribution will be nearly Gaussian” and “A Gaussian approximation is called “quadratic approximation” because the logarithm of a Gaussian distribution forms a parabola.” Algorithm: find the posterior mode with an optimizer (hill climbing), then estimate the curvature at the peak, which gives the standard deviation. Two numbers describe the whole approximate posterior. The book's `quap` function does this."
  - id: quap-mle
    prompt: "How is quadratic approximation related to maximum likelihood estimation (MLE)?"
    options:
      - text: "They are unrelated"
      - text: "MLE is always more accurate"
      - text: "Quadratic approximation is only possible when the MLE does not exist"
      - text: "With a uniform prior or a lot of data, quadratic approximation is often equivalent to the MLE and its standard error"
        correct: true
    explanation: "**p. 44:** “The quadratic approximation, either with a uniform prior or with a lot of data, is often equivalent to a maximum likelihood estimate (MLE) and its standard error.” That is a blessing (you can reinterpret many classical results) and a curse (MLE's drawbacks, such as poor behavior with small samples or skewed posteriors, are shared). With 9 tosses the true posterior is skewed and the Gaussian approximation is noticeably off; with 36 tosses it is close (Figure 2.8)."
  - id: hessian
    prompt: "When `quap` fails with an error about the *Hessian*, what is going on?"
    options:
      - text: "The Hessian is the matrix of second derivatives of the log-posterior at the mode, used to compute the approximation's spread; if it cannot be computed or inverted, the curvature estimate fails"
        correct: true
      - text: "The data file is corrupted"
      - text: "The prior is too narrow"
      - text: "MCMC is required for all models"
    explanation: "From the box “The Hessians are coming” (p. 44): the standard deviation of the quadratic approximation comes from the curvature of the log-posterior at its peak, which is the Hessian. Its inverse gives the variance–covariance matrix. If the surface is flat in some direction (non-identified parameters), or the optimizer stopped somewhere odd, the Hessian is not invertible. It is the statistical equivalent of a singular matrix exception, and it usually points to a modeling issue, not a software bug."
  - id: mcmc-why
    prompt: "Which statements explain why the book eventually needs Markov chain Monte Carlo?"
    type: multiple
    options:
      - text: "Many important models, such as multilevel models, have posteriors that are not well approximated by a Gaussian"
        correct: true
      - text: "Grid approximation cannot handle many parameters"
        correct: true
      - text: "MCMC draws samples from the posterior instead of computing it directly, which works for very complex models"
        correct: true
      - text: "MCMC always gives exact posteriors with no error"
    explanation: "Grid approximation dies of dimensionality and quadratic approximation assumes a Gaussian shape, which fails for multilevel models and many GLMs. MCMC instead produces samples whose frequencies approximate the posterior (p. 45). It is approximate — sample size and convergence matter, as Chapter 9 explains. The box on p. 45 shows a tiny Metropolis sampler for the globe model, a few lines of code, which is enough to see the idea: wander around parameter space and visit each region in proportion to its posterior probability."
---

The posterior is defined by Bayes' theorem, but computing it requires an engine. The book introduces three.

## Learning objectives

- State Bayes' theorem in words and explain the role of the denominator
- Explain what is distinctively Bayesian about Bayesian data analysis
- Show how different priors change the posterior
- Compare grid approximation, quadratic approximation and MCMC

## In a nutshell

| Engine | How it works | Strength | Weakness |
|---|---|---|---|
| Grid | Evaluate prior × likelihood at many points, normalize | Exact on the grid; teaches the logic | Exponential in number of parameters |
| Quadratic (`quap`) | Find the mode, fit a Gaussian using curvature | Fast; matches MLE with flat priors | Wrong for skewed or multimodal posteriors |
| MCMC (`ulam`, Stan) | Random walk that visits regions in proportion to posterior probability | Handles complex models | Needs diagnostics; slower |

```text
Posterior = Probability of the data × Prior / Average probability of the data
```

## Grid approximation in C#

```csharp
var grid = Enumerable.Range(0, 20).Select(i => i / 19.0).ToArray();
double Binom(int k, int n, double p) =>
    Math.Exp(LogChoose(n, k) + k * Math.Log(p) + (n - k) * Math.Log(1 - p));
double LogChoose(int n, int k) =>
    Enumerable.Range(1, k).Sum(i => Math.Log(n - k + i) - Math.Log(i));
var unstd = grid.Select(p => 1.0 * Binom(6, 9, p)).ToArray();
var posterior = unstd.Select(u => u / unstd.Sum()).ToArray();
```

## Sources

- McElreath, *Statistical Rethinking*, 2nd ed., Section 2.4, pp. 36–46; Figures 2.6–2.8.
- Gelman A et al. *Bayesian Data Analysis*, 3rd ed. CRC Press, 2013, Chapter 4 (normal approximations).
