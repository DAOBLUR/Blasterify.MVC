using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Models;

namespace Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly DataContext _context;

        public RentController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Rent rent)
        {
            await _context!.Rents!.AddAsync(rent);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("CreateRentItems")]
        public async Task<IActionResult> CreateRentItems(List<RentItem> rentItems)
        {
            for (int i = 0; i < rentItems.Count; i++)
            {
                await _context!.RentItems!.AddAsync(rentItems[i]);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAllRentsClientUser")]
        public async Task<ActionResult<IEnumerable<Rent>>> GetAllRentsClientUser(int clientUserId)
        {
            var rents = await _context!.Rents!.Where(r => r.ClientUserId == clientUserId).ToListAsync();
            return Ok(rents);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Rent>>> GetAll()
        {
            var rents = await _context!.Rents!.ToListAsync();
            return Ok(rents);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var rent = await _context!.Rents!.FindAsync(id);

            if (rent == null)
            {
                return NotFound();
            }

            return Ok(rent);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, Rent rent)
        {
            var getRent = await _context!.Rents!.FindAsync(id);
            getRent!.BuyDate = rent.BuyDate;
            getRent!.ClientUserId = rent.ClientUserId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var rent = await _context!.Rents!.FindAsync(id);
            _context.Rents.Remove(rent!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}