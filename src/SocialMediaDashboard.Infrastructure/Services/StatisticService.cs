﻿using Microsoft.Extensions.Logging;
using SocialMediaDashboard.Application.Interfaces;
using SocialMediaDashboard.Domain.Entities;
using SocialMediaDashboard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaDashboard.Infrastructure.Services
{
    /// <inheritdoc cref="IStatisticService"/>
    public class StatisticService : IStatisticService
    {
        private readonly ILogger<StatisticService> _logger;
        private readonly IGenericRepository<Statistic> _statisticRepository;
        private readonly ISubscriptionTypeService _subscriptionTypeService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IVkService _vkService;
        private readonly IInstagramService _instagramService;
        private readonly IYouTubeService _youTubeService;

        public StatisticService(ILogger<StatisticService> logger,
                                IGenericRepository<Statistic> statisticRepository,
                                ISubscriptionTypeService subscriptionTypeService,
                                ISubscriptionService subscriptionService,
                                IVkService vkService,
                                IInstagramService instagramService,
                                IYouTubeService youTubeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _statisticRepository = statisticRepository ?? throw new ArgumentNullException(nameof(statisticRepository));
            _subscriptionTypeService = subscriptionTypeService ?? throw new ArgumentNullException(nameof(subscriptionTypeService));
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _vkService = vkService ?? throw new ArgumentNullException(nameof(vkService));
            _instagramService = instagramService ?? throw new ArgumentNullException(nameof(instagramService));
            _youTubeService = youTubeService ?? throw new ArgumentNullException(nameof(youTubeService));
        }

        public async Task GetFollowersFromVkAsync()
        {
            await SaveStatisticsAsync(
                PlatformType.Vk,
                ObservationType.Follower,
                _vkService.GetFollowersByUserNameAsync);
        }

        public async Task GetFollowersFromInstagramAsync()
        {
            await SaveStatisticsAsync(
                PlatformType.Instagram,
                ObservationType.Follower,
                _instagramService.GetFollowersByUserNameAsync);
        }

        public async Task GetSubscribersFromYouTubeAsync()
        {
            await SaveStatisticsAsync(
                PlatformType.YouTube,
                ObservationType.Subscriber,
                _youTubeService.GetSubscribersByChannelAsync);
        }

        private async Task SaveStatisticsAsync(PlatformType platformType, ObservationType observationType, Func<string, Task<int>> getCounts)
        {
            var statistics = new List<Statistic>();
            var subscriptionTypeId = await _subscriptionTypeService.GetByParametersAsync(platformType, observationType);

            if (!(subscriptionTypeId == default))
            {
                var subscriptions = await _subscriptionService.GetAccountNamesBySubscriptionTypeIdAsync(subscriptionTypeId);

                if (subscriptions.Any())
                {
                    foreach (var subscription in subscriptions)
                    {
                        int count;

                        try
                        {
                            count = await getCounts(subscription.AccountName);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                            throw;
                        }

                        var statistic = new Statistic
                        {
                            Count = count,
                            Date = DateTime.Now,
                            SubscriptionId = subscription.Id
                        };

                        statistics.Add(statistic);
                    }

                    await _statisticRepository.CreateRangeAsync(statistics);
                    await _statisticRepository.SaveChangesAsync();
                }
            }
        }
    }
}