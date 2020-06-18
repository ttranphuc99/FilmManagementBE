using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmManagement_BE.Models;
using FilmManagement_BE.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FilmManagement_BE.Services
{
    public class LoginService
    {
        private readonly FilmManagerContext _context;

        public LoginService(FilmManagerContext context)
        {
            _context = context;
        }

        public AccountVModel CheckLogin(Account account)
        {
            AccountVModel result = null;
            string username = account.Username;
            string password = account.Password;
            System.Diagnostics.Debug.WriteLine("SomeText " + account.DeviceToken);
            Account dbAccount = _context.Account
                .Where(record =>
                    record.Username == username
                    && record.Password == password
                ).FirstOrDefault();

            if (dbAccount != null)
            {
                dbAccount.DeviceToken = account.DeviceToken;
                _context.Entry(dbAccount).State = EntityState.Modified;
                _context.SaveChanges();

                result = new AccountVModel()
                {
                    Id = dbAccount.Id,
                    Username = dbAccount.Username,
                    Fullname = dbAccount.Fullname,
                    Description = dbAccount.Description,
                    Email = dbAccount.Email,
                    Phone = dbAccount.Phone,
                    Image = dbAccount.Image,
                    Role = dbAccount.Role
                };
            }

            return result;
        }

        public AccountVModel Register(Account account)
        {
            AccountVModel result = null;

            _context.Account.Add(account);
            _context.SaveChanges();
            var newAcc = _context.Account
                .Where(record => record.Username == account.Username).FirstOrDefault();

            result = new AccountVModel()
            {
                Id = newAcc.Id,
                Username = newAcc.Username,
                Fullname = newAcc.Fullname,
                Description = newAcc.Description,
                Email = newAcc.Email,
                Phone = newAcc.Phone,
                Image = newAcc.Image,
                Role = newAcc.Role
            };

            return result;
        }

        public bool IsExistedUsername(string username)
        {
            return _context.Account
                .Where(record => record.Username == username)
                .ToList().Count
                > 0;
        }
    }
}
