using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebHttpClient.Data;
using WebHttpClient.Models;
using WebHttpClient.UserSecurity;

namespace WebHttpClient.Controllers
{
    public class SqlAdminController : ApiController
    {
        public AppDbContext appDbContext = new AppDbContext();

        // GET Get all users from admin user api/sqladmin
        [Authorize(Roles = "1")]
        [HttpGet]
        [Route ("api/sqladmin/getallusers")]
        public HttpResponseMessage GetAllUsers()
        {
            var cU = CuuUser.GetCurrUser();

            var currentUser = appDbContext.Users.Where(u => u.UserName == cU).FirstOrDefault();

            HttpResponseMessage respone = new HttpResponseMessage();

            if (currentUser.UserStatus == 1)
            {
                var selectedUsers = appDbContext.Users;

                var allUsers = appDbContext.Users;

                respone = Request.CreateResponse(HttpStatusCode.OK, allUsers);

                return respone;
            }
            else
            {
                respone = Request.CreateResponse(HttpStatusCode.Forbidden, "You are not admin");

                return respone;
            }
        }

        // GET Get single user by id from admin user api/sqladmin
        [Authorize(Roles = "1")]
        [HttpGet]
        [Route ("api/sqladmin/GetSingleUser/{userid}")]
        public HttpResponseMessage GetSingleUser( int userid)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            var cU = CuuUser.GetCurrUser();

            var currentUser = appDbContext.Users.Where(u => u.UserName == cU).FirstOrDefault();

            User requestedUser = appDbContext.Users.Where(u => u.Id == userid).FirstOrDefault();

            if (currentUser.UserStatus == 1)
            {
                //response.Content = new StringContent(JsonConvert.SerializeObject(requestedUser), System.Text.Encoding.UTF8, "application/json");

                response = Request.CreateResponse(HttpStatusCode.OK,requestedUser);
                return response;
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden, "You are not admin");
                return response;
            }
        }

        // PUT Update selected user by user id api/sqladmin/
        [Authorize(Roles = "1")]
        [Route ("api/sqladmin/updatesingleuser")]
        [HttpPut]
        public HttpResponseMessage UpdateSingleUser([FromBody] User updatedUser) 
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
                  
            bool doesUserExist = appDbContext.Users.Where(u => u.Id == updatedUser.Id).Any();

            var cU = CuuUser.GetCurrUser();

            var currentUser = appDbContext.Users.Where(u => u.UserName == cU).FirstOrDefault();

            bool userIsAdmin = appDbContext.Users.Any(u => u.UserStatus == 1);


            if (doesUserExist && userIsAdmin)
            {
                User user = appDbContext.Users.Where(u => u.Id == updatedUser.Id).FirstOrDefault();

                user.UserName=updatedUser.UserName;
                user.Password = updatedUser.Password;
                user.Posts = updatedUser.Posts;
                user.UserStatus = updatedUser.UserStatus;
                       
                appDbContext.SaveChanges();

                var response = httpRequest.CreateResponse(HttpStatusCode.OK,user);
                             
                return response;
            }

            else 
            {
                var response = httpRequest.CreateResponse(HttpStatusCode.BadRequest,"User does not exist");

                return response;
            }

        
        }
        // DELETE Delete single user by id from admin user api/sqladmin/DeleteSingleUser/{Id:int}
        [HttpDelete]
        [Route("api/sqladmin/DeleteSingleUser/{Id:int}")]
        public HttpResponseMessage DeleteSingleUser([FromBody] User adminUser, [FromUri] int Id)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();

            var isAdminUserId = appDbContext.Users.FirstOrDefault(u => u.Id == adminUser.Id) as User;


            if (isAdminUserId.UserStatus == 1)
            {
                User requestedUser = appDbContext.Users.Where(u => u.Id == Id).FirstOrDefault();

                appDbContext.Users.Attach(requestedUser);

                appDbContext.Users.Remove(requestedUser);

                appDbContext.SaveChanges();

                var response = httpRequest.CreateResponse(HttpStatusCode.OK);

                response.Content = new StringContent(JsonConvert.SerializeObject(requestedUser), System.Text.Encoding.UTF8, "application/json");

                return response;
            }
            else
            {
                var response = httpRequest.CreateResponse(HttpStatusCode.NotFound);

                response.Content = new StringContent("User not found", System.Text.Encoding.UTF8, "application/json");

                return response;
            }
        }
    }
}
