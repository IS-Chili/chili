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
        public async Task<MeteorologicalDataDto> GetMeteorologicalData(int? id, DateTime? dateTime) {
            // Some handy metric to english conversion variables
            var ninefifths = 9/5;
            var mm2inches = 0.0393700787;
            var mb2inHg = 0.029529983071;
            var wpsqm2lymin = 0.00143197;
            var mps2Mph = 2.2369363;

            var mdDto = new MeteorologicalDataDto();

            // Query database for all data on indicated station at the most recent Time Stamp
            // for that station
            var md = await _dbContext.Station_Data
                .Include(sd => sd.Station)
                .Where(sd => sd.StationId == id)
                .OrderByDescending(sd => sd.TS)
                .FirstOrDefaultAsync();

            // Start building MeteorologicalDataDto from Station_Data object 
            // retrieved in above database query
            mdDto.StationID = md.StationId.GetValueOrDefault();
            mdDto.StationName = md.Station.DisplayName;
            mdDto.StationTimestamp = md.TS;

            if (md.Door == null) {
               mdDto.Door = "N/A";
            } else if (md.Door == 1) {
                  mdDto.Door = "Open";
               } else {
                    mdDto.Door = "Closed";
                  }

            // Apply rounding, calculate English equivalent measurements, and set NULL values to "N/A"
            if (md.Batt == null) {
               mdDto.Battery = "N/A";
            } else {
                  mdDto.Battery = Math.Round(md.Batt.GetValueOrDefault(),2).ToString();
               }
            
            if (md.Precip_TB3_Tot != null) {
               mdDto.Precip_TB3_Tot_en = Math.Round(md.Precip_TB3_Tot.GetValueOrDefault() * mm2inches,3).ToString();
               mdDto.Precip_TB3_Tot = Math.Round(md.Precip_TB3_Tot.GetValueOrDefault(),3).ToString();
            } else {
                  mdDto.Precip_TB3_Tot = "N/A";
                  mdDto.Precip_TB3_Tot_en = "N/A";
               }
               
            if (md.Precip_TX_Tot != null) {
               mdDto.Precip_TX_Tot_en = Math.Round(md.Precip_TX_Tot.GetValueOrDefault() * mm2inches,3).ToString();
               mdDto.Precip_TX_Tot    = Math.Round(md.Precip_TX_Tot.GetValueOrDefault(),3).ToString();
            } else {
                  mdDto.Precip_TX_Tot = "N/A";
                  mdDto.Precip_TX_Tot_en = "N/A";
               }

            if (md.Precip_TB3_Today != null) {
               mdDto.Precip_TB3_Today_en = Math.Round(md.Precip_TB3_Today.GetValueOrDefault() * mm2inches,3).ToString();
               mdDto.Precip_TB3_Today    = Math.Round(md.Precip_TB3_Today.GetValueOrDefault(),3).ToString();
            } else {
                  mdDto.Precip_TB3_Today = "N/A";
                  mdDto.Precip_TB3_Today_en = "N/A";
               }

            if (md.Precip_TX_Today != null) {
               mdDto.Precip_TX_Today_en = Math.Round( md.Precip_TX_Today.GetValueOrDefault() * mm2inches,3).ToString();
               mdDto.Precip_TX_Today    = Math.Round(md.Precip_TX_Today.GetValueOrDefault(),3).ToString();
            } else {
                  mdDto.Precip_TX_Today = "N/A";
                  mdDto.Precip_TX_Today_en = "N/A";
               }

            if (md.SoilSfcT != null) {
               mdDto.SoilSfcT_en = Math.Round(ninefifths * md.SoilSfcT.GetValueOrDefault() + 32,2).ToString();
               mdDto.SoilSfcT    = Math.Round(md.SoilSfcT.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.SoilSfcT = "N/A";
                  mdDto.SoilSfcT_en = "N/A";
               }

            if (md.SoilT_5cm != null) {
               mdDto.SoilT_5cm_en = Math.Round(ninefifths * md.SoilT_5cm.GetValueOrDefault() + 32,2).ToString();
               mdDto.SoilT_5cm    = Math.Round(md.SoilT_5cm.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.SoilT_5cm = "N/A";
                  mdDto.SoilT_5cm_en = "N/A";
               }

            if (md.SoilT_10cm != null) {
                  mdDto.SoilT_10cm_en = Math.Round(ninefifths * md.SoilT_10cm.GetValueOrDefault() + 32,2).ToString();
                  mdDto.SoilT_10cm    = Math.Round(md.SoilT_10cm.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.SoilT_10cm = "N/A";
                  mdDto.SoilT_10cm_en = "N/A";
               }

            if (md.SoilT_20cm!= null) {
               mdDto.SoilT_20cm_en = Math.Round(ninefifths * md.SoilT_20cm.GetValueOrDefault() + 32,2).ToString();
               mdDto.SoilT_20cm    = Math.Round(md.SoilT_20cm.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.SoilT_20cm = "N/A";
                  mdDto.SoilT_20cm_en = "N/A";
               }

            if (md.SoilT_50cm !=null) {
               mdDto.SoilT_50cm_en = Math.Round(ninefifths * md.SoilT_50cm.GetValueOrDefault() + 32,2).ToString();
               mdDto.SoilT_50cm    = Math.Round(md.SoilT_50cm.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.SoilT_50cm = "N/A";
                  mdDto.SoilT_50cm_en = "N/A";
               }

            if (md.SoilT_100cm != null) {
               mdDto.SoilT_100cm_en = Math.Round(ninefifths * md.SoilT_100cm.GetValueOrDefault() + 32,2).ToString();
               mdDto.SoilT_100cm    = Math.Round(md.SoilT_100cm.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.SoilT_100cm = "N/A";
                  mdDto.SoilT_100cm_en = "N/A";
               }

            if (md.AirT_1pt5m != null) {
               mdDto.AirT_1pt5m_en = Math.Round(ninefifths * md.AirT_1pt5m.GetValueOrDefault() + 32,2).ToString();
               mdDto.AirT_1pt5m    = Math.Round(md.AirT_1pt5m.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.AirT_1pt5m = "N/A";
                  mdDto.AirT_1pt5m_en = "N/A";
               }

            if (md.AirT_2m != null) {
               mdDto.AirT_2m_en = Math.Round(ninefifths * md.AirT_2m.GetValueOrDefault() + 32,2).ToString();
               mdDto.AirT_2m    = Math.Round(md.AirT_2m.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.AirT_2m = "N/A";
                  mdDto.AirT_2m_en = "N/A";
               }

            if (md.AirT_9pt5m != null) {
               mdDto.AirT_9pt5m_en = Math.Round(ninefifths * md.AirT_9pt5m.GetValueOrDefault() + 32,2).ToString();
               mdDto.AirT_9pt5m    = Math.Round(md.AirT_9pt5m.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.AirT_9pt5m = "N/A";
                  mdDto.AirT_9pt5m_en = "N/A";
               }

            if (md.AirT_10m != null) {
               mdDto.AirT_10m_en = Math.Round(ninefifths * md.AirT_10m.GetValueOrDefault() + 32,2).ToString();
               mdDto.AirT_10m    = Math.Round(md.AirT_10m.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.AirT_10m = "N/A";
                  mdDto.AirT_10m_en = "N/A";
               }
               
            if (md.RH_2m == null) {
               mdDto.RH_2m = "N/A";
            } else {
                  mdDto.RH_2m = Math.Round(md.RH_2m.GetValueOrDefault(),2).ToString();
               }

            if (md.RH_10m == null) {
               mdDto.RH_10m = "N/A";
            } else {
                  mdDto.RH_10m = Math.Round(md.RH_10m.GetValueOrDefault(),2).ToString();
               }

            if (md.Pressure_1 != null) {
               mdDto.Pressure_1_en = Math.Round(md.Pressure_1.GetValueOrDefault() * mb2inHg,2).ToString();
               mdDto.Pressure_1    = Math.Round(md.Pressure_1.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.Pressure_1 = "N/A";
                  mdDto.Pressure_1_en = "N/A";
               }

            if (md.Pressure_2 != null) {
               mdDto.Pressure_2_en = Math.Round(md.Pressure_2.GetValueOrDefault() * mb2inHg,2).ToString();
               mdDto.Pressure_2    = Math.Round(md.Pressure_2.GetValueOrDefault(),2).ToString();
            } else {
               mdDto.Pressure_2 = "N/A";
               mdDto.Pressure_2_en = "N/A";
               }

            if (md.TotalRadn!= null) {
               mdDto.TotalRadn_en = Math.Round(md.TotalRadn.GetValueOrDefault() * wpsqm2lymin,5).ToString();
               mdDto.TotalRadn    = Math.Round(md.TotalRadn.GetValueOrDefault(),5).ToString();
            } else {
                  mdDto.TotalRadn = "N/A";
                  mdDto.TotalRadn_en = "N/A";
               }

            if (md.QuantRadn == null) {
               mdDto.QuantRadn = "N/A";
            } else {
                  mdDto.QuantRadn = Math.Round(md.QuantRadn.GetValueOrDefault(),2).ToString();
               }

            if (md.WndDir_2m == null) {
               mdDto.WndDir_2m = "N/A";
            } else {
                  mdDto.WndDir_2m = Math.Round(md.WndDir_2m.GetValueOrDefault(),1).ToString();
               }

            if (md.WndDir_10m == null) {
               mdDto.WndDir_10m = "N/A";
            } else {
                  mdDto.WndDir_10m = Math.Round(md.WndDir_10m.GetValueOrDefault(),1).ToString();
               }

            if (md.WndSpd_2m != null) {
               mdDto.WndSpd_2m_en = Math.Round(md.WndSpd_2m.GetValueOrDefault() * mps2Mph,2).ToString();
               mdDto.WndSpd_2m    = Math.Round(md.WndSpd_2m.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.WndSpd_2m = "N/A";
                  mdDto.WndSpd_2m_en = "N/A";
               }

            if (md.WndSpd_10m != null) {
               mdDto.WndSpd_10m_en = Math.Round(md.WndSpd_10m.GetValueOrDefault() * mps2Mph,2).ToString();
               mdDto.WndSpd_10m    = Math.Round(md.WndSpd_10m.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.WndSpd_10m = "N/A";
                  mdDto.WndSpd_10m_en = "N/A";
               }

            if (md.WndSpd_Vert != null) {
               mdDto.WndSpd_Vert_en = Math.Round(md.WndSpd_Vert.GetValueOrDefault() * mps2Mph,2).ToString();
               mdDto.WndSpd_Vert    = Math.Round(md.WndSpd_Vert.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.WndSpd_Vert = "N/A";
                  mdDto.WndSpd_Vert_en = "N/A";
               }

            if (md.WndSpd_2m_Max != null) {
               mdDto.WndSpd_2m_Max_en = Math.Round(md.WndSpd_2m_Max.GetValueOrDefault() * mps2Mph,2).ToString();
               mdDto.WndSpd_2m_Max    = Math.Round(md.WndSpd_2m_Max.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.WndSpd_2m_Max = "N/A";
                  mdDto.WndSpd_2m_Max_en = "N/A";
               }

            if (md.WndSpd_10m_Max != null) {
               mdDto.WndSpd_10m_Max_en = Math.Round(md.WndSpd_10m_Max.GetValueOrDefault() * mps2Mph,2).ToString();
               mdDto.WndSpd_10m_Max    = Math.Round(md.WndSpd_10m_Max.GetValueOrDefault(),2).ToString();
            } else {
                  mdDto.WndSpd_10m_Max = "N/A";
                  mdDto.WndSpd_10m_Max_en = "N/A";
               }

            if (md.wfv == null) {
               mdDto.wfv = "N/A";
            } else {
                  mdDto.wfv = Math.Round(md.wfv.GetValueOrDefault() * 100,2).ToString();
               }

            if (md.SoilCond_tc == null) {
               mdDto.SoilCond_tc = "N/A";
            } else {
                  mdDto.SoilCond_tc = Math.Round(md.SoilCond_tc.GetValueOrDefault(),2).ToString();
               }

            if (md.SoilWaCond_tc == null) {
               mdDto.SoilWaCond_tc = "N/A";
            } else {
                  mdDto.SoilWaCond_tc = Math.Round(md.SoilWaCond_tc.GetValueOrDefault(),2).ToString();
               }

            if (md.SoilType == null) {
               mdDto.SoilType = "N/A";
            } else if (md.SoilType == 1) {
               mdDto.SoilType = "Sand";
            } else if (md.SoilType == 2) {
               mdDto.SoilType = "Silt";
            } else if (md.SoilType == 3) {
               mdDto.SoilType = "Clay";
            } else {
               mdDto.SoilType = "N/A";
            }

            if (md.Temp_C != null) {
               mdDto.Temp_C_en = Math.Round(ninefifths * md.Temp_C.GetValueOrDefault() + 32,2).ToString();
               mdDto.Temp_C    = Math.Round(md.Temp_C.GetValueOrDefault(),2).ToString();
            } else {
               mdDto.Temp_C = "N/A";
               mdDto.Temp_C_en = "N/A";
               }

            return mdDto;   

        } // GetMeteorologicalData

   } // StationDataService
      
}

            
            /*return await _dbContext.Station_Data
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
                
                .FirstOrDefaultAsync();*/
