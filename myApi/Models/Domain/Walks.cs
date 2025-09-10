namespace myApi.Models.Domain
{
    public class Walks
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public double LenthlnKm { set; get; }
        public string? WalkImageUrl { set; get; }
        public Guid DifficultyId { set; get; }
        public Guid RegionId { set; get; }
        // Navigation properties
        public Difficulty Difficulty { set; get; }
        public Region Region { set; get; }

    }
}
