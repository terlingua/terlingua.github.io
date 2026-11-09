---
title: "Chapter Opening — From Bounded to Unbounded Data"
questions:
  - id: sort-bounded
    prompt: "Why must a MapReduce sort read its entire input before producing output?"
    options:
      - id: o7f3e0f
        text: "The very last record might have the lowest key and need to be output first"
        correct: true
      - id: o60ea99
        text: "Reducers start only after midnight"
      - id: oa38dbf
        text: "Disks can't be read in parallel"
      - id: o6ce979
        text: "Sorting requires a network shuffle"
    explanation: "“the sorting operation that is central to MapReduce must read its entire input before it can start producing output because the very last input record could be the one with the lowest key”."
  - id: daily-problem
    prompt: "What is the problem with daily batch processes for many users?"
    options:
      - id: o684440
        text: "Changes in the input appear in the output a day later"
        correct: true
      - id: o0873f2
        text: "They lose data"
      - id: o01d698
        text: "They require streaming hardware"
      - id: o732728
        text: "They can't sort"
    explanation: "“The problem with daily batch processes is that changes in the input are reflected in the output a day later, which is too slow for many impatient users.”"
  - id: stream-def
    prompt: "What does \"stream\" mean in general?"
    options:
      - id: o84f2a7
        text: "A Kafka topic only"
      - id: od63fe8
        text: "Data that is incrementally made available over time"
        correct: true
      - id: oa08855
        text: "A TCP connection only"
      - id: o220f7f
        text: "Video only"
    explanation: "“In general, a stream refers to data that is incrementally made available over time.” It appears in stdin/stdout, lazy lists, TCP, and more."
---
Batch processing assumes bounded input. Real data is unbounded, so we process events continuously as
they arrive.

## Learning objectives

- Explain why sorting needs bounded input
- Define a stream and where the concept appears

## In a nutshell

- Daily batches mean day-old results; streams process events as they happen.
- A stream is data made available incrementally over time.

## Connections to today

- **Real-time features** — fraud scoring, personalization, LLM agent event triggers — are pushing many
  pipelines from nightly batch to streaming.
