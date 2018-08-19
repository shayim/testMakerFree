using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SQLitePCL;
using TestMakerFreeWebApp.Models;

namespace TestMakerFreeWebApp.Data
{
    public class DbSeeder
    {
        public static void Seed(
            AppDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IHostingEnvironment env)
        {
            context.Database.Migrate();
            if (!context.Users.Any())
            {
                CreateUsers(context, userManager, roleManager, env).GetAwaiter().GetResult();

                if (env.IsDevelopment())
                {
                    CreateSampleQuizzes(context);
                }
            }
        }

        private static void CreateSampleQuizzes(AppDbContext context)
        {
            var appUsers = context.Users.ToList();
            var admin = appUsers.Single(u => u.UserName == "shayim");
            var users = appUsers.Where(u => u.UserName != "shayim").ToList();

            var quiz = new Quiz
            {
                Title = "Which Shingeki No Kyojin character are you?",
                Description = "Anime-related personality test",
                CreatedDate = DateTime.Now.AddDays(-100),
                LastModifiedDate = DateTime.Now.AddDays(-50),
                User = admin
            };

            var sampleQuizzes = new List<Quiz>();

            for (int i = 1; i <= 47; i++)
            {
                Random rand = new Random();
                var createdDate = DateTime.Now.AddDays(-rand.Next(100));
                sampleQuizzes.Add(new Quiz()
                {
                    User = users[(rand.Next(3) % 3)],
                    Title = String.Format("Sample Quiz Title {0}", i),
                    Description = $"This is a sample description for quiz {i}",
                    Text = @"This is a sample quiz created by the DbSeeder class for testing purposes.All the questions, answers & results are auto-generated as well.",
                    ViewCount = rand.Next(50),
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate.AddDays(rand.Next(10))
                });
            }

            EntityEntry<Quiz> e1 = context.Quizzes.Add(new Quiz()
            {
                UserId = admin.Id,
                Title = "Are you more Light or Dark side of the Force?",
                Description = "Star Wars personality test",
                Text = @"Choose wisely you must, young padawan: " +
          "this test will prove if your will is strong enough " +
          "to adhere to the principles of the light side of the Force " +
          "or if you're fated to embrace the dark side. " +
          "No you want to become a true JEDI, you can't possibly miss this!",
                ViewCount = 2343,
            });

            EntityEntry<Quiz> e2 = context.Quizzes.Add(new Quiz()
            {
                UserId = admin.Id,
                Title = "GenX, GenY or Genz?",
                Description = "Find out what decade most represents you",
                Text = @"Do you feel confortable in your generation? What year should you have been born in? Here's a bunch of questions that will help you to find out!",
                ViewCount = 4180,
            });

            EntityEntry<Quiz> e3 = context.Quizzes.Add(new Quiz()
            {
                UserId = admin.Id,
                Title = "Which Shingeki No Kyojin character are you?",
                Description = "Attack On Titan personality test",
                Text = @"Do you relentlessly seek revenge like Eren? Are you willing to put your like on the stake to protect your friends like Mikasa? Would you trust your fighting skills like Levi or rely on your strategies and tactics like Arwin? Unveil your true self with this Attack On Titan personality test!",
                ViewCount = 5203,
            });

            context.Add(quiz);
            context.AddRange(sampleQuizzes);
            context.SaveChanges();
        }

        private static async Task CreateUsers(AppDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IHostingEnvironment env)
        {
            var roleAdmin = "Admin";
            var roleUser = "User";

            if (!await roleManager.RoleExistsAsync(roleAdmin))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole(roleUser));
            }

            var admin = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "shayim",
                DisplayName = "Howard Wang",
                Email = "shaynv@hotmail.com",
                LockoutEnabled = false
            };

            var passwordValidators = userManager.PasswordValidators;

            if (await userManager.FindByNameAsync(admin.UserName) == null)
            {
                // should have an error violate password rules
                admin.SecurityStamp = Guid.NewGuid().ToString();
                var idResult = await userManager.CreateAsync(admin, password: "Pa$$w0rd");

                if (idResult.Succeeded)
                {
                    await userManager.AddToRolesAsync(admin, new[] { roleAdmin, roleUser });
                }

                var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(admin);
                await userManager.ConfirmEmailAsync(admin, emailConfirmToken);

                // see whether the security stamp created automatically & is emailed confirmed
                var securityStamp = await userManager.GetSecurityStampAsync(admin);
                var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(admin);
            }

            if (env.IsDevelopment())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "Ryan",
                        Email = "ryan@testmakerfree.com",
                       DisplayName = "Ryan",
                        LockoutEnabled = true
                    },
                    new AppUser {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "Solice",
                        Email = "solice@testmakerfree.com",
                       DisplayName = "Solice",
                        LockoutEnabled = true
                    },
                    new AppUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "Vodan",
                        Email = "vodan@testmakerfree.com",
                        DisplayName = "Vodan",
                        LockoutEnabled = true
                    },
                };

                foreach (var user in users)
                {
                    if (await userManager.FindByNameAsync(user.UserName) == null)
                    {
                        // should have an error violate password rules
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        await userManager.CreateAsync(user, password: "Pa$$w0rd");

                        await userManager.AddToRolesAsync(admin, new[] { roleUser });
                    }
                }
            }
        }
    }
}