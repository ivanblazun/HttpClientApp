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

        // Navigation props

        public int UserId { get; set; }

        public User User { get; set; }

        public int PostId { get; set; }

        public List<Post> Posts { get; set; }
    }
}