// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

namespace Usa.chili.Dto
{
    /// <summary>
    /// DTO for station map.
    /// </summary>
    public class StationMapDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsActive { get; set; }
        public double? AirTemperatureHigh { get; set; }
        public double? AirTemperatureLow { get; set; }
    }
}