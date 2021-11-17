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

[assembly:Dependency(typeof(UserAccess))]
namespace Encountify.Services
{
    public class UserAccess : IUser
    {
        public async Task<bool> AddAsync(User user)
        {
            const string url = "https://encountify.azurewebsites.net/API/Users";

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("username", user.Username);
            queryString.Add("password", user.Password);
            queryString.Add("email", user.Email);

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

        public Task<bool> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string url = "https://encountify.azurewebsites.net/API/Users" + id.ToString();
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

            var response = await client.DeleteAsync("https://encountify.azurewebsites.net/API/Users");

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

        public async Task<User> GetAsync(int id)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = await client.GetAsync("https://encountify.azurewebsites.net/API/Users/Id/" + id.ToString());

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<User> obj = JsonConvert.DeserializeObject<IEnumerable<User>>(result);
                Console.WriteLine("Success");
                return obj.FirstOrDefault();
            }
            else
            {
                Console.WriteLine("Fail");
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync(bool forceRefresh = true)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(1);
            HttpResponseMessage response = null;
            try
            {
                // here application freezes when entering scoreboard tab
                response = client.GetAsync("https://encountify.azurewebsites.net/API/Users").Result;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            if ((response != null) && (response.IsSuccessStatusCode))
            {
                var result = response.Content.ReadAsStringAsync().Result;
                IEnumerable<User> obj = JsonConvert.DeserializeObject<IEnumerable<User>>(result);
                return obj;
            }
            else
            {
                return null;
            }
        }
    }
}
