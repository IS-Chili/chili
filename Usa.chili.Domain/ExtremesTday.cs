using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class ExtremesTday
    {
        public string StationKey { get; set; }
        public DateTime? Ts { get; set; }
        public DateTime? AirT2mTmx { get; set; }
        public double? AirT2mMax { get; set; }
        public DateTime? AirT2mTmn { get; set; }
        public double? AirT2mMin { get; set; }
        public DateTime? DewPt2mTmx { get; set; }
        public double? DewPt2mMax { get; set; }
        public DateTime? DewPt2mTmn { get; set; }
        public double? DewPt2mMin { get; set; }
        public DateTime? Rh2mTmx { get; set; }
        public double? Rh2mMax { get; set; }
        public DateTime? Rh2mTmn { get; set; }
        public double? Rh2mMin { get; set; }
        public DateTime? WndSpd10mTmx { get; set; }
        public double? WndSpd10mMax { get; set; }

        public virtual Station StationKeyNavigation { get; set; }
    }
}
