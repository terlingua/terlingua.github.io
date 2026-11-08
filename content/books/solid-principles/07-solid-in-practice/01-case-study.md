---
title: "Case study: a notification service"
questions:
  - id: which-principle
    prompt: Adding SMS by writing a new `SmsChannel` class and registering it, without editing `NotificationService`, demonstrates which principle most directly?
    options:
      - text: Single Responsibility
      - text: Liskov Substitution
      - text: Interface Segregation
      - id: ocp
        text: Open/Closed
        correct: true
    explanation: The service gains behaviour through a new implementation, while existing code stays closed to modification.
  - id: lsp-check
    prompt: "`PushChannel.SendAsync` silently does nothing when the user has no device registered, while other channels throw. Which principle is at risk?"
    options:
      - text: Dependency Inversion, because push notifications use a third-party SDK
      - id: lsp
        text: Liskov Substitution, because channels behave inconsistently under the same contract
        correct: true
      - text: None — implementation details do not matter
    explanation: Callers of `INotificationChannel` need one predictable contract. Make "cannot deliver" explicit, for example by returning a result.
  - id: benefits
    prompt: Which benefits came from the refactoring in this case study?
    type: multiple
    options:
      - text: Each channel can be unit tested in isolation
        correct: true
      - text: The service can be tested with fake channels and no network
        correct: true
      - text: New channels require no changes to the dispatcher
        correct: true
      - text: The number of classes in the system decreased
    explanation: The design traded a few more types for isolation, testability and extensibility.
---
## Learning objectives

- Apply the five principles together on a realistic example
- Recognise which principle each design decision supports

## Before

```csharp
public class NotificationService
{
    public void Notify(User user, string message)
    {
        if (user.PrefersEmail)
            new SmtpClient("smtp.example.com").Send("noreply@example.com", user.Email, "Notice", message);
        else if (user.PrefersSms)
            TwilioClient.Send(user.Phone, message);
        File.AppendAllText("notifications.log", $"{DateTime.Now}: {user.Id} {message}\n");
    }
}
```

## After

```csharp
public sealed record DeliveryResult(bool Delivered, string? Reason = null);

public interface INotificationChannel
{
    string Name { get; }
    bool CanDeliverTo(User user);
    Task<DeliveryResult> SendAsync(User user, string message, CancellationToken ct);
}

public sealed class EmailChannel(IEmailSender sender) : INotificationChannel
{
    public string Name => "email";
    public bool CanDeliverTo(User user) => user.PrefersEmail && user.Email is not null;
    public async Task<DeliveryResult> SendAsync(User user, string message, CancellationToken ct)
    {
        await sender.SendAsync(user.Email!, "Notice", message, ct);
        return new DeliveryResult(true);
    }
}

public sealed partial class NotificationService(
    IEnumerable<INotificationChannel> channels,
    ILogger<NotificationService> logger)
{
    public async Task NotifyAsync(User user, string message, CancellationToken ct)
    {
        foreach (var channel in channels.Where(c => c.CanDeliverTo(user)))
        {
            var result = await channel.SendAsync(user, message, ct);
            LogDelivery(user.Id, channel.Name, result.Delivered);
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Notified {UserId} via {Channel}: {Delivered}")]
    private partial void LogDelivery(int userId, string channel, bool delivered);
}
```

## Mapping decisions to principles

| Decision | Principle |
|----------|-----------|
| Each channel has its own class | SRP |
| New channels are new classes plus a registration | OCP |
| Every channel returns a `DeliveryResult` instead of throwing or silently skipping | LSP |
| `IEmailSender` exposes only sending, not mailbox management | ISP |
| `NotificationService` depends on interfaces composed in `Program.cs` | DIP |
