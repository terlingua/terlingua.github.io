using ObserverMagazine.ContentProcessor.Books;
using Xunit;

namespace ObserverMagazine.Integration.Tests;

public class QuestionCompilerTests
{
    private static QuestionDefinition Question(string? type, params (string Text, bool Correct)[] options) => new()
    {
        Prompt = "Which?",
        Type = type,
        Options = options.Select(o => new OptionDefinition { Text = o.Text, Correct = o.Correct }).ToList()
    };

    private static (IReadOnlyList<QuestionDocument> Questions, BuildDiagnostics Diagnostics) Compile(params QuestionDefinition[] definitions)
    {
        var diagnostics = new BuildDiagnostics();
        return (QuestionCompiler.Compile(definitions, "test.md", diagnostics), diagnostics);
    }

    [Fact]
    public void InfersSingleAnswer_WhenOneOptionCorrect()
    {
        var (questions, diagnostics) = Compile(Question(null, ("A", true), ("B", false)));

        Assert.False(diagnostics.HasErrors);
        var question = Assert.Single(questions);
        Assert.False(question.MultipleAnswers);
        Assert.Equal("q1", question.Id);
        Assert.Equal(["a", "b"], question.Options.Select(o => o.Id).ToArray());
    }

    [Fact]
    public void InfersMultipleAnswers_WhenSeveralOptionsCorrect()
    {
        var (questions, _) = Compile(Question(null, ("A", true), ("B", true), ("C", false)));

        Assert.True(questions[0].MultipleAnswers);
    }

    [Fact]
    public void ExplicitMultiple_AllowsSingleCorrectOption()
    {
        var (questions, diagnostics) = Compile(Question("multiple", ("A", true), ("B", false)));

        Assert.False(diagnostics.HasErrors);
        Assert.True(questions[0].MultipleAnswers);
    }

    [Fact]
    public void ExplicitSingle_WithSeveralCorrectOptions_IsError()
    {
        var (questions, diagnostics) = Compile(Question("single", ("A", true), ("B", true)));

        Assert.True(diagnostics.HasErrors);
        Assert.Empty(questions);
    }

    [Theory]
    [InlineData("bogus")]
    [InlineData("checkbox")]
    public void UnknownType_IsError(string type)
    {
        var (_, diagnostics) = Compile(Question(type, ("A", true), ("B", false)));

        Assert.True(diagnostics.HasErrors);
    }

    [Fact]
    public void NoCorrectOption_IsError()
    {
        var (_, diagnostics) = Compile(Question(null, ("A", false), ("B", false)));

        Assert.Contains(diagnostics.Errors, e => e.Contains("marked correct"));
    }

    [Fact]
    public void TooFewOptions_IsError()
    {
        var (_, diagnostics) = Compile(Question(null, ("A", true)));

        Assert.Contains(diagnostics.Errors, e => e.Contains("at least 2 options"));
    }

    [Fact]
    public void MissingPrompt_IsError()
    {
        var definition = new QuestionDefinition
        {
            Options = [new OptionDefinition { Text = "A", Correct = true }, new OptionDefinition { Text = "B" }]
        };

        var (_, diagnostics) = Compile(definition);

        Assert.Contains(diagnostics.Errors, e => e.Contains("prompt is required"));
    }

    [Fact]
    public void DuplicateQuestionIds_AreErrors()
    {
        var first = new QuestionDefinition { Id = "same", Prompt = "1", Options = [new() { Text = "A", Correct = true }, new() { Text = "B" }] };
        var second = new QuestionDefinition { Id = "same", Prompt = "2", Options = [new() { Text = "A", Correct = true }, new() { Text = "B" }] };

        var (questions, diagnostics) = Compile(first, second);

        Assert.Single(questions);
        Assert.Contains(diagnostics.Errors, e => e.Contains("duplicate question id"));
    }

    [Fact]
    public void InvalidQuestionId_IsError()
    {
        var definition = new QuestionDefinition { Id = "Bad Id", Prompt = "?", Options = [new() { Text = "A", Correct = true }, new() { Text = "B" }] };

        var (_, diagnostics) = Compile(definition);

        Assert.True(diagnostics.HasErrors);
    }

    [Fact]
    public void Markdown_IsRenderedInPromptOptionsAndExplanation()
    {
        var definition = new QuestionDefinition
        {
            Prompt = "What does `var` do?",
            Explanation = "It **infers** the type.",
            Options = [new() { Text = "Infers the `type`", Correct = true }, new() { Text = "Nothing" }]
        };

        var (questions, _) = Compile(definition);
        var question = questions[0];

        Assert.Contains("<code>var</code>", question.PromptHtml);
        Assert.Contains("<strong>infers</strong>", question.ExplanationHtml);
        Assert.Equal("Infers the <code>type</code>", question.Options[0].TextHtml);
    }

    [Fact]
    public void DefaultOptionIds_FollowLettersThenNumbers()
    {
        Assert.Equal("a", QuestionCompiler.DefaultOptionId(0));
        Assert.Equal("z", QuestionCompiler.DefaultOptionId(25));
        Assert.Equal("o27", QuestionCompiler.DefaultOptionId(26));
    }
}
