﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmManagement_BE.Models;
using FilmManagement_BE.ViewModels;
using System.Security.Claims;
using FilmManagement_BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FilmManagement_BE.Constants;

namespace FilmManagement_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScenariosController : ControllerBase
    {
        private readonly FilmManagerContext _context;
        private readonly ScenarioService _service;

        public ScenariosController(FilmManagerContext context)
        {
            _context = context;
            _service = new ScenarioService(context);
        }

        [HttpGet("/api/scenarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        public ActionResult<IEnumerable<Scenario>> GetScenario()
        {
            return Ok(_service.GetListScenario());
        }

        [HttpGet("/api/scenarios/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<Scenario> GetScenario(long id)
        {
            var scenario = _service.GetById(id);

            if (scenario != null) return Ok(scenario);

            return NotFound();
        }

        // PUT: api/Scenarios/5
        [HttpPut("/api/scenarios/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        public IActionResult PutScenario(long id, ScenarioVModel scenario)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);

            scenario.LastModifiedBy = new AccountVModel() { Id = userId };

            if (id != scenario.Id)
            {
                scenario.Id = id;
            }

            var result = _service.UpdateScenario(scenario);
            
            if (result != null) return Ok();

            return BadRequest("Cannot find object with ID: " + id);
        }

        [HttpPost("/api/scenarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        public ActionResult PostScenario(ScenarioVModel scenario)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);

            scenario.CreatedBy = new AccountVModel() { Id = userId };

            return Created("", _service.AddScenario(scenario));
        }

        // DELETE: api/Scenarios/5
        [HttpDelete("/api/scenarios/{id}")]
        public async Task<ActionResult<Scenario>> DeleteScenario(long id)
        {
            var scenario = await _context.Scenario.FindAsync(id);
            if (scenario == null)
            {
                return NotFound();
            }

            if (_service.DeleteScenario(scenario))
            {
                return Ok();
            }

            return BadRequest("Cannot delete scenario");
        }

        [HttpGet("/api/scenarios/{scenId}/actors")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult GetListActors(long scenId)
        {
            return Ok(_service.GetListActors(scenId));
        }

        [HttpGet("/api/scenarios/{scenId}/equipments")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult GetListEquipments(long scenId)
        {
            return Ok(_service.GetListEquipment(scenId));
        }

        [HttpPost("/api/scenarios/{scenId}/actors/{actorId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        public ActionResult AddActorToScen(long scenId, int actorId, [FromBody] ScenarioAccountVModel scenAcc)
        {
            scenAcc.Account = new AccountVModel() { Id = actorId };
            scenAcc.Scenario = new ScenarioVModel() { Id = scenId };

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);

            scenAcc.CreateBy = new AccountVModel() { Id = userId };

            if (_service.AddActorToScenario(scenAcc))
            {
                return Created("", scenAcc);
            }

            return BadRequest("Actor is already in");
        }

        [HttpPut("/api/scenarios/{scenId}/actors/{actorId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        public ActionResult EditCharacter(long scenId, int actorId, [FromBody] ScenarioAccountVModel scenAcc)
        {
            scenAcc.Account = new AccountVModel() { Id = actorId };
            scenAcc.Scenario = new ScenarioVModel() { Id = scenId };

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);

            scenAcc.LastModifiedBy = new AccountVModel() { Id = userId };

            if (_service.ChangeCharacterForActor(scenAcc))
            {
                return Ok(scenAcc);
            }

            return BadRequest("Not find actor and scenario");
        }

        [HttpDelete("/api/scenarios/{scenId}/actors/{actorId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        public ActionResult DeleteCharacter(long scenId, int actorId)
        {
            var scenAcc = new ScenarioAccountVModel();
            scenAcc.Account = new AccountVModel() { Id = actorId };
            scenAcc.Scenario = new ScenarioVModel() { Id = scenId };

            if (_service.RemoveActorFromScenario(scenAcc))
            {
                return Ok(scenAcc);
            }

            return BadRequest("Cannot remove actor");
        }

        [HttpGet("/api/equipments-available/{equipId}/scenarios/{scenId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        public ActionResult GetAvaibleQuantityEquipmentForScenario(long? equipId, long? scenId)
        {
            return Ok(_service.GetEquipmentAvailableForScence(equipId, scenId));
        }

        [HttpPost("/api/scenarios/{scenId}/equipments/{equipId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        public ActionResult AddEquipmentToScen(long? scenId, long? equipId, ScenarioEquipmentVModel scenEqui)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);

            scenEqui.Equipment = new EquipmentVModel() { Id = equipId };
            scenEqui.Scenario = new ScenarioVModel() { Id = scenId };
            scenEqui.CreatedBy = new AccountVModel() { Id = userId };

            return Ok(_service.AddEquipmentToScen(scenEqui));
        }

        [HttpPut("/api/scenarios/{scenId}/equipments/{equipId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        public ActionResult UpdateEquipmentToScen(long? scenId, long? equipId, ScenarioEquipmentVModel scenEqui)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId;
            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);

            scenEqui.Equipment = new EquipmentVModel() { Id = equipId };
            scenEqui.Scenario = new ScenarioVModel() { Id = scenId };
            scenEqui.LastModifiedBy = new AccountVModel() { Id = userId };

            var result = _service.UpdateEquipmentInScence(scenEqui);

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("/api/scenarios/{scenId}/equipments/{equipId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.DIRECTOR_STR)]
        public ActionResult DeleteEquipmentToScen(long? scenId, long? equipId)
        {
            var result = _service.DeleteEquipmentInScence(equipId, scenId);

            if (!result) return NotFound();

            return Ok();
        }
    }
}
