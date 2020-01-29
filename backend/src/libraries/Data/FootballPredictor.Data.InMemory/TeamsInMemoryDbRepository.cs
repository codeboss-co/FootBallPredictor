using FootballPredictor.Data.Abstractions;
using FootballPredictor.Domain.Model;
using FootballPredictor.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace FootballPredictor.Data.InMemory
{
    public class TeamsInMemoryDbRepository : InMemoryDbRepository<Team>, ITeamsDbRepository
    {
        public TeamsInMemoryDbRepository(DbSeeder<Team> seeder) : base(seeder)
        {
        }
    }

    public class TeamsDbSeeder : DbSeeder<Team>
    {
        public override IEnumerable<Team> Seed()
        {
            return new List<Team>(10)
            {
                new Team
                {
                    Id = Guid.NewGuid(),
                    Name = "AC Milan"
                },
                new Team
                {
                    Id = Guid.NewGuid(),
                    Name = "Juventus"
                }
            };
        }
    }
}
