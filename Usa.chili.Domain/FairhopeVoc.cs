using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class FairhopeVoc
    {
        public DateTime Ts { get; set; }
        public uint? RecId { get; set; }
        public ushort? TableCode { get; set; }
        public ushort? Year { get; set; }
        public byte? Month { get; set; }
        public byte? DayOfMon { get; set; }
        public ushort? DayOfYear { get; set; }
        public byte? Hour { get; set; }
        public byte? Minute { get; set; }
        public ushort? StationId { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public double? Elev { get; set; }
        public double? Sign { get; set; }
        public double? RaeguardAvg { get; set; }
        public double? BattMin { get; set; }
    }
}
