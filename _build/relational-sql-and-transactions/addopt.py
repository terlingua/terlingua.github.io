import sys
def add(path, qid, after_line, new_line, expl_append):
    s=open(path).read()
    i=s.index(f'id: "{qid}"'); j=s.index('    explanation: |-', i)
    block=s[i:j]
    assert after_line in block, (qid, after_line)
    s=s[:i]+block.replace(after_line, after_line+"\n"+new_line,1)+s[j:]
    k=s.index('    explanation: |-', i)
    ends=[x for x in (s.find('\n    checks:',k), s.find('\n  - id:',k), s.find('\n---',k)) if x!=-1]
    e=min(ends)
    s=s[:e]+"\n\n      "+expl_append+s[e:]
    open(path,'w').write(s)
