---
title: "Storage and Indexing for OLTP"
questions:
  - id: index-tradeoff
    prompt: "Why don't databases index every column by default?"
    options:
      - id: ocea53f
        text: "Indexes change the contents of the data"
      - id: o3d5ec3
        text: "Indexes cannot be removed once created"
      - id: o93c5a1
        text: "Every index consumes disk space and slows down writes, so you choose indexes based on query patterns"
        correct: true
      - id: ofcc4a6
        text: "Indexes slow down reads"
    explanation: "“well-chosen indexes speed up read queries, but every index consumes additional disk space and slows down writes, sometimes substantially. For this reason, databases don’t usually index everything by default”."
  - id: log-meaning
    prompt: "What does *log* mean in this book?"
    options:
      - id: o61a287
        text: "Application text output for debugging"
      - id: obb3a9d
        text: "An append-only sequence of records on disk, not necessarily human-readable"
        correct: true
      - id: o2d5890
        text: "A list of user logins"
      - id: oaab5d2
        text: "A logarithm"
    explanation: "“In this book, log is used in the more general sense: an append-only sequence of records on disk. It doesn’t have to be human-readable”."
  - id: db-get
    prompt: "Why does `db_get` in the bash example scale poorly?"
    options:
      - id: oa415f2
        text: "Appends are slow"
      - id: o07e74a
        text: "Each lookup scans the whole file: O(n)"
        correct: true
      - id: oe036e3
        text: "grep cannot read JSON"
      - id: o34802f
        text: "The file is sorted"
    explanation: "“Every time you want to look up a key, db_get has to scan the entire database file from beginning to end ... the cost of a lookup is O(n)”."
  - id: hash-index-problems
    prompt: "An append-only log indexed by an in-memory hash map has which problems?"
    type: multiple
    options:
      - id: oc65cab
        text: "The hash map must be rebuilt by scanning the log after restart"
        correct: true
      - id: odf75f4
        text: "All keys must fit in memory"
        correct: true
      - id: o145cdc
        text: "Writes require random I/O"
      - id: o1a2310
        text: "Disk space from overwritten entries is never freed"
        correct: true
      - id: o151529
        text: "Range queries are inefficient"
        correct: true
    explanation: "The chapter lists all four: never freeing old entries, the hash map “is not persisted”, “The hash table must fit in memory”, and “Range queries are not efficient.”"
  - id: sparse-index
    prompt: "In an SSTable with a sparse index, you look up `handiwork`. The index has `handbag` and `handsome`. What happens?"
    options:
      - id: o2cb4d2
        text: "The key is missing because it is not in the index"
      - id: o6b504e
        text: "Scan the entire file"
      - id: o8a2972
        text: "Seek to the block starting at handbag and scan forward until handiwork is found or passed"
        correct: true
      - id: o9de051
        text: "Binary-search the Bloom filter"
    explanation: "“Because of the sorting, you know that handiwork must appear between handbag and handsome. This means you can seek to the offset for handbag and scan the file from there”."
  - id: block-compression
    prompt: "What does compressing SSTable blocks trade?"
    options:
      - id: oc7480e
        text: "Durability for speed"
      - id: o5efcda
        text: "Faster writes for slower reads"
      - id: o1d0a94
        text: "Memory for disk space"
      - id: of88478
        text: "Less disk space and I/O bandwidth for a bit more CPU time"
        correct: true
    explanation: "“Besides saving disk space, compression reduces the I/O bandwidth use, at the cost of using a bit more CPU time.”"
  - id: memtable-flow
    prompt: "Put these LSM steps in the right order."
    options:
      - id: o7ce2f9
        text: "Write to SSTable → sort in place → copy to memtable → delete WAL"
      - id: o25effd
        text: "Append to Bloom filter → hash to segment → overwrite page → fsync"
      - id: oa9a11d
        text: "Write to B-tree page → split → append to memtable → compact"
      - id: o5f12ce
        text: "Write to memtable → flush to a sorted SSTable segment when large → reads check memtable then newer-to-older segments → background merge and compaction"
        correct: true
    explanation: "The four steps: add writes to “an in-memory ordered map ... called the memtable”; when large, “write it out to disk in sorted order as an SSTable file”; reads check the memtable and then “the next-older segment”; and “From time to time, run a merging and compaction process”."
  - id: memtable-wal
    prompt: "How does an LSM engine avoid losing memtable contents on a crash?"
    options:
      - id: o991599
        text: "It writes the memtable synchronously to an SSTable on every write"
      - id: oe3bec2
        text: "It keeps two memtables in RAM"
      - id: od10e51
        text: "It relies on replication only"
      - id: o33f734
        text: "It appends every write immediately to a separate unsorted log on disk used only to restore the memtable"
        correct: true
    explanation: "“the storage engine keeps a separate log on disk to which every write is immediately appended. This log is not sorted by key, but that doesn’t matter, because its only purpose is to restore the memtable after a crash.”"
  - id: tombstone
    prompt: "How does an LSM engine delete a key, and when can the deletion marker disappear?"
    options:
      - id: o300488
        text: "Delete the whole segment immediately"
      - id: ob5186d
        text: "Remove the key from the memtable only"
      - id: o3056d8
        text: "Append a tombstone; it can be dropped once merged into the oldest segment"
        correct: true
      - id: of4d500
        text: "Overwrite the key in place in every segment"
    explanation: "“you have to append a special deletion record called a tombstone ... Once the tombstone is merged into the oldest segment, it can be dropped.”"
  - id: merge-newest
    prompt: "During a merge, the same key appears in three segments. Which value survives?"
    options:
      - id: o29c036
        text: "All three values"
      - id: oc95b30
        text: "The largest value"
      - id: o550515
        text: "The value from the oldest segment"
      - id: o7dfc16
        text: "The value from the most recent segment"
        correct: true
    explanation: "“If the same key appears in more than one input file, keep only the more recent value.”"
  - id: immutable-recovery
    prompt: "How do immutable segment files simplify crash recovery?"
    options:
      - id: oc50769
        text: "They make WAL unnecessary"
      - id: obd82eb
        text: "An unfinished SSTable from a crashed flush or merge can just be deleted and redone"
        correct: true
      - id: o03e3da
        text: "They prevent crashes"
      - id: o6a2bcd
        text: "They never need checksums"
    explanation: "“If a crash happens while writing out the memtable or while merging segments, the database can just delete the unfinished SSTable and start afresh.”"
  - id: lsm-history
    prompt: "Which source introduced the terms SSTable and memtable, and what name did the 1996 paper give the algorithm?"
    options:
      - id: o8da104
        text: "Google's Bigtable paper; Log-Structured Merge-tree (LSM-tree)"
        correct: true
      - id: ofa51fa
        text: "RocksDB docs; leveled compaction"
      - id: o0e2564
        text: "The B-tree paper; B+ tree"
      - id: o77343d
        text: "Cassandra's paper; Merkle tree"
    explanation: "RocksDB and others “were inspired by Google’s Bigtable paper (which introduced the terms SSTable and memtable). The algorithm was originally published in 1996 under the name Log-Structured Merge-tree, or LSM-tree”."
  - id: bloom-zero
    prompt: "A Bloom filter lookup finds that one of the key's bits is 0. What can you conclude?"
    options:
      - id: o39e2f0
        text: "The key is probably in the SSTable"
      - id: o4aa3ae
        text: "The key definitely is not in that SSTable"
        correct: true
      - id: o2d6b42
        text: "The key is definitely in the SSTable"
      - id: o9e8be2
        text: "The filter is corrupted"
    explanation: "“If at least one of the bits is 0, we know that the key definitely does not appear in the SSTable.”"
  - id: bloom-false-positive
    prompt: "Why are Bloom filter false positives harmless in an LSM engine?"
    options:
      - id: o4bd32e
        text: "They never happen with 10 bits per key"
      - id: o3228e4
        text: "They are corrected by the WAL"
      - id: oc8adbb
        text: "They just cause some unnecessary work checking the block before moving to the next segment"
        correct: true
      - id: o2986e0
        text: "They cause the key to be deleted"
    explanation: "“If it was a false positive, we have done a bit of unnecessary work, but otherwise no harm is done—we just continue the search with the next-oldest segment.”"
  - id: bloom-bits
    prompt: "Using the chapter's rule of thumb, how many bits per key give about a 0.1% false-positive rate?"
    options:
      - id: oa1f374
        text: "About 15"
        correct: true
      - id: o3498ac
        text: "About 10"
      - id: oe0a3f1
        text: "About 5"
      - id: od3b723
        text: "About 100"
    explanation: "“you need to allocate 10 bits of Bloom filter space for every key in the SSTable to get a false-positive probability of 1%, and the probability is reduced tenfold for every 5 additional bits you allocate per key.” 1% → 0.1% needs 5 more bits."
  - id: bloom-helps
    prompt: "For which reads do Bloom filters help most in LSM engines?"
    options:
      - id: of74050
        text: "Keys still in the memtable"
      - id: o7210da
        text: "Full table scans"
      - id: o6ae8d3
        text: "Range scans"
      - id: oc4f4e1
        text: "Keys updated long ago and keys that do not exist"
        correct: true
    explanation: "“it can be slow to read a key that was last updated a long time ago, or to attempt to read a key that does not exist, since the storage engine will need to check several segment files. To speed up such reads, LSM storage engines often include a Bloom filter”."
  - id: size-tiered
    prompt: "Four 256 MB SSTables compact into one 898 MB SSTable under size-tiered compaction. Why not 1,024 MB?"
    options:
      - id: o7b255a
        text: "Deletions, overwrites and TTL expirations remove data during the merge"
        correct: true
      - id: oce8d48
        text: "Compression always halves data"
      - id: o0bc661
        text: "Size-tiered compaction drops the oldest SSTable"
      - id: ob55f68
        text: "The merge failed partially"
    explanation: "“four 256 MB SSTables might be compacted into one 898 MB SSTable (the result is not 1,024 MB because of deletions, overwrites, time-to-live expirations, and so on).”"
  - id: compaction-choice
    prompt: "Your workload is dominated by reads. Which compaction strategy does the rule of thumb favor?"
    options:
      - id: o243a1e
        text: "Size-tiered compaction, because it handles high write throughput"
      - id: o5eb9ce
        text: "No compaction"
      - id: o3d3c06
        text: "Random compaction"
      - id: o838eb2
        text: "Leveled compaction, because reads check fewer SSTables"
        correct: true
    explanation: "“Leveled compaction is more efficient for reads than size-tiered compaction because the storage engine needs to read fewer SSTables” and “size-tiered compaction performs better if you have mostly writes and few reads”."
  - id: size-tiered-space
    prompt: "What is a downside of size-tiered compaction?"
    options:
      - id: o97c7a3
        text: "It forbids deletes"
      - id: obe4866
        text: "Large old SSTables need lots of temporary disk space to merge"
        correct: true
      - id: o923004
        text: "It cannot handle high write throughput"
      - id: o5b095c
        text: "It requires a B-tree"
    explanation: "“The SSTables containing older data can get very large, and merging them requires a lot of temporary disk space.”"
  - id: embedded
    prompt: "When does the chapter suggest an embedded database per tenant on the backend?"
    options:
      - id: o350d79
        text: "Never; embedded databases are only for mobile"
      - id: o3c659e
        text: "When there are many concurrent transactions"
      - id: oe6c6ab
        text: "When each tenant is small and completely separate, with no cross-tenant queries"
        correct: true
      - id: oea1676
        text: "When tenants share large datasets"
    explanation: "“in a multitenant system in which each tenant is small enough and completely separate from others ... you can potentially use a separate embedded database instance per tenant”."
  - id: btree-pages
    prompt: "How does a B-tree's structure differ from an LSM's?"
    options:
      - id: o0a1a95
        text: "Variable-size segments versus fixed pages"
      - id: ob29ea3
        text: "Fixed-size pages that may be overwritten in place, versus variable-size immutable segments"
        correct: true
      - id: o08f4fa
        text: "In-memory only versus on-disk only"
      - id: o552d7f
        text: "Unsorted data versus sorted data"
    explanation: "“The log-structured indexes we saw earlier break the database into variable-size segments ... that are written once and are then immutable. By contrast, B-trees break the database into fixed-size blocks or pages and may overwrite a page in place.”"
  - id: page-sizes
    prompt: "What default page sizes does the chapter mention?"
    options:
      - id: o1cbc26
        text: "512 bytes everywhere"
      - id: o4ceda6
        text: "All databases use 1 MB"
      - id: ofe3716
        text: "PostgreSQL 16 KiB, MySQL 8 KiB"
      - id: o769576
        text: "PostgreSQL 8 KiB, MySQL 16 KiB, traditionally 4 KiB"
        correct: true
    explanation: "“A page is traditionally 4 KiB in size, but PostgreSQL now uses 8 KiB and MySQL uses 16 KiB by default.”"
  - id: branching
    prompt: "A four-level B-tree has 4 KiB pages and a branching factor of 500. Roughly how much data can it address?"
    options:
      - id: oab85b6
        text: "Up to 250 TB"
        correct: true
      - id: o95239b
        text: "Up to 250 MB"
      - id: oaa8b18
        text: "Unlimited"
      - id: o5923df
        text: "Up to 2 TB"
    explanation: "“A four-level tree of 4 KiB pages with a branching factor of 500 can store up to 250 TB.” (500⁴ × 4 KiB.)"
  - id: split
    prompt: "What happens when inserting into a full B-tree leaf?"
    options:
      - id: o33b382
        text: "The insert is rejected"
      - id: o33f92a
        text: "The page splits into two half-full pages and the parent is updated, possibly cascading to the root"
        correct: true
      - id: o10bc1a
        text: "The whole tree is rebuilt"
      - id: o31a9bc
        text: "The key goes into an overflow log"
    explanation: "“If there isn’t enough free space in the page to accommodate the new key, the page is split into two half-full pages, and the parent page is updated ... the splits can continue all the way to the root of the tree.”"
  - id: torn-page
    prompt: "What is a torn page, and what protects against it?"
    options:
      - id: ocfc0d8
        text: "A partially written page after a crash; a write-ahead log"
        correct: true
      - id: o1b9925
        text: "A page on a failed disk; RAID"
      - id: oefc3d6
        text: "A page read by two threads; a mutex"
      - id: oc32007
        text: "A page with too many keys; a Bloom filter"
    explanation: "“If the hardware can’t atomically write an entire page, you can also end up with a partially written page (this is known as a torn page).” The fix: “a write-ahead log (WAL)”."
  - id: wal
    prompt: "What rule does a WAL impose?"
    options:
      - id: o904cbd
        text: "Every B-tree modification must be written to the log before it is applied to the tree's pages"
        correct: true
      - id: o2608e2
        text: "The log is written only at shutdown"
      - id: of954ca
        text: "The log stores only reads"
      - id: ob5d80d
        text: "Pages are written before the log"
    explanation: "The WAL is “an append-only file to which every B-tree modification must be written before it can be applied to the pages of the tree itself.”"
  - id: durable-when
    prompt: "When is a B-tree write durable?"
    options:
      - id: o8372e9
        text: "Once the client receives a TCP ACK"
      - id: o920d2d
        text: "Once the page is modified in memory"
      - id: o734e2d
        text: "Once it is in the WAL and the WAL has been fsynced"
        correct: true
      - id: ob025b6
        text: "Only after the page is flushed"
    explanation: "“As long as data has been written to the WAL and flushed to disk using the fsync system call, the data will be durable”."
  - id: cow
    prompt: "How does LMDB avoid needing a WAL?"
    options:
      - id: o032e6c
        text: "Copy-on-write: modified pages are written elsewhere and new parent versions point to them"
        correct: true
      - id: ocfb196
        text: "It never updates data"
      - id: oa67a3a
        text: "It uses a Bloom filter"
      - id: o050ea6
        text: "It keeps everything in memory"
    explanation: "“some databases (like LMDB) use a copy-on-write scheme. A modified page is written to a different location, and a new version of the parent pages in the tree is created”."
  - id: abbreviate
    prompt: "Why abbreviate keys in interior B-tree pages?"
    options:
      - id: oc6b106
        text: "To make leaf pages smaller only"
      - id: o4064f3
        text: "To hide data from attackers"
      - id: od054dd
        text: "More keys per page means a higher branching factor and fewer levels"
        correct: true
      - id: o4242c1
        text: "Because keys must be under 8 bytes"
    explanation: "“Packing more keys into a page allows the tree to have a higher branching factor and thus fewer levels.”"
  - id: rule-of-thumb
    prompt: "What is the rule of thumb comparing LSM-trees and B-trees, and its caveat?"
    options:
      - id: o24a323
        text: "LSM for write-heavy, B-trees for reads — but benchmarks are workload-sensitive, so test your own workload"
        correct: true
      - id: o1bb665
        text: "B-trees for writes, LSM for reads, always"
      - id: od48737
        text: "They are identical"
      - id: o3386de
        text: "LSM is always faster"
    explanation: "“As a rule of thumb, LSM-trees are better suited for write-heavy applications, whereas B-trees are faster for reads. However, benchmarks are often sensitive to details of the workload. You need to test systems with your particular workload”."
  - id: lsm-range
    prompt: "Why are range queries relatively more expensive on LSM engines?"
    options:
      - id: o905ce4
        text: "Range queries require a WAL replay"
      - id: oce961e
        text: "SSTables are not sorted"
      - id: od62353
        text: "They must scan all segments in parallel and merge, and Bloom filters cannot help"
        correct: true
      - id: o8182be
        text: "They must lock the memtable"
    explanation: "Range queries “need to scan all the segments in parallel and combine the results. Bloom filters don’t help for range queries (since you would need to compute the hash of every possible key within the range, which is impractical)”."
  - id: latency-spike
    prompt: "Why can heavy writes cause latency spikes in RocksDB?"
    options:
      - id: o15f2ad
        text: "Bloom filters overflow"
      - id: o9978da
        text: "If compaction can't keep up, the memtable fills and the engine applies backpressure, suspending reads and writes until it flushes"
        correct: true
      - id: oc4bfdc
        text: "B-tree splits cascade"
      - id: o08e13a
        text: "The WAL is deleted"
    explanation: "“High write throughput can cause latency spikes in a log-structured storage engine if the memtable fills up ... RocksDB, apply backpressure in this situation: they suspend all reads and writes until the memtable has been written out to disk”."
  - id: seq-vs-random
    prompt: "Why can LSM engines generally sustain higher write throughput on the same hardware?"
    options:
      - id: o2090eb
        text: "They compress keys only"
      - id: ofee39e
        text: "They skip the WAL"
      - id: oe82414
        text: "They do not write to disk"
      - id: o77bcc1
        text: "They write large segments sequentially, while B-trees scatter small random page writes"
        correct: true
    explanation: "“Disks generally have higher sequential write throughput than random write throughput, which means that a log-structured storage engine can generally handle higher write throughput on the same hardware than a B-tree.”"
  - id: ssd-gc
    prompt: "Why are random writes slower and more wearing on SSDs, despite no moving parts?"
    options:
      - id: ob9b79e
        text: "SSDs only accept sequential writes"
      - id: ocb22bb
        text: "SSD heads must seek"
      - id: o252811
        text: "Flash is erased in large blocks, so random writes leave mixed blocks that need garbage collection, consuming bandwidth and causing extra writes"
        correct: true
      - id: o3bcaa9
        text: "Random writes disable the controller cache"
    explanation: "Flash “can be read or written one page (typically 4 KiB) at a time, but it can be erased only one block (typically 512 KiB) at a time” and “random writes wear out the drive faster than sequential writes.”"
  - id: write-amp-def
    prompt: "How is write amplification defined?"
    options:
      - id: o9f31e7
        text: "Read bytes divided by write bytes"
      - id: obce53e
        text: "Total bytes written to disk divided by the bytes an append-only log with no index would write"
        correct: true
      - id: ob80cad
        text: "Disk capacity divided by data size"
      - id: od6167d
        text: "Number of replicas × write size"
    explanation: "“If you take the total number of bytes written to disk in a workload and divide that by the number of bytes you would have to write if you simply wrote an append-only log with no index, you get the write amplification.”"
  - id: write-amp-which
    prompt: "For typical workloads, which tends to have lower write amplification and why?"
    options:
      - id: oeb38d7
        text: "Neither has write amplification"
      - id: ob3f9be
        text: "B-trees, because they write each value once"
      - id: o38532b
        text: "LSM-trees, because they don't write entire pages and can compress SSTable chunks"
        correct: true
      - id: o4a7372
        text: "They are always equal"
    explanation: "“For typical workloads, LSM-trees tend to have lower write amplification because they don’t have to write entire pages and they can compress chunks of the SSTable”."
  - id: benchmark-length
    prompt: "Why must LSM write benchmarks run for a long time?"
    options:
      - id: o3ac800
        text: "Bloom filters need warm-up"
      - id: o6b0422
        text: "An empty LSM has no compaction yet; as data grows, writes share bandwidth with compaction"
        correct: true
      - id: o4a852b
        text: "The WAL starts empty"
      - id: o9296bf
        text: "SSDs are faster when new"
    explanation: "“When writing to an empty LSM-tree, no compactions are going on yet, so all the disk bandwidth is available for new writes. As the database grows, new writes need to share the disk bandwidth with compaction.”"
  - id: fragmentation
    prompt: "After deleting many keys from a B-tree, why doesn't the file shrink?"
    options:
      - id: o1c2f20
        text: "The WAL holds the space"
      - id: of4a759
        text: "The OS caches the file"
      - id: ocd2cdd
        text: "Freed pages are in the middle of the file; they can be reused but not easily returned to the OS without a process like VACUUM"
        correct: true
      - id: o6619e5
        text: "B-trees never delete keys"
    explanation: "“Subsequent additions to the B-tree can use those free pages, but they can’t easily be returned to the operating system because they are in the middle of the file ... such as the vacuum process in PostgreSQL.”"
  - id: delete-compliance
    prompt: "Why can data deletion for compliance be slow to take effect in LSM engines?"
    options:
      - id: oa7bf22
        text: "Tombstones are never written"
      - id: o1f3d64
        text: "LSM engines cannot delete"
      - id: oe94bbf
        text: "A deleted record may persist in higher levels until the tombstone propagates through all compaction levels"
        correct: true
      - id: oadd7be
        text: "Compaction runs only once a year"
    explanation: "“in most LSM storage engines a deleted record may still exist in the higher levels until the tombstone representing the deletion has been propagated through all the compaction levels, which might take a long time.”"
  - id: snapshot
    prompt: "Why are snapshots easy in LSM engines?"
    options:
      - id: o0c96bd
        text: "Segments are immutable, so you flush the memtable and record which files existed, without copying them"
        correct: true
      - id: o5aaf9c
        text: "They write a full copy of the database"
      - id: o80d20e
        text: "They lock the database during backups"
      - id: obf154b
        text: "B-trees and LSM are equally hard"
    explanation: "“You can write out the memtable and record which segment files existed at that time. As long as you don’t delete the files that are part of the snapshot, you don’t need to actually copy them.”"
  - id: secondary-unique
    prompt: "How do secondary indexes handle non-unique values?"
    options:
      - id: o588bf5
        text: "They hash duplicates into separate tables"
      - id: o0ae383
        text: "They store only the first match"
      - id: oabce0f
        text: "Store a list of matching row IDs per entry, or make entries unique by appending the row ID"
        correct: true
      - id: o401995
        text: "They reject duplicates"
    explanation: "“either by making each value in the index a list of matching row identifiers (like a postings list in a full-text index) or by making each entry unique by appending a row identifier to it.”"
  - id: clustered
    prompt: "In InnoDB, what does a secondary index store as its value?"
    options:
      - id: ob97712
        text: "A heap-file location"
      - id: o10d8e5
        text: "The primary key of the row"
        correct: true
      - id: obf2dec
        text: "The full row"
      - id: o9e4c18
        text: "Nothing"
    explanation: "The value can be “the primary key of the row in question (InnoDB does this for secondary indexes)”, while “the primary key of a table is always a clustered index”."
  - id: covering
    prompt: "What does a covering index trade?"
    options:
      - id: oeb1a31
        text: "Less disk space for slower reads"
      - id: o3853e3
        text: "Some queries answered from the index alone, at the cost of extra disk space and slower writes"
        correct: true
      - id: o9e1f1f
        text: "Faster writes for slower reads"
      - id: o2734a7
        text: "It has no trade-off"
    explanation: "A covering index “allows some queries to be answered by using the index alone ... This can make some queries faster, but the duplication of data means the index uses more disk space and slows down writes.”"
  - id: heap-move
    prompt: "A heap-file row grows and must move. What must happen?"
    options:
      - id: o6ac4d4
        text: "Rebuild the table"
      - id: o86a1bf
        text: "Nothing; indexes use primary keys"
      - id: oa1e496
        text: "Delete the row"
      - id: o056752
        text: "Update all indexes to the new location, or leave a forwarding pointer"
        correct: true
    explanation: "“all indexes need to be updated to point at the new heap location of the record, or a forwarding pointer must be left behind in the old heap location”."
  - id: in-memory-why
    prompt: "Why are in-memory databases faster, according to the chapter?"
    options:
      - id: o458128
        text: "They skip durability"
      - id: o21ab1d
        text: "They never read from disk, while disk databases always do"
      - id: oe5f371
        text: "RAM is infinitely fast"
      - id: o7d110b
        text: "They avoid the overhead of encoding in-memory structures into an on-disk form"
        correct: true
    explanation: "“Counterintuitively, the performance advantage of in-memory databases is not due to the fact that they don’t need to read from disk ... Rather, they are faster because they avoid the overheads of encoding in-memory data structures in a form that can be written to disk”."
  - id: in-memory-durable
    prompt: "How can in-memory databases still be durable?"
    type: multiple
    options:
      - id: o06d170
        text: "Write periodic snapshots"
        correct: true
      - id: oef121c
        text: "Rely on the CPU cache"
      - id: ob4b388
        text: "Write a log of changes to disk"
        correct: true
      - id: od85226
        text: "Replicate state to other machines"
        correct: true
    explanation: "Durability is achieved “with special hardware ... or, more commonly, writing a log of changes to disk, by writing periodic snapshots to disk, or replicating the in-memory state to other machines.”"
  - id: redis-models
    prompt: "What other benefit of in-memory databases does Redis illustrate?"
    options:
      - id: oc8740e
        text: "SQL compatibility"
      - id: o03c38a
        text: "Columnar compression"
      - id: o1026e1
        text: "Offering data structures such as priority queues and sets that are hard to implement with disk-based indexes"
        correct: true
      - id: oec1ffc
        text: "Strict serializability"
    explanation: "“Redis offers a database-like interface to various data structures, such as priority queues and sets. Because it keeps all data in memory, its implementation is comparatively simple.”"
