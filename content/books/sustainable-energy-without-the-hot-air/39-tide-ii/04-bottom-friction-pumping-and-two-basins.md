---
title: "Bottom friction, the pumping trick, and two basins"
questions:
- id: q1
  prompt: What model of bottom friction does MacKay use?
  options:
  - text: Stress proportional to U
  - text: No friction
  - text: "Stress R1ρU² (power R1ρU³), with shear friction coefficient R1 around 0.003–0.01"
    correct: true
  - text: Stress proportional to depth
  explanation: "**p. 318:** “One widely used model says that the magnitude of the stress (force per unit area) is R1ρU², where U is the average flow velocity and R1 is a dimensionless quantity called the shear friction coefficient.”"
- id: q2
  prompt: "Why is the bottom-friction comparison \"good news\"?"
  options:
  - text: It shows friction is zero
  - text: It shows tide farms are impossible
  - text: It shows tidal power is tiny
  - text: "Natural friction already dissipates about as much as a tide farm would extract, so turbines won't radically alter the flow"
    correct: true
  explanation: "**p. 318:** “the friction power is very similar to the estimated power that a tide farm would deliver. This is good news, because it suggests that planting a forest of underwater windmills … won’t radically alter the flow.”"
- id: q3
  prompt: "By how much does the pumping trick sometimes boost La Rance's net generation?"
  options:
  - text: "About 10%"
    correct: true
  - text: "About 1%"
  - text: "About 50%"
  - text: "About 400%"
  explanation: "**p. 318:** “The pumping trick is sometimes used at La Rance, boosting its net power generation by about 10%”."
- id: q4
  prompt: "With a 4 m range and 76% round-trip efficiency, what is the optimal pumping boost height?"
  options:
  - text: About 30 m
  - text: About 4 m
  - text: About 13 m
    correct: true
  - text: About 1.3 m
  explanation: "**p. 319:** “For example, with a tidal range of 2h = 4 m, and a round-trip efficiency of ε = 76%, the optimal boost is b = 13 m.”"
- id: q5
  prompt: Why will full pumping rarely be exploited?
  options:
  - text: It reduces power
  - text: It is illegal
  - text: "It needs walls about 4 times the tidal range, and wall material scales as height squared"
    correct: true
  - text: Pumps are too inefficient
  explanation: "**p. 319:** “full exploitation of pumping requires the total height of the pool to be roughly 4 times the tidal range … But the amount of material in a sea-wall of height H scales as H², so the cost of constructing a wall four times as high will be more than four times as big.”"
- id: q6
  prompt: "How can the pumping trick be used \"for free\"?"
  options:
  - text: At spring tides only
  - text: "At neap tides, pumping the pool up to the springs range — halving the two-weekly power variation"
    correct: true
  - text: By using wind power
  - text: By closing the sluices
  explanation: "**p. 320:** “Pumping the pool at neaps so that the full springs range is used thus allows neap tides to deliver roughly twice as much power … So a system with pumping would show two-weekly variations in power of just a factor of 2 instead of 4.”"
- id: q7
  prompt: "What steady power density did MacKay's simulation of a two-lagoon system with a 4 m range achieve?"
  options:
  - text: About 45 W/m²
  - text: About 3 W/m²
  - text: "About 4.5 W/m² — 50% more than an ordinary tide-pool's 3 W/m²"
    correct: true
  - text: About 0.45 W/m²
  explanation: "**p. 320:** “a two-lagoon system in a location with a natural tidal range of 4 m can, with an appropriate pumping schedule, deliver a steady power of 4.5 W/m² … 50% bigger than the maximum possible average power density of an ordinary tide-pool in the same location (3 W/m²).”"
- id: q8
  prompt: What delightful coincidence makes two-pool self-pumping work?
  options:
  - text: Electricity is free at high tide
  - text: The moon pumps the water
  - text: High tide is the best time both to pump into the high pool and to generate from the low pool (and vice versa at low tide)
    correct: true
  - text: Both pools are always full
  explanation: "**p. 320:** “It’s a delightful feature of a two-pool solution that the optimal time to pump water into the high pool is high tide, which is also the optimal time to generate power from the low pool.”"
- id: q9
  prompt: Where is there an existing two-basin tidal power plant?
  options:
  - text: "La Rance, France"
  - text: "Haishan, Maoyan Island, China — about 39 kW on average"
    correct: true
  - text: Strangford Lough
  - text: The Severn
  explanation: "**Notes, p. 321:** “There is a two-basin tidal power plant at Haishan, Maoyan Island, China … and generates 39 kW on average.”"
---
## Where to read

Technical chapter G, book pages 318–321 (PDF pages 331–334 of the free edition at [withouthotair.com](http://www.withouthotair.com)).

## Learning objectives

- Use bottom friction as a cross-check on tide-farm power
- Explain the pumping trick and its practical limits
- Describe the always-on two-basin scheme
