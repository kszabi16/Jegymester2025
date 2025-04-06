using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;
using Microsoft.EntityFrameworkCore;



namespace Jegymester.Services
    {
        public interface IUserService
        {
            Task<UserDto> RegisterAsync(RegisterUserDto userDto);
            Task<UserDto> LoginAsync(UserLoginDto loginDto);
            Task<IEnumerable<MovieDto>> GetAvailableMoviesAsync();
            Task<IEnumerable<ScreeningDto>> GetAvailableScreeningsAsync();
            Task<TicketDto> PurchaseTicketAsync(int screeningId, int seatId, string userId);
            Task<bool> CancelTicketAsync(int ticketId, string userId);
            Task<UserDto> UpdateUserDataAsync(string userId, UserUpdateDto updateDto);
            Task<IEnumerable<TicketDto>> GetUserTicketsAsync(string userId);
        }

    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly JegymesterDbContext _context;

        public UserService(IMapper mapper, JegymesterDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<bool> CancelTicketAsync(int ticketId, string userId)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == ticketId && t.UserId.ToString() == userId);

            if (ticket == null) return false;

            ticket.Deleted = true;
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<IEnumerable<MovieDto>> GetAvailableMoviesAsync()
        {
            return await _context.Movies
                .Where(m => !m.Deleted)
                .Select(m => _mapper.Map<MovieDto>(m))
                .ToListAsync();
        }


        public async Task<IEnumerable<ScreeningDto>> GetAvailableScreeningsAsync()
        {
            return await _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .Include(s => s.Tickets)
                .Where(s => s.StartTime > DateTime.Now)
                .Select(s => new ScreeningDto
                {
                    Id = s.Id,
                    Deleted = false,
                    MovieId = s.MovieId,
                    Title = s.Name,
                    StartTime = s.StartTime,
                    EndTime = s.StartTime.AddMinutes(s.Movie.Length),
                    RoomId = s.RoomId,
                    AvailableSeats = s.Room.Capacity - s.Tickets.Count
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<TicketDto>> GetUserTicketsAsync(string userId)
        {
            int uid = int.Parse(userId);

            return await _context.Tickets
                .Include(t => t.Screenings)
                .Where(t => t.UserId == uid && !t.Deleted)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    Deleted = false,
                    ScreeningId = t.ScreeningId,
                    ScreeningTime = t.Screenings.StartTime,
                    Title = t.Screenings.Name,
                    TicketType = t.TicketType,
                    Price = t.Price,
                    PurchaseDate = DateTime.Now
                })
                .ToListAsync();
        }


        public async Task<UserDto> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Password == loginDto.Password);

            if (user == null || user.Deleted) return null;

            return _mapper.Map<UserDto>(user);
        }


        public async Task<TicketDto> PurchaseTicketAsync(int screeningId, int seatId, string userId)
        {
            var screening = await _context.Screenings.FindAsync(screeningId);
            if (screening == null) return null;

            var seat = await _context.Seats.FindAsync(seatId);
            if (seat == null) return null;

            bool exists = await _context.Tickets.AnyAsync(t => t.ScreeningId == screeningId && t.SeatId == seatId);
            if (exists) return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) return null;

            var ticket = new Ticket
            {
                ScreeningId = screeningId,
                SeatId = seatId,
                RoomId = screening.RoomId,
                UserId = user.Id,
                TicketType = "User",
                Price = 2000
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return new TicketDto
            {
                Id = ticket.Id,
                Deleted = false,
                ScreeningId = ticket.ScreeningId,
                ScreeningTime = screening.StartTime,
                Title = screening.Name,
                TicketType = ticket.TicketType,
                Price = ticket.Price,
                PurchaseDate = DateTime.Now
            };
        }


        public async Task<UserDto> RegisterAsync(RegisterUserDto userDto)
        {
            
            var exists = await _context.Users.AnyAsync(u => u.Email == userDto.Email);
            if (exists) return null;

            var user = new User
            {
                Name = userDto.Username,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Password = userDto.Password, 
                RoleId = userDto.RoleIds.FirstOrDefault() 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }


        public async Task<UserDto> UpdateUserDataAsync(string userId, UserUpdateDto updateDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null || user.Deleted) return null;

            user.Name = updateDto.Username;
            user.Email = updateDto.Email;
            user.PhoneNumber = updateDto.PhoneNumber;
            user.RoleId = updateDto.RoleIds.FirstOrDefault();

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

    }
}
