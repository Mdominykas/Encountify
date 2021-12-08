using Encountify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Encountify.Services
{
    public class ScoreboardCreation
    {
        public ScoreboardEntry this[int i] => CreateScoreboard().ToArray()[i];
        public List<ScoreboardEntry> CreateScoreboard(bool reversed = true)
        {
            IUser userData = DependencyService.Get<IUser>();    //new DatabaseAccess<User>();
            List<User> users = (List<User>)userData.GetAllAsync().Result;
            DatabaseAccess<VisitedLocations> visitedLocationsData = new DatabaseAccess<VisitedLocations>();
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

            var results = query.Aggregate(new List<ScoreboardEntry>(),
                (list, group) =>
                {
                    list.Add(new ScoreboardEntry()
                    {
                        Name = group.Users,
                        Score = group.Points.Sum(),
                        UserId = group.UserId
                    });
                    return list;
                });

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
