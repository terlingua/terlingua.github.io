---
title: "Skin and Wrap the API vs. Responsibility-Based Extraction"
questions:
  - id: skin-def
    prompt: "What does Skin and Wrap the API involve?"
    options:
      - id: o0305bd
        text: "Extracting methods named after your responsibilities"
      - id: mirror
        text: "Interfaces that mirror the API as closely as possible, plus wrappers around library classes, preserving signatures"
        correct: true
      - id: o16458b
        text: "Copying the library source into your project"
    explanation: "“When we Skin and Wrap the API, we make interfaces that mirror the API as close as possible and then create wrappers around library classes. To minimize our chances of making mistakes, we can Preserve Signatures (312) as we work.”"
  - id: skin-advantage
    prompt: "What is a key advantage of skinning and wrapping?"
    options:
      - id: o0034c0
        text: "It's always less work"
      - id: o5dee80
        text: "It improves runtime performance"
      - id: no-deps
        text: "You can end up with no dependencies on the underlying API — wrappers delegate in production, fakes in tests"
        correct: true
    explanation: "“One advantage to skinning and wrapping an API is that we can end up having no dependencies on the underlying API code. Our wrappers can delegate to the real API in production code and we can use fakes during test.”"
  - id: session-final
    prompt: "Why is the mailing list code a poor candidate for skinning?"
    options:
      - id: oedce45
        text: "JavaMail has no classes"
      - id: o66066c
        text: "The code has tests already"
      - id: session
        text: "`Transport` comes from `Session`, which is final, and the API is relatively complicated"
        correct: true
    explanation: "“Can we create a wrapper for Session? Not really—Session is a final class.” and “This mailing list code is really a poor candidate for skinning. The API is relatively complicated.”"
  - id: rbe-def
    prompt: "What is Responsibility-Based Extraction?"
    options:
      - id: o9ea556
        text: "Wrapping each API class one-to-one"
      - id: o3942a8
        text: "Assigning code ownership by team responsibility"
      - id: extract
        text: "Identify responsibilities in the code and extract methods (and classes) for them, like `MailSender.sendMessage`"
        correct: true
    explanation: "“In Responsibility-Based Extraction, we identify responsibilities in the code and start extracting methods for them.”"
  - id: when-skin
    prompt: "Skin and Wrap the API is good when…"
    type: multiple
    options:
      - id: o7400b2
        text: "You have a safe extract-method tool and the API is complex"
      - id: small
        text: "The API is relatively small"
        correct: true
      - id: separate
        text: "You want to completely separate out dependencies on a third-party library"
        correct: true
      - id: no-tests
        text: "You can't write tests because you can't test through the API"
        correct: true
    explanation: "The bullets: “The API is relatively small.” “You want to completely separate out dependencies on a third-party library.” “You don’t have tests, and you can’t write them because you can’t test through the API.”"
  - id: when-rbe
    prompt: "Responsibility-Based Extraction is good when…"
    type: multiple
    options:
      - id: tool
        text: "You have safe extract-method support or can extract safely by hand"
        correct: true
      - id: complicated
        text: "The API is more complicated"
        correct: true
      - id: o3374e5
        text: "You must remove every dependency on the library"
    explanation: "“The API is more complicated.” and “You have a tool that provides a safe extract method support, or you feel confident that you can do the extractions safely by hand.”"
  - id: rbe-downside
    prompt: "What is a downside of Responsibility-Based Extraction?"
    options:
      - id: o259113
        text: "It cannot raise the level of abstraction"
      - id: o72baac
        text: "It requires a final class"
      - id: untested
        text: "You may extract some of your own logic along with API code, so that extracted code may not get under test"
        correct: true
    explanation: "“we might end up extracting some of our own logic with the API code just so that we can extract a method with a higher-level name. If we do, our code can depend on higher-level interfaces rather than low-level API calls, but we might not be able to get the code we’ve extracted under test.”"
  - id: both
    prompt: "What do many teams end up doing?"
    options:
      - id: o5c675c
        text: "Avoiding third-party libraries entirely"
      - id: both-layers
        text: "Using both — a thin wrapper for testing and a higher-level wrapper presenting a better interface"
        correct: true
      - id: of2991d
        text: "Choosing one technique forever"
    explanation: "“Many teams use both techniques: a thin wrapper for testing and a higherlevel wrapper to present a better interface to their application.”"
  - id: scenario
    prompt: "Your service calls a large cloud storage SDK in 40 places with complex options objects. You have a reliable IDE Extract Method. Which approach fits first?"
    options:
      - id: o486a42
        text: "Skin and Wrap every SDK type one-to-one"
      - id: ode8836
        text: "Neither — test against the real cloud"
      - id: rbe-first
        text: "Responsibility-Based Extraction — pull out `SaveInvoicePdf`-style methods into a small class, then put an interface on that"
        correct: true
    explanation: "A complicated API plus safe extraction tooling matches the Responsibility-Based Extraction criteria: “The API is more complicated.” and “You have a tool that provides a safe extract method support”."
---
## Learning objectives

- Choose between Skin and Wrap the API and Responsibility-Based Extraction
- Combine thin and high-level wrappers

## Connections to today

```csharp
public interface IInvoiceArchive
{
    Task SaveAsync(InvoiceId id, Stream pdf, CancellationToken ct);
}

public sealed class BlobInvoiceArchive(BlobContainerClient container) : IInvoiceArchive
{
    public Task SaveAsync(InvoiceId id, Stream pdf, CancellationToken ct) =>
        container.GetBlobClient($"{id}.pdf").UploadAsync(pdf, overwrite: true, ct);
}
```

`IInvoiceArchive` is a responsibility-based interface: it speaks your domain, not the SDK's. A
thin skin over `BlobContainerClient` is rarely needed because the SDK is already mockable.
