---
title: "Distributed Versus Single-Node Systems"
questions:
  - id: node-def
    prompt: "In the book's terminology, what is a *node*?"
    options:
      - id: oec8de8
        text: "Each process participating in a distributed system"
        correct: true
      - id: ofa1b4b
        text: "A JavaScript runtime"
      - id: o0e5dc6
        text: "A physical rack in a datacenter"
      - id: od3f187
        text: "A row in a graph database only"
    explanation: "“A system that involves several machines communicating via a network is called a distributed system. Each of the processes participating in a distributed system is called a node.”"
  - id: inherent-distribution
    prompt: "A collaborative whiteboard app has users on different phones editing together. Which reason for distribution applies even if the backend were a single server?"
    options:
      - id: o883e1d
        text: "Sustainability"
      - id: o44f4d3
        text: "Inherent distribution: interacting users on their own devices must communicate over a network"
        correct: true
      - id: o7ae0f5
        text: "Legal compliance"
      - id: o5ee9b0
        text: "Specialized hardware"
    explanation: "“If an application involves two or more interacting users, each using their own device, then the system is unavoidably distributed”."
  - id: reasons-match
    prompt: "Match: \"data about people in a jurisdiction must be stored and processed within that country\"."
    options:
      - id: ob43eec
        text: "Elasticity"
      - id: o5fc357
        text: "Legal compliance (data residency)"
        correct: true
      - id: o17417a
        text: "Latency"
      - id: oc76555
        text: "Fault tolerance"
    explanation: "“Some countries have data residency laws that require data about people in their jurisdiction to be stored and processed geographically within that country”."
  - id: elasticity-single
    prompt: "Why is elasticity harder on a single machine?"
    options:
      - id: ofd89a2
        text: "Single machines are always faster"
      - id: o225ea9
        text: "A single machine cannot be monitored"
      - id: oea3495
        text: "The machine must be provisioned for peak load even when it is barely used"
        correct: true
      - id: oc3e9b7
        text: "Single machines cannot run databases"
    explanation: "A single machine “needs to be provisioned to handle the maximum load, even at times when it is barely used.”"
  - id: ambiguous-timeout
    prompt: "A request to a payment service times out. Why is blindly retrying it risky?"
    options:
      - id: o68ee52
        text: "The network guarantees exactly-once delivery"
      - id: o60114e
        text: "Timeouts always mean the request was never sent"
      - id: oaaeea2
        text: "You do not know whether the service received and processed the request, so a retry might charge twice"
        correct: true
      - id: o7ce720
        text: "Retrying is forbidden by HTTP"
    explanation: "“any request may time out without receiving a response. In this case, we don’t know whether the service received the request, and simply retrying it might not be safe.”"
  - id: compute-to-data
    prompt: "When processing large data volumes, why can it be faster to move computation to the data?"
    options:
      - id: o309e6f
        text: "Transferring large data over the network to a separate machine is slower than running code where the data already is"
        correct: true
      - id: o2f4df4
        text: "Computation is always cheaper on storage nodes"
      - id: oa3142b
        text: "CPUs are only available on storage nodes"
      - id: oc9b0da
        text: "Networks cannot carry large data"
    explanation: "“When operating on large volumes of data, rather than transferring the data from storage to a separate machine that processes it, it can be faster to bring the computation to the machine that already has the data”."
  - id: more-nodes
    prompt: "A team plans a 100-core cluster for a 50 GB dataset. What caution does the chapter offer?"
    options:
      - id: o30874b
        text: "Clusters are always faster for any dataset"
      - id: o79d767
        text: "A simple single-threaded program on one computer can sometimes outperform a cluster with over 100 cores"
        correct: true
      - id: o2e9ccb
        text: "Distributed systems are always cheaper"
      - id: o02ab8d
        text: "Clusters cannot process 50 GB"
    explanation: "“More nodes are not always faster; in some cases, a simple single-threaded program on one computer can perform significantly better than a cluster with over 100 CPU cores”."
  - id: observability
    prompt: "What does *observability* involve, per the chapter?"
    options:
      - id: o712499
        text: "Collecting execution data and allowing it to be queried for both high-level metrics and individual events"
        correct: true
      - id: o9dd2e9
        text: "Only CPU and memory dashboards"
      - id: o7c1ae2
        text: "Writing logs to local disk"
      - id: oc6e5ab
        text: "Running load tests before release"
    explanation: "Observability “involves collecting data about the execution of a system and allowing that data to be queried in ways that allow both high-level metrics and individual events to be analyzed.”"
  - id: microservice-consistency
    prompt: "Two microservices each own a database. Keeping their data consistent becomes whose problem, and why are distributed transactions rarely the answer?"
    options:
      - id: o36048c
        text: "The load balancer's"
      - id: o753117
        text: "Nobody's; microservices are always consistent"
      - id: od01ae2
        text: "The database vendor's; they coordinate automatically"
      - id: o95b2ed
        text: "The application's; distributed transactions run counter to service independence and many databases do not support them"
        correct: true
    explanation: "“when each service has its own database, maintaining consistency of data across those different services becomes the application’s problem.” Distributed transactions “are rarely used in a microservices context because they run counter to the goal of making services independent from each other, and many databases don’t support them”."
  - id: single-node-default
    prompt: "What does the chapter conclude about single-machine processing?"
    options:
      - id: o33f7cd
        text: "It is only for prototypes"
      - id: o261b5b
        text: "It is often much simpler and cheaper, and modern hardware plus engines such as DuckDB and SQLite handle many workloads"
        correct: true
      - id: o2fe48f
        text: "It is obsolete in the cloud era"
      - id: oca92c6
        text: "It cannot provide durability"
    explanation: "“performing a task on a single machine is often much simpler and cheaper than setting up a distributed system” and “When combined with single-node databases such as DuckDB, SQLite, and KùzuDB, many workloads can now run on a single node.”"
  - id: shared-db
    prompt: "Why do microservices typically avoid sharing a database?"
    type: multiple
    options:
      - id: of5f77c
        text: "One service's queries could hurt other services' performance"
        correct: true
      - id: o9685d8
        text: "The database structure would effectively become part of each service's API and be hard to change"
        correct: true
      - id: o17902e
        text: "Sharing a database is illegal under GDPR"
      - id: o37b6c8
        text: "Databases cannot accept connections from more than one service"
    explanation: "“Sharing a database would effectively make the entire database structure a part of the service’s API, and then that structure would be difficult to change. Shared databases could also cause one service’s queries to negatively impact the performance of other services.”"
  - id: people-problem
    prompt: "A five-person startup is considering splitting its app into twelve microservices. What does the chapter suggest?"
    options:
      - id: od0c997
        text: "Microservices remove all operational overhead"
      - id: oa6484f
        text: "Microservices mainly solve a people problem of large organizations; a small company should keep it simple"
        correct: true
      - id: oefaac4
        text: "Twelve is the right number for any team"
      - id: o1dacf6
        text: "Microservices are mandatory for cloud deployment"
    explanation: "“Microservices are primarily a technical solution to a people problem ... in a small company with fewer teams, using microservices is likely to be unnecessary overhead, and implementing the application in the simplest way possible is preferable”."
  - id: microservice-costs
    prompt: "Which are costs of having many services, according to the chapter?"
    type: multiple
    options:
      - id: o0ddf0c
        text: "API changes can break clients, often discovered late"
        correct: true
      - id: o3d07f1
        text: "Each service needs deployment, scaling, logging, monitoring and alerting infrastructure"
        correct: true
      - id: o4b247e
        text: "Testing needs the dependent services running too"
        correct: true
      - id: ob17d35
        text: "Each service must use a different programming language"
    explanation: "The chapter notes “Testing a service during development can be complicated”, that “each service requires infrastructure for deploying new releases ... and alerting an on-call engineer”, and that “microservice APIs can be challenging to evolve”."
  - id: serverless-billing
    prompt: "What analogy does the chapter draw for serverless?"
    options:
      - id: o5ffba9
        text: "Serverless is like a supercomputer for web apps"
      - id: od295d8
        text: "Serverless is like a data lake"
      - id: o48af28
        text: "As cloud storage replaced capacity planning with metered billing, serverless brings metered billing to code execution"
        correct: true
      - id: o1323b1
        text: "Serverless is like running on bare metal"
    explanation: "“Just as cloud storage replaced capacity planning ... with a metered billing model, the serverless approach is bringing metered billing to code execution”."
  - id: serverless-limits
    prompt: "Which limitations of serverless does the chapter mention?"
    type: multiple
    options:
      - id: oaf01cb
        text: "Slow start when a function is first invoked"
        correct: true
      - id: o2ed362
        text: "Functions must run on a single dedicated server forever"
      - id: o5fc0de
        text: "Time limits on function execution"
        correct: true
      - id: o4f0f88
        text: "Restricted runtime environments"
        correct: true
    explanation: "“many serverless infrastructure providers impose a time limit on function execution and limit runtime environments, and services might suffer from slow start times when a function is first invoked.”"
  - id: serverless-misleading
    prompt: "Why is the term \"serverless\" misleading?"
    options:
      - id: oabce72
        text: "There is no billing"
      - id: o608481
        text: "Each execution still runs on a server; only later executions may run on a different one"
        correct: true
      - id: o97d8f1
        text: "Serverless functions run on users' laptops"
      - id: oc0dccb
        text: "Serverless means peer-to-peer"
    explanation: "“each serverless function execution still runs on a server, but subsequent executions might run on a different one.”"
  - id: hpc-failure
    prompt: "How do supercomputers typically handle a failed node, and why is that unacceptable for most cloud services?"
    options:
      - id: o0bca29
        text: "They ignore the failure; cloud services do the same"
      - id: o92b8eb
        text: "They failover instantly; cloud services cannot"
      - id: o193621
        text: "Stop the whole job, repair, restart from the last checkpoint; cloud services must keep serving users continuously"
        correct: true
      - id: o3da73e
        text: "They delete all results"
    explanation: "“If a node fails, a common solution is to simply stop the entire cluster workload, repair the faulty node, and then restart the computation from the last checkpoint ... With cloud services, stopping the entire cluster is usually not desirable, since the services need to continually serve users with minimal interruptions.”"
  - id: hpc-trust
    prompt: "Why can supercomputers use shared memory and RDMA more freely than clouds?"
    options:
      - id: oc52180
        text: "They assume a high level of trust among users, while clouds host mutually untrusting organizations needing isolation, encryption and authentication"
        correct: true
      - id: oe77354
        text: "Supercomputers have no network"
      - id: o297517
        text: "RDMA is illegal in public clouds"
      - id: o58e7da
        text: "Clouds lack Ethernet"
    explanation: "Supercomputer RDMA communication “assume[s] a high level of trust among the users of the system. In cloud computing, the network and the machines are often shared by mutually untrusting organizations”. (Exact wording: “but assume a high level of trust among the users of the system”.)"
  - id: bisection
    prompt: "What does *bisection bandwidth* measure in the chapter?"
    options:
      - id: o1c2fe1
        text: "Disk throughput"
      - id: of19451
        text: "Power consumption"
      - id: odf7cf6
        text: "A commonly used measure of a network's overall performance, provided by Clos topologies in cloud datacenters"
        correct: true
      - id: o0f88f3
        text: "The latency of a single packet"
    explanation: "“Cloud datacenter networks are often based on IP and Ethernet, arranged in Clos topologies to provide high bisection bandwidth—a commonly used measure of a network’s overall performance”."
---
Why distribute at all, what it costs, and how microservices, serverless and supercomputers fit in.

## Learning objectives

- List reasons to distribute a system and the costs of doing so
- Explain when a single machine is the better choice
- Weigh microservices and serverless against their overheads
- Contrast cloud computing with HPC

## In a nutshell

- Reasons to distribute: inherent distribution, cross-service requests, fault tolerance, scalability,
  latency, elasticity, specialized hardware, legal compliance, sustainability.
- Costs: any network request can fail ambiguously; remote calls are slow; debugging is hard; cross-service
  consistency becomes the application's problem.
- "More nodes are not always faster" — try one machine first.
- Microservices solve a people problem; serverless brings metered billing to code execution.

## Connections to today

- **DuckDB and embedded analytics.** Laptops now crunch datasets that once needed a Hadoop cluster — the
  single-node revival the chapter mentions.
- **OpenTelemetry** has become the default tracing standard — the chapter's observability tooling.
- **Carbon-aware scheduling.** Some schedulers shift batch jobs to times and regions with cleaner power,
  the book's sustainability reason.
- **Modular monoliths.** Many teams now start with one deployable and split later, echoing the advice that
  small companies rarely need microservices.
