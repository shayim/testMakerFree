using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Models;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    //    [Route("[controller]")]
    //    public class TokenController : Controller
    //    {
    //        private readonly IConfiguration _config;
    //        private readonly AppDbContext _context;
    //
    //        public TokenController(AppDbContext context, IConfiguration config)
    //        {
    //            _context = context;
    //            _config = config;
    //        }
    //
    //        [HttpPost("Auth")]
    //        public IActionResult GetToken([FromBody]TokenRequestViewModel model)
    //        {
    //            if (!ModelState.IsValid) return Unauthorized();
    //            AppUser user = _context.Users.SingleOrDefault(u => u.UserName == model.Username);
    //
    //            if (user == null) return Unauthorized();
    //
    //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Auth:Jwt:Key"]));
    //            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    //
    //            var claims = new List<Claim>()
    //            {
    //                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
    //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    //            };
    //
    //            var iat = DateTime.Now;
    //            var exp = iat.AddHours(2);
    //
    //            var token = new JwtSecurityToken(
    //                _config["Auth:Jwt:Issuer"],
    //                _config["Auth:Jwt:Audience"],
    //                claims,
    //                iat,
    //                exp,
    //                cred);
    //
    //            var handler = new JwtSecurityTokenHandler();
    //
    //            var jwtToken = handler.WriteToken(token);
    //
    //            var tokenResponse = new TokenResponseViewModel { Token = jwtToken, Expiration = 7200 };
    //
    //            return Json(tokenResponse);
    //        }
    //    }

    [Route("[controller]")]
    public class TokenController : BaseApiController
    {
        public TokenController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : base(context, userManager, roleManager, config)
        { }

        [HttpPost("Auth")]
        public async Task<IActionResult> Jwt([FromBody] TokenRequestViewModel model)
        {
            if (!ModelState.IsValid) return Unauthorized();

            AppUser user = await _userManager.FindByNameAsync(model.Username);
            if (user == null) return Unauthorized();

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }

            // reach this line, we have a valid user, start to create token

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Auth:Jwt:Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var iat = DateTime.Now;
            var exp = iat.AddMinutes(2);

            var token = new JwtSecurityToken(
                _config["Auth:Jwt:Issuer"],
                _config["Auth:Jwt:Audience"],
                claims,
                iat,
                exp,
                cred);

            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.WriteToken(token);

            var tokenResponse = new TokenResponseViewModel
            {
                Token = jwtToken,
                Expiration = exp.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
            };

            return Json(tokenResponse);
        }
    }
}