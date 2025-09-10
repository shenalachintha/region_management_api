﻿using System.ComponentModel.DataAnnotations;

namespace myApi.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be maximum 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name has maximum 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { set; get; }
    }
}
