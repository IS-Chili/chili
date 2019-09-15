using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class Pascagoula202Flags
    {
        [Key]
        public DateTime Ts { get; set; }
        public byte? RecIdFlag { get; set; }
        public byte? TableCodeFlag { get; set; }
        public byte? YearFlag { get; set; }
        public byte? MonthFlag { get; set; }
        public byte? DayOfMonFlag { get; set; }
        public byte? DayOfYearFlag { get; set; }
        public byte? HourFlag { get; set; }
        public byte? MinuteFlag { get; set; }
        public byte? StationIdFlag { get; set; }
        public byte? LatFlag { get; set; }
        public byte? LonFlag { get; set; }
        public byte? ElevFlag { get; set; }
        public byte? SignFlag { get; set; }
        public byte? DoorFlag { get; set; }
        public byte? BattFlag { get; set; }
        public byte? ObsInSummTotFlag { get; set; }
        public byte? PrecipTb3TotFlag { get; set; }
        public byte? PrecipTxTotFlag { get; set; }
        public byte? PrecipTb3TodayFlag { get; set; }
        public byte? PrecipTxTodayFlag { get; set; }
        public byte? PtempFlag { get; set; }
        public byte? IrtsTrgtFlag { get; set; }
        public byte? IrtsBodyFlag { get; set; }
        public byte? SoilSfcTFlag { get; set; }
        public byte? SoilT5cmFlag { get; set; }
        public byte? SoilT10cmFlag { get; set; }
        public byte? SoilT20cmFlag { get; set; }
        public byte? SoilT50cmFlag { get; set; }
        public byte? SoilT100cmFlag { get; set; }
        public byte? AirT1pt5mFlag { get; set; }
        public byte? AirT2mFlag { get; set; }
        public byte? AirT9pt5mFlag { get; set; }
        public byte? AirT10mFlag { get; set; }
        public byte? Rh2mFlag { get; set; }
        public byte? Rh10mFlag { get; set; }
        public byte? Pressure1Flag { get; set; }
        public byte? Pressure2Flag { get; set; }
        public byte? TotalRadnFlag { get; set; }
        public byte? QuantRadnFlag { get; set; }
        public byte? WndDir2mFlag { get; set; }
        public byte? WndDir10mFlag { get; set; }
        public byte? WndSpd2mFlag { get; set; }
        public byte? WndSpd10mFlag { get; set; }
        public byte? WndSpdVertFlag { get; set; }
        public byte? Vitel5cmAFlag { get; set; }
        public byte? Vitel5cmBFlag { get; set; }
        public byte? Vitel5cmCFlag { get; set; }
        public byte? Vitel5cmDFlag { get; set; }
        public byte? Vitel10cmAFlag { get; set; }
        public byte? Vitel10cmBFlag { get; set; }
        public byte? Vitel10cmCFlag { get; set; }
        public byte? Vitel10cmDFlag { get; set; }
        public byte? Vitel20cmAFlag { get; set; }
        public byte? Vitel20cmBFlag { get; set; }
        public byte? Vitel20cmCFlag { get; set; }
        public byte? Vitel20cmDFlag { get; set; }
        public byte? Vitel50cmAFlag { get; set; }
        public byte? Vitel50cmBFlag { get; set; }
        public byte? Vitel50cmCFlag { get; set; }
        public byte? Vitel50cmDFlag { get; set; }
        public byte? Vitel100cmAFlag { get; set; }
        public byte? Vitel100cmBFlag { get; set; }
        public byte? Vitel100cmCFlag { get; set; }
        public byte? Vitel100cmDFlag { get; set; }
        public byte? WndSpd2mMaxFlag { get; set; }
        public byte? WndSpd10mMaxFlag { get; set; }
        public byte? WndSpdVertMaxFlag { get; set; }
        public byte? WndSpdVertMinFlag { get; set; }
        public byte? WndSpdVertTotFlag { get; set; }
        public byte? WndSpd2mWvc1Flag { get; set; }
        public byte? WndSpd2mWvc2Flag { get; set; }
        public byte? WndSpd2mWvc3Flag { get; set; }
        public byte? WndSpd2mWvc4Flag { get; set; }
        public byte? WndSpd10mWvc1Flag { get; set; }
        public byte? WndSpd10mWvc2Flag { get; set; }
        public byte? WndSpd10mWvc3Flag { get; set; }
        public byte? WndSpd10mWvc4Flag { get; set; }
        public byte? WndSpd2mStdFlag { get; set; }
        public byte? WndSpd10mStdFlag { get; set; }
        public byte? SoilTypeFlag { get; set; }
        public byte? ERFlag { get; set; }
        public byte? EIFlag { get; set; }
        public byte? TempCFlag { get; set; }
        public byte? ERTcFlag { get; set; }
        public byte? EITcFlag { get; set; }
        public byte? WfvFlag { get; set; }
        public byte? NaCiFlag { get; set; }
        public byte? SoilCondFlag { get; set; }
        public byte? SoilCondTcFlag { get; set; }
        public byte? SoilWaCondTcFlag { get; set; }
    }
}
