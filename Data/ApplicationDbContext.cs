using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestingFramework.Models;
using TestingFramework.Models.Tasks;

namespace TestingFramework.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<CategoryTestModel> CategoryTests { get; set; }
        public DbSet<ScorecardModel> Scorecards { get; set; }
        public DbSet<ScorecardTestModel> ScorecardTests { get; set; }
        public DbSet<ScorecardProgressModel> ScorecardsInProgress { get; set; }
        public DbSet<ScorecardResultModel> ScorecardResults { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<TaskHistoryModel> TaskHistory { get; set; }
        public DbSet<TaskCommentModel> TaskComments { get; set; }
        public DbSet<IdentityUser> AspNetUsers { get; set; }
    }
}
