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
                CreateTime = DateTime.UtcNow.AddHours(7),
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
                current.LastModified = DateTime.UtcNow.AddHours(7);
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

        public ScenarioAccountVModel AddActorToScenario(ScenarioAccountVModel scenAcc)
        {
            var check = _context.ScenarioAccountDetail
                .Where(
                    record => 
                        record.AccountId == scenAcc.Account.Id 
                        && record.ScenarioId == scenAcc.Scenario.Id
                ).FirstOrDefault();

            if (check != null) return null;

            var model = new ScenarioAccountDetail()
            {
                AccountId = scenAcc.Account.Id ?? default,
                ScenarioId = scenAcc.Scenario.Id ?? default,
                CreateTime = DateTime.UtcNow.AddHours(7),
                CreateById = scenAcc.CreateBy.Id,
                Characters = scenAcc.Characters
            };
            _context.ScenarioAccountDetail.Add(model);
            _context.SaveChanges();

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
                LastModified = model.LastModified,
                Scenario = this.ParseToVModel(new List<Scenario>() { model.Scenario }).FirstOrDefault()
            };

            return vmodel;
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
            check.LastModified = DateTime.UtcNow.AddHours(7);
            check.LastModifiedById = scenAcc.LastModifiedBy.Id;

            _context.Entry(check).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public ScenarioAccountVModel RemoveActorFromScenario(ScenarioAccountVModel scenAcc)
        {
            var model = _context.ScenarioAccountDetail
                .Where(
                    record =>
                        record.AccountId == scenAcc.Account.Id
                        && record.ScenarioId == scenAcc.Scenario.Id
                ).FirstOrDefault();

            if (model == null) return null;
            
            _context.ScenarioAccountDetail.Remove(model);
            _context.SaveChanges();

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
                LastModified = model.LastModified,
                Scenario = this.ParseToVModel(new List<Scenario>() { model.Scenario }).FirstOrDefault()
            };

            return vmodel;
        }

        public long? GetEquipmentAvailableForScence(long? equipId, long? scenId)
        {
            var equipment = _context.Equipment.Find(equipId);
            var scence = _context.Scenario.Find(scenId);

            if (equipment == null || scence == null) return null;

            var listScenInTime = _context.Scenario
                .Where(record =>
                    record.Id != scenId &&
                    record.Status != -1 &&
                    record.Status != 2 &&
                    (
                        (record.TimeStart <= scence.TimeStart &&
                        record.TimeEnd >= scence.TimeEnd)
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
                        record.ScenarioId == scen.Id &&
                        record.EquipmentId == equipment.Id
                    ).FirstOrDefault()
                    .Quantity ?? default;
                quantity += eachQuantity;
            }

            return equipment.Quantity - quantity;
        }

        public int? GetEquipmentAvailableForScence(DateTime timeStart, DateTime timeEnd, long? equipId)
        {
            var equipment = _context.Equipment.Find(equipId);
            if (equipment == null) return null;

            var listScenInTime = _context.Scenario
                .Where(record =>
                    record.Status != -1 &&
                    record.Status != 2 &&
                    (
                        (record.TimeStart <= timeStart &&
                        record.TimeEnd >= timeEnd)
                        ||
                        (record.TimeStart >= timeStart &&
                        record.TimeStart <= timeEnd
                        )
                        ||
                        (record.TimeEnd >= timeStart &&
                        record.TimeEnd <= timeEnd)
                    ) &&
                    _context.ScenarioEquipmentDetail
                        .Where(detail =>
                            detail.ScenarioId == record.Id &&
                            detail.EquipmentId == equipId
                        ).Count() > 0
                    )
                .ToList();

            if (listScenInTime.Count == 0) return equipment.Quantity;

            var quantity = 0;
            foreach (var scen in listScenInTime)
            {
                var eachQuantity = _context.ScenarioEquipmentDetail
                    .Where(record =>
                        record.ScenarioId == scen.Id &&
                        record.EquipmentId == equipment.Id
                    ).FirstOrDefault()
                    .Quantity ?? default;
                quantity += eachQuantity;
            }

            return equipment.Quantity - quantity;
        }

        public IEnumerable<EquipmentVModel> GetListEquimentAndAvai(DateTime timeStart, DateTime timeEnd)
        {
            var listModel = _context.Equipment
                .Include(record => record.CreateBy)
                .Include(record => record.LastModifiedBy)
                .Include(record => record.EquipmentImage)
                .OrderByDescending(record => record.CreateTime)
                .ToList();

            List<EquipmentVModel> result = new List<EquipmentVModel>();
            foreach (var model in listModel)
            {
                var vmodel = new EquipmentVModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Quantity = this.GetEquipmentAvailableForScence(timeStart, timeEnd, model.Id),
                    Status = model.Status,
                    CreateBy = model.CreateBy != null ? new AccountVModel()
                    {
                        Id = model.CreateBy.Id,
                        Username = model.CreateBy.Username,
                        Fullname = model.CreateBy.Fullname,
                        Email = model.CreateBy.Email,
                        Phone = model.CreateBy.Phone
                    } : null,
                    CreateTime = model.CreateTime,
                    LastModifiedBy = model.LastModifiedBy != null ? new AccountVModel()
                    {
                        Id = model.LastModifiedBy.Id,
                        Username = model.LastModifiedBy.Username,
                        Fullname = model.LastModifiedBy.Fullname,
                        Email = model.LastModifiedBy.Email,
                        Phone = model.LastModifiedBy.Phone
                    } : null,
                    LastModified = model.LastModified
                };

                if (model.EquipmentImage != null && model.EquipmentImage.Count > 0)
                {
                    var listImg = new List<EquipmentImageVModel>();

                    foreach (var img in model.EquipmentImage)
                    {
                        var imgVmodel = new EquipmentImageVModel()
                        {
                            Id = img.Id,
                            Url = img.Url
                        };

                        listImg.Add(imgVmodel);
                    }

                    vmodel.ListImages = listImg;
                }

                result.Add(vmodel);
            }

            return result;
        }

        public ScenarioEquipmentVModel AddEquipmentToScen(ScenarioEquipmentVModel scenEqui)
        {
            var currentAvailable = this.GetEquipmentAvailableForScence(scenEqui.Equipment.Id, scenEqui.Scenario.Id);

            if (currentAvailable < scenEqui.Quantity) return null;

            var model = new ScenarioEquipmentDetail()
            {
                ScenarioId = scenEqui.Scenario.Id ?? default,
                EquipmentId = scenEqui.Equipment.Id ?? default,
                Description = scenEqui.Description,
                Quantity = scenEqui.Quantity,
                CreatedById = scenEqui.CreatedBy.Id,
                CreatedTime = DateTime.UtcNow.AddHours(7)
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

            var currentAvai = this.GetEquipmentAvailableForScence(scenEqui.Equipment.Id, scenEqui.Scenario.Id);

            if (currentAvai < scenEqui.Quantity) return new ScenarioEquipmentVModel() {Status = -99 } ;

            current.Description = scenEqui.Description;
            current.Quantity = scenEqui.Quantity;
            current.LastModifiedById = scenEqui.LastModifiedBy.Id;
            current.LastModified = DateTime.UtcNow.AddHours(7);

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
                .Include(record => record.Scenario)
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
                    Scenario = model.Scenario != null ? new ScenarioVModel()
                    {
                        Id = model.Scenario.Id,
                        Name = model.Scenario.Name,
                        Location = model.Scenario.Location,
                        Description = model.Scenario.Description,
                        Script = model.Scenario.Script,
                        RecordQuantity = model.Scenario.RecordQuantity ?? default,
                        Status = model.Scenario.Status ?? default,
                        TimeStart = model.Scenario.TimeStart ?? default,
                        TimeEnd = model.Scenario.TimeEnd ?? default,
                    } : null,
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