---
From a two-line bash key-value store to LSM-trees and B-trees: how OLTP engines trade write speed, read
speed, space and durability.

## Learning objectives

- Explain why indexes speed reads and slow writes
- Walk through memtables, SSTables, compaction, tombstones and Bloom filters
- Compare size-tiered and leveled compaction
- Explain B-tree pages, splits, WALs and torn pages
- Compare B-trees and LSM-trees on reads, writes, amplification and space
- Distinguish clustered, heap-file and covering indexes, and in-memory databases

## In a nutshell

- Appending to a log is the fastest write; lookups need an index.
- LSM: memtable → sorted immutable segments → background merge. Bloom filters skip segments.
- B-tree: fixed pages overwritten in place, WAL for crash safety, depth O(log n).
- Rule of thumb: LSM for write-heavy, B-tree for read-heavy; benchmark your own workload.
- In-memory databases win by avoiding encoding overheads, not by avoiding disk reads.

## Connections to today

- **RocksDB everywhere.** It underpins many systems (e.g., Kafka Streams state stores and CockroachDB's
  former engine; Cockroach now uses Pebble, an LSM inspired by RocksDB).
- **LSM on object storage.** SlateDB writes SSTables straight to S3, the direction the chapter describes.
- **PostgreSQL VACUUM and bloat** are the B-tree fragmentation and heap-file issues in daily operations.
- **SQLite on the edge** (Turso, Cloudflare D1) shows embedded engines serving production backends.
