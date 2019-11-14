// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;

namespace Usa.chili.Dto
{
    public class MeteorologicalDataDto
    {
        public int StationID {get; set;}
        public string StationName { get; set; }
        public DateTime StationTimestamp { get; set; }
        public int Door {get; set;}
        public double Battery {get; set;}
        public double Precip_TB3_Tot {get; set;}
        public double Precip_TX_Tot {get; set;}
        public double Precip_TB3_Today {get; set;}
        public double Precip_TX_Today {get; set;}
        public double SoilSfcT {get; set;}
        public double SoilT_5cm {get; set;}
        public double SoilT_10cm {get; set;}
        public double SoilT_20cm {get; set;}
        public double SoilT_50cm {get; set;}
        public double SoilT_100cm {get; set;}
        public double AirT_1pt5m {get; set;}    
        public double AirT_2m {get; set;}
        public double AirT_9pt5m {get; set;}
        public double AirT_10m {get; set;}
        public double RH_2m {get; set;}
        public double RH_10m {get; set;}
        public double Pressure_1 {get; set;}
        public double Pressure_2 {get; set;}
        public double TotalRadn {get; set;}
        public double QuantRadn {get; set;}
        public double WndDir_2m {get; set;}
        public double WndDir_10m {get; set;}
        public double WndSpd_2m {get; set;}
        public double WndSpd_10m {get; set;}
        public double WndSpd_Vert {get; set;}
        public double WndSpd_2m_Max {get; set;}
        public double WndSpd_10m_Max {get; set;}
        public double SoilType {get; set;}
        public double Temp_C {get; set;}
        public double wfv {get; set;}
        public double SoilCond_tc {get; set;}
        public double SoilWaCond_tc {get; set;}
    
    }
}