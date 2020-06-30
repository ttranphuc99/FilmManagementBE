using FilmManagement_BE.Models;
using FilmManagement_BE.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmManagement_BE.Services
{
    public class EquipmentImageService 
    {
        private readonly FilmManagerContext _context;

        public EquipmentImageService(FilmManagerContext context)
        {
            _context = context;
        }

        /*public IEnumerable<EquipmentImageVModel> GetList()
        {
            var listModel = _context.EquipmentImage.Include(record => record.Equipment).OrderByDescending(record => record.Id).ToList();

            return this.ParseToVModel(listModel);
        }

        public EquipmentImageVModel GetById(long id)
        {
            var model = _context.EquipmentImage.Find(id);
            if (model == null) return null;

            return this.ParseToVModel(new List<EquipmentImage>() { model }).FirstOrDefault();
        }*/

        public bool Insert(List<EquipmentImageVModel> listImg, long equipmentId)
        {
            foreach (var img in listImg)
            {
                var model = new EquipmentImage()
                {
                    Url = img.Url,
                    EquipmentId = equipmentId
                };
                _context.EquipmentImage.Add(model);
            }
            _context.SaveChanges();
            return true;
        }

        public bool Delete(List<long> listId)
        {
            foreach (var id in listId)
            {
                _context.Remove(_context.EquipmentImage.Find(id));
            }
            _context.SaveChanges();
            return true;
        }

        private IEnumerable<EquipmentImageVModel> ParseToVModel(IEnumerable<EquipmentImage> listModel)
        {
            var result = new List<EquipmentImageVModel>();

            foreach (var model in listModel)
            {
                var vmodel = new EquipmentImageVModel()
                {
                    Id = model.Id,
                    Url = model.Url,
                    Equipment = model.Equipment != null ? new EquipmentVModel()
                    {
                        Id = model.Equipment.Id
                    } : null
                };

                result.Add(vmodel);
            }
            return result;
        }
    }
}
