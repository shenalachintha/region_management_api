using myApi.Models.Domain;

namespace myApi.DTO
{
    public class WalksDto
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public double LenthlnKm { set; get; }
        public string? WalkImageUrl { set; get; }
        public required RegionDto Region { get; set; }
        public required DifficultyDto Difficulty { get; set; }


    }
}
