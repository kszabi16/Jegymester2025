using AutoMapper;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jegymester.Services
{
    public interface ICashierService
    {
       
        Task<TicketDto> PurchaseTicketForGuestAsync(int seatId,CashierTicketPurchaseDto guestDto, string ticketType, decimal price);
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

        public async Task<TicketDto> PurchaseTicketForGuestAsync(int seatId, CashierTicketPurchaseDto guestDto,string ticketType,decimal price)
        {
           

            var seat = await _context.Seats.FindAsync(seatId);
            if (seat == null || seat.IsOccupied)
                throw new InvalidOperationException("Seat is not available.");


            var guest = new User
            {
                Email = guestDto.CustomerEmail, 
                PhoneNumber = guestDto.CustomerPhone,
                Name = guestDto.Name
            };
            
            _context.Users.Add(guest);
            await _context.SaveChangesAsync();

            var ticket = new Ticket
            {
               
                SeatId = seatId,
                TicketType = ticketType,
                Price = price,
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
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
