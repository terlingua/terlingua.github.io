---
title: "Reliability and Fault Tolerance"
questions:
  - id: fault-vs-failure
    prompt: "In a storage system with three replicas, one disk dies but users notice nothing. In the book's terms, what happened?"
    options:
      - id: o67e3f1
        text: "A failure occurred, but no fault"
      - id: oc94413
        text: "A Byzantine fault occurred"
      - id: o366f29
        text: "A single point of failure occurred"
      - id: oc79d21
        text: "A fault occurred, but no failure — the system still met its SLO"
        correct: true
    explanation: "“A fault occurs when a particular part of a system stops working correctly” whereas “A failure occurs when the system as a whole stops providing the required service to the user—in other words, when it does not meet the SLO.”"
  - id: same-thing
    prompt: "Why does the chapter say the fault/failure distinction can be confusing?"
    options:
      - id: od73b3b
        text: "Faults are always permanent"
      - id: o4d7901
        text: "Failures are always caused by humans"
      - id: oc8e025
        text: "Faults are hardware and failures are software"
      - id: o3b58e7
        text: "They are the same thing at different levels: a disk's failure is a fault from the larger system's view"
        correct: true
    explanation: "“The distinction between faults and failures can be confusing because they are the same thing, just at different levels.”"
  - id: spof
    prompt: "What is a single point of failure?"
    options:
      - id: o9829fa
        text: "A component replicated three times"
      - id: oee1f30
        text: "The busiest server in a cluster"
      - id: od1e0d5
        text: "A part whose fault escalates to failure of the whole system"
        correct: true
      - id: o1736c9
        text: "Any component that can crash"
    explanation: "“If a system cannot tolerate a certain part becoming faulty, we call that part a single point of failure (SPOF), because a fault in that part escalates to cause the failure of the whole system.”"
  - id: fanout-fault
    prompt: "For the timeline fan-out to be fault-tolerant when a worker crashes, what must the replacement achieve?"
    options:
      - id: o7e4cd7
        text: "Deliver every post without missing or duplicating any — exactly-once semantics"
        correct: true
      - id: ocae431
        text: "Restart from the beginning of the day"
      - id: o5d86c8
        text: "Deliver posts at most once, dropping some"
      - id: o24761c
        text: "Stop new posts until the worker returns"
    explanation: "“ensure that another machine can take over this task without missing any posts that should have been delivered, and without duplicating any posts. (This idea is known as exactly-once semantics”."
  - id: bounded
    prompt: "Why is fault tolerance always limited to certain numbers and types of faults?"
    options:
      - id: oe1475b
        text: "Tolerating any number of faults is impossible — if all nodes crash, nothing can be done"
        correct: true
      - id: oa703f3
        text: "Because only hardware faults can be tolerated"
      - id: ocfe869
        text: "Because faults are always independent"
      - id: o49b9de
        text: "Because regulators limit redundancy"
    explanation: "“It would not make sense to tolerate any number of faults; if all nodes crash, nothing can be done.”"
  - id: fault-injection
    prompt: "Why might a team deliberately kill processes at random in production?"
    options:
      - id: o9ced6e
        text: "To speed up deployments"
      - id: oec2aa5
        text: "To reduce cloud costs"
      - id: o99dde4
        text: "To test user interfaces"
      - id: o15ec58
        text: "To keep fault-tolerance machinery exercised, since many critical bugs come from poor error handling"
        correct: true
    explanation: "“Many critical bugs are actually due to poor error handling; by deliberately inducing faults, you ensure that the fault-tolerance machinery is continually exercised and tested”."
  - id: prevention
    prompt: "For which kind of fault does the chapter say prevention is better than cure?"
    options:
      - id: o98dac8
        text: "Security breaches, because leaked data cannot be un-leaked"
        correct: true
      - id: o210a53
        text: "Software bugs in general"
      - id: o67cfc8
        text: "Disk failures"
      - id: o05fdf8
        text: "Network partitions"
    explanation: "“This is the case with security matters, for example; if an attacker has compromised a system and gained access to sensitive data, that event cannot be undone.”"
  - id: disk-rate
    prompt: "With 10,000 magnetic disks and a 2–5% annual failure rate, roughly how often should you expect a disk to fail?"
    options:
      - id: o95b833
        text: "About one per month"
      - id: o9e02a0
        text: "About one per day"
        correct: true
      - id: obd9de9
        text: "About one per hour"
      - id: o712dbb
        text: "About one per year"
    explanation: "“Approximately 2%–5% of magnetic hard drives fail per year; in a storage cluster with 10,000 disks, we should therefore expect on average one disk failure per day.” 10,000 × 3.65% ≈ 365 per year."
  - id: ssd-errors
    prompt: "How do SSD error characteristics compare with magnetic disks according to the chapter?"
    options:
      - id: o1d2d7b
        text: "SSD errors only occur in old drives"
      - id: o5758b3
        text: "SSDs fail less often overall, but uncorrectable errors occur about once per year per drive — a higher error rate than HDDs"
        correct: true
      - id: o460c7c
        text: "SSDs never have uncorrectable errors"
      - id: o9e2ac9
        text: "SSDs fail at 10% per year"
    explanation: "“Approximately 0.5%–1% of solid state drives (SSDs) fail per year” but “uncorrectable errors occur approximately once per year per drive, even in drives that are fairly new ... This error rate is higher than that of magnetic hard drives”."
  - id: cpu-wrong
    prompt: "What makes defective CPU cores particularly dangerous?"
    options:
      - id: oe77620
        text: "They may silently return wrong results instead of crashing"
        correct: true
      - id: o2e8eab
        text: "They only affect GPUs"
      - id: oc378d5
        text: "They always crash immediately"
      - id: o1561ec
        text: "They are fixed by ECC memory"
    explanation: "“Approximately 1 in 1,000 machines has a CPU core that occasionally computes the wrong result ... in other cases it leads to a program simply returning the wrong result.”"
  - id: correlation
    prompt: "Why is hardware redundancy less effective than independence assumptions suggest?"
    options:
      - id: o547c19
        text: "RAID always loses data"
      - id: ocd32f1
        text: "Redundant parts are never tested"
      - id: of23d59
        text: "Diesel generators are unreliable"
      - id: oe24155
        text: "Component failures are significantly correlated, and whole racks or datacenters fail more often than we'd like"
        correct: true
    explanation: "“Redundancy is most effective when component faults are independent ... However, experience has shown significant correlations between component failures”."
  - id: cloud-focus
    prompt: "Why do cloud systems focus less on making individual machines reliable?"
    options:
      - id: o92249b
        text: "Cloud providers forbid redundancy"
      - id: oe0c8c8
        text: "Individual reliability is too cheap to matter"
      - id: o140964
        text: "They achieve availability by tolerating faulty nodes in software, across machines, racks and zones"
        correct: true
      - id: of16968
        text: "Cloud machines never fail"
    explanation: "“cloud systems tend to focus less on the reliability of individual machines and instead aim to make services highly available by tolerating faulty nodes at the software level.”"
  - id: az
    prompt: "What do availability zones tell you?"
    options:
      - id: o801c3d
        text: "Which regions have the cheapest prices"
      - id: oea9e08
        text: "Which customers share a tenant"
      - id: ofe0157
        text: "Which servers have the most CPU"
      - id: oa3f2c3
        text: "Which resources are physically co-located and so more likely to fail together"
        correct: true
    explanation: "“Cloud providers use availability zones to identify which resources are physically co-located; resources in the same place are more likely to fail at the same time”."
  - id: rolling-upgrade
    prompt: "What operational advantage does tolerating whole-machine loss bring?"
    options:
      - id: o540644
        text: "Unlimited throughput"
      - id: oc424e9
        text: "No need for monitoring"
      - id: oae058a
        text: "Rolling upgrades: patching one node at a time without downtime"
        correct: true
      - id: o8abd6b
        text: "No need for backups"
    explanation: "“a multi-node fault-tolerant system can be patched by restarting one node at a time, without affecting the service for users. This is called a rolling upgrade”."
  - id: software-correlated
    prompt: "Why do software faults tend to cause more failures than hardware faults?"
    options:
      - id: o29d8c4
        text: "Software faults are always permanent"
      - id: oa2efd0
        text: "Many nodes run the same software with the same bugs, so faults are highly correlated"
        correct: true
      - id: o0a85a2
        text: "Hardware faults are always caught by ECC"
      - id: oc3aeb0
        text: "Software runs slower than hardware"
    explanation: "“software faults are often very highly correlated, because it is common for many nodes to run the same software and thus have the same bugs ... they tend to cause many more system failures than uncorrelated hardware faults”."
  - id: leap-second
    prompt: "What do the 2012 leap-second bug and the SSD firmware failure at 32,768 hours have in common?"
    options:
      - id: o61c5b0
        text: "A dormant bug triggered by unusual circumstances hit every affected node at the same time"
        correct: true
      - id: oa34cf0
        text: "Both affected only one machine"
      - id: o462ff6
        text: "Both were caused by cosmic rays"
      - id: ocdcefe
        text: "Both were operator configuration mistakes"
    explanation: "The chapter gives both as examples of “A software bug that causes every node to fail at the same time in particular circumstances.”"
  - id: assumptions
    prompt: "What is often revealed when a dormant software bug is finally triggered?"
    options:
      - id: o12994c
        text: "The software assumed something about its environment that was usually but not always true"
        correct: true
      - id: o371dbf
        text: "The compiler was buggy"
      - id: o99ea26
        text: "Hardware was faulty"
      - id: od784bb
        text: "An attacker was present"
    explanation: "“it is revealed that the software is making some kind of assumption about its environment—and while that assumption is usually true, it eventually stops being true for some reason”."
  - id: cascading
    prompt: "A slow database causes an API tier to exhaust its threads, which makes the gateway time out and retry. What kind of software fault is this?"
    options:
      - id: o7d439b
        text: "A Byzantine fault"
      - id: o2fba43
        text: "A cascading failure"
        correct: true
      - id: of0b902
        text: "Jitter"
      - id: oe568eb
        text: "A hardware fault"
    explanation: "“Cascading failures, where a problem in one component causes another component to become overloaded and slow down, which in turn brings down another component”."
  - id: operators-cause
    prompt: "What did one study find was the leading cause of outages at large internet services?"
    options:
      - id: ob6ba63
        text: "Network hardware"
      - id: ob6880c
        text: "Disk failures"
      - id: o35c9df
        text: "Cosmic rays"
      - id: oe40f89
        text: "Configuration changes by operators"
        correct: true
    explanation: "“configuration changes by operators were the leading cause of outages, whereas hardware faults (servers or network) played a role in only 10%–25% of cases”."
  - id: human-error
    prompt: "What does the chapter say \"human error\" really is?"
    options:
      - id: o09a1f8
        text: "A symptom of a problem in the sociotechnical system, not the root cause"
        correct: true
      - id: o6dd39e
        text: "Impossible to reduce"
      - id: o12eb8e
        text: "Something that only affects junior engineers"
      - id: o34d2ee
        text: "The root cause of most incidents, fixable by stricter rules"
    explanation: "“What we call “human error” is not really the cause of an incident, but rather a symptom of a problem with the sociotechnical system in which people are trying their best to do their jobs”."
  - id: measures
    prompt: "Which technical measures reduce the impact of human mistakes?"
    type: multiple
    options:
      - id: o3a595d
        text: "Interfaces that encourage the right thing"
        correct: true
      - id: o96802c
        text: "Property testing on many random inputs"
        correct: true
      - id: o4d2ec0
        text: "Quick rollback of configuration changes"
        correct: true
      - id: o70e8aa
        text: "Firing whoever made the last deploy"
      - id: o9e8a08
        text: "Gradual rollouts of new code"
        correct: true
    explanation: "The chapter lists thorough testing including “property testing on lots of random inputs”, “rollback mechanisms for quickly reverting configuration changes, gradual rollouts of new code”, monitoring, observability and “well-designed interfaces”."
  - id: postmortem-answers
    prompt: "After an outage, which postmortem conclusion does the chapter consider unproductive?"
    type: multiple
    options:
      - id: o40e6ce
        text: "\"Bob should have been more careful\""
        correct: true
      - id: oc5c28e
        text: "\"We must rewrite the backend in Haskell\""
        correct: true
      - id: o8a9f4a
        text: "Learning how the sociotechnical system works from people who operate it"
    explanation: "““Bob should have been more careful when deploying that change” is not productive, but neither is “We must rewrite the backend in Haskell.””"
  - id: org-priorities
    prompt: "A preventable outage happens after leadership chose features over testing. Where does the chapter place the problem?"
    options:
      - id: o00791f
        text: "Nowhere — outages are inevitable"
      - id: od2e430
        text: "In the organization's priorities, not in the person who made the mistake"
        correct: true
      - id: o10746f
        text: "In the testing framework"
      - id: oebab5d
        text: "In the individual engineer"
    explanation: "“when a preventable mistake inevitably occurs, blaming the person who made the mistake does not make sense; the problem is the organization’s priorities.”"
  - id: loss-vs-outage
    prompt: "For a family photo app, which is more catastrophic per the chapter?"
    options:
      - id: o87f5ce
        text: "Permanent data loss or corruption"
        correct: true
      - id: o67a3e2
        text: "Slow thumbnails"
      - id: o819fb1
        text: "An outdated UI"
      - id: o78c16c
        text: "A two-hour outage"
    explanation: "“In many applications, a temporary outage of a few minutes or even a few hours is tolerable, but permanent data loss or corruption would be catastrophic.”"
  - id: horizon
    prompt: "What legal assumption contributed to the Post Office Horizon scandal?"
    options:
      - id: o6acf24
        text: "That data must be stored in the UK"
      - id: ocb0c2b
        text: "That accountants must audit all software"
      - id: o260475
        text: "That software vendors are liable for all bugs"
      - id: o2fc574
        text: "English law presumed computers operate correctly unless shown otherwise"
        correct: true
    explanation: "The miscarriage of justice stemmed from “an assumption by English law that computers operate correctly (and hence, evidence produced by computers is reliable) unless evidence exists to the contrary”."
  - id: cutting-corners
    prompt: "When is it acceptable to sacrifice reliability?"
    options:
      - id: o3010b1
        text: "Whenever the deadline is tight"
      - id: o267ac8
        text: "Only for internal tools"
      - id: oa2e81c
        text: "Never"
      - id: o335801
        text: "Sometimes, e.g., a prototype for an unproven market, as long as you are conscious of the trade-off"
        correct: true
    explanation: "“In some situations we may choose to sacrifice reliability in order to reduce development cost (e.g., when developing a prototype product for an unproven market)—but we should be very conscious of when we are cutting corners”."
---
Reliability means continuing to work correctly even when things go wrong. The chapter separates faults
(a part breaks) from failures (the whole system misses its SLO).

## Learning objectives

- Distinguish faults, failures and single points of failure
- Quote realistic hardware failure rates and explain why correlation matters
- Explain why software faults are harder than hardware faults
- Argue for blameless postmortems over "human error"

## In a nutshell

- Fault tolerance is always bounded: a certain number of certain kinds of faults.
- Fault injection and chaos engineering exercise recovery paths deliberately.
- Hardware faults are mostly independent; software faults are highly correlated.
- Operator configuration changes are a leading cause of outages; "human error" is a symptom.
- Temporary outages are often tolerable; permanent data loss is not.

## Connections to today

- **The July 2024 CrowdStrike outage** — one faulty update crashing millions of Windows machines at
  once — is a textbook correlated software fault.
- **Silent data corruption** from defective CPU cores is now publicly documented by hyperscalers,
  matching the chapter's "1 in 1,000 machines" figure.
- **Progressive delivery** (canaries, feature flags, automatic rollback) is the chapter's "gradual
  rollouts" and "rollback mechanisms" in practice.
