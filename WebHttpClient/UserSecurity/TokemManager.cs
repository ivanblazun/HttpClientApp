using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WebHttpClient.UserSecurity
{
    public class TokemManager
    {
        //private static string Secret = Guid.NewGuid().ToString();

        private static string Secret = "123h21bcde012t45abjde1623fg9bcde";

        public static string GenerateToken(string userName)
        {   
            //key get from secret key for JWT
 

            byte[] key = Convert.FromBase64String(Secret);


            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            // Decripting class 
            SecurityTokenDescriptor decriptor = new SecurityTokenDescriptor
            {
            
                Subject=new System.Security.Claims.ClaimsIdentity(claims:new[] { new Claim( type:ClaimTypes.Name,value:userName) }),

                Expires=DateTime.UtcNow.AddDays(20),

                SigningCredentials=new SigningCredentials(securityKey,
                algorithm:SecurityAlgorithms.HmacSha256Signature)
            };

            // Handler that create JWt from hashed secret key
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            // Actual token created by handler
            JwtSecurityToken token = handler.CreateJwtSecurityToken(decriptor);

            // Returned token
            return handler.WriteToken(token);
        }
    }
}