using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Providers
{
    public class UserService_ForTest:IUserService
    {
        private List<User_ForTest> userList = new List<User_ForTest>();
        public UserService_ForTest()
        {
            for (var i = 1; i <= 10; i++)
            {
                userList.Add(new User_ForTest
                {
                    Id = i,
                    Name = $"User {i}",
                    Password = $"pass{i}",
                    Email = $"user{i}@dummy.com",
                    Roles = new string[] { i % 2 == 0 ? "Admin" : "User" }
                });
            }
        }

        public User_ForTest Validate(string email, string password)
            => userList.FirstOrDefault(x => x.Email == email && x.Password == password);

        public List<User_ForTest> GetUserList() => userList;

        public User_ForTest GetUserById(int id)
            => userList.FirstOrDefault(x => x.Id == id);

        public List<User_ForTest> SearchByName(string name)
            => userList.Where(x => x.Name.Contains(name)).ToList();
    }
}