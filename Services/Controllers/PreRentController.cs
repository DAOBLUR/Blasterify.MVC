using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Data;
using Services.Models;
using System.Data;
using System.Security.Principal;

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

        [HttpGet]
        [Route("Testing")]
        public async Task<IActionResult> GetTesting()
        {
            var preRent = await _context!.Movies!.FromSqlRaw("exec GetAllMovies").ToListAsync();

            if (preRent == null)
            {
                return NotFound();
            }

            return Ok(preRent);
        }

        [HttpGet]
        [Route("GetLastPreRent")]
        public async Task<IActionResult> GetLastPreRent(int clientUserId)
        {
            var getPreRent = new Blasterify.Models.Response.PreRent();
            getPreRent.PreRentItems = new List<Blasterify.Models.Response.PreRentItem>();

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
                    var preRentItem = new Blasterify.Models.Response.PreRentItem
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
            var preRent = await _context!.PreRents!.FindAsync(id);

            var preRentItems = await _context.PreRentItems!.Where(pri => pri.RentId == id).ToListAsync();

            foreach (var item in preRentItems)
            {
                _ = _context.PreRentItems!.Remove(item!);
            }

            _context.PreRents.Remove(preRent!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}