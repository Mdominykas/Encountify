using Encountify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encountify.Services
{
    public class ScoreboardCreation
    {
        public ScoreboardEntry this[int i] => CreateScoreboard().ToArray()[i];
        public List<ScoreboardEntry> CreateScoreboard(bool reversed = false)
        {
            DatabaseAccess<User> userData = new DatabaseAccess<User>();
            List<User> users = (List<User>) userData.GetAllAsync().Result;
            DatabaseAccess<VisitedLocations> visitedLocationsData = new DatabaseAccess<VisitedLocations>();
            List<VisitedLocations> visitedLocations = (List<VisitedLocations>) visitedLocationsData.GetAllAsync().Result;

            var query =
            users.GroupJoin(visitedLocations,
                user => user.Id,
                loc => loc.UserId,
                (user, locations) =>
                    new
                    {
                        Users = user.Username,
                        Locations = locations.Select(loc => loc.LocationId)
                    });
            List<ScoreboardEntry> results = new List<ScoreboardEntry>();

            foreach(var group in query)
            {
                results.Add(new ScoreboardEntry()
                {
                    Name = group.Users,
                    Score = group.Locations.Count()
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
