using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmManagement_BE.Models;

namespace FilmManagement_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentImagesController : ControllerBase
    {
        private readonly FilmManagerContext _context;

        public EquipmentImagesController(FilmManagerContext context)
        {
            _context = context;
        }

        // GET: api/EquipmentImages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentImage>>> GetEquipmentImage()
        {
            return await _context.EquipmentImage.ToListAsync();
        }

        // GET: api/EquipmentImages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentImage>> GetEquipmentImage(long id)
        {
            var equipmentImage = await _context.EquipmentImage.FindAsync(id);

            if (equipmentImage == null)
            {
                return NotFound();
            }

            return equipmentImage;
        }

        // PUT: api/EquipmentImages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipmentImage(long id, EquipmentImage equipmentImage)
        {
            if (id != equipmentImage.Id)
            {
                return BadRequest();
            }

            _context.Entry(equipmentImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EquipmentImages
        [HttpPost]
        public async Task<ActionResult<EquipmentImage>> PostEquipmentImage(EquipmentImage equipmentImage)
        {
            _context.EquipmentImage.Add(equipmentImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipmentImage", new { id = equipmentImage.Id }, equipmentImage);
        }

        // DELETE: api/EquipmentImages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EquipmentImage>> DeleteEquipmentImage(long id)
        {
            var equipmentImage = await _context.EquipmentImage.FindAsync(id);
            if (equipmentImage == null)
            {
                return NotFound();
            }

            _context.EquipmentImage.Remove(equipmentImage);
            await _context.SaveChangesAsync();

            return equipmentImage;
        }

        private bool EquipmentImageExists(long id)
        {
            return _context.EquipmentImage.Any(e => e.Id == id);
        }
    }
}
