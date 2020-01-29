using FootballPredictor.Data.Abstractions.Model;
using System;

namespace FootballPredictor.Domain.Model
{
    public class Team : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
