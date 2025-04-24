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
    public interface ISeatService
    {
        Task<List<SeatDto>> GetSeatsByRoomAsync(int roomId);
        Task<SeatDto> GetSeatByIdAsync(int seatId);
        Task<SeatDto> CreateSeatAsync(SeatCreateDto createDto);
        Task<SeatDto> UpdateSeatAsync(int seatId, SeatUpdateDto updateDto);
        Task DeleteSeatAsync(int seatId);
    }

    public class SeatService : ISeatService
    {
        private readonly JegymesterDbContext _context;
        private readonly IMapper _mapper;

        public SeatService(JegymesterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SeatDto>> GetSeatsByRoomAsync(int roomId)
        {
            var room = await _context.Rooms
                .AnyAsync(r => r.Id == roomId && !r.Deleted);
            if (!room)
                throw new KeyNotFoundException("Room not found.");

            var seats = await _context.Seats
                .Where(s => s.RoomId == roomId && !s.Deleted)
                .ToListAsync();
            return _mapper.Map<List<SeatDto>>(seats);
        }

        public async Task<SeatDto> GetSeatByIdAsync(int seatId)
        {
            var seat = await _context.Seats
                .FirstOrDefaultAsync(s => s.Id == seatId && !s.Deleted);
            if (seat == null)
                throw new KeyNotFoundException("Seat not found.");

            return _mapper.Map<SeatDto>(seat);
        }

        public async Task<SeatDto> CreateSeatAsync(SeatCreateDto createDto)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == createDto.RoomId && !r.Deleted);
            if (room == null)
                throw new KeyNotFoundException("Room not found");

            var currentSeatCount = await _context.Seats
                .CountAsync(s => s.RoomId == createDto.RoomId && !s.Deleted);

            if (currentSeatCount >= room.Capacity)
                throw new InvalidOperationException($"The room ({room.Id}) with ({room.Capacity}) seats is full.");

           
            var seatExists = await _context.Seats
                .AnyAsync(s => s.RoomId == createDto.RoomId && !s.Deleted && s.SeatNumber == createDto.SeatNumber);
            if (seatExists)
                throw new InvalidOperationException($"A seat already exists with the ({createDto.SeatNumber}) seat number in this room. ");

            var seat = _mapper.Map<Seat>(createDto);
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();

            return _mapper.Map<SeatDto>(seat);
        }

        public async Task<SeatDto> UpdateSeatAsync(int seatId, SeatUpdateDto updateDto)
        {
            var seat = await _context.Seats
                .FirstOrDefaultAsync(s => s.Id == seatId && !s.Deleted);
            if (seat == null)
                throw new KeyNotFoundException("Seat not found.");

            _mapper.Map(updateDto, seat);
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();

            return _mapper.Map<SeatDto>(seat);
        }

        public async Task DeleteSeatAsync(int seatId)
        {
            var seat = await _context.Seats
                .FirstOrDefaultAsync(s => s.Id == seatId && !s.Deleted);
            if (seat == null)
                throw new KeyNotFoundException("Seat not found.");

            seat.Deleted = true;
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }
    }
}
