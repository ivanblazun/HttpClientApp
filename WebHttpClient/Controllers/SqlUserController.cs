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
        [Route("api/sqluser/registeruser")]
        public HttpResponseMessage RegisterUser([FromBody] User newUser)
        {

            bool doesUserExist = appDbContext.Users.Where(u => u.UserName == newUser.UserName || u.Email == newUser.Email).Any();

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

            if (httpRequest.Headers.Authorization == null)
            {
                return response.CreateResponse(HttpStatusCode.Unauthorized);
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
                    string userAndPassword = username + password;

                    response.Content = new StringContent(JsonConvert.SerializeObject(userAndPassword), System.Text.Encoding.UTF8, "application/json");

                    return response.CreateResponse(HttpStatusCode.Accepted);
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
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Searched user does not have profile created yet");

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
        public HttpResponseMessage CreateUserProfile(HttpRequestMessage httpRequest, [FromUri] int userId, [FromBody] UserProfile createdProfile)
        {
            // radi sto bi trebao ali netreba, neka ostane za nedaj bože
            bool doesUserProfileExist = appDbContext.UserProfiles
                .Where(u => u.Id == createdProfile.Id)
                .Any();

            var userProfile = appDbContext.UserProfiles
                .Where(u => u.FirstName == createdProfile.FirstName || u.LastName == createdProfile.LastName)
                .FirstOrDefault();

            bool doesUserAlredyHaveProfile = appDbContext.UserProfiles
                .Where(uP => uP.UserId == userId)
                .Any();


            var response = new HttpResponseMessage();

            UserProfile newProfile = new UserProfile();

            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered user to create your profile");

                return response;
            }
            else
            {
                // auth procedure
                string authToken = httpRequest.Headers.Authorization.Parameter;

                string decodedauthtoken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                string[] usernamePasswordArray = decodedauthtoken.Split(':');

                string username = usernamePasswordArray[0];

                string password = usernamePasswordArray[1];



                if (UserSecurityAuth.Login(username, password))
                {
                    if (!doesUserProfileExist && !doesUserAlredyHaveProfile)
                    {
                        newProfile = createdProfile;

                        appDbContext.UserProfiles.Add(newProfile);

                        appDbContext.SaveChanges();

                        response = Request.CreateResponse(HttpStatusCode.Accepted, newProfile);

                        return response;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "You alredy have profile created, maybe update?");

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

        // PUT Update user profile
        [HttpPut]
        [Route("api/sqluser/updateuserfullprofile")]
        public HttpResponseMessage UpdateUserProfile(HttpRequestMessage httpRequest, [FromUri] int userId, [FromBody] UserProfile updatedProfile)
        {
            // radi sto bi trebao ali netreba, neka ostane za nedaj bože
            bool doesUserProfileExist = appDbContext.UserProfiles
                .Where(u => u.Id == updatedProfile.Id)
                .Any();

            bool doesUserAlredyHaveProfile = appDbContext.UserProfiles
                .Where(uP => uP.UserId == userId)
                .Any();                   

            var response = new HttpResponseMessage();

            UserProfile newUpdatedProfile = new UserProfile();

            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered user to create your profile");

                return response;
            }
            else
            {
                // auth procedure
                string authToken = httpRequest.Headers.Authorization.Parameter;

                string decodedauthtoken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                string[] usernamePasswordArray = decodedauthtoken.Split(':');

                string username = usernamePasswordArray[0];

                string password = usernamePasswordArray[1];
                             

                if (UserSecurityAuth.Login(username, password))
                {
                    if (!doesUserProfileExist && doesUserAlredyHaveProfile)
                    {
                                                                                             

                        bool userProfile = appDbContext.UserProfiles
                             .Any(u => u.UserId==userId);

                        bool doesUser = appDbContext.Users
                            .All(u => u.UserName == username && u.Password == password && u.Id==userId);

                        if (userProfile && doesUser)
                        {
                            var searchedProfile = appDbContext.UserProfiles.Where(uP => uP.Id == updatedProfile.Id).First<UserProfile>();


                            searchedProfile.FirstName = updatedProfile.FirstName;
                            searchedProfile.LastName = updatedProfile.LastName;
                            searchedProfile.Avatar = updatedProfile.Avatar;
                            searchedProfile.AboutMyself = updatedProfile.AboutMyself;

                          
                            appDbContext.SaveChanges();

                            response = Request.CreateResponse(HttpStatusCode.Accepted, newUpdatedProfile);

                            return response;
                        }
                        else 
                        {
                            response = Request.CreateResponse(HttpStatusCode.NotFound, "You alredy have profile created, maybe update?");

                            return response;

                        }

                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "You alredy have profile created, maybe update?");

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
