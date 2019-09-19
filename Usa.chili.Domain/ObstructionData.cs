using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class ObstructionData
    {
        public string StationKey { get; set; }
        public double Azimuth { get; set; }
        public double Distance { get; set; }
        public double? Horizontal { get; set; }
        public double? Height { get; set; }
        public string Type { get; set; }
    }
}
