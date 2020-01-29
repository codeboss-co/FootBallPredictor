using FootballPredictor.Dto;
using System.Threading.Tasks;

namespace FootballPredictor.Domain.Services
{
    public interface IMatchDataProvider
    {
        Task<MatchDayData> GetMatchDayDataAsync(string competition, int matchday, string accessToken);
    }
}
