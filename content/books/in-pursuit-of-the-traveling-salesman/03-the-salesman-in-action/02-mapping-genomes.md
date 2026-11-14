---
title: "Mapping Genomes (pp. 49–51)"
questions:
  - id: rh-distance
    prompt: >-
      In radiation hybrid (RH) mapping, how is an “experimental distance” between two DNA markers
      inferred?
    explanation: >-
      X-rays break the genome into fragments that are combined into hybrid cell lines. “If two
      markers A and B are close on the genome, then they are unlikely to be split apart in the
      radiation step. Thus … if A is present in a cell line, it is likely that B is present as
      well. On the other hand, if A and B are far apart … we can expect to have cell lines that
      contain just A or just B” (p. 50). Distance comes from co-occurrence statistics, not from
      a ruler. Many log-correlation and service-dependency inference methods use the same idea:
      things that keep appearing together are probably close.
    options:
      - text: "By measuring the molecules with a microscope"
      - text: "From how often the two markers appear together in the same hybrid cell lines"
        correct: true
      - text: "From alphabetical order of marker names"
      - text: "By sequencing the entire genome first"
  - id: genome-path
    prompt: >-
      Why is ordering markers on a chromosome a TSP, and what adjustment is needed?
    explanation: >-
      “A genome ordering can be viewed as a path traveling through each marker in the collection.
      As usual, such a Hamiltonian path is readily converted to a tour by adding an extra city to
      permit the ends of the path to be joined” (p. 50). Markers are the cities and experimental
      distances are the costs. The chromosome is linear, so the answer is a path, obtained with the
      dummy-city reduction from chapter 2.
    options:
      - text: "It is a shortest-path problem between two markers"
      - text: "It is a minimum spanning tree"
      - text: "Chromosomes are circular, so no adjustment is needed"
      - text: "The order is a Hamiltonian path through the markers; add a dummy city to make it a tour"
        correct: true
  - id: noisy-data
    prompt: >-
      The NIH software by Agarwala and Schäffer wraps Concorde for genome maps. Besides calling the
      solver, what did it have to handle?
    explanation: >-
      The team developed “procedures for dealing with erroneous data (a common occurrence in
      laboratories)” (p. 50). An exact solver gives the optimal order *for the given costs*. If the
      costs are noisy, the optimum can still be biologically wrong. Robust preprocessing and
      validation are part of the system. The package was used for human, macaque, horse, dog, cat,
      mouse, rat, cow, sheep and river buffalo maps (p. 51).
    options:
      - text: "Nothing else; Concorde handles everything"
      - text: "Erroneous laboratory data — optimal tours on noisy costs need robust preprocessing"
        correct: true
      - text: "Converting DNA to binary"
      - text: "Parallelizing across GPUs"
  - id: markers-unique
    prompt: >-
      Why must genome markers be segments that appear *exactly once* in the genome?
    explanation: >-
      “The markers in these maps are segments of DNA that appear exactly once in the genome and can
      be reliably detected … The ability to recognize these unique segments allows researchers to
      use them to verify, compare, and combine physical maps created across different
      laboratories” (p. 49). A marker works like a primary key. If a marker occurred twice,
      co-occurrence evidence would be ambiguous and maps from different labs could not be joined.
    options:
      - text: "To save sequencing cost"
      - text: "So they act as unambiguous landmarks that can be detected and matched across labs, like unique keys"
        correct: true
      - text: "Because repeats are illegal"
      - text: "So the TSP becomes symmetric"
  - id: breaks-metric
    prompt: >-
      Chapter 3 later notes that texture layout on a DVD uses “the same measure used in the
      genome-mapping application.” Which measure?
    explanation: >-
      Counting *breaks*. A set (a cell line, or a scene’s texture set) split into k intervals in
      the ordering contributes k − 1 breaks. The cost between two items is the number of sets
      containing one but not the other (p. 58). Minimizing total cost over a path keeps items
      that appear together next to each other. The same objective applies to columnar storage
      layout, cache locality and cluster-friendly ordering of files in a container image.
    options:
      - text: "Euclidean distance between items"
      - text: "Number of sets containing one item but not the other, which counts breaks in the ordering"
        correct: true
      - text: "Alphabetical distance"
      - text: "File size difference"
---
## Learning objectives

- Turn a co-occurrence signal into a distance and an ordering problem.
- Apply the path-to-tour reduction to genome maps.

## How it works

- **Goal:** order unique DNA markers along each chromosome (p. 49).
- **Data:** radiation hybrid mapping. Fragments end up in rodent-hybrid cell lines, and nearby markers tend to land in the same lines (p. 50).
- **Distance:** derived from co-occurrence across cell lines.
- **Model:** a shortest Hamiltonian path through the markers, made into a tour with a dummy city (p. 50).
- **System:** the NIH software of Agarwala, Schäffer et al. adds error handling and calls Concorde for optimal tours. It has been used for many mammalian maps (pp. 50–51).

## In software terms

The same pattern turns up in data engineering. When items that are used together should be stored together, define cost(i, j) as the number of contexts that use exactly one of i and j, then solve a TSP path. Uses include file layout, column ordering for compression, and caching.

## Sources

- Cook (2012), pp. 49–51, 58.
- Agarwala, R., et al. “A fast and scalable radiation hybrid map construction and integration strategy.” *Genome Research* 10 (2000), 350–364.
- Ben-Dor, A., B. Chor. “On constructing radiation hybrid maps.” *J. Computational Biology* 4 (1997), 517–533.
