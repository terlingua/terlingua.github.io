const KEYWORDS = {
    csharp: /^(abstract|as|async|await|base|bool|break|byte|case|catch|char|checked|class|closed|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extension|extern|false|field|finally|fixed|float|for|foreach|goto|if|implicit|in|init|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|partial|private|protected|public|readonly|record|ref|required|return|sbyte|scoped|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|union|unsafe|ushort|using|var|virtual|void|volatile|when|where|while|with|yield|and|or|not|global|file|nint|nuint|nameof|value)$/,
    javascript: /^(async|await|break|case|catch|class|const|continue|debugger|default|delete|do|else|export|extends|false|finally|for|from|function|if|import|in|instanceof|let|new|null|of|return|static|super|switch|this|throw|true|try|typeof|undefined|var|void|while|with|yield)$/,
    typescript: /^(abstract|as|async|await|break|case|catch|class|const|continue|debugger|declare|default|delete|do|else|enum|export|extends|false|finally|for|from|function|if|implements|import|in|instanceof|interface|keyof|let|module|namespace|new|null|of|override|readonly|return|satisfies|static|super|switch|this|throw|true|try|type|typeof|undefined|var|void|while|with|yield|infer|never|unknown|any|string|number|boolean|symbol|bigint)$/,
    python: /^(False|None|True|and|as|assert|async|await|break|class|continue|def|del|elif|else|except|finally|for|from|global|if|import|in|is|lambda|nonlocal|not|or|pass|raise|return|try|while|with|yield|self|print)$/,
    sql: /^(SELECT|FROM|WHERE|INSERT|UPDATE|DELETE|CREATE|ALTER|DROP|TABLE|INDEX|VIEW|JOIN|INNER|LEFT|RIGHT|OUTER|CROSS|ON|AND|OR|NOT|NULL|IS|IN|LIKE|BETWEEN|EXISTS|HAVING|GROUP|BY|ORDER|ASC|DESC|LIMIT|OFFSET|UNION|ALL|AS|SET|INTO|VALUES|DISTINCT|COUNT|SUM|AVG|MIN|MAX|CASE|WHEN|THEN|ELSE|END|BEGIN|COMMIT|ROLLBACK|TRANSACTION|PRIMARY|KEY|FOREIGN|REFERENCES|CONSTRAINT|DEFAULT|CHECK|UNIQUE|TOP|GO|USE|EXEC|DECLARE|NVARCHAR|VARCHAR|INT|BIT|DATETIME|IDENTITY|NOLOCK|WITH)$/i,
    bash: /^(if|then|else|elif|fi|for|while|do|done|case|esac|function|return|in|select|until|echo|exit|export|source|alias|set|unset|readonly|local|declare|eval|exec|trap|cd|ls|grep|awk|sed|cat|mkdir|rm|cp|mv|chmod|chown|sudo|apt|dnf|pip|npm|dotnet|git|docker|podman)$/
};

const ALIASES = { cs: 'csharp', ts: 'typescript', js: 'javascript', sh: 'bash', shell: 'bash', zsh: 'bash', yml: 'yaml', xml: 'html' };

const esc = s => s.replaceAll('&', '&amp;').replaceAll('<', '&lt;').replaceAll('>', '&gt;');
const span = (cls, text) => `<span class="hl-${cls}">${esc(text)}</span>`;

function highlightGeneral(code, lang) {
    const kw = KEYWORDS[lang];
    const hashComments = lang === 'python' || lang === 'bash';
    const dashComments = lang === 'sql';
    const n = code.length;
    let out = '';
    let i = 0;

    const identifier = () => {
        let j = i;
        while (j < n && /[a-zA-Z0-9_$]/.test(code[j])) j++;
        if (j === i) {
            out += esc(code[i]);
            i++;
            return;
        }
        const word = code.slice(i, j);
        if (kw?.test(word)) {
            out += span('keyword', word);
        } else if (/^[A-Z][a-zA-Z0-9]*$/.test(word) && word.length > 1 && !/^(False|True|None)$/.test(word)) {
            out += span('type', word);
        } else {
            let k = j;
            while (k < n && code[k] === ' ') k++;
            out += code[k] === '(' ? span('function', word) : esc(word);
        }
        i = j;
    };

    while (i < n) {
        const ch = code[i];

        if ((ch === '/' && code[i + 1] === '/') ||
            (hashComments && ch === '#' && (i === 0 || /\s/.test(code[i - 1]))) ||
            (dashComments && ch === '-' && code[i + 1] === '-')) {
            let end = code.indexOf('\n', i);
            if (end === -1) end = n;
            out += span('comment', code.slice(i, end));
            i = end;
            continue;
        }

        if (ch === '/' && code[i + 1] === '*') {
            const close = code.indexOf('*/', i + 2);
            const end = close === -1 ? n : close + 2;
            out += span('comment', code.slice(i, end));
            i = end;
            continue;
        }

        if (ch === '"' || ch === "'" || ch === '`') {
            if (code[i + 1] === ch && code[i + 2] === ch) {
                const close = code.indexOf(ch.repeat(3), i + 3);
                const end = close === -1 ? n : close + 3;
                out += span('string', code.slice(i, end));
                i = end;
                continue;
            }
            let j = i + 1;
            while (j < n) {
                if (code[j] === '\\') { j += 2; continue; }
                if (code[j] === ch) { j++; break; }
                if (ch !== '`' && code[j] === '\n') break;
                j++;
            }
            out += span('string', code.slice(i, j));
            i = j;
            continue;
        }

        if (lang === 'csharp' && (ch === '@' || ch === '$')) {
            const next = code[i + 1] ?? '';
            const start = i;
            if (next === '"') {
                i += 2;
            } else if ((next === '@' || next === '$') && code[i + 2] === '"') {
                i += 3;
            } else {
                identifier();
                continue;
            }
            let j = i;
            while (j < n) {
                if (code[j] === '"') {
                    if (code[j + 1] === '"') { j += 2; continue; }
                    j++;
                    break;
                }
                j++;
            }
            out += span('string', code.slice(start, j));
            i = j;
            continue;
        }

        if (/\d/.test(ch) && (i === 0 || !/[a-zA-Z_$]/.test(code[i - 1]))) {
            let j = i;
            if (ch === '0' && /[xXbBoO]/.test(code[j + 1] ?? '')) j += 2;
            while (j < n && /[\da-fA-F._eEuUlLfFdDmMnN]/.test(code[j])) j++;
            out += span('number', code.slice(i, j));
            i = j;
            continue;
        }

        if (/[a-zA-Z_$]/.test(ch)) {
            identifier();
            continue;
        }

        out += esc(ch);
        i++;
    }

    return out;
}

