---
title: "Connection Teardown, TIME_WAIT and CLOSE_WAIT"
questions:
  - id: "who-gets-time-wait"
    type: "single"
    prompt: "An HTTP/1.0-style server sends its response and then closes the connection immediately; the client closes after reading to end of stream. Which side ends up in TIME_WAIT?"
    explanation: |-
      In RFC 9293's state machine, the side that sends the first FIN (the *active closer*) moves through FIN-WAIT-1 and FIN-WAIT-2, receives the peer's FIN, sends the final ACK, and enters TIME-WAIT for 2 × MSL. The *passive closer* goes through CLOSE-WAIT and LAST-ACK and reaches CLOSED as soon as its FIN is acknowledged. Here the server closes first, so the server holds TIME_WAIT.

      "The client, because it receives the first FIN" reverses the roles; receiving the first FIN leads to CLOSE-WAIT, not TIME-WAIT.

      "Both sides" is wrong because only the active closer can be left wondering whether its final ACK arrived; the passive closer learns that its FIN was acknowledged.

      "Only follows a reset" inverts it: a RST tears the connection down immediately, with no TIME-WAIT.

      "Sent the final ACK of the opening handshake" mixes up connection setup with teardown; TIME-WAIT belongs to whoever sends the first FIN.
    options:
      - text: "The client, because it receives the first FIN and must wait for stray segments"
      - text: "Both sides, each holding the connection for 2 × MSL after its own FIN"
      - text: "Neither side, because TIME_WAIT only follows a connection reset"
      - text: "The server, because it sent the first FIN and so performed the active close"
        correct: true
      - text: "The client, because it sent the final ACK of the opening handshake"
  - id: "time-wait-reasons"
    type: "multiple"
    prompt: "Why does the active closer stay in TIME-WAIT for twice the maximum segment lifetime? Select all that apply."
    explanation: |-
      TIME-WAIT exists for two reasons that both follow from IP being unreliable.

      - The final ACK may be lost. The peer then retransmits its FIN; a TCP that had already forgotten the connection would answer with a RST, and the peer would report an error for a connection that closed cleanly. Staying around lets it re-ACK.
      - Old duplicates may still be in flight. MSL bounds how long a segment can live in the network; waiting 2 × MSL (one MSL for a segment to arrive and one for its response) ensures none of them is accepted by a new connection with the same addresses and ports.

      "Time to read the receive buffer" is unrelated; the application has already closed the socket by then.

      "Keep a listening port reserved" confuses TIME-WAIT with the listen socket; a listening socket does not need it, and `SO_REUSEADDR` exists precisely so restarts can bind despite TIME-WAIT connections.

      "Wait for an ACK of the final ACK" describes something TCP never does; ACKs are not acknowledged.
    options:
      - text: "To let delayed segments of this connection expire before the same 4-tuple can be reused"
        correct: true
      - text: "To give the application time to read data still waiting in its receive buffer"
      - text: "To keep a listening socket's port reserved so it can accept new connections"
      - text: "To wait for the peer's delayed ACK of the final ACK before releasing memory"
      - text: "To resend the final ACK if the peer's FIN is retransmitted because that ACK was lost"
        correct: true
  - id: "time-wait-exhaustion"
    type: "single"
    prompt: "A client opens 500 short TCP connections per second to one database at `10.0.0.9:5432` and closes each one first. Its TIME_WAIT lasts 60 seconds and its ephemeral port range is 32768–60999. What happens in steady state?"
    explanation: |-
      Sockets in TIME_WAIT = close rate × TIME_WAIT duration = 500 × 60 = 30,000. Each keeps its local port bound for the 4-tuple (client IP, port, `10.0.0.9`, 5432). The client can use only 60999 − 32768 + 1 = 28,232 local ports for that destination, so once they are all in use, `connect()` fails with "address not available" (`EADDRNOTAVAIL` on Linux). The sustainable rate is 28,232 / 60 ≈ 470 connections per second.

      "Freed once its final ACK is delivered" ignores that TIME-WAIT is a timer; the closer cannot know the ACK arrived.

      "60,000" doubles the duration: the stated 60 seconds is already the full TIME_WAIT length, not the MSL.

      "64,512 ports above 1023" assumes the kernel picks from all non-privileged ports; outgoing connections use the configured ephemeral range.

      Fixes: reuse connections with a pool; let the server close first so TIME_WAIT lands on the server, which has no per-destination port limit for accepted connections; or widen the ephemeral range.
    options:
      - text: "About 500 sockets sit in TIME_WAIT, because each one is freed once its final ACK is delivered"
      - text: "About 60,000 sockets sit in TIME_WAIT, because TIME_WAIT lasts 2 × MSL with an MSL of 60 seconds"
      - text: "About 30,000 sockets sit in TIME_WAIT, more than the 28,232 ports, so connects start failing"
        correct: true
      - text: "About 30,000 sockets sit in TIME_WAIT, well under the 64,512 ports above 1023, so nothing fails"
  - id: "close-wait-leak"
    type: "single"
    prompt: "`ss -tan` on an API server shows thousands of sockets in `CLOSE-WAIT`, and the number grows all day until the process runs out of file descriptors. What does this indicate?"
    explanation: |-
      CLOSE-WAIT means "the peer has sent FIN; waiting for the local application to close". The kernel cannot leave this state on its own, because only the application knows whether it still wants to send data (a half-closed connection is legal). A growing CLOSE-WAIT count is therefore an application bug: some code path reads end-of-stream (or gets an error) and never calls `close()` or disposes the socket, typically a missing `finally` or `using` on an error path.

      "Holding them for 2 × MSL" describes TIME-WAIT, which is on the active closer's side and expires by itself.

      "Final ACKs lost" would affect LAST-ACK, the state *after* the application closes; keepalive probes do not move a socket out of CLOSE-WAIT.

      "The listen backlog is full" concerns connections not yet accepted; CLOSE-WAIT sockets were accepted long ago.
    options:
      - text: "The kernel is holding them for 2 × MSL and they will drain on their own after the timer expires"
      - text: "The peers' final ACKs are being lost on the network; enabling TCP keepalive will release the sockets"
      - text: "The listen backlog is full; raising `somaxconn` will move these sockets into the accept queue"
      - text: "The peers closed their side, but the application never closes its sockets; it is leaking them"
        correct: true
  - id: "linger-zero-reset"
    type: "single"
    prompt: "A client sets `SO_LINGER` with a timeout of 0 and then calls `close()` while 10 KB of data is still unsent in its send buffer. What happens?"
    explanation: |-
      A linger timeout of zero requests an *abortive* close. The stack discards the send buffer and sends RST instead of FIN. RFC 9293 treats a RST as "this connection is gone": the peer drops it immediately and reports a reset (`ECONNRESET`) to its application, and any data it had not read may be lost. Because there is no FIN handshake, the client does not enter TIME-WAIT, which is why the option is sometimes misused to avoid TIME_WAIT at the cost of correctness.

      "Flushes and sends a FIN" is the default graceful `close()` (linger off), not linger with 0.

      "Sends a FIN at once and discards unacknowledged bytes" mixes the two: FIN means everything before it is delivered reliably.

      "Blocks until acknowledged" is the behavior of a *non-zero* linger timeout, which waits up to that long.
    options:
      - text: "It sends a RST: unsent data is discarded, the peer sees a reset error, and the client skips TIME_WAIT"
        correct: true
      - text: "It flushes the 10 KB, sends a FIN, and then enters TIME_WAIT as the active closer"
      - text: "It sends a FIN at once and discards only the bytes that the peer has not yet acknowledged"
      - text: "It blocks in `close()` until the peer has acknowledged all 10 KB of the buffered data"
  - id: "half-close-state"
    type: "single"
    prompt: "A client sends its request and calls `shutdown(SHUT_WR)`. The server reads to end of stream, sends a 2 MB response, and then closes. In which TCP state is the client while it receives the response?"
    explanation: |-
      `shutdown(SHUT_WR)` sends a FIN for the client's direction only (a half-close). Trace:

      1. Client sends FIN → FIN-WAIT-1.
      2. Server ACKs the FIN → client moves to FIN-WAIT-2. The server is now in CLOSE-WAIT and may keep sending.
      3. The 2 MB response flows to the client, which is still in FIN-WAIT-2 and receives normally.
      4. Server closes → its FIN arrives → client ACKs and enters TIME-WAIT.

      "CLOSE-WAIT" is the server's state during the response: it has received a FIN but not yet closed.

      "TIME-WAIT" comes only after the server's FIN arrives, at the end of the response.

      "LAST-ACK" is the server's state after it sends its own FIN, while it waits for the final ACK.
    options:
      - text: "CLOSE-WAIT"
      - text: "FIN-WAIT-2"
        correct: true
      - text: "TIME-WAIT"
      - text: "LAST-ACK"
