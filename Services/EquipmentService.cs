using FilmManagement_BE.Models;
using FilmManagement_BE.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmManagement_BE.Services
{
    public class EquipmentService
    {
        private FilmManagerContext _context;

        public EquipmentService(FilmManagerContext context)
        {
            _context = context;
        }

        public IEnumerable<EquipmentVModel> GetListEquipments()
        {
            var listModel = _context.Equipment
                .Include(record => record.CreateBy)
                .Include(record => record.LastModifiedBy)
                .Include(record => record.EquipmentImage)
                .OrderByDescending(record => record.CreateTime)
                .ToList();

            return this.ParseToVModel(listModel);
        }

        public EquipmentVModel GetById(long? id)
        {
            var model = _context.Equipment
                .Include(record => record.CreateBy)
                .Include(record => record.LastModifiedBy)
                .Include(record => record.EquipmentImage)
                .Where(record => record.Id == id)
                .FirstOrDefault();

            if (model == null) return null;

            return this.ParseToVModel(new List<Equipment> { model }).FirstOrDefault();
        }

        public EquipmentVModel UpdateEquipment(EquipmentVModel equipment)
        {
            var current = _context.Equipment
                .Include(record => record.CreateBy)
                .Include(record => record.LastModifiedBy)
                .Include(record => record.EquipmentImage)
                .Where(record => record.Id == equipment.Id)
                .FirstOrDefault();

            current.Name = equipment.Name;
            current.Description = equipment.Description;
            current.Quantity = equipment.Quantity;
            current.LastModifiedById = equipment.LastModifiedBy.Id;
            current.LastModified = DateTime.UtcNow.AddHours(7);

            _context.Entry(current).State = EntityState.Modified;
            _context.SaveChanges();

            return equipment;
        }

        public EquipmentVModel InsertEquipment(EquipmentVModel equipment)
        {
            var model = new Equipment()
            {
                Name = equipment.Name,
                Description = equipment.Description,
                Quantity = equipment.Quantity,
                Status = true,
                CreateById = equipment.CreateBy.Id,
                CreateTime = DateTime.UtcNow.AddHours(7)
            };

            _context.Equipment.Add(model);
            _context.SaveChanges();

            return equipment;
        }

        public bool Delete(long id)
        {
            var current = _context.Equipment.Find(id);
            if (current == null) return false;
            try
            {                
                _context.Equipment.Remove(current);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                current.Status = false;
                _context.Entry(current).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return true;
        }

        private IEnumerable<EquipmentVModel> ParseToVModel(IEnumerable<Equipment> listModel)
        {
            List<EquipmentVModel> result = new List<EquipmentVModel>();

            foreach (var model in listModel)
            {
                var vmodel = new EquipmentVModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Quantity = model.Quantity,
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
    }
}
