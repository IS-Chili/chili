// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;

namespace Usa.chili.Dto
{
    /// <summary>
    /// DTO for realtime data.
    /// </summary>
    public class RealtimeDataDto
    {
        public uint StationId { get; set; }
        public bool IsStationOffline { get; set; }
        public bool IsWindChill { get; set; }
        public string StationName { get; set; }
        public DateTime? StationTimestamp { get; set; }
        public double? AirTemperature { get; set; }
        public double? Felt { get; set; }
        public double? DewPoint { get; set; }
        public double? RealHumidity { get; set; }
        public double? WindDirection { get; set; }
        public double? WindSpeed { get; set; }
        public double? Pressure { get; set; }
        public double? Precipitation { get; set; }
        public ExtremeDto TodayExtreme { get; set; }
        public ExtremeDto YesterdayExtreme { get; set; }
    }
}