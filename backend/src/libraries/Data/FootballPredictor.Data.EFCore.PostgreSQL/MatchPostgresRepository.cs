using FootballPredictor.Domain.Model;
using FootballPredictor.Domain.Repositories;

namespace FootballPredictor.Data.EFCore.PostgreSQL
{
    public class MatchPostgresRepository : PostgreSQLRepository<Match, long>, IMatchDbRepository
    {
        public MatchPostgresRepository(FootballDbContext dbContext) : base(dbContext)
        {
        }
    }
}
