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
    public class ValuesController : ApiController
    {
         HttpClient client = new HttpClient();

        // GET api/values
        public async Task <List<Response>> Get()
        {        
                string url = "https://jsonplaceholder.typicode.com/posts";
         
                var response = await client.GetAsync(url);                
                response.EnsureSuccessStatusCode();
            
                var content = await response.Content.ReadAsStringAsync();
                                       
                var model = JsonConvert.DeserializeObject<List<Response>>(content);

                return model;
        }

        //GET api/values/5
        public async Task<Response> Get(int id)
        {

            string url = "https://jsonplaceholder.typicode.com/posts";

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<List<Response>>(content);


            //var selectedModel = model.Where(i => i.Id == id);

            var selectedModel = new Response();

            foreach (var item in model)
            {
                if (item.Id == id)
                {
                    selectedModel = item;
                                      
                }
            }
            
            return selectedModel;

        }

        // POST api/values
        public void Post([FromBody] Response sendInput)
        { 
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
