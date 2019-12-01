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

        public async Task<List<RealtimeDataDto>> ListRealtimeData(bool isMetricUnits, bool? isWindChill)
        {
            DateTime now = DateTime.Now.Date;

            // Get initial station data
            List<RealtimeDataDto> realtimeDataDtos = await _dbContext.Station
                .Include(x => x.Public)
                .Where(x => x.IsActive)
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
                    dto.AirTemperature = publicData.AirT2m.HasValue ? Math.Round(publicData.AirT2m.Value, 2) : (double?)null;
                    dto.HeatIndex = publicData.HtIdx.HasValue ? Math.Round(publicData.HtIdx.Value, 2) : (double?)null;
                    dto.WindChill = publicData.Felt.HasValue ? Math.Round(publicData.Felt.Value, 2) : (double?)null;
                    dto.DewPoint = publicData.DewPoint.HasValue ? Math.Round(publicData.DewPoint.Value, 2) : (double?)null;
                    dto.RealHumidity = publicData.Rh.HasValue ? Math.Round(publicData.Rh.Value, 2) : (double?)null;
                    dto.WindDirection = publicData.WndDir10m.HasValue ? Math.Round(publicData.WndDir10m.Value, 2) : (double?)null;
                    dto.WindSpeed = publicData.WndSpd10m.HasValue ? Math.Round(publicData.WndSpd10m.Value, 2) : (double?)null;
                    dto.Pressure = publicData.PressSealev1.HasValue ? Math.Round(publicData.PressSealev1.Value, 2) : (double?)null;
                    dto.Precipitation = publicData.PrecipTb3Today.HasValue ? Math.Round(publicData.PrecipTb3Today.Value, 2) : (double?)null;

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
                        AirTemperatureHigh = extremesYdayData.AirT2mMax.HasValue ? Math.Round(extremesYdayData.AirT2mMax.Value, 2) : (double?)null,
                        AirTemperatureLow = extremesYdayData.AirT2mMin.HasValue ? Math.Round(extremesYdayData.AirT2mMin.Value, 2) : (double?)null,
                        DewPointHigh = extremesYdayData.DewPt2mMax.HasValue ? Math.Round(extremesYdayData.DewPt2mMax.Value, 2) : (double?)null,
                        DewPointLow = extremesYdayData.DewPt2mMin.HasValue ? Math.Round(extremesYdayData.DewPt2mMin.Value, 2) : (double?)null,
                        RealHumidityHigh = extremesYdayData.Rh2mMax.HasValue ? Math.Round(extremesYdayData.Rh2mMax.Value, 2) : (double?)null,
                        RealHumidityLow = extremesYdayData.Rh2mMin.HasValue ? Math.Round(extremesYdayData.Rh2mMin.Value, 2) : (double?)null,
                        WindSpeedMax = extremesYdayData.WndSpd10mMax.HasValue ? Math.Round(extremesYdayData.WndSpd10mMax.Value, 2) : (double?)null,
                        Precipitation = extremesYdayData.PrecipTb3Today.HasValue ? Math.Round(extremesYdayData.PrecipTb3Today.Value, 2) : (double?)null
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
                        AirTemperatureHigh = extremesTdayData.AirT2mMax.HasValue ? Math.Round(extremesTdayData.AirT2mMax.Value, 2) : (double?)null,
                        AirTemperatureLow = extremesTdayData.AirT2mMin.HasValue ? Math.Round(extremesTdayData.AirT2mMin.Value, 2) : (double?)null,
                        DewPointHigh = extremesTdayData.DewPt2mMax.HasValue ? Math.Round(extremesTdayData.DewPt2mMax.Value, 2) : (double?)null,
                        DewPointLow = extremesTdayData.DewPt2mMin.HasValue ? Math.Round(extremesTdayData.DewPt2mMin.Value, 2) : (double?)null,
                        RealHumidityHigh = extremesTdayData.Rh2mMax.HasValue ? Math.Round(extremesTdayData.Rh2mMax.Value, 2) : (double?)null,
                        RealHumidityLow = extremesTdayData.Rh2mMin.HasValue ? Math.Round(extremesTdayData.Rh2mMin.Value, 2) : (double?)null,
                        WindSpeedMax = extremesTdayData.WndSpd10mMax.HasValue ? Math.Round(extremesTdayData.WndSpd10mMax.Value, 2) : (double?)null
                    };
                }
                else {
                    dto.YesterdayExtreme = new ExtremeDto();
                    dto.TodayExtreme = new ExtremeDto();
                }
            });

            return realtimeDataDtos;
        }
    }
}
