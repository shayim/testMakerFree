using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Models;

namespace TestMakerFreeWebApp.Controllers
{
    public class BaseApiController : Controller
    {
        protected IConfiguration _config;
        protected AppDbContext _context;
        protected RoleManager<IdentityRole> _roleManager;
        protected UserManager<AppUser> _userManager;

        public BaseApiController(
            AppDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }
    }
}