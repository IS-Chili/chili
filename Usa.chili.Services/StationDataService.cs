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
                                (double) new DateTimeOffset(x.Ts).ToUnixTimeMilliseconds(),
                                x.ValueForVariable(isMetricUnits, variableTypeEnum, variableEnum) ?? 0
                            })
                            .ToListAsync();
                    }
                }
            }

            return stationGraphDto;
        }

        public async Task<List<RealtimeDataDto>> ListRealtimeData(bool isMetricUnits, bool? isWindChill)
        {
            DateTime now = DateTime.Now.Date;

            // Get initial station data
            List<RealtimeDataDto> realtimeDataDtos = await _dbContext.Station
                .Include(x => x.Public)
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayName)
                .Select(x => new RealtimeDataDto
                {
                    StationId = x.Id,
                    IsStationOffline = !x.Public.Ts.HasValue, // || x.Public.Ts.Value.Date < now,
                    StationName = x.DisplayName,
                    StationTimestamp = x.Public.Ts
                })
                .ToListAsync();

            // Get realtime data for online stations
            realtimeDataDtos.ForEach(dto => {
                if(!dto.IsStationOffline) {
                    Public publicData = _dbContext.Public
                        .Where(x => x.StationKeyNavigation.Id == dto.StationId)
                        .Select(x => x.ConvertUnits(isMetricUnits, isWindChill))
                        .Single();

                    ExtremesTday extremesTdayData = _dbContext.ExtremesTday
                        .Where(x => x.StationKeyNavigation.Id == dto.StationId)
                        .Select(x => x.ConvertUnits(isMetricUnits))
                        .Single();

                    ExtremesYday extremesYdayData = _dbContext.ExtremesYday
                        .Where(x => x.StationKeyNavigation.Id == dto.StationId)
                        .Select(x => x.ConvertUnits(isMetricUnits))
                        .Single();

                    // Set public data
                    dto.AirTemperature = publicData.AirT2m;
                    dto.IsWindChill = publicData.IsWindChill;
                    dto.Felt = publicData.Felt;
                    dto.DewPoint = publicData.DewPoint;
                    dto.RealHumidity = publicData.Rh;
                    dto.WindDirection = publicData.WndDir10m;
                    dto.WindSpeed = publicData.WndSpd10m;
                    dto.Pressure = publicData.PressSealev1;
                    dto.Precipitation = publicData.PrecipTb3Today;

                    // Set Yesterday's extremes data
                    dto.YesterdayExtreme = new ExtremeDto
                    {
                        AirTemperatureHighTimestamp = extremesYdayData.AirT2mTmx,
                        AirTemperatureLowTimestamp = extremesYdayData.AirT2mTmn,
                        DewPointHighTimestamp = extremesYdayData.DewPt2mTmx,
                        DewPointLowTimestamp = extremesYdayData.DewPt2mTmn,
                        RealHumidityHighTimestamp = extremesYdayData.Rh2mTmx,
                        RealHumidityLowTimestamp = extremesYdayData.Rh2mTmn,
                        WindSpeedMaxTimestamp = extremesYdayData.WndSpd10mTmx,
                        AirTemperatureHigh = extremesYdayData.AirT2mMax,
                        AirTemperatureLow = extremesYdayData.AirT2mMin,
                        DewPointHigh = extremesYdayData.DewPt2mMax,
                        DewPointLow = extremesYdayData.DewPt2mMin,
                        RealHumidityHigh = extremesYdayData.Rh2mMax,
                        RealHumidityLow = extremesYdayData.Rh2mMin,
                        WindSpeedMax = extremesYdayData.WndSpd10mMax,
                        Precipitation = extremesYdayData.PrecipTb3Today
                    };

                    // Set Today's extremes data
                    dto.TodayExtreme = new ExtremeDto
                    {
                        AirTemperatureHighTimestamp = extremesTdayData.AirT2mTmx,
                        AirTemperatureLowTimestamp = extremesTdayData.AirT2mTmn,
                        DewPointHighTimestamp = extremesTdayData.DewPt2mTmx,
                        DewPointLowTimestamp = extremesTdayData.DewPt2mTmn,
                        RealHumidityHighTimestamp = extremesTdayData.Rh2mTmx,
                        RealHumidityLowTimestamp = extremesTdayData.Rh2mTmn,
                        WindSpeedMaxTimestamp = extremesTdayData.WndSpd10mTmx,
                        AirTemperatureHigh = extremesTdayData.AirT2mMax,
                        AirTemperatureLow = extremesTdayData.AirT2mMin,
                        DewPointHigh = extremesTdayData.DewPt2mMax,
                        DewPointLow = extremesTdayData.DewPt2mMin,
                        RealHumidityHigh = extremesTdayData.Rh2mMax,
                        RealHumidityLow = extremesTdayData.Rh2mMin,
                        WindSpeedMax = extremesTdayData.WndSpd10mMax
                    };
                }
                else {
                    dto.YesterdayExtreme = new ExtremeDto();
                    dto.TodayExtreme = new ExtremeDto();
                }
            });

            return realtimeDataDtos;
        }

        public async Task<MeteorologicalDataDto> GetMeteorologicalData(int stationId, DateTime? dateTime) {
            var meteorologicalDataDto = new MeteorologicalDataDto();

            // Get the first datetime data entry for the station
            meteorologicalDataDto.FirstDateTimeEntry = await _dbContext.StationData
                .Where(x => x.Station.Id == stationId)
                .OrderBy(x => x.Ts)
                .AsNoTracking()
                .Select(x => x.Ts)
                .FirstOrDefaultAsync();

            // Get the last datetime data entry for the station
            meteorologicalDataDto.LastDateTimeEntry = await _dbContext.StationData
                .Where(x => x.Station.Id == stationId)
                .OrderByDescending(x => x.Ts)
                .AsNoTracking()
                .Select(x => x.Ts)
                .FirstOrDefaultAsync();

            // Set first and last datetime entries null if defaulted
            if(meteorologicalDataDto.FirstDateTimeEntry == DateTime.MinValue){
                meteorologicalDataDto.FirstDateTimeEntry = null;
            }
            // Set first and last datetime entries null if defaulted
            if(meteorologicalDataDto.LastDateTimeEntry == DateTime.MinValue){
                meteorologicalDataDto.LastDateTimeEntry = null;
            }

            // Return DTO if dateTime is null
            if(!dateTime.HasValue) {
                return meteorologicalDataDto;
            }

            // Get Station Data in English and Metric Units
            return await _dbContext.StationData
                .Where(x => x.Station.Id == stationId)
                .Where(x => x.Ts == dateTime)
                .OrderByDescending(x => x.Ts)
                .Select(x => new MeteorologicalDataDto {
                    Door = x.Door,
                    FirstDateTimeEntry = meteorologicalDataDto.FirstDateTimeEntry,
                    LastDateTimeEntry = meteorologicalDataDto.LastDateTimeEntry
            }).FirstOrDefaultAsync();
        }
    }
}
