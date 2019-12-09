using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class RoughnessData
    {
        public string StationKey { get; set; }
        public double AngleToNorth { get; set; }
        public sbyte RoughnessClassification { get; set; }
    }
}
