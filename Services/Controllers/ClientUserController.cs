﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Models;
using System.Security.Cryptography;
using System.Text;

namespace Services.Controllers
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
        public async Task<IActionResult> Create(ClientUser clientUser)
        {
            await _context.ClientUsers.AddAsync(clientUser);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ClientUser>>> GetAll()
        {
            var clientUsers = await _context.ClientUsers.ToListAsync();
            return Ok(clientUsers);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var clientUser = await _context.ClientUsers.FindAsync(id);

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
                var clientUser = await _context.ClientUsers.FirstOrDefaultAsync(cu =>  cu.Email == logIn.Email);

                if (clientUser == null)
                {
                    return NotFound();
                }
                else
                {
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        byte[] bytes = logIn.PasswordHash;
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != clientUser.PasswordHash[i])
                                return Unauthorized();
                        }
                    }

                    //if (clientUser.PasswordHash == logIn.PasswordHash)
                  //  {
                        return Ok(new ClientUser
                        {
                            Id = clientUser.Id,
                            Username = clientUser.Username,
                            CardNumber = clientUser.CardNumber,
                            Email = clientUser.Email,
                            SuscriptionDate = clientUser.SuscriptionDate,
                            SubscriptionId = clientUser.SubscriptionId,
                            //CountryId = clientUser.CountryId,
                        });
                  //  }
                  
                   // return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return NotFound();
            }
            /*
            if (clientUser == null)
            {
                return NotFound();
            }
            else
            {
                if(clientUser.PasswordHash == logIn.PasswordHash)
                {
                    return Ok(new ClientUser
                    {
                        Id = clientUser.Id,
                        Username = clientUser.Username,
                        CardNumber = clientUser.CardNumber,
                        Email = clientUser.Email,
                        SuscriptionDate = clientUser.SuscriptionDate,
                        SubscriptionId = clientUser.SubscriptionId,
                        //CountryId = clientUser.CountryId,
                    });
                }

                return NotFound();
            }*/
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, ClientUser clientUser)
        {
            var getClientUser = await _context.ClientUsers.FindAsync(id);
            if (getClientUser == null)
            {
                return NotFound();
            }

            getClientUser!.Username = clientUser.Username;
            getClientUser!.CardNumber = clientUser.CardNumber;
            getClientUser!.Status = clientUser.Status;
            getClientUser!.Email = clientUser.Email;
            getClientUser!.PasswordHash = clientUser.PasswordHash;
            getClientUser!.SuscriptionDate = clientUser.SuscriptionDate;
            getClientUser!.SubscriptionId = clientUser.SubscriptionId;
            //getClientUser!.CountryId = clientUser.CountryId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var clientUser = await _context.ClientUsers.FindAsync(id);
            _context.ClientUsers.Remove(clientUser!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}