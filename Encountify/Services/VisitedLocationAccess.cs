using Encountify.Models;
using Encountify.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(VisitedLocationAccess))]
namespace Encountify.Services
{
    public class VisitedLocationAccess : IVisitedLocationAccess
    {
        public Task<bool> AddAsync(VisitedLocations visLoc)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VisitedLocations>> GetAllAsync()
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            HttpResponseMessage response = null;

            try
            {
                response = await client.GetAsync("https://encountify.azurewebsites.net/API/VisitedLocations").ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            if ((response != null) && response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<VisitedLocations> obj = JsonConvert.DeserializeObject<IEnumerable<VisitedLocations>>(result);
                return obj;
            }
            else
            {
                return null;
            }
        }

        public Task<VisitedLocations> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VisitedLocations>> GetLastsAsync(int id, int numberOfLocations = 1)
        {
            throw new NotImplementedException();
        }
    }
}
