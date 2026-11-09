---
title: "Adding New Behavior"
questions:
  - id: bias
    prompt: "In procedural legacy code, what should you bias toward when adding behaviour?"
    options:
      - id: o148780
        text: "Adding code inline to existing functions"
      - id: ode6a9a
        text: "Using global variables"
      - id: new-functions
        text: "Introducing new functions rather than adding code to old ones — at least those can be tested"
        correct: true
    explanation: "“In procedural legacy code, it pays to bias toward introducing new functions rather than adding code to old ones. At the very least, we can write tests for the new functions that we write.”"
  - id: form-command
    prompt: "Why split `send_command` into `form_command` plus a small `send_command`?"
    options:
      - id: pure-logic
        text: "All the logic before `mart_key_send` becomes a testable pure function returning the message; the wrapper binds logic to the dependency"
        correct: true
      - id: oa4570a
        text: "To reduce memory usage"
      - id: o471bb3
        text: "Because `mart_key_send` must be called twice"
    explanation: "“We put all of the pure logic into one set of functions so we can keep them free of problematic dependencies. When we do this, we end up with little wrapper functions such as send_command, which bind our logic and our dependencies.”"
  - id: printed-bug
    prompt: "In the printed final `send_command`, the body calls `form_command` into `command`, sends it, then calls `free(message)`. What's wrong, and what would a reviewer flag?"
    options:
      - id: o30d190
        text: "`mart_key_send` should come after `free`"
      - id: of80a4a
        text: "Nothing"
      - id: free-command
        text: "`message` isn't declared there — it should free `command`"
        correct: true
    explanation: "The listing reads “char *command = form_command(id, name, command_string); mart_key_send(command); free(message);” — `message` is a local of `form_command`, so the wrapper should free `command`. Small slips like this are exactly what a compiler or test harness catches and Edit and Pray misses."
  - id: sequencing
    prompt: "For functions with little computation but important sequencing of external calls (like `calculate_loan_interest`), what options exist?"
    type: multiple
    options:
      - id: fn-pointers
        text: "In C, pass a struct of function pointers so tests can supply fakes"
        correct: true
      - id: o3bb1f0
        text: "Use reflection to intercept calls"
      - id: skip-tdd
        text: "In many procedural languages, skip test-first and test at a higher level"
        correct: true
    explanation: "“In many procedural languages, the best choice is to just skip writing the test first and write the function as best we can. Maybe we can test that it does the right thing at a higher level. But in C, we have another option. C supports function pointers”."
  - id: fp-seam
    prompt: "In the function-pointer technique, what is the enabling point?"
    options:
      - id: o50f2ff
        text: "A preprocessor define"
      - id: o5fe720
        text: "The linker"
      - id: struct-init
        text: "Where the `struct database` is initialised — real functions in production, fakes in tests"
        correct: true
    explanation: "“We can initialize those pointers to the addresses of the database-access functions.” — “In production code, the functions can point to the real databaseaccess functions. We can have them point at fakes when we are testing.”"
  - id: not-c-specific
    prompt: "Is the function-pointer technique C-specific?"
    options:
      - id: general
        text: "No — it works in most languages that support function pointers"
        correct: true
      - id: ob15954
        text: "Yes — it relies on C syntax"
    explanation: "“This technique isn’t C specific. It can be used in most languages that support function pointers.”"
---
## Learning objectives

- Separate pure computation from external calls
- Use function pointers as a seam in procedural code

## Connections to today

```csharp
public sealed record LoanDatabase(
    Func<RecordId, Record> Retrieve,
    Action<RecordId, RecordSet> Update);
```

A record of delegates is C#'s version of the struct of function pointers — handy for wrapping
static legacy helpers without inventing an interface hierarchy. Functional-core/imperative-shell
architecture is the `form_command`/`send_command` split as a design principle.
