using Blasterify.Services.Data;
using Blasterify.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blasterify.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly DataContext _context;

        public CountryController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Country country)
        {
            await _context.Countries!.AddAsync(country);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Country>>> GetAll()
        {
            var countries = await _context.Countries!.ToListAsync();
            return Ok(countries);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var country = await _context.Countries!.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, Country country)
        {
            var getCountry = await _context.Countries!.FindAsync(id);
            getCountry!.Name = country.Name;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var country = await _context.Countries!.FindAsync(id);
            _context.Countries.Remove(country!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}