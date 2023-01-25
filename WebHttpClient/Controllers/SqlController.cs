using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebHttpClient.Data;
using WebHttpClient.Models;
using System.Text;
using WebHttpClient.UserSecurity;

namespace WebHttpClient.Controllers
{

    public class SqlController : ApiController
    {

        public AppDbContext appDbContext = new AppDbContext();


        // GET Get all posts from user id api/sql
        [HttpGet]
        public List<string> Get([FromBody] User user)
        {

            List<string> allPosts = new List<string>();

            var selectedPosts = appDbContext.Posts.Where(i => i.UserId == user.Id);

            var help = JsonConvert.SerializeObject(selectedPosts);

            allPosts.Add(help);

            return allPosts;
        }

        //GET Get post by id from user id api/sql/GetById/5
        [Route("api/sql/CallById")]
        [HttpGet]
        public string CallById([FromBody] User user, [FromUri] int id)
        {

            var selectedPosts = appDbContext.Posts.Where(i => i.UserId == user.Id);

            string deserializedPost = "";

            var selPost = selectedPosts.Where(i => i.Id == id);

            return JsonConvert.SerializeObject(selPost);

            deserializedPost = selPost.ToString();

            return deserializedPost;
        }

        //GET BY ID api/sql/GetByTitle?title={}
        [Route("api/sql/GetByTitle")]
        [HttpGet]
        public string GetByTitle([FromBody] User user, [FromUri] string title)
        {

            var selectedPosts = appDbContext.Posts.Where(i => i.UserId == user.Id);

            string deserializedPost = "";

            var selPost = selectedPosts.Where(i => i.Title == title);

            return JsonConvert.SerializeObject(selPost);

            deserializedPost = selPost.ToString();

            return deserializedPost;
        }

        //POST Create new post api/sql
        [HttpPost]
        [Route("api/sql/makenewpost")]
        public HttpResponseMessage MakeNewPost([FromBody] Post sendInput, HttpRequestMessage httpRequest)
        {

            var response = new HttpResponseMessage();

            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered user to create post");

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
                    var SendNewPostData = new Models.Post
                    {
                        Title = sendInput.Title,
                        Body = sendInput.Body,
                        Value = sendInput.Value,
                        UserId = sendInput.UserId,
                        ThemeId = sendInput.ThemeId
                    };

                    appDbContext.Posts.Add(SendNewPostData);

                    appDbContext.SaveChanges();

                    response = Request.CreateResponse(HttpStatusCode.OK, SendNewPostData);

                    return response;
                }
            }

            return null;
        }

        // PUT Update post api/sql/updatepost/5
        [HttpPut]
        [Route("api/sql/updatepost/{id}")]
        public HttpResponseMessage UpdatePost(int id, [FromBody] Post updateInput, HttpRequestMessage httpRequest)
        {
            var response = new HttpResponseMessage();

            bool doesPostExist = appDbContext.Posts.Any(p => p.Id == id);

            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered first to update post");

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

                if (UserSecurityAuth.Login(username, password) && doesPostExist)
                {
                    var requestedPost = appDbContext.Posts.Where(i => i.Id == id).FirstOrDefault();

                    var UserRequester = appDbContext.Users.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();

                    bool isUserOwner = appDbContext.Posts.Any(p => p.UserId == UserRequester.Id);

                    if (requestedPost != null && isUserOwner)
                    {
                        requestedPost.Title = updateInput.Title;
                        requestedPost.Body = updateInput.Body;
                        requestedPost.Value = updateInput.Value;

                        appDbContext.SaveChanges();

                        response = Request.CreateResponse(HttpStatusCode.OK, "Post updated");

                        return response;
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.Forbidden, "You have to be owner of post to update post");

                    return response;
                }

            }

            return null;
        }


        // DELETE Delete Post api/sql/5
        [HttpDelete]
        [Route("api/sql/deletepost/{id}")]
        public HttpResponseMessage DeletePost(int id, HttpRequestMessage httpRequest)
        {
            var response = new HttpResponseMessage();

            bool doesPostExist = appDbContext.Posts.Any(p => p.Id == id);

            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered first to delete post");

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

                var selectedPost = appDbContext.Posts.Where(i => i.Id == id).FirstOrDefault<Post>();

                if (UserSecurityAuth.Login(username, password) && doesPostExist)
                {
                    var requestedPost = appDbContext.Posts.Where(i => i.Id == id).FirstOrDefault();

                    var UserRequester = appDbContext.Users.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();

                    bool isUserOwner = appDbContext.Posts.Any(p => p.UserId == UserRequester.Id);

                    if (requestedPost != null && isUserOwner)
                    {

                        appDbContext.Posts.Attach(selectedPost);
                        appDbContext.Posts.Remove(selectedPost);
                        appDbContext.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK, "Post deleted");

                        return response;
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Forbidden, "You have to be owner of post to delete post");

                        return response;
                    }

                }
            }
            return null;
        }
    }
}
