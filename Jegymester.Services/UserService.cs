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
        public Task<bool> CancelTicketAsync(int ticketId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MovieDto>> GetAvailableMoviesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ScreeningDto>> GetAvailableScreeningsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketDto>> GetUserTicketsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> LoginAsync(UserLoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<TicketDto> PurchaseTicketAsync(int screeningId, int seatId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> RegisterAsync(RegisterUserDto userDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> UpdateUserDataAsync(string userId, UserUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
