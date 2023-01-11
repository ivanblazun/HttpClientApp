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
        public int ThemesCounter { get; set; }

        // Navigation props
        
        public List<Theme> Posts { get; set; }
    }
}