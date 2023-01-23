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
    public class SqlForumCallsController : ApiController
    {
        public AppDbContext appDbContext = new AppDbContext();

        // GET Get main forum entry point
        [HttpGet]
        [Route("api/sqlforumcalls/getforum")]
        public HttpResponseMessage GetForum()
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();

            var response = new HttpResponseMessage();

            Forum forum = appDbContext.Forums.Where(f => f.Id == 3).FirstOrDefault();

            response = Request.CreateResponse(HttpStatusCode.Accepted, forum);

            return response;
        }

        // GET Get all forum themes
        [HttpGet]
        [Route("api/sqlforumcalls/getallthemes")]
        public HttpResponseMessage GetAllThemes()
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();

            var response = new HttpResponseMessage();

            Forum forum = appDbContext.Forums.Where(f => f.Id == 3).FirstOrDefault();

            var joinedThemesToForum =           
                              (from themes in appDbContext.Themes
                              join forums in appDbContext.Forums on themes.ForumId equals forums.Id
                              select new
                              {
                                  ThemeId=themes.Id,
                                  ThemeTitle=themes.Title,
                                  ThemeValue=themes.Value,
                                  ThemeUserId=themes.UserId
                              }).Take(10);

            response = Request.CreateResponse(HttpStatusCode.Accepted, joinedThemesToForum);

            return response;
        }

        // GEt Get all posts and their from single theme
        [HttpGet]
        [Route("api/sqlforumcalls/getallpostsfromtheme/{themeName}")]
        public HttpResponseMessage GetAllPostsFromSingleTheme([FromUri] string themeName) 
        {
            var response = new HttpResponseMessage();

            bool themeExist = appDbContext.Themes.Any(t => t.Title == themeName);

            if (!themeExist)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Theme was not found");

                return response;
            }
            else 
            {
                var requestedTheme = appDbContext.Themes.Where(t => t.Title == themeName).FirstOrDefault();

                var joinedPostsOnTheme= (from posts in appDbContext.Posts
                                         join theme in appDbContext.Themes on posts.ThemeId equals requestedTheme.Id
                                         select new
                                         {
                                             PostId = posts.Id,
                                             PostTitle = posts.Title,
                                             PostBody=posts.Body,
                                             PostValue = posts.Value,
                                             PostUserId = posts.UserId,
                                             PostAnswers=posts.Answers,
                                             Post_From_ThemeTitle= requestedTheme.Title,
                                             ThemeId= requestedTheme.Id
                                         });

                response = Request.CreateResponse(HttpStatusCode.OK, joinedPostsOnTheme);

                return response;
            }

       
        }
    }
}


