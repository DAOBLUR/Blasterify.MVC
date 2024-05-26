using Blasterify.Services.Data;
using Blasterify.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blasterify.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : Controller
    {
        private readonly DataContext _context;

        public SubscriptionController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Subscription subscription)
        {
            await _context.Subscriptions!.AddAsync(subscription);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetAll()
        {
            var subscriptions = await _context.Subscriptions!.ToListAsync();
            return Ok(subscriptions);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var subscription = await _context.Subscriptions!.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return Ok(subscription);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, Subscription subscription)
        {
            var getSubscription = await _context.Subscriptions!.FindAsync(id);
            if (getSubscription == null)
            {
                return NotFound();
            }

            getSubscription!.Name = subscription.Name;
            getSubscription!.Price = subscription.Price;
            getSubscription!.Features = subscription.Features;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var subscription = await _context.Subscriptions!.FindAsync(id);
            _context.Subscriptions.Remove(subscription!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}