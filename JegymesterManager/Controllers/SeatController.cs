using Jegymester.DataContext.Dtos;
using Jegymester.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JegymesterManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet("GetSeatsByRoomId/{roomId}")]
        public async Task<IActionResult> GetSeatsByRoom(int roomId)
        {
            try
            {
                var seats = await _seatService.GetSeatsByRoomAsync(roomId);
                return Ok(seats);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetSeatById/{seatId}")]
        public async Task<IActionResult> GetSeatById(int seatId)
        {
            try
            {
                var seat = await _seatService.GetSeatByIdAsync(seatId);
                return Ok(seat);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("CreateSeat")]
        public async Task<IActionResult> CreateSeat([FromBody] SeatCreateDto createDto)
        {
            try
            {
                var seat = await _seatService.CreateSeatAsync(createDto);
                return CreatedAtAction(nameof(GetSeatById), new { seatId = seat.Id }, seat);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("UpdateSeat/{seatId}")]
        public async Task<IActionResult> UpdateSeat(int seatId, [FromBody] SeatUpdateDto updateDto)
        {
            try
            {
                var seat = await _seatService.UpdateSeatAsync(seatId, updateDto);
                return Ok(seat);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteSeat/{seatId}")]
        public async Task<IActionResult> DeleteSeat(int seatId)
        {
            try
            {
                await _seatService.DeleteSeatAsync(seatId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
