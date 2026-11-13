#!/usr/bin/env bash
set -euo pipefail

readonly PROJECT="terlingua.github.io"
readonly BOOK_ROOT="content/books"
readonly BLOG_GLOB="content/blog/*.md"
readonly SKIP_GLOBS=("docs/*")
readonly DATA_GLOBS=("src/*/wwwroot/plaintext/*")
FULL=0

usage() {
    cat <<USAGE
Usage: bash export.sh [-o FILE] [-f] [-- PATHSPEC...]

Writes an LLM context dump of the repository (default: docs/llm/dump.txt).

  Source, config, docs     full text
  ${BLOG_GLOB}        front matter only
  ${BOOK_ROOT}/           one inventory line per book
  Binary and data files    path only

  -o, --output FILE   write to FILE
  -f, --full          full text for blog posts, books and data files too
  -h, --help          show this help

PATHSPECs are passed to git ls-files to narrow the export:

  bash export.sh -- src tools
  bash export.sh -- ':!tests' ':!*Showcase*'
  bash export.sh -f -o /tmp/ddia.txt -- ${BOOK_ROOT}/designing-data-intensive-applications
USAGE
}

matches_any() {
    local path="$1" glob
    shift
    for glob in "$@"; do
        [[ "$path" == $glob ]] && return 0
    done
    return 1
}

is_text() {
    [[ ! -s "$1" ]] || LC_ALL=C grep -qI '' "$1"
}

classify() {
    local path="$1"
    if [[ "$path" == "$OUTPUT_REL" ]] || matches_any "$path" "${SKIP_GLOBS[@]}"; then
        echo skip
    elif ! is_text "$path"; then
        echo binary
    elif (( FULL )); then
        echo full
    elif [[ "$path" == "$BOOK_ROOT"/* ]]; then
        echo book
    elif matches_any "$path" "${DATA_GLOBS[@]}"; then
        echo data
    elif [[ "$path" == $BLOG_GLOB ]]; then
        echo frontmatter
    else
        echo full
    fi
}

front_matter() {
    awk 'NR == 1 && $0 != "---" { exit } { print } NR > 1 && $0 == "---" { exit }' "$1"
}

yaml_scalar() {
    sed -n "s/^$2:[[:space:]]*//p" "$1" | head -n 1 | sed -E 's/^"(.*)"$/\1/; s/^'\''(.*)'\''$/\1/'
}

book_inventory() {
    local manifest slug dir chapters sections
    printf '%s\n' "${BOOK_FILES[@]}" | { grep -E "^${BOOK_ROOT}/[^/]+/book\.yml$" || true; } | while read -r manifest; do
        dir="${manifest%/book.yml}"
        slug="${dir##*/}"
        chapters=$(printf '%s\n' "${BOOK_FILES[@]}" | grep -cE "^${dir}/[^/]+/chapter\.yml$" || true)
        sections=$(printf '%s\n' "${BOOK_FILES[@]}" | grep -cE "^${dir}/[^/]+/[^/]+\.md$" || true)
        printf '%s | %s | %s | %s\n' "$slug" "$chapters" "$sections" "$(yaml_scalar "$manifest" title)"
    done
}

emit_file() {
    printf '==> %s <==\n' "$1"
    if [[ "$2" == frontmatter ]]; then
        front_matter "$1"
    else
        cat "$1"
        [[ -z "$(tail -c 1 "$1")" ]] || echo
    fi
    echo
}

write_dump() {
    echo "# ${PROJECT} export $(date -u '+%Y-%m-%dT%H:%M:%SZ')"
    if (( FULL )); then
        echo "# Full text of every text file. Binary files: path only."
    else
        echo "# Full text of source, config and docs. Blog posts: front matter only. Books: inventory only, format in README.md (Books). Data and binary files: path only."
    fi
    echo
    if (( ${#BOOK_FILES[@]} )); then
        echo "## Books (slug | chapters | sections | title)"
        book_inventory
        echo
    fi
    if (( ${#LISTED_FILES[@]} )); then
        echo "## Path only"
        printf '%s\n' "${LISTED_FILES[@]}"
        echo
    fi
    echo "## Files"
    echo
    local path
    for path in "${TEXT_FILES[@]}"; do
        emit_file "$path" "${KIND[$path]}"
    done
}

report() {
    local bytes
    bytes=$(wc -c < "$OUTPUT")
    {
        echo "Wrote ${OUTPUT}: ${bytes} bytes, ~$((bytes / 4)) tokens"
        echo "  text files: ${#TEXT_FILES[@]}, book files summarized: ${#BOOK_FILES[@]}, path only: ${#LISTED_FILES[@]}"
        echo "  largest:"
        local path
        for path in "${TEXT_FILES[@]}"; do
            printf '%s\t%s\n' "$(wc -c < "$path")" "$path"
        done | sort -rn | head -n 10 | awk -F'\t' '{ printf "    %8d  %s\n", $1, $2 }'
    } >&2
}

main() {
    local output="" path kind
    local -a pathspecs=()
    while [[ $# -gt 0 ]]; do
        case "$1" in
            -o|--output) output="$(realpath -m "$2")"; shift 2 ;;
            -f|--full) FULL=1; shift ;;
            -h|--help) usage; exit 0 ;;
            --) shift; pathspecs=("$@"); break ;;
            *) usage >&2; exit 1 ;;
        esac
    done

    cd "$(git rev-parse --show-toplevel)"
    OUTPUT="${output:-$PWD/docs/llm/dump.txt}"
    OUTPUT_REL="$(realpath -m --relative-to="$PWD" "$OUTPUT")"

    declare -gA KIND=()
    TEXT_FILES=()
    BOOK_FILES=()
    LISTED_FILES=()

    while IFS= read -r path; do
        [[ -f "$path" ]] || continue
        kind="$(classify "$path")"
        case "$kind" in
            skip) ;;
            book) BOOK_FILES+=("$path") ;;
            data|binary) LISTED_FILES+=("$path") ;;
            *) TEXT_FILES+=("$path"); KIND[$path]="$kind" ;;
        esac
    done < <(git ls-files --cached --others --exclude-standard -- "${pathspecs[@]}" | LC_ALL=C sort -u)

    mkdir -p "$(dirname "$OUTPUT")"
    write_dump > "$OUTPUT"
    report
}

main "$@"
