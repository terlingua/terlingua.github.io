---
title: "Chapter Opening — Online, Batch and Why Immutable Inputs Help"
questions:
  - id: batch-measure
    prompt: "What is the primary performance measure of batch jobs?"
    options:
      - id: o5040a8
        text: "Tail latency"
      - id: o627097
        text: "Response time"
      - id: o64ca1e
        text: "Availability"
      - id: o3fea61
        text: "Throughput"
        correct: true
    explanation: "“The primary measure of performance is usually throughput: how much data the job can process per unit of time.”"
  - id: human-fault
    prompt: "What does \"human fault tolerance\" mean for batch jobs?"
    options:
      - id: o5c1974
        text: "Operators restart failed nodes"
      - id: oa52b45
        text: "Jobs tolerate typos in SQL"
      - id: o7ecf17
        text: "If buggy code produces bad output, you can roll back the code and rerun to get correct output"
        correct: true
      - id: o5fe211
        text: "Humans can't make mistakes in batch jobs"
    explanation: "“If you introduce a bug into the code and the output is wrong or corrupted, you can simply roll back to a previous version of the code and rerun the job ... The idea of being able to recover from buggy code has been called human fault tolerance.”"
  - id: db-contrast
    prompt: "Why don't read/write databases have the same property?"
    options:
      - id: o1dc5ef
        text: "Databases are immutable"
      - id: oeae631
        text: "Databases can't be rolled back"
      - id: obbc962
        text: "Databases never have bugs"
      - id: odecca8
        text: "Rolling back buggy code doesn't fix bad data it already wrote"
        correct: true
    explanation: "“Most databases with read/write transactions do not have this property: if you deploy buggy code that writes bad data to the database, rolling back the code will do nothing to fix that data.”"
  - id: batch-downsides
    prompt: "Which are challenges of batch processing?"
    type: multiple
    options:
      - id: o18c2bd
        text: "Batch jobs can't run in parallel"
      - id: o2509a8
        text: "Any input change, even one byte, means reprocessing the entire input"
        correct: true
      - id: oc1e1ef
        text: "Outputs are mutable"
      - id: o61daa5
        text: "Output can usually be consumed only after the whole job finishes"
        correct: true
    explanation: "“With most frameworks, output can be processed by other jobs only after the whole job finishes ... any change to the input data—even a single byte—requires the batch job to reprocess the entire input dataset.”"
  - id: mapreduce-status
    prompt: "What is MapReduce's status today?"
    options:
      - id: odc25ca
        text: "Replaced by HDFS"
      - id: o5ed992
        text: "Largely obsolete and no longer used at Google; Spark, Flink and warehouse engines are used instead"
        correct: true
      - id: o073b39
        text: "Used only for streaming"
      - id: o63b00b
        text: "The dominant batch framework"
    explanation: "MapReduce “now ... is largely obsolete and no longer used at Google. Batch processing today is more often done using frameworks such as Spark or Flink, or data warehouse query engines.”"
  - id: storage-shift
    prompt: "Where are batch storage layers shifting?"
    options:
      - id: oad620f
        text: "From columnar to row formats"
      - id: o677401
        text: "From disks to tape"
      - id: o5464ca
        text: "From object stores to HDFS"
      - id: o2e8160
        text: "From distributed filesystems like HDFS to object storage like S3"
        correct: true
    explanation: "“Batch storage layers are shifting from distributed filesystems (DFSs) like HDFS ... to object storage systems such as S3.”"
---
Online systems answer requests fast; batch jobs read immutable inputs and regenerate outputs from
scratch. That simple discipline buys "human fault tolerance".

## Learning objectives

- Contrast online and batch systems and their performance measures
- Explain the benefits of immutable inputs and side-effect-free jobs
- Place MapReduce in history

## In a nutshell

- Batch: throughput matters; online: response time matters.
- Bad code? Roll back and rerun — or switch back to old output (time travel).
- Downsides: output available only when the job finishes; any input change means reprocessing everything.
- MapReduce is largely obsolete; Spark, Flink and cloud warehouses replaced it; Airflow/Dagster/Prefect replaced Oozie.

## Connections to today

- **Iceberg/Delta time travel** makes "keep the old output and switch back" a table-format feature.
- **Spot instances** make batch cheap precisely because jobs can be retried.
