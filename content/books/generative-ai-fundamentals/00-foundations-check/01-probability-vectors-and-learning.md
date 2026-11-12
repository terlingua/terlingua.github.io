---
title: Probability, vectors and learning
questions:
  - id: distribution
    prompt: What is a probability distribution over a vocabulary?
    options:
      - text: A list of words sorted alphabetically
      - id: probs
        text: An assignment of a probability to each possible token, with all probabilities summing to 1
        correct: true
      - text: The frequency of each word in a dictionary
      - text: A random number
    explanation: An LLM outputs one of these at every step.
  - id: softmax
    prompt: What does the softmax function do?
    options:
      - text: Sorts numbers
      - text: Removes negative values
      - id: normalise
        text: Converts a vector of raw scores (logits) into probabilities that are positive and sum to 1
        correct: true
      - text: Compresses images
    explanation: Temperature divides logits before softmax, sharpening or flattening the distribution.
  - id: sampling
    prompt: What does sampling from a distribution mean?
    options:
      - id: draw
        text: Randomly choosing an outcome with likelihood proportional to its probability
        correct: true
      - text: Always choosing the most likely outcome
      - text: Choosing uniformly at random
      - text: Averaging all outcomes
    explanation: This is why the same prompt can produce different outputs.
  - id: dot-product
    prompt: What does a large positive dot product between two normalised vectors indicate?
    options:
      - text: They are unrelated
      - text: They are opposite
      - id: similar
        text: They point in similar directions — high similarity
        correct: true
      - text: One is longer
    explanation: For unit vectors, the dot product equals cosine similarity.
  - id: dims
    prompt: An embedding has 1,024 dimensions. What does that mean?
    options:
      - text: It is 1,024 bytes long
      - text: It describes 1,024 words
      - text: It took 1,024 seconds to compute
      - id: numbers
        text: It is a vector of 1,024 numbers; individual dimensions usually have no human-readable meaning
        correct: true
    explanation: Meaning lives in relationships between vectors, not in single coordinates.
  - id: weights
    prompt: In a neural network, what are weights?
    options:
      - id: params
        text: Learned numbers that determine how inputs are transformed into outputs
        correct: true
      - text: The size of the training data
      - text: The importance of each user
      - text: Physical mass of GPUs
    explanation: Training adjusts weights to reduce a loss.
  - id: loss
    prompt: What is a loss function?
    options:
      - text: A record of lost data
      - id: error
        text: A measure of how wrong the model's predictions are on training examples, which training tries to minimise
        correct: true
      - text: A business metric
      - text: A function that deletes data
    explanation: LLM pretraining minimises cross-entropy loss on next-token prediction.
  - id: gradient-descent
    prompt: What does gradient descent do?
    options:
      - text: Randomly changes weights
      - text: Downloads better weights
      - id: step
        text: Repeatedly adjusts weights a small step in the direction that most reduces the loss, using gradients computed by backpropagation
        correct: true
      - text: Sorts the training data
    explanation: The learning rate controls step size.
  - id: train-test
    prompt: Why hold out a test set?
    options:
      - text: To save storage
      - text: To train faster
      - text: Regulators require it
      - id: generalise
        text: To estimate performance on unseen data; evaluating on training data overstates quality
        correct: true
    explanation: The same principle applies to prompt engineering — don't tune and evaluate on the same examples.
  - id: overfitting
    prompt: What is overfitting?
    options:
      - id: memorise
        text: A model fitting training data so closely, including noise, that it performs worse on new data
        correct: true
      - text: A model too small to learn
      - text: Using too much GPU memory
      - text: A model that runs too fast
    explanation: Regularisation, more data and early stopping counter it.
  - id: precision-recall
    prompt: For a classifier, what do precision and recall measure?
    options:
      - text: Speed and memory
      - text: Accuracy and cost
      - id: pr
        text: Precision — of items flagged positive, the share that are truly positive; recall — of truly positive items, the share that were flagged
        correct: true
      - text: The same thing
    explanation: Retrieval in RAG is evaluated with the same ideas.
  - id: base-rate
    prompt: A fraud classifier is 99% accurate on data where 0.5% of transactions are fraud. Is it good?
    options:
      - text: Yes, 99% is excellent
      - id: unknown
        text: Not necessarily — predicting "not fraud" for everything gives 99.5% accuracy; look at precision, recall and costs of errors
        correct: true
      - text: No, 99% is always bad
      - text: Accuracy cannot be computed
    explanation: Base rates matter; accuracy alone misleads on imbalanced data.
---
## Learning objectives

- Recall the probability, vector and ML basics behind LLMs
- Recognise evaluation pitfalls inherited from classic ML

## Softmax with temperature

```python
import numpy as np

def softmax(logits: np.ndarray, temperature: float = 1.0) -> np.ndarray:
    z = logits / temperature
    z = z - z.max()
    e = np.exp(z)
    return e / e.sum()

logits = np.array([2.0, 1.0, 0.1])
print(softmax(logits, 1.0))
print(softmax(logits, 0.2))
```

At low temperature almost all probability goes to the top token.

## Sources

- Goodfellow, Bengio, Courville, *Deep Learning* (MIT Press, 2016) — <https://www.deeplearningbook.org/>
- 3Blue1Brown, "Neural networks" series — <https://www.3blue1brown.com/topics/neural-networks>
