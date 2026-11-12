---
title: Evaluating agents
questions:
  - id: levels
    prompt: Which levels of agent evaluation are useful?
    type: multiple
    options:
      - id: outcome
        text: Final outcome — did the task succeed (answer correct, ticket resolved)?
        correct: true
      - id: trajectory
        text: Trajectory — were the right tools called with the right arguments, in a sensible order?
        correct: true
      - id: step
        text: Individual steps — retrieval quality, tool-call validity, intermediate reasoning
        correct: true
      - id: efficiency
        text: Efficiency — steps, tokens, latency and cost per task
        correct: true
    explanation: Outcome tells you whether it works; trajectory and step evaluations tell you why not.
  - id: state-check
    prompt: For an agent that books meetings, what is the most reliable success check?
    options:
      - text: Asking the agent whether it succeeded
      - id: env-state
        text: Inspecting the resulting state of the environment — is the meeting in the calendar with the right attendees and time?
        correct: true
      - text: Counting the words in its reply
      - text: Checking that it called at least one tool
    explanation: τ-bench and similar benchmarks compare final database state to a goal state.
  - id: self-report
    prompt: Why not trust an agent's own claim that it completed a task?
    options:
      - text: Agents never lie
      - text: Self-reports are always accurate
      - id: unreliable
        text: Agents can report success after failing, partially completing or hallucinating tool results; verify against ground truth
        correct: true
      - text: It is fine for low-stakes tasks only
    explanation: Several documented incidents involved agents claiming success that had not happened.
  - id: multiple-runs
    prompt: Why run each evaluation task multiple times?
    options:
      - text: To increase cost
      - text: Because the first run is always wrong
      - text: Providers require it
      - id: variance
        text: Agents are non-deterministic; multiple runs reveal variance and consistency (e.g. pass^k), not just a lucky single success
        correct: true
    explanation: Report both average success and consistency.
  - id: simulated-users
    prompt: How can you evaluate a conversational agent at scale?
    options:
      - id: simulate
        text: Use LLM-simulated users with scripted goals and personas, then check outcomes — validated against some real conversations
        correct: true
      - text: Only with live customers
      - text: It cannot be evaluated
      - text: By counting messages
    explanation: Simulators have their own biases; calibrate them against real transcripts.
  - id: safety-evals
    prompt: Which safety-focused evaluations belong in an agent test suite?
    type: multiple
    options:
      - id: injection
        text: Indirect prompt injection in documents, emails and tool results
        correct: true
      - id: scope
        text: Requests outside the agent's scope or permissions
        correct: true
      - id: destructive
        text: Attempts to trigger destructive or irreversible actions
        correct: true
      - text: Only the happy path
    explanation: Benchmarks such as AgentDojo test injection robustness of tool-using agents.
  - id: judges-agents
    prompt: What is a key challenge with LLM judges for agent trajectories?
    options:
      - text: Trajectories are too short
      - id: valid-paths
        text: Many different trajectories can be valid; judges must assess reasonableness rather than match one expected path, which makes rubric design harder
        correct: true
      - text: Judges cannot read JSON
      - text: There is no challenge
    explanation: Combine judges with deterministic checks of required and forbidden actions.
  - id: from-production
    prompt: Where do the best new evaluation cases come from?
    options:
      - text: Public benchmarks
      - text: Random generation
      - id: production
        text: Real production traces — especially failures, escalations and user complaints — labelled by experts
        correct: true
      - text: The vendor's demo
    explanation: Every incident should become a regression test.
---
## Learning objectives

- Evaluate outcomes, trajectories, steps and efficiency
- Verify against environment state, not self-reports
- Measure consistency and safety

## An evaluation record

```json
{
  "task": "Reschedule my 3pm with Priya to tomorrow at the same time",
  "expected_state": { "event": "Priya sync", "start": "2026-11-13T15:00" },
  "required_tools": ["find_event", "update_event"],
  "forbidden_tools": ["delete_event"],
  "max_steps": 6
}
```

## Sources

- Yao et al., "τ-bench", 2024 — <https://arxiv.org/abs/2406.12045>
- Debenedetti et al., "AgentDojo", 2024 — <https://arxiv.org/abs/2406.13352>
- MLflow, "Evaluate agents" — <https://mlflow.org/docs/latest/genai/eval-monitor/>
