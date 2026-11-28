from relen import opt
opt('01-*/01-*.md', "`send()` returning only means the kernel buffered the bytes; only a reply from the remote application proves the record was stored", "A successful `send()` only means the kernel buffered the bytes; only the peer application's reply proves storage")
opt('01-*/03-*.md', "D, because `10.20.32.0/20` contains the address and is the longest matching prefix", "D: `10.20.32.0/20` contains the address and is the longest match")
opt('01-*/05-*.md', "Drops it and sends ICMP Destination Unreachable, code 4, carrying the next-hop MTU of 1400", "Drops it and returns ICMP Destination Unreachable, code 4, with next-hop MTU 1400")
opt('07-*/02-*.md', "1,400; without it, small exchanges work but full-size segments vanish and transfers stall", "1,400; without it, small exchanges work but full-size segments vanish")
