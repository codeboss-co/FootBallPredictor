using FootballPredictor.Data.Abstractions;
using FootballPredictor.Data.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictor.Data.EFCore.PostgreSQL
{
    public class PostgreSQLRepository<TEntity, TId> : IGenericDbRepository<TEntity, TId> where TEntity : class, IEntity<TId>, IAggregateRoot<TId>
    {
        private readonly DbContext _dbContext;

        public PostgreSQLRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<TEntity>> Query()
        {
            throw new NotImplementedException();
        }
    }
}
