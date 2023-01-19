using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebHttpClient.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }

        public string AboutMyself { get; set; }

        //Relations
        //[ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }


    }
}