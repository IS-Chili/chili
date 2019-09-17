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
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Elevation { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
