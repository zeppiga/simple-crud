using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using simple_crud.Data.Contexts;
using simple_crud.Data.Entities;

namespace simple_crud.Data.DatabaseInitializer
{
    public static class DatabaseInitializer
    {
        public static async Task Initialize(NoveltyContext context, ILogger logger, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Preparing for database initialization..");

            await context.Database.EnsureCreatedAsync(cancellationToken);

            if (context.Novelties.Any())
            {
                logger.LogInformation("Database has been already fed, skipping initialization..");
                return;
            }

            var novelties = new[]
            {
                new Novelty
                {
                    Name = "first", Version = 1, Created = new DateTime(2020, 01, 01, 12, 0, 0),
                    Description = "some description", LastChanged = DateTime.UtcNow
                },
                new Novelty
                {
                    Name = "second", Version = 1, Created = new DateTime(2020, 01, 02, 12, 0, 0),
                    Description = "some description 2", LastChanged = DateTime.UtcNow
                },
            };

            foreach (var novelty in novelties)
            {
                await context.Novelties.AddAsync(novelty, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Database has been initialized");
        }
    }
}
