using Jegymester.DataContext.Dtos;
using Jegymester.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    // 🔓 Minden bejelentkezett felhasználónak (Admin + Customer)
    [Authorize]
    [HttpGet("GetAllRooms")]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _roomService.GetRoomsAsync();
        return Ok(rooms);
    }

    [Authorize]
    [HttpGet("GetRoomById/{roomId}")]
    public async Task<IActionResult> GetRoomById(int roomId)
    {
        try
        {
            var room = await _roomService.GetRoomByIdAsync(roomId);
            return Ok(room);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // 🔐 Csak Admin
    [Authorize(Roles = "Admin")]
    [HttpPost("CreateRoom")]
    public async Task<IActionResult> CreateRoom([FromBody] RoomCreateDto createDto)
    {
        var room = await _roomService.CreateRoomAsync(createDto);
        return CreatedAtAction(nameof(GetRoomById), new { roomId = room.Id }, room);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateRoom/{roomId}")]
    public async Task<IActionResult> UpdateRoom(int roomId, [FromBody] RoomUpdateDto updateDto)
    {
        try
        {
            var room = await _roomService.UpdateRoomAsync(roomId, updateDto);
            return Ok(room);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteRoom/{roomId}")]
    public async Task<IActionResult> DeleteRoom(int roomId)
    {
        try
        {
            await _roomService.DeleteRoomAsync(roomId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
