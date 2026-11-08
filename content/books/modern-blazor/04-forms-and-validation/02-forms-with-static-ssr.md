---
title: Forms with static server rendering
questions:
  - id: formname
    prompt: Why does an `EditForm` in a statically rendered component need a `FormName`?
    options:
      - text: It sets the HTML `<title>`
      - text: It is required for client-side validation only
      - text: It names the database table to save to
      - id: identify
        text: It identifies which form handler should process the POST, because several forms may exist on a page
        correct: true
    explanation: Static SSR forms post back to the server, and `FormName` routes the submission to the right component.
  - id: supply
    prompt: How does a static SSR component receive posted form values?
    options:
      - text: Through `[Parameter]` properties set by the parent
      - id: supply-from-form
        text: Through a property marked `[SupplyParameterFromForm]`
        correct: true
      - text: By reading `HttpContext.Request.Form` in `OnAfterRender`
    explanation: The model property is bound from the form data on POST. Initialise it with `Model ??= new()` so GET requests still have an instance.
  - id: antiforgery
    prompt: Which statements about antiforgery in Blazor Web Apps are true?
    type: multiple
    options:
      - text: "`EditForm` renders an antiforgery token automatically"
        correct: true
      - text: "`app.UseAntiforgery()` must be in the request pipeline"
        correct: true
      - text: "A plain `<form method=\"post\">` can include the token with the `<AntiforgeryToken />` component"
        correct: true
      - text: Antiforgery is only needed in WebAssembly apps
    explanation: Antiforgery protects form posts against cross-site request forgery in all server-rendered forms.
---
Static SSR pages have no live connection, yet forms still work — they post back to the server,
just like classic web apps, with Blazor handling binding and validation.

## Learning objectives

- Build forms that work without interactivity
- Bind posted data with `[SupplyParameterFromForm]`
- Use enhanced form posts for a smoother experience

## A static SSR form

```razor
@page "/contact"
@inject IContactService Contacts

<EditForm Model="Model" FormName="contact" OnValidSubmit="SubmitAsync" Enhance>
    <DataAnnotationsValidator />
    <InputText @bind-Value="Model!.Name" placeholder="Name" />
    <ValidationMessage For="() => Model!.Name" />
    <InputTextArea @bind-Value="Model!.Message" />
    <button type="submit">Send</button>
</EditForm>

@if (sent)
{
    <p role="status">Thanks! We'll be in touch.</p>
}

@code {
    [SupplyParameterFromForm] private ContactModel? Model { get; set; }
    private bool sent;

    protected override void OnInitialized() => Model ??= new();

    private async Task SubmitAsync()
    {
        await Contacts.SendAsync(Model!);
        sent = true;
    }
}
```

On GET, `Model` is null and gets a fresh instance. On POST, Blazor binds the form fields into
`Model`, validates, and calls `OnValidSubmit`. No WebSocket and no WebAssembly are involved.

## Enhanced form handling

The `Enhance` parameter (or `data-enhance` on a plain `<form>`) makes `blazor.web.js` submit with
`fetch` and patch the page in place instead of a full reload — scroll position and unchanged DOM
are preserved.

## Plain HTML forms

```razor
<form method="post" @onsubmit="Subscribe" @formname="newsletter" data-enhance>
    <AntiforgeryToken />
    <input name="Email" type="email" />
    <button>Subscribe</button>
</form>

@code {
    [SupplyParameterFromForm(FormName = "newsletter")] private string? Email { get; set; }
    private Task Subscribe() => Newsletter.AddAsync(Email);
}
```

## When to choose static forms

Static forms are ideal for content sites, sign-in pages, contact forms and checkout steps: they
are fast, work before (or without) WebAssembly loads, and keep the server load low because no
circuit is held open.
