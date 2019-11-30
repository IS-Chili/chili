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
        public DateTime StationTimestamp { get; set; }
        public string Door {get; set;}
        public string Battery {get; set;}
        public string Precip_TB3_Tot {get; set;}
        public string Precip_TB3_Tot_en {get; set;}
        public string Precip_TX_Tot {get; set;}
        public string Precip_TX_Tot_en {get; set;}
        public string Precip_TB3_Today {get; set;}
        public string Precip_TB3_Today_en {get; set;}
        public string Precip_TX_Today {get; set;}
        public string Precip_TX_Today_en {get; set;}
        public string SoilSfcT {get; set;}
         public string SoilSfcT_en {get; set;}
        public string SoilT_5cm {get; set;}
        public string SoilT_5cm_en {get; set;}
        public string SoilT_10cm {get; set;}
        public string SoilT_10cm_en {get; set;}
        public string SoilT_20cm {get; set;}
        public string SoilT_20cm_en {get; set;}
        public string SoilT_50cm {get; set;}
         public string SoilT_50cm_en {get; set;}
        public string SoilT_100cm {get; set;}
        public string SoilT_100cm_en {get; set;}
        public string AirT_1pt5m {get; set;}
        public string AirT_1pt5m_en {get; set;}    
        public string AirT_2m {get; set;}
        public string AirT_2m_en {get; set;}
        public string AirT_9pt5m {get; set;}
        public string AirT_9pt5m_en {get; set;}
        public string AirT_10m {get; set;}
         public string AirT_10m_en {get; set;}
        public string RH_2m {get; set;}
        public string RH_10m {get; set;}
        public string Pressure_1 {get; set;}
        public string Pressure_1_en {get; set;}
        public string Pressure_2 {get; set;}
        public string Pressure_2_en {get; set;}
        public string TotalRadn {get; set;}
        public string TotalRadn_en {get; set;}
        public string QuantRadn {get; set;}
        public string WndDir_2m {get; set;}
        public string WndDir_10m {get; set;}
        public string WndSpd_2m {get; set;}
        public string WndSpd_2m_en {get; set;}
        public string WndSpd_10m {get; set;}
        public string WndSpd_10m_en {get; set;}
        public string WndSpd_Vert {get; set;}
        public string WndSpd_Vert_en {get; set;}
        public string WndSpd_2m_Max {get; set;}
        public string WndSpd_2m_Max_en {get; set;}
        public string WndSpd_10m_Max {get; set;}
        public string WndSpd_10m_Max_en {get; set;}
        public string SoilType {get; set;}
        public string Temp_C {get; set;}
        public string Temp_C_en {get; set;}
        public string Temp_F {get; set;}
        public string wfv {get; set;}
        public string SoilCond_tc {get; set;}
        public string SoilWaCond_tc {get; set;}
    
    }
}