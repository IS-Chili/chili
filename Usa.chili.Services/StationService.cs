// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Usa.chili.Common;
using Usa.chili.Data;
using Usa.chili.Domain;
using Usa.chili.Dto;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
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

        // TODO: Replace with a database call
        public async Task<StationObservationDto> GetStationObservationById(int id) {
            Station station = await _dbContext.Station
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstAsync();

            return await Task.FromResult(new StationObservationDto {
                StationName = station.DisplayName,
                StationTimestamp = DateTime.Now,
                AirTemperature = 81.98,
                DewPoint = 75.71,
                RealHumidity = 81.35,
                WindDirection = 264.91,
                WindSpeed = 0.73,
                Pressure = 29.96,
                Precipitation = 1.0
            });
        }
    }
}
