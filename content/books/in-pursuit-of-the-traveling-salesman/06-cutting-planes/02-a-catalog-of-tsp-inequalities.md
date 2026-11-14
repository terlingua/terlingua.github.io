---
title: "A Catalog of TSP Inequalities (pp. 131–137)"
questions:
  - id: fulkerson-letter
    prompt: >-
      In 1954 Fulkerson wrote that “C₅ can be characterized by a system of 25 hyperplanes in 10
      dimensional space.” What is C₅ and why were the RAND researchers asking around?
    explanation: >-
      Cₙ is “the convex hull of tours for an n-city problem” (p. 131). Five cities have 10 edges, so the
      polytope lives in 10 dimensions, and 25 inequalities describe it. RAND sent letters to Heller, Kuhn
      and Tucker because “the RAND researchers were actively seeking information on the structure of the
      TSP polytope, to better equip their cutting-plane method” (p. 132). Better knowledge of the polytope
      gives a better supply of cuts.
    options:
      - text: "The 5-city TSP polytope; knowing its facets would supply cutting planes"
        correct: true
      - text: "A dataset of 5 cities"
      - text: "A chess opening"
      - text: "A computer; they needed time on it"
  - id: comb-structure
    prompt: >-
      What is a comb, and what does the comb inequality say?
    explanation: >-
      A comb is a *handle* (set H) intersecting k pairwise disjoint *teeth* (sets T₁…T_k), each tooth
      meeting the handle and sticking out of it, with k **odd and at least 3**. “Every tour must cross the
      borders of the comb at least 3k + 1 times” (p. 133). Each tooth is crossed ≥ 2 times, giving 2k. A
      tour that traverses each tooth as a single path must cross the handle at least k times, and the
      number of crossings of any set is even, so with k odd the handle is crossed ≥ k + 1 times. Total
      ≥ 2k + k + 1 = 3k + 1. Chvátal introduced combs in 1973 and Grötschel and Padberg extended them
      (p. 132).
    options:
      - text: "A handle and any number of teeth; ≥ 2k crossings"
      - text: "A handle plus an odd number k ≥ 3 of disjoint teeth; every tour crosses the borders ≥ 3k + 1 times"
        correct: true
      - text: "A set of three subtours; ≥ 6 crossings"
      - text: "A path through all cities; exactly n crossings"
  - id: even-teeth
    prompt: >-
      Why must a comb have an *odd* number of teeth?
    explanation: >-
      With six teeth, “the tour crosses the borders only 18 times, one less than the 3k + 1 rule would
      require. In this even case we were able to pair up the teeth, avoiding the extra exit from the handle”
      (p. 133). The +1 comes from parity: a tour crosses any set’s border an even number of times. With k
      odd, k handle crossings must round up to k + 1. With k even, k crossings are already even, so there is
      no extra crossing and the inequality is not valid.
    options:
      - text: "Tradition"
      - text: "Parity: the handle must be crossed an even number of times, so k odd forces k + 1; with even k a tour can achieve 3k"
        correct: true
      - text: "Because even teeth overlap"
      - text: "It need not be odd"
  - id: grotschel-120
    prompt: >-
      Grötschel's 1977 record of 120 German cities used which kinds of cutting planes, and how were they
      found?
    explanation: >-
      His hand drawings show “1/2-valued edges … by wiggly lines, violated subtour inequalities … by red
      enclosures, and violated combs … by blue enclosures for the handle and teeth. In each round of his
      computation Grötschel added multiple cutting planes, which is highly recommended” (pp. 133–134).
      Chapter 8 gives the totals: 13 LP rounds, 36 subtours and 60 combs, found by eye, with IBM’s MPSX
      solving the LPs (pp. 156–157).
    options:
      - text: "Only degree constraints"
      - text: "Random inequalities"
      - text: "Gomory cuts, found automatically"
      - text: "Subtours and combs, spotted by hand on drawings, several added per round"
        correct: true
  - id: facet-definition
    prompt: >-
      What is a facet-defining inequality, and why focus on them?
    explanation: >-
      For a polytope, the facets are the maximal-dimensional faces, like the six edges of the hexagon in
      figure 6.11. “The facet-defining TSP inequalities, together with the degree constraints, provide a
      complete description of the convex hull of tours. And each of these inequalities must be included in
      any such description” (p. 136). Facets are the strongest possible cuts. Grötschel and Padberg argued
      they are “of substantial computational value” (p. 136). Subtours, combs (Grötschel–Padberg) and clique
      trees (Grötschel–Pulleyblank) are all facet-defining.
    options:
      - text: "The objective function"
      - text: "Any inequality touching one tour"
      - text: "An inequality whose face has maximal dimension; facets are necessary in every complete description and are the strongest cuts"
        correct: true
      - text: "An inequality with integer coefficients"
  - id: facet-non-facet
    prompt: >-
      In figure 6.11, two halfspaces both touch the convex hull, but only one is facet-defining. What is
      the difference?
    explanation: >-
      Both are valid and both touch at least one point, but the facet-defining halfspace touches along an
      entire side (two corners in 2-D, which determine a line), while the other touches at only a single
      corner. The notes generalize this: “we analyze the tours that satisfy the inequality as an equation
      … check that it lives in a space of dimension exactly one less than the space of the TSP polytope”
      (p. 218). Non-facet cuts are weaker, because some facet-defining cut dominates them.
    options:
      - text: "The non-facet one is invalid"
      - text: "There is no difference"
      - text: "The facet one has bigger coefficients"
      - text: "The facet one touches along a full side (dimension one less than the polytope); the other touches only a corner"
        correct: true
  - id: clique-tree
    prompt: >-
      Clique-tree inequalities (Grötschel and Pulleyblank) generalize combs how?
    explanation: >-
      They extend “combs to multiple-handle configurations. Their structure is called a clique tree due to
      the tree-like nature of the permitted Venn diagrams” (p. 134), with handles and teeth arranged in a
      tree. They are facet-defining (p. 136). Later work by Naddef and Rinaldi in the 1990s added many more
      families, yet theory explains “only a small fraction of the fifty-one billion facets that are known for
      the ten-city polytope” (p. 137).
    options:
      - text: "By removing the handle"
      - text: "By restricting to 3 cities"
      - text: "By using even numbers of teeth"
      - text: "By allowing several handles linked with teeth in a tree-like structure"
        correct: true
  - id: target-rich
    prompt: >-
      What does Cook mean by calling TSP polyhedral theory “a target-rich environment”?
    explanation: >-
      “Our knowledge of the TSP polytope is far from complete … we know that much remains to be discovered”
      (p. 137). Known families explain a sliver of the facets even at n = 10. New understandable families
      with fast separation could raise LP bounds and speed exact solvers. The catalog work by Naddef and
      Rinaldi “has provided a wealth of information that has not yet been fully exploited in computational
      studies” (p. 137).
    options:
      - text: "Only combs matter"
      - text: "The TSP is solved"
      - text: "Most facets are unexplained, so there is plenty of room for new useful inequalities"
        correct: true
      - text: "There are no more facets to find"
