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

            
            var defaultRole = await GetDefaultCustomerRoleAsync();
            if (defaultRole == null)
                throw new InvalidOperationException("Default customer role not found.");

            user.Roles = new List<Role> { defaultRole };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> RegisterWithRolesAsync(RegisterWithRolesDto userDto)
        {
            
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new InvalidOperationException("Email already exists.");

            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            List<Role> rolesToAssign = new List<Role>();

            if (userDto.RoleIds != null && userDto.RoleIds.Any())
            {
                Console.WriteLine($"RoleIds: {string.Join(", ", userDto.RoleIds)}"); 
                rolesToAssign = await _context.Roles
                    .Where(r => userDto.RoleIds.Contains(r.Id))
                    .ToListAsync();

                if (rolesToAssign.Count != userDto.RoleIds.Count)
                {
                    var foundRoleIds = rolesToAssign.Select(r => r.Id).ToList();
                    var missingRoleIds = userDto.RoleIds.Except(foundRoleIds).ToList();
                    throw new KeyNotFoundException($"One or more roles not found. Missing RoleIds: {string.Join(", ", missingRoleIds)}");
                }
            }
            else
            {
                throw new ArgumentException("RoleIds must be provided for this registration type.");
            }

            user.Roles = rolesToAssign;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            return await GenerateToken(user);
        }

        private async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

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
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture))
            };

            if (user.Roles != null && user.Roles.Any())
            {
                claims.AddRange(user.Roles.Select(role => new Claim("roleIds", Convert.ToString(role.Id))));
                claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
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
                .Include(u => u.Roles)
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

        private async Task<Role> GetDefaultCustomerRoleAsync()
        {
            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");
            if (customerRole == null)
            {
                customerRole = new Role { Name = "Customer" };
                await _context.Roles.AddAsync(customerRole);
                await _context.SaveChangesAsync();
            }
            return customerRole;
        }
    }
}