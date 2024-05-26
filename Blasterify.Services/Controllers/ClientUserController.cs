using Blasterify.Services.Data;
using Blasterify.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Blasterify.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientUserController : Controller
    {
        private readonly DataContext _context;

        public ClientUserController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Blasterify.Models.Model.ClientUserModel clientUserModel)
        {
            var createClientUser = await _context!.ClientUsers!.AddAsync(new ClientUser()
            {
                FirstName = clientUserModel.FirstName,
                LastName = clientUserModel.LastName,
                CardNumber = clientUserModel.CardNumber,
                Email = clientUserModel.Email,
                PasswordHash = clientUserModel.PasswordHash,
                SubscriptionDate = clientUserModel.SubscriptionDate,
                SubscriptionId = clientUserModel.SubscriptionId
            });

            await _context.SaveChangesAsync();

            var yunoId = await Services.YunoServices.CreateCustomer(new Blasterify.Models.Yuno.CustomerRequest()
            {
                merchant_customer_id = $"{createClientUser.Entity.Id}",
                merchant_customer_created_at = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"),
                first_name = clientUserModel.FirstName,
                last_name = clientUserModel.LastName,
                email = clientUserModel.Email,
                created_at = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"),
                updated_at = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ")
            });

            if(Services.YunoServices.ErrorCodes.Contains(yunoId))
            {
                return BadRequest();
            }
            
            createClientUser.Entity.YunoId = yunoId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ClientUser>>> GetAll()
        {
            var clientUsers = await _context!.ClientUsers!.ToListAsync();
            return Ok(clientUsers);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var clientUser = await _context!.ClientUsers!.FindAsync(id);

            if (clientUser == null)
            {
                return NotFound();
            }

            return Ok(clientUser);
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<ActionResult<ClientUser>> LogIn(LogIn logIn)
        {
            try
            {
                var clientUser = await _context!.ClientUsers!.FirstOrDefaultAsync(cu => cu.Email == logIn.Email);

                if (clientUser == null)
                {
                    return NotFound();
                }
                else
                {
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        byte[] bytes = logIn.PasswordHash!;
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != clientUser.PasswordHash![i])
                            {
                                return Unauthorized();
                            }
                        }
                    }

                    return Ok(new ClientUser
                    {
                        Id = clientUser.Id,
                        FirstName = clientUser.FirstName,
                        LastName = clientUser.LastName,
                        CardNumber = clientUser.CardNumber,
                        Email = clientUser.Email,
                        SubscriptionDate = clientUser.SubscriptionDate,
                        SubscriptionId = clientUser.SubscriptionId,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return NotFound();
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, ClientUser clientUser)
        {
            var getClientUser = await _context!.ClientUsers!.FindAsync(id);
            if (getClientUser == null)
            {
                return NotFound();
            }

            getClientUser!.FirstName = clientUser.FirstName;
            getClientUser!.LastName = clientUser.LastName;
            getClientUser!.CardNumber = clientUser.CardNumber;
            getClientUser!.IsConnected = clientUser.IsConnected;
            getClientUser!.Email = clientUser.Email;
            getClientUser!.PasswordHash = clientUser.PasswordHash;
            getClientUser!.SubscriptionDate = clientUser.SubscriptionDate;
            getClientUser!.SubscriptionId = clientUser.SubscriptionId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("UpdateLastConnection")]
        public async Task<IActionResult> UpdateLastConnectionDate(Blasterify.Models.Request.LastUserConnection lastUserConnection)
        {
            var getClientUser = await _context!.ClientUsers!.FindAsync(lastUserConnection.Id);
            if (getClientUser == null)
            {
                return NotFound();
            }

            getClientUser!.LastConnectionDate = lastUserConnection.Date;
            getClientUser!.IsConnected = lastUserConnection.IsConnected;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var clientUser = await _context!.ClientUsers!.FindAsync(id);
            _context!.ClientUsers!.Remove(clientUser!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}