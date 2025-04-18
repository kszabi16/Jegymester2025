using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Entities;
using Jegymester.Services;
using Jegymester.DataContext.Dtos;

namespace JegymesterManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningsController : ControllerBase
    {
        private readonly IScreeningService _screeningService;

        public ScreeningsController(IScreeningService screeningService)
        {
            _screeningService = screeningService;
        }

        // GET api/screenings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScreeningDto>>> GetScreenings()
        {
            var screenings = await _screeningService.GetAllAsync();
            return Ok(screenings);
        }

        // GET api/screenings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ScreeningDto>> GetScreening(int id)
        {
            var screening = await _screeningService.GetByIdAsync(id);

            if (screening == null)
            {
                return NotFound();
            }

            return Ok(screening);
        }

        // POST api/screenings
        [HttpPost]
        public async Task<ActionResult<ScreeningDto>> CreateScreening([FromBody] ScreeningCreateDto screeningDto)
        {
            var createdScreening = await _screeningService.CreateAsync(screeningDto);
            return CreatedAtAction(nameof(GetScreening), new { id = createdScreening.Id }, createdScreening);
        }


        // PUT api/screenings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScreening(int id, [FromBody] ScreeningUpdateDto screeningDto)
        {
            if (screeningDto == null)
            {
                return BadRequest();
            }

            var updated = await _screeningService.UpdateAsync(id, screeningDto);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        // DELETE api/screenings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreening(int id)
        {
            var success = await _screeningService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

}
