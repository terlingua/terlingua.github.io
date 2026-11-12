using System.Collections.Frozen;
using System.Text;

namespace ObserverMagazine.ContentProcessor;

public static class SyntaxHighlighter
{
    private static readonly FrozenDictionary<string, string> Aliases = new Dictionary<string, string>
    {
        ["cs"] = "csharp",
        ["ts"] = "typescript",
        ["js"] = "javascript",
        ["sh"] = "bash",
        ["shell"] = "bash",
        ["zsh"] = "bash",
        ["yml"] = "yaml",
        ["xml"] = "html"
    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<string, FrozenSet<string>> Keywords = new Dictionary<string, FrozenSet<string>>
    {
        ["csharp"] = Set("abstract as async await base bool break byte case catch char checked class closed const continue decimal default delegate do double else enum event explicit extension extern false field finally fixed float for foreach goto if implicit in init int interface internal is lock long namespace new null object operator out override params partial private protected public readonly record ref required return sbyte scoped sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked union unsafe ushort using var virtual void volatile when where while with yield and or not global file nint nuint nameof value"),
        ["javascript"] = Set("async await break case catch class const continue debugger default delete do else export extends false finally for from function if import in instanceof let new null of return static super switch this throw true try typeof undefined var void while with yield"),
        ["typescript"] = Set("abstract as async await break case catch class const continue debugger declare default delete do else enum export extends false finally for from function if implements import in instanceof interface keyof let module namespace new null of override readonly return satisfies static super switch this throw true try type typeof undefined var void while with yield infer never unknown any string number boolean symbol bigint"),
        ["python"] = Set("False None True and as assert async await break class continue def del elif else except finally for from global if import in is lambda nonlocal not or pass raise return try while with yield self print"),
        ["sql"] = Set("SELECT FROM WHERE INSERT UPDATE DELETE CREATE ALTER DROP TABLE INDEX VIEW JOIN INNER LEFT RIGHT OUTER CROSS ON AND OR NOT NULL IS IN LIKE BETWEEN EXISTS HAVING GROUP BY ORDER ASC DESC LIMIT OFFSET UNION ALL AS SET INTO VALUES DISTINCT COUNT SUM AVG MIN MAX CASE WHEN THEN ELSE END BEGIN COMMIT ROLLBACK TRANSACTION PRIMARY KEY FOREIGN REFERENCES CONSTRAINT DEFAULT CHECK UNIQUE TOP GO USE EXEC DECLARE NVARCHAR VARCHAR INT BIT DATETIME IDENTITY NOLOCK WITH", StringComparer.OrdinalIgnoreCase),
        ["bash"] = Set("if then else elif fi for while do done case esac function return in select until echo exit export source alias set unset readonly local declare eval exec trap cd ls grep awk sed cat mkdir rm cp mv chmod chown sudo apt dnf pip npm dotnet git docker podman")
    }.ToFrozenDictionary();

    private static readonly FrozenSet<string> YamlLiterals = Set("true false yes no on off null ~", StringComparer.OrdinalIgnoreCase);

    public static string Highlight(string code, string? language)
    {
        var lang = (language ?? "").Trim().ToLowerInvariant();
        if (lang.StartsWith("language-", StringComparison.Ordinal))
        {
            lang = lang["language-".Length..];
        }
        lang = Aliases.GetValueOrDefault(lang, lang);

        return lang switch
        {
            "json" => HighlightJson(code),
            "yaml" => HighlightYaml(code),
            "html" => HighlightHtml(code),
            "css" => HighlightCss(code),
            _ => HighlightGeneral(code, Keywords.ContainsKey(lang) ? lang : "")
        };
    }

    private static FrozenSet<string> Set(string words, StringComparer? comparer = null) =>
        words.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToFrozenSet(comparer ?? StringComparer.Ordinal);

    private static bool IsAsciiLetter(char c) => char.IsAsciiLetter(c);
    private static bool IsIdentStart(char c) => char.IsAsciiLetter(c) || c is '_' or '$';
    private static bool IsIdentPart(char c) => char.IsAsciiLetterOrDigit(c) || c is '_' or '$';
    private static bool IsWordChar(char c) => char.IsAsciiLetterOrDigit(c) || c == '_';

    private static char At(string s, int index) => index >= 0 && index < s.Length ? s[index] : '\0';

    private static void Escape(StringBuilder sb, ReadOnlySpan<char> text)
    {
        foreach (var c in text)
        {
            switch (c)
            {
                case '&': sb.Append("&amp;"); break;
                case '<': sb.Append("&lt;"); break;
                case '>': sb.Append("&gt;"); break;
                default: sb.Append(c); break;
            }
        }
    }

    private static void Wrap(StringBuilder sb, string cls, ReadOnlySpan<char> text)
    {
        sb.Append("<span class=\"hl-").Append(cls).Append("\">");
        Escape(sb, text);
        sb.Append("</span>");
    }

    private static int EndOf(string code, string marker, int from, int extra)
    {
        var found = code.IndexOf(marker, from, StringComparison.Ordinal);
        return found < 0 ? code.Length : found + extra;
    }

    private static string HighlightGeneral(string code, string lang)
    {
        var keywords = Keywords.GetValueOrDefault(lang);
        var hashComments = lang is "python" or "bash";
        var dashComments = lang == "sql";
        var n = code.Length;
        var sb = new StringBuilder(code.Length * 2);
        var i = 0;

        while (i < n)
        {
            var ch = code[i];

            if ((ch == '/' && At(code, i + 1) == '/') ||
                (hashComments && ch == '#' && (i == 0 || char.IsWhiteSpace(code[i - 1]))) ||
                (dashComments && ch == '-' && At(code, i + 1) == '-'))
            {
                var end = code.IndexOf('\n', i);
                if (end < 0) end = n;
                Wrap(sb, "comment", code.AsSpan(i, end - i));
                i = end;
                continue;
            }

            if (ch == '/' && At(code, i + 1) == '*')
            {
                var end = EndOf(code, "*/", i + 2, 2);
                Wrap(sb, "comment", code.AsSpan(i, end - i));
                i = end;
                continue;
            }

            if (ch is '"' or '\'' or '`')
            {
                if (At(code, i + 1) == ch && At(code, i + 2) == ch)
                {
                    var end = EndOf(code, new string(ch, 3), i + 3, 3);
                    Wrap(sb, "string", code.AsSpan(i, end - i));
                    i = end;
                    continue;
                }

                var j = i + 1;
                while (j < n)
                {
                    if (code[j] == '\\') { j += 2; continue; }
                    if (code[j] == ch) { j++; break; }
                    if (ch != '`' && code[j] == '\n') break;
                    j++;
                }
                j = Math.Min(j, n);
                Wrap(sb, "string", code.AsSpan(i, j - i));
                i = j;
                continue;
            }

            if (lang == "csharp" && ch is '@' or '$')
            {
                var next = At(code, i + 1);
                var start = i;
                var prefix = next == '"' ? 2 : (next is '@' or '$') && At(code, i + 2) == '"' ? 3 : 0;
                if (prefix == 0)
                {
                    i = Identifier(code, i, keywords, sb);
                    continue;
                }

                var j = i + prefix;
                while (j < n)
                {
                    if (code[j] == '"')
                    {
                        if (At(code, j + 1) == '"') { j += 2; continue; }
                        j++;
                        break;
                    }
                    j++;
                }
                j = Math.Min(j, n);
                Wrap(sb, "string", code.AsSpan(start, j - start));
                i = j;
                continue;
            }

            if (char.IsAsciiDigit(ch) && (i == 0 || !IsIdentStart(code[i - 1])))
            {
                var j = i;
                if (ch == '0' && At(code, j + 1) is 'x' or 'X' or 'b' or 'B' or 'o' or 'O') j += 2;
                while (j < n && (char.IsAsciiHexDigit(code[j]) || code[j] is '.' or '_' or 'u' or 'U' or 'l' or 'L' or 'm' or 'M' or 'n' or 'N')) j++;
                j = Math.Min(j, n);
                Wrap(sb, "number", code.AsSpan(i, j - i));
                i = j;
                continue;
            }

            if (IsIdentStart(ch))
            {
                i = Identifier(code, i, keywords, sb);
                continue;
            }

            Escape(sb, [ch]);
            i++;
        }

        return sb.ToString();
    }

    private static int Identifier(string code, int i, FrozenSet<string>? keywords, StringBuilder sb)
    {
        var n = code.Length;
        var j = i;
        while (j < n && IsIdentPart(code[j])) j++;
        if (j == i)
        {
            Escape(sb, [code[i]]);
            return i + 1;
        }

        var word = code[i..j];
        if (keywords?.Contains(word) == true)
        {
            Wrap(sb, "keyword", word);
        }
        else if (word.Length > 1 && char.IsAsciiLetterUpper(word[0]) && word.All(char.IsAsciiLetterOrDigit) && word is not ("False" or "True" or "None"))
        {
            Wrap(sb, "type", word);
        }
        else
        {
            var k = j;
            while (k < n && code[k] == ' ') k++;
            if (At(code, k) == '(')
            {
                Wrap(sb, "function", word);
            }
            else
            {
                Escape(sb, word);
            }
        }

        return j;
    }

    private static string HighlightJson(string code)
    {
        var n = code.Length;
        var sb = new StringBuilder(code.Length * 2);
        var i = 0;

        while (i < n)
        {
            var ch = code[i];
            if (ch == '"')
            {
                var j = i + 1;
                while (j < n && code[j] != '"')
                {
                    if (code[j] == '\\') j++;
                    j++;
                }
                if (j < n) j++;
                j = Math.Min(j, n);
                var k = j;
                while (k < n && char.IsWhiteSpace(code[k])) k++;
                Wrap(sb, At(code, k) == ':' ? "attr" : "string", code.AsSpan(i, j - i));
                i = j;
                continue;
            }

            if (char.IsAsciiDigit(ch) || (ch == '-' && char.IsAsciiDigit(At(code, i + 1))))
            {
                var j = i;
                if (code[j] == '-') j++;
                while (j < n && (char.IsAsciiDigit(code[j]) || code[j] is '.' or 'e' or 'E' or '+' or '-')) j++;
                Wrap(sb, "number", code.AsSpan(i, j - i));
                i = j;
                continue;
            }

            if (IsAsciiLetter(ch))
            {
                var j = i;
                while (j < n && IsAsciiLetter(code[j])) j++;
                var word = code.AsSpan(i, j - i);
                if (word is "true" or "false" or "null")
                {
                    Wrap(sb, "literal", word);
                }
                else
                {
                    Escape(sb, word);
                }
                i = j;
                continue;
            }

            Escape(sb, [ch]);
            i++;
        }

        return sb.ToString();
    }

    private static string HighlightYaml(string code)
    {
        var sb = new StringBuilder(code.Length * 2);
        var lines = code.Split('\n');

        for (var index = 0; index < lines.Length; index++)
        {
            if (index > 0) sb.Append('\n');
            var line = lines[index];

            if (line.TrimStart().StartsWith('#'))
            {
                Wrap(sb, "comment", line);
                continue;
            }

            var indent = 0;
            while (indent < line.Length && char.IsWhiteSpace(line[indent])) indent++;

            var keyEnd = indent;
            if (keyEnd < line.Length && IsWordChar(line[keyEnd]))
            {
                keyEnd++;
                while (keyEnd < line.Length && (IsWordChar(line[keyEnd]) || line[keyEnd] is '.' or '-')) keyEnd++;
            }

            var colon = keyEnd;
            while (colon < line.Length && char.IsWhiteSpace(line[colon])) colon++;

            if (keyEnd == indent || colon >= line.Length || line[colon] != ':')
            {
                Escape(sb, line);
                continue;
            }

            Escape(sb, line.AsSpan(0, indent));
            Wrap(sb, "attr", line.AsSpan(indent, colon - indent));
            sb.Append(':');

            var value = line[(colon + 1)..].Trim();
            if (value.Length == 0) continue;

            sb.Append(' ');
            if (YamlLiterals.Contains(value))
            {
                Wrap(sb, "literal", value);
            }
            else if (char.IsAsciiDigit(value[0]) || (value[0] == '-' && value.Length > 1 && char.IsAsciiDigit(value[1])))
            {
                Wrap(sb, "number", value);
            }
            else if (value[0] is '"' or '\'')
            {
                Wrap(sb, "string", value);
            }
            else
            {
                Escape(sb, value);
            }
        }

        return sb.ToString();
    }

    private static string HighlightHtml(string code)
    {
        var n = code.Length;
        var sb = new StringBuilder(code.Length * 2);
        var i = 0;

        while (i < n)
        {
            var ch = code[i];

            if (ch == '<' && code.AsSpan(i).StartsWith("<!--"))
            {
                var end = EndOf(code, "-->", i + 4, 3);
                Wrap(sb, "comment", code.AsSpan(i, end - i));
                i = end;
                continue;
            }

            if (ch == '<' && code.AsSpan(i).StartsWith("<![CDATA["))
            {
                var end = EndOf(code, "]]>", i + 9, 3);
                Wrap(sb, "comment", code.AsSpan(i, end - i));
                i = end;
                continue;
            }

            if (ch == '<' && At(code, i + 1) is '!' or '?')
            {
                var end = EndOf(code, ">", i, 1);
                Wrap(sb, "comment", code.AsSpan(i, end - i));
                i = end;
                continue;
            }

            if (ch == '<' && i + 1 < n && (IsAsciiLetter(code[i + 1]) || code[i + 1] == '/'))
            {
                sb.Append("&lt;");
                i++;

                if (code[i] == '/')
                {
                    sb.Append('/');
                    i++;
                }

                var nameStart = i;
                while (i < n && (IsWordChar(code[i]) || code[i] is ':' or '-')) i++;
                if (i > nameStart) Wrap(sb, "tag", code.AsSpan(nameStart, i - nameStart));

                while (i < n && code[i] != '>')
                {
                    if (char.IsWhiteSpace(code[i]) || code[i] == '/')
                    {
                        sb.Append(code[i]);
                        i++;
                        continue;
                    }

                    if (IsWordChar(code[i]) || code[i] is ':' or '-')
                    {
                        var attrStart = i;
                        while (i < n && (IsWordChar(code[i]) || code[i] is ':' or '.' or '-')) i++;
                        Wrap(sb, "attr", code.AsSpan(attrStart, i - attrStart));

                        var look = i;
                        while (look < n && char.IsWhiteSpace(code[look])) look++;
                        if (At(code, look) == '=')
                        {
                            sb.Append(code.AsSpan(i, look - i)).Append('=');
                            i = look + 1;
                            while (i < n && char.IsWhiteSpace(code[i]))
                            {
                                sb.Append(code[i]);
                                i++;
                            }

                            var valueStart = i;
                            if (At(code, i) is '"' or '\'')
                            {
                                var quote = code[i];
                                i++;
                                while (i < n && code[i] != quote) i++;
                                if (i < n) i++;
                            }
                            else
                            {
                                while (i < n && !char.IsWhiteSpace(code[i]) && code[i] is not ('>' or '/')) i++;
                            }

                            if (i > valueStart) Wrap(sb, "string", code.AsSpan(valueStart, i - valueStart));
                        }
                        continue;
                    }

                    Escape(sb, [code[i]]);
                    i++;
                }

                if (i < n)
                {
                    sb.Append("&gt;");
                    i++;
                }
                continue;
            }

            Escape(sb, [ch]);
            i++;
        }

        return sb.ToString();
    }

    private static string HighlightCss(string code)
    {
        var n = code.Length;
        var sb = new StringBuilder(code.Length * 2);
        var i = 0;

        while (i < n)
        {
            var ch = code[i];

            if (ch == '/' && At(code, i + 1) == '*')
            {
                var end = EndOf(code, "*/", i + 2, 2);
                Wrap(sb, "comment", code.AsSpan(i, end - i));
                i = end;
                continue;
            }

            if (ch is '"' or '\'')
            {
                var j = i + 1;
                while (j < n && code[j] != ch)
                {
                    if (code[j] == '\\') j++;
                    j++;
                }
                if (j < n) j++;
                j = Math.Min(j, n);
                Wrap(sb, "string", code.AsSpan(i, j - i));
                i = j;
                continue;
            }

            if (char.IsAsciiDigit(ch) && (i == 0 || !(IsAsciiLetter(code[i - 1]) || code[i - 1] is '_' or '-')))
            {
                var j = i;
                while (j < n && (char.IsAsciiLetterOrDigit(code[j]) || code[j] is '.' or '%')) j++;
                Wrap(sb, "number", code.AsSpan(i, j - i));
                i = j;
                continue;
            }

            if (ch == '#' && char.IsAsciiHexDigit(At(code, i + 1)))
            {
                var j = i + 1;
                while (j < n && char.IsAsciiHexDigit(code[j])) j++;
                Wrap(sb, "number", code.AsSpan(i, j - i));
                i = j;
                continue;
            }

            if (ch is '.' or '#' or '@' or ':' or '[' || (IsAsciiLetter(ch) && (i == 0 || code[i - 1] is '\n' or '}')))
            {
                var j = i;
                while (j < n && code[j] is not ('{' or ';' or '\n')) j++;
                if (At(code, j) == '{')
                {
                    Wrap(sb, "selector", code.AsSpan(i, j - i));
                    i = j;
                    continue;
                }
            }

            if (IsAsciiLetter(ch) || ch == '-')
            {
                var j = i;
                while (j < n && (char.IsAsciiLetterOrDigit(code[j]) || code[j] == '-')) j++;
                var k = j;
                while (k < n && code[k] == ' ') k++;
                if (At(code, k) == ':' && At(code, k + 1) != ':')
                {
                    Wrap(sb, "property", code.AsSpan(i, j - i));
                }
                else
                {
                    Escape(sb, code.AsSpan(i, j - i));
                }
                i = j;
                continue;
            }

            Escape(sb, [ch]);
            i++;
        }

        return sb.ToString();
    }
}
