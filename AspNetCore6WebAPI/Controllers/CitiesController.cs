using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore6WebAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore6WebAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore;

        public CitiesController(CitiesDataStore citiesDataStore)
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(_citiesDataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity( int id)
        {
            var result = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}

