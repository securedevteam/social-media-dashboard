﻿using FluentValidation;
using SocialMediaDashboard.WebAPI.Contracts.Requests;

namespace SocialMediaDashboard.WebAPI.Validators
{
    /// <summary>
    /// User login request validator.
    /// </summary>
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("Token is required.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("RefreshToken is required.");
        }
    }
}
