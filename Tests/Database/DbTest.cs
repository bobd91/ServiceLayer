using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using WebApp.Data;
using WebApp.Models;

namespace Tests
{
    public class DbTest
    {
        public DatabaseContext MakeInMemoryContext()
        {
            return MakeDbContext(false);
        }

        public DatabaseContext MakeSqliteContext()
        {
            return MakeDbContext(true);
        }

        protected DatabaseContext MakeDbContext(bool useSqlite)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>()
                .EnableSensitiveDataLogging();

            builder = useSqlite
                ? builder.UseSqlite("Data Source=:memory:")
                : builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var context = new DatabaseContext(builder.Options);

            if (useSqlite)
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
            }

            SeedDatabase(context);

            return context;
        }

        protected void SeedDatabase(DatabaseContext context)
        {
            SeedInbox(context);
        }

        protected void SeedInbox(DatabaseContext context)
        {
            context.Inbox.Add(new Inbox
            {
                Id = 1,
                Value = "Do first",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            });
            context.Inbox.Add(new Inbox
            {
                Id = 2,
                Value = "Do second",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
            });
            context.SaveChanges();
        }

    }
}
