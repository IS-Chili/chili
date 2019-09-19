using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class Windtest
    {
        public string StationKey { get; set; }
        public byte? WindEvent { get; set; }
        public DateTime? GustTime { get; set; }
        public double? GustSpeed { get; set; }
    }
}
