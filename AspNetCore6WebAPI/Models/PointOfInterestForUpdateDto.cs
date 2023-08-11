using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore6WebAPI.Controllers
{
	public class PointOfInterestForUpdateDto
	{
        [Required(ErrorMessage = "You should provide a name value")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}

