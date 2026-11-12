---
title: Fine-tuning, LoRA and distillation
questions:
  - id: full-ft
    prompt: What does full fine-tuning update?
    options:
      - text: Only the prompt
      - text: Only the output layer
      - id: all
        text: All model weights, requiring memory for weights, gradients and optimiser state — several times the model size
        correct: true
      - text: Only the tokenizer
    explanation: For a 70B model, full fine-tuning needs a multi-GPU cluster.
  - id: lora
    prompt: What is LoRA (Low-Rank Adaptation)?
    options:
      - id: lora
        text: Freezing the base weights and training small low-rank matrices added to selected layers, cutting trainable parameters by orders of magnitude
        correct: true
      - text: A long-range radio protocol
      - text: Removing layers from the model
      - text: A prompt template
    explanation: Hu et al., 2021. Adapters are small files that can be swapped on one base model.
  - id: qlora
    prompt: What does QLoRA add to LoRA?
    options:
      - text: Quantum computing
      - text: More parameters
      - id: quantized-base
        text: Training LoRA adapters on top of a 4-bit quantised base model, making fine-tuning of large models feasible on a single GPU
        correct: true
      - text: Query rewriting
    explanation: Dettmers et al., 2023.
  - id: peft
    prompt: What is a practical advantage of serving many LoRA adapters?
    options:
      - text: Each needs its own GPU
      - id: multi-tenant
        text: One base model can serve many tasks or tenants by swapping small adapters, instead of hosting many full models
        correct: true
      - text: Adapters are larger than the base
      - text: There is no advantage
    explanation: Serving frameworks support batching requests across adapters.
  - id: distillation
    prompt: What is knowledge distillation?
    options:
      - text: Removing water from training data
      - text: Summarising documents
      - text: Encrypting models
      - id: teacher-student
        text: Training a smaller "student" model to imitate a larger "teacher" model's outputs, getting much of the quality at lower cost
        correct: true
    explanation: Many small models are distilled. Check provider terms — some forbid using outputs to train competing models.
  - id: synthetic
    prompt: Which risks come with training on synthetic (LLM-generated) data?
    type: multiple
    options:
      - id: errors
        text: Errors and biases of the generating model are copied
        correct: true
      - id: diversity
        text: Reduced diversity — outputs converge on typical patterns
        correct: true
      - id: collapse
        text: Model collapse when models are recursively trained on their own outputs over generations
        correct: true
      - text: Synthetic data is always illegal
    explanation: Shumailov et al. (Nature, 2024) demonstrated model collapse. Filtering and mixing with real data mitigate it.
  - id: overfit
    prompt: After fine-tuning, training loss is near zero but evaluation quality drops. What happened?
    options:
      - id: overfit
        text: Overfitting — the model memorised training examples instead of generalising; use a held-out set, fewer epochs, or more varied data
        correct: true
      - text: The GPU overheated
      - text: Success
      - text: The tokenizer changed
    explanation: Always evaluate on data not used in training.
  - id: data-quality
    prompt: What matters most for a successful fine-tune?
    options:
      - text: The number of GPUs
      - text: The learning rate alone
      - id: data
        text: The quality, consistency and representativeness of training examples
        correct: true
      - text: The model's name
    explanation: The LIMA paper (Zhou et al., 2023) showed strong behaviour from about 1,000 carefully curated examples.
  - id: rft
    prompt: When is reinforcement fine-tuning (RL on task-specific graders) worth considering?
    options:
      - text: For changing the model's tone
      - text: For adding new facts
      - id: graded
        text: For tasks with clear, automatically gradable outcomes where reasoning quality matters — e.g. code that passes tests or extraction checked against ground truth
        correct: true
      - text: Never
    explanation: Without a reliable grader, RL optimises the wrong thing ("reward hacking").
---
## Learning objectives

- Distinguish full fine-tuning, LoRA and QLoRA
- Understand distillation and synthetic data risks
- Avoid overfitting

## LoRA in brief

```text
W (frozen, d × d)  +  B (d × r) · A (r × d)   with r ≪ d   (e.g. r = 16)
Trainable parameters: 2·d·r instead of d²
```

```python
from peft import LoraConfig, get_peft_model
config = LoraConfig(r=16, lora_alpha=32, target_modules=["q_proj", "v_proj"], lora_dropout=0.05)
model = get_peft_model(base_model, config)
model.print_trainable_parameters()
```

## Sources

- Hu et al., "LoRA", 2021 — <https://arxiv.org/abs/2106.09685>
- Dettmers et al., "QLoRA", 2023 — <https://arxiv.org/abs/2305.14314>
- Hinton, Vinyals, Dean, "Distilling the Knowledge in a Neural Network", 2015 — <https://arxiv.org/abs/1503.02531>
- Shumailov et al., "AI models collapse when trained on recursively generated data", Nature 2024 — <https://www.nature.com/articles/s41586-024-07566-y>
- Zhou et al., "LIMA: Less Is More for Alignment", 2023 — <https://arxiv.org/abs/2305.11206>
