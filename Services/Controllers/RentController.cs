using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Models;
using Services.Models.Response;

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

            //Thread RentThread = new Thread(async () => await DeletePreRent(_context, rent));
            //RentThread.Start();

            await DeletePreRent(_context, rent);



            return Ok();
        }

        private async Task DeletePreRent(DataContext context, Rent rent)
        {
            var preRent = await context!.PreRents!.FindAsync(rent.Id);
            context.PreRents.Remove(preRent!);

            var preRentItems = await context!.PreRentItems!.Where(pri => pri.RentId == rent.Id).ToListAsync();

            foreach(var item in preRentItems)
            {
                _ = context.PreRentItems!.Remove(item!);
            }

            await context.SaveChangesAsync();
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
        public async Task<IActionResult> Get(Guid id)
        {
            var rent = await _context!.Rents!.FindAsync(id);

            if (rent == null)
            {
                return NotFound();
            }

            return Ok(rent);
        }

        [HttpGet]
        [Route("GetRentDetail")]
        public async Task<IActionResult> GetRentDetail(Guid rentId)
        {
            var rent = await _context!.Rents!.FindAsync(rentId);

            var rentItems = await _context!.RentItems!.Where(ri => ri.RentId == rentId).ToListAsync();

            var rentDetail = new RentDetail()
            { 
                Id = rentId,
                RentDate = rent!.RentDate,
                ClientUserId = rent.ClientUserId,
                RentMovies = new List<Blasterify.Client.Models.RentMovie>(rentItems.Count)
            };

            foreach (var item in rentItems)
            {
                var movie = await _context!.Movies!.FindAsync(item.MovieId);
                rentDetail.RentMovies.Add(new Blasterify.Client.Models.RentMovie(movie!, item.RentDuration));
            }

            return Ok(rentDetail);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Guid id, Rent rent)
        {
            var getRent = await _context!.Rents!.FindAsync(id);
            getRent!.RentDate = rent.RentDate;
            getRent!.ClientUserId = rent.ClientUserId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var rent = await _context!.Rents!.FindAsync(id);
            _context.Rents.Remove(rent!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}