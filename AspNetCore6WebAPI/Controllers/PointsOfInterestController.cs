using System;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{pointofinterestid}",Name = "GetPointOfInterest")]
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

		[HttpPost]
		public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId,PointOfInterestForCreationDto pointOfInterest)
		{
			var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
			if(city == null)
			{
				return NotFound();
			}

			var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(city => city.PointsOfInterest).Max(p => p.Id);
			var finalPointOfInterest = new PointOfInterestDto()
			{
				Id = ++maxPointOfInterestId,
				Name = pointOfInterest.Name,
				Description = pointOfInterest.Description
			};

			city.PointsOfInterest.Add(finalPointOfInterest);

			return CreatedAtRoute("GetPointOfInterest",
				new
				{
					cityId = cityId,
					pointOfInterestId = finalPointOfInterest.Id
				},
				finalPointOfInterest);
		}

		[HttpPut("{pointofinterestid}")]

		public ActionResult UpdatePointOfInterest (int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
		{
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

			var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

			pointOfInterestFromStore.Name = pointOfInterest.Name;
			pointOfInterestFromStore.Description = pointOfInterest.Description;

			return NoContent();

        }

    }
}

