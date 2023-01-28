using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;
using WebHttpClient.Models;
using System.Text;
using System.Security;
using System.Security.Principal;
using System.Threading;


namespace WebHttpClient.UserSecurity
{
    public class TokemManager
    {
        //private static string Secret = Guid.NewGuid().ToString();

        private static string Secret = "123h21bcde012t45abjde1623fg9bcde";

        // JWT V1
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



        // JWT V2 Create Token 
        public static readonly byte[] _signInKey = Encoding.UTF8.GetBytes("123h21bcde012t45abjde1623fg9bcde");
        public static string CreateJWT(User user ) 
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));

            claims.Add(new Claim(ClaimTypes.Role, user.UserStatus.ToString()));

            var id = new ClaimsIdentity(claims);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateToken(new SecurityTokenDescriptor()
            {   

                // Need to change after testing
                Expires=DateTime.UtcNow.AddDays(60),

                Subject = id,

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_signInKey),SecurityAlgorithms.HmacSha256),


            }) ;

            return handler.WriteToken(token);
        }

        // JWT V2 Validate Token 
        public static IPrincipal ValidateJWT(string token)
        {
           
            var handler = new JwtSecurityTokenHandler();

            handler.ValidateToken(token, new TokenValidationParameters()
            {

                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },

                ValidateAudience = false,

                ValidateIssuer = false,

                IssuerSigningKey=new SymmetricSecurityKey(_signInKey),

                ValidateIssuerSigningKey=true,

                ValidateLifetime=true

            }, out var securityToken); ;

            var jwt = securityToken as JwtSecurityToken;

            var id = new ClaimsIdentity(jwt.Claims, "jwt", "name", "role");

            return new ClaimsPrincipal(id);
        }

        public static void AuthenticateUser() 
        {
            try
            {
                var token = HttpContext.Current.Request.Headers.Get("Authorization");

                var principal = ValidateJWT(token);

                HttpContext.Current.User = principal;

                Thread.CurrentPrincipal = principal;

            }
            catch { }

        }
    }
}