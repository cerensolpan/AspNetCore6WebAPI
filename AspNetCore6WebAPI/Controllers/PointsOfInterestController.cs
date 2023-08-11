using System;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore6WebAPI.Controllers
{
	[Route("api/cities/{cityId}/pointsofinterest")]
	[ApiController]
	public class PointsOfInterestController : ControllerBase
	{
		[HttpGet()]
		public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
		{
			var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

			if(city == null)
			{
				return NotFound();
			}
			return Ok(city.PointsOfInterest);
		}
        [HttpGet("{pointofinterestid}")]
		public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
		{
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
			var pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
			return Ok(pointOfInterest);
        }

    }
}

