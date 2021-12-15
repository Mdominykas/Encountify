using Encountify.Services;
using Xunit;
using Encountify.Models;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace AppTests
{
    public class LocationTests
    {
        private readonly LocationAccess locationAccess;
        private List<Location> Locations;

        public LocationTests()
        {
            locationAccess = new LocationAccess();
            Locations = (List<Location>)locationAccess.GetAllAsync().Result;
        }

        [Fact]
        //Should return true
        public async void ValidateAdd()
        {
            Location location = new Location { Id = 10, Name = "Test", Latitude = 54.69295, Longitude = 25.35268, Category = (int)Category.Park };

            bool validate = await locationAccess.AddAsync(location);

            Assert.IsTrue(validate);
        }

        [Fact]
        //Should be not null
        public async void ValidateGetAll()
        {
            Assert.IsNotNull(Locations);
        }

        [Fact]
        //Should not be null
        public async void ValidateGet()
        {
            var location = Locations.First();

            var test = await locationAccess.GetAsync(location.Id);

            Assert.IsNotNull(test);
        }

        [Fact]
        //Should be true
        public async void ValidateDelete()
        {
            var location = Locations.Last();

            bool validate = await locationAccess.DeleteAsync(location.Id);

            Assert.IsTrue(validate);
        }
    }
}
