﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialMediaDashboard.Common.DTO;
using SocialMediaDashboard.Common.Helpers;
using SocialMediaDashboard.Common.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaDashboard.Logic.Services
{
    /// <inheritdoc cref="IIdentityService"/>
    public class IdentityService : IIdentityService
    {
        private readonly ApplicationSettings _appSettings;
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityService(IOptions<ApplicationSettings> appSettings,
                               UserManager<IdentityUser> userManager)
        {
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <inheritdoc/>
        public async Task<ConfirmationResult> RegistrationAsync(string email, string password)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser != null)
            {
                return new ConfirmationResult
                {
                    Errors = new[] { "The email you specified is already in the system." }
                };
            }

            var user = new IdentityUser
            {
                Email = email,
                UserName = email // UNDONE: username
            };

            var createdUser = await _userManager.CreateAsync(user, password);

            if (!createdUser.Succeeded)
            {
                return new ConfirmationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            return new ConfirmationResult
            {
                IsSuccessful = true,
                Email = user.Email,
                Code = await _userManager.GenerateEmailConfirmationTokenAsync(user)
            };
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist." }
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(identityUser, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Email or password is incorrect." }
                };
            }

            var emailConfirmationResult = await EmailConfirmHandlerAsync(identityUser);

            if (!emailConfirmationResult.IsSuccessful)
            {
                return new AuthenticationResult
                {
                    Errors = emailConfirmationResult.Errors
                };
            }

            return GenerateAuthenticationResult(identityUser);
        }

        /// <inheritdoc />
        public async Task<AuthenticationResult> ConfirmEmailAsync(string email, string code)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist." }
                };
            }

            var identityResult = await _userManager.ConfirmEmailAsync(identityUser, code);

            if (!identityResult.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Unexpected token issues.." }
                };
            }

            return GenerateAuthenticationResult(identityUser);
        }

        /// <inheritdoc />
        public async Task<ConfirmationResult> RestorePasswordAsync(string email)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser == null)
            {
                return new ConfirmationResult
                {
                    Errors = new[] { "User does not exist." }
                };
            }

            var emailConfirmationResult = await EmailConfirmHandlerAsync(identityUser);

            if (!emailConfirmationResult.IsSuccessful)
            {
                return new ConfirmationResult
                {
                    Errors = emailConfirmationResult.Errors
                };
            }

            return new ConfirmationResult
            {
                IsSuccessful = true,
                Email = identityUser.Email,
                Code = await _userManager.GeneratePasswordResetTokenAsync(identityUser)
            };
        }

        /// <inheritdoc />
        public async Task<AuthenticationResult> ResetPasswordAsync(string email, string newPassword, string code)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist." }
                };
            }

            var identityResult = await _userManager.ResetPasswordAsync(identityUser, code, newPassword);

            if (!identityResult.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "An unexpected error occurred while resetting the password.." }
                };
            }

            return GenerateAuthenticationResult(identityUser);
        }

        /// <inheritdoc/>
        public async Task<UserResult> GetUserByEmailAsync(string email)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser == null)
            {
                return null;
            }

            return new UserResult
            {
                Id = identityUser.Id,
                Email = identityUser.Email,
                UserName = identityUser.UserName,
                PhoneNumber = identityUser.PhoneNumber
            };
        }

        /// <inheritdoc/>
        public async Task<UserResult> GetUserByIdAsync(string id)
        {
            var identityUser = await _userManager.FindByIdAsync(id);

            if (identityUser == null)
            {
                return null;
            }

            return new UserResult
            {
                Id = identityUser.Id,
                Email = identityUser.Email,
                UserName = identityUser.UserName,
                PhoneNumber = identityUser.PhoneNumber
            };
        }

        /// <inheritdoc/>
        public async Task<UserResult> GetUserByNameAsync(string username)
        {
            var identityUser = await _userManager.FindByNameAsync(username);

            if (identityUser == null)
            {
                return null;
            }

            return new UserResult
            {
                Id = identityUser.Id,
                Email = identityUser.Email,
                UserName = identityUser.UserName,
                PhoneNumber = identityUser.PhoneNumber
            };
        }

        private async Task<ConfirmationResult> EmailConfirmHandlerAsync(IdentityUser identityUser)
        {
            var isConfirmed = await _userManager.IsEmailConfirmedAsync(identityUser);

            if (!isConfirmed)
            {
                return new ConfirmationResult
                {
                    Errors = new[] { "You have not verified your email." }
                };
            }

            return new ConfirmationResult
            {
                IsSuccessful = true
            };
        }

        private AuthenticationResult GenerateAuthenticationResult(IdentityUser identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                    new Claim(JwtRegisteredClaimNames.NameId, identityUser.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                IsSuccessful = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
