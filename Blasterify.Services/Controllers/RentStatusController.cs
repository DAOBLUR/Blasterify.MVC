using Blasterify.Services.Data;
using Blasterify.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blasterify.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentStatusController : ControllerBase
    {
        private readonly DataContext _context;

        public RentStatusController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(RentStatus rentStatus)
        {
            await _context.RentStatuses!.AddAsync(rentStatus);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<RentStatus>>> GetAll()
        {
            var genres = await _context.RentStatuses!.ToListAsync();
            return Ok(genres);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var rentStatus = await _context.RentStatuses!.FindAsync(id);

            if (rentStatus == null)
            {
                return NotFound();
            }

            return Ok(rentStatus);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, RentStatus rentStatus)
        {
            var getRentStatus = await _context.RentStatuses!.FindAsync(id);
            getRentStatus!.Name = rentStatus.Name;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var rentStatus = await _context.RentStatuses!.FindAsync(id);
            _context.RentStatuses.Remove(rentStatus!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}