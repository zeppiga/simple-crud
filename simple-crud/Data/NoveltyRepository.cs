using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using simple_crud.Data.Contexts;
using simple_crud.Data.Entities;
using simple_crud.ServicesRegistration;

namespace simple_crud.Data
{
        public interface INoveltyRepository
        {
            public Task<INovelty> GetAsync(int id, CancellationToken cancellationToken);

            public Task<NoveltyInfo[]> GetInfosAsync(int take, int offset, CancellationToken cancellationToken);

            public Task<bool> TryRemove(int id, CancellationToken cancellationToken);

            public Task<bool> TryUpdate(int id, INovelty novelty, CancellationToken cancellationToken);

            public Task<bool> TryAdd(INovelty novelty, CancellationToken cancellationToken);

            public Task<int> TotalCountAsync(CancellationToken cancellationToken);
        }

        [Transient(typeof(INoveltyRepository))]
        public class NoveltyRepository : INoveltyRepository
        {
            private readonly NoveltyContext _context;

            public NoveltyRepository(NoveltyContext context)
            {
                _context = context;
            }

            public Task<INovelty> GetAsync(int id, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }

            //todo consider iasyncenumerable usage
            public async Task<NoveltyInfo[]> GetInfosAsync(int take, int offset, CancellationToken cancellationToken)
            {
                var novelties = await _context.Novelties.OrderByDescending(x => x.LastChanged).Skip(offset).Take(take).Select(x => new NoveltyInfo(x)).ToArrayAsync(cancellationToken);
                return novelties;
            }

            public Task<bool> TryRemove(int id, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }

            public Task<bool> TryUpdate(int id, INovelty novelty, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }

            public Task<bool> TryAdd(INovelty novelty, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public async Task<int> TotalCountAsync(CancellationToken cancellationToken)
            {
                return await _context.Novelties.CountAsync(cancellationToken);
            }
        }

        public class NoveltyRepositoryException : Exception
        { }
}