using FootballPredictor.Data.Abstractions;
using FootballPredictor.Domain.Model;
using System;

namespace FootballPredictor.Domain.Repositories
{
    public interface ITeamsDbRepository : IGenericDbRepository<Team, Guid>
    {
        
    }
}
