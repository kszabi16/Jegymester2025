using AutoMapper;
using Jegymester.DataContext.Context;
using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;

namespace Jegymester.Services
{
    public interface IAdminService
    {
        Task<MovieDto> AddMovieAsync(MovieCreateDto movieDto);
        Task<bool> UpdateMovieAsync(int movieId, MovieUpdateDto movieDto);
        Task<bool> DeleteMovieAsync(int movieId);
        Task<ScreeningDto> AddScreeningAsync(ScreeningCreateDto screeningDto);
        Task<bool> UpdateScreeningAsync(int screeningId, ScreeningUpdateDto screeningDto);
        Task<bool> DeleteScreeningAsync(int screeningId);
    }

    public class AdminService : IAdminService
    {
        private readonly IMapper _mapper;
        private readonly JegymesterDbContext _context;

        public AdminService(IMapper mapper, JegymesterDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public Task<MovieDto> AddMovieAsync(MovieCreateDto movieDto)
        {
            throw new NotImplementedException();
        }

        public Task<ScreeningDto> AddScreeningAsync(ScreeningCreateDto screeningDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMovieAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteScreeningAsync(int screeningId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMovieAsync(int movieId, MovieUpdateDto movieDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateScreeningAsync(int screeningId, ScreeningUpdateDto screeningDto)
        {
            throw new NotImplementedException();
        }
    }
}
