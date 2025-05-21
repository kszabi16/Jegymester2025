using Jegymester.DataContext.Dtos;
using Jegymester.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ScreeningsController : ControllerBase
{
    private readonly IScreeningService _screeningService;

    public ScreeningsController(IScreeningService screeningService)
    {
        _screeningService = screeningService;
    }

    // 🔓 Elérhető mindenkinek (akár vendégként is)
    [AllowAnonymous]
    [HttpGet("GetAllScreenings")]
    public async Task<ActionResult<IEnumerable<ScreeningDto>>> GetScreenings()
    {
        var screenings = await _screeningService.GetAllAsync();
        return Ok(screenings);
    }

    [AllowAnonymous]
    [HttpGet("GetScreeningById/{id}")]
    public async Task<ActionResult<ScreeningDto>> GetScreening(int id)
    {
        var screening = await _screeningService.GetByIdAsync(id);

        if (screening == null)
            return NotFound();

        return Ok(screening);
    }

    // 🔐 Csak adminnak
    [Authorize(Roles = "Admin")]
    [HttpPost("CreateScreening")]
    public async Task<ActionResult<ScreeningDto>> CreateScreening([FromBody] ScreeningCreateDto screeningDto)
    {
        var createdScreening = await _screeningService.CreateAsync(screeningDto);
        return CreatedAtAction(nameof(GetScreening), new { id = createdScreening.Id }, createdScreening);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateScreening/{id}")]
    public async Task<IActionResult> UpdateScreening(int id, [FromBody] ScreeningUpdateDto screeningDto)
    {
        if (screeningDto == null)
            return BadRequest();

        var updated = await _screeningService.UpdateAsync(id, screeningDto);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteScreening/{id}")]
    public async Task<IActionResult> DeleteScreening(int id)
    {
        var success = await _screeningService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
