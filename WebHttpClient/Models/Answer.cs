using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHttpClient.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public string Body { get; set; }

        // Navigation props

        public int PostId { get; set; }
        public Post ReferToPost { get; set; }
        public int UserId { get; set; }
        public User AnswerFromUser { get; set; }

    }
}