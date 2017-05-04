using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;
        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting All Trips From The Database");
            return _context.Trips.ToList();
        }

        public IEnumerable<Trip> GetTripsByUsername(string name)
        {
            return _context
                   .Trips
                   .Include(t => t.Stops)
                   .Where(t => t.UserName == name)
                   .ToList();
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public void AddStop(string tripName, Stop newStop, string userName)
        {
            var trip = GetUserTripByName(tripName, userName);
            if (trip != null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                    .Include(t => t.Stops)
                    .Where(t => t.Name == tripName)
                    .FirstOrDefault();
        }

        public Trip GetUserTripByName(string tripName, string userName)
        {
            return _context.Trips
                   .Include(t => t.Stops)
                   .Where(t => t.Name == tripName && t.UserName == userName)
                   .FirstOrDefault();
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
