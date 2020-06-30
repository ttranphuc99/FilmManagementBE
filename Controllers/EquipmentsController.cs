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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FilmManagement_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EquipmentsController : ControllerBase
    {
        private readonly FilmManagerContext _context;
        private readonly EquipmentService _service;

        public EquipmentsController(FilmManagerContext context)
        {
            _context = context;
            _service = new EquipmentService(context);
        }

        // GET: api/Equipments
        [HttpGet("/api/equipments")]
        public ActionResult GetEquipment()
        {
            return Ok(_service.GetListEquipments());
        }

        [HttpGet("/api/equipments/{id}")]
        public ActionResult GetEquipment(long id)
        {
            var equipment = _service.GetById(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return Ok(equipment);
        }

        [HttpPut("/api/equipments/{id}")]
        public ActionResult PutEquipment(long id, EquipmentVModel equipment)
        {
            if (id != equipment.Id) equipment.Id = id;

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId);

            equipment.LastModifiedBy = new AccountVModel() { Id = userId };

            _service.UpdateEquipment(equipment);

            return Ok(equipment);
        }

        [HttpPost("/api/equipments")]
        public ActionResult PostEquipment(EquipmentVModel equipment)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId);

            equipment.CreateBy = new AccountVModel() { Id = userId };

            _service.InsertEquipment(equipment);

            return Created("", equipment);
        }

        [HttpDelete("/api/equipments/{id}")]
        public ActionResult DeleteEquipment(long id)
        {
            if (_service.Delete(id))
            {
                return Ok();
            }

            return BadRequest("Cannot delete");
        }
    }
}
