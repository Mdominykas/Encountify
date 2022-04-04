using Encountify.Models;
using Encountify.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AssignedAchievmentAccess))]
namespace Encountify.Services
{
    public class AssignedAchievmentAccess : IAssignedAchievmentAccess
    {
        public async Task<bool> AddAsync(AssignedAchievment assignedAchievment)
        {
            const string url = "https://encountify.azurewebsites.net/API/AssignedAchievments";

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("userid", assignedAchievment.UserId.ToString());
            queryString.Add("achievmentid", assignedAchievment.AchievmentId.ToString());

            // this might be a bad solution, I don't know
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

        public async Task<int> DeleteAllAsync()
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = await client.DeleteAsync("https://encountify.azurewebsites.net/API/AssignedAchievments");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<User> obj = JsonConvert.DeserializeObject<IEnumerable<User>>(result);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string url = "https://encountify.azurewebsites.net/API/AssignedAchievments" + id.ToString();
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

        public async Task<IEnumerable<AssignedAchievment>> GetAllAsync(bool forceRefresh = false)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);
            HttpResponseMessage response = null;
            try
            {
                // here application freezes when entering scoreboard tab
                response = await client.GetAsync("https://encountify.azurewebsites.net/API/AssignedAchievments").ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            if ((response != null) && (response.IsSuccessStatusCode))
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<AssignedAchievment> obj = JsonConvert.DeserializeObject<IEnumerable<AssignedAchievment>>(result);
                return obj;
            }
            else
            {
                return null;
            }
        }

        public async Task<AssignedAchievment> GetAsync(int id)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = client.GetAsync("https://encountify.azurewebsites.net/API/AssignedAchievments/Id/" + id.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<AssignedAchievment> obj = JsonConvert.DeserializeObject<IEnumerable<AssignedAchievment>>(result);
                Console.WriteLine("Success");
                return obj.FirstOrDefault();
            }
            else
            {
                Console.WriteLine("Fail");
                return null;
            }
        }

        public Task<bool> UpdateAsync(AssignedAchievment assignedAchievment)
        {
            throw new NotImplementedException();
        }
    }
}
