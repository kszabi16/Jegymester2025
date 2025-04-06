using Jegymester.DataContext.Dtos;
using Jegymester.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jegymester.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashierController : ControllerBase
    {
        private readonly ICashierService _cashierService;

        public CashierController(ICashierService cashierService)
        {
            _cashierService = cashierService;
        }

       

        [HttpPost("PurchaseGuest")]
        public async Task<IActionResult> PurchaseTicketForGuest( int seatId, CashierTicketPurchaseDto guestDto, string ticketType, decimal price)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var ticket = await _cashierService.PurchaseTicketForGuestAsync(seatId,guestDto,ticketType,price);
                return Ok(ticket);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
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