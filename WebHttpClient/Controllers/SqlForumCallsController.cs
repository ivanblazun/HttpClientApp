using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebHttpClient.Data;
using WebHttpClient.Models;
using System.Web.Http.Cors;
using Newtonsoft.Json;
namespace WebHttpClient.Controllers
{
        public class SqlForumCallsController : ApiController
    {
       

        public AppDbContext appDbContext = new AppDbContext();

        //Get main forum entrypoint
        [HttpGet]
        [Route("api/sqlforumcalls/getmainforum")]
        public HttpResponseMessage GetmainForum() 
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();

            var response = new HttpResponseMessage();

            MainForum mainForum = appDbContext.MainForums.Where(f => f.Id == 1).FirstOrDefault();

            response = Request.CreateResponse(HttpStatusCode.Accepted, mainForum);

            return response;
        }

        // GET Get all subforums 
        [HttpGet]
        [Route("api/sqlforumcalls/getallsubforum")]
        public HttpResponseMessage GetForum()
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();

            var response = new HttpResponseMessage();

            List <Forum> listForums = appDbContext.Forums.Where(f => f.MainForumId == 1).ToList();

            response = Request.CreateResponse(HttpStatusCode.Accepted, listForums);

            return response;
        }

        // GET Get subforums by id
        [HttpGet]
        [Route("api/sqlforumcalls/getforum/{id}")]
        public HttpResponseMessage GetForum(int id)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();

            var response = new HttpResponseMessage();

            Forum forum = appDbContext.Forums.Where(f => f.Id == id).FirstOrDefault();

            response = Request.CreateResponse(HttpStatusCode.Accepted, forum);

            return response;
        }
       
        // GET Get all subforum themes by subforum id
        [HttpGet]
        [Route("api/sqlforumcalls/getallthemes/{id}")]
        public HttpResponseMessage GetAllThemes(int id)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();

            var response = new HttpResponseMessage();

            Forum forum = appDbContext.Forums.Where(f => f.Id == id).FirstOrDefault();

            var joinedThemesToForum =
                              (from themes in appDbContext.Themes
                               join forums in appDbContext.Forums on themes.ForumId equals forum.Id
                               select new
                               {
                                   ThemeId = themes.Id,
                                   ThemeTitle = themes.Title,
                                   ThemeValue = themes.Value,
                                   ThemeUserId = themes.UserId,
                                   ThemeBody=themes.ThemeBody

                               }).Distinct().ToList();

            response = Request.CreateResponse(HttpStatusCode.Accepted, joinedThemesToForum);

            return response;
        }

        // GET Get theme by theme name
        [HttpGet]
        [Route("api/sqlforumcalls/getthemebyname/{themename}")]
        public HttpResponseMessage GetThemebyname(string themename)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();

            var response = new HttpResponseMessage();

            var theme = appDbContext.Themes.Where(t => t.Title.Contains(themename)).ToList();

            bool themeIsFound = appDbContext.Themes.Any(t => t.Title.Contains(themename));

            if (themeIsFound)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, theme);
                return response;
            }
            else 
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "Theme is not founded");
                return response;
            }
        }

        // GEt Get all posts  from single theme
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
                int postCounter = 1;

                foreach (var i in appDbContext.Themes) 
                {
                    postCounter++;
                }

                var requestedTheme = appDbContext.Themes.Where(t => t.Title == themeName).FirstOrDefault();

                User user = new User();

                var joinedPostsOnTheme = (from posts in appDbContext.Posts
                                          where posts.ThemeId == requestedTheme.Id
                                          join theme in appDbContext.Themes
                                          on posts.ThemeId equals requestedTheme.Id
                                          into postsPosts

                                          select new
                                          {
                                              PostId = posts.Id,
                                              PostTitle = posts.Title,
                                              PostBody = posts.Body,
                                              PostValue = posts.Value,
                                              PostUserId = posts.UserId,
                                              PostAnswers = posts.Answers,
                                              Post_From_ThemeTitle = requestedTheme.Title,
                                              ThemeId = requestedTheme.Id,
                                              UserName = appDbContext.Users.Where(u => u.Id == posts.UserId).FirstOrDefault().UserName
                                          });

                response = Request.CreateResponse(HttpStatusCode.OK, joinedPostsOnTheme);

                return response;
            }

       
        }
    }
}


