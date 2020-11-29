﻿using System;
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

            public Task<BasicNoveltyInfo[]> GetInfosAsync(int take, int offset, CancellationToken cancellationToken);

            public Task<bool> TryRemove(int id, CancellationToken cancellationToken);

            public Task<bool> TryUpdate(NoveltyToAdd novelty, CancellationToken cancellationToken);

            public Task<bool> TryAdd(NoveltyToAdd novelty, CancellationToken cancellationToken);

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

            public async Task<INovelty> GetAsync(int id, CancellationToken cancellationToken)
            {
                return await _context.Novelties.SingleAsync(x => x.ID == id, cancellationToken);
            }

            //todo consider iasyncenumerable usage
            public async Task<BasicNoveltyInfo[]> GetInfosAsync(int take, int offset, CancellationToken cancellationToken)
            {
                var novelties = await _context.Novelties.OrderByDescending(x => x.LastChanged).Skip(offset).Take(take).Select(x => new BasicNoveltyInfo(x)).ToArrayAsync(cancellationToken);
                return novelties;
            }

            public async Task<bool> TryRemove(int id, CancellationToken cancellationToken)
            {
                var toRemove = await _context.Novelties.SingleAsync(x => x.ID == id, cancellationToken);
                _context.Novelties.Remove(toRemove);

                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }

            public async Task<bool> TryUpdate(NoveltyToAdd novelty, CancellationToken cancellationToken)
            {
                var now = DateTime.UtcNow;
                var noveltyToUpdate = await _context.Novelties.SingleAsync(x => x.ID == novelty.ID, cancellationToken);
                noveltyToUpdate.LastChanged = now;
                noveltyToUpdate.Version++;
                noveltyToUpdate.Name = novelty.Name;
                noveltyToUpdate.Description = novelty.Description;

                var changes = await _context.SaveChangesAsync(cancellationToken);

                return changes >= 1;
            }

            public async Task<bool> TryAdd(NoveltyToAdd novelty, CancellationToken cancellationToken)
            {
                var now = DateTime.UtcNow;
                var noveltyEntity = new Novelty
                {
                    Created = now, Description = novelty.Description, Name = novelty.Name, LastChanged = now,
                    Version = 1
                };

                await _context.Novelties.AddAsync(noveltyEntity, cancellationToken);
                var added = await _context.SaveChangesAsync(cancellationToken);

                return added >= 1;
            }

            public async Task<int> TotalCountAsync(CancellationToken cancellationToken)
            {
                return await _context.Novelties.CountAsync(cancellationToken);
            }
        }

        public class NoveltyRepositoryException : Exception
        { }
}