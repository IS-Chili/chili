// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;

namespace Usa.chili.Dto
{
    public class ExtremeDto
    {
        public DateTime? AirTemperatureHighTimestamp { get; set; }
        public DateTime? AirTemperatureLowTimestamp { get; set; }
        public DateTime? DewPointHighTimestamp { get; set; }
        public DateTime? DewPointLowTimestamp { get; set; }
        public DateTime? RealHumidityHighTimestamp { get; set; }
        public DateTime? RealHumidityLowTimestamp { get; set; }
        public DateTime? WindSpeedMaxTimestamp { get; set; }
        public double? AirTemperatureHigh { get; set; }
        public double? AirTemperatureLow { get; set; }
        public double? DewPointHigh { get; set; }
        public double? DewPointLow { get; set; }
        public double? RealHumidityHigh { get; set; }
        public double? RealHumidityLow { get; set; }
        public double? WindSpeedMax { get; set; }
        public double? Precipitation { get; set; }
    }
}