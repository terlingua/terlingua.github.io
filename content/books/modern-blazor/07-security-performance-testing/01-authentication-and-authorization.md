---
title: Authentication and authorization
questions:
  - id: authorizeview
    prompt: What does `<AuthorizeView>` do?
    options:
      - text: Blocks the HTTP request on the server
      - text: Encrypts the component's markup
      - id: conditional
        text: Renders different content depending on whether the user is authenticated or satisfies a policy or role
        correct: true
      - text: Redirects every anonymous user to a login page
    explanation: It uses the cascaded authentication state and shows `Authorized`, `NotAuthorized` or `Authorizing` content.
  - id: wasm-trust
    prompt: In a WebAssembly app, hiding an "Delete" button with `AuthorizeView` for non-admins is…
    options:
      - text: Sufficient security, because WebAssembly code cannot be modified
      - text: Unnecessary, because browsers enforce roles
      - id: ux-only
        text: Good for user experience, but the server API must still enforce authorization
        correct: true
    explanation: Anyone can inspect, modify or bypass client code. Every protected API endpoint must authorize requests itself.
  - id: facts
    prompt: Which statements about Blazor authentication are true?
    type: multiple
    options:
      - text: "`AuthenticationStateProvider` supplies the current user's `ClaimsPrincipal` to components"
        correct: true
      - text: "`@attribute [Authorize]` on a routable component restricts access to the page"
        correct: true
      - text: "`AddCascadingAuthenticationState()` makes the authentication state available as a cascading parameter"
        correct: true
      - text: Storing access tokens in `localStorage` is immune to cross-site scripting
    explanation: Tokens in `localStorage` can be stolen by any injected script. Prefer cookie-based sessions with a backend (the BFF pattern) where possible.
---
## Learning objectives

- Show and hide UI based on the user
- Protect pages and APIs correctly in each render mode
- Understand the trust boundary in WebAssembly

## Setup

```csharp
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization(o =>
    o.AddPolicy("catalog-admin", p => p.RequireRole("Admin").RequireClaim("department", "catalog")));
```

## In components

```razor
@page "/admin/products"
@attribute [Authorize(Policy = "catalog-admin")]

<AuthorizeView Roles="Admin">
    <Authorized>
        <p>Signed in as @context.User.Identity?.Name</p>
        <button @onclick="DeleteAsync">Delete</button>
    </Authorized>
    <NotAuthorized>
        <a href="account/login">Sign in</a>
    </NotAuthorized>
</AuthorizeView>
```

```csharp
[CascadingParameter] private Task<AuthenticationState>? AuthState { get; set; }

private async Task<bool> IsOwnerAsync(Order order)
{
    var user = (await AuthState!).User;
    return user.FindFirstValue(ClaimTypes.NameIdentifier) == order.OwnerId;
}
```

## Trust boundaries by render mode

| Render mode | Where checks are enforced |
|-------------|---------------------------|
| Static SSR | On the server, per request — trustworthy |
| Interactive Server | On the server, in the circuit — trustworthy |
| WebAssembly / Auto (client) | In the browser — **UX only**; the API must authorize every call |

## Sharing auth state with WebAssembly

In a Blazor Web App with WebAssembly components, the server can serialize the authentication
state into the page so the client knows who is signed in (.NET 9+):

```csharp
// server
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

// client
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();
```

API calls from the client then use the same authentication cookie, and the server's minimal API
endpoints call `.RequireAuthorization()`.
