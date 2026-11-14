---
title: "Road Trips (pp. 44–49)"
questions:
  - id: gps-small-tsp
    prompt: >-
      Consumer GPS units often embed a TSP solver for about a dozen stops. Why is that enough,
      and what makes their answers realistic?
    explanation: >-
      “Mapping software running on the GPS unit often includes a TSP solver for small instances
      having a dozen or so cities, and this is usually adequate for daily trips. Detailed maps
      stored in the unit can be used to deliver accurate estimates of the time to travel from
      point to point, allowing TSP solutions to reflect actual driving conditions” (p. 44). Small
      n allows exact methods, even Held–Karp at 12 stops (12² · 2¹² ≈ 600,000 steps). The
      realism comes from the cost matrix, which uses travel *times* from road data.
    options:
      - text: "Daily trips have few stops, which exact methods handle easily; realism comes from road-network travel times"
        correct: true
      - text: "Because they use straight-line distance"
      - text: "Because they ignore the return trip"
      - text: "Because GPS units are supercomputers"
  - id: asymmetric-one-way
    prompt: >-
      The Rapidis routing for Forbruger-Kontakt in Denmark obeys one-way streets. What property
      of the cost matrix follows?
    explanation: >-
      “The route in the drawing obeys one-way streets and other travel restrictions, making the
      cost to travel between two points depend on the direction that is chosen” (p. 45). That is
      an **asymmetric** TSP: cost(i, j) ≠ cost(j, i). The book mostly treats symmetric costs, but
      its notes point out that any n-city asymmetric instance can be converted to a symmetric one
      with 2n cities (p. 215). Concorde handled this instance. In practice your cost matrix
      should be directional, because traffic, turn restrictions and network egress costs are rarely
      symmetric.
    options:
      - text: "The triangle inequality must hold with equality"
      - text: "It becomes symmetric"
      - text: "It becomes asymmetric: cost(i, j) may differ from cost(j, i)"
        correct: true
      - text: "All costs become zero"
  - id: spacefilling-curve
    prompt: >-
      Atlanta's Meals on Wheels used a spacefilling-curve heuristic stored on index cards. What
      was the main advantage?
    explanation: >-
      Each location gets a position h along a fine spacefilling curve, and the tour is the order
      of increasing h. “To delete a client, his two cards are simply removed. To insert a new
      client … the table is used to look up the corresponding value of h, and h is used to insert
      the client’s card into the tour order” (pp. 46–47). Insert and delete are local operations
      and need no re-optimization: “an ingenious, low-tech solution.” Tours are typically about
      25% above optimal for random points, but the scheme is easy to maintain by hand. Today the
      same idea appears in Hilbert-curve indexes and Z-order keys in databases.
    options:
      - text: "Clients can be inserted or removed in sorted order of a precomputed curve position, without re-solving"
        correct: true
      - text: "It requires a supercomputer"
      - text: "It handles one-way streets perfectly"
      - text: "It produces provably optimal tours"
  - id: route-split
    prompt: >-
      Meals on Wheels built one tour through about 200 clients and cut it into segments of 30–40
      stops per driver. What general technique is this?
    explanation: >-
      “All 200 locations are placed in a tour that is divided into segments of the appropriate
      lengths” (p. 46). This is *route-first, cluster-second*: solve one giant TSP, then cut it
      into feasible routes for each vehicle. It is a simple and often decent way to approach
      vehicle-routing problems with a TSP engine.
    options:
      - text: "Simulated annealing"
      - text: "Cluster-first, route-second"
      - text: "Route-first, cluster-second: one giant tour split into driver segments"
        correct: true
      - text: "Branch-and-bound"
  - id: not-exact-tsp
    prompt: >-
      Extra Miler Club members try to visit every U.S. county, and the Iron Butt 48-states ride
      needs a receipt from each state. Why are these *not* plain TSPs?
    explanation: >-
      For counties, “crossing any point of a county line is sufficient” (p. 48), and for the
      Iron Butt ride “there are many choices for potential stops in each state” (p. 48). Each
      region can be satisfied by any of many points. This is a generalized (or set) TSP, which
      visits one location from each group. Cook suggests fixing a list of gas stations per state
      as a modeling choice. When requirements say ‘visit each region’, check whether you need one
      specific point per region or any point in it.
    options:
      - text: "Because distances are unknown"
      - text: "Because motorcycles are faster than cars"
      - text: "Because each region can be satisfied by any of many points, which makes it a generalized TSP"
        correct: true
      - text: "Because there is no return trip"
  - id: schreck-modification
    prompt: >-
      Pilot Ron Schreck flew to 109 North Carolina airports in one day using a Concorde tour that he
      “modified slightly.” Why modify an optimal tour?
    explanation: >-
      He changed it to reach “before sunrise several airports having lighted runways” (p. 49).
      Concorde had optimized pure distance. The real problem had time windows, since unlit
      airports needed daylight. An optimal solution to a simplified model is a starting point,
      and side constraints such as time windows, capacities and precedence often require changes.
      The same goes for Gatensby’s remark that “sometimes the shortest route isn’t the ‘best’
      route” (p. 48).
    options:
      - text: "Concorde's tour was wrong"
      - text: "Real constraints (lighted runways before sunrise) were not in the distance-only model"
        correct: true
      - text: "To make the route longer for fun"
      - text: "Because airports moved"
---
## Learning objectives

- Recognize asymmetric costs, generalized TSPs and route splitting in practical routing.
- Appreciate the gap between the pure model and operational constraints.

## Examples

- **GPS units** solve TSPs of about a dozen stops using road-network travel times (p. 44). Alain Kornhauser describes trucks that find unmapped lanes, which the server then adds to the road grid (p. 45).
- **Pick-ups and deliveries.** Flood began with school buses. Morton and Land started with laundry vans. Rapidis used Concorde with direction-dependent costs (one-way streets) (p. 45).
- **Meals on Wheels**, Atlanta. A spacefilling curve orders about 200 clients, and the tour is cut into driver routes. Index cards allow O(1)-style inserts and deletes (pp. 46–47).
- **Remote inspections.** Helicopter tours of 47 Nigerian oil platforms. About 200 blue-crab monitoring stations in Chesapeake Bay (p. 47).
- **Book tours, Extra Milers, Iron Butt**, where each region can be satisfied by many possible points (pp. 47–48).
- **Ron Schreck** flew to all 109 public airports in North Carolina, 1,991 miles in 17 hours, starting from a Concorde tour adjusted for lighting (p. 49).

## Modeling checklist

| Situation | Model consequence |
|---|---|
| One-way streets, traffic | Asymmetric costs |
| Any point in a region counts | Generalized (set) TSP |
| Several vehicles | Route-first/cluster-second, or a vehicle-routing model |
| Time windows (daylight, SLAs) | Side constraints. The pure TSP optimum needs adjusting |
| Frequent insert/delete | Stable orderings such as spacefilling curves |

## Sources

- Cook (2012), pp. 44–49.
- Bartholdi, J. J., et al. “A minimal technology routing system for Meals on Wheels.” *Interfaces* 13(3) (1983), 1–8.
- Platzman, L. K., J. J. Bartholdi. “Spacefilling curves and the planar travelling salesman problem.” *J. ACM* 36 (1989), 719–737.
