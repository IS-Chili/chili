using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class NonAmbientSourceData
    {
        [Key]
        public string StationKey { get; set; }
        public double Distance { get; set; }
        public double? Height { get; set; }
        public double? AngularWidth { get; set; }
        public double Azimuth { get; set; }
        public string Material { get; set; }
    }
}
