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
        public async Task<MovieDto> AddMovieAsync(MovieCreateDto movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            movie.Deleted = false;

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<ScreeningDto> AddScreeningAsync(ScreeningCreateDto screeningDto)
        {
            // Validálás: Film létezik
            var movie = await _context.Movies.FindAsync(screeningDto.MovieId);
            if (movie == null || movie.Deleted)
                return null;

            // Validálás: Terem létezik
            var room = await _context.Rooms.FindAsync(screeningDto.RoomId);
            if (room == null)
                return null;

            // Validálás: StartTime a jövőben van
            if (screeningDto.StartTime <= DateTime.Now)
                return null;

            
            var screening = new Screening
            {
                Name = screeningDto.Title,
                StartTime = screeningDto.StartTime,
                RoomId = screeningDto.RoomId,
                MovieId = screeningDto.MovieId,
                Tickets = new List<Ticket>() 
            };

            _context.Screenings.Add(screening);
            await _context.SaveChangesAsync();

            
            var screeningResult = new ScreeningDto
            {
                Id = screening.Id,
                Deleted = false,
                MovieId = screening.MovieId,
                Title = screening.Name,
                StartTime = screening.StartTime,
                EndTime = screening.StartTime.AddMinutes(movie.Length),
                RoomId = screening.RoomId,
                AvailableSeats = screeningDto.AvailableSeats
            };

            return screeningResult;
        }


        public async Task<bool> DeleteMovieAsync(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null || movie.Deleted)
                return false;

            movie.Deleted = true;
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteScreeningAsync(int screeningId)
        {
            var screening = await _context.Screenings.FindAsync(screeningId);
            if (screening == null)
                return false;

            _context.Screenings.Remove(screening);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> UpdateMovieAsync(int movieId, MovieUpdateDto movieDto)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null || movie.Deleted)
                return false;

            _mapper.Map(movieDto, movie);

            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateScreeningAsync(int screeningId, ScreeningUpdateDto screeningDto)
        {
            var screening = await _context.Screenings.FindAsync(screeningId);
            if (screening == null)
                return false;

            // Validálás: Film létezik
            var movie = await _context.Movies.FindAsync(screeningDto.MovieId);
            if (movie == null || movie.Deleted)
                return false;

            // Validálás: Terem létezik
            var room = await _context.Rooms.FindAsync(screeningDto.RoomId);
            if (room == null)
                return false;

            // Validálás: StartTime a jövőben van
            if (screeningDto.StartTime <= DateTime.Now)
                return false;

            screening.MovieId = screeningDto.MovieId;
            screening.RoomId = screeningDto.RoomId;
            screening.StartTime = screeningDto.StartTime;
            screening.Name = screeningDto.Title;

            _context.Screenings.Update(screening);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
