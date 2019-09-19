using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class NonAmbientSourceData
    {
        public string StationKey { get; set; }
        public double Distance { get; set; }
        public double? Height { get; set; }
        public double? AngularWidth { get; set; }
        public double Azimuth { get; set; }
        public string Material { get; set; }
    }
}
