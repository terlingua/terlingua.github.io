---
title: "Batch Processing with Unix Tools"
questions:
  - id: pipeline-steps
    prompt: "In the pipeline, why is the list of URLs sorted before `uniq -c`?"
    options:
      - id: obcf3cc
        text: "To speed up awk"
      - id: o530dc0
        text: "To rank by popularity"
      - id: oe1d228
        text: "So that n requests for a URL appear as n adjacent lines that uniq can count"
        correct: true
      - id: od69504
        text: "To remove duplicates"
    explanation: "“The reason for sorting is to ensure that, if a URL has been requested n times, the sorted file will contain the same URL repeated n times in a row.”"
  - id: second-sort
    prompt: "What does `sort -r -n` do in the pipeline?"
    options:
      - id: o02e481
        text: "Removes duplicates"
      - id: o45cfd9
        text: "Sorts URLs alphabetically in reverse"
      - id: o825544
        text: "Sorts numerically by the count, largest first"
        correct: true
      - id: oed83e1
        text: "Picks the top five"
    explanation: "“The second sort sorts by the number (-n) at the start of each line ... It then returns the results in reverse (-r) order—with the largest number first.”"
  - id: working-set
    prompt: "In the Python hash-table version, what determines memory use?"
    options:
      - id: o579a41
        text: "The total log size"
      - id: o369f99
        text: "The number of lines per URL"
      - id: o86cad5
        text: "The number of distinct URLs, not the number of log lines"
        correct: true
      - id: o2c00cc
        text: "The line length"
    explanation: "“the working set of the job ... depends only on the number of distinct URLs. If there are a million log entries for a single URL, the space required in the hash table is still just one URL plus the size of the counter.”"
  - id: sort-advantage
    prompt: "When does the sorting approach beat an in-memory hash table?"
    options:
      - id: o5faaa3
        text: "Only for numeric data"
      - id: o8aa85a
        text: "Always"
      - id: o6099c8
        text: "Never"
      - id: o87af4e
        text: "When the working set is larger than available memory, since sorting uses disk efficiently"
        correct: true
    explanation: "“if the job’s working set is larger than the available memory, the sorting approach has the advantage that it can make efficient use of disks.”"
  - id: gnu-sort
    prompt: "What makes GNU sort scale to large datasets?"
    options:
      - id: o27a3ff
        text: "It uses a GPU"
      - id: o047408
        text: "It samples the data"
      - id: ofd20f6
        text: "It loads data into a database"
      - id: o97d442
        text: "It spills to disk and parallelizes across CPU cores automatically"
        correct: true
    explanation: "“The sort utility in GNU Coreutils (Linux) automatically handles larger-than-memory datasets by spilling to disk and automatically parallelizes sorting across multiple CPU cores”."
  - id: unix-limit
    prompt: "What is the key limitation of Unix tools for batch processing?"
    options:
      - id: od1630e
        text: "They can't sort"
      - id: oa84629
        text: "They run on a single machine"
        correct: true
      - id: ofd5719
        text: "They are too slow on small data"
      - id: o8a9fd7
        text: "They can't read files"
    explanation: "“A limitation of Unix tools is that they run on a single machine ... that’s where distributed batch processing frameworks come in.”"
---
A five-command pipeline finds the most popular pages in a log, and teaches the sort-versus-hash
trade-off that distributed systems reuse.

## Learning objectives

- Explain each stage of the log-analysis pipeline
- Compare in-memory hash aggregation with sorting
- Explain how GNU sort scales beyond memory

## In a nutshell

- awk extracts, sort groups, uniq -c counts, sort -rn ranks, head trims.
- Hash table: working set = distinct keys; fine if it fits in memory.
- Sorting spills to disk and parallelizes; the limit becomes disk read speed and one machine.

## Connections to today

- **DuckDB** applies the same choices (hash aggregation with spilling) on a single machine and often
  beats clusters for tens of gigabytes.
