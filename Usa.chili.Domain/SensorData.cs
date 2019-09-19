using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class SensorData
    {
        public string StationKey { get; set; }
        public string Type { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNo { get; set; }
        public string SerialNo { get; set; }
        public DateTime? CommissioningDate { get; set; }
        public string Misc { get; set; }
    }
}
