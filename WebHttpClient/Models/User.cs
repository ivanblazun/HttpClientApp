using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebHttpClient.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegisteredDate { get; set; }

        public int UserStatus { get; set; }

        // Navigation props

        //public List<Post> Posts { get; set; }

        //public List<Answer> Answers { get; set; }

        //[ForeignKey("UserProfile")]
        //public int UserProfileId { get; set; }
        //public UserProfile UserProfile { get; set; }

    }
}