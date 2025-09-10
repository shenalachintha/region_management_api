using System.ComponentModel.DataAnnotations;

namespace myApi.DTO
{
    public class UpdateWalkRequestDto
    {

        [Required]
        [MaxLength(100)]
        public string Name { set; get; }
        [Required]
        [MaxLength(100)]
        public string Description { set; get; }
        [Required]
        [Range(0, 50)]
        public double LenthlnKm { set; get; }
        public string? WalkImageUrl { set; get; }
        [Required]
        public Guid DifficultyId { set; get; }
        [Required]
        public Guid RegionId { set; get; }
    }
}
