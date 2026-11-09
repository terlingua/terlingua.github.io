---
title: "Operational Versus Analytical Systems"
questions:
  - id: point-query
    prompt: "A mobile banking app shows a user's current balance by looking up their account by ID. What kind of access is this?"
    options:
      - id: off01a6
        text: "A reverse ETL job"
      - id: o28091a
        text: "A point query in an operational (OLTP) system"
        correct: true
      - id: o41b8ca
        text: "An aggregate query in an analytical system"
      - id: o4e9dfd
        text: "A full-table scan in a data lake"
    explanation: "“An operational system typically looks up a small number of records by a key (this is called a point query).”"
  - id: analytical-query
    prompt: "Which of these is an analytical query in the book's sense?"
    options:
      - id: o1d3322
        text: "How many more bananas than usual were sold during the latest promotion?"
        correct: true
      - id: ob4f1f6
        text: "Update the shipping address on order 991"
      - id: o238b8d
        text: "Check whether this user may delete this post"
      - id: o24f18e
        text: "Fetch the profile of user 4512"
    explanation: "The chapter's examples of analytical queries include “How many more bananas than usual did we sell during our latest promotion?” Such queries scan many records and compute aggregates rather than returning individual records."
  - id: machine-use
    prompt: "Table 1-1 gives a machine-use example for each kind of system. Which pairing is correct?"
    options:
      - id: oa2d86d
        text: "OLTP: detecting fraud patterns; OLAP: checking authorization"
      - id: ofb1ff5
        text: "OLTP: training an ML model; OLAP: serving a web page"
      - id: oe50d5d
        text: "OLTP: checking if an action is authorized; OLAP: detecting fraud/abuse patterns"
        correct: true
      - id: o3bd546
        text: "Both: bulk ETL import"
    explanation: "Table 1-1 lists “Checking if an action is authorized” for operational systems and “Detecting fraud/abuse patterns” for analytical systems."
  - id: represents
    prompt: "Table 1-1 contrasts what data *represents* in each system. What is the distinction, and why does it matter for schema design?"
    options:
      - id: ofcf367
        text: "OLTP holds the latest state; OLAP holds the history of events — so analytical schemas keep many time-stamped facts instead of overwriting"
        correct: true
      - id: o3848d5
        text: "OLTP holds only aggregates"
      - id: odcf1de
        text: "OLTP holds history; OLAP holds only the latest state"
      - id: o525e88
        text: "Both hold only the latest state"
    explanation: "Table 1-1: operational data represents the “Latest state of data (current point in time)”; analytical data represents the “History of events that happened over time”."
  - id: no-custom-sql
    prompt: "Why do OLTP systems rarely let end users run arbitrary SQL?"
    type: multiple
    options:
      - id: of5c991
        text: "SQL cannot express point queries"
      - id: oe4b0cc
        text: "Users could write expensive queries that hurt performance for others"
        correct: true
      - id: oef6727
        text: "Users could read or modify data they lack permission for"
        correct: true
      - id: o6b3c45
        text: "OLTP databases do not support SQL"
    explanation: "“users are generally not allowed to construct custom SQL queries ... since that would potentially allow them to read or modify data that they do not have permission to access. They might also write queries that are expensive to execute and hence affect the database performance for other users.”"
  - id: product-analytics
    prompt: "A SaaS product shows each customer a live dashboard of their usage, computed by aggregating millions of events with sub-second latency. Which category fits?"
    options:
      - id: oeb01a2
        text: "Pure OLTP with point queries"
      - id: of1f6e8
        text: "A data lake of raw files"
      - id: o1ba06f
        text: "Traditional batch OLAP with nightly loads"
      - id: o82ee83
        text: "Product or real-time analytics, such as Pinot, Druid or ClickHouse"
        correct: true
    explanation: "“Another type of system is designed for analytical workloads (queries that aggregate over many records) but embedded into user-facing products. Systems designed for this type of use, known as product analytics or real-time analytics, include Pinot, Druid, and ClickHouse”. They “ingest data in real time and are optimized for low-latency query responses.”"
  - id: online-meaning
    prompt: "What does the book say about the meaning of *online* in OLAP?"
    options:
      - id: oac2336
        text: "It means data is updated in real time"
      - id: ob46426
        text: "It means the system serves end users"
      - id: o89f9de
        text: "It means the system is connected to the internet"
      - id: of56818
        text: "It is unclear; it probably means analysts use the system interactively for explorative queries"
        correct: true
    explanation: "“The meaning of online in OLAP is unclear; it probably indicates that queries are not just for predefined reports, but that analysts use the OLAP system interactively for explorative queries.”"
  - id: warehouse-reasons
    prompt: "Which are reasons the chapter gives for not running analytics directly on OLTP databases?"
    type: multiple
    options:
      - id: o8399c2
        text: "OLTP schemas and layouts are poorly suited to analytics"
        correct: true
      - id: obfa2d1
        text: "Expensive analytical queries would hurt other users' performance"
        correct: true
      - id: od8f793
        text: "Data of interest may be spread across several operational systems (data silos)"
        correct: true
      - id: o359efb
        text: "OLTP systems may sit in a network analysts may not access for security or compliance reasons"
        correct: true
      - id: ob4dd2f
        text: "SQL cannot express analytical queries"
    explanation: "The four bullets: data silos, “The kinds of schemas and data layouts that are good for OLTP are less well suited for analytics”, “running them on an OLTP database would impact the performance for other users”, and the OLTP systems “might reside in a separate network that users are not allowed to directly access”."
  - id: etl-vs-elt
    prompt: "Your team loads raw CRM data into the warehouse first and then transforms it with SQL models inside the warehouse. What is this called?"
    options:
      - id: o6f12f9
        text: "ETL"
      - id: oee177b
        text: "HTAP"
      - id: of51b2f
        text: "Reverse ETL"
      - id: o0b6082
        text: "ELT"
        correct: true
    explanation: "“Sometimes the order of the transform and load steps is swapped (i.e., the transformation is done in the data warehouse, after loading), resulting in ELT.”"
  - id: saas-etl
    prompt: "Why might you build ETL from a SaaS CRM into your own warehouse rather than query the CRM's API directly?"
    options:
      - id: o273d66
        text: "SaaS APIs never return data"
      - id: oa680b5
        text: "ETL is required by GDPR"
      - id: o76fedb
        text: "It eliminates the need for a data warehouse"
      - id: o3f191f
        text: "You have no direct database access, and joining the data in your warehouse enables analyses the SaaS API cannot do"
        correct: true
    explanation: "“you do not have direct access to the original database, since it is accessible only via the software vendor’s API. Bringing the data from these external systems into your own data warehouse can enable analyses that are not possible via the SaaS API.”"
  - id: htap-not-replace
    prompt: "A vendor claims their HTAP database means you no longer need a data warehouse. What is the book's counterargument?"
    options:
      - id: of96fc7
        text: "Each operational system typically has its own database, while analysts need one warehouse that combines data across them"
        correct: true
      - id: o439355
        text: "HTAP is slower than OLTP for point queries"
      - id: o385284
        text: "HTAP systems cannot run SQL"
      - id: o2174c1
        text: "HTAP systems do not support transactions"
    explanation: "“it is considered good practice for each operational system to have its own database ... on the other hand, an enterprise usually has a single data warehouse, so that business analysts can combine data from several operational systems in a single query. HTAP, therefore, does not replace data warehouses.”"
  - id: htap-use
    prompt: "When is HTAP useful, according to the chapter?"
    options:
      - id: ocb0637
        text: "When one application must both scan many rows analytically and read/update individual records with low latency, such as fraud detection"
        correct: true
      - id: of18bb3
        text: "When analysts want to combine data from many companies"
      - id: o16f26b
        text: "When you want to avoid all indexes"
      - id: o316187
        text: "When data must be stored as raw files"
    explanation: "HTAP “is useful when the same application needs to both perform analytical queries that scan a large number of rows and read and update individual records with low latency. Fraud detection can involve such workloads”."
  - id: htap-internals
    prompt: "What does the chapter say about how many HTAP systems work internally, and why does it matter?"
    options:
      - id: o2510f8
        text: "They often couple an OLTP system with a separate analytical system behind one interface, so the OLTP/OLAP distinction still helps you reason about them"
        correct: true
      - id: o608d95
        text: "They store everything in memory"
      - id: oa63fa0
        text: "They use a single unified storage layout optimal for both"
      - id: o3e4c3a
        text: "They run analytics only at night"
    explanation: "“many HTAP systems internally consist of an OLTP system coupled with a separate analytical system, hidden behind a common interface—so the distinction between the two remains important for understanding how these systems work.”"
  - id: specialization
    prompt: "What wider trend does the operational/analytical split illustrate?"
    options:
      - id: o9a8f95
        text: "As scale grows, systems become more specialized for particular workloads"
        correct: true
      - id: oe48667
        text: "General-purpose systems are winning at every scale"
      - id: o2d98ee
        text: "SQL is being replaced by custom languages"
      - id: o471ab9
        text: "Data volumes are shrinking"
    explanation: "“General-purpose systems can handle small data volumes comfortably, but the greater the scale, the more specialized systems tend to become”."
  - id: data-lake-why
    prompt: "Why did data scientists push organizations toward data lakes?"
    options:
      - id: o708e85
        text: "Data scientists cannot write SQL"
      - id: o83996c
        text: "Data lakes are always faster than warehouses"
      - id: o663acb
        text: "Warehouses cannot store numbers"
      - id: o28a982
        text: "Feature engineering and NLP or vision tasks need custom code and formats that a relational warehouse handles poorly"
        correct: true
    explanation: "The chapter lists feature engineering, which “commonly requires custom code that is difficult to express using SQL”, and extracting structure from text or images, and notes many data scientists prefer Pandas, scikit-learn, R or Spark."
  - id: lake-vs-warehouse
    prompt: "What is the key difference between a data lake and a data warehouse?"
    options:
      - id: o1a0bfa
        text: "A data lake supports only relational tables"
      - id: oe2b3bb
        text: "A data lake holds files without imposing a file format, data model or schema"
        correct: true
      - id: o5bdeb6
        text: "A data warehouse cannot hold historical data"
      - id: ob2d8e0
        text: "A data lake is always on premises"
    explanation: "“The difference from a data warehouse is that a data lake simply contains files, without imposing any particular file format, data model, or schema”."
  - id: sushi
    prompt: "What does the \"sushi principle\" argue, and what is its benefit?"
    options:
      - id: o5eaf90
        text: "Data should be served fresh within minutes or discarded"
      - id: oa93349
        text: "Data should be sliced thinly into microservices"
      - id: oa7792a
        text: "\"Raw data is better\": keep data untransformed so each consumer can transform it into the form that best suits them"
        correct: true
      - id: odc9282
        text: "Data must be cleaned before storage"
    explanation: "Keeping data untransformed “has the advantage that each consumer of the data can transform the raw data into the form that best suits their needs. It’s sometimes called the sushi principle: ‘raw data is better’”."
  - id: reverse-etl
    prompt: "An ML model trained in the warehouse is deployed to recommend products on the website. What is this flow from analytical to operational systems sometimes called?"
    options:
      - id: oa2ade9
        text: "Reverse ETL"
        correct: true
      - id: oc9246e
        text: "HTAP"
      - id: o3ad069
        text: "ELT"
      - id: oc3c152
        text: "Change data capture"
    explanation: "“In some cases the outputs of analytical systems are made available to operational systems (a process sometimes known as reverse ETL”."
  - id: streams-analytics
    prompt: "Why might a fraud team prefer stream-based analytics to daily file-based analysis?"
    options:
      - id: o5f358d
        text: "Streams are cheaper to store than files"
      - id: o50ebaa
        text: "Streams let the system respond to events in seconds rather than at the next periodic rerun"
        correct: true
      - id: o5176f0
        text: "File-based analysis cannot compute aggregates"
      - id: o290a22
        text: "Streams remove the need for governance"
    explanation: "“With file-based data analysis, you can rerun the analysis periodically (e.g., daily) to respond to changes in the data, but stream processing allows analytical systems to respond to events much faster, on the order of seconds.”"
  - id: sor-def
    prompt: "In a system of record, what happens if another system disagrees with it?"
    options:
      - id: o635e0c
        text: "Both values are kept as conflicts"
      - id: oda08c4
        text: "The disagreement is resolved by majority vote"
      - id: o95be02
        text: "The system of record's value is correct by definition"
        correct: true
      - id: ob42adb
        text: "The most recently updated system wins"
    explanation: "“If there is any discrepancy between another system and the system of record, the value in the system of record is (by definition) the correct one.”"
  - id: derived-examples
    prompt: "Which are derived data in the book's sense?"
    type: multiple
    options:
      - id: oece38c
        text: "The primary database where user input is first written"
      - id: o9ac36e
        text: "An ML model trained on a dataset"
        correct: true
      - id: od5d3c5
        text: "A secondary index"
        correct: true
      - id: o1bf8f2
        text: "A materialized view"
        correct: true
      - id: obbabf9
        text: "A cache"
        correct: true
    explanation: "“Denormalized values, indexes, materialized views, transformed data representations, and models trained on a dataset also fall into this category.” The primary database where data is first written is the system of record."
  - id: tool-not-role
    prompt: "Can a PostgreSQL database be a derived data system?"
    options:
      - id: o31e618
        text: "Only if it runs in the cloud"
      - id: of27ad4
        text: "No — relational databases are always systems of record"
      - id: o0a5438
        text: "Yes — whether something is a system of record depends on how you use it, not on the tool"
        correct: true
      - id: od3fd72
        text: "Only if it is read-only"
    explanation: "“Most databases, storage engines, and query languages are not inherently systems of record or derived systems. A database is just a tool; how you use it is up to you.”"
  - id: derived-redundant
    prompt: "If derived data is redundant, why keep it?"
    options:
      - id: o7f2572
        text: "It is often essential for good read performance and lets you view the same data from different angles"
        correct: true
      - id: od312d1
        text: "Because systems of record may lose data"
      - id: o9a5a30
        text: "Because regulations require duplicates"
      - id: o02fc6b
        text: "It should always be eliminated"
    explanation: "“Technically speaking, derived data is redundant ... However, this data is often essential for getting good performance on read queries. You can derive several datasets from a single source, enabling you to look at the data from different points of view.”"
  - id: derived-lost
    prompt: "Your Elasticsearch cluster (fed from the primary database) is corrupted. What does the system-of-record framing tell you about recovery?"
    options:
      - id: o059b2e
        text: "The data is permanently lost"
      - id: o84a17d
        text: "You must ask users to re-enter data"
      - id: o778fde
        text: "It can be rebuilt by reprocessing data from the system of record"
        correct: true
      - id: oc1f784
        text: "You must restore Elasticsearch from its own backup only"
    explanation: "“If you lose derived data, you can re-create it from the original source.”"
  - id: integration-gap
    prompt: "What problem does the chapter identify with many databases when you have derived data?"
    options:
      - id: o2ee168
        text: "They require derived data to be stored on the same disk"
      - id: oeaeae6
        text: "They assume your application uses only that one database, so propagating updates to other systems is hard"
        correct: true
      - id: o820cd0
        text: "They refuse to create indexes"
      - id: o03c386
        text: "They cannot be backed up"
    explanation: "“many databases are designed based on the assumption that your application will always need to use only that one database, and they do not make it easy to integrate multiple systems in order to propagate such updates.”"
  - id: new-roles
    prompt: "Which role \"models and transforms data to make it more useful for business analysts and data scientists\"?"
    options:
      - id: o481736
        text: "Database administrator"
      - id: o750263
        text: "Analytics engineer"
        correct: true
      - id: o9755ce
        text: "Site reliability engineer"
      - id: oe1c8f5
        text: "Data engineer"
    explanation: "“Analytics engineers model and transform data to make it more useful for the business analysts and data scientists in an organization”. Data engineers “know how to integrate the operational and analytical systems”."
