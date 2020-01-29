using FootballPredictor.Data.Abstractions;
using FootballPredictor.Data.Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballPredictor.Data.EFCore.PostgreSQL
{
    public class PostgreSQLRepository<TEntity, TId> : IGenericDbRepository<TEntity, TId> where TEntity : class, IEntity<TId>, IAggregateRoot<TId>
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public PostgreSQLRepository(FootballDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task InsertAsync(TEntity entity, CancellationToken token = default)
        {
            await _dbSet.AddAsync(entity);
            int inserted = await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);
        }

        public async Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
        {
            await _dbSet.AddRangeAsync(entities);
            int inserted = await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<TEntity>> Query(CancellationToken token = default)
        {
            var entities = await _dbSet.AsQueryable().ToListAsync(token);
            return entities.AsReadOnly();
        }
    }
}
