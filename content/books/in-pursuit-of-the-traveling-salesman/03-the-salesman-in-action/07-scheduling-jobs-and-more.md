---
title: "Scheduling Jobs, and More (pp. 60–61)"
questions:
  - id: setup-times
    prompt: >-
      BÖWE CARDTEC sequences smart-card production jobs. What are the cities and costs?
    explanation: >-
      “The cities are the jobs and the travel cost between jobs i and j is the time it takes to
      reconfigure the machine for job j after it has completed job i” (p. 60). Ribbon color changes
      and blank-card swaps make the setup significant. Using Concorde tours “reduced the total setup
      time by up to 65% in typical applications” (p. 60).
    options:
      - text: "Customers, with shipping cost"
      - text: "Machines, with electricity cost"
      - text: "Jobs, with machine reconfiguration (setup) time from one job to the next"
        correct: true
      - text: "Cards, with printing time"
  - id: sequence-asymmetric
    prompt: >-
      Why are sequence-dependent setup problems usually *asymmetric* TSPs?
    explanation: >-
      “In typical examples, the setup time to move from job i to job j is different than the time to
      move from job j back to job i. The TSP thus takes the asymmetrical form, where the cost of a
      tour depends on the direction of travel” (p. 60). Switching from dark to light ribbon may need
      cleaning while light to dark does not. Merrill Flood described this application in 1954. In
      CI/CD terms, going from a cold cache to a warm build is not the reverse of going from warm to
      cold.
    options:
      - text: "Because the first job is fixed"
      - text: "Because jobs have different priorities"
      - text: "Because reconfiguring from i to j often differs from j to i"
        correct: true
      - text: "Because there are more jobs than machines"
  - id: ci-runner
    prompt: >-
      A CI runner executes 30 test suites per night. Switching suites requires swapping container
      images and warming caches, with costs that depend on the pair. Which statements are correct?
    type: multiple
    explanation: >-
      This is the BÖWE CARDTEC model: suites are cities and switch costs are travel costs (p. 60).
      Since cost(i → j) ≠ cost(j → i) in general, it is an asymmetric TSP. With a fixed first suite
      and no return to the start, it is a path problem solved with a dummy city. Reductions of up to
      65% in setup time were seen in card production. Ordering suites alphabetically ignores the
      switch costs entirely.
    options:
      - text: "It is a TSP with suites as cities and switch costs as travel costs"
        correct: true
      - text: "Costs are likely asymmetric"
        correct: true
      - text: "If the run need not end where it started, add a dummy city to get a path"
        correct: true
      - text: "Alphabetical order is optimal"
  - id: and-more
    prompt: >-
      Which of these appear in the book's “And More” list of reported TSP applications?
    type: multiple
    explanation: >-
      The list includes “planning hiking paths in a nature park; minimizing wallpaper waste; picking
      items in a rectangular warehouse; cutting patterns in the glass industry; constructing
      universal DNA linkers; estimating the trenching costs for connecting a telescope array;
      studying problems in evolutionary change; assembling a genome map …; gathering geophysical
      seismic data; compressing large data sets of zero-one-valued arrays” (pp. 60–61). Stock
      portfolio selection is not on it, because it is not a sequencing problem.
    options:
      - text: "Minimizing wallpaper waste"
        correct: true
      - text: "Warehouse order picking"
        correct: true
      - text: "Compressing zero-one arrays"
        correct: true
      - text: "Stock portfolio selection"
---
## Learning objectives

- Model sequence-dependent setup scheduling as an asymmetric TSP.
- Recognize the breadth of TSP applications.

## Summary

Jobs are cities, and cost(i, j) is the setup time to switch from job i to job j. Setup costs are usually **asymmetric**. BÖWE CARDTEC cut setup time by up to 65% with Concorde. Flood described the idea in 1954 (p. 60).

Other reported uses: hiking paths, wallpaper waste, warehouse picking, glass cutting, DNA linkers, telescope-array trenching, evolution studies, genome assembly, seismic surveys, and compressing 0/1 arrays (pp. 60–61).

## In software terms

Batch schedulers, CI runners, GPU kernels sharing state, ETL jobs sharing warm caches, and database maintenance windows all have sequence-dependent switch costs. When a switch costs minutes, ordering the work is a TSP.

## Sources

- Cook (2012), pp. 60–61.
- Applegate et al. (2006), *The Traveling Salesman Problem: A Computational Study*, section 2.7 (application references).
