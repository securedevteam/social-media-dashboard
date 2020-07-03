﻿using Microsoft.Extensions.Options;
using SocialMediaDashboard.Common.Helpers;
using SocialMediaDashboard.Common.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;

namespace SocialMediaDashboard.Logic.Services
{
    /// <inheritdoc cref="IVkService"/>
    public class VkService : IVkService
    {
        private readonly IOptionsSnapshot<VkSettings> _vkSettings;
        private readonly VkApi _api;

        public VkService(IOptionsSnapshot<VkSettings> vkSettings,
                         VkApi api)
        {
            _vkSettings = vkSettings ?? throw new ArgumentNullException(nameof(vkSettings));
            _api = api ?? throw new ArgumentNullException(nameof(api));
        }

        /// <inheritdoc/>
        public async Task<int?> GetFollowers(string userName)
        {
            await _api.AuthorizeAsync(new ApiAuthParams
            {
                AccessToken = _vkSettings.Value.AccessToken
            });

            var response = await _api.Users.GetAsync(new string[] { userName }, VkNet.Enums.Filters.ProfileFields.Counters);
            var user = response.FirstOrDefault();

            return user.Counters.Followers;
        }

        /// <inheritdoc/>
        public async Task<int?> GetFollowers(long userId)
        {
            await _api.AuthorizeAsync(new ApiAuthParams
            {
                AccessToken = _vkSettings.Value.AccessToken
            });

            var response = await _api.Users.GetAsync(new long[] { userId }, VkNet.Enums.Filters.ProfileFields.Counters);
            var user = response.FirstOrDefault();

            return user.Counters.Followers;
        }
    }
}