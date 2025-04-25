using Jegymester.DataContext.Dtos;
using Jegymester.Services;
using Microsoft.AspNetCore.Mvc;

namespace JegymesterManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> UserCreateBooking([FromBody] CreateUserBookingDto dto)
        {
            try
            {
                var result = await _bookingService.UserCreateBookingAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("guest")]
        public async Task<ActionResult<BookingDto>> GuestCreateBooking([FromBody] CreateGuestBookingDto dto)
        {
            try
            {
                var result = await _bookingService.GuestCreateBookingAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
