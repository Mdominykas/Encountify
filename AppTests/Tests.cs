//using NUnit.Framework;
//using Xamarin.UITest;
//using Xamarin.UITest.Queries;
using Encountify.Services;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Encountify.Models;
using System.Collections.Generic;
using NUnit.Framework;

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
            User user = new User { Username = "Tester", Password = "Testing123/", Email = "testing@gmail.com", IsAdmin = false };

            bool validate = await _userAccess.AddAsync(user);

            Assert.IsTrue(validate);
        }

        [Fact]
        //Should return false, cannot remove user that is not in database
        public async void ValidateDelete()
        {
            User user = new User { Username = "Tester", Password = "Testing123/", Email = "testing@gmail.com", IsAdmin = false };

            bool validate= await _userAccess.DeleteAsync(user.Id);

            Assert.IsFalse(validate);
        }

        [Fact]
        //Should get null, because there is no such user in database of ours
        public async void ValidateGet()
        {
            User user = new User { Username = "Tester", Password = "Testing123/", Email = "testing@gmail.com", IsAdmin = false };

            User validate = await _userAccess.GetAsync(user.Id);
            Assert.AreNotEqual(user, validate);
        }

       
    }
}
