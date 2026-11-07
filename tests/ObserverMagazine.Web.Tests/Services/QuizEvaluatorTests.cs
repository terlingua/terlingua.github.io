using ObserverMagazine.Web.Services;
using Xunit;

namespace ObserverMagazine.Web.Tests.Services;

public class QuizEvaluatorTests
{
    [Fact]
    public void SingleChoice_CorrectOption_IsCorrect()
    {
        var result = QuizEvaluator.Evaluate(BookTestData.SingleChoice(), ["b"]);

        Assert.True(result.IsCorrect);
    }

    [Fact]
    public void SingleChoice_WrongOption_IsIncorrectAndFlagged()
    {
        var result = QuizEvaluator.Evaluate(BookTestData.SingleChoice(), ["a"]);

        Assert.False(result.IsCorrect);
        Assert.True(result.IsWronglySelected("a"));
        Assert.False(result.IsWronglySelected("b"));
    }

    [Fact]
    public void SingleChoice_MoreThanOneSelection_IsIncorrect()
    {
        var result = QuizEvaluator.Evaluate(BookTestData.SingleChoice(), ["a", "b"]);

        Assert.False(result.IsCorrect);
    }

    [Fact]
    public void MultipleChoice_AllCorrectOptions_IsCorrect()
    {
        var result = QuizEvaluator.Evaluate(BookTestData.MultipleChoice(), ["c", "a"]);

        Assert.True(result.IsCorrect);
    }

    [Fact]
    public void MultipleChoice_PartialSelection_IsIncorrect()
    {
        var result = QuizEvaluator.Evaluate(BookTestData.MultipleChoice(), ["a"]);

        Assert.False(result.IsCorrect);
    }

    [Fact]
    public void MultipleChoice_ExtraWrongOption_IsIncorrect()
    {
        var result = QuizEvaluator.Evaluate(BookTestData.MultipleChoice(), ["a", "b", "c"]);

        Assert.False(result.IsCorrect);
        Assert.True(result.IsWronglySelected("b"));
    }

    [Fact]
    public void UnknownOptionIds_AreIgnored()
    {
        var result = QuizEvaluator.Evaluate(BookTestData.SingleChoice(), ["b", "zzz"]);

        Assert.True(result.IsCorrect);
        Assert.DoesNotContain("zzz", result.Selected);
    }

    [Fact]
    public void EmptySelection_IsIncorrect()
    {
        var result = QuizEvaluator.Evaluate(BookTestData.MultipleChoice(), []);

        Assert.False(result.IsCorrect);
    }
}
