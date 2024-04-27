using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Models;

namespace Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreRentController : ControllerBase
    {
        private readonly DataContext _context;

        public PreRentController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(PreRent preRent)
        {
            await _context!.PreRents!.AddAsync(preRent);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("CreatePreRentItems")]
        public async Task<IActionResult> CreatePreRentItems(List<PreRentItem> preRentItems)
        {
            foreach (var item in preRentItems)
            {
                await _context!.PreRentItems!.AddAsync(item);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAllPreRentsClientUser")]
        public async Task<ActionResult<IEnumerable<PreRent>>> GetAllPreRentsClientUser(int clientUserId)
        {
            var preRents = await _context!.PreRents!.Where(pr => pr.ClientUserId == clientUserId).ToListAsync();
            return Ok(preRents);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<PreRent>>> GetAll()
        {
            var preRents = await _context!.PreRents!.ToListAsync();
            return Ok(preRents);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(Guid id)
        {
            var preRent = await _context!.PreRents!.FindAsync(id);

            if (preRent == null)
            {
                return NotFound();
            }

            return Ok(preRent);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Guid id, PreRent preRent)
        {
            var getPreRent = await _context!.PreRents!.FindAsync(id);
            getPreRent!.Date = preRent.Date;
            getPreRent!.ClientUserId = preRent.ClientUserId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var rent = await _context!.PreRents!.FindAsync(id);
            _context.PreRents.Remove(rent!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}