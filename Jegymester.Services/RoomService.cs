using AutoMapper;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.Services
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetRoomsAsync();
        Task<RoomDto> GetRoomByIdAsync(int roomId);
        Task<RoomDto> CreateRoomAsync(RoomCreateDto createDto);
        Task<RoomDto> UpdateRoomAsync(int roomId, RoomUpdateDto updateDto);
        Task DeleteRoomAsync(int roomId);
    }

    public class RoomService : IRoomService
    {
        private readonly JegymesterDbContext _context;
        private readonly IMapper _mapper;

        public RoomService(JegymesterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RoomDto>> GetRoomsAsync()
        {
            var rooms = await _context.Rooms
                .Where(r => !r.Deleted)
                .ToListAsync();
            return _mapper.Map<List<RoomDto>>(rooms);
        }

        public async Task<RoomDto> GetRoomByIdAsync(int roomId)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == roomId && !r.Deleted);
            if (room == null)
                throw new KeyNotFoundException("Room not found.");
            return _mapper.Map<RoomDto>(room);
        }

        public async Task<RoomDto> CreateRoomAsync(RoomCreateDto createDto)
        {
            var room = _mapper.Map<Room>(createDto);
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoomDto>(room);
        }

        public async Task<RoomDto> UpdateRoomAsync(int roomId, RoomUpdateDto updateDto)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == roomId && !r.Deleted);
            if (room == null)
                throw new KeyNotFoundException("Room not found.");

            _mapper.Map(updateDto, room);
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoomDto>(room);
        }

        public async Task DeleteRoomAsync(int roomId)
        {
            var room = await _context.Rooms
                .Include(r => r.Screenings)
                .FirstOrDefaultAsync(r => r.Id == roomId && !r.Deleted);
            if (room == null)
                throw new KeyNotFoundException("Room not found.");

            if (room.Screenings.Any(s => !s.Deleted))
                throw new InvalidOperationException("Cannot delete room with active screenings.");

            room.Deleted = true;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }
    }
}
