using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHttpClient.Models
{
    public class UserStatus
    {
        public int Id { get; set; }
        public string StatusTitle { get; set; }

        public int UStatus { get; set; }

        // Navigation props

        public int UserId { get; set; }

        public User User { get; set; }
    }
}