using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Jegymester.DataContext.Context;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.Services
{
    public interface IBookingService
    {
        Task<BookingDto> UserCreateBookingAsync(CreateUserBookingDto dto);
        Task<BookingDto> GuestCreateBookingAsync(CreateGuestBookingDto dto);
    }
    public class BookingService : IBookingService
    {
        private readonly JegymesterDbContext _context;
        private readonly IMapper _mapper;
        public BookingService(JegymesterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookingDto> UserCreateBookingAsync(CreateUserBookingDto dto)
        {
            var screening = await _context.Screenings.FindAsync(dto.ScreeningId);
            if (screening == null)
                throw new Exception("Screening not Found");



            var tickets = new List<Ticket>();
            foreach (var seatId in dto.SeatId)
            {
                var seat = await _context.Seats.FindAsync(seatId);

                if (seat == null)
                    throw new Exception($"Seat doesn't exist: {seatId}");

                if (seat.RoomId != screening.RoomId)
                    throw new InvalidOperationException($"Seat {seatId} is not in the same room as the screening.");

                if (seat.IsOccupied)
                    throw new InvalidOperationException($"Seat {seatId} is already occupied.");

                seat.IsOccupied = true;

                tickets.Add(new Ticket
                {
                    Price = dto.Price,
                    TicketType = dto.TicketType,
                    ScreeningId = dto.ScreeningId,
                    SeatId = seatId,
                    PurchaseDate = DateTime.Now,
                    ScreeningTime = screening.StartTime,
                    UserId = dto.UserId
                });
            }

            var booking = new Booking
            {
                UserId = dto.UserId,
                BuyDate = DateTime.Now,
                Quantity = tickets.Count,
                Tickets = tickets
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return _mapper.Map<BookingDto>(booking);
        }
        public async Task<BookingDto> GuestCreateBookingAsync(CreateGuestBookingDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) && string.IsNullOrEmpty(dto.PhoneNumber))
                throw new Exception("Email or Phone is required.");

            var screening = await _context.Screenings.FindAsync(dto.ScreeningId);
            if (screening == null)
                throw new Exception("Screening not Found.");

            var tickets = new List<Ticket>();

            foreach (var seatId in dto.SeatIds)
            {
                var seat = await _context.Seats.FindAsync(seatId);

                if (seat == null)
                    throw new Exception($"Seat doesn't exist: {seatId}");

                if (seat.RoomId != screening.RoomId)
                    throw new InvalidOperationException($"Seat {seatId} is not in the same room as the screening.");

                if (seat.IsOccupied)
                    throw new InvalidOperationException($"Seat {seatId} is already occupied.");

                seat.IsOccupied = true;

                tickets.Add(new Ticket
                {
                    Price = dto.Price,
                    TicketType = dto.TicketType,
                    ScreeningId = dto.ScreeningId,
                    SeatId = seatId,
                    PurchaseDate = DateTime.Now,
                    ScreeningTime = screening.StartTime,
                });
            }

            var booking = new Booking
            {
                BuyDate = DateTime.Now,
                Quantity = tickets.Count,
                Tickets = tickets,
               
            };

            Console.WriteLine($"Guest purchase - Email: {dto.Email}, Phone: {dto.PhoneNumber}");

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return _mapper.Map<BookingDto>(booking);
        }

    }
}
