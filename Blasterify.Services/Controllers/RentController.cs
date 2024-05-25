using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Blasterify.Services.Data;
using Blasterify.Services.Models;
using System.Data;
using Hangfire.Dashboard;


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
        public async Task<IActionResult> Create(Blasterify.Models.Request.PreRentRequest preRent)
        {
            var id = Guid.NewGuid();

            if (preRent.Id == Guid.Empty || preRent.Date == DateTime.MinValue)
            {
                var rent = new Rent()
                {
                    Id = id,
                    Date = DateTime.UtcNow,
                    Name = preRent.Name,
                    Address = preRent.Address,
                    CardNumber = preRent.CardNumber,
                    IsEnabled = true, //For Cart
                    ClientUserId = preRent.ClientUserId,
                    StatusId = 2, //Pending
                };
                await _context.Rents!.AddAsync(rent);

                foreach (var item in preRent.PreRentItems!)
                {
                    await _context.RentItems!.AddAsync(new RentItem()
                    {
                        Id = 0,
                        Price = item.Price,
                        RentDuration = item.RentDuration,
                        RentId = id,
                        MovieId = item.MovieId
                    });
                }
            }
            else
            {
                id = preRent.Id;
                var rent = await _context!.Rents!.FindAsync(id);
                rent = new Rent()
                {
                    Date = DateTime.UtcNow,
                    Name = preRent.Name,
                    Address = preRent.Address,
                    CardNumber = preRent.CardNumber,
                    IsEnabled = true, //For Cart
                    StatusId = 2, //Pending
                };
                
                var rentItems = await _context!.RentItems!.Where(pr => pr.RentId == preRent.Id).ToListAsync();

                foreach (var item in preRent.PreRentItems!)
                {
                    var rentItem = rentItems.Find(ri => ri.Id == item.Id);

                    if (rentItem == null)
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
                    else
                    {
                        rentItem.Price = item.Price;
                        rentItem.RentDuration = item.RentDuration;

                        rentItems.Remove(rentItem);

                        await _context.SaveChangesAsync();
                    }
                }

                foreach(var item in rentItems)
                {
                    _context.RentItems!.Remove(item);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(id);
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

            var rentDetail = new Blasterify.Models.Model.RentDetailModel()
            { 
                Id = rentId,
                Date = rent!.Date,
                Name = rent.Name,
                Address = rent.Address,
                CardNumber = rent.CardNumber,
                RentItemDetailModels = new List<Blasterify.Models.Model.RentItemDetailModel>(rentItems.Count)
            };

            foreach (var item in rentItems)
            {
                var movie = await _context.Movies!.FindAsync(item.MovieId);
                rentDetail.RentItemDetailModels.Add(new Blasterify.Models.Model.RentItemDetailModel()
                {
                    MovieId = item.MovieId,
                    RentDuration = item.RentDuration,
                    Title = movie!.Title,
                    Duration = movie.Duration,
                    Description = movie.Description,
                    FirebasePosterId = movie.FirebasePosterId,
                    Price = item.Price
                });
            }

            return Ok(rentDetail);
        }

        [HttpGet]
        [Route("GetLastPreRent")]
        public async Task<IActionResult> GetLastPreRent(int clientUserId)
        {
            var getPreRent = new Blasterify.Models.Response.PreRentResponse
            {
                PreRentItems = new List<Blasterify.Models.Response.PreRentItemResponse>()
            };

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
        [Route("CompleteRent")]
        public async Task<IActionResult> CompleteRent([FromBody] Guid rentId)
        {
            var getRent = await _context!.Rents!.FindAsync(rentId);

            if (getRent == null)
            {
                return NotFound();
            }

            getRent!.StatusId = 1;
            getRent!.IsEnabled = false;

            await Services.YunoServices.CreateCustomer(new Blasterify.Models.Yuno.CustomerRequest()
            {
                merchant_customer_id = "4321",
                merchant_customer_created_at = DateTime.UtcNow.ToString(),
                first_name = "Harry",
                last_name = "Potter",
                gender = "H",
                date_of_birth = "1980-07-31",
                email = "karlonaix@gmail.com",
                nationality = "US",
                country = "US",
                document = new Blasterify.Models.Yuno.Document()
                {
                    document_type = "CPF",
                    document_number = "12345678909"
                },
                phone = new Blasterify.Models.Yuno.Phone()
                {
                    number = "973215894",
                    country_code = "51"
                },
                billing_address = new Blasterify.Models.Yuno.Address()
                {
                    address_line_1 = "Av. Parra",
                    address_line_2 = "Nª 105",
                    city = "Lima",
                    state = "Lima",
                    country = "US",
                    zip_code= "12345",
                    neighborhood = "Comuna 57"
                },
                shipping_address = new Blasterify.Models.Yuno.Address()
                {
                    address_line_1 = "Av. Parra",
                    address_line_2 = "Nª 105",
                    city = "Lima",
                    state = "Lima",
                    country = "US",
                    zip_code= "12345",
                    neighborhood = "Comuna 57"
                },
                metadata = new Blasterify.Models.Yuno.Metadata[]
                {
                    new Blasterify.Models.Yuno.Metadata()
                    {
                        key = "key1",
                        value = "value1"
                    },
                    new Blasterify.Models.Yuno.Metadata()
                    {
                        key = "key2",
                        value = "value2"
                    }
                },
                created_at = DateTime.UtcNow.ToString(),
                updated_at = DateTime.UtcNow.ToString()
            });

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