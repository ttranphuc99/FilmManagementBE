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
            Account dbAccount = _context.Account
                .Where(record =>
                    record.Username == username
                    && record.Password == password
                    && record.Status == true
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

        public bool Logout(int userId) 
        {
            Account account = _context.Account.Where(record => record.Id == userId).FirstOrDefault();

            if (account != null)
            {
                account.DeviceToken = null;
                _context.Entry(account).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
