import pathlib, sys, yaml
SRC = pathlib.Path(__file__).parent / "src"
def add(rel, qid, distractor, expl):
    p = next(SRC.glob(rel))
    s = p.read_text()
    i = s.index(f'  - id: "{qid}"')
    j = s.find('\n  - id: "', i + 1)
    if j < 0: j = s.index('\n---', i)
    block = s[i:j]
    k = block.index('    explanation: |-')
    d = block.index('    distractors:\n')
    dl = block[d:k]
    new_dl = dl.rstrip('\n') + '\n      - ' + __import__('json').dumps(distractor, ensure_ascii=False) + '\n'
    block = block[:d] + new_dl + block[k:]
    # append explanation paragraph at end of explanation scalar (before checks: or end)
    e = block.index('    explanation: |-')
    c = block.find('\n    checks:', e)
    end = c if c >= 0 else len(block.rstrip('\n'))
    block = block[:end] + '\n\n      ' + expl + block[end:]
    s = s[:i] + block + s[j:]
    p.write_text(s)
    yaml.safe_load(s[3:s.index('\n---', 3)])
A = [
 ("01-*/03-*.md","network-and-broadcast","`172.16.45.130` and `172.16.45.191`",'"`.130` and `.191`" treats the given address as the network address; a network address has all host bits zero, and 130 = 128 + 2 has a host bit set.'),
 ("02-*/01-*.md","flow-byte-count-bug","Delayed ACKs acknowledge two extra sequence numbers so that the sender can detect the end of the stream","\"Delayed ACKs acknowledge two extra numbers\" invents a rule: delaying an ACK changes when it is sent, never the number it carries."),
 ("02-*/02-*.md","who-gets-time-wait","The client, because it sent the final ACK of the opening handshake",'"Sent the final ACK of the opening handshake" mixes up connection setup with teardown; TIME-WAIT belongs to whoever sends the first FIN.'),
 ("02-*/04-*.md","slow-start-rounds","3",'"3" stops at 80 segments, the window at the start of the fourth round trip.'),
 ("02-*/04-*.md","loss-based-vs-model-based","CUBIC paces at a fixed rate derived from link speed; BBR halves its window on every loss event",'"CUBIC paces at a fixed rate; BBR halves on loss" inverts both designs: CUBIC is window-based and loss-driven, BBR is the one that paces.'),
 ("06-*/02-*.md","post-redirect-303","`GET /orders`, the URL of the original form",'"`GET /orders`, the original URL" ignores the `Location` header, which names the resource to fetch next.'),
 ("06-*/03-*.md","stale-while-revalidate-window","Served fresh; served stale while a background revalidation starts; served stale while a background revalidation starts",'"Served stale at 95 s as well" treats the window as open-ended; it ends 30 seconds after the response became stale, at an age of 90 s.'),
 ("06-*/04-*.md","if-none-match-changed","`200 OK` with the body of version `\"a1\"`, since that is the version the client named",'"`200 OK` with the body of `\\"a1\\"`" misreads the header: `If-None-Match` names versions the client already has; the server always serves its current version.'),
 ("07-*/01-*.md","cold-https-tcp","230 ms",'"230 ms" counts the 20 ms of server processing twice.'),
 ("07-*/01-*.md","slow-start-transfer-time","About 480 ms",'"About 480 ms" starts slow start from one segment (cumulative 1, 3, 7, …, 255 segments), needing eight round trips instead of five.'),
 ("07-*/02-*.md","fixed-buffer-cap","About 33.5 Mbit/s; about 150 MB",'"About 33.5 Mbit/s" divides the window by one second instead of by the 0.12-second RTT.'),
 ("07-*/02-*.md","parallel-streams-clue","Other traffic on the 10 Gbit/s link leaves exactly 280 Mbit/s of spare capacity for each new stream",'"Other traffic leaves 280 Mbit/s per stream" would mean spare capacity grows with the number of streams, which shared capacity cannot do.'),
 ("07-*/03-*.md","capped-backoff-total","5,000 ms",'"5,000 ms" assumes every one of the five waits hits the 1-second cap.'),
]
for a in A: add(*a)
print("added", len(A))
