---
title: "Foundations Check — Before Chapter 1"
questions:
  - id: sql-aggregate
    prompt: "A table `orders(store_id, amount, created_at)` holds every sale. Which query answers \"total revenue per store in January 2026\"?"
    options:
      - id: of5dfd1
        text: "`SELECT store_id, SUM(amount) FROM orders WHERE created_at >= '2026-01-01' AND created_at < '2026-02-01' GROUP BY store_id`"
        correct: true
      - id: odfd48f
        text: "`SELECT store_id, amount FROM orders WHERE created_at LIKE '2026-01%'`"
      - id: ofa3c95
        text: "`SELECT store_id, COUNT(*) FROM orders HAVING created_at = 'January'`"
      - id: o487382
        text: "`SELECT SUM(amount) FROM orders GROUP BY created_at`"
    explanation: "Aggregation needs `GROUP BY` on the grouping key and an aggregate function such as `SUM`. A half-open date range (`>=` start, `<` next month) avoids boundary bugs with timestamps. This is exactly the kind of query the book calls analytical."
  - id: primary-key
    prompt: "What does a primary key guarantee in a relational table?"
    options:
      - id: ofaeb8f
        text: "The column is automatically encrypted"
      - id: oa005f5
        text: "The table can be joined only on that column"
      - id: oc2ab9f
        text: "Rows are physically stored in insertion order"
      - id: o37763d
        text: "Each row is uniquely identified by its key value, and the key cannot be NULL"
        correct: true
    explanation: "A primary key is a uniqueness constraint plus NOT NULL. It is usually backed by an index, but physical order and join options are separate matters."
  - id: join-meaning
    prompt: "Why would a normalized schema store `customer_id` in `orders` instead of copying the customer's name and address into every order row?"
    options:
      - id: o402913
        text: "So each fact about a customer lives in one place and updates do not have to touch many rows"
        correct: true
      - id: o48c236
        text: "Because relational databases cannot store text columns in more than one table"
      - id: oea4a25
        text: "Because joins are always faster than reading a single row"
      - id: ob9e47b
        text: "Because foreign keys compress data automatically"
    explanation: "Normalization removes duplication so a fact has one authoritative copy. The cost is that reads need a join. Chapter 3 returns to this trade-off in depth."
  - id: http-stateless
    prompt: "A backend handles each HTTP request and then forgets everything about it. Where must state that has to survive between requests live?"
    options:
      - id: o5db940
        text: "In the TCP connection, which keeps request history"
      - id: o713394
        text: "On the client or in server-side data infrastructure such as a database or cache"
        correct: true
      - id: ofa0540
        text: "In the HTTP status code"
      - id: o27f1ff
        text: "In the web server's local variables, which persist across requests"
    explanation: "HTTP is request/response; a stateless service keeps nothing between requests. Persistent state lives in a datastore (or on the client, for example in a cookie)."
  - id: tcp-guarantees
    prompt: "Which statements about TCP are correct?"
    type: multiple
    options:
      - id: o345021
        text: "It delivers bytes to the application in the order they were sent"
        correct: true
      - id: ofd8814
        text: "It guarantees the receiving application has processed the data"
      - id: ocf6931
        text: "It retransmits segments that appear to have been lost"
        correct: true
      - id: od69e63
        text: "It guarantees a message arrives within a bounded time"
    explanation: "TCP gives ordered, reliable delivery within a connection while it stays up, but no timing bound and no proof the remote application acted on the data. Chapter 9 builds on exactly these limits."
  - id: latency-numbers
    prompt: "Roughly how do these compare: reading from RAM, a round trip within a datacenter, and a round trip between continents?"
    options:
      - id: ob6658b
        text: "An intercontinental round trip is about 1 ms"
      - id: o168814
        text: "A datacenter round trip is faster than a RAM read"
      - id: o7f08d6
        text: "RAM ≈ 100 ns, datacenter round trip ≈ 0.5 ms, intercontinental round trip ≈ 100–150 ms"
        correct: true
      - id: o44d451
        text: "All three are within a factor of 10 of each other"
    explanation: "These classic orders of magnitude explain why the book says remote calls are \"vastly slower\" than local function calls and why geographically distributed users motivate multi-region deployments."
  - id: ssd-vs-hdd
    prompt: "Why are random reads far cheaper on an SSD than on a spinning hard disk?"
    options:
      - id: o60a807
        text: "Hard disks can only read sequentially"
      - id: o5b040e
        text: "SSDs compress all data so reads are smaller"
      - id: o3f0c72
        text: "A hard disk must physically move a head and wait for the platter to rotate; an SSD has no moving parts"
        correct: true
      - id: oeebad7
        text: "SSDs keep all data in RAM"
    explanation: "Seek time and rotational latency dominate HDD random I/O. SSDs remove that mechanical cost, though sequential access is still faster than random on both. Chapter 4 relies on this."
  - id: index-purpose
    prompt: "What is the main trade-off of adding an index to a table?"
    options:
      - id: o4c6eb8
        text: "Lower storage use at the cost of slower reads"
      - id: oab6cc8
        text: "Faster writes at the cost of slower reads"
      - id: o69571a
        text: "No cost; indexes are always beneficial"
      - id: o4cbb0b
        text: "Faster reads for queries that use it, at the cost of slower writes and extra storage"
        correct: true
    explanation: "Every write must also update each index. Chapter 4 calls this the fundamental trade-off of storage systems."
  - id: kib-units
    prompt: "A cloud block device uses 4 KiB blocks. How many bytes is that?"
    options:
      - id: o6201eb
        text: "40,960"
      - id: o88e497
        text: "512"
      - id: od7433a
        text: "4,000"
      - id: o37308c
        text: "4,096"
        correct: true
    explanation: "KiB is a binary unit: 1 KiB = 1,024 bytes. The book uses binary units such as KiB when it talks about block sizes."
  - id: vm-vs-container
    prompt: "Which is the best description of a virtual machine in IaaS?"
    options:
      - id: o127d15
        text: "A physical server rented by the hour"
      - id: ofedc35
        text: "An emulated computer with its own OS kernel, sharing physical hardware with other VMs via a hypervisor"
        correct: true
      - id: of86a99
        text: "A serverless function with no operating system"
      - id: o616697
        text: "A process that shares the host kernel and is isolated only by namespaces"
    explanation: "VMs virtualize hardware; containers share the host kernel. The book's \"Layering of cloud services\" discussion starts from VMs as generic computing resources."
---
The Preface says the book assumes experience building web applications, familiarity with relational
databases and SQL, and a high-level understanding of protocols such as TCP and HTTP. These questions
check that background. They are not drawn from the book's text, so their explanations carry no quotes.

## Learning objectives

- Confirm the SQL, HTTP and networking background the book assumes
- Recall basic storage and hardware facts the early chapters build on

## How to use this check

If you miss several of these, spend an hour on a SQL refresher and on how an HTTP request travels over
TCP before starting Chapter 1. Everything later in the book stacks on this vocabulary.
