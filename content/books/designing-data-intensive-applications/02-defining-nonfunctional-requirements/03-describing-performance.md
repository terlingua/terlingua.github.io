---
title: "Describing Performance"
questions:
  - id: queueing-curve
    prompt: "Why does response time increase sharply as throughput nears the hardware's maximum?"
    options:
      - id: o643056
        text: "Networks drop all packets above 50% load"
      - id: o7d2c22
        text: "Requests wait for earlier ones to finish; queueing delays grow sharply near capacity"
        correct: true
      - id: oa82e1c
        text: "CPUs slow down when busy"
      - id: of61cc5
        text: "Databases switch to a slower algorithm under load"
    explanation: "“This is because of queueing: when a request arrives on a highly loaded system, the CPU is likely already in the process of handling an earlier request ... As throughput approaches the maximum that the hardware can handle, queueing delays increase sharply.”"
  - id: metastable
    prompt: "A service briefly overloaded keeps failing even after traffic returns to normal, until it is restarted. What is this called and what usually drives it?"
    options:
      - id: oc0aa84
        text: "Head-of-line blocking in TCP"
      - id: o2a6ad3
        text: "A cascading failure caused by disk faults"
      - id: o29ac36
        text: "A metastable failure, often driven by clients timing out and retrying (a retry storm)"
        correct: true
      - id: o408d25
        text: "A Byzantine fault"
    explanation: "“Even when the load is reduced again, such a system may remain in an overloaded state until it is rebooted or otherwise reset. This phenomenon is called a metastable failure”. The example cause: “clients time out and resend their requests ... a retry storm.”"
  - id: retry-mitigations
    prompt: "Which techniques does the chapter list for preventing retries from overloading a service?"
    type: multiple
    options:
      - id: o597431
        text: "Exponential backoff with randomized delays on the client"
        correct: true
      - id: o409ffd
        text: "Circuit breakers or token buckets that stop requests to a failing service"
        correct: true
      - id: of11438
        text: "Backpressure telling clients to slow down"
        correct: true
      - id: ocfef3c
        text: "Load shedding on the server"
        correct: true
      - id: of94db5
        text: "Retrying immediately in a tight loop"
    explanation: "The sidebar lists “exponential backoff”, “a circuit breaker ... or token bucket algorithm”, “load shedding” and “backpressure”."
  - id: what-users-care
    prompt: "Which metric do users usually care about most, and which determines cost?"
    options:
      - id: o4f8cdc
        text: "Users care about throughput; response time determines cost"
      - id: oa2df20
        text: "Users care about response time; throughput determines required resources and cost"
        correct: true
      - id: o6b66c4
        text: "Users care only about availability"
      - id: oceffe4
        text: "Both are equally irrelevant to users"
    explanation: "“the response time is usually what users care about the most, whereas the throughput determines the required computing resources (e.g., how many servers you need) and hence the cost”."
  - id: scalable-def
    prompt: "In this chapter, when is a system said to be scalable?"
    options:
      - id: of7baad
        text: "When response time is constant regardless of resources"
      - id: o53a637
        text: "When it runs in the cloud"
      - id: o65dc7a
        text: "When its maximum throughput can be significantly increased by adding computing resources"
        correct: true
      - id: o070535
        text: "When it never exceeds 50% CPU"
    explanation: "“a system is said to be scalable if its maximum throughput can be significantly increased by adding computing resources.”"
  - id: terms
    prompt: "A request spends 20 ms in transit, waits 30 ms for a CPU, and is processed for 10 ms. In the book's terms, which is the service time?"
    options:
      - id: o607518
        text: "50 ms"
      - id: o07713a
        text: "60 ms"
      - id: o1940f1
        text: "10 ms"
        correct: true
      - id: of631b0
        text: "30 ms"
    explanation: "“The service time is the duration for which the service is actively processing the client’s request.” Waiting for a CPU is queueing delay, and transit is network latency; all of them together make up the response time."
  - id: latency-meaning
    prompt: "How does the book define *latency*?"
    options:
      - id: odde1bb
        text: "A catchall for time during which a request is not being actively processed"
        correct: true
      - id: o32b2ce
        text: "The CPU time spent on a request"
      - id: o96f364
        text: "The number of requests per second"
      - id: oa0ead3
        text: "The total time the client waits"
    explanation: "“Latency is a catchall term for time during which a request is not being actively processed—that is, during which it is latent.”"
  - id: client-side
    prompt: "Why should response times be measured on the client side?"
    options:
      - id: o8527eb
        text: "Queueing delay is not part of service time, so server-side measurement misses it"
        correct: true
      - id: o6fa0ac
        text: "Client measurements are always lower"
      - id: obffb2a
        text: "Clients have more accurate clocks"
      - id: o1e267b
        text: "Servers cannot log timings"
    explanation: "“The queueing delay is not part of the service time, and for this reason it is important to measure response times on the client side.”"
  - id: hol
    prompt: "A server with 4 cores receives four slow 5-second requests followed by many 1 ms requests. What do clients of the fast requests experience, and what is the effect called?"
    options:
      - id: o4405ed
        text: "Errors, because the server is full"
      - id: o11b27e
        text: "Jitter caused by the network"
      - id: o4afabb
        text: "Fast responses, because their service time is 1 ms"
      - id: of9bcb4
        text: "Slow responses because they wait behind the slow requests — head-of-line blocking"
        correct: true
    explanation: "“it takes only a small number of slow requests to hold up the processing of subsequent requests—an effect known as head-of-line blocking. Even if those subsequent requests have fast service times, the client will see a slow overall response time”."
  - id: random-delays
    prompt: "Which can add random delay to the same repeated request?"
    type: multiple
    options:
      - id: o261baa
        text: "Packet loss and TCP retransmission"
        correct: true
      - id: oc234ed
        text: "A garbage collection pause"
        correct: true
      - id: oc3f170
        text: "Mechanical vibration in the server rack"
        correct: true
      - id: o49b66c
        text: "The request's URL length"
      - id: o705a6e
        text: "A page fault forcing a disk read"
        correct: true
    explanation: "“Many factors can add random delays—for example, a context switch to a background process, the loss of a network packet and TCP retransmission, a garbage collection pause, a page fault forcing a read from disk, or mechanical vibrations in the server rack”."
  - id: mean-use
    prompt: "What is the mean response time actually useful for?"
    options:
      - id: o775791
        text: "Measuring outliers"
      - id: ofa4b9d
        text: "Knowing the typical user's wait"
      - id: o5a6a94
        text: "Estimating throughput limits"
        correct: true
      - id: o5ebb78
        text: "Setting SLA refunds"
    explanation: "“The mean response time is useful for estimating throughput limits. However, the mean is not a very good metric if you want to know your “typical” response time, because it doesn’t tell you how many users actually experienced that delay.”"
  - id: p95-interpret
    prompt: "If the 95th percentile is 1.5 s, which statement is correct?"
    options:
      - id: o03332d
        text: "95 out of 100 requests take 1.5 s or more"
      - id: o7ad621
        text: "The average is 1.5 s"
      - id: o1db42f
        text: "The slowest request took 1.5 s"
      - id: o5bb045
        text: "5 out of 100 requests take 1.5 s or more"
        correct: true
    explanation: "“if the 95th percentile response time is 1.5 seconds, that means 95 out of 100 requests take less than 1.5 seconds, and 5 out of 100 requests take 1.5 seconds or more.”"
  - id: amazon-p999
    prompt: "Why did Amazon set internal targets at p99.9 even though it affects 1 in 1,000 requests?"
    options:
      - id: o379868
        text: "Because p50 is impossible to measure"
      - id: o5f326a
        text: "Because p99.9 is cheapest to optimize"
      - id: o6d37cc
        text: "Because regulators require it"
      - id: o3752f4
        text: "The slowest requests often belong to the customers with the most data — the most valuable customers"
        correct: true
    explanation: "“the customers with the slowest requests are often those who have the most data on their accounts, as … they have made many purchases—that is, they’re the most valuable customers”."
  - id: p9999
    prompt: "Why didn't Amazon optimize the 99.99th percentile?"
    options:
      - id: o579723
        text: "It is mathematically undefined"
      - id: o88201c
        text: "No customers experience it"
      - id: o391c61
        text: "It was deemed too expensive for too little benefit; very high percentiles are dominated by random events outside your control"
        correct: true
      - id: o25c4cf
        text: "It is always equal to p99.9"
    explanation: "“Optimizing the 99.99th percentile ... was deemed too expensive and found to not yield enough benefit for Amazon’s purposes. Reducing response times at very high percentiles is difficult because they are easily affected by random events outside of your control, and the benefits are diminishing.”"
  - id: user-impact-studies
    prompt: "What does the chapter conclude about widely cited statistics on latency and revenue?"
    options:
      - id: o78e6a6
        text: "They consistently show 1% revenue loss per 100 ms"
      - id: o543627
        text: "They prove latency has no effect on users"
      - id: o295107
        text: "Only Google's 2006 figure is reliable"
      - id: o0e3d88
        text: "Reliable data is surprisingly hard to find; some often-cited figures are unreliable or confounded"
        correct: true
    explanation: "“it is surprisingly difficult to get hold of reliable data to quantify the effect that latency has on user behavior. Some often-cited statistics are unreliable.”"
  - id: akamai-confound
    prompt: "Why are the Akamai study's conclusions probably not meaningful?"
    options:
      - id: ofcc907
        text: "It used too few websites"
      - id: o30892f
        text: "It only studied mobile users"
      - id: o2fa733
        text: "Very fast pages were often content-free (e.g., 404s), and the study did not separate content effects from load time"
        correct: true
      - id: of67944
        text: "It measured server CPU, not latency"
    explanation: "“the pages that load fastest are often those that have no useful content (e.g., 404 error pages). However, since the study makes no effort to separate the effects of page content from the effects of load time, its results are probably not meaningful.”"
  - id: tail-amplification
    prompt: "An end-user request fans out in parallel to 50 backends. Why does it see slow responses more often than any single backend does?"
    options:
      - id: oa5c4a7
        text: "Parallel calls are slower than sequential ones"
      - id: o1a54d3
        text: "It must wait for the slowest call, so the chance that at least one call is slow grows with the number of calls"
        correct: true
      - id: odcd74a
        text: "The client's CPU becomes the bottleneck"
      - id: o741657
        text: "Each backend slows down when called in parallel"
    explanation: "“Even if you make the calls in parallel, the request still needs to wait for the slowest of the parallel calls to complete.” This is “tail latency amplification”."
  - id: slo-sla
    prompt: "What distinguishes an SLA from an SLO?"
    options:
      - id: od75924
        text: "An SLA concerns only throughput"
      - id: o80051d
        text: "An SLA is a contract specifying what happens if the SLO is not met"
        correct: true
      - id: o897cca
        text: "An SLO is a contract; an SLA is an internal target"
      - id: ofc65d3
        text: "They are synonyms"
    explanation: "“An SLA is a contract that specifies what happens if the SLO is not met (e.g., customers may be entitled to a refund).”"
  - id: averaging-percentiles
    prompt: "A dashboard combines p99 values from 20 machines by averaging them. What is wrong?"
    options:
      - id: of1176c
        text: "It should use the median of p99s"
      - id: o69795a
        text: "Nothing, as long as all machines have similar load"
      - id: o57bc6c
        text: "Averaging percentiles is mathematically meaningless; add the histograms instead"
        correct: true
      - id: oa5abda
        text: "It should use the maximum p99, which is exact"
    explanation: "“Beware that averaging percentiles (e.g., to reduce the time resolution or to combine data from several machines) is mathematically meaningless. The right way of aggregating response time data is to add the histograms”."
  - id: estimators
    prompt: "Which are open source libraries for approximating percentiles cheaply?"
    type: multiple
    options:
      - id: o8a821f
        text: "t-digest"
        correct: true
      - id: ofbbbec
        text: "DDSketch"
        correct: true
      - id: o351985
        text: "HdrHistogram"
        correct: true
      - id: o340f70
        text: "Bloom filter"
    explanation: "“Open source percentile estimation libraries include HdrHistogram, t-digest, OpenHistogram, and DDSketch”."
  - id: jitter
    prompt: "What is *jitter*?"
    options:
      - id: o01937e
        text: "Packet duplication"
      - id: od96ee8
        text: "A garbage collection pause"
      - id: oe7e749
        text: "The median response time"
      - id: o384726
        text: "Variation in network delay"
        correct: true
    explanation: "“Variation in network delay is also known as jitter.”"
---
Response time versus throughput, why queues dominate variability, and why percentiles beat averages.

## Learning objectives

- Distinguish response time, service time, queueing delay and latency
- Explain metastable failure and the techniques that prevent retry storms
- Use percentiles, understand tail latency amplification, and aggregate them correctly
- Distinguish SLOs from SLAs

## In a nutshell

- Response time rises sharply as throughput approaches capacity, because of queueing.
- Measure on the client side; queueing is not part of service time.
- The mean is for throughput estimation; the median for typical experience; p99/p999 for tails.
- Fan-out to many backends amplifies tail latency.
- Never average percentiles; add histograms.

## Connections to today

- **LLM APIs** are a vivid example: time-to-first-token and tokens-per-second are response-time and
  throughput metrics, and rate limits plus exponential backoff are standard client practice.
- **Prometheus histograms and OpenTelemetry exponential histograms** let you merge distributions across
  instances — the "add the histograms" advice.
- **Metastable failures** have caused major cloud incidents; adaptive retry budgets in modern SDKs exist
  to stop retry storms.
