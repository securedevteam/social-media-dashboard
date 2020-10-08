﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMediaDashboard.Application.Interfaces;
using SocialMediaDashboard.Application.Models;
using SocialMediaDashboard.Domain.Entities;
using SocialMediaDashboard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaDashboard.Infrastructure.Services
{
    ///// <inheritdoc cref="ISubscriptionService"/>
    //public class SubscriptionService : ISubscriptionService
    //{
    //    private readonly IRepository<Subscription> _subscriptionRepository;
    //    private readonly IMapper _mapper;

    //    public SubscriptionService(IRepository<Subscription> subscriptionRepository,
    //                               IMapper mapper)
    //    {
    //        _subscriptionRepository = subscriptionRepository ?? throw new ArgumentNullException(nameof(subscriptionRepository));
    //        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    //    }

    //    public async Task<bool> AddSubscriptionAsync(string userId, int accountId, string account, AccountKind accountType, SubscriptionKind subscriptionType)
    //    {
    //        var canCreateSubscription = await CanUserCreateSubscription(userId, account, accountType, subscriptionType);
    //        if (!canCreateSubscription)
    //        {
    //            return false;
    //        }

    //        var subscription = new Subscription
    //        {
    //            Type = subscriptionType,
    //            IsDisplayed = true,
    //            AccountId = accountId
    //        };

    //        await _subscriptionRepository.AddAsync(subscription);
    //        await _subscriptionRepository.SaveChangesAsync();

    //        return true;
    //    }

    //    public async Task<IEnumerable<SubscriptionDto>> GetAllUserSubscriptionsAsync(string userId)
    //    {
    //        var subscriptions = await _subscriptionRepository.GetAllWithoutTracking()
    //            .Include(s => s.Account)
    //            .Where(s => s.Account.UserId == userId)
    //            .ToListAsync();

    //        var subscriptionsDto = _mapper.Map<List<SubscriptionDto>>(subscriptions);

    //        return subscriptionsDto;
    //    }

    //    public async Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsByTypeAsync(AccountKind accountType, SubscriptionKind subscriptionType)
    //    {
    //        var subscriptions = await _subscriptionRepository.GetAllWithoutTracking()
    //            .Include(s => s.Account)
    //            .Where(s => s.Type == subscriptionType && s.Account.Type == accountType)
    //            .ToListAsync();

    //        var subscriptionsDto = _mapper.Map<List<SubscriptionDto>>(subscriptions);

    //        return subscriptionsDto;
    //    }

    //    public async Task<bool> DeleteSubscriptionAsync(int id, string userId)
    //    {
    //        var subscription = await _subscriptionRepository.GetAll()
    //            .Include(s => s.Account)
    //            .FirstOrDefaultAsync(s => s.Account.UserId == userId && s.Id == id);

    //        //var subscription = await _subscriptionRepository
    //        if (subscription == null)
    //        {
    //            return false;
    //        }

    //        _subscriptionRepository.Delete(subscription);
    //        await _subscriptionRepository.SaveChangesAsync();

    //        return true;
    //    }

    //    public async Task<bool> SubscriptionExistAsync(int id)
    //    {
    //        var subscription = await _subscriptionRepository.GetEntityAsync(m => m.Id == id);
    //        if (subscription == null)
    //        {
    //            return false;
    //        }

    //        return true;
    //    }

    //    private async Task<bool> CanUserCreateSubscription(string userId, string account, AccountKind accountType, SubscriptionKind subscriptionType)
    //    {
    //        var selectedSubscriptions = await _subscriptionRepository.GetAllWithoutTracking()
    //            .Include(s => s.Account)
    //            .Where(s => s.Account.UserId == userId && s.Account.Name == account && s.Account.Type == accountType)
    //            .ToListAsync();

    //        foreach (var subscription in selectedSubscriptions)
    //        {
    //            if (subscription.Type == subscriptionType)
    //            {
    //                return false;
    //            }
    //        }

    //        return true;
    //    }
    //}
}
