using SurveyBasket.Api.Abstractions;

namespace SurveyBasket.Api.Errors;

public static class QuestionErrors
{
    public static readonly Error QuestionNotFound =
       new("Question.Notfound", "No Question was found with the given ID");
    public static readonly Error DuplicatedQuestionContent =
        new("Question.DuplicatedContent", "The same Question with the same content is exists");
}
