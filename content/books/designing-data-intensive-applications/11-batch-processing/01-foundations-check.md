---
title: "Foundations Check — Before Chapter 11"
questions:
  - id: pipe
    prompt: "In `cat log | awk '{print $7}' | sort | uniq -c`, what does `uniq -c` require of its input to count correctly?"
    options:
      - id: ob65fce
        text: "Nothing; it counts across the whole file"
      - id: o2cc4e7
        text: "Lines must be unique already"
      - id: o9c9cb5
        text: "Identical lines must be adjacent, which `sort` ensures"
        correct: true
      - id: oad0fbe
        text: "Lines must be numeric"
    explanation: "`uniq` only compares neighbouring lines, so sorting first is essential."
  - id: external-sort
    prompt: "How does external merge sort handle data larger than memory?"
    options:
      - id: ofef03f
        text: "It cannot"
      - id: o572c19
        text: "Load everything into swap and sort"
      - id: o8f4aef
        text: "Sort chunks that fit in memory, write them as sorted runs, then merge the runs"
        correct: true
      - id: o823216
        text: "Use quicksort on disk in place"
    explanation: "This is the same idea as LSM segments and Hadoop's sort-based shuffle."
  - id: map-reduce-fp
    prompt: "In functional programming, what do `map` and `reduce` (fold) do?"
    options:
      - id: oc199ad
        text: "`map` joins; `reduce` splits"
      - id: o877d30
        text: "`map` sorts; `reduce` filters"
      - id: o01401f
        text: "Both mutate a shared list"
      - id: o234422
        text: "`map` transforms each element; `reduce` combines elements into one result"
        correct: true
    explanation: "They are pure higher-order functions, which is what makes parallel execution safe."
  - id: embarrassingly
    prompt: "What is an \"embarrassingly parallel\" workload?"
    options:
      - id: ob66e5b
        text: "One that runs only on one core"
      - id: oa15e98
        text: "One that cannot be parallelized"
      - id: oe00e4d
        text: "One that splits into independent tasks needing no coordination"
        correct: true
      - id: odeb75c
        text: "One that requires locks"
    explanation: "Filtering or projecting rows independently is a classic example."
  - id: dag
    prompt: "What is a DAG?"
    options:
      - id: od3a1fa
        text: "A data encryption standard"
      - id: of9ea6a
        text: "A type of B-tree"
      - id: o720ec2
        text: "A distributed database"
      - id: oe1d6f4
        text: "A directed graph with no cycles, often used to express job dependencies"
        correct: true
    explanation: "Workflow schedulers model pipelines as DAGs of tasks or jobs."
  - id: np-hard
    prompt: "What does it mean that optimal cluster scheduling is NP-hard?"
    options:
      - id: o205781
        text: "It requires quantum computers"
      - id: od94fe1
        text: "It is impossible to schedule anything"
      - id: obe74af
        text: "No known algorithm finds optimal solutions quickly for large inputs, so heuristics are used"
        correct: true
      - id: o959476
        text: "It can be solved in linear time"
    explanation: "NP-hard problems are believed to need super-polynomial time in general; practical schedulers use heuristics."
---
Chapter 11 assumes basic Unix shell skills, sorting algorithms and functional map/reduce. These
questions are not drawn from the book's text.

## Learning objectives

- Read a Unix pipeline
- Recall external merge sort and hash aggregation
- Recall map and reduce from functional programming
