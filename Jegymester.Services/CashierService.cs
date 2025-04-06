using AutoMapper;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jegymester.Services
{
    public interface ICashierService
    {
        Task<TicketDto> PurchaseTicketForUserAsync(int screeningId, int seatId, int userId);
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

        public async Task<TicketDto> PurchaseTicketForUserAsync(int screeningId, int seatId, int userId)
        {
            
            var screening = await _context.Screenings.FindAsync(screeningId);
            if (screening == null)
                throw new ArgumentException("Screening not found.");

            var seat = await _context.Seats.FindAsync(seatId);
            if (seat == null || seat.IsOccupied)
                throw new InvalidOperationException("Seat is not available.");

            var ticket = new Ticket
            {
                ScreeningId = screeningId,
                SeatId = seatId,
                UserId = userId,
                TicketType = "User",
                Price = 2500,
            };

            seat.IsOccupied = true;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto> PurchaseTicketForGuestAsync(int screeningId, int seatId, GuestTicketPurchaseDto guestDto)
        {
            var screening = await _context.Screenings.FindAsync(screeningId);
            if (screening == null)
                throw new ArgumentException("Screening not found.");

            var seat = await _context.Seats.FindAsync(seatId);
            if (seat == null || seat.IsOccupied)
                throw new InvalidOperationException("Seat is not available.");



            var guest = new User
            {
                Email = guestDto.CustomerEmail, 
                PhoneNumber = guestDto.CustomerPhone,              
            };
            
            _context.Users.Add(guest);
            await _context.SaveChangesAsync();

            var ticket = new Ticket
            {
                ScreeningId = screeningId,
                SeatId = seatId,
                TicketType = "Guest",
            };

            seat.IsOccupied = true;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<bool> ValidateTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null || ticket.Deleted)
                return false;

            var screening = await _context.Screenings.FindAsync(ticket.ScreeningId);
            if (screening == null || screening.StartTime < DateTime.UtcNow)
                return false;

            
            ticket.Deleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
