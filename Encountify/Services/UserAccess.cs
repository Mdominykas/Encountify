using Encountify.Models;
using Encountify.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly:Dependency(typeof(UserAccess))]
namespace Encountify.Services
{
    public class UserAccess : IUser
    {

        public Task<bool> AddAsync(User user)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.UserDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);

            List<User> oldElements = GetAllAsync().Result.ToList();

            foreach (User oldElement in oldElements)
            {
                if (String.Equals(user.Username, user.Username))
                    return Task.FromResult(false);
            }

            int result;
            try
            {
                result = db.Insert(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return Task.FromResult(false);
            }
            return Task.FromResult(result == 1);
        }

        public Task<bool> UpdateAsync(User user)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.UserDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);

            return Task.FromResult(db.Update(user) == 1);
        }

        public Task<bool> DeleteAsync(int id)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.UserDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);

            return Task.FromResult(db.Delete(GetAsync(id)) == 1);
        }

        public Task<int> DeleteAllAsync()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.UserDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);

            return Task.FromResult(db.DeleteAll<User>());
        }

        public Task<User> GetAsync(int id)
        {
            var list = GetAllAsync().Result;
            return Task.FromResult(list.FirstOrDefault(s => s.Id == id));
        }

        public Task<IEnumerable<User>> GetAllAsync(bool forceRefresh = false)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                DatabaseAccessConstants.UserDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);
            //haven't tested it, however we may not need to create new SQLiteConnection in every request

            db.CreateTable<User>();

            var table = db.Table<User>();

            List<User> answer = new List<User>();
            foreach (User s in table)
            {
                answer.Add(s);
            }

            return Task.FromResult(answer as IEnumerable<User>);
        }


    }
}
