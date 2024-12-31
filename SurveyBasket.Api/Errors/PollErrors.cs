﻿using SurveyBasket.Api.Abstractions;

namespace SurveyBasket.Api.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound =
        new("Poll.Notfound", "No poll was found with the given ID");
    public static readonly Error DuplicatedPollTitle =
        new("Poll.DuplicatedTitle", "Another poll with the same title is exists");
}
