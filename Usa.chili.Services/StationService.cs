// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Usa.chili.Data;
using Usa.chili.Dto;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Usa.chili.Services
{
    public class StationService: IStationService
    {
        private readonly ILogger _logger;
        private readonly ChiliDbContext _dbContext;

        static StationService()
        {
        }

        public StationService(ILogger<StationService> logger, ChiliDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<DropdownDto>> ListActiveStations() {
            return await _dbContext.Station
                .AsNoTracking()
                .Where(x => x.IsActive == true)
                .OrderBy(x => x.DisplayName)
                .Select(x => new DropdownDto {
                    Id = x.Id,
                    Text = x.DisplayName
                })
                .ToListAsync();
        }

        public async Task<List<StationMapDto>> GetStationMapData() {
            return await _dbContext.Station
                .AsNoTracking()
                .Select(x => new StationMapDto {
                    Id = x.Id,
                    DisplayName = x.DisplayName,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }

         public async Task<List<DropdownDto>> ListAllStations() {
            return await _dbContext.Station
                .AsNoTracking()
                .OrderBy(x => x.DisplayName)
                .Select(x => new DropdownDto {
                    Id = x.Id,
                    Text = x.DisplayName
                })
                .ToListAsync();
        }
    }
}
