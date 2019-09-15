using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class MadisStatus
    {
        [Key]
        public string StationKey { get; set; }
        public DateTime Ts { get; set; }
    }
}
