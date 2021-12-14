using Encountify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Encountify.Services
{
    public class ScoreboardCreation
    {
        public ScoreboardEntry this[int i] => CreateScoreboard().Result.ToArray()[i];
        public async Task<List<ScoreboardEntry>> CreateScoreboard(bool reversed = true)
        {
            IUserAccess userData = DependencyService.Get<IUserAccess>();    //new DatabaseAccess<User>();
            List<User> users = (List<User>) userData.GetAllAsync().Result;
            IVisitedLocationAccess visitedLocationsData = DependencyService.Get<IVisitedLocationAccess>();
            //DatabaseAccess<VisitedLocations> visitedLocationsData = new DatabaseAccess<VisitedLocations>();
            List<VisitedLocations> visitedLocations = (List<VisitedLocations>)visitedLocationsData.GetAllAsync().Result;

            var query =
            users.GroupJoin(visitedLocations,
                user => user.Id,
                loc => loc.UserId,
                (user, locations) =>
                    new
                    {
                        Users = user.Username,
                        UserId = user.Id,
                        Locations = locations.Select(loc => loc.LocationId),
                        Points = locations.Select(loc => loc.Points)
                    });
            List<ScoreboardEntry> results = new List<ScoreboardEntry>();

            foreach (var group in query)
            {
                results.Add(new ScoreboardEntry()
                {
                    Name = group.Users,
                    Score = group.Points.Aggregate(0, (agg, next) => agg + next),
                    UserId = group.UserId
                });
            }

            if (reversed)
            {
                results.SortDescendingOrder();
            }
            else
            {
                results.Sort();
            }

            return results;
        }
    }
}
