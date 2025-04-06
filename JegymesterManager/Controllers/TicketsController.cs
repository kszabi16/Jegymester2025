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
            _ticketService=ticketService;
        }

        
        [HttpGet("GetAllTicket")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets); 
        }

       
        [HttpGet("GetById")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        
        [HttpPut("CreateTicket")]
        public async Task<ActionResult<Ticket>> CreateTicket(TicketDto ticketDto)
        {
            var createdTicket = await _ticketService.CreateAsync(ticketDto);
            return CreatedAtAction(nameof(GetTicket), new { id = createdTicket.Id }, createdTicket);
        }

        
        [HttpPost("UpdateTicket")]
        public async Task<ActionResult<Ticket>> PostTicket(int id,TicketDto ticket)
        {
            var updatedTicket = await _ticketService.UpdateAsync(id, ticket);
            if (updatedTicket == null)
            {
                return NotFound();
            }

            return NoContent();
        }

       
        [HttpDelete("DeleteTicket")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var movie = await _ticketService.DeleteAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            await _ticketService.DeleteAsync(id);
            return NoContent();

        }


    }
}
