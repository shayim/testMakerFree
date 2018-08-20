using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestMakerFreeWebApp.Models;

namespace TestMakerFreeWebApp.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<Result> Results { get; set; }

        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<AppUser>().HasMany<Quiz>().WithOne(q => q.User).HasForeignKey(q => q.UserId).IsRequired();

            mb.Entity<Quiz>().HasMany(q => q.Questions).WithOne().HasForeignKey(q => q.QuizId).IsRequired();

            mb.Entity<Question>().HasMany<Answer>().WithOne().HasForeignKey(a => a.QuestionId).IsRequired();

            mb.Entity<Quiz>().HasMany<Result>(q => q.Results).WithOne().HasForeignKey(r => r.QuizId).IsRequired();

            mb.Entity<AppUser>().HasMany<Token>(u => u.Tokens).WithOne(t => t.AppUser).HasForeignKey(t => t.UserId).IsRequired();

            base.OnModelCreating(mb);

            mb.Entity<AppUser>().ToTable("AppUser");
        }
    }
}