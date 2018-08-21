using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly string _authKey;
        private readonly double _expirationInMinutes;

        public TokenController(AppDbContext context, UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration config) : base(context, userManager, roleManager,
            config)
        {
            _authKey = _config["Auth:Jwt:Key"];
            if (_authKey == null)
            {
                throw new ApplicationException("AuthKey is null");
            }
            if (!double.TryParse(_config["Auth:Jwt:ExpirationInMinutes"], out _expirationInMinutes))
            {
                _expirationInMinutes = 60;
            }
        }

        [HttpPost("Auth")]
        public async Task<IActionResult> Jwt([FromBody] TokenRequestViewModel model)
        {
            switch (model.GrantType)
            {
                case "password":
                    return await GetTokenByPassword(model);

                case "refresh_token":
                    return await GetTokenByRefreshToken(model);

                default:
                    return Unauthorized();
            }
        }

        private string CreateAccessToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var iat = DateTime.UtcNow;
            var exp = iat.AddMinutes(_expirationInMinutes);
            var token = new JwtSecurityToken(
                _config["Auth:Jwt:Issuer"],
                _config["Auth:Jwt:Audience"],
                claims,
                iat,
                exp,
                cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string CreateRefreshToken(string userId, string clientId)
        {
            var token = new Token
            {
                UserId = userId,
                ClientId = clientId,
                Value = Guid.NewGuid().ToString("N")
            };

            _context.Tokens.Add(token);

            return token.Value;
        }

        private async Task<IActionResult> CreateTokenResponse(AppUser user, string clientId)
        {
            string accessToken = CreateAccessToken(user);
            string refreshToken = CreateRefreshToken(user.Id, clientId);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new TokenResponseViewModel
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_expirationInMinutes).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
            });
        }

        private async Task<IActionResult> GetTokenByPassword(TokenRequestViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password) && string.IsNullOrWhiteSpace(model.ClientId))
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

            return await CreateTokenResponse(user, model.ClientId);
        }

        private async Task<IActionResult> GetTokenByRefreshToken(TokenRequestViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RefreshToken) || string.IsNullOrWhiteSpace(model.ClientId))
            {
                return Unauthorized();
            }

            var refreshToken = await _context.Tokens.SingleOrDefaultAsync(t => t.ClientId == model.ClientId && t.Value == model.RefreshToken);

            if (refreshToken == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(refreshToken.UserId);
            if (user == null)
            {
                return Unauthorized();
            }

            _context.Remove(refreshToken);

            return await CreateTokenResponse(user, model.ClientId);
        }
    }
}