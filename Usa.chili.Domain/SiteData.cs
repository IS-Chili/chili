using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class SiteData
    {
        public string StationKey { get; set; }
        public DateTime? DateInstalled { get; set; }
        public string DistributionRestrictions { get; set; }
        public string PositionDescription { get; set; }
        public string VegetationTypes { get; set; }
        public string Land { get; set; }
        public DateTime? MaintenanceFrequency { get; set; }
        public string MaintenanceScheduled { get; set; }
        public string TransmissionMethod { get; set; }
        public string DataFormat { get; set; }
        public DateTime? TransmissionFrequency { get; set; }
        public string SoilType { get; set; }
        public string Timezone { get; set; }
    }
}
