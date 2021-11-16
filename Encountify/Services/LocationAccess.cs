using Encountify.Models;
using Encountify.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationAccess))]

namespace Encountify.Services
{
    public class LocationAccess : ILocation
    {
        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = await client.GetAsync("https://encountify.azurewebsites.net/API/Locations");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<Location> obj = JsonConvert.DeserializeObject<IEnumerable<Location>>(result);
                return obj;
            }
            else
            {
                return null;
            }
        }

        public async Task<Location> GetAsync(int id)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = await client.GetAsync("https://encountify.azurewebsites.net/API/Locations/Id/" + id.ToString());

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<Location> obj = JsonConvert.DeserializeObject<IEnumerable<Location>>(result);
                return obj.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> AddAsync(Location location)
        {
            const string url = "https://encountify.azurewebsites.net/API/Locations";

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);


            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (!String.IsNullOrEmpty(location.Name))
                queryString.Add("name", location.Name);
            if(!String.IsNullOrEmpty(location.Description))
                queryString.Add("description", location.Description);
            queryString.Add("longitude", location.Longitude.ToString());
            queryString.Add("latitude", location.Latitude.ToString());
            queryString.Add("category", location.Category.ToString());

            var values = new Dictionary<string, string>
            {
            };

            var content = new FormUrlEncodedContent(values);

            string goal = url + "?" + queryString.ToString();
            var response = await client.PostAsync(goal, content);
            if (response.IsSuccessStatusCode)
            {
                return true;// it may succedd but do not delete user
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string url = "https://encountify.azurewebsites.net/API/Locations/" + id.ToString();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);


            var response = await client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<int> DeleteAllAsync()
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = await client.DeleteAsync("https://encountify.azurewebsites.net/API/Locations");

            if (response.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public Task<bool> UpdateAsync(Location user)
        {
            throw new NotImplementedException();
        }
    }

    public interface ILocation
    {
        Task<bool> AddAsync(Location user);
        Task<bool> UpdateAsync(Location user);
        Task<bool> DeleteAsync(int id);
        Task<int> DeleteAllAsync();
        Task<Location> GetAsync(int id);
        Task<IEnumerable<Location>> GetAllAsync();
    }

}
