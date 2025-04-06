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

            List<Role> rolesToAssign = new List<Role>();

            // Ellenőrizzük, hogy van-e megadott RoleId és tartalmaz-e valid szerepköröket
            if (userDto.RoleIds != null && userDto.RoleIds.Any())
            {
                foreach (var roleId in userDto.RoleIds)
                {
                    var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                    if (existingRole != null)
                    {
                        rolesToAssign.Add(existingRole);  // Ha valid szerepkör, hozzáadjuk
                    }
                    else
                    {
                        // Ha valamelyik szerepkör nem létezik, dobunk egy hibát
                        throw new KeyNotFoundException($"Role with ID {roleId} not found.");
                    }
                }
            }

            // Ha nem adtak meg szerepköröket, akkor alapértelmezett szerepkört rendelünk hozzá
            if (!rolesToAssign.Any())
            {
                rolesToAssign.Add(await GetDefaultCustomerRoleAsync());
            }

            // Hozzárendeljük a szerepköröket a felhasználóhoz
            user.Roles = rolesToAssign;

            // Mivel új felhasználóról van szó, használjuk az AddAsync-t
            await _context.Users.AddAsync(user);

            // Frissítjük a RoleId mezőt is, ha szükséges
            user.RoleId = rolesToAssign.First().Id;

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

            //return _jwtService.GenerateToken(user);
            return user.Name;
        }

        public async Task<UserDto> UpdateProfileAsync(int userId, UserUpdateDto userDto)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _mapper.Map(userDto, user);

            // Ha új szerepkörök vannak, akkor frissítjük a szerepköröket
            if (userDto.RoleIds != null && userDto.RoleIds.Any())
            {
                user.Roles.Clear();  // Kiürítjük a régi szerepköröket

                foreach (var roleId in userDto.RoleIds)
                {
                    var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                    if (existingRole != null)
                    {
                        user.Roles.Add(existingRole);  // Hozzáadjuk a valid szerepköröket
                    }
                    else
                    {
                        throw new KeyNotFoundException($"Role with ID {roleId} not found.");
                    }
                }
            }

            _context.Users.Update(user);
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
