using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Transactions;
using WebApp.Models;

namespace WebApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Inbox> Inbox { get; set; }
    }
}