---
## Learning objectives

- Define combs and prove the 3k + 1 bound.
- Explain facets and why they are the best cuts.
- Know the main families: subtour, comb, clique tree, and others.

## Combs

A **handle** H and an **odd** number k ≥ 3 of disjoint **teeth**, each meeting H and also leaving it. Every tour crosses the borders at least **3k + 1** times (p. 133):

- each tooth ≥ 2, giving 2k;
- the handle ≥ k, and crossings of a set are even, so with k odd it is ≥ k + 1.

Using crossing counts x(δ(S)) = Σ_{i∈S, j∉S} xᵢⱼ, the comb inequality reads

> x(δ(H)) + Σᵢ x(δ(Tᵢ)) ≥ 3k + 1.

Dantzig et al.’s four-set configuration (k = 3, giving 10) is a comb. Chvátal (1973) introduced combs, Grötschel–Padberg (1979) extended them, and Grötschel used them by hand for 120 cities (pp. 132–134).

## Facets

In a d-dimensional polytope, a **facet** is a (d − 1)-dimensional face. Facet-defining inequalities are necessary in every complete description and are the strongest cuts (p. 136). Subtours, combs and clique trees are facet-defining (p. 136). The 10-city polytope has about 51 billion facets, and theory explains only a fraction of them (p. 137).

| Family | Who | Structure |
|---|---|---|
| Subtour | DFJ 1954 | One set S |
| Comb | Chvátal 1973; Grötschel–Padberg 1979 | Handle + odd teeth |
| Clique tree | Grötschel–Pulleyblank 1986 | Tree of handles and teeth |
| Many more | Naddef–Rinaldi 1990s | Path, wheelbarrow, ladder, … |

## In software terms

A catalog of constraint *templates* plus fast detectors is a pattern you can reuse. Static analyzers work the same way: a rule library and a matcher that finds violations in the current code.

## Sources

- Cook (2012), pp. 131–137, 217–218.
- Chvátal, V. “Edmonds polytopes and weakly hamiltonian graphs.” *Math. Programming* 5 (1973), 29–40.
- Grötschel, M., M. W. Padberg. “On the symmetric travelling salesman problem I and II.” *Math. Programming* 16 (1979), 265–302.
- Naddef, D. “Polyhedral theory and branch-and-cut algorithms for the symmetric TSP.” In Gutin and Punnen (2002), 29–116.
