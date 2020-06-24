using FilmManagement_BE.Models;
using FilmManagement_BE.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmManagement_BE.Services
{
    public class ScenarioService
    {
        private readonly FilmManagerContext _context;

        public ScenarioService(FilmManagerContext context)
        {
            _context = context;
        }

        public ScenarioVModel AddScenario(ScenarioVModel scenario)
        {
            Scenario model = new Scenario()
            {
                Name = scenario.Name,
                Description = scenario.Description,
                Location = scenario.Location,
                TimeStart = scenario.TimeStart,
                TimeEnd = scenario.TimeEnd,
                Script = scenario.Script,
                Status = 0,
                CreateTime = DateTime.Now,
                CreateById = scenario.CreatedBy.Id
            };
            _context.Scenario.Add(model);
            _context.SaveChanges();

            return scenario;
        }

        public ScenarioVModel UpdateScenario(ScenarioVModel scenario)
        {
            var current = _context.Scenario.Where(record => record.Id == scenario.Id).FirstOrDefault();

            if (current != null)
            {
                current.Name = scenario.Name;
                current.Description = scenario.Description;
                current.Location = scenario.Location;
                current.TimeStart = scenario.TimeStart;
                current.TimeEnd = scenario.TimeEnd;
                current.RecordQuantity = scenario.RecordQuantity;
                current.Script = scenario.Script;
                current.Status = scenario.Status;
                current.LastModified = DateTime.Now;
                current.LastModifiedById = scenario.LastModifiedBy.Id;

                _context.Entry(current).State = EntityState.Modified;
                _context.SaveChanges();

                return scenario;
            }

            return null;
        }

        public bool DeleteScenario(Scenario scenario)
        {
            try
            {
                _context.Remove(scenario);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                scenario.Status = -1;
                _context.Entry(scenario).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
        }

        public ICollection<ScenarioVModel> GetListScenario()
        {
            var result = _context.Scenario.OrderBy(record => record.CreateTime).ToList();
            return this.ParseToVModel(result);
        }

        public ScenarioVModel GetById(long id)
        {
            var result = _context.Scenario
                .Where(record => record.Id == id)
                .Include(record => record.CreateBy)
                .Include(record => record.LastModifiedBy)
                .FirstOrDefault();

            return this.ParseToVModel(new List<Scenario> { result }).FirstOrDefault();
        }

        public bool AddActorToScenario(ScenarioAccountVModel scenAcc)
        {
            var check = _context.ScenarioAccountDetail
                .Where(
                    record => 
                        record.AccountId == scenAcc.Account.Id 
                        && record.ScenarioId == scenAcc.Scenario.Id
                ).FirstOrDefault();

            if (check != null) return false;

            var model = new ScenarioAccountDetail()
            {
                AccountId = scenAcc.Account.Id,
                ScenarioId = scenAcc.Scenario.Id,
                CreateTime = DateTime.Now,
                CreateById = scenAcc.CreateBy.Id,
                Characters = scenAcc.Characters
            };
            _context.ScenarioAccountDetail.Add(model);
            _context.SaveChanges();

            return true;
        }

        public bool ChangeCharacterForActor(ScenarioAccountVModel scenAcc)
        {
            var check = _context.ScenarioAccountDetail
                .Where(
                    record =>
                        record.AccountId == scenAcc.Account.Id
                        && record.ScenarioId == scenAcc.Scenario.Id
                ).FirstOrDefault();

            if (check == null) return false;

            check.Characters = scenAcc.Characters;
            check.LastModified = DateTime.Now;
            check.LastModifiedById = scenAcc.LastModifiedBy.Id;

            _context.Entry(check).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public bool RemoveActorFromScenario(ScenarioAccountVModel scenAcc)
        {
            var check = _context.ScenarioAccountDetail
                .Where(
                    record =>
                        record.AccountId == scenAcc.Account.Id
                        && record.ScenarioId == scenAcc.Scenario.Id
                ).FirstOrDefault();

            if (check == null) return false;

            _context.ScenarioAccountDetail.Remove(check);
            _context.SaveChanges();

            return true;
        }

        private ICollection<ScenarioVModel> ParseToVModel(ICollection<Scenario> list)
        {
            var result = new List<ScenarioVModel>();
            foreach (Scenario scen in list)
            {
                var vmodel = new ScenarioVModel()
                {
                    Id = scen.Id,
                    Name = scen.Name,
                    Location = scen.Location,
                    Description = scen.Description,
                    Script = scen.Script,
                    RecordQuantity = scen.RecordQuantity ?? default,
                    CreatedTime = scen.CreateTime ?? default,
                    CreatedBy = scen.CreateBy != null ? new AccountVModel()
                    {
                        Id = scen.CreateBy.Id,
                        Fullname = scen.CreateBy.Fullname,
                        Username = scen.CreateBy.Username
                    } : null,
                    Status = scen.Status ?? default,
                    TimeStart = scen.TimeStart ?? default,
                    TimeEnd = scen.TimeEnd ?? default,
                    LastModified = scen.LastModified ?? default,
                    LastModifiedBy = scen.LastModifiedBy != null ? new AccountVModel()
                    {
                        Id = scen.LastModifiedBy.Id,
                        Fullname = scen.LastModifiedBy.Fullname,
                        Username = scen.LastModifiedBy.Username
                    } : null,
                };

                result.Add(vmodel);
            }
            return result;
        }
    }
}
