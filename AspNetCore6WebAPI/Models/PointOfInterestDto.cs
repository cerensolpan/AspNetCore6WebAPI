using System;
namespace AspNetCore6WebAPI.Controllers
{
	public class PointOfInterestDto
	{
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}

