using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class SiteVisit
    {
        public string StationKey { get; set; }
        public DateTime Arrive { get; set; }
        public DateTime Depart { get; set; }
        public string Technician { get; set; }
        public string PurposeOfVisit { get; set; }
        public string WorkPerformed { get; set; }
    }
}
