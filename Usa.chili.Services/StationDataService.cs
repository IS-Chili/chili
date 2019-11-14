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
    public class StationDataService: IStationDataService
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

        public async Task<StationGraphDto> StationGraphData(int stationId, int variableId, DateTime? date, bool isMetricUnits) {
            StationGraphDto stationGraphDto =  new StationGraphDto {
                Title = "24 hour graph of Air Temperature at 1.5m (4.92ft)",
                YAxisTitle = "Degrees Fahrenheit",
                Series = new List<StationGraphSeriesDto> {
                    new StationGraphSeriesDto {
                        Name = "Temperature",
                        LineWidth = 0.5,
                        Data = new List<List<double>>()
                    }
                }
            };

            DateTime dateStart = new DateTime(2019, 11, 1, 0, 0, 0);
            DateTime dateToReach = new DateTime(2019, 11, 2, 0, 0, 0);

            Random random = new Random();

            double i = 0.1;

            while(dateStart < dateToReach) {
                stationGraphDto.Series[0].Data.Add(new List<double>() {
                    (double) new DateTimeOffset(dateStart.ToUniversalTime()).ToUnixTimeMilliseconds(),
                    75 + i
                });
                dateStart = dateStart.AddMinutes(1);

                i += 0.1;

                if(i > 25) {
                    i = 15;
                }
            }

            return stationGraphDto;
        }

        public async Task<List<RealtimeDataDto>> ListRealtimeData() {
            DateTime now = DateTime.Now;

            return await _dbContext.Station
                .AsNoTracking()
                .OrderBy(x => x.DisplayName)
                .Select(x => new RealtimeDataDto {
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
                    YesterdayExtreme = new ExtremeDto {
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
                    TodayExtreme = new ExtremeDto {
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
        public async Task<MeteorologicalDataDto> GetMeteorologicalData(int? id) {
           
            return await _dbContext.Station_Data
                .AsNoTracking()
                .Include(sd => sd.Station)
                .Where(sd => sd.StationId == id)
                .OrderByDescending(sd => sd.TS)
                .Select(sd => new MeteorologicalDataDto{
                    StationID = id.GetValueOrDefault(),
                    StationName = sd.Station.DisplayName,
                    StationTimestamp = sd.TS,
                    Door = sd.Door.GetValueOrDefault(),
                    Battery = sd.Batt.GetValueOrDefault(),
                    Precip_TB3_Tot = sd.Precip_TB3_Tot.GetValueOrDefault(),
                    Precip_TX_Tot = sd.Precip_TX_Tot.GetValueOrDefault(),
                    Precip_TB3_Today = sd.Precip_TB3_Today.GetValueOrDefault(),
                    Precip_TX_Today = sd.Precip_TX_Today.GetValueOrDefault(),
                    SoilSfcT = sd.SoilSfcT.GetValueOrDefault(),
                    SoilT_5cm = sd.SoilT_5cm.GetValueOrDefault(),
                    SoilT_10cm = sd.SoilT_10cm.GetValueOrDefault(),
                    SoilT_20cm = sd.SoilT_20cm.GetValueOrDefault(),
                    SoilT_50cm = sd.SoilT_50cm.GetValueOrDefault(),
                    SoilT_100cm = sd.SoilT_100cm.GetValueOrDefault(),
                    AirT_1pt5m = sd.AirT_1pt5m.GetValueOrDefault(),
                    AirT_2m = sd.AirT_2m.GetValueOrDefault(),
                    AirT_9pt5m = sd.AirT_9pt5m.GetValueOrDefault(),
                    AirT_10m = sd.AirT_10m.GetValueOrDefault(),
                    RH_2m = sd.RH_2m.GetValueOrDefault(),
                    RH_10m = sd.RH_10m.GetValueOrDefault(),
                    Pressure_1 = sd.Pressure_1.GetValueOrDefault(),
                    Pressure_2 = sd.Pressure_2.GetValueOrDefault(),
                    TotalRadn = sd.TotalRadn.GetValueOrDefault(),
                    QuantRadn = sd.QuantRadn.GetValueOrDefault(),
                    WndDir_2m = sd.WndDir_2m.GetValueOrDefault(),
                    WndDir_10m = sd.WndDir_10m.GetValueOrDefault(),
                    WndSpd_2m = sd.WndSpd_2m.GetValueOrDefault(),
                    WndSpd_10m = sd.WndSpd_10m.GetValueOrDefault(),
                    WndSpd_Vert = sd.WndSpd_Vert.GetValueOrDefault(),
                    WndSpd_2m_Max = sd.WndSpd_2m_Max.GetValueOrDefault(),
                    WndSpd_10m_Max = sd.WndSpd_10m_Max.GetValueOrDefault(),
                    SoilType = sd.SoilType.GetValueOrDefault(),
                    SoilCond_tc = sd.SoilCond_tc.GetValueOrDefault(),
                    SoilWaCond_tc = sd.SoilWaCond_tc.GetValueOrDefault(),
                    Temp_C = sd.Temp_C.GetValueOrDefault(),
                    wfv = sd.wfv.GetValueOrDefault()
                })
                
                .FirstOrDefaultAsync();
            
                
        }
    }
}
