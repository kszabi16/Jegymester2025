using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Entities;
using Jegymester.Services;
using Jegymester.DataContext.Dtos;

namespace JegymesterManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("RegisterCustomer")]
        public async Task<IActionResult> Register(RegisterUserDto userDto)
        {
            var result = await _userService.RegisterCustomerAsync(userDto);
            return Ok(result);
        }
        [HttpPost("RegisterWithRoles")]
        public async Task<IActionResult> RegisterWithRolesAsync(RegisterWithRolesDto userDto)
        {
            var result = await _userService.RegisterWithRolesAsync(userDto);
            return Ok(result);
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            var token = await _userService.LoginAsync(userDto);
            return Ok(new { Token = token });
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(int userId, UserUpdateDto userDto)
        {
            var result = await _userService.UpdateUserAsync(userId, userDto);
            return Ok(result);
        }
        [HttpGet("GetUserTickets")]
        public async Task<IActionResult> GetUserTickets(int userId)
        {
            var tickets = await _userService.GetUserTicketsAsync(userId);
            return Ok(tickets);
        }

    }
}
