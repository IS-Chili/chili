using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class MonthlySummary
    {
        public string Year { get; set; }
        public string Month { get; set; }
        [Key]
        public string StationKey { get; set; }
        public double? RainTb3Tot { get; set; }
        public double? RainTxTot { get; set; }
        public double? AirTMax { get; set; }
        public DateTime? AirTMaxTs { get; set; }
        public double? AirTMin { get; set; }
        public DateTime? AirTMinTs { get; set; }
        public double? AirTAvg { get; set; }
        public double? HtIdxMax { get; set; }
        public DateTime? HtIdxMaxTs { get; set; }
        public double? HtIdxMin { get; set; }
        public DateTime? HtIdxMinTs { get; set; }
        public double? HtIdxAvg { get; set; }
        public double? WdChlMax { get; set; }
        public DateTime? WdChlMaxTs { get; set; }
        public double? WdChlMin { get; set; }
        public DateTime? WdChlMinTs { get; set; }
        public double? WdChlAvg { get; set; }
        public double? WdSpdMax { get; set; }
        public DateTime? WdSpdMaxTs { get; set; }
    }
}
