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
    public class SqlPostController : ApiController
    {

        public AppDbContext appDbContext = new AppDbContext();


        // GET Get all posts from user id api/sqlpost/1
        [HttpGet]
        [Route("api/sqlpost/getallpostfromuser/{userId}")]
        public HttpResponseMessage GetAllPostFromUser(int userId)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            var selectedPosts = appDbContext.Posts.Where(i => i.UserId == userId);

            var help = JsonConvert.SerializeObject(selectedPosts);

            response = Request.CreateResponse(HttpStatusCode.OK, selectedPosts);

            return response;
        }

        //GET Get post by id from user id api/sqlpost/GetById/5
        [Route("api/sqlpost/getspecpostfromuser/{postId}")]
        [HttpGet]
        public HttpResponseMessage GetSpecPostFromUser([FromBody] User user, [FromUri] int postId)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            bool doesPostExist = appDbContext.Posts.Any(i => i.UserId == user.Id && i.Id == postId);

            var selectedPost = appDbContext.Posts.Where(i => i.UserId == user.Id && i.Id == postId);

            if (doesPostExist)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, selectedPost);
                return response;
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "Post not found");
                return response;
            }

        }

        //GET BY ID api/sqlpost/GetByTitle/{title}
        [Route("api/sqlpost/getpostbytitle/{title}")]
        [HttpGet]
        public HttpResponseMessage GetPostByTitle( string title)
        {
            HttpResponseMessage response = new HttpResponseMessage();       

            var post = appDbContext.Posts.Where(t => t.Title.Contains(title)).ToList();

            bool postIsFound = appDbContext.Posts.Any(t => t.Title.Contains(title));

            if (postIsFound)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, post);
                return response;
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "Post does not exist");
                return response;
            }

        }

        //GET BY ID api/sqlpost/GetByContent/{content}
        [Route("api/sqlpost/getpostbycontent/{content}")]
        [HttpGet]
        public HttpResponseMessage GetPostByContent(string content)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            var post = appDbContext.Posts.Where(t => t.Body.Contains(content)).ToList();

            bool postIsFound = appDbContext.Posts.Any(t => t.Body.Contains(content));

            if (postIsFound)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, post);
                return response;
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "Post does not exist");
                return response;
            }

        }

        //POST Create new post api/sql
        [Authorize]
        [HttpPost]
        [Route("api/sqlpost/makenewpost")]
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
                var cU = CuuUser.GetCurrUser();
                var currentUser = appDbContext.Users.Where(u => u.UserName == cU).FirstOrDefault();

                bool doesThemeExist = appDbContext.Themes.Any(t => t.Id == sendInput.ThemeId);

                if (!doesThemeExist)
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, "Post dont contain accurate themeId to belong in");

                    return response;
                }


                if (currentUser != null)
                {
                    var SendNewPostData = new Models.Post
                    {
                        Title = sendInput.Title,
                        Body = sendInput.Body,
                        Value = sendInput.Value,
                        UserId = currentUser.Id,
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

        // PUT Update post api/sqlpost/updatepost/5
        [Authorize]
        [HttpPut]
        [Route("api/sqlpost/updatepost/{postid}")]
        public HttpResponseMessage UpdatePost(int postid, [FromBody] Post updateInput, HttpRequestMessage httpRequest)
        {
            var response = new HttpResponseMessage();


            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered first to update post");

                return response;
            }
            else
            {
                // auth procedure

                var cU = CuuUser.GetCurrUser();
                var currentUser = appDbContext.Users.Where(u => u.UserName == cU).FirstOrDefault();

                var requestedPost = appDbContext.Posts.Where(p => p.Id == postid).FirstOrDefault();

                bool doesPostExist = appDbContext.Posts.Any(p => p.Id == postid);

                bool doesUserOwnPost = appDbContext.Posts.Any(p => p.UserId == currentUser.Id && requestedPost.UserId == currentUser.Id);

                bool doesPostBelogToTheme = requestedPost.ThemeId == updateInput.ThemeId;

                if (doesUserOwnPost && doesPostExist && doesPostBelogToTheme)
                {
                    requestedPost.Title = updateInput.Title;
                    requestedPost.Body = updateInput.Body;
                    requestedPost.Value = updateInput.Value;

                    appDbContext.SaveChanges();

                    response = Request.CreateResponse(HttpStatusCode.OK, requestedPost);

                    return response;
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.Forbidden, "You have to be owner of post to update post or post dont belog to theme");

                    return response;
                }

            }

            return null;
        }


        // DELETE Delete Post api/sqlpost/5
        [Authorize]
        [HttpDelete]
        [Route("api/sqlpost/deletepost/{postid}")]
        public HttpResponseMessage DeletePost(int postid, HttpRequestMessage httpRequest)
        {
            var response = new HttpResponseMessage();

            if (httpRequest.Headers.Authorization == null)
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "You have to be registered first to delete post");

                return response;
            }
            else
            {
                // auth procedure
                var cU = CuuUser.GetCurrUser();
                var currentUser = appDbContext.Users.Where(u => u.UserName == cU).FirstOrDefault();

                var requestedPost = appDbContext.Posts.Where(i => i.Id == postid).FirstOrDefault();

                bool doesPostExist = appDbContext.Posts.Any(p => p.Id == postid);
                if (!doesPostExist)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound, "Post dont exist");
                    
                    return response;
                }

                bool doesUserOwnPost = appDbContext.Posts.Any(p => p.UserId == currentUser.Id && requestedPost.UserId == currentUser.Id);
                if (doesUserOwnPost && doesPostExist)
                {
                    appDbContext.Posts.Attach(requestedPost);
                    appDbContext.Posts.Remove(requestedPost);
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
            return null;
        }
    }
}
