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

        public IEnumerable<ScenarioAccountVModel> GetListScenOfActor(int actorId)
        {
            var list = _context.ScenarioAccountDetail
                .Include(record => record.Scenario)
                .Include(record => record.CreateBy)
                .Include(record => record.LastModifiedBy)
                .Where(record => record.AccountId == actorId)
                .OrderByDescending(record => record.Scenario.TimeStart)
                .ToList();

            var result = new List<ScenarioAccountVModel>();

            foreach (var model in list)
            {
                var vmodel = new ScenarioAccountVModel()
                {
                    Account = new AccountVModel()
                    {
                        Id = actorId
                    },
                    Scenario = model.Scenario != null ?
                        this.ParseToVModel(new List<Scenario>() { model.Scenario }).FirstOrDefault()
                        : null,
                    Characters = model.Characters,
                    CreateBy = model.CreateBy != null ? new AccountVModel()
                    {
                        Id = model.CreateBy.Id,
                        Fullname = model.CreateBy.Fullname,
                        Username = model.CreateBy.Username
                    } : null,
                    CreateTime = model.CreateTime,
                    LastModified = model.LastModified,
                    LastModifiedBy = model.LastModifiedBy != null ? new AccountVModel()
                    {
                        Id = model.LastModifiedBy.Id,
                        Fullname = model.LastModifiedBy.Fullname,
                        Username = model.LastModifiedBy.Username
                    } : null
                };
                result.Add(vmodel);
            }

            return result;
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
                CreateById = scenario.CreatedBy.Id,
                RecordQuantity = scenario.RecordQuantity
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
            var result = _context.Scenario.OrderByDescending(record => record.CreateTime).ToList();
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
                AccountId = scenAcc.Account.Id ?? default,
                ScenarioId = scenAcc.Scenario.Id ?? default,
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

        public long? GetEquipmentAvailableForScence(long? equipId, long? scenId)
        {
            var equipment = _context.Equipment.Find(equipId);
            var scence = _context.Scenario.Find(scenId);

            if (equipment == null || scence == null) return null;

            var listScenInTime = _context.Scenario
                .Where(record =>
                    record.Id != scenId &&
                    (
                        (record.TimeStart >= scence.TimeStart &&
                        record.TimeEnd <= scence.TimeEnd)
                        ||
                        (record.TimeStart >= scence.TimeStart &&
                        record.TimeStart <= scence.TimeEnd
                        )
                        ||
                        (record.TimeEnd >= scence.TimeStart &&
                        record.TimeEnd <= scence.TimeEnd)
                    ) &&
                    _context.ScenarioEquipmentDetail
                        .Where(detail =>
                            detail.ScenarioId == record.Id &&
                            detail.EquipmentId == equipment.Id
                        ).Count() > 0
                    )
                .ToList();

            if (listScenInTime.Count == 0) return equipment.Quantity;
            var quantity = 0;
            foreach (var scen in listScenInTime)
            {
                var eachQuantity = _context.ScenarioEquipmentDetail
                    .Where(record =>
                        record.ScenarioId == scence.Id &&
                        record.EquipmentId == equipment.Id
                    ).FirstOrDefault()
                    .Quantity ?? default;
                quantity += eachQuantity;
            }

            return equipment.Quantity - quantity;
        }

        public ScenarioEquipmentVModel AddEquipmentToScen(ScenarioEquipmentVModel scenEqui)
        {
            var model = new ScenarioEquipmentDetail()
            {
                ScenarioId = scenEqui.Scenario.Id ?? default,
                EquipmentId = scenEqui.Equipment.Id ?? default,
                Description = scenEqui.Description,
                Quantity = scenEqui.Quantity,
                CreatedById = scenEqui.CreatedBy.Id,
                CreatedTime = DateTime.Now
            };
            _context.ScenarioEquipmentDetail.Add(model);
            _context.SaveChanges();

            scenEqui.CreatedTime = model.CreatedTime;

            return scenEqui;
        }

        public ScenarioEquipmentVModel UpdateEquipmentInScence(ScenarioEquipmentVModel scenEqui)
        {
            var current = _context.ScenarioEquipmentDetail
                .Where(record => record.ScenarioId == scenEqui.Scenario.Id && record.EquipmentId == scenEqui.Equipment.Id)
                .FirstOrDefault();

            if (current == null) return null;

            current.Description = scenEqui.Description;
            current.Quantity = scenEqui.Quantity;
            current.LastModifiedById = scenEqui.LastModifiedBy.Id;
            current.LastModified = DateTime.Now;

            _context.Entry(current).State = EntityState.Modified;
            _context.SaveChanges();

            scenEqui.LastModified = current.LastModified;

            return scenEqui;
        }

        public bool DeleteEquipmentInScence(long? equipId, long? scenId)
        {
            var current = _context.ScenarioEquipmentDetail
                .Where(record => record.ScenarioId == scenId && record.EquipmentId == equipId)
                .FirstOrDefault();

            if (current == null) return false;

            _context.ScenarioEquipmentDetail.Remove(current);
            _context.SaveChanges();

            return true;
        }

        public IEnumerable<ScenarioAccountVModel> GetListActors(long? scenId)
        {
            var list = _context.ScenarioAccountDetail
                .Include(record => record.Account)
                .Include(record => record.CreateBy)
                .Include(record => record.LastModifiedBy)
                .Where(record => record.ScenarioId == scenId)
                .ToList();

            var result = new List<ScenarioAccountVModel>();

            foreach (var model in list) {
                var vmodel = new ScenarioAccountVModel()
                {
                    Account = model.Account != null ? new AccountVModel()
                    {
                        Id = model.Account.Id,
                        Username = model.Account.Username,
                        Fullname = model.Account.Fullname,
                        Phone = model.Account.Phone,
                        Email = model.Account.Email,
                        Image = model.Account.Image,
                        Status = model.Account.Status ?? default
                    } : null,
                    Characters = model.Characters,
                    CreateBy = model.CreateBy != null ? new AccountVModel()
                    {
                        Id = model.CreateBy.Id,
                        Username = model.CreateBy.Username,
                        Fullname = model.CreateBy.Fullname,
                    } : null,
                    CreateTime = model.CreateTime,
                    LastModifiedBy = model.LastModifiedBy != null ? new AccountVModel()
                    {
                        Id = model.LastModifiedBy.Id,
                        Username = model.LastModifiedBy.Username,
                        Fullname = model.LastModifiedBy.Fullname,
                    } : null,
                    LastModified = model.LastModified
                };

                result.Add(vmodel);
            }

            return result;
        }

        public IEnumerable<ScenarioEquipmentVModel> GetListEquipment(long? scenId)
        {
            var list = _context.ScenarioEquipmentDetail
                .Include(record => record.Equipment)
                .Include(record => record.CreatedBy)
                .Include(record => record.LastModifiedBy)
                .Where(record => record.ScenarioId == scenId)
                .ToList();

            var result = new List<ScenarioEquipmentVModel>();

            foreach (var model in list)
            {
                var vmodel = new ScenarioEquipmentVModel()
                {
                    Equipment = model.Equipment != null ? new EquipmentVModel()
                    {
                        Id = model.Equipment.Id,
                        Name = model.Equipment.Name,
                        Quantity = model.Equipment.Quantity,
                        Status = model.Equipment.Status
                    } : null,
                    Quantity = model.Quantity,
                    Description = model.Description,
                    CreatedBy = model.CreatedBy != null ? new AccountVModel()
                    {
                        Id = model.CreatedBy.Id,
                        Username = model.CreatedBy.Username,
                        Fullname = model.CreatedBy.Fullname,
                    } : null,
                    CreatedTime = model.CreatedTime,
                    LastModifiedBy = model.LastModifiedBy != null ? new AccountVModel()
                    {
                        Id = model.LastModifiedBy.Id,
                        Username = model.LastModifiedBy.Username,
                        Fullname = model.LastModifiedBy.Fullname,
                    } : null,
                    LastModified = model.LastModified
                };

                result.Add(vmodel);
            }

            return result;
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
