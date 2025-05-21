using Jegymester.DataContext.Dtos;
using Jegymester.DataContext.Entities;
using Jegymester.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // 🔓 Mindenki (vendég is) láthatja
    [AllowAnonymous]
    [HttpGet("GetAllMovie")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        var movies = await _movieService.GetAllMoviesAsync();
        return Ok(movies);
    }

    [AllowAnonymous]
    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        return Ok(movie);
    }

    // 🔐 Csak adminoknak
    [Authorize(Roles = "Admin")]
    [HttpPost("CreateMovie")]
    public async Task<ActionResult<Movie>> CreateMovie(MovieCreateDto movieDto)
    {
        var createdMovie = await _movieService.CreateMovieAsync(movieDto);
        return CreatedAtAction(nameof(GetMovie), new { id = createdMovie.Id }, createdMovie);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateMovie/{id}")]
    public async Task<IActionResult> UpdateMovie(int id, MovieUpdateDto movieDto)
    {
        var updatedMovie = await _movieService.UpdateMovieAsync(id, movieDto);
        if (updatedMovie == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteMovie/{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        await _movieService.DeleteMovieAsync(id);
        return NoContent();
    }
}
