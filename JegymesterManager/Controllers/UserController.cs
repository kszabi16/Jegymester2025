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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JegymesterManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("RegisterCustomer")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserDto userDto)
        {
            var result = await _userService.RegisterCustomerAsync(userDto);
            return Ok(result);
        }
        [HttpPost("RegisterWithRoles")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterWithRolesAsync(RegisterWithRolesDto userDto)
        {
            var result = await _userService.RegisterWithRolesAsync(userDto);
            return Ok(result);
        }

        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            var token = await _userService.LoginAsync(userDto);
            return Ok(new { Token = token });
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UserUpdateDto userDto)
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _userService.UpdateUserAsync(userId, userDto);
            return Ok(result);
        }

        [HttpGet("GetUserTickets")]
        public async Task<IActionResult> GetUserTickets()
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var tickets = await _userService.GetUserTicketsAsync(userId);
            return Ok(tickets);
        }

    }
}
