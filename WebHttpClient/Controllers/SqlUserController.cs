using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using WebHttpClient.Data;
using WebHttpClient.Models;
using WebHttpClient.UserSecurity;

namespace WebHttpClient.Controllers
{
    public class SqlUserController : ApiController
    {
        HttpClient client = new HttpClient();

        HttpRequestMessage message = new HttpRequestMessage();

        public AppDbContext appDbContext = new AppDbContext();


        // POSt Create and register new user
        [HttpPost]
        [Route ("api/sqluser/registeruser")]
        public HttpResponseMessage RegisterUser([FromBody] User newUser)
        {

            bool doesUserExist = appDbContext.Users.Where(u => u.UserName == newUser.UserName || u.Email==newUser.Email).Any();            
            
            HttpRequestMessage httpRequest = new HttpRequestMessage();

            string userName = newUser.UserName.ToString();

            string password = newUser.Password.ToString();

            string email = newUser.Email.ToString();

            List<string> resp = new List<string> { userName, password, email };

            if (!doesUserExist)
            {
                User registerUser = newUser;

                appDbContext.Users.Add(registerUser);

                appDbContext.SaveChanges();

                var response = httpRequest.CreateResponse(HttpStatusCode.OK);

                response.Content = new StringContent(JsonConvert.SerializeObject(resp), System.Text.Encoding.UTF8, "application/json");

                return response;
            }
            else 
            {
                var response = httpRequest.CreateResponse(HttpStatusCode.NotFound);

                response.Content = new StringContent("User alredy exist", System.Text.Encoding.UTF8, "application/json");

                return response;
            }           
        }

        // POST Login user 
        [HttpPost]
        [Route("api/sqluser/loginuser")]
        public HttpResponseMessage LoginUser(HttpRequestMessage httpRequest) 
        {
            var response = httpRequest;

           if( httpRequest.Headers.Authorization == null)
            {
              return  response.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string authToken = httpRequest.Headers.Authorization.Parameter;

                string decodedauthtoken= Encoding.UTF8.GetString( Convert.FromBase64String(authToken));

                string[] usernamePasswordArray=  decodedauthtoken.Split(':');

                string username = usernamePasswordArray[0];
                
                string password = usernamePasswordArray[1];

                if(UserSecurityAuth.Login(username, password)) 
                {
                  string userAndPassword = username + password;

                  response.Content = new StringContent(JsonConvert.SerializeObject(userAndPassword), System.Text.Encoding.UTF8, "application/json");

                  return  response.CreateResponse(HttpStatusCode.Accepted);
                }
                else
                {
                   return response.CreateResponse(HttpStatusCode.Unauthorized);
                }
                
            }           
        }


        // POST Search for user profile 
        [HttpPost]
        [Route("api/sqluser/getuserfullprofile")]
        public HttpResponseMessage GetUserFullProfile(HttpRequestMessage httpRequest, [FromBody] UserProfile requestedProfile)
        {
            bool doesUserProfileExist = appDbContext.UserProfiles
                .Where(u => u.FirstName == requestedProfile.FirstName || u.LastName == requestedProfile.LastName).Any();

            var userProfile = appDbContext.UserProfiles
                .Where(u => u.FirstName == requestedProfile.FirstName || u.LastName == requestedProfile.LastName).FirstOrDefault();

            var response = new HttpResponseMessage();

            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered");
                              
                return response;
            }
            else
            {
                string authToken = httpRequest.Headers.Authorization.Parameter;

                string decodedauthtoken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                string[] usernamePasswordArray = decodedauthtoken.Split(':');

                string username = usernamePasswordArray[0];

                string password = usernamePasswordArray[1];

                if (UserSecurityAuth.Login(username, password))
                {
                    if (!doesUserProfileExist)
                    {
                        response =Request.CreateResponse(HttpStatusCode.NotFound,"Searched user does not have profile created yet");

                        return response;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, userProfile);

                        return response;
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered");

                    return response;
                }

            }
        }

        // POST Create user profile
        [HttpPost]
        [Route("api/sqluser/createuserprofile")]
        public HttpResponseMessage CreateUserProfile(HttpRequestMessage httpRequest,[FromBody] User user, [FromBody] UserProfile createdProfile)
        {
            bool doesUserProfileExist = appDbContext.UserProfiles
                .Where(u => u.Id==createdProfile.Id).Any();

            var userProfile = appDbContext.UserProfiles
                .Where(u => u.FirstName == createdProfile.FirstName || u.LastName == createdProfile.LastName).FirstOrDefault();

            bool isCurrentUser = appDbContext.Users
                .Where(u => u.Id == user.Id).Any();

            bool doesUserOwnProfile = appDbContext.UserProfiles.Where(uP => uP.UserId == user.Id).Any();

            var response = new HttpResponseMessage();

            UserProfile newProfile = new UserProfile();
            
            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered");

                return response;
            }
            else
            {
                string authToken = httpRequest.Headers.Authorization.Parameter;

                string decodedauthtoken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                string[] usernamePasswordArray = decodedauthtoken.Split(':');

                string username = usernamePasswordArray[0];

                string password = usernamePasswordArray[1];

                if (UserSecurityAuth.Login(username, password))
                {
                    if (!doesUserProfileExist && isCurrentUser)
                    {
                        newProfile = createdProfile;

                        appDbContext.UserProfiles.Add(newProfile);

                        appDbContext.SaveChanges();

                        response = Request.CreateResponse(HttpStatusCode.Accepted, newProfile);

                        return response;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Searched user does not have profile created yet");

                        return response;
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered");

                    return response;
                }

            }
        }

    }
}
