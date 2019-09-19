using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class Stormtrack
    {
        public short Year { get; set; }
        public byte Stormnum { get; set; }
        public DateTime Ts { get; set; }
        public string Advisory { get; set; }
        public string Stage { get; set; }
        public string Name { get; set; }
        public float? Lat { get; set; }
        public float? Lon { get; set; }
        public float? WindKts { get; set; }
        public float? PresMb { get; set; }
    }
}
