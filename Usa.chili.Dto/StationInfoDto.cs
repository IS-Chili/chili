// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;

namespace Usa.chili.Dto
{
    public class StationInfoDto
    {
        public int StationID {get; set;}
        public DateTime StationTimeStamp {get; set;}
        public string StationKey {get; set;}
        public string DisplayName {get; set;}
        public decimal Latitude {get; set;}
        public decimal Longitude {get; set;}
        public decimal Elevation {get; set;}
        public DateTime BeginDate {get; set;}
        public DateTime EndDate {get; set;}
        public int IsActive {get; set;}
    }
}