using FootballPredictor.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FootballPredictor.Data.Abstractions.Model;

namespace FootballPredictor.Data.InMemory
{
    public class InMemoryDbRepository<TEntity> : IGenericDbRepository<TEntity, Guid> where TEntity : class, IEntity<Guid>, IAggregateRoot<Guid>
    {
        public List<TEntity> Db = new List<TEntity>(10);

        public InMemoryDbRepository(DbSeeder<TEntity> seeder)
        {
            Db.AddRange(seeder.Seed());
        }


        public Task InsertAsync(TEntity entity, CancellationToken token = default) => throw new NotImplementedException();
        public Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default) => throw new NotImplementedException();

        public Task<IReadOnlyCollection<TEntity>> Query(CancellationToken token = default) => Task.FromResult(Db.AsReadOnly() as IReadOnlyCollection<TEntity>);
    }
}
