using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using WebApp.Models;
using WebApp.Data;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

/**
 * Business logic for Inbox Items
 * Uses EF as the Repository layer
 * 
 * There is much internet debate about the utility of adding own Repository pattern over EF
 * If we decide that is necessary then we can easily add that layer here
 * 
 * TODO: Validation prior to insert/update
 */

namespace WebApp.Services
{
	public class InboxService : IInboxService, IAsyncDisposable
    {
        private readonly DatabaseContext _context;

        public InboxService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Inbox>> GetAll()
        {
            return await _context
                .Inbox
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Inbox> GetById(int id)
        {
            try
            {
                return await _context
                    .Inbox
                    .AsNoTracking()
                    .SingleAsync(_ => _.Id == id);
            }
            catch(InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<Inbox> Create(Inbox inbox)
        {
            // TODO: validation
            // Just set the fields we want
            var now = DateTime.UtcNow;
            var newInbox = new Inbox()
            { 
                Value = inbox.Value,
                CreatedAt = now,
                UpdatedAt = now
            };

            _context.Inbox.Add(newInbox);
            await _context.SaveChangesAsync();

            return newInbox;
        }

        public async Task<Inbox> Update(Inbox inbox)
        {
            // TODO: validation
            // Just update the fields we want
            var modInbox = new Inbox() { Id = inbox.Id };

            // Attach then modify to only update certain fields
            _context.Inbox.Attach(modInbox);

            modInbox.Value = inbox.Value;
            modInbox.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return modInbox;
        }

        public async Task Delete(int id)
        {
            var inbox = new Inbox() { Id = id };

            _context.Entry(inbox).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return;
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

    }
}