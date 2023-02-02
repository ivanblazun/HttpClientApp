using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebHttpClient.Data;
using WebHttpClient.Models;

namespace WebHttpClient.Controllers
{
    public class SqlAdminController : ApiController
    {
        HttpClient client = new HttpClient();

        HttpRequestMessage message = new HttpRequestMessage();

        public AppDbContext appDbContext = new AppDbContext();


        // GET Get all users from admin user api/sqladmin
        [HttpGet]
        public List<string> Get([FromBody] User adminUser)
        {
            var isAdminUserId = appDbContext.Users.FirstOrDefault(u => u.Id == adminUser.Id) as User;

            List<string> allUsers = new List<string>();

            if (isAdminUserId.UserStatus == 1)
            {
                var selectedUsers = appDbContext.Users;

                foreach (var user in selectedUsers)
                {
                    allUsers.Add(JsonConvert.SerializeObject(user));
                }

                return allUsers;
            }
            else
            {
                return allUsers;
            }
        }

        // GET Get single user by id from admin user api/sqladmin
        [HttpGet]
        [Route ("api/sqladmin/GetSingleUser/{Id:int}")]
        public HttpResponseMessage GetSingleUser([FromBody] User adminUser,[FromUri] int Id)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();

            var isAdminUserId = appDbContext.Users.FirstOrDefault(u => u.Id == adminUser.Id) as User;

            User requestedUser = appDbContext.Users.Where(u => u.Id == Id).FirstOrDefault();

            if (isAdminUserId.UserStatus == 1)
            {

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

        // PUT Update selected user by user id api/sqladmin/
        [Route ("api/sqladmin/UpdateUser")]
        [HttpPut]
        public HttpResponseMessage UpdateUser([FromBody] User updatedUser) 
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
                  
            bool doesUserExist = appDbContext.Users.Where(u => u.Id == updatedUser.Id).Any();


            if (doesUserExist)
            {
                User user = appDbContext.Users.Where(u => u.Id == updatedUser.Id).FirstOrDefault();

                user.UserName=updatedUser.UserName;
                user.Password = updatedUser.Password;
                user.Posts = updatedUser.Posts;
                user.UserStatus = updatedUser.UserStatus;
                       
                appDbContext.SaveChanges();

                var response = httpRequest.CreateResponse(HttpStatusCode.OK);

                response.Content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");

                return response;
            }

            else 
            {
                var response = httpRequest.CreateResponse(HttpStatusCode.NotFound);

                response.Content = new StringContent("User not found", System.Text.Encoding.UTF8, "application/json");

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
