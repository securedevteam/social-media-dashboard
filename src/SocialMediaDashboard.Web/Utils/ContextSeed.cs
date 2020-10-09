﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SocialMediaDashboard.Application.Context;
using SocialMediaDashboard.Domain.Entities;
using System;

namespace SocialMediaDashboard.Web.Utils
{
    /// <summary>
    /// Fill in the database after creation.
    /// </summary>
    public static class ContextSeed
    {
        private const string logErrorMessage = "An error occurred seeding the DB.";
        private const string logInformationMessage = "The database is successfully seeded.";

        /// <summary>
        /// Context seed.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                var contextOptions = serviceProvider.GetRequiredService<DbContextOptions<SocialMediaDashboardContext>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                using var applicationContext = new SocialMediaDashboardContext(contextOptions);

                SocialMediaDashboardContextSeed.SeedRolesAsync(applicationContext, roleManager).GetAwaiter().GetResult();

                Log.Information(logInformationMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex, logErrorMessage);
            }
        }
    }
}
