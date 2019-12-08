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
using Usa.chili.Domain;

namespace Usa.chili.Services
{
    /// <summary>
    /// Service for the Station table.
    /// </summary>
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

        /// <summary>
        /// Gets active stations as DTOs.
        /// </summary>
        /// <returns>List of DropdownDtos</returns>
        public async Task<List<DropdownDto>> ListActiveStations() {
            return await _dbContext.Station
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayName)
                .Select(x => new DropdownDto {
                    Id = x.Id,
                    Text = x.DisplayName
                })
                .ToListAsync();
        }

        /// <summary>
        /// Gets all stations as DTOs.
        /// </summary>
        /// <returns>List of DropdownDtos</returns>
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

        /// <summary>
        /// Gets data for station info displays.
        /// </summary>
        /// <param name="id">Station Id filter</param>
        /// <returns>A StationDto for a station</returns>
        public async Task<StationDto> GetStationInfo(int id) {
            return await _dbContext.Station
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new StationDto {
                    Id = x.Id,
                    DisplayName = x.DisplayName,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Elevation = x.Elevation,
                    BeginDate = x.BeginDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive
                })
                .SingleAsync();
        }

        /// <summary>
        /// Gets data for the station map.
        /// </summary>
        /// <returns>List of StationMapDtos</returns>
        public async Task<List<StationMapDto>> GetStationMapData() {
            List<StationMapDto> stationMapDtos = await _dbContext.Station
                .AsNoTracking()
                .Select(x => new StationMapDto {
                    Id = x.Id,
                    DisplayName = x.DisplayName,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            // Get high and low temperatures for each station
            stationMapDtos.ForEach(dto => {
                ExtremesTday extremesTdayData = _dbContext.ExtremesTday
                    .AsNoTracking()
                    .Where(x => x.StationKeyNavigation.Id == dto.Id)
                    // Perform any necessary calculations and conversions
                    .Select(x => x.ConvertUnits(false))
                    .SingleOrDefault();

                if(extremesTdayData != null) {
                    dto.AirTemperatureHigh = extremesTdayData.AirT2mMax;
                    dto.AirTemperatureLow = extremesTdayData.AirT2mMin;
                }
            });

            return stationMapDtos;
        }
    }
}
