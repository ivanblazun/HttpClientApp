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


namespace WebHttpClient.Controllers
{
    public class SqlController : ApiController
    {


        HttpClient client = new HttpClient();

        HttpRequestMessage message = new HttpRequestMessage();

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
        [Route ("api/sql/GetByTitle")]
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
        public void Post([FromBody] Post sendInput)
        {
        
            var SendNewPostData = new Models.Post
            {
               
                Title = sendInput.Title,
                Body = sendInput.Body,
                Value=sendInput.Value,
                UserId = sendInput.UserId
            };

            appDbContext.Posts.Add(SendNewPostData);

            appDbContext.SaveChanges();

        }

        // PUT Update post api/sql/5
        public void Put(int id, [FromBody] Post updateInput)
        {
            var requestedPost = appDbContext.Posts.Where(i => i.Id==id).FirstOrDefault<Post>();
                      
            if(requestedPost !=null)
            {
                requestedPost.Title = updateInput.Title;
                requestedPost.Body = updateInput.Body;
                requestedPost.Value = updateInput.Value;
                                
               appDbContext.SaveChanges();
            }           
        }

        // DELETE Delete Post api/sql/5
        public void Delete(int id)
        {
            var selectedPost = appDbContext.Posts.Where(i => i.Id == id).FirstOrDefault<Post>();

            appDbContext.Posts.Attach(selectedPost);
            appDbContext.Posts.Remove(selectedPost);
            appDbContext.SaveChanges();

            return ;
        }
    }
}
