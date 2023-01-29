using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace WebHttpClient.UserSecurity
{
    public class CuuUser 
    {
        public static void GetCurrUser()
        {
            try
            {
                var token = HttpContext.Current.Request.Headers.Get("Authorization");



                var cU = HttpContext.Current.User;

 

            }
            catch { }

        }
    }
}