using System;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore6WebAPI.Controllers
{
	[Route("api/cities/{cityId}/pointsofinterest")]
	[ApiController]
	public class PointsOfInterestController : ControllerBase
	{
		private readonly ILogger<PointsOfInterestController> _logger;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet()]
		public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
		{
			try
			{
				// throw new Exception("Exception sample.");
                var city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }
                return Ok(city.PointsOfInterest);

            }
			catch(Exception ex)
			{
				_logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.",ex);
				return StatusCode(500, "A problem happened while handling your request.");
			}
			
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

        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
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

			var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
			{
				Name = pointOfInterestFromStore.Name,
				Description = pointOfInterestFromStore.Description
			};

			patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (!TryValidateModel(pointOfInterestToPatch))
			{
				return BadRequest(ModelState);
			}

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

		[HttpDelete("{pointofinterestid}")]
		public ActionResult DeletePointOfInteres(int cityId, int pointOfInterestId)
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

			city.PointsOfInterest.Remove(pointOfInterestFromStore);
			return NoContent();
        }
    }
}

