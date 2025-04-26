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

        Task<BookingDto> PurchaseBookingForCustomerAsync(CashierBookingDto dto);
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

        public async Task<BookingDto> PurchaseBookingForCustomerAsync(CashierBookingDto dto)
        {
            var screening = await _context.Screenings
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(s => s.Id == dto.ScreeningId);

            if (screening == null || screening.Deleted)
                throw new InvalidOperationException("Screening not found.");

            var screeningEndTime = screening.StartTime.AddMinutes(screening.Movie.Length);
            if (DateTime.Now > screeningEndTime)
                throw new InvalidOperationException("You can't book tickets to this screening anymore.");

            if (dto.SeatIds == null || !dto.SeatIds.Any())
                throw new InvalidOperationException("At least one seat must be selected.");

            var tickets = new List<Ticket>();

            foreach (var seatId in dto.SeatIds)
            {
                var seat = await _context.Seats.FindAsync(seatId);
                if (seat == null)
                    throw new InvalidOperationException($"Seat {seatId} is not available.");

                if (seat.RoomId != screening.RoomId)
                    throw new InvalidOperationException($"Seat {seatId} is not in the same room as the screening.");

                if (seat.IsOccupied)
                    throw new InvalidOperationException($"Seat {seatId} is already occupied.");

                seat.IsOccupied = true;

                tickets.Add(new Ticket
                {
                    ScreeningId = dto.ScreeningId,
                    SeatId = seatId,
                    UserId = dto.UserId,
                    TicketType = dto.TicketType,
                    Price = TicketPricing.GetPrice(dto.TicketType),
                    PurchaseDate = DateTime.Now,
                    ScreeningTime = screening.StartTime,
                });
            }

            var booking = new Booking
            {
                BuyDate = DateTime.Now,
                Quantity = tickets.Count,
                UserId = dto.UserId ?? 0,
                Tickets = tickets,
                TotalPrice = tickets.Sum(t => t.Price)
            };

            
            if (dto.UserId == null)
            {
                Console.WriteLine($"Guest booking by cashier. Email: {dto.Email ?? "N/A"}, Phone: {dto.PhoneNumber ?? "N/A"}");
            }
            else
            {
                Console.WriteLine($"Registered user booking by cashier. UserId: {dto.UserId}");
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return _mapper.Map<BookingDto>(booking);
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

            if (screening.StartTime <= DateTime.Now)
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
