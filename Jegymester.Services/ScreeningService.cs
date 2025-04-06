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
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task<TicketDto> GetByIdAsync(int id);
        Task<TicketDto> CreateAsync(TicketDto dto);
        Task<TicketDto> UpdateAsync(int id, TicketDto dto);
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

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var screenings = await _context.Screenings.ToListAsync();
            return _mapper.Map<IEnumerable<TicketDto>>(screenings);
        }
        public async Task<TicketDto> GetByIdAsync(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                throw new KeyNotFoundException("Screening not found.");
            }
            return _mapper.Map<TicketDto>(screening);
        }
        public async Task<TicketDto> CreateAsync(TicketDto dto)
        {
            var screening = _mapper.Map<Screening>(dto);
            _context.Screenings.Add(screening);
            await _context.SaveChangesAsync();
            return _mapper.Map<TicketDto>(screening);
        }
        public async Task<TicketDto> UpdateAsync(int id, TicketDto dto)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                throw new KeyNotFoundException("Screening not found.");
            }
            _mapper.Map(dto, screening);
            await _context.SaveChangesAsync();
            return _mapper.Map<TicketDto>(screening);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                throw new KeyNotFoundException("Screening not found.");
            }
            _context.Screenings.Remove(screening);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
