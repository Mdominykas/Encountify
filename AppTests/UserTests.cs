using Encountify.Services;
using Xunit;
using Encountify.Models;
using NUnit.Framework;
using System.Linq;

namespace AppTests
{
    public class UserTesting
    {
        private readonly UserAccess _userAccess;

        public UserTesting()
        {
            _userAccess = new UserAccess();
        }

        [Fact]
        //Should be true
        public async void ValidateAdding()
        {
            User user = new User {Id = 100, Username = "Tester", Password = "Testing123/", Email = "testing@gmail.com", IsAdmin = false };

            bool validate = await _userAccess.AddAsync(user);

            Assert.IsTrue(validate);
        }

        [Fact]
        //Should be false
        public async void ValidateDelete()
        {
            User user = new User {Username = "Tester", Password = "Testing123/", Email = "testing@gmail.com", IsAdmin = false };

            bool validate= await _userAccess.DeleteAsync(user.Id);

            Assert.IsFalse(validate);
        }

        [Fact]
        //Should be equal
        public async void ValidateGet()
        {
           var users = await _userAccess.GetAllAsync();

            var user = users.First();

            var test = await _userAccess.GetAsync(user.Id);
            
            Assert.NotNull(test);
        }

        [Fact]
        //Should be not null
        public async void ValidateGetAll()
        {
            var users = await _userAccess.GetAllAsync();

            Assert.IsNotNull(users);
        }
    }
}
