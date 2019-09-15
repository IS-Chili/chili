using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class RoughnessData
    {
        [Key]
        public string StationKey { get; set; }
        public double AngleToNorth { get; set; }
        public byte RoughnessClassification { get; set; }
    }
}
