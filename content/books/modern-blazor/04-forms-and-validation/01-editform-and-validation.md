---
title: EditForm, inputs and validation
questions:
  - id: valid-submit
    prompt: Which `EditForm` callback runs only when the model passes validation?
    options:
      - id: onvalidsubmit
        text: "`OnValidSubmit`"
        correct: true
      - text: "`OnSubmit`"
      - text: "`OnInvalidSubmit`"
      - text: "`OnChange`"
    explanation: "`OnValidSubmit` and `OnInvalidSubmit` are used together; `OnSubmit` replaces both and leaves validation to you."
  - id: validator
    prompt: What does the `<DataAnnotationsValidator />` component do?
    options:
      - text: Validates the HTML markup of the page
      - text: Sends the form to a server-side validation API
      - id: annotations
        text: Validates the form model using attributes such as `[Required]` and `[StringLength]`
        correct: true
    explanation: It hooks DataAnnotations into the form's `EditContext`; `ValidationSummary` and `ValidationMessage` display the results.
  - id: inputs
    prompt: Which are built-in Blazor input components?
    type: multiple
    options:
      - text: "`InputText`"
        correct: true
      - text: "`InputNumber<TValue>`"
        correct: true
      - text: "`InputDate<TValue>`"
        correct: true
      - text: "`InputSelect<TValue>`"
        correct: true
    explanation: There are also `InputCheckbox`, `InputTextArea`, `InputRadioGroup` and `InputFile`.
  - id: client-only
    prompt: Is client-side validation in a WebAssembly app enough to protect your data?
    options:
      - id: server-too
        text: No — the server API must validate again, because anyone can call it directly
        correct: true
      - text: Yes, because the user cannot change WebAssembly code
      - text: Yes, if the form uses `EditForm`
    explanation: Client validation is for user experience; server validation is for correctness and security.
---
## Learning objectives

- Build forms with `EditForm` and input components
- Validate with DataAnnotations
- Customise validation and keep it secure

## A complete form

```csharp
public sealed class SignupModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, StringLength(64, MinimumLength = 12, ErrorMessage = "Use at least 12 characters.")]
    public string Password { get; set; } = "";

    [Range(typeof(bool), "true", "true", ErrorMessage = "Please accept the terms.")]
    public bool AcceptTerms { get; set; }
}
```

```razor
<EditForm Model="model" OnValidSubmit="SubmitAsync">
    <DataAnnotationsValidator />
    <ValidationSummary />

    <label>
        E-mail
        <InputText @bind-Value="model.Email" type="email" autocomplete="email" />
    </label>
    <ValidationMessage For="() => model.Email" />

    <label>
        Password
        <InputText @bind-Value="model.Password" type="password" autocomplete="new-password" />
    </label>
    <ValidationMessage For="() => model.Password" />

    <label><InputCheckbox @bind-Value="model.AcceptTerms" /> I accept the terms</label>

    <button type="submit" disabled="@busy">Sign up</button>
</EditForm>

@code {
    private readonly SignupModel model = new();
    private bool busy;

    private async Task SubmitAsync()
    {
        busy = true;
        try { await Accounts.SignupAsync(model); }
        finally { busy = false; }
    }
}
```

## EditContext

`EditForm` creates an `EditContext` that tracks field changes and validation messages. Use it
directly for advanced scenarios:

```csharp
private EditContext? editContext;
protected override void OnInitialized() => editContext = new EditContext(model);
private bool CanSubmit => editContext?.IsModified() == true && editContext.GetValidationMessages().Any() == false;
```

## Custom validation

Implement `IValidatableObject` for cross-field rules, or a `ValidationAttribute` for reusable rules.
For complex rule sets, libraries such as FluentValidation integrate through a custom validator
component that adds messages to a `ValidationMessageStore`.

## Security reminder

In WebAssembly and Auto apps, component code is downloaded to the browser and can be modified.
Always validate again on the server — sharing the same model class between client and API makes
that cheap.
