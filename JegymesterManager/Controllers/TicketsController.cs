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
        public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] TicketDto ticketDto)
        {
            var created = await _ticketService.CreateAsync(ticketDto);
            return CreatedAtAction(nameof(GetTicketById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] TicketDto ticketDto)
        {
            var result = await _ticketService.UpdateAsync(id, ticketDto);
            if (result == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var success = await _ticketService.DeleteAsync(id);
            if (!success)
                return NotFound();

        
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
