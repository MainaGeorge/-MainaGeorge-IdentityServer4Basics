using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.API.Data;
using Movies.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MoviesController : ControllerBase
    {
        private readonly MoviesContext _context;

        public MoviesController(MoviesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Movie>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return Ok(await _context.Movies.ToListAsync());
        }

        [HttpGet("{movieId:int}", Name = nameof(GetMovieById))]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Movie>> GetMovieById(int movieId)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Id == movieId);
            if(movie is null) return NotFound();

            return Ok(movie);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
        public async Task<ActionResult<Movie>> AddMovie([FromBody]Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtRoute(nameof(GetMovieById), new { movieId=movie.Id }, movie);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{movieId:int}")]
        public async Task<IActionResult> UpdateMovie(int movieId, [FromBody]Movie movie)
        {
            if(movie.Id != movieId) return BadRequest();

            var movieToUpdate = await _context.Movies.SingleOrDefaultAsync(m => m.Id == movieId);
            if(movieToUpdate is null) return NotFound();

            _context.Entry(movieToUpdate).State = EntityState.Modified;
            //perform the update
            movieToUpdate = SwapData(movieToUpdate, movie);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

       [ProducesResponseType(StatusCodes.Status204NoContent)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
       [HttpDelete("{movieId:int}")]
        public async Task<IActionResult> DeleteMovie(int movieId)
        {
            var moviesToDelete = await _context.Movies.SingleOrDefaultAsync(m => m.Id == movieId);
            if (moviesToDelete is null) return NotFound();

            _context.Movies.Remove(moviesToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static Movie SwapData(Movie firstMovie, Movie secondMovie)
        {
            firstMovie.ImageUrl = secondMovie.ImageUrl is null ? firstMovie.ImageUrl : secondMovie.ImageUrl;
            firstMovie.Title = secondMovie.Title is null ? firstMovie.Title : secondMovie.Title;
            firstMovie.Owner = secondMovie.Owner is null ? firstMovie.Owner : secondMovie.Owner;    
            firstMovie.Rating = secondMovie.Rating is null ? firstMovie.Rating : secondMovie.Rating;
            firstMovie.Genre = secondMovie.Genre is null ?  firstMovie.Genre : secondMovie.Genre;

            return firstMovie;
        }
    }
}
