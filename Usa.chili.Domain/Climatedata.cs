using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class Climatedata
    {
        public string Year { get; set; }
        public string Month { get; set; }
        [Key]
        public string StationKey { get; set; }
        public double? AirT2m { get; set; }
        public double? AirT1pt5m { get; set; }
        public double? PrecipTb3 { get; set; }
        public double? PrecipTx { get; set; }
        public double? PctColl { get; set; }
    }
}
