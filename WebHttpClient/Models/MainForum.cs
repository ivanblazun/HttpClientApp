using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHttpClient.Models
{
    public class MainForum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ForumsCounter { get; set; }
        public int UserCounter { get; set; }
    }
}