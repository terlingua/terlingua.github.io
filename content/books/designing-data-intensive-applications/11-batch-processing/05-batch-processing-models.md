---
title: "Batch Processing Models"
questions:
  - id: map-steps
    prompt: "In MapReduce, which steps do you write yourself?"
    options:
      - id: ocd2641
        text: "The shuffle network protocol"
      - id: ob88f3e
        text: "The input record parser only"
      - id: o0c97f6
        text: "The mapper and the reducer"
        correct: true
      - id: oab2af6
        text: "The sort"
    explanation: "“Steps 2 (map) and 4 (reduce) are where you write your custom data processing code ... Step 3, the sort step, is implicit in MapReduce”."
  - id: unix-mapping
    prompt: "Map the Unix log example onto MapReduce. What corresponds to the reducer?"
    options:
      - id: o3fb037
        text: "`head -n 5`"
      - id: o2c79a1
        text: "The first `sort`"
      - id: o4cfb9d
        text: "`awk '{print $7}'`"
      - id: obf4ef5
        text: "`uniq -c`"
        correct: true
    explanation: "“In the Unix tools example, the reducer is implemented by the command uniq -c”. The mapper is awk and the implicit sort is the first sort."
  - id: mapper-state
    prompt: "Why does the mapper keep no state between records?"
    options:
      - id: oaab373
        text: "So each record is processed independently, enabling parallelism and safe retries"
        correct: true
      - id: od033ba
        text: "Because records are sorted"
      - id: oebc366
        text: "Because it's written in Java"
      - id: ob29be1
        text: "Because memory is limited"
    explanation: "The mapper “does not keep any state from one input record to the next, so each record is handled independently.” The sidebar notes avoiding mutable state “helps enable parallel execution”."
  - id: second-job
    prompt: "How does MapReduce implement the second (ranking) sort from the Unix example?"
    options:
      - id: o1579f7
        text: "With a second MapReduce job reading the first job's output"
        correct: true
      - id: o66b66a
        text: "With a combiner"
      - id: o82d3e1
        text: "In the same reducer"
      - id: ocdd57e
        text: "It can't"
    explanation: "“if you need a second sorting stage, you can implement it by writing a second MapReduce job and using the output of the first job as input to the second job.”"
  - id: mr-slow
    prompt: "Why is MapReduce slow compared with newer engines?"
    options:
      - id: o7ef26d
        text: "It has no parallelism"
      - id: o1c283d
        text: "File-based I/O between jobs prevents pipelining downstream work before upstream completes"
        correct: true
      - id: of7cf3a
        text: "It doesn't sort"
      - id: o751ce6
        text: "It uses Python"
    explanation: "“its file-based I/O prevents job pipelining (i.e., processing output data in a downstream job before the upstream job is complete).”"
  - id: dataflow-adv
    prompt: "Which are advantages of dataflow engines over MapReduce?"
    type: multiple
    options:
      - id: o483b6c
        text: "Starting operators as soon as input is ready"
        correct: true
      - id: o1ba24a
        text: "Keeping intermediate state in memory or on local disk"
        correct: true
      - id: od6dc6f
        text: "Sorting only where required"
        correct: true
      - id: o5181d9
        text: "Always writing every stage to the DFS"
      - id: oa54bed
        text: "Fusing consecutive operators that keep the same sharding"
        correct: true
    explanation: "The chapter lists these plus locality optimizations and process reuse; dataflow engines “usually execute significantly faster”."
  - id: shuffle-not-random
    prompt: "Why is \"shuffle\" a misleading term?"
    options:
      - id: o098688
        text: "It means compression"
      - id: od59c29
        text: "It randomizes records"
      - id: ob528fb
        text: "It produces a sorted order with no randomness"
        correct: true
      - id: o5faa9b
        text: "It only applies to cards"
    explanation: "“When you shuffle a deck of cards, you end up with a random order. In contrast, the shuffle we’re talking about produces a sorted order, with no randomness.”"
  - id: shuffle-mech
    prompt: "In Hadoop's shuffle, how does a key reach the right reducer?"
    options:
      - id: o211fe0
        text: "A central server routes each record"
      - id: o33c43b
        text: "Reducers scan all mapper output"
      - id: o704072
        text: "The mapper writes a separate sorted file per reducer, choosing by hash of the key; reducers fetch and merge their files"
        correct: true
      - id: o06d468
        text: "Keys are random"
    explanation: "“each mapper creates a separate output file on its local disk for every reducer ... a hash of the key typically determines which reducer file it is written to”. Reducers then merge “mergesort-style”."
  - id: reducers-count
    prompt: "Who decides the number of reduce tasks?"
    options:
      - id: oa934eb
        text: "The job's author"
        correct: true
      - id: o028ca5
        text: "The number of input shards"
      - id: o55ce0a
        text: "The cluster size"
      - id: ode1259
        text: "The DFS"
    explanation: "“While the number of map tasks is determined by the number of input shards, the number of reduce tasks is configured by the job’s author”."
  - id: sort-merge-join
    prompt: "How does a sort-merge join of activity events with user profiles work in MapReduce?"
    options:
      - id: o54a728
        text: "The user table is broadcast to every mapper"
      - id: oe43b9a
        text: "Events are joined in the database"
      - id: o04d2e8
        text: "Both inputs are keyed by user ID; the shuffle brings them together, and a secondary sort puts the profile first"
        correct: true
      - id: o01662f
        text: "Each event looks up the user over the network"
    explanation: "“The shuffle then ensures that a reducer function can access a particular user’s date of birth and all of that user’s page view events at the same time ... This technique is known as a secondary sort.”"
  - id: join-memory
    prompt: "Why does the sort-merge join need little memory and no network requests in the reducer?"
    options:
      - id: ob40c7a
        text: "It reads from a remote database"
      - id: o39e10b
        text: "The reducer processes one user's records at a time, keeping only one user record in memory"
        correct: true
      - id: o033ccc
        text: "It uses Bloom filters"
      - id: of1f227
        text: "It caches the whole user table"
    explanation: "“Since a reducer processes all the records for a particular user ID in one go, it needs to keep only one user record in memory at any one time, and it never needs to make any requests over the network.”"
  - id: sql-benefits
    prompt: "Why did batch systems embrace SQL?"
    type: multiple
    options:
      - id: o6f0731
        text: "SQL forbids joins"
      - id: o6a5206
        text: "Analysts and tools already know it, and it enables interactive exploration"
        correct: true
      - id: oc6ebdc
        text: "SQL is required by HDFS"
      - id: oe62fa0
        text: "Optimizers can choose join algorithms and reorder joins automatically"
        correct: true
    explanation: "SQL is “the lingua franca for batch processing” and “High-level query languages don’t just make the humans using the system more productive; they also improve job execution efficiency at a machine level.”"
  - id: not-sql
    prompt: "Which workloads are hard to express in SQL?"
    type: multiple
    options:
      - id: of2482c
        text: "Iterative graph algorithms such as PageRank"
        correct: true
      - id: oc0ede0
        text: "Simple aggregations"
      - id: o363d79
        text: "Multimodal AI data processing such as images and audio"
        correct: true
      - id: o38e19d
        text: "Filtering rows"
    explanation: "“Not all batch jobs are easily expressed in SQL, though, including iterative graph algorithms such as PageRank, complex ML tasks ... AI data processing, which includes nonrelational and multimodal data”."
  - id: warehouse-cost
    prompt: "Why might you run a large job on Spark rather than a cloud warehouse?"
    options:
      - id: oc7a515
        text: "Warehouses can't run SQL"
      - id: o943d12
        text: "Warehouses tend to be more expensive, and row-by-row computation is inefficient on columnar storage"
        correct: true
      - id: o626ab9
        text: "Warehouses have no scheduling"
      - id: o72f0af
        text: "Spark is always simpler"
    explanation: "“Row-by-row computation is less efficient when using column-oriented storage formats ... Cloud data warehouses also tend to be more expensive than other batch processing systems.”"
  - id: distributed-df
    prompt: "What surprise can occur when moving from Pandas to distributed DataFrames?"
    options:
      - id: o985605
        text: "Local DataFrames are indexed and ordered, but distributed ones generally aren't"
        correct: true
      - id: od14641
        text: "Spark executes each call immediately"
      - id: oc4f416
        text: "Distributed DataFrames don't support filters"
      - id: o3f5291
        text: "Pandas runs lazily"
    explanation: "“local DataFrames are usually indexed and ordered, while distributed DataFrames are generally not. This can lead to performance surprises”. Also, “Spark first translates all the DataFrame API calls into a query plan and runs query optimization”."
---
MapReduce's map–sort–reduce, the dataflow engines that replaced it, the shuffle that powers joins and
grouping, and the SQL and DataFrame interfaces on top.

## Learning objectives

- Map the Unix pipeline onto MapReduce
- List dataflow engines' advantages over MapReduce
- Explain the shuffle and a sort-merge join with secondary sort
- Explain why SQL and DataFrames improve both productivity and execution

## In a nutshell

- Mapper: stateless per record; reducer: per key over sorted values.
- Dataflow engines (Spark, Flink) treat a whole workflow as one job: sort only where needed, fuse operators, keep state local, pipeline.
- Shuffle = distributed sort, not random.
- Sort-merge join: both sides keyed by user ID; reducer sees the profile first, then events.
- Warehouses and batch frameworks have converged; DataFrames go distributed (but are unordered).

## Connections to today

- **Spark Connect and DataFrame APIs in BigQuery and Snowflake (Snowpark)** show the convergence the
  sidebar describes.
- **Polars and Daft** offer lazy, optimized DataFrames that plan queries before executing, like Spark.
