---
title: "Foundations Check — Before Chapter 4"
questions:
  - id: hash-range
    prompt: "Why can't a hash map answer \"all keys between 1000 and 1999\" efficiently?"
    options:
      - id: o11a4d9
        text: "Hash maps only support deletes"
      - id: od9d6c7
        text: "Hash lookups are O(n)"
      - id: od49367
        text: "Hashing scatters keys, so neighbouring keys are not stored near each other"
        correct: true
      - id: o9f965f
        text: "Hash maps cannot store integers"
    explanation: "Hash maps give expected O(1) point lookups but no order. Range queries need a sorted structure such as a B-tree or a sorted file."
  - id: bst-balance
    prompt: "Why do databases use balanced trees rather than plain binary search trees?"
    options:
      - id: o35b96a
        text: "Binary trees cannot store strings"
      - id: o4d1322
        text: "Balanced trees use less memory than arrays"
      - id: oafb243
        text: "Balancing is required for hashing"
      - id: oddafa2
        text: "A plain BST can degenerate into a linked list with O(n) depth for sorted inserts; balancing keeps depth O(log n)"
        correct: true
    explanation: "Insert keys in ascending order into an unbalanced BST and every node gets one child. Balanced trees (red–black trees, B-trees) bound the height."
  - id: merge-sorted
    prompt: "How much memory does merging k already-sorted files require if you stream them?"
    options:
      - id: o213d9d
        text: "Exactly k × the total size"
      - id: o2c97e5
        text: "Roughly one current record per file, regardless of total size"
        correct: true
      - id: oea7db6
        text: "Enough to hold all files in memory"
      - id: ob614dc
        text: "Enough to hold the largest file"
    explanation: "A k-way merge only keeps the head of each input. That is why LSM compaction can merge huge files with little memory."
  - id: fsync
    prompt: "What does calling `fsync` on a file do?"
    options:
      - id: o27028b
        text: "Deletes the file's cache"
      - id: o41a1fd
        text: "Synchronizes clocks between machines"
      - id: o23f06b
        text: "Compresses the file"
      - id: o8c245e
        text: "Asks the OS to flush the file's buffered writes to durable storage before returning"
        correct: true
    explanation: "Without fsync, a successful `write` may only have reached the OS page cache and can be lost on power failure. Chapter 4 says data is durable once written to the WAL and fsynced."
  - id: page-cache
    prompt: "A database reads a block that it read a moment ago. Why might no disk I/O happen?"
    options:
      - id: o1ee4fa
        text: "The disk remembers the last read"
      - id: o6fb713
        text: "The CPU's L1 cache holds the whole file"
      - id: ofa3bcf
        text: "The OS page cache keeps recently used disk blocks in memory"
        correct: true
      - id: o8c5624
        text: "Reads never touch disk on SSDs"
    explanation: "The operating system caches file data in RAM. The chapter points out that this is why in-memory databases are not fast merely because they avoid disk reads."
  - id: rle
    prompt: "Run-length encode the sequence 0,0,0,0,1,1,0,0,0."
    options:
      - id: obe3530
        text: "4 zeros, 2 ones, 3 zeros"
        correct: true
      - id: o173b83
        text: "4,2,3,1"
      - id: ob3a0b7
        text: "0,1,0"
      - id: oa436a0
        text: "9 values"
    explanation: "Run-length encoding stores counts of consecutive repeated values. It shines on sorted or sparse columns."
  - id: cosine
    prompt: "Two vectors point in exactly the same direction but one is twice as long. What is their cosine similarity?"
    options:
      - id: obf4d7e
        text: "2"
      - id: o12d91b
        text: "0"
      - id: ob50e17
        text: "1"
        correct: true
      - id: o8ac1db
        text: "0.5"
    explanation: "Cosine similarity depends only on the angle between vectors, not their lengths."
  - id: log2
    prompt: "A balanced tree has a branching factor of 2 and holds about one million keys. Roughly how deep is it?"
    options:
      - id: o0f2fbe
        text: "About 20 levels"
        correct: true
      - id: oad6bb5
        text: "About 6 levels"
      - id: ob303b9
        text: "About 1,000 levels"
      - id: o855d9e
        text: "About 3 levels"
    explanation: "log₂(1,000,000) ≈ 20. With a branching factor of hundreds, as in B-trees, depth drops to three or four."
---
Chapter 4 assumes basic data structures (hash maps, balanced trees, sorting), Big-O notation, and the
memory/disk hierarchy. These questions are not drawn from the book's text.

## Learning objectives

- Recall complexity of common data structures
- Explain mergesort and why sorted data enables range scans
- Recall what fsync, page caches and compression do
