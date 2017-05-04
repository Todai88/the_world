using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        private IWorldRepository _repository;
        private ILogger _logger;
        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            { 
                var results = _repository.GetTripsByUsername(this.User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get All Trips: {ex}");
                return BadRequest($"Something bad happened.");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] TripViewModel trip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(trip);
                newTrip.UserName = User.Identity.Name;

                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }
                else
                {
                    return BadRequest("Failed to save changes to the database.");
                }
            }
            return BadRequest("Failed to save the trip...");
        }
    }
}
