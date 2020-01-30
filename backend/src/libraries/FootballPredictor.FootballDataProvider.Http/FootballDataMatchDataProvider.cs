using FootballPredictor.Common;
using FootballPredictor.Domain.Services;
using FootballPredictor.Dto;
using System.Net.Http;
using System.Threading.Tasks;

namespace FootballPredictor.FootballDataProvider.Http
{
    /// <summary>
    ///  https://api.football-data.org/v2/competitions/PL/matches?matchday=11
    /// </summary>
    public class FootballDataMatchDataProvider : IMatchDataProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FootballDataMatchDataProvider(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<MatchDayData> GetMatchDayDataAsync(string competition, int matchday, string accessToken)
        {
            var httpClient = _httpClientFactory.CreateClient("football-data");

            var response = await httpClient.GetAsync($"/v2/competitions/{competition}/matches?matchday={matchday}")
                                            .ConfigureAwait(false);

            MatchDayData matchData = null;
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                matchData = data.FromJsonOrNull<MatchDayData>();
            }

            return matchData;
        }
    }
}
