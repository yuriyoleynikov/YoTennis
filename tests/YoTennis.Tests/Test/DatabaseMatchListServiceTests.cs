using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using YoTennis.Data;
using YoTennis.Services;

namespace YoTennis.Tests.Test
{
    public class DatabaseMatchListServiceTests : MatchListServiceTests
    {
        private MyDbContext _context = new MyDbContext(CreateNewContextOptions());

        public override IMatchListService GetMatchListService() => new DatabaseMatchListService(_context);

        private static DbContextOptions<MyDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<MyDbContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }    
}
