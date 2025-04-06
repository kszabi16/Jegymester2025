using AutoMapper;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jegymester.Services
{
    public interface ICashierService
    {
        Task<TicketDto> PurchaseTicketForUserAsync(int screeningId, int seatId, string userId);
        Task<TicketDto> PurchaseTicketForGuestAsync(int screeningId, int seatId, GuestTicketPurchaseDto guestDto);
        Task<bool> ValidateTicketAsync(int ticketId);
    }

    public class CashierService : ICashierService
    {
        private readonly IMapper _mapper;
        private readonly JegymesterDbContext _context;

        public CashierService(IMapper mapper, JegymesterDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<TicketDto> PurchaseTicketForGuestAsync(int screeningId, int seatId, GuestTicketPurchaseDto guestDto)
        {
            var screening = await _context.Screenings.FindAsync(screeningId);
            if (screening == null)
                return null;

            
            bool seatTaken = await _context.Tickets
                .AnyAsync(t => t.ScreeningId == screeningId && t.SeatId == seatId);
            if (seatTaken)
                return null;

            
            var ticket = new Ticket
            {
                ScreeningId = screeningId,
                SeatId = seatId,
                TicketType = "Guest",
                Price = 3000,
                RoomId = screening.RoomId,
                
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }


        public async Task<TicketDto> PurchaseTicketForUserAsync(int screeningId, int seatId, string userId)
        {
            
            var screening = await _context.Screenings.FindAsync(screeningId);
            if (screening == null)
                return null;

            
            if (!int.TryParse(userId, out int parsedUserId))
                return null;

            var user = await _context.Users.FindAsync(parsedUserId);
            if (user == null)
                return null;

            
            bool seatTaken = await _context.Tickets
                .AnyAsync(t => t.ScreeningId == screeningId && t.SeatId == seatId);
            if (seatTaken)
                return null;

            
            var ticket = new Ticket
            {
                ScreeningId = screeningId,
                SeatId = seatId,
                UserId = parsedUserId,
                TicketType = "Standard",
                Price = 3000, 
                RoomId = screening.RoomId
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }


        public async Task<bool> ValidateTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
                return false;

            

            
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
