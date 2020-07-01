using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmManagement_BE.Models;
using FilmManagement_BE.Services;
using FilmManagement_BE.ViewModels;

namespace FilmManagement_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentImagesController : ControllerBase
    {
        private readonly FilmManagerContext _context;
        private readonly EquipmentImageService _service;

        public EquipmentImagesController(FilmManagerContext context)
        {
            _context = context;
            _service = new EquipmentImageService(context);
        }

        [HttpPost("/api/equipments/{equipmentId}/equipment-images")]
        public ActionResult PostEquipmentImage(long equipmentId, List<EquipmentImageVModel> listImg)
        {
            if (_service.Insert(listImg, equipmentId))
            {
                return Created("", listImg);
            }

            return BadRequest("Cannot inset");
        }

        [HttpPut("/api/equipment-images")]
        public ActionResult DeleteEquipmentImage(List<long> listId)
        {
            if (_service.Delete(listId))
            {
                return Ok();
            }
            return BadRequest("Cannot delete");
        }
    }
}
