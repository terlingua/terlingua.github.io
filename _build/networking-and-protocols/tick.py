import pathlib,re
p=pathlib.Path('progress.md'); out=[]
root=pathlib.Path('../../content/books/networking-and-protocols')
for line in p.read_text().splitlines():
    m=re.match(r'- \[ \] (\S+)',line)
    if m and (root/m.group(1)).exists() or (m and m.group(1).endswith('book.yml') and (root/'book.yml').exists()):
        line=line.replace('- [ ]','- [x]',1)
    out.append(line)
p.write_text("\n".join(out)+"\n")
