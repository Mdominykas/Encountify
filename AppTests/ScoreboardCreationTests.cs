using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Encountify.Services;
using Encountify.Models;

namespace AppTests
{
    public class ScoreboardCreationTests
    {
        [Fact]
        public async void CreateScoreboardTest()
        {

            Mock<IUserAccess> ussAcc = new Mock<IUserAccess>();
            Mock<IVisitedLocationAccess> visLoc = new Mock<IVisitedLocationAccess>();
            ScoreboardCreation sc = new ScoreboardCreation(ussAcc.Object, visLoc.Object);

            List<User> userList = new List<User> { new User { Id = 1, Username = "testName", Password = "password" } };
            List<VisitedLocations> visLoca = new List<VisitedLocations> { new VisitedLocations { Id = 1, UserId = 1, LocationId = 1, Points = 100} };

            ussAcc.Setup(p => p.GetAllAsync(false)).Returns(Task.FromResult(userList.AsEnumerable()));

            visLoc.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(visLoca.AsEnumerable()));

            await sc.CreateScoreboard();
        }
    }
}
