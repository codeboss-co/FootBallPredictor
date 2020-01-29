using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FootballPredictor.Data.Abstractions.Model;

namespace FootballPredictor.Data.Abstractions
{
    public interface IGenericDbRepository<TEntity, TId> where TEntity : class, IEntity<TId>, IAggregateRoot<TId>
    {
        Task InsertAsync(TEntity entity, CancellationToken token = default);
        Task<IReadOnlyCollection<TEntity>> Query(CancellationToken token = default);
    }
}
