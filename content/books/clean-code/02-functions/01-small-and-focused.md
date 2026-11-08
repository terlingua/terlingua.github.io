---
title: Small and focused
questions:
  - id: one-thing
    prompt: How can you tell that a function does more than one thing?
    options:
      - text: It contains more than one line
      - id: extract
        text: You can extract another function from it with a name that is not merely a restatement of its implementation
        correct: true
      - text: It has a return value
      - text: It is called from more than one place
    explanation: If meaningful sub-steps can be extracted and named, the function is doing several things at different levels.
  - id: abstraction-level
    prompt: "What does \"one level of abstraction per function\" mean?"
    options:
      - text: Each function may only call functions in the same class
      - id: mixing
        text: A function should not mix high-level steps like "charge customer" with low-level details like string concatenation
        correct: true
      - text: Functions must be declared in order of abstraction
    explanation: Mixing levels forces readers to switch between "what" and "how" constantly.
  - id: guard
    prompt: Which techniques reduce nesting and make a function easier to read?
    type: multiple
    options:
      - text: Guard clauses that return early for invalid cases
        correct: true
      - text: Extracting the body of a loop into a well-named function
        correct: true
      - text: Replacing nested ifs with pattern-matching switch expressions where appropriate
        correct: true
      - text: Adding more `else` branches
    explanation: Early returns, extraction and switch expressions flatten code. Additional `else` branches usually deepen nesting.
---
## Learning objectives

- Write functions that do one thing
- Keep each function at one level of abstraction
- Reduce nesting with guard clauses and extraction

## A function that does everything

```csharp
public string Register(string email, string password)
{
    if (email != null)
    {
        if (email.Contains('@'))
        {
            if (password.Length >= 12)
            {
                var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
                _db.Execute("INSERT INTO Users ...", new { email, hash });
                _smtp.Send("noreply@example.com", email, "Welcome", "Hi!");
                return "ok";
            }
            return "weak password";
        }
    }
    return "bad email";
}
```

## Refactored

```csharp
public async Task<RegistrationResult> RegisterAsync(string email, string password, CancellationToken ct)
{
    if (!EmailAddress.IsValid(email)) return RegistrationResult.InvalidEmail;
    if (!PasswordPolicy.IsStrong(password)) return RegistrationResult.WeakPassword;

    var user = User.Create(email, _passwordHasher.Hash(password));
    await _users.AddAsync(user, ct);
    await _welcomeMailer.SendAsync(user, ct);
    return RegistrationResult.Success;
}
```

The new function reads as a list of steps at one level. Details — hashing, SQL, SMTP — live in
collaborators. (It also fixes a security bug: a bare SHA-256 is not a password hash. Use a slow,
salted algorithm such as ASP.NET Core Identity's `PasswordHasher<T>`, PBKDF2, bcrypt or Argon2.)

## Guard clauses

Return early for the special cases so the main path is not indented:

```csharp
decimal Discount(Customer? customer)
{
    if (customer is null) return 0m;
    if (!customer.IsActive) return 0m;
    return customer.Tier switch
    {
        Tier.Gold => 0.15m,
        Tier.Silver => 0.05m,
        _ => 0m
    };
}
```

## How small?

There is no magic line count. A function should fit in your head: you should be able to read its
name and body and predict exactly what it does. When you need to scroll, or when you find yourself
writing a comment above a block to explain it, extract that block and let the name be the comment.
