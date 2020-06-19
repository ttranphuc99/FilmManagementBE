using FilmManagement_BE.Constants;
using FilmManagement_BE.Models;
using FilmManagement_BE.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmManagement_BE.Services
{
    public class AccountService
    {
        private readonly FilmManagerContext _context;

        public AccountService(FilmManagerContext context)
        {
            _context = context;
        }

        public AccountVModel Register(AccountVModel account)
        {
            AccountVModel result = null;

            var accountModel = new Account()
            {
                Username = account.Username,
                Password = account.Password,
                Fullname = account.Fullname,
                Email = account.Email,
                Phone = account.Phone,
                Role = account.Role,
                Status = true
            };

            _context.Account.Add(accountModel);
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
                Role = newAcc.Role,
                Status = newAcc.Status?? default(bool)
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

        public bool DeleteAccount(int id)
        {
            Account account = _context.Account.Where(record => record.Id == id).FirstOrDefault();
            account.Status = false;

            _context.Entry(account).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }

        public AccountVModel UpdateAccount(AccountVModel account)
        {
            var currentAccount = _context.Account.Where(record => record.Id == account.Id && record.Status == true).FirstOrDefault();

            if (currentAccount == null)
            {
                return null;
            }

            currentAccount.Fullname = account.Fullname;
            currentAccount.Image = account.Image;
            currentAccount.Phone = account.Phone;
            currentAccount.Email = account.Email;
            currentAccount.Description = account.Description;

            _context.Entry(currentAccount).State = EntityState.Modified;
            _context.SaveChanges();
            return account;
        }

        public AccountVModel GetById(int id)
        {
            var currentAcc = _context.Account.Where(record => record.Id == id).FirstOrDefault();

            var result = new AccountVModel()
            {
                Id = currentAcc.Id,
                Username = currentAcc.Username,
                Fullname = currentAcc.Fullname,
                Description = currentAcc.Description,
                Email = currentAcc.Email,
                Phone = currentAcc.Phone,
                Image = currentAcc.Image,
                Role = currentAcc.Role,
                Status = currentAcc.Status ?? default(bool)
            };

            return result;
        }

        public List<AccountVModel> GetList()
        {
            var listModel = _context.Account.Where(record => record.Role == RoleConstants.ACTOR).ToList();

            var result = new List<AccountVModel>();

            foreach (Account account in listModel) {
                AccountVModel accountVModel = new AccountVModel()
                {
                    Id = account.Id,
                    Username = account.Username,
                    Fullname = account.Fullname,
                    Image = account.Image,
                    Description = account.Description,
                    Phone = account.Phone,
                    Email = account.Email,
                    Status = account.Status ?? default,
                };

                result.Add(accountVModel);
            }

            return result;
        }

        public bool ChangePassword(int id, String newPassword)
        {
            var account = _context.Account.Where(record => record.Id == id && record.Status == true).FirstOrDefault();

            if (account == null) return false;

            account.Password = newPassword;

            _context.Entry(account).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }
    }
}
