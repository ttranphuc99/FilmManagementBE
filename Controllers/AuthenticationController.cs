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

namespace FilmManagement_BE.Controllers
{
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly FilmManagerContext _context;
        private readonly IConfiguration _config;
        private readonly LoginService _service;

        public AuthenticationController(FilmManagerContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _service = new LoginService(_context);
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

        [AllowAnonymous]
        [HttpPost("api/register")]
        public ActionResult Register(Account account)
        {
            account.Role = 2;
            if (_service.IsExistedUsername(account.Username))
            {
                return BadRequest(new { message = "Username is existed" });
            }

            var result = _service.Register(account);
            result.Token = this.GenerateJSONWebToken(result);

            return Created("", result);
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

        [Authorize(Roles = "1", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("api/add-director")]
        public ActionResult<AccountVModel> AddAdmin(Account account)
        {
            return null;
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
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}