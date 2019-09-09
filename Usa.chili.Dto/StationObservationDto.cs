// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using Newtonsoft.Json;
using Usa.chili.Common.Converters;

namespace Usa.chili.Dto
{
    public class StationObservationDto
    {
        public string StationName { get; set; }
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime StationTimestamp { get; set; }
        public double AirTemperature { get; set; }
        public double DewPoint { get; set; }
        public double RealHumidity { get; set; }
        public double WindDirection { get; set; }
        public double WindSpeed { get; set; }
        public double Pressure { get; set; }
        public double Precipitation { get; set; }
    }
}