using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class Windtest
    {
        [Key]
        public string StationKey { get; set; }
        public byte? WindEvent { get; set; }
        public DateTime? GustTime { get; set; }
        public double? GustSpeed { get; set; }
    }
}
