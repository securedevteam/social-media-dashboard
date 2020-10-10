﻿using FluentValidation;
using SocialMediaDashboard.Web.Contracts.Requests;

namespace SocialMediaDashboard.Web.Validators
{
    /// <summary>
    /// User registration request validator.
    /// </summary>
    public class UserRegistrationRequestValidator : AbstractValidator<UserSignUpRequest>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public UserRegistrationRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("UserName is required.")
                .Length(1, 50)
                .WithMessage("UserName should be from 1 to 50.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}
