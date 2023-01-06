using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebHttpClient.Models;

namespace WebHttpClient.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public DbSet<User> Users { get; set; }
    }
}