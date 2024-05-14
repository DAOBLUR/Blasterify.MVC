using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blasterify.Services.Data;
using Blasterify.Services.Models;

namespace Blasterify.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly DataContext _context;

        public MovieController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Movie movie)
        {
            await _context.Movies!.AddAsync(movie);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("CreateWithList")]
        public async Task<IActionResult> CreateWithList(List<Movie> movies)
        {
            foreach(var movie in movies)
            {
                await _context!.Movies!.AddAsync(movie);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAll()
        {
            var movies = await _context.Movies!.ToListAsync();
            return Ok(movies);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var movie = await _context.Movies!.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, Movie movie)
        {
            var getMovie = await _context.Movies!.FindAsync(id);
            getMovie!.Title = movie.Title;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("DeleteCountry")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies!.FindAsync(id);
            _context.Movies.Remove(movie!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}