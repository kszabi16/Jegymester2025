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
    public interface IScreeningService
    {
        Task<IEnumerable<ScreeningDto>> GetAllAsync();
        Task<ScreeningDto> GetByIdAsync(int id);
        Task<ScreeningDto> CreateAsync(ScreeningCreateDto dto);
        Task<ScreeningDto> UpdateAsync(int id, ScreeningUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }


    public class ScreeningService : IScreeningService
    {
        private readonly JegymesterDbContext _context;
        private readonly IMapper _mapper;

        public ScreeningService(JegymesterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScreeningDto>> GetAllAsync()
        {
            var screenings = await _context.Screenings.Where(s => !s.Deleted).ToListAsync();
            return _mapper.Map<IEnumerable<ScreeningDto>>(screenings);
        }

        public async Task<ScreeningDto> GetByIdAsync(int id)
        {
            var screening = await _context.Screenings.Where(s => !s.Deleted && s.Id == id).FirstOrDefaultAsync();
            if (screening == null)
            {
                throw new KeyNotFoundException("Screening not found.");
            }
            return _mapper.Map<ScreeningDto>(screening);
        }

        public async Task<ScreeningDto> CreateAsync(ScreeningCreateDto dto)
        {
            var screening = _mapper.Map<Screening>(dto);
            _context.Screenings.Add(screening);
            await _context.SaveChangesAsync();
            return _mapper.Map<ScreeningDto>(screening);
        }

        public async Task<ScreeningDto> UpdateAsync(int id, ScreeningUpdateDto dto)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                throw new KeyNotFoundException("Screening not found.");
            }

            screening.MovieId = dto.MovieId;
            screening.StartTime = dto.StartTime;
            screening.RoomId = dto.RoomId;

            await _context.SaveChangesAsync();
            return _mapper.Map<ScreeningDto>(screening);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                throw new KeyNotFoundException("Screening not found.");
            }

            screening.Deleted = true;
            _context.Screenings.Update(screening);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
