using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebHttpClient.Models
{
    public class Post
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Value { get; set; }

        // Navigation props

        public int UserId { get; set; }
        public User User { get; set; }

        //public int ThemeId { get; set; }
        //public Theme Theme { get; set; }
        //public List<Answer> Answers { get; set; }

    }
}