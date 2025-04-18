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
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetAllTickets()
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicketById(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }
        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateAsync(TicketCreateDto dto)
        {
            var ticket = await _ticketService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TicketDto>> UpdateTicket(int id, [FromBody] TicketUpdateDto ticketDto)
        {
            if (ticketDto == null)
                return BadRequest();

            var updatedTicket = await _ticketService.UpdateAsync(id, ticketDto);
            if (updatedTicket == null)
                return NotFound();

            return Ok(updatedTicket);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var result = await _ticketService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
