using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using FilmManagement_BE.Models;
using FilmManagement_BE.ViewModels;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FilmManagement_BE.Services;
using Microsoft.AspNetCore.Http;
using FilmManagement_BE.Constants;

namespace FilmManagement_BE.Controllers
{
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly FilmManagerContext _context;
        private readonly IConfiguration _config;
        private readonly LoginService _service;
        private readonly AccountService _accountService;

        public AuthenticationController(FilmManagerContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _service = new LoginService(_context);
            _accountService = new AccountService(_context);
        }

        [AllowAnonymous]
        [HttpPost("api/login")]
        public ActionResult<AccountVModel> Login(Account account)
        {
            var result = _service.CheckLogin(account);
            if (result != null)
            {
                result.Token = this.GenerateJSONWebToken(result);
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpGet("api/logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult Logout()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);

            if (_service.Logout(userId))
            {
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Processing Failed" });
        }

        [HttpGet("api/profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult GetProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);

            var profile = _accountService.GetProfile(userId);

            if (profile == null) return NotFound("Not found user ID " + userId);

            return Ok(profile);
        }

        [HttpPost("api/change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult ChangePassword(AccountVModel account)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);

            if (account.Password != null && account.Password.Trim().Length > 0)
            {
                if (_accountService.ChangePassword(userId, account.Password))
                {
                    return Ok();
                }
                return BadRequest("Process changing password failed");
            }

            return BadRequest("Cannot change password");
        }

        private string GenerateJSONWebToken(AccountVModel account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Id + ""),
                    new Claim(ClaimTypes.GivenName, account.Username),
                    new Claim(ClaimTypes.Role, account.Role + "")
                }),
                Audience = _config["Jwt:Issuer"],
                Issuer = _config["Jwt:Issuer"],
                Expires = DateTime.Now.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}