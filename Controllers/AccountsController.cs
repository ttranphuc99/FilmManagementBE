using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmManagement_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FilmManagement_BE.Constants;
using FilmManagement_BE.Services;
using FilmManagement_BE.ViewModels;
using System.Security.Claims;

namespace FilmManagement_BE.Controllers
{
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly FilmManagerContext _context;
        private readonly AccountService _service;

        public AccountsController(FilmManagerContext context)
        {
            _context = context;
            _service = new AccountService(context);
        }

        [Authorize(Roles = RoleConstants.DIRECTOR_STR, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("/api/actors")]
        public ActionResult<IEnumerable<Account>> GetAccount()
        {
            return Ok(_service.GetList());
        }

        [Authorize(Roles = RoleConstants.DIRECTOR_STR, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("/api/actors/{id}")]
        public ActionResult<AccountVModel> GetAccount(int id)
        {
            var account = _service.GetById(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("/api/accounts")]
        public ActionResult ChangeProfile(AccountVModel account)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);

            account.Id = userId;
            var result = _service.UpdateAccount(account);

            if (result != null) {
                return Ok(result);
            }

            return BadRequest("Cannot change profile");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        [HttpPost("/api/actors")]
        public ActionResult Register(AccountVModel account)
        {
            account.Role = 2;
            if (_service.IsExistedUsername(account.Username))
            {
                return BadRequest(new { message = "Username is existed" });
            }

            var result = _service.Register(account);

            return Created("", result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        [HttpDelete("/api/actors/{id}")]
        public ActionResult<Account> DeleteAccount(int id)
        {
            if (_service.DeleteAccount(id))
            {
                return Ok();
            }
            return BadRequest("Cannot delete account");
        }
    }
}
