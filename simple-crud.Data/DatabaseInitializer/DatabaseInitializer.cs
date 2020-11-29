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
                    Description = LoremIpsum, LastChanged = DateTime.UtcNow
                },
                new Novelty
                {
                    Name = "second", Version = 1, Created = new DateTime(2020, 01, 02, 12, 0, 0),
                    Description = Cicero, LastChanged = DateTime.UtcNow
                },
                new Novelty
                {
                    Name = "third", Version = 1, Created = new DateTime(2020, 01, 03, 12, 0, 0),
                    Description = Kafka, LastChanged = DateTime.UtcNow
                }
            };

            var fillerNovelties = Enumerable.Range(1, 100).Select(x => new Novelty
            {
                Name = $"filler novelty {x}", Version = 1, Created = new DateTime(1900, 01, 01),
                LastChanged = DateTime.UtcNow, Description = "some boring description"
            });

            foreach (var novelty in novelties.Concat(fillerNovelties))
            {
                await context.Novelties.AddAsync(novelty, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Database has been initialized");
        }

        private const string LoremIpsum =
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc,";

        private const string Cicero =
            "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur? At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere";

        private const string Kafka =
            "One morning, when Gregor Samsa woke from troubled dreams, he found himself transformed in his bed into a horrible vermin.He lay on his armour-like back, and if he lifted his head a little he could see his brown belly, slightly domed and divided by arches into stiff sections.The bedding was hardly able to cover it and seemed ready to slide off any moment. His many legs, pitifully thin compared with the size of the rest of him, waved about helplessly as he looked. \"What's happened to me?\" he thought. It wasn't a dream. His room, a proper human room although a little too small, lay peacefully between its four familiar walls. A collection of textile samples lay spread out on the table - Samsa was a travelling salesman - and above it there hung a picture that he had recently cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out with a fur hat and fur boa who sat upright, raising a heavy fur muff that covered the whole of her lower arm towards the viewer. Gregor then turned to look out the window at the dull weather. Drops";
    }
}
