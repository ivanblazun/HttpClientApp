﻿using Newtonsoft.Json;
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

    }
}
