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
	public interface IInboxService : IAsyncDisposable
    {
        public Task<List<Inbox>> GetAll();

        public Task<Inbox> GetById(int id);

        public Task<Inbox> Create(Inbox inbox);

        public Task<Inbox> Update(Inbox inbox);

        public Task Delete(int id);

    }
}