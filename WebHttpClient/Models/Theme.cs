using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHttpClient.Models
{
    public class Theme
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Value { get; set; }

        public DateTime TimeThemeCreated { get; set; }


        // Navigation props

        public System.Nullable<int> UserId { get; set; }

        public User User { get; set; }

        public int ForumId { get; set; }
        public Forum Forum { get; set; }
        public List<Post> Posts { get; set; }
    }
}