---
This section introduces the split that runs through the whole book: systems that serve users and
modify data (OLTP), and systems that analyse a read-only copy of that data (OLAP).

## Learning objectives

- Contrast OLTP and OLAP access patterns, users and dataset sizes
- Explain why data warehouses separate analytics from operational databases
- Distinguish data warehouse, data lake, ETL, ELT, HTAP and reverse ETL
- Classify a system as a system of record or derived data

## In a nutshell

| | Operational (OLTP) | Analytical (OLAP) |
|---|---|---|
| Reads | Point queries by key | Aggregates over many records |
| Writes | Individual create/update/delete | Bulk import or event stream |
| Queries | Fixed, baked into the app | Ad hoc exploration |
| Data | Latest state | History of events |

- A **data warehouse** is a separate, read-only, analysis-friendly copy fed by ETL.
- A **data lake** holds files in any format; "raw data is better".
- **Systems of record** hold the authoritative copy; **derived data** can be rebuilt from it.

## Connections to today

- **Lakehouse.** Table formats such as Delta Lake and Apache Iceberg put warehouse-style tables on top
  of data-lake files in object storage, blurring the lake/warehouse line the book draws.
- **Real-time analytics.** ClickHouse, Pinot and Druid power user-facing dashboards — the book's
  "product analytics" category.
- **Feature stores and vector stores** are derived data: if lost, they can be rebuilt from the system of
  record, which is why their freshness pipeline matters more than their backups.
