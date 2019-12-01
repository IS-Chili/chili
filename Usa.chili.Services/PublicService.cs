// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Usa.chili.Data;
using Usa.chili.Domain;
using Usa.chili.Dto;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
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
                .AsEnumerable()
                .Select(p => p.ConvertUnits(false, null))
                .Select(p => new StationObservationDto {
                    StationId = station.Id,
                    StationName = station.DisplayName,
                    StationTimestamp = p.Ts,
                    AirTemperature = p.AirT2m,
                    Felt = p.Felt,
                    DewPoint = p.DewPoint,
                    RealHumidity = p.Rh,
                    WindDirection = p.WndDir10m,
                    WindSpeed = p.WndSpd10m,
                    Pressure = p.PressSealev1,
                    Precipitation = p.PrecipTb3Today
            }).FirstOrDefault();
        }
    }
}