function highlightJson(code) {
    const n = code.length;
    let out = '';
    let i = 0;

    while (i < n) {
        if (code[i] === '"') {
            let j = i + 1;
            while (j < n && code[j] !== '"') {
                if (code[j] === '\\') j++;
                j++;
            }
            if (j < n) j++;
            const str = code.slice(i, j);
            let k = j;
            while (k < n && /\s/.test(code[k])) k++;
            out += span(code[k] === ':' ? 'attr' : 'string', str);
            i = j;
            continue;
        }
        if (/\d/.test(code[i]) || (code[i] === '-' && /\d/.test(code[i + 1] ?? ''))) {
            let j = i;
            if (code[j] === '-') j++;
            while (j < n && /[\d.eE+\-]/.test(code[j])) j++;
            out += span('number', code.slice(i, j));
            i = j;
            continue;
        }
        if (/[a-zA-Z]/.test(code[i])) {
            let j = i;
            while (j < n && /[a-zA-Z]/.test(code[j])) j++;
            const word = code.slice(i, j);
            out += /^(true|false|null)$/.test(word) ? span('literal', word) : esc(word);
            i = j;
            continue;
        }
        out += esc(code[i]);
        i++;
    }

    return out;
}

function highlightYamlValue(raw) {
    const val = raw.trim();
    if (!val) return '';
    if (/^(true|false|yes|no|on|off|null|~)$/i.test(val)) return ' ' + span('literal', val);
    if (/^-?\d/.test(val)) return ' ' + span('number', val);
    if (/^["']/.test(val)) return ' ' + span('string', val);
    return ' ' + esc(val);
}

function highlightYaml(code) {
    return code.split('\n').map(line => {
        if (/^\s*#/.test(line)) return span('comment', line);
        const m = line.match(/^(\s*)([\w][\w.-]*\s*)(:)(.*)/);
        return m
            ? esc(m[1]) + span('attr', m[2]) + esc(m[3]) + highlightYamlValue(m[4])
            : esc(line);
    }).join('\n');
}

function highlightHtml(code) {
    const n = code.length;
    let out = '';
    let i = 0;

    const until = (marker, from, extra) => {
        const found = code.indexOf(marker, from);
        return found === -1 ? n : found + extra;
    };

    while (i < n) {
        const ch = code[i];

        if (ch === '<' && code.startsWith('<!--', i)) {
            const end = until('-->', i + 4, 3);
            out += span('comment', code.slice(i, end));
            i = end;
            continue;
        }

        if (ch === '<' && code.startsWith('<![CDATA[', i)) {
            const end = until(']]>', i + 9, 3);
            out += span('comment', code.slice(i, end));
            i = end;
            continue;
        }

        if (ch === '<' && (code[i + 1] === '!' || code[i + 1] === '?')) {
            const end = until('>', i, 1);
            out += span('comment', code.slice(i, end));
            i = end;
            continue;
        }

        if (ch === '<' && i + 1 < n && /[a-zA-Z\/]/.test(code[i + 1])) {
            out += '&lt;';
            i++;

            if (code[i] === '/') {
                out += '/';
                i++;
            }

            const nameStart = i;
            while (i < n && /[\w:-]/.test(code[i])) i++;
            if (i > nameStart) out += span('tag', code.slice(nameStart, i));

            while (i < n && code[i] !== '>') {
                if (/\s/.test(code[i]) || code[i] === '/') {
                    out += code[i];
                    i++;
                    continue;
                }

                if (/[\w:-]/.test(code[i])) {
                    const attrStart = i;
                    while (i < n && /[\w:.-]/.test(code[i])) i++;
                    out += span('attr', code.slice(attrStart, i));

                    let look = i;
                    while (look < n && /\s/.test(code[look])) look++;
                    if (code[look] === '=') {
                        out += code.slice(i, look) + '=';
                        i = look + 1;
                        while (i < n && /\s/.test(code[i])) {
                            out += code[i];
                            i++;
                        }
                        const valStart = i;
                        if (code[i] === '"' || code[i] === "'") {
                            const q = code[i];
                            i++;
                            while (i < n && code[i] !== q) i++;
                            if (i < n) i++;
                        } else {
                            while (i < n && !/\s/.test(code[i]) && code[i] !== '>' && code[i] !== '/') i++;
                        }
                        if (i > valStart) out += span('string', code.slice(valStart, i));
                    }
                    continue;
                }

                out += esc(code[i]);
                i++;
            }

            if (i < n) {
                out += '&gt;';
                i++;
            }
            continue;
        }

        out += esc(ch);
        i++;
    }

    return out;
}

function highlightCss(code) {
    const n = code.length;
    let out = '';
    let i = 0;

    while (i < n) {
        if (code[i] === '/' && code[i + 1] === '*') {
            const close = code.indexOf('*/', i + 2);
            const end = close === -1 ? n : close + 2;
            out += span('comment', code.slice(i, end));
            i = end;
            continue;
        }
        if (code[i] === '"' || code[i] === "'") {
            const q = code[i];
            let j = i + 1;
            while (j < n && code[j] !== q) {
                if (code[j] === '\\') j++;
                j++;
            }
            if (j < n) j++;
            out += span('string', code.slice(i, j));
            i = j;
            continue;
        }
        if (/\d/.test(code[i]) && (i === 0 || !/[a-zA-Z_-]/.test(code[i - 1]))) {
            let j = i;
            while (j < n && /[\d.%a-zA-Z]/.test(code[j])) j++;
            out += span('number', code.slice(i, j));
            i = j;
            continue;
        }
        if (code[i] === '#' && /[0-9a-fA-F]/.test(code[i + 1] ?? '')) {
            let j = i + 1;
            while (j < n && /[0-9a-fA-F]/.test(code[j])) j++;
            out += span('number', code.slice(i, j));
            i = j;
            continue;
        }
        if (/[.#@:[]/.test(code[i]) || (/[a-zA-Z]/.test(code[i]) && (i === 0 || code[i - 1] === '\n' || code[i - 1] === '}'))) {
            let j = i;
            while (j < n && code[j] !== '{' && code[j] !== ';' && code[j] !== '\n') j++;
            if (code[j] === '{') {
                out += span('selector', code.slice(i, j));
                i = j;
                continue;
            }
        }
        if (/[a-zA-Z-]/.test(code[i])) {
            let j = i;
            while (j < n && /[a-zA-Z0-9-]/.test(code[j])) j++;
            const word = code.slice(i, j);
            let k = j;
            while (k < n && code[k] === ' ') k++;
            out += code[k] === ':' && code[k + 1] !== ':' ? span('property', word) : esc(word);
            i = j;
            continue;
        }
        out += esc(code[i]);
        i++;
    }

    return out;
}

export function highlight(code, language) {
    let lang = (language ?? '').toLowerCase().replace(/^language-/, '');
    lang = ALIASES[lang] ?? lang;

    switch (lang) {
        case 'json': return highlightJson(code);
        case 'yaml': return highlightYaml(code);
        case 'html': return highlightHtml(code);
        case 'css': return highlightCss(code);
        default: return highlightGeneral(code, KEYWORDS[lang] ? lang : '');
    }
}

function highlightAll(root) {
    for (const el of root.querySelectorAll('pre code:not([data-highlighted])')) {
        const lang = el.className.match(/language-(\S+)/)?.[1] ?? '';
        el.innerHTML = highlight(el.textContent, lang);
        el.dataset.highlighted = '';
    }
}

function trackScroll(root) {
    for (const wrap of root.querySelectorAll('.code-block-wrap:not([data-scroll-tracked])')) {
        const pre = wrap.querySelector('pre');
        if (!pre) continue;
        wrap.dataset.scrollTracked = '';

        const update = () => {
            const overflows = pre.scrollWidth > pre.clientWidth + 2;
            wrap.classList.toggle('no-overflow', !overflows);
            wrap.classList.toggle('scrolled', pre.scrollLeft > 8);
            wrap.classList.toggle('scrolled-end', pre.scrollLeft + pre.clientWidth >= pre.scrollWidth - 4);
        };

        pre.addEventListener('scroll', update, { passive: true });
        new ResizeObserver(update).observe(pre);
    }
}

export function enhance(root) {
    if (!root) return;
    highlightAll(root);
    trackScroll(root);
}
