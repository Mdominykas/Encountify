using Encountify.Models;
using Encountify.Services;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AchievmentAccess))]
namespace Encountify.Services
{
    class AchievmentAccess : IAchievmentAccess
    {
        public async Task<bool> AddAsync(Achievment achievment)
        {
            const string url = "https://encountify.azurewebsites.net/API/Achievments";

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("name", achievment.Name);
            queryString.Add("description", achievment.Description);
            queryString.Add("category", achievment.Category.ToString());

            // this might be a bad solution, I don't know
            var values = new Dictionary<string, string>
            {
            };

            var content = new FormUrlEncodedContent(values);

            string goal = url + "?" + queryString.ToString();
            var response = await client.PostAsync(goal, content);
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

            var response = await client.DeleteAsync("https://encountify.azurewebsites.net/API/Achievments");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<Achievment> obj = JsonConvert.DeserializeObject<IEnumerable<Achievment>>(result);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string url = "https://encountify.azurewebsites.net/API/Achievments" + id.ToString();
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

        public async Task<IEnumerable<Achievment>> GetAllAsync(bool forceRefresh = false)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);
            HttpResponseMessage response = null;
            try
            {
                // here application freezes when entering scoreboard tab
                response = await client.GetAsync("https://encountify.azurewebsites.net/API/Achievments").ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            if ((response != null) && (response.IsSuccessStatusCode))
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<Achievment> obj = JsonConvert.DeserializeObject<IEnumerable<Achievment>>(result);
                return obj;
            }
            else
            {
                return null;
            }
        }

        public async Task<Achievment> GetAsync(int id)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = client.GetAsync("https://encountify.azurewebsites.net/API/Achievments/Id/" + id.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<Achievment> obj = JsonConvert.DeserializeObject<IEnumerable<Achievment>>(result);
                Console.WriteLine("Success");
                return obj.FirstOrDefault();
            }
            else
            {
                Console.WriteLine("Fail");
                return null;
            }
        }

        public Task<bool> UpdateAsync(Achievment achievment)
        {
            throw new NotImplementedException();
        }
    }
}
