---
title: "Modes of Dataflow"
questions:
  - id: future-self
    prompt: "In what sense is writing to a database \"sending a message to your future self\"?"
    options:
      - id: o1388d5
        text: "The database encrypts data for later"
      - id: oc7faf5
        text: "A later version of the code must decode what earlier code wrote, so backward compatibility is needed"
        correct: true
      - id: o22d5e4
        text: "Only the same binary can read it"
      - id: o08b697
        text: "Databases deliver emails"
    explanation: "“you can think of storing something in the database as sending a message to your future self. Backward compatibility is clearly necessary here”."
  - id: db-forward
    prompt: "Why do databases often also need forward compatibility?"
    options:
      - id: o7615ed
        text: "During rolling upgrades, newer code may write values that older instances then read"
        correct: true
      - id: o327660
        text: "SQL requires it"
      - id: of48331
        text: "Databases store only old data"
      - id: o64bc90
        text: "Clients always upgrade first"
    explanation: "“a value in the database may be written by a newer version of the code and subsequently read by an older version of the code that is still running. Thus, forward compatibility is also often required for databases.”"
  - id: data-outlives-code
    prompt: "What does \"data outlives code\" mean?"
    options:
      - id: o22e614
        text: "Data never changes"
      - id: o9e49ad
        text: "Data is backed up longer than code"
      - id: o01ebeb
        text: "Code should be stored in the database"
      - id: o60a75b
        text: "Code can be replaced in minutes, but years-old data remains in its original encoding unless rewritten"
        correct: true
    explanation: "“The same is not true of database contents; the five-year-old data will still be there, in the original encoding, unless you have explicitly rewritten it since then. This observation is sometimes summed up as data outlives code.”"
  - id: lazy-migration
    prompt: "How do LSM storage engines help with schema evolution?"
    options:
      - id: o52e394
        text: "They rewrite all data on every ALTER"
      - id: obf4a69
        text: "They forbid schema changes"
      - id: odc8e6e
        text: "They store schemas in Bloom filters"
      - id: oc26746
        text: "They rewrite data in the latest format during compaction"
        correct: true
    explanation: "“LSM-tree storage engines ... will rewrite data using the latest format during compaction.”"
  - id: archive
    prompt: "When dumping a database for backup or a warehouse, which encoding approach does the chapter suggest?"
    options:
      - id: ofa0482
        text: "Encode the dump consistently with the latest schema, e.g., Avro container files or Parquet"
        correct: true
      - id: o677c0b
        text: "Use CSV without headers"
      - id: oc9b42b
        text: "Use the language's built-in serialization"
      - id: o98c387
        text: "Preserve every historical encoding"
    explanation: "“the data dump will typically be encoded using the latest schema ... Since you’re copying the data anyway, you might as well encode the copy of the data consistently.” Avro object container files or Parquet fit well."
  - id: service-vs-db
    prompt: "How do services differ from databases in what clients can do?"
    options:
      - id: o5a21aa
        text: "They are identical"
      - id: o5a9d8c
        text: "Services allow arbitrary SQL"
      - id: oe861ed
        text: "Services expose application-specific inputs and outputs, giving encapsulation; databases allow arbitrary queries"
        correct: true
      - id: o3179ee
        text: "Databases cannot be queried remotely"
    explanation: "“services expose an application-specific API that allows only inputs and outputs that are predetermined by the business logic ... This restriction provides a degree of encapsulation”."
  - id: rest
    prompt: "What does REST emphasize?"
    options:
      - id: o361cb8
        text: "Simple data formats, URLs to identify resources, and HTTP features for caching, authentication and content negotiation"
        correct: true
      - id: oc6c2c9
        text: "Location transparency"
      - id: oae3e77
        text: "Binary encodings and tag numbers"
      - id: oec2063
        text: "Distributed transactions"
    explanation: "“REST emphasizes simple data formats, using URLs for identifying resources and using HTTP features for cache control, authentication, and content type negotiation.”"
  - id: idls
    prompt: "Which are the two most popular service IDLs mentioned?"
    type: multiple
    options:
      - id: oc76e74
        text: "OpenAPI for JSON web services"
        correct: true
      - id: oeae17e
        text: "Protocol Buffers for gRPC"
        correct: true
      - id: oc92cca
        text: "WSDL"
      - id: o86d767
        text: "CORBA IDL"
    explanation: "“The two most popular service IDLs are OpenAPI (also known as Swagger), used for web services that send and receive JSON, and Protocol Buffers, used for gRPC services.”"
  - id: location-transparency
    prompt: "Why does the chapter call RPC's location transparency fundamentally flawed?"
    options:
      - id: o027da9
        text: "Local calls are slower than remote calls"
      - id: o2c38b2
        text: "RPC requires XML"
      - id: oc1c5fb
        text: "RPC cannot pass integers"
      - id: obcd97e
        text: "Network requests can be lost, time out ambiguously, have variable latency, and need parameter encoding — unlike local calls"
        correct: true
    explanation: "“The RPC model tries to make a request to a remote network service look the same as calling a function ... the approach is fundamentally flawed.” The chapter lists unpredictability, timeouts, retries causing duplicates, variable latency, encoding of parameters and cross-language types."
  - id: timeout-outcome
    prompt: "What outcome does a network request have that a local call lacks?"
    options:
      - id: od49062
        text: "Throwing an exception"
      - id: occe01f
        text: "Returning with no result because of a timeout, leaving you unsure whether it went through"
        correct: true
      - id: o2051f0
        text: "Never returning because of an infinite loop"
      - id: ob04109
        text: "Returning a value"
    explanation: "“A network request has another possible outcome: it may return without a result, because of a timeout. In that case, you simply don’t know what happened”."
  - id: retry-dup
    prompt: "Why can retrying a failed network request cause duplicate actions?"
    options:
      - id: o5e9f44
        text: "Retries use a different protocol"
      - id: o0ae006
        text: "Servers process every request twice"
      - id: o34541f
        text: "Networks always duplicate packets"
      - id: o4a8c6d
        text: "The original request may have succeeded with only the response lost; idempotence is needed to deduplicate"
        correct: true
    explanation: "“it could happen that the previous request actually got through, and only the response was lost. In that case, retrying will cause the action to be performed multiple times, unless you build a mechanism for deduplication (idempotence) into the protocol”."
  - id: rest-appeal
    prompt: "What is part of REST's appeal relative to RPC?"
    options:
      - id: o6dd024
        text: "It hides the network entirely"
      - id: o6a6ff6
        text: "It treats state transfer over a network as distinct from a function call"
        correct: true
      - id: o97e7cf
        text: "It uses binary encoding"
      - id: oc82784
        text: "It guarantees exactly-once delivery"
    explanation: "“Part of the appeal of REST is that it treats state transfer over a network as a process that is distinct from a function call.”"
  - id: dns-lb
    prompt: "What is a drawback of DNS-based load balancing?"
    options:
      - id: o4aa2fc
        text: "DNS requires a service mesh"
      - id: o979838
        text: "Caching and slow propagation mean clients may see stale IPs when servers change frequently"
        correct: true
      - id: od337dc
        text: "DNS encrypts traffic"
      - id: ofda880
        text: "DNS cannot map one name to several IPs"
    explanation: "“DNS is designed to propagate changes over longer periods of time and to cache DNS entries. If servers are started, stopped, or moved frequently, clients might see stale IP addresses”."
  - id: discovery
    prompt: "How does a service discovery system know an instance is still alive?"
    options:
      - id: oc0a1b8
        text: "The instance registers its endpoint and sends periodic heartbeats"
        correct: true
      - id: oe7c9b7
        text: "The client pings every server"
      - id: od78302
        text: "DNS TTLs expire"
      - id: o47d733
        text: "The load balancer guesses"
    explanation: "“The service then periodically sends a heartbeat signal to the discovery system to signal that the service is still available.”"
  - id: mesh
    prompt: "What advantages does a service mesh offer?"
    type: multiple
    options:
      - id: o4ebdd4
        text: "Elimination of network latency"
      - id: o909df1
        text: "TLS handled at the load-balancer level, shielding services from certificate complexity"
        correct: true
      - id: o7e9f3f
        text: "No extra processes or libraries"
      - id: o0773c3
        text: "Rich observability of which services call each other"
        correct: true
    explanation: "“connection encryption can be handled entirely at the load balancer level ... Mesh systems also provide sophisticated observability.”"
  - id: rpc-evolution
    prompt: "Why does the chapter say RPC needs only backward compatibility on requests and forward on responses?"
    options:
      - id: o75d069
        text: "Clients never change"
      - id: o460d49
        text: "It's reasonable to assume all servers are updated before clients"
        correct: true
      - id: o921354
        text: "Servers never change"
      - id: o8a860f
        text: "RPC frameworks forbid evolution"
    explanation: "“it is reasonable to assume that all the servers will be updated first and all the clients second. Thus, you need backward compatibility only on requests, and forward compatibility on responses.”"
  - id: api-versioning
    prompt: "For cross-organization APIs where you can't force clients to upgrade, what often happens after a breaking change?"
    options:
      - id: o1873b5
        text: "The old API is deleted immediately"
      - id: o7ab53d
        text: "The provider switches to RPC"
      - id: od5de96
        text: "The provider maintains multiple API versions side by side, perhaps indefinitely"
        correct: true
      - id: odf64ab
        text: "All clients are forced to upgrade on a date"
    explanation: "“If a compatibility-breaking change is required, the service provider frequently ends up maintaining multiple versions of the service API side by side.” Common approaches: “a version number in the URL or in the HTTP Accept header”."
  - id: workflow-engine
    prompt: "What two components typically make up a workflow engine?"
    options:
      - id: o6182a0
        text: "A compiler and a linker"
      - id: oa55278
        text: "An orchestrator that schedules tasks and an executor that runs them"
        correct: true
      - id: o2f344e
        text: "A leader and followers"
      - id: obf923c
        text: "A broker and a consumer"
    explanation: "“Workflow engines are typically composed of an orchestrator and an executor: the orchestrator is responsible for scheduling tasks to be executed, and the executor is responsible for executing tasks.”"
  - id: durable-exec
    prompt: "How does a durable execution framework provide exactly-once semantics when a task fails midway?"
    options:
      - id: of72b55
        text: "It re-executes the task but replays previously logged RPC results instead of repeating successful calls"
        correct: true
      - id: oa42df7
        text: "It never retries"
      - id: oc8abab
        text: "It runs every task twice and compares"
      - id: o833334
        text: "It wraps all services in a database transaction"
    explanation: "“If a task fails, the framework will re-execute the task, but will skip any RPC calls or state changes that the task made successfully before failing ... This is possible because durable execution frameworks log all RPCs and state changes to durable storage”."
  - id: durable-pitfalls
    prompt: "Which are pitfalls of durable execution frameworks?"
    type: multiple
    options:
      - id: oee3dfc
        text: "They cannot call external services at all"
      - id: of0e8b8
        text: "Reordering calls in workflow code can cause undefined behaviour on replay"
        correct: true
      - id: oc37309
        text: "External services still need idempotent APIs with unique IDs"
        correct: true
      - id: o82a9ce
        text: "Nondeterministic code such as random numbers or clocks breaks replay"
        correct: true
    explanation: "“External services ... must still provide an idempotent API”, “you might introduce undefined behavior simply by reordering function calls”, and “nondeterministic code such as calling random number generators or system clocks is problematic”."
  - id: durable-versioning
    prompt: "What's the safer way to change a durable workflow's code?"
    options:
      - id: o9c2406
        text: "Delete all running workflows first"
      - id: o40e34b
        text: "Deploy a new version separately so existing invocations keep using the old code"
        correct: true
      - id: ofdee89
        text: "Randomize the call order"
      - id: od89835
        text: "Edit the existing workflow in place"
    explanation: "“Instead of modifying the code of an existing workflow, it is safer to deploy a new version of the code separately, so that re-executions of existing workflow invocations continue to use the old version”."
  - id: broker-advantages
    prompt: "Which are advantages of a message broker over direct RPC?"
    type: multiple
    options:
      - id: oc9528f
        text: "Guarantees the sender gets an immediate response"
      - id: o27bb17
        text: "Redelivers messages to crashed consumers"
        correct: true
      - id: oe5e8e4
        text: "Sends one message to several recipients"
        correct: true
      - id: o92af8a
        text: "Buffers messages when the recipient is unavailable or overloaded"
        correct: true
      - id: o48312d
        text: "Avoids the need for service discovery"
        correct: true
    explanation: "The chapter lists buffering, redelivery, avoiding service discovery, fan-out to several recipients, and logical decoupling. Communication is asynchronous: the sender “doesn’t wait for the message to be delivered”."
  - id: queue-vs-topic
    prompt: "With several consumers, how do the queue and topic patterns differ?"
    options:
      - id: o0cd26c
        text: "Both deliver to one"
      - id: oac0c37
        text: "A queue delivers each message to one consumer; a topic delivers each message to all subscribers"
        correct: true
      - id: o11e89f
        text: "Both deliver to all"
      - id: ocbf312
        text: "A queue delivers to all; a topic to one"
    explanation: "“If there are multiple consumers, one of them receives the message.” For topics: “If there are multiple subscribers, they all receive the message.”"
  - id: broker-retention
    prompt: "Which broker property matters if you want event sourcing?"
    options:
      - id: o397713
        text: "Using a single queue"
      - id: o6a52fa
        text: "Storing messages indefinitely rather than deleting them after consumption"
        correct: true
      - id: occ03e5
        text: "Supporting XML"
      - id: of84f49
        text: "Low latency only"
    explanation: "“many message brokers automatically delete messages after they have been consumed. However, some brokers can be configured to store messages indefinitely, which you would require if you wanted to use event sourcing”."
  - id: republish
    prompt: "A consumer republishes messages to another topic. Which earlier problem can recur?"
    options:
      - id: o665ee4
        text: "Write skew"
      - id: ocd5d81
        text: "Torn pages"
      - id: o262c04
        text: "Losing unknown fields, as in Figure 5-1"
        correct: true
      - id: o3e7a71
        text: "Head-of-line blocking"
    explanation: "“If a consumer republishes messages to another topic, you may need to be careful to preserve unknown fields, to prevent the issue described previously in the context of databases (Figure 5-1).”"
  - id: actors
    prompt: "Why does location transparency work better for actors than for RPC?"
    options:
      - id: o2ccaab
        text: "Actors share memory"
      - id: o38cb1f
        text: "Actors never communicate over a network"
      - id: o8c87f0
        text: "The actor model already assumes messages may be lost even within one process"
        correct: true
      - id: o67a356
        text: "Actor messages are synchronous"
    explanation: "“Location transparency works better in the actor model than in RPC, because the actor model already assumes that messages may be lost, even within a single process.”"
  - id: actors-upgrade
    prompt: "Do distributed actor frameworks remove the need for compatible encodings?"
    options:
      - id: ob90730
        text: "Yes — actors don't encode messages"
      - id: o4b3d16
        text: "Only for Erlang"
      - id: oc7376b
        text: "Yes — the framework upgrades all nodes atomically"
      - id: oa50297
        text: "No — during rolling upgrades, messages flow between nodes running different versions"
        correct: true
    explanation: "“if you want to perform rolling upgrades of your actor-based application, you still have to worry about forward and backward compatibility, as messages may be sent from a node running the new version to a node running the old version”."
---
Who encodes and who decodes? Data flows through databases, services, durable workflows and message
brokers, and each path has its own compatibility needs.

## Learning objectives

- Explain why databases need forward and backward compatibility ("data outlives code")
- Explain why RPC's location transparency is flawed
- Compare load balancing and service discovery options
- Describe durable execution and its pitfalls
- Compare message brokers and distributed actors

## In a nutshell

- Writing to a database is sending a message to your future self.
- Services: assume servers upgrade first — backward on requests, forward on responses.
- Remote calls can time out ambiguously, are slower, need encoding, and cross languages.
- Durable execution logs every RPC and replays deterministically; code must stay deterministic and calls idempotent.
- Brokers buffer, redeliver, decouple and fan out.

## Connections to today

- **Temporal, Restate and Azure Durable Functions** are mainstream ways to build long-running business
  processes and AI-agent workflows that survive crashes.
- **Service meshes** (Istio, Linkerd) and ambient/sidecar-less modes are the chapter's in-process or
  sidecar load balancers.
- **gRPC with protobuf** dominates internal service calls; OpenAPI dominates public JSON APIs.
- **Microsoft Orleans** — named in the chapter — is a .NET distributed actor framework.
