using Bunit;
using ObserverMagazine.Web.Components;
using ObserverMagazine.Web.Models;
using ObserverMagazine.Web.Tests.Services;
using Xunit;

namespace ObserverMagazine.Web.Tests.Components;

public class QuizQuestionCardTests : IDisposable
{
    private readonly BunitContext _ctx = new();

    private IRenderedComponent<QuizQuestionCard> Render(QuizQuestion question, Action<QuizAnswer>? onAnswered = null, QuestionProgress? initial = null) =>
        _ctx.Render<QuizQuestionCard>(p => p
            .Add(c => c.Question, question)
            .Add(c => c.Number, 1)
            .Add(c => c.InitialProgress, initial)
            .Add(c => c.OnAnswered, onAnswered ?? (_ => { })));

    [Fact]
    public void SingleChoice_RendersRadioButtons()
    {
        var cut = Render(BookTestData.SingleChoice());

        Assert.Equal(3, cut.FindAll("input[type=radio]").Count);
        Assert.Contains("Select one answer", cut.Markup);
    }

    [Fact]
    public void MultipleChoice_RendersCheckboxes()
    {
        var cut = Render(BookTestData.MultipleChoice());

        Assert.Equal(3, cut.FindAll("input[type=checkbox]").Count);
        Assert.Contains("Select all that apply", cut.Markup);
    }

    [Fact]
    public void SubmitButton_DisabledUntilSelection()
    {
        var cut = Render(BookTestData.SingleChoice());

        Assert.True(cut.Find(".quiz-submit").HasAttribute("disabled"));

        cut.Find("input[value=b]").Change("b");

        Assert.False(cut.Find(".quiz-submit").HasAttribute("disabled"));
    }

    [Fact]
    public void CorrectSingleAnswer_ShowsSuccessAndExplanation()
    {
        QuizAnswer? answer = null;
        var cut = Render(BookTestData.SingleChoice(), a => answer = a);

        cut.Find("input[value=b]").Change("b");
        cut.Find(".quiz-submit").Click();

        Assert.NotNull(answer);
        Assert.True(answer.Result.IsCorrect);
        Assert.Contains("Correct!", cut.Markup);
        Assert.Contains("Because B.", cut.Markup);
        Assert.Empty(cut.FindAll(".quiz-retry"));
    }

    [Fact]
    public void PartialMultipleAnswer_ShowsRetryAndHidesSolution()
    {
        QuizAnswer? answer = null;
        var cut = Render(BookTestData.MultipleChoice(), a => answer = a);

        cut.Find("input[value=a]").Change(true);
        cut.Find(".quiz-submit").Click();

        Assert.False(answer!.Result.IsCorrect);
        Assert.Contains("Not quite.", cut.Markup);
        Assert.Empty(cut.FindAll(".quiz-option.is-answer"));
        Assert.NotNull(cut.Find(".quiz-retry"));
    }

    [Fact]
    public void Reveal_HighlightsAllCorrectOptions()
    {
        var cut = Render(BookTestData.MultipleChoice());

        cut.Find("input[value=b]").Change(true);
        cut.Find(".quiz-submit").Click();
        cut.Find(".quiz-reveal").Click();

        Assert.Equal(2, cut.FindAll(".quiz-option.is-answer").Count);
        Assert.Single(cut.FindAll(".quiz-option.is-wrong"));
    }

    [Fact]
    public void Retry_ClearsSelectionAndResult()
    {
        var cut = Render(BookTestData.SingleChoice());

        cut.Find("input[value=a]").Change("a");
        cut.Find(".quiz-submit").Click();
        cut.Find(".quiz-retry").Click();

        Assert.DoesNotContain("Not quite.", cut.Markup);
        Assert.True(cut.Find(".quiz-submit").HasAttribute("disabled"));
    }

    [Fact]
    public void PreviouslyCorrectAnswer_IsRestored()
    {
        var initial = new QuestionProgress
        {
            Attempts = 2,
            AnsweredCorrectly = true,
            LastAttemptCorrect = true,
            LastSelection = ["b"]
        };

        var cut = Render(BookTestData.SingleChoice(), initial: initial);

        Assert.Contains("Correct!", cut.Markup);
        Assert.Contains("2 attempts", cut.Markup);
    }

    public void Dispose() => _ctx.Dispose();
}
