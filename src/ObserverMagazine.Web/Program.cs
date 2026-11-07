using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ObserverMagazine.Web;
using ObserverMagazine.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddSingleton<TelemetryService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookAnalytics, BookAnalytics>();
builder.Services.AddScoped<IndexedDbProgressStore>();
builder.Services.AddScoped<IProgressStore>(sp => new FallbackProgressStore(
    sp.GetRequiredService<IndexedDbProgressStore>(),
    new InMemoryProgressStore(),
    sp.GetRequiredService<ILogger<FallbackProgressStore>>()));
builder.Services.AddScoped<IReadingProgressService, ReadingProgressService>();

builder.Logging.SetMinimumLevel(LogLevel.Information);

var host = builder.Build();

var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("App");
logger.LogInformation("Terlingua Magazine started at {Time}", DateTime.UtcNow);

var telemetry = host.Services.GetRequiredService<TelemetryService>();
telemetry.TrackEvent("AppStarted");

await host.RunAsync();
