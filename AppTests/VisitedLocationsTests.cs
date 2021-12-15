using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encountify.Services;
using NUnit.Framework;
using Xunit;

namespace AppTests
{
    public class VisitedLocationsTests
    {
        private readonly VisitedLocationAccess visitedLocations;

        public VisitedLocationsTests()
        {
            visitedLocations = new VisitedLocationAccess();
        }

        [Fact]
        public async void ValidateGetAll()
        {
            var locations = await visitedLocations.GetAllAsync();

            Assert.NotNull(locations);
        }
    }
}
