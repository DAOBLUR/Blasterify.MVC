using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Models;

namespace Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly DataContext _context;

        public GenreController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Genre genre)
        {
            await _context!.Genres!.AddAsync(genre);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAll()
        {
            var genres = await _context!.Genres!.ToListAsync();
            return Ok(genres);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var genre = await _context!.Genres!.FindAsync(id);
            
            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, Genre genre)
        {
            var getGenre = await _context!.Genres!.FindAsync(id);
            getGenre!.Name = genre.Name;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _context!.Genres!.FindAsync(id);
            _context.Genres.Remove(genre!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}