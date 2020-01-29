using FootballPredictor.Data.Abstractions.Model;
using System;
using System.Collections.Generic;

namespace FootballPredictor.Data.Abstractions
{
    public abstract class DbSeeder<TEntity> where TEntity : class, IEntity<Guid>, IAggregateRoot<Guid>
    {
        public abstract IEnumerable<TEntity> Seed();
    }
}
