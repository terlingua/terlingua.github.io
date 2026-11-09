---
title: "Unreliable Networks"
questions:
  - id: no-response
    prompt: "You send a request and get no response. What can you conclude?"
    options:
      - id: odc5838
        text: "The network is partitioned"
      - id: o3f7483
        text: "The request was not received"
      - id: o478db1
        text: "Nothing about why — the request or response may be lost or delayed, or the node crashed or paused"
        correct: true
      - id: o53bb42
        text: "The remote node crashed"
    explanation: "“If you send a request to another node and don’t receive a response, it is impossible to tell why.”"
  - id: timeout-uncertain
    prompt: "After a timeout fires, what do you still not know?"
    options:
      - id: o8e63b5
        text: "Whether the remote node received or processed your request, which may still be delivered later"
        correct: true
      - id: o3bec86
        text: "Whether the timeout fired"
      - id: ofc541a
        text: "Whether TCP was used"
      - id: o7a2142
        text: "The remote node's IP"
    explanation: "“when a timeout occurs, you still don’t know whether the remote node got your request (and if the request is still queued somewhere, it may still be delivered to the recipient”."
  - id: tcp-limits
    prompt: "Which are limits of TCP's \"reliability\"?"
    type: multiple
    options:
      - id: o5158c3
        text: "An ack means the remote kernel received data, not that the application processed it"
        correct: true
      - id: ofb7c30
        text: "Deduplication applies only within one connection, so reconnect-and-retry can duplicate"
        correct: true
      - id: oe7d3a1
        text: "It cannot detect corrupted packets at all"
      - id: obae4b3
        text: "It can't tell whether the outbound packet or the acknowledgment was lost"
        correct: true
    explanation: "The chapter explains each: TCP “can’t tell whether it was the outbound packet or the acknowledgment that was lost”, “TCP’s deduplication and retransmission capabilities apply to only a single connection”, and an ack “means only that the operating system kernel on the remote node received it”."
  - id: app-response
    prompt: "To be sure a request succeeded, what do you need?"
    options:
      - id: oac930e
        text: "A positive response from the application itself"
        correct: true
      - id: oa431eb
        text: "An ICMP reply"
      - id: o15aa6a
        text: "A TCP ACK"
      - id: o0d8972
        text: "A DNS lookup"
    explanation: "“If you want to be sure that a request was successful, you need a positive response from the application itself.”"
  - id: redundant-gear
    prompt: "Why doesn't adding redundant networking gear reduce faults as much as expected?"
    options:
      - id: o2b8576
        text: "Redundant gear fails more often"
      - id: ofc819f
        text: "Redundancy slows the network"
      - id: o64ecda
        text: "It doesn't guard against human error such as misconfigured switches"
        correct: true
      - id: o9184d5
        text: "Switches cannot be redundant"
    explanation: "“adding redundant networking gear doesn’t reduce faults as much as you might expect, since it doesn’t guard against human error (e.g., misconfigured switches), which is a major cause of outages.”"
  - id: asymmetric
    prompt: "Which surprising faults does the chapter describe?"
    type: multiple
    options:
      - id: o4770b5
        text: "Packet delays of over a minute during switch reconfiguration"
        correct: true
      - id: oc42822
        text: "Networks that never drop packets"
      - id: o66aebc
        text: "A and B can talk, B and C can talk, but A and C cannot"
        correct: true
      - id: of2650a
        text: "A NIC that drops inbound packets but sends outbound ones"
        correct: true
    explanation: "The chapter describes partial interruptions, “a network interface that sometimes drops all inbound packets but sends outbound packets successfully”, and “packet delay of more than a minute”."
  - id: handle-not-tolerate
    prompt: "Does handling network faults always mean tolerating them?"
    options:
      - id: obd1e40
        text: "Yes — every fault must be masked"
      - id: ob0d4b4
        text: "Only for databases"
      - id: ob49268
        text: "No — showing an error may be fine, but you must know how the software reacts and ensure it recovers"
        correct: true
      - id: o9990d2
        text: "No — you can ignore them"
    explanation: "“Handling network faults doesn’t necessarily mean tolerating them ... However, you do need to know how your software reacts to network problems and ensure that the system can recover from them.”"
  - id: untested-faults
    prompt: "What can happen if network-fault handling is undefined and untested?"
    options:
      - id: o53456e
        text: "Nothing; networks self-heal"
      - id: o99b7d8
        text: "Arbitrarily bad things, such as a permanently deadlocked cluster or deleted data"
        correct: true
      - id: o51a2b9
        text: "Automatic failover always fixes it"
      - id: oc7807a
        text: "Slightly higher latency only"
    explanation: "“the cluster could become deadlocked and permanently unable to serve requests, even when the network recovers, or it could potentially delete all of your data.”"
  - id: explicit-feedback
    prompt: "Which signals can sometimes tell you quickly that a node is down?"
    type: multiple
    options:
      - id: offe593
        text: "ICMP Destination Unreachable from a router"
        correct: true
      - id: o2379e1
        text: "A script notifying peers when a process crashes but the OS runs"
        correct: true
      - id: ofa30fe
        text: "Silence from the node"
      - id: o4fed3b
        text: "TCP RST/FIN when no process listens on the port"
        correct: true
    explanation: "The chapter lists RST/FIN, crash notification scripts (as in HBase), switch management interfaces, and ICMP. But “you can’t count on it ... in general you have to assume that you will get no response at all.”"
  - id: timeout-tradeoff
    prompt: "Why is declaring a node dead too early harmful?"
    options:
      - id: od49272
        text: "It speeds up recovery"
      - id: oea2e7b
        text: "Its action may be performed twice, and moving its load can overload others into a cascading failure"
        correct: true
      - id: off7eeb
        text: "It disables replication"
      - id: oa0c2e2
        text: "It deletes the node's data"
    explanation: "“If the node is actually alive and in the middle of performing an action ... the action may end up being performed twice” and “transferring its load to other nodes can cause a cascading failure”."
  - id: bounded-delay
    prompt: "If packets were delivered within d and requests handled within r, what timeout would be reasonable?"
    options:
      - id: o8a5237
        text: "r"
      - id: o6926bb
        text: "d + r / 2"
      - id: o3f6ab7
        text: "2d + r"
        correct: true
      - id: o5251b9
        text: "d"
    explanation: "“you could guarantee that every successful request receives a response within time 2d + r”. Real systems have neither guarantee."
  - id: queueing-sources
    prompt: "Which are sources of variable network delay?"
    type: multiple
    options:
      - id: o0a7db6
        text: "OS queueing when CPUs or threads are busy"
        correct: true
      - id: o74393e
        text: "Switch queues on congested links"
        correct: true
      - id: oec7763
        text: "TCP's own rate limiting at the sender"
        correct: true
      - id: ocad014
        text: "Hypervisor pausing a VM while another uses the core"
        correct: true
      - id: oc90ef0
        text: "The speed of light changing"
    explanation: "All four are listed; “the variability of packet delays on computer networks is most often due to queueing”."
  - id: udp-voip
    prompt: "Why do VoIP and video calls often use UDP?"
    options:
      - id: oa1d315
        text: "UDP has lower bandwidth costs"
      - id: o3ffa37
        text: "UDP guarantees delivery"
      - id: o2c0678
        text: "Delayed data is worthless, so retransmitting lost packets has no point"
        correct: true
      - id: oe5375d
        text: "UDP is encrypted"
    explanation: "“UDP is a good choice when delayed data is worthless ... The retry happens at the human layer instead.”"
  - id: phi-accrual
    prompt: "How does a Phi Accrual failure detector improve on fixed timeouts?"
    options:
      - id: o5a1881
        text: "It continuously measures response-time distribution and jitter and adapts the timeout"
        correct: true
      - id: o5db69e
        text: "It never declares nodes dead"
      - id: of501d3
        text: "It requires circuit switching"
      - id: o32e0db
        text: "It uses GPS clocks"
    explanation: "“systems can continually measure response times and their variability (jitter) and automatically adjust timeouts according to the observed response time distribution. The Phi Accrual failure detector ... is one way of doing this.”"
  - id: circuit
    prompt: "Why is a telephone circuit's delay bounded?"
    options:
      - id: oa773f8
        text: "Calls use TCP"
      - id: o6af909
        text: "Bandwidth is reserved along the whole route, so there is no queueing"
        correct: true
      - id: o8ac4f5
        text: "Calls are compressed"
      - id: oeee6f6
        text: "Telephone wires are faster than fibre"
    explanation: "“even as data passes through several routers, it does not suffer from queueing, because the 16 bits of space for the call have already been reserved in the next hop ... We call this a bounded delay.”"
  - id: why-packet
    prompt: "Why do datacenters and the internet use packet switching instead of circuits?"
    options:
      - id: o63dd71
        text: "Packet switching has bounded delay"
      - id: o382dc4
        text: "They're optimized for bursty traffic where you want transfers to finish as fast as possible"
        correct: true
      - id: o89239f
        text: "Circuits can't carry data"
      - id: oee7773
        text: "Circuits are illegal"
    explanation: "“The answer is that they are optimized for bursty traffic ... TCP dynamically adapts the rate of data transfer to the available network capacity.”"
  - id: utilization
    prompt: "What trade-off underlies variable network delay?"
    options:
      - id: o18c408
        text: "Encryption versus speed"
      - id: o765659
        text: "Faster hardware versus slower software"
      - id: o06c9ea
        text: "IPv4 versus IPv6"
      - id: odce831
        text: "Dynamic resource sharing maximizes utilization (cheaper) at the cost of queueing; static partitioning gives guarantees at higher cost"
        correct: true
    explanation: "“Variable delays in networks are not a law of nature but simply the result of a cost/benefit trade-off.”"
  - id: qos
    prompt: "Can you get latency guarantees in public clouds today with QoS mechanisms?"
    options:
      - id: o47013c
        text: "Only with UDP"
      - id: o07ba06
        text: "Yes — by default"
      - id: o57160b
        text: "No — such mechanisms aren't enabled in multitenant clouds or over the internet, so timeouts must be set experimentally"
        correct: true
      - id: o33683b
        text: "Only with IPv6"
    explanation: "“Such QoS mechanisms, however, are not currently enabled in multitenant datacenters and public clouds, or when communicating via the internet ... there’s no “correct” value for timeouts—they need to be determined experimentally.”"
---
In asynchronous packet networks, a missing response can mean many things. TCP helps but does not solve
this; timeouts are a guess.

## Learning objectives

- List what can go wrong with a request and why the causes are indistinguishable
- Explain what TCP does and doesn't guarantee
- Weigh short versus long timeouts and why delays are unbounded
- Explain why datacenters use packet switching rather than circuits

## In a nutshell

- Lost request, queued request, crashed node, paused node, lost response, delayed response — all look the same.
- TCP's guarantees end at the connection and the kernel; you need an application-level response.
- Network faults are common, even inside one datacenter.
- Queueing (switches, OS, hypervisor, TCP) causes most delay variability.
- Variable delay is a cost/benefit choice for higher utilization.
- Adaptive failure detectors (Phi Accrual) beat fixed timeouts.

## Connections to today

- **QUIC/HTTP/3** — mentioned in the chapter — shares TCP's limitations at this level.
- **Noisy neighbours** in shared cloud networks are why p99.9 network latencies are so much worse than
  medians.
- **AWS and Azure partial network events**, where some zones can't reach others, are the chapter's
  "A can talk to B, B to C, but not A to C" in real life.
