﻿using SocialMediaDashboard.Common.Enums;

namespace SocialMediaDashboard.WebAPI.Contracts.Requests
{
    /// <summary>
    /// Subscription create request.
    /// </summary>
    public class SubscriptionCreateRequest
    {
        /// <summary>
        /// Account identifier.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Subscription type.
        /// </summary>
        public SubscriptionType SubscriptionType { get; set; }
    }
}
