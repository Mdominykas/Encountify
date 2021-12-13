using Encountify.Services;
using Xunit;
using Encountify.Models;
using NUnit.Framework;
using System.Linq;

namespace AppTests
{
    public class LocationTests
    {
        private readonly LocationAccess locationAccess;

        public LocationTests()
        {
            locationAccess = new LocationAccess();
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
            var locations = await locationAccess.GetAllAsync();

            Assert.IsNotNull(locations);
        }

        [Fact]
        //Should not be null
        public async void ValidateGet()
        {
            var locations = await locationAccess.GetAllAsync();

            var location = locations.First();

            var test = await locationAccess.GetAsync(location.Id);

            Assert.IsNotNull(test);
        }

        [Fact]
        //Should be true
        public async void ValidateDelete()
        {
            Location location = new Location {Name = "Testing", Latitude = 54.69295, Longitude = 25.35268, Category = (int)Category.Park };

            bool validate = await locationAccess.DeleteAsync(location.Id);

            Assert.IsTrue(validate);
        }
    }
}
