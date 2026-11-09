---
title: "Case Study — Social Network Home Timelines"
questions:
  - id: functional-vs-nonfunctional
    prompt: "Which is a nonfunctional requirement?"
    options:
      - id: o616d0c
        text: "Users can follow other users"
      - id: o9f6f0d
        text: "A post can contain up to 280 characters"
      - id: of8e510
        text: "Users can delete their own posts"
      - id: ob729f1
        text: "The home timeline must load within 200 ms for 99% of requests"
        correct: true
    explanation: "Functional requirements describe what the app does; nonfunctional ones describe qualities such as speed and reliability. The chapter says nonfunctional requirements “might not be explicitly written down, because they may seem somewhat obvious, but they are just as important as the app’s functionality”."
  - id: polling-cost
    prompt: "With 10 million online users polling every five seconds, how many timeline queries per second result?"
    options:
      - id: ofd6fb9
        text: "2 million"
        correct: true
      - id: o34d6a9
        text: "5,800"
      - id: o142039
        text: "200,000"
      - id: o26af75
        text: "50 million"
    explanation: "“If we assume that 10 million users are online and logged in at the same time, that would mean running the query 2 million times per second.” 10,000,000 ÷ 5 = 2,000,000."
  - id: lookup-cost
    prompt: "Why is each timeline query expensive in the on-demand design?"
    options:
      - id: o29034f
        text: "It fetches recent posts for each of ~200 followed accounts and merges them, giving ~400 million lookups per second overall"
        correct: true
      - id: o0f6941
        text: "It scans every post ever written"
      - id: ob9fc73
        text: "It requires a distributed transaction"
      - id: obea331
        text: "It recomputes follower counts"
    explanation: "“Two million timeline queries per second times 200 followed accounts makes 400 million lookups per second—a huge number. And that’s the average case.”"
  - id: fan-out-def
    prompt: "What does *fan-out* mean in the case study?"
    options:
      - id: oe30321
        text: "The rate of posts per second"
      - id: o8c6b17
        text: "The size of a timeline cache"
      - id: obbf0af
        text: "The factor by which one initial request multiplies into downstream requests"
        correct: true
      - id: ob13da6
        text: "The number of servers in a cluster"
    explanation: "“When one initial request results in several downstream requests being carried out, we use the term fan-out to describe the factor by which the number of requests increases.”"
  - id: fan-out-writes
    prompt: "At 5,800 posts per second and 200 followers per post, roughly how many timeline writes per second does fan-out on write need?"
    options:
      - id: o956f71
        text: "29 million"
      - id: obd522d
        text: "400 million"
      - id: oaa1bae
        text: "Just over 1 million"
        correct: true
      - id: o5085fc
        text: "5,800"
    explanation: "“if the average post reaches 200 followers (i.e., a fan-out factor of 200), we will need to do just over 1 million home timeline writes per second.” 5,800 × 200 = 1.16 million."
  - id: why-better
    prompt: "Why is fan-out on write better here despite doing more work per post?"
    options:
      - id: oaca6c2
        text: "1 million writes/s is far less than 400 million lookups/s because reads vastly outnumber posts"
        correct: true
      - id: o709f5a
        text: "It removes the need to store posts"
      - id: oa4c475
        text: "It guarantees no celebrity problems"
      - id: oa3aded
        text: "Writes are free in relational databases"
    explanation: "“This is a lot, but it’s still a significant saving compared to the 400 million per-sender post lookups per second that we would otherwise have to do.”"
  - id: spike-handling
    prompt: "During a load spike in posts, what does the materialized-timeline design allow?"
    options:
      - id: o407c37
        text: "Switch back to polling"
      - id: o9bece1
        text: "Drop all celebrity posts"
      - id: o90cbd8
        text: "Enqueue deliveries and accept slightly delayed timeline updates while timelines still load fast from cache"
        correct: true
      - id: od41ced
        text: "Reject all posts until the spike ends"
    explanation: "“we don’t have to do the timeline deliveries immediately—we can enqueue them and accept that it will temporarily take a bit longer for posts to show up in followers’ timelines. Even during such load spikes, timelines remain fast to load”."
  - id: materialization
    prompt: "What is the precomputed timeline cache an example of?"
    options:
      - id: o6707f6
        text: "A system of record"
      - id: o78b0a8
        text: "A secondary index on posts"
      - id: o5e91ab
        text: "A write-ahead log"
      - id: o5a4b60
        text: "A materialized view: faster reads in return for more work on writes"
        correct: true
    explanation: "“This process of precomputing and updating the results of a query is called materialization, and the timeline cache is an example of a materialized view” and “The materialized view speeds up reads, but in return we have to do more work on writes.”"
  - id: heavy-follower
    prompt: "A user follows 50,000 very active accounts. Why is it acceptable to drop some writes to their timeline?"
    options:
      - id: oe1d03c
        text: "Their account is probably a bot, so it should be banned"
      - id: o18a98e
        text: "Dropped writes are retried later"
      - id: o39b39e
        text: "Their timeline is computed on demand anyway"
      - id: o6dd886
        text: "They are unlikely to read all those posts, so showing a sample is fine"
        correct: true
    explanation: "“that user is not likely reading all the posts in their timeline, so it’s OK to simply drop some of their timeline writes and show the user only a sample”."
  - id: celebrity
    prompt: "How does the chapter suggest handling posts from celebrities with millions of followers?"
    options:
      - id: o49d725
        text: "Deliver them synchronously before acknowledging the post"
      - id: o280481
        text: "Store them separately and merge them into the materialized timeline at read time"
        correct: true
      - id: o679f63
        text: "Drop some of the deliveries like for heavy followers"
      - id: o9af2e2
        text: "Limit celebrities to one post per day"
    explanation: "“dropping some of those writes is not OK. One way of solving this problem is to handle celebrity posts separately ... by storing them separately and merging them with the materialized timeline when it is read.”"
  - id: push-subscribe
    prompt: "Instead of polling, how can a client learn of new posts with a materialized timeline?"
    options:
      - id: o1a5361
        text: "Download the whole posts table"
      - id: o3ab9f7
        text: "Run the timeline SQL query every second"
      - id: oda961b
        text: "Ask each followed user directly"
      - id: of80268
        text: "Subscribe to the stream of posts being added to its home timeline"
        correct: true
    explanation: "“to receive a notification about any new posts on their timeline, the user’s client simply needs to subscribe to the stream of posts being added to their home timeline.”"
  - id: skew-follows
    prompt: "The case study assumes 200 followers on average. Why is the average misleading for capacity planning?"
    options:
      - id: o77d87e
        text: "Follower counts are extremely skewed: most have a handful, a few have over 100 million, and extremes drive the hardest work"
        correct: true
      - id: o74e173
        text: "Only the median matters for fan-out"
      - id: o9a89f3
        text: "Follower counts are normally distributed"
      - id: od69adc
        text: "Averages are always exact for social graphs"
    explanation: "The chapter notes “there is a very wide range: most people have only a handful of followers, and a few celebrities, such as Barack Obama, have over 100 million followers”, and later that “a social network also has to consider some extreme cases”."
---
The chapter opens with a deliberately simplified X/Twitter timeline. It is the book's first worked example
of trading write cost for read speed.

## Learning objectives

- Estimate the read load of computing timelines on demand
- Explain fan-out and materialized timelines
- Handle the extremes: heavy followers and celebrities

## In a nutshell

- On-demand: 2 million polls/s × 200 followees ≈ 400 million lookups/s.
- Fan-out on write: 5,800 posts/s × 200 followers ≈ 1.2 million timeline writes/s.
- A materialized timeline is derived data: faster reads, more work on writes.
- Celebrities are handled separately and merged at read time; heavy followers can be sampled.

## Connections to today

- **Bluesky and Mastodon.** Federated and decentralized networks face the same fan-out maths, plus
  cross-server delivery.
- **Feeds everywhere.** Notification inboxes, activity feeds and "for you" pages in many apps are
  materialized views kept up to date by queues.
- **Read/write ratio thinking** is the core of most system design interviews — this case study is the
  canonical example.
