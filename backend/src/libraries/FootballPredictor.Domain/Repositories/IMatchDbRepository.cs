using FootballPredictor.Data.Abstractions;
using FootballPredictor.Domain.Model;

namespace FootballPredictor.Domain.Repositories
{
    public interface IMatchDbRepository : IGenericDbRepository<Match, long>
    {
    }
}
