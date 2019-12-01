using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class Station
    {
        public Station()
        {
            StationData = new HashSet<StationData>();
        }

        public int Id { get; set; }
        public string StationKey { get; set; }
        public string DisplayName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Elevation { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ExtremesTday ExtremesTday { get; set; }
        public virtual ExtremesYday ExtremesYday { get; set; }
        public virtual Public Public { get; set; }
        public virtual ICollection<StationData> StationData { get; set; }
    }
}
