namespace ObserverMagazine.ContentProcessor.Books;

public sealed class BuildDiagnostics
{
    private readonly List<string> _errors = [];
    private readonly List<string> _warnings = [];

    public IReadOnlyList<string> Errors => _errors;
    public IReadOnlyList<string> Warnings => _warnings;
    public bool HasErrors => _errors.Count > 0;

    public void Error(string location, string message) => _errors.Add($"{location}: {message}");

    public void Warning(string location, string message) => _warnings.Add($"{location}: {message}");
}
