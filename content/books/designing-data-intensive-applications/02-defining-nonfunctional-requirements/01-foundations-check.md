---
title: "Foundations Check — Before Chapter 2"
questions:
  - id: median-vs-mean
    prompt: "Ten requests take 10, 10, 11, 11, 12, 12, 13, 13, 14 and 900 ms. Which statement is true?"
    options:
      - id: o1588bb
        text: "The mean and median are both about 12 ms"
      - id: oab1d1b
        text: "The median is 12 ms while the mean is about 100.6 ms, so the mean is dominated by one outlier"
        correct: true
      - id: ob46737
        text: "The mean is lower than the median"
      - id: ocf79df
        text: "The median is 900 ms"
    explanation: "Sorted, the middle two values are 12 and 12, so the median is 12 ms. The sum is 1,006 ms, so the mean is 100.6 ms. One outlier drags the mean far from the typical experience."
  - id: percentile-def
    prompt: "If p99 latency is 800 ms, what does that mean?"
    options:
      - id: oe33a99
        text: "The fastest 1% took 800 ms"
      - id: oc14c09
        text: "99 requests took exactly 800 ms"
      - id: ob2d922
        text: "99% of requests completed faster than 800 ms; 1% took 800 ms or longer"
        correct: true
      - id: o55c1db
        text: "The average request took 800 ms"
    explanation: "A percentile is a point in the sorted distribution: p99 is the value below which 99% of observations fall."
  - id: at-least-one
    prompt: "Each of 100 independent calls has a 1% chance of being slow. What is the probability that at least one is slow?"
    options:
      - id: o760a43
        text: "1%"
      - id: o3eb185
        text: "About 10%"
      - id: of2a8e5
        text: "100%"
      - id: o06c1b8
        text: "About 63%, computed as 1 − 0.99^100"
        correct: true
    explanation: "P(no slow call) = 0.99^100 ≈ 0.366, so P(at least one) ≈ 0.634. This is the arithmetic behind tail latency amplification."
  - id: utilization
    prompt: "A single server handles requests at random arrival times. As utilization rises from 50% to 95%, what typically happens to average waiting time in the queue?"
    options:
      - id: o056f3c
        text: "It grows non-linearly and explodes as utilization approaches 100%"
        correct: true
      - id: ob63977
        text: "It decreases because the server is warmed up"
      - id: ofc47e4
        text: "It grows linearly with utilization"
      - id: o52f7b7
        text: "It stays constant until 100%"
    explanation: "Basic queueing theory (for example the M/M/1 model, where waiting time is proportional to ρ/(1−ρ)) shows waiting time rising sharply near full utilization."
  - id: throughput-units
    prompt: "Which of these is a throughput metric?"
    options:
      - id: oc5eb03
        text: "Requests per second"
        correct: true
      - id: oce4b14
        text: "Time to first byte"
      - id: o9137cd
        text: "p99 latency"
      - id: oec4290
        text: "Milliseconds per request"
    explanation: "Throughput is a rate (things per unit time). The others are response-time measures."
  - id: histogram-merge
    prompt: "Two servers each report their p99. Why can't you average the two p99 values to get the combined p99?"
    options:
      - id: o512a1b
        text: "The servers may use different time zones"
      - id: obc93f9
        text: "Averages are only defined for integers"
      - id: o5a99ae
        text: "You can; averaging percentiles is exact"
      - id: o755d89
        text: "Percentiles are not linear; you need the underlying distributions (e.g., merged histograms)"
        correct: true
    explanation: "A percentile depends on the whole distribution. With unequal load or different shapes, the average of two p99s can be far from the combined p99."
  - id: independence
    prompt: "Two disks each fail with probability 3% per year, independently. What is the probability both fail in the same year?"
    options:
      - id: ofa83ee
        text: "0.3%"
      - id: o47203a
        text: "0.09%"
        correct: true
      - id: od0f83c
        text: "6%"
      - id: od62969
        text: "3%"
    explanation: "For independent events, P(A and B) = 0.03 × 0.03 = 0.0009. Correlated failures make the real figure higher, which is why the book warns about correlation."
  - id: big-o
    prompt: "Merging k sorted lists of total length n using a min-heap costs roughly what?"
    options:
      - id: od2d61a
        text: "O(k)"
      - id: o56ca0c
        text: "O(n²)"
      - id: o53c1b2
        text: "O(n log k)"
        correct: true
      - id: o864381
        text: "O(log n)"
    explanation: "Each of the n output elements needs a heap operation over k heads. The timeline query in the case study merges recent posts from every followed account."
---
Chapter 2 leans on basic statistics, probability and queueing intuition. These questions are not drawn
from the book's text.

## Learning objectives

- Compute means, medians and percentiles by hand
- Reason about the probability that at least one of many independent events occurs
- Recall why utilization near 100% causes waiting times to explode
