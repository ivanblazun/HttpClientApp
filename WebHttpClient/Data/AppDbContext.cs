using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebHttpClient.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebHttpClient.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserStatus> UserStatuses { get; set; }

        public DbSet<Forum> Forums { get; set; }

        public DbSet<MainForum> MainForums{ get; set; }
        public DbSet<Theme> Themes { get; set; }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Post>()
            //   .HasRequired(f => f.Answers)
            //   .WithRequiredDependent()
            //   .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //   .HasRequired(f => f.Answers)
            //   .WithRequiredDependent()
            //   .WillCascadeOnDelete(false);
          
        }


    }
}
