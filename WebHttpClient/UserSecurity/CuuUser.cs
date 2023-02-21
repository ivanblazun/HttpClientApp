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
        public static string GetCurrUser()
        {
            try
            {
                //var token = HttpContext.Current.Request.Headers.Get("Authorization");

                ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                                

                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();

                //Filter specific claim    
              var c= claims?.FirstOrDefault(x => x.Type.Equals("nameId", StringComparison.OrdinalIgnoreCase))?.Value;

                return c;
            }
            catch { }

            return null;
        }
    }
}