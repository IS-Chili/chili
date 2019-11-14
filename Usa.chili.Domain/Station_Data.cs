using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class Station_Data
    {
        public int Id {get; set;}
        public DateTime TS {get; set;}
        public int? RecId {get; set;}
        public int? TableCode {get; set;}
        public int? Year {get; set;}
        public int? Month {get; set;}
        public int? DayOfMon {get; set;}
        public int? DayOfYear {get; set;}
        public int? Hour {get; set;}
        public int? Minute {get; set;}
        public int? StationId {get; set;}
        public virtual Station Station {get; set;} 
        public double? Lat {get; set;}
        public double? Lon {get; set;}
        public double? Elev {get; set;}
        public double? Sign {get; set;}
        public int? Door {get; set;}
        public double? Batt {get; set;}
        public int? ObsInSumm_Tot {get; set;}
        public double? Precip_TB3_Tot {get; set;}
        public double? Precip_TX_Tot {get; set;}
        public double? Precip_TB3_Today {get; set;}
        public double? Precip_TX_Today {get; set;}
        public double? PTemp {get; set;}
        public double? IRTS_Trgt {get; set;}
        public double? IRTS_Body {get; set;}
        public double? SoilSfcT {get; set;}
        public double? SoilT_5cm {get; set;}
        public double? SoilT_10cm {get; set;}
        public double? SoilT_20cm {get; set;}
        public double? SoilT_50cm {get; set;}
        public double? SoilT_100cm {get; set;}
        public double? AirT_1pt5m {get; set;}
        public double? AirT_2m {get; set;}
        public double? AirT_9pt5m {get; set;}
        public double? AirT_10m {get; set;}
        public double? RH_2m {get; set;}
        public double? RH_10m {get; set;}
        public double? Pressure_1 {get; set;}
        public double? Pressure_2 {get; set;}
        public double? TotalRadn {get; set;}
        public double? QuantRadn {get; set;}
        public double? WndDir_2m {get; set;}
        public double? WndDir_10m {get; set;}
        public double? WndSpd_2m {get; set;}
        public double? WndSpd_10m {get; set;}
        public double? WndSpd_Vert {get; set;}
        public double? Vitel_5cm_a {get; set;}
        public double? Vitel_5cm_b {get; set;}
        public double? Vitel_5cm_c {get; set;}
        public double? Vitel_5cm_d {get; set;}
        public double? Vitel_10cm_a {get; set;}
        public double? Vitel_10cm_b {get; set;}
        public double? Vitel_10cm_c {get; set;}
        public double? Vitel_10cm_d {get; set;}
        public double? Vitel_20cm_a {get; set;}
        public double? Vitel_20cm_b {get; set;}
        public double? Vitel_20cm_c {get; set;}
        public double? Vitel_20cm_d {get; set;}
        public double? Vitel_50cm_a {get; set;}
        public double? Vitel_50cm_b {get; set;}
        public double? Vitel_50cm_c {get; set;}
        public double? Vitel_50cm_d {get; set;}
        public double? Vitel_100cm_a {get; set;}
        public double? Vitel_100cm_b {get; set;}
        public double? Vitel_100cm_c {get; set;}
        public double? Vitel_100cm_d {get; set;}
        public double? WndSpd_2m_Max {get; set;}
        public double? WndSpd_10m_Max {get; set;}
        public double? WndSpd_Vert_Max {get; set;}
        public double? WndSpd_Vert_Min {get; set;}
        public double? WndSpd_Vert_Tot {get; set;}
        public double? WndSpd_2m_WVc_1 {get; set;}
        public double? WndSpd_2m_WVc_2 {get; set;}
        public double? WndSpd_2m_WVc_3 {get; set;}
        public double? WndSpd_2m_WVc_4 {get; set;}
        public double? WndSpd_10m_WVc_1 {get; set;}
        public double? WndSpd_10m_WVc_2 {get; set;}
        public double? WndSpd_10m_WVc_3 {get; set;}
        public double? WndSpd_10m_WVc_4 {get; set;}
        public double? WndSpd_2m_Std {get; set;}
        public double? WndSpd_10m_Std {get; set;}
        public double? SoilType {get; set;}
        public double? eR {get; set;}
        public double? eI {get; set;}
        public double? Temp_C {get; set;}
        public double? eR_tc {get; set;}
        public double? eI_tc {get; set;}
        public double? wfv {get; set;}
        public double? NaCI {get; set;}
        public double? SoilCond {get; set;}
        public double? SoilCond_tc {get; set;}
        public double? SoilWaCond_tc {get; set;}
 
    } 
}