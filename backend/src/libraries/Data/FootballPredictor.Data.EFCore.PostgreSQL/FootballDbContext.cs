using FootballPredictor.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictor.Data.EFCore.PostgreSQL
{
    public class FootballDbContext : DbContext
    {
        public DbSet<Match> Matches { get; set; }

        public FootballDbContext(DbContextOptions<FootballDbContext> options) : base(options)
        {
        }
    }
}
