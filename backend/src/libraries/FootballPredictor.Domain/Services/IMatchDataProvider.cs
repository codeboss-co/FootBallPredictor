using System.Threading;
using FootballPredictor.Dto;
using System.Threading.Tasks;

namespace FootballPredictor.Domain.Services
{
    public interface IMatchDataProvider
    {
        Task<MatchDayData> GetMatchDayDataAsync(string competition, int matchday, int? season = null,CancellationToken token = default);
    }
}
