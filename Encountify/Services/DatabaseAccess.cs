using Encountify.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Encountify.Services
{
    /// <summary>
    /// generic class to manage operations with database
    /// </summary>
    /// <typeparam name="T">class in the database</typeparam>
    public class DatabaseAccess<T> : IDataStore<T>
        where T : new()
    {
        public Task<bool> AddAsync(T element)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName());
            SQLiteConnection db = new SQLiteConnection(dbPath);

            List<T> oldElements = GetAllAsync().Result.ToList();

            foreach (T oldElement in oldElements)
            {
                if (String.Equals(GetName(element), GetName(oldElement)))
                    return Task.FromResult(false);
            }

            int result;
            try
            {
                result = db.Insert(element);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return Task.FromResult(false);
            }
            return Task.FromResult(result == 1);

        }

        public Task<bool> UpdateAsync(T element)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName());
            SQLiteConnection db = new SQLiteConnection(dbPath);

            return Task.FromResult(db.Update(element) == 1);
        }

        public Task<bool> DeleteAsync(int id)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName());
            SQLiteConnection db = new SQLiteConnection(dbPath);

            return Task.FromResult(db.Delete(GetAsync(id)) == 1);
        }

        public Task<int> DeleteAllAsync()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName());
            SQLiteConnection db = new SQLiteConnection(dbPath);

            return Task.FromResult(db.DeleteAll<T>());
        }

        public Task<T> GetAsync(int id)
        {
            var list = GetAllAsync().Result;
            return Task.FromResult(list.FirstOrDefault(s => GetId(s) == id));
        }

        public Task<IEnumerable<T>> GetAllAsync(bool forceRefresh = false)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                DatabaseName());
            SQLiteConnection db = new SQLiteConnection(dbPath);
            //haven't tested it, however we may not need to create new SQLiteConnection in every request

            db.CreateTable<T>();

            var table = db.Table<T>();

            List<T> answer = new List<T>();
            foreach (T s in table)
            {
                answer.Add(s);
            }

            return Task.FromResult(answer as IEnumerable<T>);
        }

        public string DatabaseName()
        {
            if (typeof(T) == typeof(Location))
                return DatabaseAccessConstants.LocationDatabaseName;
            else if (typeof(T) == typeof(User))
                return DatabaseAccessConstants.UserDatabaseName;
            else
                throw new Exception("Database of given type does not exist");
        }

        private int GetId(T element)
        {
            if (typeof(T) == typeof(Location))
                return (element as Location).Id;
            else if (typeof(T) == typeof(User))
                return (element as User).Id;
            else
                throw new Exception("Database of given type does not exist");
        }

        private string GetName(T element)
        {
            if (typeof(T) == typeof(Location))
                return (element as Location).Name;
            else if (typeof(T) == typeof(User))
                return (element as User).Username;
            else
                throw new Exception("Database of given type is not supported");
        }

    }
}
