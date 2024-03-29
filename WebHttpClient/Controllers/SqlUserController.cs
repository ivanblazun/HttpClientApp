﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http;
using WebHttpClient.Data;
using WebHttpClient.Models;
using WebHttpClient.UserSecurity;
using System.Net.Mail;

namespace WebHttpClient.Controllers
{
    public class SqlUserController : ApiController
    {


        public AppDbContext appDbContext = new AppDbContext();

        // Send email after user register
        
        public IHttpActionResult SendRegistermail([FromBody]RegisterEmail regMail)
        {
            string emailUser= System.Configuration.ConfigurationManager.AppSettings["EmailUser"];
            string emailPassword = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];

            string subject = regMail.subject;
            string body = regMail.body;
            string to = regMail.to;

            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(emailUser);
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(emailUser, emailPassword);
            smtp.Send(mailMessage);

            return Ok();

        }

        // POSt Create and register new user
        [HttpPost]
        [Route("api/sqluser/registeruser")]
        public HttpResponseMessage RegisterUser([FromBody] User newUser)
        {

            bool doesUserExist = appDbContext.Users.Where(u => u.UserName == newUser.UserName || u.Email == newUser.Email).Any();

            HttpResponseMessage response = new HttpResponseMessage();

            string userName = newUser.UserName.ToString();

            string password = newUser.Password.ToString();

            string email = newUser.Email.ToString();

            DateTime dateTime = DateTime.Now;
                       

            if (!doesUserExist)
            {
                User registerUser = new User
                { 
                    UserName=userName,
                    Password=password,
                    Email=email,
                    RegisteredDate=dateTime,
                    UserStatus=0
                };


                appDbContext.Users.Add(registerUser);

                appDbContext.SaveChanges();

                response = Request.CreateResponse(HttpStatusCode.OK,registerUser);

                RegisterEmail fromUserCredentials = new RegisterEmail();

                fromUserCredentials.body = "Than you for registration, your user name is " + registerUser.UserName + " and your password is " +registerUser.Password;
                fromUserCredentials.subject = "Success registration, thank you " +registerUser.UserName;
                fromUserCredentials.to = registerUser.Email;
                SendRegistermail(fromUserCredentials);


                return response;
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "User alredy exist");
                       
                return response;
            }
        }

        // POST View other users profile
        [Authorize]
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

        }

        // POST api/sqluser/createuserprofile  
        [Authorize]
        [HttpPost]
        [Route("api/sqluser/createuserprofile")]
        public HttpResponseMessage CreateUserProfile(HttpRequestMessage httpRequest, [FromBody] UserProfile createdProfile)
        {

            var cU = CuuUser.GetCurrUser();

            var currentUser = appDbContext.Users.Where(u => u.UserName == cU).FirstOrDefault();
            
            // radi sto bi trebao ali netreba, neka ostane za nedaj bože
            bool doesUserProfileExist = appDbContext.UserProfiles
                .Where(uP => uP.UserId == currentUser.Id)
                .Any();
            
            var userProfile = appDbContext.UserProfiles
                .Where(u => u.FirstName == createdProfile.FirstName || u.LastName == createdProfile.LastName)
                .FirstOrDefault();
                        
            var response = new HttpResponseMessage();

            UserProfile newProfile = new UserProfile();

            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered user to create your profile");

                return response;
            }
            else
            {
                if (!doesUserProfileExist)
                {

                    newProfile.FirstName = createdProfile.FirstName;
                    newProfile.LastName = createdProfile.LastName;
                    newProfile.Avatar = createdProfile.Avatar;
                    newProfile.AboutMyself = createdProfile.AboutMyself;
                    newProfile.UserId = currentUser.Id;

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
        }

        //GET Get users own full profile
        [Authorize]
        [HttpGet]
        [Route("api/sqluser/getspecificfullprofile/{userId}")]
        public HttpResponseMessage GetSpecificFullProfile(HttpRequestMessage httpRequest, int userId)
        {
            var response = new HttpResponseMessage();
            UserProfile newUpdatedProfile = new UserProfile();

            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered user to view profile");
                return response;
            }
            else
            {
                var cU = CuuUser.GetCurrUser();

                var currentUser = appDbContext.Users.Where(u => u.UserName == cU).FirstOrDefault();

                var searchedProfile = appDbContext.UserProfiles.Where(uP => uP.UserId == userId).FirstOrDefault();

                bool doesUserOwnProfile = appDbContext.UserProfiles.Any(uP => uP.UserId == currentUser.Id && searchedProfile.UserId == currentUser.Id);

                if (doesUserOwnProfile)
                {
                    var joinedUserProfileOnUser = (from user in appDbContext.Users
                                                   join userprofile in appDbContext.UserProfiles on currentUser.Id equals searchedProfile.UserId
                                                   select new
                                                   {
                                                       currentUser.Id,
                                                       currentUser.UserName,
                                                       searchedProfile.FirstName,
                                                       searchedProfile.LastName,
                                                       searchedProfile.AboutMyself,
                                                       searchedProfile.Avatar,
                                                       currentUser.UserStatus,
                                                       currentUser.Email,
                                                       currentUser.Password,
                                                       currentUser.RegisteredDate,
                                                   }).Take(1);


                    response = Request.CreateResponse(HttpStatusCode.Accepted, joinedUserProfileOnUser);

                    return response;
                }
            }
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be owner of profile to view full profile");

            return response;
        }

        // PUT Update user profile
        [Authorize]
        [HttpPut]
        [Route("api/sqluser/updateuserfullprofile/{profileId}")]
        public HttpResponseMessage UpdateUserProfile(HttpRequestMessage httpRequest, int profileId, [FromBody] UserProfile updatedProfile)
        {
            var response = new HttpResponseMessage();
            UserProfile newUpdatedProfile = new UserProfile();

            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered user to update or create your profile");
                return response;
            }
            else
            {

                var cU = CuuUser.GetCurrUser();

                var currentUser = appDbContext.Users.Where(u => u.UserName == cU).FirstOrDefault();

                var requestedProfile = appDbContext.UserProfiles.Where(uP => uP.Id == profileId).FirstOrDefault();

                bool doesUserOwnProfile = appDbContext.UserProfiles.Any(uP => uP.UserId == currentUser.Id && requestedProfile.UserId == currentUser.Id);

                if (doesUserOwnProfile)
                {
                    var searchedProfile = appDbContext.UserProfiles.Where(uP => uP.Id == profileId).FirstOrDefault();

                    searchedProfile.FirstName = updatedProfile.FirstName;
                    searchedProfile.LastName = updatedProfile.LastName;
                    searchedProfile.Avatar = updatedProfile.Avatar;
                    searchedProfile.AboutMyself = updatedProfile.AboutMyself;

                    appDbContext.SaveChanges();

                    response = Request.CreateResponse(HttpStatusCode.Accepted, searchedProfile);

                    return response;
                }
            }
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be owner of profile to update profile");

            return response;
        }


        //Get current user ?????
        [HttpGet]
        [Route("api/sqluser/getuser")]
        public HttpResponseMessage GetUser()
        {
            var cU = CuuUser.GetCurrUser();

            var currentUser = appDbContext.Users.Where(u => u.UserName == cU).FirstOrDefault();

            bool userExist = appDbContext.Users.Any(uE => uE.Id == currentUser.Id);

            if (userExist)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, currentUser);
            }
            else 
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User doest exist");
            }
            

        }

        [Authorize]
        //Request login via JWt V2, also "Userstatus" value is required in request body (1=user,2=powerUser,3=admin)
        [AllowAnonymous]
        [HttpPost]
        [Route("api/sqluser/validlogin2")]
        public HttpResponseMessage ValidLogin2(User user)
        {
            bool isUser = appDbContext.Users.Any(u => u.UserName == user.UserName && u.Password == user.Password && u.UserStatus == user.UserStatus);
                        
            if (isUser)
            {
                var token = TokemManager.CreateJWT(user);
                
                return Request.CreateResponse(HttpStatusCode.Accepted, token);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "User name or password are invalid");
            }


        }

        //Login as user with token
        [Authorize(Roles = "3")]
        [HttpGet]
        [Route("api/sqluser/uservalidlogin2")]
        public HttpResponseMessage userValidLoginSuccess()
        {

            return Request.CreateResponse(HttpStatusCode.OK, "Success login as user");

        }

        //Login as power user with token
        [Authorize(Roles = "2")]
        [HttpGet]
        [Route("api/sqluser/poweruservalidlogin2")]
        public HttpResponseMessage powerUserValidLoginSuccess()
        {

            return Request.CreateResponse(HttpStatusCode.OK, "Success login as user");

        }

        // Login as admin with token
        [Authorize(Roles = "1")]
        [HttpGet]
        [Route("api/sqluser/adminvalidlogin2")]
        public HttpResponseMessage AdminValidLoginSuccess()
        {

            return Request.CreateResponse(HttpStatusCode.OK, "Success login as admin");

        }
    }
}
