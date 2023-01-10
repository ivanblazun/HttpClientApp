using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebHttpClient.Data;

namespace WebHttpClient.UserSecurity
{
    public class UserSecurityAuth
    {
        public static bool Login (string username, string password) 
        {
            using (AppDbContext appDbContext = new AppDbContext())
            {

             return appDbContext.Users.Any(
                 u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) 
                 && u.Password == password);
            }


        }

    }
}