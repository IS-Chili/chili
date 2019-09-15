using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class SiteVisit
    {
        [Key]
        public string StationKey { get; set; }
        public DateTime Arrive { get; set; }
        public DateTime Depart { get; set; }
        public string Technician { get; set; }
        public string PurposeOfVisit { get; set; }
        public string WorkPerformed { get; set; }
    }
}
