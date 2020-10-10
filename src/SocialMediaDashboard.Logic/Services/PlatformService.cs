﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMediaDashboard.Application.Interfaces;
using SocialMediaDashboard.Application.Models;
using SocialMediaDashboard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaDashboard.Infrastructure.Services
{
    /// <inheritdoc cref="IPlatformService"/>
    public class PlatformService : IPlatformService
    {
        private readonly IRepository<Platform> _platformRepository;
        private readonly IMapper _mapper;

        public PlatformService(IRepository<Platform> platformRepository,
                               IMapper mapper)
        {
            _platformRepository = platformRepository ?? throw new ArgumentNullException(nameof(platformRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PlatformDto>> GetAllAsync()
        {
            var platforms = await _platformRepository
                .GetAllWithoutTracking()
                .ToListAsync();

            return _mapper.Map<List<PlatformDto>>(platforms);
        }

        public async Task<PlatformDto> GetByIdAsync(int id)
        {
            var platform = await _platformRepository
                .GetEntityWithoutTrackingAsync(platform => platform.Id == id);

            if (platform is null)
            {
                // TODO: message
                return null;
            }

            return _mapper.Map<PlatformDto>(platform);
        }
    }
}
