using AutoMapper;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;
using Jegymester.DataContext.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Jegymester.Services
{
    public interface ICashierService
    {
       
        Task<TicketDto> PurchaseTicketForGuestAsync(CashierTicketPurchaseDto guestDto);
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

        public async Task<TicketDto> PurchaseTicketForGuestAsync(CashierTicketPurchaseDto guestDto)
        {
            var seat = await _context.Seats.FindAsync(guestDto.SeatId);
            if (seat == null || seat.IsOccupied)
                throw new InvalidOperationException("Seat is not available.");
            var screening = await _context.Screenings
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(s => s.Id == guestDto.ScreeningId);
            if (screening == null)
                throw new InvalidOperationException("Invalid screening ID.");

            await _context.SaveChangesAsync();

            var ticket = new Ticket
            {
                ScreeningId = guestDto.ScreeningId,
                SeatId = guestDto.SeatId,
                UserId = guestDto.UserId,
                TicketType = guestDto.TicketType,
                Price = guestDto.Price,
                PurchaseDate = DateTime.UtcNow,
                ScreeningTime = screening.StartTime,
                Title = screening.Movie.Title
            };

            seat.IsOccupied = true;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<bool> ValidateTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                Console.WriteLine("Ticket not found");
                return false;
            }

            if (ticket.Deleted)
            {
                Console.WriteLine("Ticket already used");
                return false;
            }

            var screening = await _context.Screenings.FindAsync(ticket.ScreeningId);
            if (screening == null)
            {
                Console.WriteLine("Screening not found");
                return false;
            }

            if (screening.StartTime <= DateTime.UtcNow)
            {
                Console.WriteLine($"Screening has already started: {screening.StartTime}");
                return false;
            }

            ticket.Deleted = true;
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
