using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Service
{
    public class LoginService
    {
        UserData _userData = new UserData();
        public User GetUser (string UserId,string Password)
        {
            User user = new User();
            user = _userData.GetUserDetailByUserIDAndPassword(UserId, Password);
            return user;
        }
        public CommanUserData GetCommanData(string UserId)
        {
            CommanUserData commanUserData = new CommanUserData();
            commanUserData= _userData.GetCommanData(UserId);
            return commanUserData;
        }

    }
}