---
## Learning objectives

- Walk the close sequence through FIN-WAIT, CLOSE-WAIT, LAST-ACK and TIME-WAIT.
- Explain the two reasons for TIME-WAIT and its 2 × MSL length.
- Compute when TIME_WAIT sockets exhaust a client's ports.
- Tell a CLOSE_WAIT leak from normal TIME_WAIT, and RST from FIN.

## The invariant

TCP connections close one direction at a time. Sending a FIN says "I will send no more data"; the peer can keep sending until it sends its own FIN. RFC 9293's state machine follows from that:

| Active closer (sends FIN first) | Passive closer |
|---|---|
| ESTABLISHED → FIN-WAIT-1 (FIN sent) | ESTABLISHED → CLOSE-WAIT (FIN received) |
| FIN-WAIT-1 → FIN-WAIT-2 (its FIN ACKed) | CLOSE-WAIT → LAST-ACK (application closes, FIN sent) |
| FIN-WAIT-2 → TIME-WAIT (peer's FIN received, ACK sent) | LAST-ACK → CLOSED (FIN ACKed) |
| TIME-WAIT → CLOSED after 2 × MSL | |

Two states cause most production incidents:

- **TIME-WAIT** belongs to the active closer and lasts twice the maximum segment lifetime. It exists because the final ACK might be lost (the peer will retransmit its FIN, and someone must re-ACK it) and because delayed segments of this connection must die before the same 4-tuple carries a new connection. RFC 9293 keeps the historical MSL of 2 minutes, so the specified TIME-WAIT is 4 minutes; Linux has long used a fixed 60 seconds. TIME-WAIT is normal and expires by itself.
- **CLOSE-WAIT** belongs to the passive closer and lasts until the *application* closes the socket. The kernel cannot end it, because a half-closed connection may still send. A growing CLOSE-WAIT count is always an application that forgets to close.

A **RST** is different from a FIN. It aborts the connection: buffered data is discarded, the peer's next operation fails with a reset error, and there is no TIME-WAIT. Setting `SO_LINGER` to zero turns `close()` into a RST.

Each TIME_WAIT socket pins its 4-tuple. On a client that opens many short connections to one server, sockets in TIME_WAIT = close rate × TIME_WAIT duration, and they draw on the client's ephemeral port range for that destination. On a server, accepted connections all share the server's port, so TIME_WAIT on the server consumes memory but not ports.

## Worked example

A batch job connects to `10.0.0.9:5432` for every query, 500 times per second, and closes first. TIME_WAIT is 60 seconds and the ephemeral range is 32768–60999.

| Quantity | Value |
|---|---|
| Ports for this destination | 60999 − 32768 + 1 = 28,232 |
| TIME_WAIT sockets in steady state | 500 × 60 = 30,000 |
| Sustainable close rate | 28,232 / 60 ≈ 470.5 per second |

The job fails with "cannot assign requested address" once the range is used up, even though the database is idle. What the states look like:

```bash
$ ss -tan state time-wait dst 10.0.0.9 | wc -l
28233
$ ss -tan state close-wait | head -3
Recv-Q Send-Q   Local Address:Port    Peer Address:Port
0      0        10.0.0.4:8080         10.0.0.7:51234
0      0        10.0.0.4:8080         10.0.0.7:51240
```

The first count (including its header line) is the TIME_WAIT pile-up; the second listing would be a CLOSE_WAIT leak if it kept growing. A connection pool fixes the first; a `finally`/`using` around socket use fixes the second.

## Where this shows up today

As of November 2026, RFC 9293 (STD 7, 2022) is still the TCP specification that defines these states and the 2 × MSL rule. HTTP clients that pool connections by default make the TIME_WAIT problem rare in steady traffic, but it returns whenever a client is constructed per request or a proxy disables keep-alive. Kernel options such as Linux's `tcp_tw_reuse` let a new outgoing connection take over a TIME_WAIT 4-tuple when TCP timestamps prove that old segments cannot be confused with new ones; they ease the symptom without removing the arithmetic.

## Pitfalls checklist

- Opening and closing a connection per request against one server.
- Using `SO_LINGER` 0 to "fix" TIME_WAIT and silently losing data with resets.
- Missing `close()` on error paths, leaving sockets in CLOSE-WAIT forever.
- Assuming TIME_WAIT is a server-only issue; on clients it consumes ports.
- Mistaking `SO_REUSEADDR` (bind despite TIME_WAIT) for a fix for outbound port exhaustion.

## Key takeaways

- Each side closes its own direction with a FIN; the first closer ends in TIME-WAIT.
- TIME-WAIT protects the final ACK and keeps old segments out of new connections.
- TIME_WAIT sockets × ports per destination is a capacity limit for clients.
- CLOSE-WAIT is the application's responsibility; RST aborts and discards data.

## Sources

- RFC 9293, Transmission Control Protocol (state diagram, TIME-WAIT, RST processing).
- RFC 1122 §4.2.2.13 (closing a connection).
