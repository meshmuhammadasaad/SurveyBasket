﻿namespace SurveyBasket.Api.Contracts.Authentications;

public record LoginRequest(
     string Email,
     string Password
);