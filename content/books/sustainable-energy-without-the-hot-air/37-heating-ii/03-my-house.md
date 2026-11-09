---
title: "Leakiness — example: MacKay's house"
questions:
- id: q1
  prompt: "What was the total leakiness of MacKay's house in 2006, and its split?"
  options:
  - text: "3220 W/°C — 50/50"
  - text: "7.7 W/°C — all ventilation"
  - text: "322 W/°C (7.7 kWh/d/°C) — 72% conductive, 28% ventilation"
    correct: true
  - text: "120 W/°C — 28% conductive"
  explanation: "**p. 293:** “The total leakiness of the house was 322 W/◦C (or 7.7 kWh/d/◦C), with conductive leakiness accounting for 72% and ventilation leakiness for 28% of the total.”"
- id: q2
  prompt: "What is the \"heat-loss parameter\" of a building?"
  options:
  - text: The U-value of the roof
  - text: "Leakiness divided by floor area, in W/°C/m²"
    correct: true
  - text: Air changes per hour
  - text: Leakiness times floor area
  explanation: "**p. 294:** “we can divide the leakiness by the floor area; this gives the heat-loss parameter of the building, which is measured in W/◦C/m². The heat-loss parameter of this house (total floor area 88 m²) is 3.7 W/◦C/m².”"
- id: q3
  prompt: "If the house is always held at 19 °C, what is the average heat loss?"
  options:
  - text: About 155 kWh/d
  - text: About 13 kWh/d
  - text: About 6 kWh/d
  - text: "About 61 kWh/d (48 at 17 °C, 75 at 21 °C)"
    correct: true
  explanation: "**p. 295:** “7.7 kWh/d/◦C × 2866 degree-days/y / (365 days/y) = 61 kWh/d. Turning the thermostat down to 17 ◦C, the average rate of heat loss drops to 48 kWh/d. Turning it up to a tropical 21 ◦C … 75 kWh/d.”"
- id: q4
  prompt: "By how much did the 2007 modifications reduce the house's leakiness?"
  options:
  - text: Not at all
  - text: "About 5%"
  - text: "About 75%"
  - text: "About 25%, from about 320 to 240 W/°C"
    correct: true
  explanation: "**p. 295:** “That’s a total reduction in leakiness of 25%, from roughly 320 to 240 W/◦C (7.7 to 6 kWh/d/◦C).”"
- id: q5
  prompt: "What heat-loss parameter do new building codes require of a \"sustainable\" house?"
  options:
  - text: "3.7 W/°C/m²"
  - text: "0.11 W/°C/m²"
  - text: "1.1 W/°C/m²"
    correct: true
  - text: "11 W/°C/m²"
  explanation: "**p. 295:** “(This is a long way from the 1.1 W/◦C/m² required of a ‘sustainable’ house in the new building codes.)”"
- id: q6
  prompt: Which modification gave the biggest predicted saving on a cold day?
  options:
  - text: Ventilation reductions — 2.9 kWh/d
  - text: Double-glazing two doors and one window — 1.9 kWh/d
  - text: Cavity-wall insulation — 4.8 kWh/d
    correct: true
  - text: Improved roof insulation — 3.5 kWh/d
  explanation: "**Table E.9, p. 295:** “Cavity-wall insulation … 4.8 kWh/d; Improved roof insulation 3.5 kWh/d; … double-glazing two doors and one window 1.9 kWh/d; Ventilation reductions … 2.9 kWh/d”."
- id: q7
  prompt: "How much did MacKay's gas consumption actually fall after insulation and thermostat changes?"
  options:
  - text: From 13 to 40 kWh/d
  - text: "From 40 to 13 kWh/d — 67%"
    correct: true
  - text: "From 40 to 30 kWh/d — 25%"
  - text: From 40 to 39 kWh/d
  explanation: "**p. 296:** “my gas consumption has gone down from 40 kWh/d to 13 kWh/d – a reduction of 67%.”"
- id: q8
  prompt: "What is \"frustratingly hard\", according to MacKay?"
  options:
  - text: Turning down a thermostat
  - text: Making a really big dent in the leakiness of an already-built house
    correct: true
  - text: Reading a meter
  - text: Installing a heat pump
  explanation: "**p. 296:** “It’s frustratingly hard to make a really big dent in the leakiness of an already-built house! … a much easier way of achieving a big dent in heat loss is to turn the thermostat down.”"
- id: q9
  prompt: What complications do internal wall insulation linings have?
  type: multiple
  options:
  - text: "Condensation may form behind it, causing damp"
    correct: true
  - text: The covering must be thicker than you might expect — about 6 cm to turn a 2.2 wall into 0.3
    correct: true
  - text: They increase the U-value
  - text: They are always illegal
  explanation: "**p. 296:** “roughly 6 cm of insulated lining board is required … Second, condensation may form on the hidden surface of such internal insulation layers, leading to damp problems.”"
- id: q10
  prompt: What U-value results from 1.8 cm insulated wallboard (U 1.7) over a 2.2 W/m²/K brick wall?
  options:
  - text: About 1 W/m²/K
    correct: true
  - text: About 0.3 W/m²/K
  - text: About 3.9 W/m²/K
  - text: About 2.2 W/m²/K
  explanation: "**p. 296:** combining in series, 1/(1/2.2 + 1/1.7) ≈ 1, so the book says the U-value falls from 2.2 W/m²/K to about 1 W/m²/K: “Definitely a worthwhile reduction.”"
---
## Where to read

Technical chapter E, book pages 293–296 (PDF pages 306–309 of the free edition at [withouthotair.com](http://www.withouthotair.com)).

## Learning objectives

- Break down a house's leakiness into conduction and ventilation
- Predict and compare savings from insulation and thermostat changes
- Assess internal wall coverings
