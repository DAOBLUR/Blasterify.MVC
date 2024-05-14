using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Blasterify.Services.Data;
using Blasterify.Services.Models;
using Blasterify.Services.Models.Response;
using System.Data;

namespace Blasterify.Services.Controllers
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
        public async Task<IActionResult> Create(Blasterify.Models.Response.PreRentResponse preRent)
        {
            var rent = new Rent() 
            {
                Id = preRent.Id,
                Date = preRent.Date ?? DateTime.UtcNow,
                Name = preRent.Name,
                Address = preRent.Address,
                CardNumber = preRent.CardNumber,
                IsEnabled = true, //For Cart
                ClientUserId = preRent.ClientUserId,
                StatusId = 2, //Pending
            };
            await _context.Rents!.AddAsync(rent);

            foreach(var item in preRent.PreRentItems!)
            {
                await _context.RentItems!.AddAsync(new RentItem()
                {
                    Id = 0,
                    Price = item.Price,
                    RentDuration = item.RentDuration,
                    RentId = preRent.Id,
                    MovieId = item.MovieId
                });
            }

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
            var rent = await _context.Rents!.FindAsync(rentId);

            var rentItems = await _context.RentItems!.Where(ri => ri.RentId == rentId).ToListAsync();

            var rentDetail = new RentDetail()
            { 
                Id = rentId,
                RentDate = rent!.Date,
                ClientUserId = rent.ClientUserId,
                RentMovies = new List<RentMovie>(rentItems.Count)
            };

            foreach (var item in rentItems)
            {
                var movie = await _context!.Movies!.FindAsync(item.MovieId);
                rentDetail.RentMovies.Add(new RentMovie(movie!, item.RentDuration));
            }

            return Ok(rentDetail);
        }

        [HttpGet]
        [Route("GetLastPreRent")]
        public async Task<IActionResult> GetLastPreRent(int clientUserId)
        {
            var getPreRent = new Blasterify.Models.Response.PreRentResponse();
            getPreRent.PreRentItems = new List<Blasterify.Models.Response.PreRentItemResponse>();

            SqlParameter parameter = new("@ClientUserId", clientUserId);
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetLastPreRent";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(parameter);

                _context.Database.OpenConnection();

                using var result = await command.ExecuteReaderAsync();
                while (await result.ReadAsync())
                {
                    getPreRent.Id = result.GetGuid(0);
                    getPreRent.Date = result.GetDateTime(1);
                    getPreRent.ClientUserId = result.GetInt32(2);
                }
            }

            if (getPreRent.Id == Guid.Empty && getPreRent.Date == default && getPreRent.ClientUserId == default)
            {
                return NotFound();
            }

            parameter = new("@PreRentId", getPreRent.Id);
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetLastPreRentItems";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(parameter);

                _context.Database.OpenConnection();

                using var result = await command.ExecuteReaderAsync();
                while (await result.ReadAsync())
                {
                    var preRentItem = new Blasterify.Models.Response.PreRentItemResponse
                    {
                        Id = result.GetInt32(0),
                        MovieId = result.GetInt32(1),
                        RentDuration = result.GetInt32(2),
                        Title = result.GetString(3),
                        FirebasePosterId = result.GetString(4),
                        Price = (double)result.GetDecimal(5),
                    };

                    getPreRent.PreRentItems!.Add(preRentItem);
                }
            }

            if (getPreRent.PreRentItems!.Count <= 0)
            {
                return NotFound();
            }

            return Ok(getPreRent);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Guid id, Rent rent)
        {
            var getRent = await _context!.Rents!.FindAsync(id);
            getRent!.Date = rent.Date;
            getRent!.ClientUserId = rent.ClientUserId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<IActionResult> Update(Guid id, int rentStatusId, bool isEnabled)
        {
            var getRent = await _context!.Rents!.FindAsync(id);
            getRent!.StatusId = rentStatusId;
            getRent!.IsEnabled = isEnabled;

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