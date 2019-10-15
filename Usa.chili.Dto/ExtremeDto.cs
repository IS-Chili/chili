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
    public class ExtremeDto
    {
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime AirTemperatureHighTimestamp { get; set; }
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime AirTemperatureLowTimestamp { get; set; }
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime DewPointHighTimestamp { get; set; }
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime DewPointLowTimestamp { get; set; }
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime RealHumidityHighTimestamp { get; set; }
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime RealHumidityLowTimestamp { get; set; }
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime WindSpeedMaxTimestamp { get; set; }
        public double AirTemperatureHigh { get; set; }
        public double AirTemperatureLow { get; set; }
        public double DewPointHigh { get; set; }
        public double DewPointLow { get; set; }
        public double RealHumidityHigh { get; set; }
        public double RealHumidityLow { get; set; }
        public double WindSpeedMax { get; set; }
        public double Precipitation { get; set; }
    }
}