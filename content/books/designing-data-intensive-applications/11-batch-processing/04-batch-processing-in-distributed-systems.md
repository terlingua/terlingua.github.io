---
title: "Batch Processing in Distributed Systems"
questions:
  - id: os-analogy
    prompt: "Which correspondences hold between a single machine and a distributed batch framework?"
    type: multiple
    options:
      - id: of2cf9b
        text: "Process scheduler ↔ job orchestrator"
        correct: true
      - id: o658a49
        text: "CPU cache ↔ coordination service"
      - id: o38f230
        text: "Filesystem ↔ distributed filesystem or object store"
        correct: true
      - id: o78aa9b
        text: "Unix pipes ↔ data passed between tasks"
        correct: true
    explanation: "“you can think of these frameworks as distributed operating systems; they have filesystems, job schedulers, and programs that send data to one another through the filesystem or other communication channels.”"
  - id: block-size
    prompt: "Why do distributed filesystems use much larger blocks (e.g., HDFS 128 MB) than ext4 (4 KiB)?"
    options:
      - id: o1f6f5f
        text: "Network packets are 128 MB"
      - id: o8dad19
        text: "Disks require it"
      - id: oc25465
        text: "Less metadata to track and lower seek overhead relative to reading"
        correct: true
      - id: of0bdc6
        text: "To waste space deliberately"
    explanation: "“Larger blocks mean less metadata to keep track of, which makes a big difference on petabyte-sized datasets. Larger blocks also lower the overhead of seeking to a block relative to reading it.”"
  - id: partial-block
    prompt: "A 900 MB file is stored with 128 MB DFS blocks. How is the last block stored?"
    options:
      - id: ofd16fd
        text: "Padded to 128 MB"
      - id: o5daec4
        text: "Merged into the previous block"
      - id: o30412c
        text: "As a 4 MB block, since DFS blocks needn't be full"
        correct: true
      - id: oab8108
        text: "Rejected"
    explanation: "“a 900 MB file stored with 128 MB blocks would have seven blocks that use 128 MB and one block that uses 4 MB.”"
  - id: erasure
    prompt: "What advantage does erasure coding (e.g., Reed–Solomon) have over full replication?"
    options:
      - id: of3cacc
        text: "It needs no network"
      - id: o6f1728
        text: "It is faster to read"
      - id: o7d885e
        text: "It recovers lost data with lower storage overhead"
        correct: true
      - id: ob175f0
        text: "It provides linearizability"
    explanation: "Erasure coding “allows lost data to be recovered with lower storage overhead than full replication”."
  - id: object-vs-file
    prompt: "Which are differences between object stores and filesystems?"
    type: multiple
    options:
      - id: od77303
        text: "Object stores support file handles with fseek"
      - id: o15f6dc
        text: "\"Directories\" are just key prefixes; empty directories don't exist"
        correct: true
      - id: ocd5f9f
        text: "Objects are immutable and must be rewritten with put"
        correct: true
      - id: oe1ae6e
        text: "Renames are non-atomic copy-and-delete"
        correct: true
    explanation: "“Unlike files on a filesystem, objects are immutable once written”, “object stores do not have the concept of directories”, and “renames are nonatomic; they’re accomplished by copying the object to the new key and then deleting the old object.”"
  - id: prefix-list
    prompt: "How does listing by prefix differ from a directory listing?"
    options:
      - id: oa20000
        text: "It sorts by size"
      - id: oce49fe
        text: "It is not supported"
      - id: o885056
        text: "It behaves like a recursive `ls -R`, returning objects in subpaths too"
        correct: true
      - id: o84da7f
        text: "It returns only immediate children"
    explanation: "“A prefix list operation behaves like a recursive ls -R call on a Unix system. It returns all objects that start with the prefix, and objects in subpaths are included.”"
  - id: locality-vs-separation
    prompt: "What trade-off separates HDFS-style task locality from object stores?"
    options:
      - id: o0fb7ef
        text: "HDFS has no locality"
      - id: offd268
        text: "They are identical"
      - id: o5770ea
        text: "HDFS can run tasks where data lives to save bandwidth; object stores separate storage and compute, using more bandwidth but scaling each independently"
        correct: true
      - id: o276dea
        text: "Object stores always run code next to data"
    explanation: "“DFSs such as HDFS allow computing tasks to be run on the machine that stores a copy of a particular file ... object stores usually keep storage and computation separate. Doing so might use more bandwidth, but modern datacenter networks are very fast”."
  - id: orchestrator-parts
    prompt: "Which are components of a job orchestrator?"
    type: multiple
    options:
      - id: ob37d55
        text: "A resource manager with a global view of cluster state"
        correct: true
      - id: o31741f
        text: "Task executors such as kubelet or NodeManager"
        correct: true
      - id: o521b72
        text: "A query optimizer"
      - id: o23c971
        text: "A scheduler that assigns tasks to nodes"
        correct: true
    explanation: "The chapter lists “Task executors”, “Resource manager” and “Scheduler”, noting “you will find these components in nearly all orchestration systems.”"
  - id: cgroups
    prompt: "Why do executors use Linux cgroups?"
    options:
      - id: o3d39fe
        text: "To elect leaders"
      - id: o897031
        text: "To compress data"
      - id: o937cfd
        text: "For security and performance isolation between tasks"
        correct: true
      - id: od7e939
        text: "To shuffle data"
    explanation: "Executors “work with the operating system to provide both security and performance isolation. YARN and Kubernetes use Linux cgroups”."
  - id: gang
    prompt: "Two jobs each want 100 of 160 cores. What is a risk of gang scheduling (waiting for all 100 cores at once)?"
    options:
      - id: ocadb23
        text: "Jobs run too fast"
      - id: o69c805
        text: "Tasks get duplicated"
      - id: o9b3dc5
        text: "It violates linearizability"
      - id: o127234
        text: "Idle reserved cores lower utilization, and competing reservations can deadlock"
        correct: true
    explanation: "“if the scheduler reserves CPU cores until all 100 are available at the same time, nodes will sit idle. The cluster’s resource utilization will drop, and a deadlock might occur”."
  - id: starvation
    prompt: "What happens if a scheduler simply waits for 100 free cores without reserving them?"
    options:
      - id: of4c0bd
        text: "Nothing; it's optimal"
      - id: o54779c
        text: "The job runs immediately"
      - id: o7c04e6
        text: "The cluster crashes"
      - id: o5b0776
        text: "Other jobs may grab cores, causing starvation"
        correct: true
    explanation: "“if the scheduler simply waits for 100 cores to become available, other jobs might grab the cores in the meantime ... which leads to starvation.”"
  - id: heuristics
    prompt: "How do schedulers cope with the NP-hard allocation problem?"
    options:
      - id: o1e6ba0
        text: "They compute the optimal solution offline"
      - id: o460e27
        text: "They use heuristics such as FIFO, dominant resource fairness, priorities, quotas and bin packing"
        correct: true
      - id: o0a17bf
        text: "They assign randomly"
      - id: of8b0ed
        text: "They refuse large jobs"
    explanation: "“In practice, schedulers therefore use heuristics to make nonoptimal but reasonable decisions. Several algorithms are commonly used, including first-in first-out (FIFO), dominant resource fairness (DRF), priority queues ...”"
  - id: workflow-meaning
    prompt: "How does \"workflow\" in batch processing differ from durable-execution workflows?"
    options:
      - id: o7c823b
        text: "They are identical"
      - id: o10e6e3
        text: "Durable execution processes more data per request"
      - id: oe8a6c3
        text: "Batch workflows make RPCs for every record"
      - id: o07d18f
        text: "Batch workflows chain jobs that read and write datasets, usually without RPCs to external services"
        correct: true
    explanation: "“In the context of batch processing, “workflow” has a different meaning: it’s a sequence of batch processes, each taking input data and producing output data, but normally not making RPCs to external services.”"
  - id: why-workflows
    prompt: "Why split work into a workflow of jobs?"
    type: multiple
    options:
      - id: oee48da
        text: "Several teams' jobs consume one job's output"
        correct: true
      - id: of08fff
        text: "To avoid writing any intermediate data"
      - id: o0ebf52
        text: "Stages need data sharded by different keys"
        correct: true
      - id: o37eb67
        text: "Data moves between different tools (e.g., Spark then Trino)"
        correct: true
    explanation: "The chapter lists these three reasons, including “You might want to transfer data from one processing tool to another” and stages where “one stage needs to shard the data by one key, and the next stage needs to shard by a different key”."
  - id: decouple
    prompt: "Why do workflows usually write intermediate output to a DFS or object store rather than streaming it?"
    options:
      - id: o6d3f6a
        text: "It avoids replication"
      - id: oce6513
        text: "Streaming between tasks is impossible"
      - id: odc937f
        text: "It is faster than memory"
      - id: o3c1eda
        text: "It decouples jobs so they can run at different times"
        correct: true
    explanation: "“it is more typical for one job in a workflow to write its output to a distributed filesystem or object store and for the next job to read it from there. This decouples the jobs from each other, allowing them to run at different times.”"
  - id: spot
    prompt: "Why are batch jobs well suited to spot or preemptible instances?"
    options:
      - id: o798f56
        text: "Batch jobs can't run on regular instances"
      - id: ob87421
        text: "Spot instances never get preempted"
      - id: ob47cff
        text: "Spot instances are faster"
      - id: o642421
        text: "They're often not time-sensitive and can be retried, using otherwise idle capacity cheaply"
        correct: true
    explanation: "“Since batch processing is often used for jobs that are not time-sensitive, it is well suited for using low-priority tasks and spot instances to reduce the cost of running jobs.” But “preemptions occur more frequently than hardware faults”."
  - id: task-retry
    prompt: "How do batch frameworks handle a single failed task?"
    options:
      - id: offdf4e
        text: "Ask the user"
      - id: o41f45d
        text: "Rerun the whole job"
      - id: o3babbe
        text: "Ignore the failure"
      - id: oc89acb
        text: "Delete its partial output and rerun just that task elsewhere"
        correct: true
    explanation: "“The system can delete the partial output from the failed execution and schedule the task to run again on another machine. It would be wasteful to rerun the entire job because of a single task failure”."
  - id: intermediate-ft
    prompt: "How do MapReduce, Spark and Flink differ in protecting intermediate data?"
    options:
      - id: oa18cf8
        text: "All keep everything in memory without recovery"
      - id: o456ff2
        text: "MapReduce writes it to the DFS; Spark keeps it in memory and tracks lineage to recompute; Flink checkpoints snapshots"
        correct: true
      - id: o00f867
        text: "All three write everything to the DFS"
      - id: oa61918
        text: "Spark checkpoints; Flink recomputes lineage"
    explanation: "MapReduce writes intermediate data to the DFS, “Spark keeps intermediate data in memory ... It also keeps track of how the intermediate data was computed”, and “Flink uses a different approach based on periodically checkpointing a snapshot of tasks.”"
---
Distributed batch frameworks are like a distributed OS: a filesystem (DFS or object store), a scheduler,
and programs connected by data. Workflows chain jobs; fault handling retries tasks.

## Learning objectives

- Map OS components (storage, scheduler, pipes) to distributed equivalents
- Compare distributed filesystems with object stores
- Explain orchestrator components and why scheduling is hard
- Explain workflow DAGs and how frameworks handle task failures

## In a nutshell

- DFS: large blocks (HDFS 128 MB), data nodes, a metadata service (NameNode), replication or erasure coding.
- Object stores: immutable objects, prefix listing, no atomic rename, compute separate from storage.
- Orchestrators: executors (kubelet, NodeManager), resource manager, scheduler; heuristics for an NP-hard problem.
- Workflows of 50–100 jobs are common; Airflow/Dagster/Prefect manage dependencies.
- Spot/preemptible capacity suits batch; retries happen per task.

## Connections to today

- **S3-compatible APIs** (MinIO, R2, Tigris, B2) are the de facto storage protocol for analytics engines.
- **Kubernetes batch scheduling** (Kueue, Volcano) adds gang scheduling and quotas for AI training jobs.
- **DeepSeek's 3FS** — named in the chapter — is a modern DFS for AI workloads.
