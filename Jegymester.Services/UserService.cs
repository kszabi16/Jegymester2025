using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        Task<string> LoginAsync(UserLoginDto userDto);
        Task<UserDto> UpdateProfileAsync(int userId, UserUpdateDto userDto);
        Task<IList<RoleDto>> GetRolesAsync();
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

        public async Task<UserDto> RegisterAsync(RegisterUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Name = userDto.Name;
                
            List<Role> rolesToAssign = new List<Role>();

            if (userDto.RoleIds != null && userDto.RoleIds.Any())
            {
                rolesToAssign = await _context.Roles
                    .Where(r => userDto.RoleIds.Contains(r.Id))
                    .ToListAsync();

                if (rolesToAssign.Count != userDto.RoleIds.Count)
                    throw new KeyNotFoundException("One or more roles not found.");
            }
            else
            {
                rolesToAssign.Add(await GetDefaultCustomerRoleAsync());
            }

            user.Roles = rolesToAssign;

            await _context.Users.AddAsync(user);
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

        public async Task<string> LoginAsync(UserLoginDto userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userDto.Email);
            if (user == null /*|| !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash)*/)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }
            return user.Name;
        }

        public async Task<UserDto> UpdateProfileAsync(int userId, UserUpdateDto userDto)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;

            if (userDto.RoleIds != null && userDto.RoleIds.Any())
            {
                var roles = await _context.Roles
                    .Where(r => userDto.RoleIds.Contains(r.Id))
                    .ToListAsync();

                if (roles.Count != userDto.RoleIds.Count)
                    throw new KeyNotFoundException("One or more roles not found.");

                user.Roles = roles;
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }





        public async Task<IList<RoleDto>> GetRolesAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return _mapper.Map<IList<RoleDto>>(roles);
        }

    }
}
