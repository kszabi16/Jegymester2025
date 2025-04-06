using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Entities;

namespace JegymesterManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningsController : ControllerBase
    {
        private readonly JegymesterDbContext _context;

        public ScreeningsController(JegymesterDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("GetAllScreening")]
        public async Task<ActionResult<IEnumerable<Screening>>> GetScreenings()
        {
            return await _context.Screenings.ToListAsync();
        }

       
        [HttpGet("GetById")]
        public async Task<ActionResult<Screening>> GetScreening(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);

            if (screening == null)
            {
                return NotFound();
            }

            return screening;
        }

        
        [HttpPut("CreateScreening")]
        public async Task<IActionResult> PutScreening(int id, Screening screening)
        {
            if (id != screening.Id)
            {
                return BadRequest();
            }

            _context.Entry(screening).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScreeningExists(id))
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

        
        [HttpPost("UpdateScreening")]
        public async Task<ActionResult<Screening>> PostScreening(Screening screening)
        {
            _context.Screenings.Add(screening);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScreening", new { id = screening.Id }, screening);
        }

        
        [HttpDelete("DeleteScreening")]
        public async Task<IActionResult> DeleteScreening(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                return NotFound();
            }

            _context.Screenings.Remove(screening);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScreeningExists(int id)
        {
            return _context.Screenings.Any(e => e.Id == id);
        }
    }
}
