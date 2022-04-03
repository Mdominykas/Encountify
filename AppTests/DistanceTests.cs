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

        /*[Fact]
        //Asserting that values are correctly parsed
        public void ValidateFormatting()
        {
            //Meters test
            var testDistance = "25,69 m";

            var distance = DistanceCounter.ConvertedToMetersDistance(testDistance);

            //Assert.AreEqual(distance, 25.69);

            //Kilometers test
            var testDistance2 = "25,69 km";

            var distance2 = DistanceCounter.ConvertedToMetersDistance(testDistance2);

            //Assert.AreEqual(distance2, 25690);

            //Yards test
            var testDistance3 = "25,69 yd";

            var distance3 = DistanceCounter.ConvertedToMetersDistance(testDistance3);

            //Assert.AreEqual(distance3, 23.48);

            //Miles test
            var testDistance4 = "25,69 mi";

            var distance4 = DistanceCounter.ConvertedToMetersDistance(testDistance4);

            //Assert.AreEqual(distance4, 41335.21);
        }*/

    }
}
