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
    public class StationDataService : IStationDataService
    {
        private readonly ILogger _logger;
        private readonly ChiliDbContext _dbContext;

        static StationDataService()
        {
        }

        public StationDataService(ILogger<StationDataService> logger, ChiliDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<StationGraphDto> StationGraphData(int stationId, int variableId, DateTime? date, bool isMetricUnits)
        {
            // Get the VariableDescription with the VariableType
            VariableDescription variableDescription = await _dbContext.VariableDescription
                .Include(x => x.VariableType)
                .Where(x => x.Id == variableId)
                .SingleAsync();

            // Build the StationGraphDto
            StationGraphDto stationGraphDto = new StationGraphDto
            {
                Title = "24 hour graph of " + variableDescription.VariableDescription1,
                YAxisTitle = isMetricUnits ? variableDescription.VariableType.MetricUnit : variableDescription.VariableType.EnglishUnit,
                Series = new List<StationGraphSeriesDto> {
                    new StationGraphSeriesDto {
                        Name = variableDescription.VariableType.VariableType1.Replace("_", " "),
                        LineWidth = 1
                    }
                }
            };

            // Get the first datetime data entry for the station
            stationGraphDto.FirstDateTimeEntry = await _dbContext.StationData
                        .Where(x => x.Station.Id == stationId)
                        .OrderBy(x => x.Ts)
                        .AsNoTracking()
                        .Select(x => x.Ts)
                        .FirstOrDefaultAsync();

            // Get the last datetime data entry for the station
            stationGraphDto.LastDateTimeEntry = await _dbContext.StationData
                        .Where(x => x.Station.Id == stationId)
                        .OrderByDescending(x => x.Ts)
                        .AsNoTracking()
                        .Select(x => x.Ts)
                        .FirstOrDefaultAsync();

            // Set first and last datetime entries null if defaulted
            if(stationGraphDto.FirstDateTimeEntry == DateTime.MinValue){
                stationGraphDto.FirstDateTimeEntry = null;
            }
            // Set first and last datetime entries null if defaulted
            if(stationGraphDto.LastDateTimeEntry == DateTime.MinValue){
                stationGraphDto.LastDateTimeEntry = null;
            }

            // Default date to LastDateTimeEntry
            if(!date.HasValue) {
                date = stationGraphDto.LastDateTimeEntry;
            }

            // Get the VariableEnum and VariableTypeEnum values based on the Variable
            // Retrive the data entries for the station and variable in a timespan of 24 hours from the date
            if(date != null) {
                if (System.Enum.TryParse(variableDescription.VariableName, out VariableEnum variableEnum))
                {
                    if (System.Enum.TryParse(variableDescription.VariableType.VariableType1, out VariableTypeEnum variableTypeEnum))
                    {
                        stationGraphDto.Series[0].Data = await _dbContext.StationData
                            .Where(x => x.Ts >= date.Value.Date && x.Ts < date.Value.Date.AddDays(1))
                            .Where(x => x.Station.Id == stationId)
                            .OrderBy(x => x.Ts)
                            .AsNoTracking()
                            .Select(x => new List<double>() {
                                (double) new DateTimeOffset(x.Ts.ToUniversalTime()).ToUnixTimeMilliseconds(),
                                x.ValueForVariable(isMetricUnits, variableTypeEnum, variableEnum) ?? 0
                            })
                            .ToListAsync();
                    }
                }
            }

            return stationGraphDto;
        }

        public async Task<List<RealtimeDataDto>> ListRealtimeData()
        {
            DateTime now = DateTime.Now;

            return await _dbContext.Station
                .AsNoTracking()
                .OrderBy(x => x.DisplayName)
                .Select(x => new RealtimeDataDto
                {
                    StationId = x.Id,
                    StationName = x.DisplayName,
                    StationTimestamp = now,
                    AirTemperature = 90.1,
                    DewPoint = 1.1,
                    HeatIndex = 1.1,
                    WindChill = 1.2,
                    RealHumidity = 1.2,
                    WindDirection = 1.3,
                    WindSpeed = 1.4,
                    Pressure = 1.5,
                    YesterdayExtreme = new ExtremeDto
                    {
                        AirTemperatureHighTimestamp = now.AddDays(-1),
                        AirTemperatureLowTimestamp = now.AddDays(-1),
                        DewPointHighTimestamp = now.AddDays(-1),
                        DewPointLowTimestamp = now.AddDays(-1),
                        RealHumidityHighTimestamp = now.AddDays(-1),
                        RealHumidityLowTimestamp = now.AddDays(-1),
                        WindSpeedMaxTimestamp = now.AddDays(-1),
                        AirTemperatureHigh = 65.3,
                        AirTemperatureLow = 80.6,
                        DewPointHigh = 1.5,
                        DewPointLow = 1.5,
                        RealHumidityHigh = 1.5,
                        RealHumidityLow = 1.5,
                        WindSpeedMax = 1.5,
                        Precipitation = 1.5,
                    },
                    TodayExtreme = new ExtremeDto
                    {
                        AirTemperatureHighTimestamp = now,
                        AirTemperatureLowTimestamp = now,
                        DewPointHighTimestamp = now,
                        DewPointLowTimestamp = now,
                        RealHumidityHighTimestamp = now,
                        RealHumidityLowTimestamp = now,
                        WindSpeedMaxTimestamp = now,
                        AirTemperatureHigh = 65.3,
                        AirTemperatureLow = 80.6,
                        DewPointHigh = 1.5,
                        DewPointLow = 1.5,
                        RealHumidityHigh = 1.5,
                        RealHumidityLow = 1.5,
                        WindSpeedMax = 1.5,
                        Precipitation = 1.5,
                    }
                })
                .ToListAsync();
        }
    }
}
