﻿using FluentValidation;

namespace SurveyBasket.Api.Contracts.Authentications;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(r => r.Token).NotEmpty();

        RuleFor(r => r.RefreshToken).NotEmpty();
    }
}
