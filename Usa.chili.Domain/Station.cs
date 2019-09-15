using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class Station
    {
        [Key]
        public int Id { get; set; }
        public string StationKey { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
    }
}
