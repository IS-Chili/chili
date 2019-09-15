using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class Extremes
    {
        [Key]
        public string StationKey { get; set; }
        public double? AirT1pt5mMax { get; set; }
        public DateTime? AirT1pt5mTmx { get; set; }
        public double? AirT1pt5mMin { get; set; }
        public DateTime? AirT1pt5mTmn { get; set; }
        public double? AirT2mMax { get; set; }
        public DateTime? AirT2mTmx { get; set; }
        public double? AirT2mMin { get; set; }
        public DateTime? AirT2mTmn { get; set; }
        public double? AirT9pt5mMax { get; set; }
        public DateTime? AirT9pt5mTmx { get; set; }
        public double? AirT9pt5mMin { get; set; }
        public DateTime? AirT9pt5mTmn { get; set; }
        public double? AirT10mMax { get; set; }
        public DateTime? AirT10mTmx { get; set; }
        public double? AirT10mMin { get; set; }
        public DateTime? AirT10mTmn { get; set; }
        public double? IrtsBodyMax { get; set; }
        public DateTime? IrtsBodyTmx { get; set; }
        public double? IrtsBodyMin { get; set; }
        public DateTime? IrtsBodyTmn { get; set; }
        public double? IrtsTrgtMax { get; set; }
        public DateTime? IrtsTrgtTmx { get; set; }
        public double? IrtsTrgtMin { get; set; }
        public DateTime? IrtsTrgtTmn { get; set; }
        public double? PressureMax1 { get; set; }
        public DateTime? PressureTmx1 { get; set; }
        public double? PressureMax2 { get; set; }
        public DateTime? PressureTmx2 { get; set; }
        public double? PressureMin1 { get; set; }
        public DateTime? PressureTmn1 { get; set; }
        public double? PressureMin2 { get; set; }
        public DateTime? PressureTmn2 { get; set; }
        public double? PtempMax { get; set; }
        public DateTime? PtempTmx { get; set; }
        public double? PtempMin { get; set; }
        public DateTime? PtempTmn { get; set; }
        public double? QuantRadnMax { get; set; }
        public DateTime? QuantRadnTmx { get; set; }
        public double? QuantRadnMin { get; set; }
        public DateTime? QuantRadnTmn { get; set; }
        public double? Rh2mMax { get; set; }
        public DateTime? Rh2mTmx { get; set; }
        public double? Rh2mMin { get; set; }
        public DateTime? Rh2mTmn { get; set; }
        public double? Rh10mMax { get; set; }
        public DateTime? Rh10mTmx { get; set; }
        public double? Rh10mMin { get; set; }
        public DateTime? Rh10mTmn { get; set; }
        public double? SoilSfcTMax { get; set; }
        public DateTime? SoilSfcTTmx { get; set; }
        public double? SoilSfcTMin { get; set; }
        public DateTime? SoilSfcTTmn { get; set; }
        public double? SoilT5cmMax { get; set; }
        public DateTime? SoilT5cmTmx { get; set; }
        public double? SoilT5cmMin { get; set; }
        public DateTime? SoilT5cmTmn { get; set; }
        public double? SoilT10cmMax { get; set; }
        public DateTime? SoilT10cmTmx { get; set; }
        public double? SoilT10cmMin { get; set; }
        public DateTime? SoilT10cmTmn { get; set; }
        public double? SoilT20cmMax { get; set; }
        public DateTime? SoilT20cmTmx { get; set; }
        public double? SoilT20cmMin { get; set; }
        public DateTime? SoilT20cmTmn { get; set; }
        public double? SoilT50cmMax { get; set; }
        public DateTime? SoilT50cmTmx { get; set; }
        public double? SoilT50cmMin { get; set; }
        public DateTime? SoilT50cmTmn { get; set; }
        public double? SoilT100cmMax { get; set; }
        public DateTime? SoilT100cmTmx { get; set; }
        public double? SoilT100cmMin { get; set; }
        public DateTime? SoilT100cmTmn { get; set; }
        public double? TotalRadnMax { get; set; }
        public DateTime? TotalRadnTmx { get; set; }
        public double? TotalRadnMin { get; set; }
        public DateTime? TotalRadnTmn { get; set; }
        public double? WndDir2mMax { get; set; }
        public DateTime? WndDir2mTmx { get; set; }
        public double? WndDir2mMin { get; set; }
        public DateTime? WndDir2mTmn { get; set; }
        public double? WndDir10mMax { get; set; }
        public DateTime? WndDir10mTmx { get; set; }
        public double? WndDir10mMin { get; set; }
        public DateTime? WndDir10mTmn { get; set; }
        public double? WndSpd2mMax { get; set; }
        public DateTime? WndSpd2mTmx { get; set; }
        public double? WndSpd2mMin { get; set; }
        public DateTime? WndSpd2mTmn { get; set; }
        public double? WndSpd10mMax { get; set; }
        public DateTime? WndSpd10mTmx { get; set; }
        public double? WndSpd10mMin { get; set; }
        public DateTime? WndSpd10mTmn { get; set; }
        public double? WndSpdVertMax { get; set; }
        public DateTime? WndSpdVertTmx { get; set; }
        public double? WndSpdVertMin { get; set; }
        public DateTime? WndSpdVertTmn { get; set; }
    }
}
