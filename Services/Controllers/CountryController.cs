using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Models;

namespace Services.Controllers
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
        public async Task<IActionResult> CreateCountry(Country country)
        {
            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("ReadAllCountries")]
        public async Task<ActionResult<IEnumerable<Country>>> ReadAllCountries()
        {
            var countries = await _context.Countries.ToListAsync();
            return Ok(countries);
        }

        [HttpGet]
        [Route("ReadCountry")]
        public async Task<IActionResult> ReadCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateCountry(int id, Country country)
        {
            var getCountry = await _context.Countries.FindAsync(id);
            getCountry!.Name = country.Name;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("DeleteCountry")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            _context.Countries.Remove(country!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}