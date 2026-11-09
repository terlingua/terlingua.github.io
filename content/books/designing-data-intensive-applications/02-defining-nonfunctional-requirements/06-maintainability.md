---
title: "Maintainability"
questions:
  - id: maintenance-cost
    prompt: "Where does the majority of software cost lie?"
    options:
      - id: o805bd3
        text: "In ongoing maintenance, not initial development"
        correct: true
      - id: oe5da65
        text: "In hardware"
      - id: o250eb2
        text: "In licensing"
      - id: o2a75ac
        text: "In initial development"
    explanation: "“the majority of the cost of software is not in its initial development but in its ongoing maintenance”."
  - id: legacy-everything
    prompt: "What does the chapter say will happen to every system built today?"
    options:
      - id: o17f246
        text: "It will migrate to the cloud automatically"
      - id: oad1690
        text: "It will never need maintenance if well designed"
      - id: o20afdc
        text: "It will one day become legacy if it is valuable enough to survive"
        correct: true
      - id: o87135e
        text: "It will be rewritten within two years"
    explanation: "“Every system we create today will one day become a legacy system if it is valuable enough to survive for a long time.”"
  - id: three-principles
    prompt: "Match each principle to its definition: making it easy for new engineers to understand the system."
    options:
      - id: ob5cd03
        text: "Operability"
      - id: o9fa22a
        text: "Scalability"
      - id: o9dcc6c
        text: "Evolvability"
      - id: oe26090
        text: "Simplicity"
        correct: true
    explanation: "“Simplicity Make it easy for new engineers to understand the system, by implementing it using well-understood, consistent patterns and structures and avoiding unnecessary complexity.”"
  - id: ops-quote
    prompt: "What does the quoted maxim say about operations and software?"
    options:
      - id: o1565ac
        text: "Good software removes the need for operations"
      - id: o0df9d2
        text: "Operations and software are independent"
      - id: o3320ca
        text: "Bad operations can be fixed by automation alone"
      - id: oc60a02
        text: "Good operations can work around bad software, but good software cannot run reliably with bad operations"
        correct: true
    explanation: "“good operations can often work around the limitations of bad (or incomplete) software, but good software cannot run reliably with bad operations”."
  - id: automation-edge
    prompt: "Why can more automation demand a more skilled operations team?"
    options:
      - id: oc40373
        text: "Automation removes monitoring"
      - id: o53f3db
        text: "The cases left for humans are the rare, most complex ones, and automated systems that go wrong are harder to troubleshoot"
        correct: true
      - id: obc0504
        text: "Automated systems never fail"
      - id: o2149ff
        text: "Automation tools are expensive to license"
    explanation: "“since the cases that cannot be handled automatically tend to be the most complex, greater automation requires a more skilled operations team” and “an automated system that goes wrong is often harder to troubleshoot”."
  - id: operability-features
    prompt: "Which are ways a data system supports good operability?"
    type: multiple
    options:
      - id: o81e93d
        text: "Allowing machines to be taken down without stopping the system"
        correct: true
      - id: o94d16c
        text: "Self-healing with manual control available"
        correct: true
      - id: o2617d2
        text: "Hiding all metrics from operators"
      - id: o7c72aa
        text: "Predictable behaviour that minimizes surprises"
        correct: true
      - id: of13787
        text: "Good defaults that administrators can override"
        correct: true
    explanation: "The list includes “Avoiding dependency on individual machines”, “Providing good default behavior, but also giving administrators the freedom to override defaults”, “Self-healing where appropriate, but also giving administrators manual control”, and “Exhibiting predictable behavior, minimizing surprises.”"
  - id: complexity-risk
    prompt: "Why does complexity increase the risk of bugs when making changes?"
    options:
      - id: o269ea8
        text: "Complex code always compiles slower"
      - id: o8ddac2
        text: "Complexity reduces test coverage automatically"
      - id: o9f5ecf
        text: "Complex systems run on older hardware"
      - id: ob94308
        text: "Hidden assumptions, unintended consequences and unexpected interactions are more easily overlooked"
        correct: true
    explanation: "“When the system is harder for developers to understand and reason about, hidden assumptions, unintended consequences, and unexpected interactions are more easily overlooked”."
  - id: simple-subjective
    prompt: "Why is \"simplicity\" hard to pin down?"
    options:
      - id: o9e6abd
        text: "Simple systems cannot be abstracted"
      - id: oda7d92
        text: "Simplicity is defined by ISO standards"
      - id: o5ec383
        text: "Simplicity can be measured with lines of code"
      - id: o6434de
        text: "There is no objective standard; a simple interface over a complex implementation versus a simple implementation exposing detail is a judgment call"
        correct: true
    explanation: "“Whether something is simple is often a subjective matter, as there is no objective standard of simplicity”."
  - id: essential-accidental
    prompt: "What is the weakness of the essential-versus-accidental complexity split?"
    options:
      - id: o585a19
        text: "The boundary shifts as tooling evolves"
        correct: true
      - id: offafe1
        text: "It ignores hardware"
      - id: o1f614d
        text: "Essential complexity can always be removed"
      - id: o57a731
        text: "It applies only to object-oriented code"
    explanation: "“this distinction is also flawed, because boundaries between the essential and the accidental shift as our tooling evolves”."
  - id: abstraction-benefits
    prompt: "Why is a good abstraction valuable beyond hiding detail?"
    options:
      - id: oe3f020
        text: "It removes the need for testing"
      - id: o23efd5
        text: "It is reused across many applications, so quality improvements benefit all users"
        correct: true
      - id: o7c3cdf
        text: "It eliminates the underlying implementation"
      - id: od3ca6e
        text: "It makes code run faster in every case"
    explanation: "“Not only is this reuse more efficient than reimplementing a similar thing multiple times, but it also leads to higher-quality software, as quality improvements in the abstracted component benefit all applications that use it.”"
  - id: sql-hides
    prompt: "What does SQL hide, according to the chapter?"
    type: multiple
    options:
      - id: o60c12d
        text: "The need to specify which data you want"
      - id: o17cb23
        text: "On-disk and in-memory data structures"
        correct: true
      - id: o77e9bf
        text: "Concurrent requests from other clients"
        correct: true
      - id: o7fbbdf
        text: "Inconsistencies after crashes"
        correct: true
    explanation: "“SQL is an abstraction that hides complex on-disk and in-memory data structures, concurrent requests from other clients, and inconsistencies after crashes.”"
  - id: book-abstractions
    prompt: "Which kind of abstraction is this book about?"
    options:
      - id: o23d00f
        text: "Application-specific ones such as DDD aggregates"
      - id: o64e60a
        text: "General-purpose ones such as transactions, indexes and event logs"
        correct: true
      - id: odf6788
        text: "Programming language syntax"
      - id: o5e6508
        text: "UI component libraries"
    explanation: "“This book is not about such application-specific abstractions, but rather about general-purpose abstractions on top of which you can build your applications, such as database transactions, indexes, and event logs.”"
  - id: evolvability
    prompt: "What does *evolvability* mean in the book?"
    options:
      - id: o2ff137
        text: "Support for many programming languages"
      - id: o800695
        text: "Agility at the level of a data system: the ease of modifying it as requirements change"
        correct: true
      - id: od0b3e7
        text: "Backward compatibility of file formats only"
      - id: o206192
        text: "The ability to scale automatically"
    explanation: "The book uses “a different word to refer to agility on a data system level: evolvability”. It is linked to “simplicity and its abstractions”."
  - id: irreversibility
    prompt: "You are migrating databases. Which choice best reflects the chapter's advice?"
    options:
      - id: o2b21da
        text: "Avoid migrations entirely"
      - id: oacfece
        text: "Delete the old database immediately to avoid drift"
      - id: odec9a2
        text: "Keep the ability to switch back to the old system if problems arise"
        correct: true
      - id: oe9f93d
        text: "Migrate all services in one night"
    explanation: "“If you cannot switch back to the old system in case of problems with the new one, the stakes are much higher than if you can easily go back ... Minimizing irreversibility improves flexibility.”"
  - id: coupling
    prompt: "Which systems are usually easier to modify?"
    options:
      - id: o8b0780
        text: "Loosely coupled, simple systems"
        correct: true
      - id: o034914
        text: "Systems with no abstractions"
      - id: obf6610
        text: "Tightly coupled, complex systems"
      - id: oaf0db8
        text: "Systems with the most features"
    explanation: "“Loosely coupled, simple systems are usually easier to modify than tightly coupled, complex ones.”"
---
Most software cost is maintenance. The chapter splits maintainability into operability, simplicity and
evolvability.

## Learning objectives

- Explain why maintenance dominates software cost
- List ways a data system supports good operability
- Distinguish essential and accidental complexity and its limits
- Explain how irreversibility hurts evolvability

## In a nutshell

- Every valuable system becomes legacy; design for maintenance.
- Automation is two-edged: the leftover cases are the hardest.
- Abstraction is the best tool against complexity; SQL is a prime example.
- Evolvability is agility at the system level; minimize irreversible steps.

## Connections to today

- **Platform engineering** teams build "paved roads" — good defaults with escape hatches — the operability
  list made into a product.
- **AI-assisted coding** can grow a big ball of mud faster than ever; simplicity and good abstractions
  matter more, not less.
- **Blue/green and dual-write migrations** keep a way back to the old database — minimizing
  irreversibility.
