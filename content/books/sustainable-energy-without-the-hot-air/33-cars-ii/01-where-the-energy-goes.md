---
title: "Where a car's energy goes — brakes versus swirling air"
questions:
- id: q1
  prompt: "What is the \"key formula\" for most of the book's calculations?"
  options:
  - text: F = ma
  - text: E = mc²
  - text: Kinetic energy = ½mv²
    correct: true
  - text: Power = voltage × current
  explanation: "**p. 254:** “The key formula for most of the calculations in this book is: kinetic energy = ½mv².”"
- id: q2
  prompt: What is the kinetic energy of a 1000 kg car at 100 km/h?
  options:
  - text: About 390 000 J ≈ 0.1 kWh
    correct: true
  - text: About 1 kWh
  - text: About 3.9 kWh
  - text: About 39 J
  explanation: "**p. 254:** “a car of mass m = 1000 kg moving at 100 km per hour or v = 28 m/s has an energy of ½mv² ≃ 390 000 J ≃ 0.1 kWh.”"
- id: q3
  prompt: Where does energy in a typical fossil-fuel car go?
  type: multiple
  options:
  - text: Rolling resistance
    correct: true
  - text: Charging the battery for headlights
  - text: Air resistance
    correct: true
  - text: "Heat — 75% thrown away by the inefficient conversion chain"
    correct: true
  - text: Speeding up then braking
    correct: true
  explanation: "**p. 254:** “1. speeding up then slowing down using the brakes; 2. air resistance; 3. rolling resistance; 4. heat – 75% of the energy is thrown away as heat”."
- id: q4
  prompt: How do both braking and air-swirling dissipation scale with speed?
  options:
  - text: As v³ — halving speed cuts power 8-fold and journey energy 4-fold
    correct: true
  - text: As v — linear
  - text: "They don't depend on speed"
  - text: As v² — halving speed halves power
  explanation: "**p. 255:** “Both forms of energy dissipation scale as v³. So this cartoon predicts that a driver who halves his speed v makes his power consumption 8 times smaller … the total energy consumed by his journey will be four times smaller.”"
- id: q5
  prompt: What is the drag coefficient cd?
  options:
  - text: The coefficient of rolling resistance
  - text: "The ratio of the swirling-air tube's effective cross-section to the car's frontal area"
    correct: true
  - text: "The car's mass divided by its area"
  - text: The ratio of top speed to engine power
  explanation: "**p. 255:** “the ratio of the tube’s effective cross-sectional area to the car area is called the drag coefficient cd.”"
- id: q6
  prompt: When is energy dissipation braking-dominated?
  options:
  - text: When the car is heavier than 2 tons
  - text: "When the car's mass exceeds the mass of the tube of air swept out between stop signs"
    correct: true
  - text: Always on motorways
  - text: When the drag coefficient is above 1
  explanation: "**p. 255:** “energy dissipation is dominated by kinetic-energy-being-dumped-into-the-brakes if the mass of the car is bigger than the mass of the tube of air from one stop sign to the next”."
- id: q7
  prompt: "What is the special distance d* between stops for a typical 1000 kg car?"
  options:
  - text: About 7.5 km
  - text: About 75 m
  - text: About 75 km
  - text: About 750 m
    correct: true
  explanation: "**p. 256:** “d* = mc/(ρ cd Acar) = 1000 kg / (1.3 kg/m³ × 1/3 × 3 m²) = 750 m.”"
- id: q8
  prompt: "When stops are less than 750 m apart, what saves energy?"
  type: multiple
  options:
  - text: "Reduce the car's mass"
    correct: true
  - text: Drive more slowly
    correct: true
  - text: Get regenerative brakes
    correct: true
  - text: Reduce the drag coefficient only
  explanation: "**p. 256:** “it’s a good idea, if you want to save energy: 1. to reduce the mass of your car; 2. to get a car with regenerative brakes …; and 3. to drive more slowly.”"
- id: q9
  prompt: "When stops are well over 750 m apart, does the car's weight matter much?"
  options:
  - text: "No — dissipation is drag-dominated, whether the car holds one person or six"
    correct: true
  - text: "Yes, it is the dominant factor"
  - text: Only uphill
  - text: Only for electric cars
  explanation: "**p. 256:** “When the stops are significantly more than 750 m apart, energy dissipation is drag-dominated. Under these conditions, it doesn’t much matter what your car weighs.”"
- id: q10
  prompt: What engine power does the theory predict at 110 km/h with cdA = 1 m²?
  options:
  - text: About 800 kW
  - text: About 20 kW
  - text: About 8 kW
  - text: About 80 kW — 80 kWh per 110 km
    correct: true
  explanation: "**p. 256:** “4 × ½ρAv³ = 2 × 1.3 kg/m³ × 1 m² × (31 m/s)³ = 80 kW.”"
- id: q11
  prompt: Could a car of the same shape use 100 times less energy at 70 mph?
  options:
  - text: "No — at 70 mph the energy goes mainly into swirling air, which materials can't change"
    correct: true
  - text: "Yes, with carbon fibre"
  - text: "Yes, with narrower tyres"
  - text: "Yes, with a better engine"
  explanation: "**p. 257:** “Could we make a new car that consumes 100 times less energy and still goes at 70 mph? No. Not if the car has the same shape.”"
- id: q12
  prompt: "What might a miraculous 50%-efficient fossil engine achieve?"
  options:
  - text: About 1 kWh per 100 km
  - text: No change
  - text: About 40 kWh per 100 km
    correct: true
  - text: About 4 kWh per 100 km
  explanation: "**p. 257:** “A miraculous improvement to the fossil-fuel engine could perhaps boost its efficiency from 25% to 50%, bringing the energy consumption of a fossil-fuelled car down to roughly 40 kWh per 100 km.”"
- id: q13
  prompt: What efficiency can electric motors achieve?
  options:
  - text: "90%"
    correct: true
  - text: "25%"
  - text: "50%"
  - text: "99.9%"
  explanation: "**p. 257:** “electric motors can be 90% efficient.”"
---
## Where to read

Technical chapter A, book pages 254–256 (PDF pages 267–269 of the free edition at [withouthotair.com](http://www.withouthotair.com)).

## Learning objectives

- Derive the power going into brakes and into swirling air
- Explain the special distance d* between braking- and drag-dominated driving
- Check the theory against 80 kWh per 100 km
