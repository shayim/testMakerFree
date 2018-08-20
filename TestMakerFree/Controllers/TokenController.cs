using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Models;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("[controller]")]
    public class TokenController : BaseApiController
    {
        public TokenController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : base(context, userManager, roleManager, config)
        { }

        [HttpPost("Auth")]
        public async Task<IActionResult> Jwt([FromBody] TokenRequestViewModel model)
        {
            switch (model.GrantType)
            {
                case "password":
                    return await GetToken(model);

                case "refresh_token":
                    return await RefreshToken(model);

                default:
                    return Unauthorized();
            }
        }

        private async Task<IActionResult> GetToken(TokenRequestViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return Unauthorized();
            }

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

        private async Task<IActionResult> RefreshToken(TokenRequestViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                return Unauthorized();
            }

            var refreshToken = _context.Tokens.SingleOrDefaultAsync(t => t.ClientId)
        }
    }
}