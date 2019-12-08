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
    /// DTO for station information.
    /// </summary>
    public class StationDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Elevation { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public string LatitudeDirection {
            get {
                if (Latitude > 0) {
                    return "N";
                } else if (Latitude < 0) {
                    return "S";
                } else {
                    return "";
                }
            }
        }
        public string LongitudeDirection {
            get {
                if (Longitude > 0) {
                    return "E";
                } else if (Longitude < 0) {
                    return "W";
                } else {
                    return "";
                }
            }
        }
    }
}