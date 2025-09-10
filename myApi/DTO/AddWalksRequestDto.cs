using myApi.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace myApi.DTO
{
    public class AddWalksRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { set; get; }
        [Required]
        [MaxLength(1000)]
        public string Description { set; get; }

        [Required]
        [Range(0,50)]
        public double LenthlnKm { set; get; }
        public string? WalkImageUrl { set; get; }
        [Required]
        public Guid DifficultyId { set; get; }
        [Required]
        public Guid RegionId { set; get; }
        // Navigation properties
        public Difficulty Difficulty { set; get; }
        public Region Region { set; get; }
    }
}
