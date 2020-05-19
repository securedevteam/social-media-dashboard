﻿using SocialMediaDashboard.Common.DTO;
using System.Threading.Tasks;

namespace SocialMediaDashboard.Common.Interfaces
{
    /// <summary>
    /// Interface for implement identity service.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Confirm email.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="code">Verify code.</param>
        /// <returns>Authentication result data transfer object.</returns>
        Task<AuthenticationResult> ConfirmEmailAsync(string id, string code);

        /// <summary>
        /// Get user by Email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>User data transfet object.</returns>
        Task<UserResult> GetUserByEmailAsync(string email);

        /// <summary>
        /// Get user by Id.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>User data transfet object.</returns>
        Task<UserResult> GetUserByIdAsync(string id);

        /// <summary>
        /// Get user by username.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <returns>User data transfet object.</returns>
        Task<UserResult> GetUserByNameAsync(string username);

        /// <summary>
        /// Sign in.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        /// <returns>Authentication result data transfer object.</returns>
        Task<AuthenticationResult> LoginAsync(string email, string password);

        /// <summary>
        /// Restore password.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>Registration result data transfer object.</returns>
        Task<ConfirmationResult> RestorePasswordAsync(string email);

        /// <summary>
        /// Sign up (create new user).
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        /// <returns>Registration result data transfer object.</returns>
        Task<ConfirmationResult> RegistrationAsync(string email, string password);
    }
}