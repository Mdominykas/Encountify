using Encountify.Services;
using Xunit;
using Encountify.Models;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace AppTests
{
    public class UserTesting
    {
        private readonly UserAccess userAccess;
        private List<User> Users;

        public UserTesting()
        {
            userAccess = new UserAccess();
            Users =  (List<User>)userAccess.GetAllAsync().Result;
        }

        [Fact]
        //Should be true
        public async void ValidateAdding()
        {
            User user = new User {Id = 100, Username = "Tester", Password = "Testing123/", Email = "testing@gmail.com", IsAdmin = false };

            bool validate = await userAccess.AddAsync(user);

            Assert.IsTrue(validate);
        }

        [Fact]
        //Should be not null
        public async void ValidateGet()
        {
            var user = Users.First();

            var test = await userAccess.GetAsync(user.Id);
            
            Assert.NotNull(test);
        }

        [Fact]
        //Should be false, cause there is no such user 
        public async void ValidateDelete()
        {
            User user = new User { Username = "AppTest", Password = "Test", Email = "test@gmail.com", IsAdmin = false };

            bool validate = await userAccess.DeleteAsync(user.Id);

            Assert.IsFalse(validate);
        }


        [Fact]
        //Should be not null
        public async void ValidateGetAll()
        {
            Assert.IsNotNull(Users);
        }
    }
}
