---
title: "Foundations Check — Before Chapter 6"
questions:
  - id: majority
    prompt: "In a cluster of 5 nodes, what is the smallest majority?"
    options:
      - id: o8b3371
        text: "4"
      - id: o2c2dcc
        text: "5"
      - id: odc143c
        text: "2"
      - id: od2a453
        text: "3"
        correct: true
    explanation: "A majority is more than half: floor(5/2) + 1 = 3. Any two majorities of the same set overlap in at least one member."
  - id: overlap
    prompt: "Out of 7 nodes, a write reaches 4 and a read queries 4. Must they share at least one node?"
    options:
      - id: o41a385
        text: "Only if the read happens after the write"
      - id: o74f3db
        text: "No, they can be disjoint"
      - id: o199b0f
        text: "Only if the nodes are in the same region"
      - id: o5e52d7
        text: "Yes, because 4 + 4 > 7"
        correct: true
    explanation: "Two subsets of sizes a and b drawn from n elements must intersect whenever a + b > n (pigeonhole principle)."
  - id: timeout-meaning
    prompt: "A request times out after 5 seconds. What can you conclude?"
    options:
      - id: o530650
        text: "Nothing certain — the request or response may be lost or delayed, or the remote may have crashed"
        correct: true
      - id: oefc0f1
        text: "The remote node has definitely crashed"
      - id: o3cc2c5
        text: "The network is definitely partitioned"
      - id: o16a63c
        text: "The request was definitely not processed"
    explanation: "A timeout only tells you that no response arrived in time. Chapter 6 uses timeouts to detect leader failure precisely because there is \"no foolproof way\" to know more."
  - id: union
    prompt: "Cart A = {soap, book}, cart B = {soap, dvd}. What is A ∪ B?"
    options:
      - id: o0fb93d
        text: "{soap, book, dvd}"
        correct: true
      - id: o7070e1
        text: "{}"
      - id: o96bcd3
        text: "{book, dvd}"
      - id: o281407
        text: "{soap}"
    explanation: "Union keeps every element in either set — which is why naive union merging can resurrect deleted items."
  - id: monotonic-counter
    prompt: "Why can't you safely merge two replicas' counters by taking the maximum value?"
    options:
      - id: o9c133e
        text: "Counters cannot be replicated"
      - id: o48e241
        text: "Maximum is not defined for integers"
      - id: obcd9dc
        text: "Increments made independently on each replica would be lost"
        correct: true
      - id: of989c3
        text: "It would double-count increments"
    explanation: "If each replica increments from 10 to 11 independently, max gives 11 but the true count is 12. Counter CRDTs track per-replica increments instead."
  - id: odd-cluster
    prompt: "Why are replica counts commonly odd (3, 5)?"
    options:
      - id: oe97ec8
        text: "Even numbers cannot form majorities"
      - id: o82e638
        text: "Odd numbers are faster"
      - id: oa4723b
        text: "Hardware comes in odd numbers"
      - id: o225b4b
        text: "Adding a node to make it even does not increase the number of failures a majority can tolerate"
        correct: true
    explanation: "With 3 nodes a majority (2) tolerates 1 failure; with 4 nodes a majority (3) still tolerates only 1."
---
Chapter 6 assumes a feel for networks, majorities and simple set reasoning. These questions are not
drawn from the book's text.

## Learning objectives

- Reason about majorities and overlapping sets
- Recall what a timeout can and cannot tell you
- Recall the semantics of set union
