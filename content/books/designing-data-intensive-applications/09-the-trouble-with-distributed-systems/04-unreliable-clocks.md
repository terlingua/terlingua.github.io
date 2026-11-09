---
title: "Unreliable Clocks"
questions:
  - id: durations-vs-points
    prompt: "Which questions need a duration rather than a point in time?"
    type: multiple
    options:
      - id: oeeee5b
        text: "Has this request timed out yet?"
        correct: true
      - id: o45fbfb
        text: "When does this cache entry expire?"
      - id: o926761
        text: "When was this article published?"
      - id: oc091ab
        text: "What's the p99 response time?"
        correct: true
    explanation: "“Questions 1–4 measure durations ... whereas questions 5–8 describe points in time”. Timeouts and percentiles are durations; publication dates and expiry times are points in time."
  - id: tod-unsuitable
    prompt: "Why are time-of-day clocks unsuitable for measuring elapsed time?"
    options:
      - id: o8f46b2
        text: "They are not synchronized"
      - id: ocdbbe8
        text: "They count from boot time"
      - id: oea35ec
        text: "They have no resolution"
      - id: o75c5ea
        text: "NTP resets and leap seconds can make them jump backward or forward"
        correct: true
    explanation: "“if the local clock is too far ahead of the NTP server, it may be forcibly reset and appear to jump back ... These jumps, as well as similar jumps caused by leap seconds, make time-of-day clocks unsuitable for measuring elapsed time”."
  - id: monotonic-compare
    prompt: "Why is comparing monotonic clock values from two machines meaningless?"
    options:
      - id: obc7b08
        text: "They have millisecond resolution only"
      - id: o11f998
        text: "NTP resets them"
      - id: o824ac8
        text: "Monotonic clocks run backward"
      - id: o9d0375
        text: "The absolute value is arbitrary, e.g., nanoseconds since boot"
        correct: true
    explanation: "“the absolute value of the clock is meaningless ... In particular, it makes no sense to compare monotonic clock values from two computers, because they don’t mean the same thing.”"
  - id: slewing
    prompt: "How can NTP affect a monotonic clock?"
    options:
      - id: oca1e94
        text: "It can reset it to zero"
      - id: o1a646d
        text: "It can make it jump backward"
      - id: o0a8d61
        text: "It cannot affect it at all"
      - id: o493152
        text: "It can slew it — speed it up or slow it down by up to 0.05% — but not make it jump"
        correct: true
    explanation: "“NTP may adjust the frequency at which the monotonic clock moves forward (this is known as slewing the clock) ... but it cannot cause the monotonic clock to jump forward or backward.”"
  - id: drift-calc
    prompt: "Google assumes 200 ppm drift. How much can a clock drift if synchronized once a day?"
    options:
      - id: o4777f5
        text: "About 6 milliseconds"
      - id: o5b2862
        text: "About 17 seconds"
        correct: true
      - id: o521926
        text: "About 2 hours"
      - id: o188866
        text: "About 200 milliseconds"
    explanation: "200 ppm is “equivalent to a 6 ms drift for a clock that is resynchronized with a server every 30 seconds, or a 17-second drift for a clock that is resynchronized once a day.”"
  - id: clock-errors
    prompt: "Which are sources of clock inaccuracy?"
    type: multiple
    options:
      - id: o43fd96
        text: "Users deliberately setting device clocks wrong"
        correct: true
      - id: oa28da1
        text: "A node accidentally firewalled off from NTP"
        correct: true
      - id: o013987
        text: "NTP accuracy limited by network delay"
        correct: true
      - id: o3d24a4
        text: "VM pauses making the clock jump forward"
        correct: true
      - id: oa7718d
        text: "Clocks being perfectly accurate in VMs"
    explanation: "The chapter lists firewalling, NTP's dependence on network delay (“a minimum error of 35 ms is achievable when synchronizing over the internet”), VM pauses, misconfigured NTP servers, leap seconds, and untrusted devices."
  - id: smearing
    prompt: "What is leap-second smearing?"
    options:
      - id: o3f64ed
        text: "NTP servers spread the leap-second adjustment gradually over a day"
        correct: true
      - id: o78e731
        text: "Adding two leap seconds at once"
      - id: o58e7d5
        text: "Resetting all clocks to zero"
      - id: o0039c4
        text: "Ignoring leap seconds entirely"
    explanation: "“The best way of handling leap seconds may be to make NTP servers “lie,” by performing the leap second adjustment gradually over the course of a day (this is known as smearing)”."
  - id: mifid
    prompt: "How tightly does MiFID II require high-frequency trading clocks to be synchronized?"
    options:
      - id: o03e3ac
        text: "Within 1 second"
      - id: oa4439e
        text: "Within 100 microseconds of UTC"
        correct: true
      - id: oc9b506
        text: "Within 10 milliseconds"
      - id: oadebae
        text: "Within 1 minute"
    explanation: "“the MiFID II European regulation ... requires all high-frequency trading funds to synchronize their clocks to within 100 microseconds of UTC”."
  - id: silent-clock
    prompt: "Why is a bad clock more dangerous than a bad CPU?"
    options:
      - id: o418eb5
        text: "Most things keep working, so drift goes unnoticed and causes silent data loss rather than a crash"
        correct: true
      - id: oa5cbef
        text: "Bad clocks crash the machine"
      - id: o04285c
        text: "Clocks control the CPU"
      - id: ob658be
        text: "Bad clocks are detected by TCP"
    explanation: "“if its quartz clock is defective or its NTP client is misconfigured, most things will seem to work fine ... the result is more likely to be silent and subtle data loss than a dramatic crash”."
  - id: clock-monitor
    prompt: "If you rely on synchronized clocks, what should you do?"
    options:
      - id: o335aed
        text: "Disable NTP"
      - id: o2e2e4f
        text: "Trust the clocks"
      - id: o42c70c
        text: "Monitor clock offsets across the cluster and remove nodes that drift too far"
        correct: true
      - id: o34af11
        text: "Use only monotonic clocks for timestamps across nodes"
    explanation: "“it is essential that you carefully monitor the clock offsets between all the machines in your cluster. Any node whose clock drifts too far from the others should be declared dead and removed.”"
  - id: lww-figure
    prompt: "In Figure 9-3, client B's increment gets timestamp 42.003 after A's write at 42.004. With LWW, what happens?"
    options:
      - id: o7923b7
        text: "Node 2 keeps x = 2"
      - id: oa222b1
        text: "Both values are kept as siblings"
      - id: ob6e40c
        text: "The write is rejected with an error"
      - id: o092f38
        text: "Node 2 keeps x = 1 and silently drops the causally later increment"
        correct: true
    explanation: "“when node 2 receives these two events, it will incorrectly conclude that x = 1 is the more recent value and drop the write x = 2, so the increment is lost.”"
  - id: lww-problems
    prompt: "Which are problems with client-clock LWW in Cassandra-style systems?"
    type: multiple
    options:
      - id: oa8ec2e
        text: "Writes can mysteriously disappear when a node's clock lags"
        correct: true
      - id: o7fa7a9
        text: "It requires a leader"
      - id: o5f6397
        text: "Equal timestamps need a tiebreaker, which can violate causality"
        correct: true
      - id: o705aaf
        text: "It can't distinguish sequential writes from concurrent ones"
        correct: true
    explanation: "The chapter lists all three problems: writes that “mysteriously disappear”, LWW “cannot distinguish between writes that occurred sequentially in quick succession ... and writes that were truly concurrent”, and same-timestamp ties."
  - id: ntp-not-enough
    prompt: "Why can't NTP be made accurate enough to order events correctly?"
    options:
      - id: oe86344
        text: "NTP is too slow to run"
      - id: o52a4a4
        text: "NTP only works with GPS"
      - id: oe0bd3e
        text: "NTP adds random offsets deliberately"
      - id: o1c529d
        text: "Its accuracy is limited by network round-trip time; you'd need clock error well below network delay"
        correct: true
    explanation: "“NTP’s synchronization accuracy is itself limited by the network round-trip time ... To guarantee a correct ordering, you would need the clock error to be significantly lower than the network delay, which is not possible.”"
  - id: logical-clocks
    prompt: "What do logical clocks measure?"
    options:
      - id: o6252d2
        text: "Only the relative ordering of events, using incrementing counters"
        correct: true
      - id: oedca8a
        text: "Time of day"
      - id: oe68f6b
        text: "Elapsed seconds"
      - id: of6ea07
        text: "CPU cycles"
    explanation: "“Logical clocks do not measure the time of day or the number of seconds elapsed, only the relative ordering of events”."
  - id: confidence
    prompt: "Why should a clock reading be thought of as an interval?"
    options:
      - id: oa25060
        text: "Intervals are faster to compare"
      - id: o55abe5
        text: "Its accuracy is limited by drift and sync error; microsecond digits may be meaningless"
        correct: true
      - id: o8ecb34
        text: "Clocks report ranges by default"
      - id: ofcecb0
        text: "Because of DST"
    explanation: "“it doesn’t make sense to think of a clock reading as a point in time. It is more like a range of times, within a confidence interval”."
  - id: truetime
    prompt: "What does Spanner's TrueTime API return?"
    options:
      - id: of105d1
        text: "A logical counter"
      - id: o277a7e
        text: "The time zone offset"
      - id: o10f76c
        text: "An interval [earliest, latest] within which the true time lies"
        correct: true
      - id: oc90ad6
        text: "A single nanosecond timestamp"
    explanation: "“When you ask it for the current time, you get back two values: [earliest, latest], which are the earliest possible and the latest possible timestamp.”"
  - id: commit-wait
    prompt: "Why does Spanner wait for the length of the confidence interval before committing?"
    options:
      - id: o662003
        text: "To batch writes"
      - id: o462639
        text: "So later transactions' intervals don't overlap with this one, making timestamp order reflect causality"
        correct: true
      - id: o924780
        text: "To save power"
      - id: of2dbe6
        text: "To let replicas catch up"
    explanation: "“Spanner deliberately waits for the length of the confidence interval before committing a read/write transaction. By doing so, it ensures that any transaction that may read the data is at a sufficiently later time that their confidence intervals do not overlap.”"
  - id: atomic-clocks
    prompt: "Are atomic clocks essential to Spanner's approach?"
    options:
      - id: o8057f3
        text: "No — the key is having a confidence interval; accurate sources just keep it small"
        correct: true
      - id: o4a4f76
        text: "No — Spanner doesn't use clocks"
      - id: ofb4ba7
        text: "Yes — for durability"
      - id: oec37b0
        text: "Yes — it cannot work without them"
    explanation: "“The atomic clocks and GPS receivers are not strictly necessary in Spanner. The important thing is to have a confidence interval—accurate clock sources only help keep that interval small.”"
  - id: lease-bug
    prompt: "What's wrong with the lease-checking loop in the chapter?"
    options:
      - id: oee2747
        text: "It uses a monotonic clock"
      - id: o9807f5
        text: "It never processes requests"
      - id: o5b7a12
        text: "It renews the lease too often"
      - id: o9b5e94
        text: "It compares a remote expiry with the local clock, and a pause between checking and processing can let the lease expire unnoticed"
        correct: true
    explanation: "“it’s relying on synchronized clocks” and “what if an unexpected pause occurs in the execution of the program? ... the lease will likely have expired by the time the request is processed, and another node will already have taken over as leader.”"
  - id: pause-causes
    prompt: "Which can pause a thread for a long time?"
    type: multiple
    options:
      - id: ob53177
        text: "A CPU cache hit"
      - id: o818dc9
        text: "Stop-the-world garbage collection"
        correct: true
      - id: ocaa092
        text: "SIGSTOP (e.g., Ctrl-Z)"
        correct: true
      - id: o48624e
        text: "VM suspension or live migration"
        correct: true
      - id: o928008
        text: "Swapping (page faults to disk)"
        correct: true
    explanation: "All four are listed, along with lock contention, laptop lids closing, context switches and steal time, and synchronous disk I/O: “All these occurrences can preempt the running thread at any point and resume it at a later time, without the thread even noticing.”"
  - id: steal-time
    prompt: "What is \"steal time\"?"
    options:
      - id: o1b801e
        text: "CPU time spent running other VMs while your VM's thread is paused"
        correct: true
      - id: o6d7dcd
        text: "Time lost to clock drift"
      - id: o6db368
        text: "Time waiting on locks"
      - id: odb9125
        text: "Time spent in GC"
    explanation: "“In the case of a VM, the CPU time spent in other VMs is known as steal time.”"
  - id: no-shared-memory
    prompt: "Why don't mutexes and semaphores translate to distributed systems?"
    options:
      - id: o010582
        text: "There is no shared memory — only messages over an unreliable network"
        correct: true
      - id: occ814f
        text: "They are too slow"
      - id: o303c84
        text: "They are patented"
      - id: of0f5a1
        text: "They require Java"
    explanation: "“these tools don’t directly translate to distributed systems, because a distributed system has no shared memory—only messages sent over an unreliable network.”"
  - id: real-time
    prompt: "What does \"real-time\" mean in embedded systems versus on the web?"
    options:
      - id: o8d52f0
        text: "Embedded: fast; web: guaranteed"
      - id: of51fe0
        text: "Neither has a meaning"
      - id: of6abbc
        text: "They mean the same thing"
      - id: o888ef4
        text: "Embedded: guaranteed deadlines in all circumstances; web: pushing data without hard constraints"
        correct: true
    explanation: "“In embedded systems, real-time means that a system is carefully designed and tested to meet specified timing guarantees in all circumstances. This meaning is in contrast to the more vague use of the term real-time on the web”."
  - id: rt-cost
    prompt: "Why are hard real-time guarantees rarely used in server-side data systems?"
    options:
      - id: o538bad
        text: "They are impossible"
      - id: oae8fc9
        text: "They require support at every level and restrict tools, making them very expensive; and real-time isn't high-performance"
        correct: true
      - id: od44d71
        text: "They require GPUs"
      - id: o32dca5
        text: "They are illegal"
    explanation: "“developing real-time systems is very expensive ... “real-time” is not the same as “high-performance” ... For most server-side data processing systems, real-time guarantees are simply not economical or appropriate.”"
  - id: gc-mitigate
    prompt: "Which techniques reduce the impact of GC pauses?"
    type: multiple
    options:
      - id: odee5f4
        text: "Use languages without GC, such as Rust or Swift"
        correct: true
      - id: o04fde7
        text: "Increase the heap without bound"
      - id: o2a28b3
        text: "Use GC only for short-lived objects and restart processes periodically"
        correct: true
      - id: o76d9b6
        text: "Treat an imminent GC as a planned outage and drain traffic from the node"
        correct: true
    explanation: "The chapter describes draining requests before GC, restarting processes “before they accumulate enough long-lived objects”, and languages like Swift (reference counting) and Rust (lifetimes)."
