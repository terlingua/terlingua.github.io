---
title: How densely can windmills be packed?
questions:
- id: q1
  prompt: How closely can windmills be spaced without losing significant power?
  options:
  - text: About one diameter apart
  - text: Any spacing is fine
  - text: About 50 diameters apart
  - text: No closer than about 5 times their diameter
    correct: true
  explanation: "**p. 265:** “Experts say that windmills can’t be spaced closer than 5 times their diameter without losing significant power.”"
- id: q2
  prompt: What power per unit land area does a 6 m/s wind farm give?
  options:
  - text: About 22 W/m²
  - text: About 140 W/m²
  - text: "About 2.2 W/m² — \"2 W per m²\" worth remembering"
    correct: true
  - text: About 0.2 W/m²
  explanation: "**p. 265:** “= 0.016 × 140 W/m² = 2.2 W/m². This number is worth remembering: a wind farm with a wind speed of 6 m/s produces a power of 2 W per m² of land area.”"
- id: q3
  prompt: "Why doesn't wind-farm power per unit area depend on windmill diameter?"
  options:
  - text: Wind speed is constant with height
  - text: All windmills are the same size
  - text: "The d's cancel because bigger windmills must be spaced further apart"
    correct: true
  - text: Bigger blades are less efficient
  explanation: "**p. 265:** “Notice that our answer does not depend on the diameter of the windmill. The ds cancelled because bigger windmills have to be spaced further apart.”"
- id: q4
  prompt: What are the only reasons to prefer big windmills?
  options:
  - text: Better appearance
  - text: "Catching higher wind speeds higher up, and economies of scale"
    correct: true
  - text: Less noise
  - text: Higher power per unit area at the same wind speed
  explanation: "**p. 265:** “Bigger windmills might be a good idea in order to catch bigger windspeeds that exist higher up …, or because of economies of scale, but those are the only reasons for preferring big windmills.”"
- id: q5
  prompt: "If 6 m/s is replaced by Bedford's 4 m/s, by what factor does the estimate shrink?"
  options:
  - text: About 0.67 — 4/6
  - text: No change
  - text: About 0.44 — (4/6)²
  - text: About 0.3 — (4/6)³
    correct: true
  explanation: "**p. 266:** “If we replace 6 m/s by Bedford’s 4 m/s … we must scale our estimate down, multiplying it by (4/6)³ ≃ 0.3. (Remember, wind power scales as wind-speed cubed.)”"
- id: q6
  prompt: Why should we use the mean cube of the wind speed rather than the cube of the mean?
  options:
  - text: The cube of the mean is always bigger
  - text: They are always equal
  - text: The average of the cube is bigger than the cube of the average
    correct: true
  - text: Wind speed is always constant
  explanation: "**p. 266:** “we shouldn’t take the mean wind speed and cube it; rather, we should find the mean cube of the windspeed. The average of the cube is bigger than the cube of the average.”"
---
## Where to read

Technical chapter B, book pages 265–266 (PDF pages 278–279 of the free edition at [withouthotair.com](http://www.withouthotair.com)).

## Learning objectives

- Derive wind-farm power per unit area from 5-diameter spacing
- Explain why the answer is independent of windmill size
- Explain how wind speed scaling affects the estimate
