﻿using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SocialMediaDashboard.WebAPI.Extensions
{
    /// <summary>
    /// Extension for JWT Token.
    /// </summary>
    public static class JwtTokenExtension
    {
        /// <summary>
        /// Get user identifier.
        /// </summary>
        /// <param name="httpContext">Application HttpContext.</param>
        /// <returns>Identifier.</returns>
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.SingleOrDefault(x => x.Type == "id").Value;
        }
    }
}
