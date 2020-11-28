using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using simple_crud.Data.Entities;

namespace simple_crud.Data
{
    public interface INoveltyRepository
    {
        public Task<Novelty> GetAsync(int id, CancellationToken cancellationToken);

        public Task<IAsyncEnumerable<string>> GetNamesAsync(CancellationToken cancellationToken);

        public Task<bool> TryRemove(int id, CancellationToken cancellationToken);

        public Task<bool> TryUpdate(int id, Novelty novelty, CancellationToken cancellationToken);

        public Task<bool> TryAdd(Novelty novelty, CancellationToken cancellationToken);
    }

    public class NoveltyRepository : INoveltyRepository
    {
        public Task<Novelty> GetAsync(int id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IAsyncEnumerable<string>> GetNamesAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TryRemove(int id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TryUpdate(int id, Novelty novelty, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TryAdd(Novelty novelty, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class NoveltyRepositoryException : Exception
    { }
}
