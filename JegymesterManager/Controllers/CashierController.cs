using Jegymester.DataContext.Dtos;
using Jegymester.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jegymester.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Cashier")]
    public class CashierController : ControllerBase
    {
        private readonly ICashierService _cashierService;

        public CashierController(ICashierService cashierService)
        {
            _cashierService = cashierService;
        }

        [HttpPost("PurchaseGuest")]
        public async Task<ActionResult<BookingDto>> CreateCashierBooking([FromBody] CashierBookingDto dto)
        {
            try
            {
                var result = await _cashierService.PurchaseBookingForCustomerAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Validate")]
        public async Task<IActionResult> ValidateTicket([FromQuery] int ticketId)
        {
            var isValid = await _cashierService.ValidateTicketAsync(ticketId);
            if (!isValid)
                return BadRequest("Ticket is not valid or already used.");

            return Ok("Ticket validated successfully.");
        }
    }
}