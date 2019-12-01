using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class Public
    {
        public string StationKey { get; set; }
        public DateTime? Ts { get; set; }
        public int? RecId { get; set; }
        public short? TableCode { get; set; }
        public short? Year { get; set; }
        public byte? Month { get; set; }
        public byte? DayOfMon { get; set; }
        public short? DayOfYear { get; set; }
        public byte? Hour { get; set; }
        public byte? Minute { get; set; }
        public short? StationId { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public double? Elev { get; set; }
        public double? Sign { get; set; }
        public byte? DoorOpen1 { get; set; }
        public byte? DoorOutput { get; set; }
        public double? Batt { get; set; }
        public int? ObsInSumm { get; set; }
        public double? PrecipTb3 { get; set; }
        public double? PrecipTx { get; set; }
        public double? PrecipTb3Today { get; set; }
        public double? PrecipTxToday { get; set; }
        public double? Ptemp { get; set; }
        public double? IrtsTrgt { get; set; }
        public double? IrtsBody { get; set; }
        public double? IrtsBodyP2 { get; set; }
        public double? IrtsBodyP3 { get; set; }
        public double? IrtsBodyP4 { get; set; }
        public double? IrtsBodyP5 { get; set; }
        public double? SoilSfcT { get; set; }
        public double? SoilT5cm { get; set; }
        public double? SoilT10cm { get; set; }
        public double? SoilT20cm { get; set; }
        public double? SoilT50cm { get; set; }
        public double? SoilT100cm { get; set; }
        public double? AirT1pt5m { get; set; }
        public double? AirT2m { get; set; }
        public double? AirT9pt5m { get; set; }
        public double? AirT10m { get; set; }
        public double? Rh2m { get; set; }
        public double? Rh10m { get; set; }
        public double? Pressure1 { get; set; }
        public double? Pressure2 { get; set; }
        public double? PressSealev1 { get; set; }
        public double? PressSealev2 { get; set; }
        public double? DP { get; set; }
        public double? TotalRadn { get; set; }
        public double? QuantRadn { get; set; }
        public double? WndDir2m { get; set; }
        public double? WndDir10m { get; set; }
        public double? WndSpd2m { get; set; }
        public double? WndSpd10m { get; set; }
        public double? WndSpdVert { get; set; }
        public double? Vitel5cmA { get; set; }
        public double? Vitel5cmB { get; set; }
        public double? Vitel5cmC { get; set; }
        public double? Vitel5cmD { get; set; }
        public double? Vitel10cmA { get; set; }
        public double? Vitel10cmB { get; set; }
        public double? Vitel10cmC { get; set; }
        public double? Vitel10cmD { get; set; }
        public double? Vitel20cmA { get; set; }
        public double? Vitel20cmB { get; set; }
        public double? Vitel20cmC { get; set; }
        public double? Vitel20cmD { get; set; }
        public double? Vitel50cmA { get; set; }
        public double? Vitel50cmB { get; set; }
        public double? Vitel50cmC { get; set; }
        public double? Vitel50cmD { get; set; }
        public double? Vitel100cmA { get; set; }
        public double? Vitel100cmB { get; set; }
        public double? Vitel100cmC { get; set; }
        public double? Vitel100cmD { get; set; }
        public double? PyranSlope { get; set; }
        public double? PyranIntercept { get; set; }
        public double? Irtspsb { get; set; }
        public double? Irtshsb { get; set; }
        public double? Irtsksb { get; set; }
        public double? Irtssec { get; set; }
        public double? ErrorWtch { get; set; }
        public double? ErrorOver { get; set; }
        public double? ErrorVolt { get; set; }
        public double? Lithium { get; set; }
        public double? ErrorFlsh { get; set; }
        public double? WsmaxToday1 { get; set; }
        public double? WsmaxToday2 { get; set; }
        public double? Raeguard { get; set; }
        public double? SoilType { get; set; }
        public double? ER { get; set; }
        public double? EI { get; set; }
        public double? TempC { get; set; }
        public double? ERTc { get; set; }
        public double? EITc { get; set; }
        public double? Wfv { get; set; }
        public double? NaCi { get; set; }
        public double? SoilCond { get; set; }
        public double? SoilCondTc { get; set; }
        public double? SoilWaCondTc { get; set; }

        public virtual Station StationKeyNavigation { get; set; }
    }
}
