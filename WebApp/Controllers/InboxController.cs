using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{

    [Route("api/inbox")]
    [ApiController]
    public class InboxController : ControllerBase
    {
        private readonly IServiceContext _context;

        public InboxController(IServiceContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Inbox.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var inbox = await _context.Inbox.GetById(id);
            if (inbox != null)
                return Ok(inbox);
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Inbox inbox)
        {
            // TODO: can inbox be null?
            // TODO: can Create fail due to validation failure (empty Inbox text)?
            // TODO: do we need to handle DbUpdateException and return something Not Ok?
            var newInbox = await _context.Inbox.Create(inbox);

            return CreatedAtRoute("inbox", new { id = newInbox.Id }, newInbox);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Inbox inbox)
        {
            // TODO: can inbox be null, what if id != inbox.Id
            // TODO: what if Update fails (id doesn't exist, empty text)
            // TODO: do we need to handle DbUpdateException and return something Not Ok?
           
            await _context.Inbox.Update(inbox);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // TODO: do we need to handle DbUpdateException and return something Not Ok?
            await _context.Inbox.Delete(id);
            return NoContent();
        }
    }
}
