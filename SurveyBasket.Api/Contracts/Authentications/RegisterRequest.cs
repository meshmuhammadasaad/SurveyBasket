﻿namespace SurveyBasket.Api.Contracts.Authentications;

public record RegisterRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
