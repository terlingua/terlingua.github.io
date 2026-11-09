---
title: "Scalability"
questions:
  - id: startup-scale
    prompt: "A three-person startup with 200 users wants to adopt a sharded multi-region database \"to be ready\". What does the chapter advise?"
    options:
      - id: oa7fe25
        text: "Use a data lake instead"
      - id: o6e1fa7
        text: "Rewrite in a faster language"
      - id: o83078e
        text: "Adopt it now because migrations are always harder later"
      - id: o6a9e27
        text: "Keep the system simple and flexible; premature scalability work may be wasted or lock you into an inflexible design"
        correct: true
    explanation: "“In the best case, investments in scalability are wasted effort and premature optimization; in the worst case, they lock you into an inflexible design and make it harder to evolve your application.”"
  - id: not-label
    prompt: "Why is \"Y doesn't scale\" a meaningless statement?"
    options:
      - id: o7683e7
        text: "Scalability can only be measured in requests per second"
      - id: od06fc6
        text: "Scalability depends on how the system grows and what options exist for adding resources"
        correct: true
      - id: o88e8fe
        text: "It is meaningful only for databases"
      - id: o8d1e7a
        text: "Every system scales infinitely in the cloud"
    explanation: "“Scalability is not a one-dimensional label—it is meaningless to say “X is scalable” or “Y doesn’t scale.” Rather, discussing scalability means considering questions like these”."
  - id: load-params
    prompt: "Which could be load parameters per the chapter?"
    type: multiple
    options:
      - id: ofbee9f
        text: "Cache hit rate"
        correct: true
      - id: o107275
        text: "Ratio of reads to writes"
        correct: true
      - id: o148645
        text: "The programming language used"
      - id: od45bf2
        text: "Number of data items per user"
        correct: true
      - id: o92acce
        text: "Peak simultaneous online users"
        correct: true
    explanation: "“you may need to know the ratio of reads to writes in a database, the hit rate on a cache, or the number of data items per user” and “Sometimes you care about the peak of a variable quantity, such as the number of simultaneously online users”."
  - id: two-views
    prompt: "Which are the two ways the chapter suggests looking at increased load?"
    type: multiple
    options:
      - id: o6589b2
        text: "Ask how much resources must grow to keep performance unchanged"
        correct: true
      - id: o2e5004
        text: "Keep resources fixed and see how performance changes"
        correct: true
      - id: o28c263
        text: "Measure only average throughput"
      - id: o25c6ff
        text: "Reduce features until load fits"
    explanation: "“When you increase the load in a certain way and keep the system resources ... unchanged, how is the performance of your system affected?” and “how much do you need to increase the resources if you want to keep performance unchanged?”"
  - id: linear
    prompt: "Doubling resources lets you handle twice the load at the same performance. What is this called, and what is more typical?"
    options:
      - id: o53ae96
        text: "Superlinear scalability; it is the norm"
      - id: oeb3a35
        text: "Vertical scaling; it is the only option"
      - id: od9d328
        text: "Elasticity; it is guaranteed in the cloud"
      - id: o9f1f3e
        text: "Linear scalability; more typically cost grows faster than linearly"
        correct: true
    explanation: "“If doubling the resources will enable you to handle twice the load while keeping performance the same, we say that you have linear scalability ... Much more likely is that the cost grows faster than linearly.”"
  - id: vertical-cost
    prompt: "What are the drawbacks of vertical scaling (shared-memory)?"
    type: multiple
    options:
      - id: od62445
        text: "It cannot use multiple threads"
      - id: o64e62b
        text: "A machine with twice the resources usually costs much more than twice as much"
        correct: true
      - id: ofe591b
        text: "Bottlenecks mean it probably cannot handle twice the load"
        correct: true
      - id: od09a16
        text: "It requires sharding"
    explanation: "“a high-end machine with twice the hardware resources of a lower-spec machine typically costs significantly more than twice as much. And because of bottlenecks, that machine is unlikely to actually be able to handle twice the load.”"
  - id: shared-disk
    prompt: "What limits the scalability of shared-disk architectures?"
    options:
      - id: o4a5109
        text: "Lack of RAM"
      - id: o5517de
        text: "Lack of networking"
      - id: ofc0588
        text: "Contention and locking overhead"
        correct: true
      - id: od330b3
        text: "Inability to use SQL"
    explanation: "“contention and the overhead of locking limit the scalability of the shared-disk approach”."
  - id: shared-nothing-pros
    prompt: "Which are advantages of shared-nothing (scale-out)?"
    type: multiple
    options:
      - id: o9f1621
        text: "No need for sharding"
      - id: o10e1c8
        text: "Greater fault tolerance across datacenters and regions"
        correct: true
      - id: o59ef6b
        text: "Potential to scale linearly"
        correct: true
      - id: obebb84
        text: "Freedom to use the best price/performance hardware"
        correct: true
    explanation: "The advantages: “it has the potential to scale linearly, it can use whatever hardware offers the best price/performance ratio ... it can more easily adjust its hardware resources ... and it can achieve greater fault tolerance”. The downsides: “it requires explicit sharding”."
  - id: cloud-shared-storage
    prompt: "How do cloud native databases with shared storage services avoid the scalability problems of old shared-disk systems?"
    options:
      - id: o28ca8c
        text: "They forbid concurrent writes"
      - id: oa6b97e
        text: "The storage service offers a specialized API designed for the database, not a generic filesystem or block device"
        correct: true
      - id: ofeab71
        text: "They use a single compute node"
      - id: o4f98dd
        text: "They store data only in RAM"
    explanation: "“Instead of providing a filesystem (NAS) or block device (SAN) abstraction, the storage service offers a specialized API that is designed for the specific needs of the database”."
  - id: same-throughput
    prompt: "Two systems each move 100 MB/s: one handles 100,000 requests/s of 1 kB, the other 3 requests/min of 2 GB. What point does the chapter make?"
    options:
      - id: o5f972a
        text: "They can use identical architectures"
      - id: o3db7d5
        text: "They look very different despite equal throughput; there is no generic scalable architecture"
        correct: true
      - id: oe0b22c
        text: "The large-request system is always simpler"
      - id: o9ffd7a
        text: "Throughput is the only parameter that matters"
    explanation: "“There is no such thing as a generic, one-size-fits-all scalable architecture” and “a system designed to handle 100,000 requests per second, each 1 kB in size, looks very different from a system designed for 3 requests per minute, each 2 GB in size—even though the two systems have the same data throughput”."
  - id: order-magnitude
    prompt: "How far ahead should a fast-growing service plan for scale?"
    options:
      - id: o121884
        text: "Exactly 2× current load"
      - id: oc0f40c
        text: "Usually not more than one order of magnitude, expecting to rethink at each 10× increase"
        correct: true
      - id: o0408f5
        text: "Not at all"
      - id: of83f69
        text: "Five orders of magnitude, to avoid any rewrites"
    explanation: "“you will need to rethink your architecture on every order of magnitude load increase ... it is usually not worth planning future scaling needs more than one order of magnitude in advance.”"
  - id: principle-independent
    prompt: "Which general scalability principle underlies microservices, sharding, stream processing and shared-nothing?"
    options:
      - id: o1a512a
        text: "Use autoscaling everywhere"
      - id: oa9006e
        text: "Prefer synchronous communication"
      - id: oc947cc
        text: "Put everything in one database"
      - id: o95f766
        text: "Break the system into smaller components that can operate largely independently"
        correct: true
    explanation: "“A good general principle for scalability is to break a system into smaller components that can operate largely independently from one another.”"
  - id: autoscale
    prompt: "Your load is very predictable. What does the chapter say about autoscaling?"
    options:
      - id: o817ac2
        text: "Autoscaling is required for SLAs"
      - id: o59fff5
        text: "Autoscaling is always better"
      - id: o5515f7
        text: "Manual scaling is impossible in the cloud"
      - id: o7b29df
        text: "A manually scaled system may have fewer operational surprises"
        correct: true
    explanation: "“Autoscaling systems ... are cool, but if your load is fairly predictable, a manually scaled system may have fewer operational surprises”."
---
Scalability is about options for coping with growth, not a label. The section covers describing load,
the three classic architectures, and two principles.

## Learning objectives

- Explain why "X is scalable" is meaningless without a growth dimension
- Describe load with the right parameters
- Compare shared-memory, shared-disk and shared-nothing architectures
- Apply the "one order of magnitude" planning rule

## In a nutshell

- Startups should keep things simple; premature scaling can lock you into rigid designs.
- Two questions: same resources and more load → performance? More load → resources needed to keep performance?
- Linear scalability is good; cost usually grows faster than linearly.
- Shared-nothing scales out but needs sharding and distributed-systems complexity.
- Break systems into independent parts; do not over-complicate.

## Connections to today

- **Very large single instances** (hundreds of cores, terabytes of RAM) keep vertical scaling viable longer
  than many expect.
- **Disaggregated storage** in Aurora, AlloyDB and Neon is the "similar to shared-disk but without its
  scalability problems" model.
- **Kubernetes autoscaling** is powerful but, as the book notes, predictable load may be easier with manual
  scaling and fewer surprises.
