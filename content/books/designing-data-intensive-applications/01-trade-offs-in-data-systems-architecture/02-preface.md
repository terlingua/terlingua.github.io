---
title: "Preface — Who the Book Is For and What Changed"
questions:
  - id: short-answer
    prompt: "A colleague asks which database your team should use. What does the Preface suggest is the honest short answer, and why?"
    options:
      - id: o2ce07c
        text: "Whatever the cloud provider recommends, because managed services remove trade-offs"
      - id: od42959
        text: "\"It depends\", because different technologies make different trade-offs and none is best for all situations"
        correct: true
      - id: o6fc1e4
        text: "PostgreSQL, because relational databases are universally applicable"
      - id: o7788e4
        text: "The newest system available, because older designs are obsolete"
    explanation: "The Preface opens: “The short answer is, “It depends.” The long answer is…this book.” and adds “Different technologies for storing and processing data make different trade-offs, and no one approach is best for all situations.”"
  - id: principles-endure
    prompt: "Why does the book focus on principles rather than tutorials for specific tools?"
    options:
      - id: odb6db3
        text: "Because the authors avoid naming products"
      - id: of78252
        text: "Because principles are easier to test in interviews"
      - id: o93f449
        text: "Tools change quickly but the underlying principles endure, and they let you see where each tool fits"
        correct: true
      - id: obf9c14
        text: "Because tutorials are copyrighted by vendors"
    explanation: "“Although the landscape of technologies for processing and storing data is diverse and fast-changing, the underlying principles endure. If you understand those principles, you’re in a position to see where each tool fits in, how to make good use of it, and how to avoid its pitfalls.”"
  - id: old-ideas
    prompt: "A teammate dismisses a 1980s research paper as irrelevant to a 2026 cloud system. Which position does the Preface take?"
    options:
      - id: o416969
        text: "Industry practice always supersedes academic research"
      - id: obbe60a
        text: "Only research from the last five years is relevant"
      - id: o63debe
        text: "Dismissing old or academic ideas is a mistake; many foundational ideas came from research, some decades old"
        correct: true
      - id: o331b18
        text: "Old research is useful only for historical interest"
    explanation: "“The computing industry tends to be attracted to things that are new and shiny and to look down on ideas that are considered old or academic. That is a mistake; many powerful, foundational ideas in computing arose from research, some recent, some decades ago.”"
  - id: academia-weakness
    prompt: "What weakness of academia does the Preface acknowledge?"
    options:
      - id: o67ca43
        text: "Its results cannot be reproduced"
      - id: oad4d12
        text: "It never studies distributed systems"
      - id: o68ee88
        text: "It sometimes lacks a clear idea of which issues matter in practice"
        correct: true
      - id: oc725bb
        text: "It ignores formal precision"
    explanation: "“On the other hand, academia sometimes lacks a clear idea of what issues are important in practice. This book combines the best of both: academic precision and attention to detail with an industrial focus on practicality.”"
  - id: assumed-knowledge
    prompt: "Which background does the Preface explicitly assume?"
    type: multiple
    options:
      - id: o3260a1
        text: "Fluency in a specific programming language"
      - id: o645e66
        text: "A high-level understanding of protocols such as TCP and HTTP"
        correct: true
      - id: o1fdd4c
        text: "Familiarity with relational databases and SQL"
        correct: true
      - id: o885963
        text: "Experience building web-based applications"
        correct: true
    explanation: "“This book assumes that you already have some experience building web-based applications and that you are familiar with relational databases and SQL. A high-level understanding of common network protocols like TCP and HTTP is helpful. Your choice of programming language or framework makes no difference for this book.”"
  - id: hidden-complexity
    prompt: "Why would a cloud engineer who uses fully managed services still benefit from the book?"
    options:
      - id: o130a9f
        text: "Cloud certifications require it"
      - id: ofd775a
        text: "Managed services expose all internals anyway"
      - id: o8a486d
        text: "Understanding underlying principles helps with performance optimization and debugging even when complexity is hidden"
        correct: true
      - id: oed06e6
        text: "Managed services require manual replication configuration"
    explanation: "“You will find that even though modern distributed systems hide a lot of complexity from you, understanding their underlying principles is extremely useful for performance optimization and debugging.”"
  - id: second-edition-drivers
    prompt: "According to the Preface, which two changes most affected the second edition?"
    options:
      - id: o3918d0
        text: "Blockchain adoption and quantum computing"
      - id: o86b443
        text: "The decline of SQL and rise of NoSQL"
      - id: o6433a9
        text: "The explosion of interest in AI and the rise of cloud native data systems architectures"
        correct: true
      - id: o549d00
        text: "The end of Moore's law and the rise of ARM processors"
    explanation: "“The biggest technical changes that have affected this book since the first edition are the explosion of interest in AI and the rise of cloud native data systems architectures.”"
  - id: ai-coverage
    prompt: "Which AI-related topics did the second edition add?"
    type: multiple
    options:
      - id: o03c826
        text: "Vector indexes for semantic search"
        correct: true
      - id: obaa175
        text: "DataFrames for training datasets"
        correct: true
      - id: o58c1c5
        text: "Batch processing systems for preparing training data"
        correct: true
      - id: od2425b
        text: "How to train large language models"
    explanation: "“we have added coverage of data systems that support AI and machine learning, including vector indexes (used for semantic search), DataFrames (used for training datasets), and batch processing systems for preparing large amounts of training data.” The book adds that it “is not about AI per se”."
  - id: mapreduce-dropped
    prompt: "Why was the batch processing chapter rewritten?"
    options:
      - id: o239a42
        text: "Hadoop was removed from cloud providers"
      - id: o24ec98
        text: "Batch processing has been replaced entirely by streaming"
      - id: oa1f699
        text: "MapReduce is now largely obsolete"
        correct: true
      - id: o5a691e
        text: "Readers found it too long"
    explanation: "“as MapReduce is now largely obsolete, we have rewritten the batch processing chapter accordingly”."
  - id: chapter-ten
    prompt: "Which chapter does the Preface single out as almost completely rewritten for clarity?"
    options:
      - id: o8a28b3
        text: "Chapter 3, on data models"
      - id: o3bb337
        text: "Chapter 10, on consistency and consensus"
        correct: true
      - id: ob575e6
        text: "Chapter 6, on replication"
      - id: oc8fa1c
        text: "Chapter 14, on ethics"
    explanation: "“others (such as Chapter 10, on consistency and consensus) were almost completely rewritten to make them clearer.”"
  - id: new-topics
    prompt: "Which of these were added as new discussions in the second edition?"
    type: multiple
    options:
      - id: o73b733
        text: "Sync engines and local-first software"
        correct: true
      - id: oa4c4d4
        text: "Workflow engines and durable execution"
        correct: true
      - id: o43ca0f
        text: "Formal methods and randomized testing"
        correct: true
      - id: o229b59
        text: "GraphQL"
        correct: true
      - id: o402e06
        text: "Tolkien-style maps"
    explanation: "“We have also added discussions of sync engines and local-first software, workflow engines and durable execution, formal methods and randomized testing, GraphQL”. The maps went the other way: “we sadly decided to drop the Tolkien-style maps.”"
  - id: papers-free
    prompt: "You find a reference to an academic paper at the end of a chapter but the publisher wants payment. What does the Preface suggest?"
    options:
      - id: o1bd29b
        text: "Search for the title in Google Scholar to find an open-access PDF"
        correct: true
      - id: o4ab731
        text: "Email the authors of the book"
      - id: o259975
        text: "Skip the reference; it is optional"
      - id: ob3e988
        text: "Buy the conference proceedings"
    explanation: "“For academic papers, search for the title in Google Scholar to find open-access (free) PDF files. Books might cost money, but you shouldn’t have to pay for research papers.”"
---
The Preface sets the book's stance: no single technology is best, and enduring principles matter more
than any product.

## Learning objectives

- State the book's guiding philosophy and intended readership
- Name the main additions in the second edition and what was removed

## In a nutshell

- "It depends" is the honest short answer to "which database?" The book is the long answer.
- It deliberately mixes research ideas with industrial practice.
- Second-edition additions: cloud native architectures, vector indexes, DataFrames, sync engines and
  local-first software, durable execution, formal methods and randomized testing, GraphQL, and GDPR.
- MapReduce coverage shrank because it is "largely obsolete".

## Connections to today

- **AI workloads.** Vector search and training-data pipelines are now ordinary data infrastructure,
  not research curiosities — hence their inclusion.
- **Lakehouse and object storage.** Systems such as Delta Lake, Iceberg and warehouse engines built on
  S3-style storage are the "cloud native ideas" that the Preface says are "woven in throughout".
