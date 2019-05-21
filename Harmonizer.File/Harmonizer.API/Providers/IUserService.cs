using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.API.Providers
{
   public interface IUserService
    {
        User_ForTest Validate(string email, string password);
        List<User_ForTest> GetUserList();
        User_ForTest GetUserById(int id);
        List<User_ForTest> SearchByName(string name);
    }
}
