using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Jegymester.DataContext;
using Jegymester.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Entities;
using System.Globalization;

namespace Jegymester.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterCustomerAsync(RegisterUserDto userDto);
        Task<UserDto> RegisterWithRolesAsync(RegisterWithRolesDto userDto);
        Task<string> LoginAsync(UserLoginDto loginDto);
        Task<UserDto> UpdateUserAsync(int userId, UserUpdateDto updateDto);
        Task<List<TicketDto>> GetUserTicketsAsync(int userId);
        Task DeleteUserAsync(int userId);

    }

    public class UserService : IUserService
    {
        private readonly JegymesterDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(JegymesterDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<UserDto> RegisterCustomerAsync(RegisterUserDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new InvalidOperationException("Email already exists.");

            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.Roles = new List<RoleType> { RoleType.Customer };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> RegisterWithRolesAsync(RegisterWithRolesDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new InvalidOperationException("Email already exists.");

            if (userDto.Roles == null)
                throw new ArgumentException("Role must be provided for this registration type.");

            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            user.Roles = userDto.Roles; // << Enum érték

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            return await GenerateToken(user);
        }

        private async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var id = await GetClaimsIdentity(user);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], id.Claims, expires: expires, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
        };
            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }

            return new ClaimsIdentity(claims, "Token");
        }

        public async Task<List<TicketDto>> GetUserTicketsAsync(int userId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Screenings)
                .ThenInclude(s => s.Movie)
                .Where(t => t.UserId == userId && !t.Deleted)
                .ToListAsync();
            return _mapper.Map<List<TicketDto>>(tickets);
        }

        public async Task<UserDto> UpdateUserAsync(int userId, UserUpdateDto updateDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && !u.Deleted);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (user.Email != updateDto.Email && await _context.Users.AnyAsync(u => u.Email == updateDto.Email))
                throw new InvalidOperationException("Email already exists.");

            _mapper.Map(updateDto, user);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            if (user.Deleted)
            {
                throw new InvalidOperationException($"User with ID {userId} is already deleted.");
            }

            user.Deleted = true;
            await _context.SaveChangesAsync();
        }
    }
}