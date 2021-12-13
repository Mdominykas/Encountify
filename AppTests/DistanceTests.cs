using Encountify.Models;
using Encountify.Services;
using NUnit.Framework;
using Xunit;
using Locations = Xamarin.Essentials.Location;

namespace AppTests
{
    public class DistanceTests
    {
        [Fact]
        //Since distance is turned off, it should be a match to Could not get distance
        public async void ValidateYardsCounting()
        {
            Location location = new Location { Name = "Testing", Latitude = 54.69295, Longitude = 25.35268, Category = (int)Category.Park };

            var distance = await DistanceCounter.DistanceInYardsAsync(new Locations(location.Latitude, location.Longitude));

            Assert.AreEqual(distance, "Could not get distance");
        }

        [Fact]
        //Same as in yards, meters should return a same answer
        public async void ValidateMetersCounting()
        {
            Location location = new Location { Name = "Testing", Latitude = 54.69295, Longitude = 25.35268, Category = (int)Category.Park };
            
            var distance = await DistanceCounter.DistanceInMetersAsync(new Locations(location.Latitude, location.Longitude));

            Assert.AreEqual(distance, "Could not get distance");
        }

        [Fact]
        //Asserting that values are correctly parsed
        public void ValidateFormatting()
        {
            //Meters test
            var distance = "25,69 m";

            var _distance = DistanceCounter.ConvertedToMetersDistance(distance);

            Assert.AreEqual(_distance, 25,69);

            //Kilometers test
            var distance2 = "25,69 km";

            var _distance2 = DistanceCounter.ConvertedToMetersDistance(distance2);

            Assert.AreEqual(_distance2, 25690);

            //Yards test
            var distance3 = "25,69 yd";

            var _distance3 = DistanceCounter.ConvertedToMetersDistance(distance3);

            Assert.AreEqual(_distance3, 23,48);

            //Miles test
            var distance4 = "25,69 mi";

            var _distance4 = DistanceCounter.ConvertedToMetersDistance(distance4);

            Assert.AreEqual(_distance4, 41335,21);
        }

    }
}
