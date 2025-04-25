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
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
        Task<MovieDto> GetMovieByIdAsync(int id);
        Task<MovieDto> CreateMovieAsync(MovieCreateDto movieDto);
        Task<MovieDto> UpdateMovieAsync(int id, MovieUpdateDto movieDto);
        Task<bool> DeleteMovieAsync(int id);
    }
    public class MovieService : IMovieService
    {
        private readonly JegymesterDbContext _context;
        private readonly IMapper _mapper;

        public MovieService(JegymesterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            var movies = await _context.Movies.ToListAsync();
            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public async Task<MovieDto> GetMovieByIdAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<MovieDto> CreateMovieAsync(MovieCreateDto movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<MovieDto> UpdateMovieAsync(int id, MovieUpdateDto movieDto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                throw new KeyNotFoundException("Movie not found.");
            }

            _mapper.Map(movieDto, movie);
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                throw new KeyNotFoundException("Movie not found.");
            }

            movie.Deleted = true;
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
