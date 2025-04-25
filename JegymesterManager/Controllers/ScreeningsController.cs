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
using Microsoft.AspNetCore.Authorization;

namespace JegymesterManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScreeningsController : ControllerBase
    {
        private readonly IScreeningService _screeningService;

        public ScreeningsController(IScreeningService screeningService)
        {
            _screeningService = screeningService;
        }

        
        [HttpGet("GetAllScreenings")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ScreeningDto>>> GetScreenings()
        {
            var screenings = await _screeningService.GetAllAsync();
            return Ok(screenings);
        }

        
        [HttpGet("GetScreeningById/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ScreeningDto>> GetScreening(int id)
        {
            var screening = await _screeningService.GetByIdAsync(id);

            if (screening == null)
            {
                return NotFound();
            }

            return Ok(screening);
        }

        
        [HttpPost("CreateScreening")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ScreeningDto>> CreateScreening([FromBody] ScreeningCreateDto screeningDto)
        {
            var createdScreening = await _screeningService.CreateAsync(screeningDto);
            return CreatedAtAction(nameof(GetScreening), new { id = createdScreening.Id }, createdScreening);
        }


        
        [HttpPut("UpdateScreening/{id}")]
        [Authorize(Roles = "Admin")]
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

        
        [HttpDelete("DeleteScreening/{id}")]
        [Authorize(Roles = "Admin")]
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