---
Time-of-day clocks jump; monotonic clocks don't but can't be compared across machines. Synchronization
is imperfect, and relying on it is dangerous. Processes can also pause at any moment.

## Learning objectives

- Distinguish time-of-day and monotonic clocks and when to use each
- List sources of clock error and their magnitudes
- Explain why LWW with wall clocks loses data and why logical clocks are safer
- Explain confidence intervals, TrueTime and Spanner's commit wait
- Identify causes of process pauses and why leases need care

## In a nutshell

- Durations: monotonic clock. Points in time: time-of-day clock (synchronized, but may jump).
- 200 ppm drift ≈ 17 s/day; internet NTP ≈ 35 ms at best, spikes to seconds.
- Faulty clocks fail silently — monitor offsets and evict outliers.
- A clock reading is an interval; Spanner waits out the uncertainty.
- GC, VM suspension, steal time, paging, SIGSTOP and laptop lids can pause a thread for seconds.

## Connections to today

- **AWS ClockBound and Azure/Google precision time services** expose bounded clock error to customers,
  bringing TrueTime-like ideas to everyone.
- **The end of leap seconds** — scheduled to stop by 2035 — will remove one classic clock bug source.
- **.NET's `Stopwatch` and `TimeProvider`** separate monotonic elapsed time from wall-clock time, and
  `TimeProvider` makes time mockable for tests.
