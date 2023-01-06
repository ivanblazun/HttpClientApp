using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHttpClient.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        // Navigation props

        public List<Post> Posts { get; set; }

    }
}