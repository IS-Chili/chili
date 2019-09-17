namespace Usa.chili.Dto
{
    public class StationMapDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsActive { get; set; }
    }
}