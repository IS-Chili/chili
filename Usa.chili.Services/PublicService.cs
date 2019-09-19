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
    public class PublicService: IPublicService
    {
        private readonly ILogger _logger;
        private readonly ChiliDbContext _dbContext;

        static PublicService()
        {
        }

        public PublicService(ILogger<PublicService> logger, ChiliDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<StationObservationDto> GetStationObservationById(int id) {
            Station station = await _dbContext.Station
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstAsync();

            return _dbContext.Public
                .AsNoTracking()
                .Where(x => x.StationKey == station.StationKey)
                .Where(x => x.Ts.HasValue && x.Ts.Value >= DateTime.Now.AddHours(-1))
                .AsEnumerable()
                .Select(p => p.ConvertUnits())
                .Select(p => new StationObservationDto {
                    StationName = station.DisplayName,
                    StationTimestamp = p.Ts ?? DateTime.Now,
                    AirTemperature = Math.Round(p.AirT2m ?? 0, 2),
                    DewPoint = Math.Round(p.DewPoint ?? 0, 2),
                    RealHumidity = Math.Round(p.Rh ?? 0, 2),
                    WindDirection = Math.Round(p.WndDir10m ?? 0, 2),
                    WindSpeed = Math.Round(p.WndDir10m ?? 0, 2),
                    Pressure = Math.Round(p.PressSealev1 ?? 0, 2),
                    Precipitation = Math.Round(p.PrecipTb3Today ?? 0, 2)
            }).FirstOrDefault();
        }
    }
}
