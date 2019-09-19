using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class GasqueVoc
    {
        public DateTime Ts { get; set; }
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
        public double? RaeguardAvg { get; set; }
        public double? BattMin { get; set; }
    }
}
