---
title: "Galanter's Murals, Continuous Lines, Bosch–Kaplan Drawings (pp. 203–207)"
questions:
  - id: galanter
    prompt: >-
      What did Philip Galanter observe about his TSP murals built from random points?
    explanation: >-
      “Somewhat mysteriously, each resulting mural exhibits a shared visual style” (p. 204). He compares it to
      plants optimizing sunlight with minimal resources and argues beauty is “both meaningful and publicly
      understandable” (p. 204). Optimal tours on random points have a recognizable look, an emergent property
      of the optimization.
    options:
      - text: "The murals were unreadable"
      - text: "They required hand-drawn tours"
      - text: "Each mural looked completely different"
      - text: "Despite random points, all murals share a visual style — emergence from optimization"
        correct: true
  - id: mo-density
    prompt: >-
      How does J. Eric Morales (“Mo”) create tone with a single continuous line?
    explanation: >-
      “He could create images of photo-like quality by modulating a continuous line’s distance from itself—
      closer for dark areas and further for lighter ones. The resulting curves have clear similarities to short
      TSP tours” (p. 205). Line density encodes darkness, as in halftone printing.
    options:
      - text: "By using multiple pens"
      - text: "By varying line color"
      - text: "By varying how closely the line approaches itself: dense for dark, sparse for light"
        correct: true
      - text: "By crossing the line"
  - id: bosch-herman
    prompt: >-
      How did Bosch and Herman place cities to turn an image into a TSP?
    explanation: >-
      “Divide an image into a grid and place between 0 and k cities at random locations in each grid cell,
      depending on the average level of gray in the cell” (p. 207). A short tour then draws the image. The
      clustered random points produced jagged paths, which Bosch and Kaplan fixed (p. 207).
    options:
      - text: "Grid cells get 0..k random cities according to darkness; a short tour draws the picture"
        correct: true
      - text: "Cities at edges detected by a filter"
      - text: "Random uniform cities"
      - text: "One city per pixel"
  - id: bosch-kaplan
    prompt: >-
      What did Bosch and Kaplan improve, and how?
    explanation: >-
      They adopted “a multiple-round algorithm for distributing the cities, based on computing centers of
      gravity … the tone of the original image is used to weight the geometric distances, so that points
      migrate to dark regions. Overall, the process avoids abrupt transitions in the density of the cities”
      (p. 207). It is based on Secord’s weighted Voronoi stippling (p. 221). The result is smooth tonal
      gradients, used for the 100k Mona Lisa and the 140,000-city *Birth of Venus* (p. 206).
    options:
      - text: "They used color instead of tone"
      - text: "They used optimal tours only"
      - text: "Iterative weighted centroid (Voronoi stippling) placement, so city density varies smoothly with tone"
        correct: true
      - text: "They removed dark areas"
  - id: pipeline
    prompt: >-
      As a software pipeline, which stage order describes TSP Art?
    explanation: >-
      Image → stippling (weighted centroid placement of cities by tone, p. 207) → heuristic TSP tour (exact is
      impractical for 100k+ cities) → render the closed line. Bosch adds side constraints and iterates when
      designing sculptures (pp. 202–203). The same pattern of preprocess, optimize, render, then review
      against the goal appears in data visualization and plotter art.
    options:
      - text: "Image → stipple cities by tone → find a short tour → render the line"
        correct: true
      - text: "Tour → image → stipple"
      - text: "Random points → render → image"
      - text: "Render → stipple → tour → image"
---
## Learning objectives

- Describe how images become TSP instances and tours become drawings.

## Summary

- **Galanter:** optimal tours on random points share an emergent style (pp. 203–205).
- **Mo’s Labyrinthine Projection:** one continuous non-crossing line, with darkness encoded by line density (p. 205).
- **TSP Art:** Bosch–Herman grid placement, then Bosch–Kaplan weighted-centroid stippling for smooth tones. Heuristic tours on 100k–200k cities (pp. 206–207).

## In software terms

Stippling plus a TSP tour is a practical way to drive pen plotters, laser engravers or CNC tools from an image. It is the same tool-path optimization as chapter 3, applied to art.

## Sources

- Cook (2012), pp. 203–207, 221.
- Kaplan, C. S., R. Bosch. “TSP Art.” *Proceedings of Bridges 2005*, 301–308.
- Secord, A. “Weighted Voronoi stippling.” *Proc. NPAR* (2002), 37–43.
