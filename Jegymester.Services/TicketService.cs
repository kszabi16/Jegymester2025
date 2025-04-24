using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;
using Jegymester.DataContext.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Jegymester.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task<TicketDto> GetByIdAsync(int id);
        Task<TicketDto> UpdateAsync(int id, TicketUpdateDto dto);
        Task<bool> DeleteAsync(int id);

    }
    public class TicketService : ITicketService
    {
        private readonly JegymesterDbContext _context;
        private readonly IMapper _mapper;

        public TicketService(JegymesterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets = await _context.Tickets.ToListAsync();
            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

        public async Task<TicketDto> GetByIdAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                throw new KeyNotFoundException("Ticket not found.");
            }
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto> UpdateAsync(int id, TicketUpdateDto dto)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                throw new KeyNotFoundException("Ticket not found.");
            }

            ticket.TicketType = dto.TicketType;
            ticket.Price = dto.Price;
            ticket.PurchaseDate = dto.PurchaseDate;

            await _context.SaveChangesAsync();
            return _mapper.Map<TicketDto>(ticket);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.Include(t => t.Screenings).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                throw new KeyNotFoundException("Ticket not found.");
            }
            if (ticket.Screenings.StartTime.AddHours(-4) >= DateTime.UtcNow)
            {
                ticket.Deleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new TimeoutException("Can't delete ticket within 4 hours of screening!");
            }
        } 
    }
}
