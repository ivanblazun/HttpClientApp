using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHttpClient.Models
{
    public class Forum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ThemesCounter { get; set; }
        public int UserCounter { get; set; }
        public DateTime TimeForumCreated { get; set; }

        // Navigation props

        public System.Nullable<int> MainForumId { get; set; }
        public MainForum MainForum { get; set; }
        public List<Theme> Posts { get; set; }
    }
}