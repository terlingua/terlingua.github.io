using ObserverMagazine.Web.Models;

namespace ObserverMagazine.Web.Services;

public static class QuizEvaluator
{
    public static QuizResult Evaluate(QuizQuestion question, IEnumerable<string> selectedOptionIds)
    {
        ArgumentNullException.ThrowIfNull(question);
        ArgumentNullException.ThrowIfNull(selectedOptionIds);

        var validIds = question.Options.Select(o => o.Id).ToHashSet(StringComparer.Ordinal);
        var selected = selectedOptionIds.Where(validIds.Contains).ToHashSet(StringComparer.Ordinal);
        var correct = question.Options.Where(o => o.Correct).Select(o => o.Id).ToHashSet(StringComparer.Ordinal);

        var isCorrect = correct.Count > 0
            && selected.SetEquals(correct)
            && (question.MultipleAnswers || selected.Count == 1);

        return new QuizResult(isCorrect, selected, correct);
    }
}
