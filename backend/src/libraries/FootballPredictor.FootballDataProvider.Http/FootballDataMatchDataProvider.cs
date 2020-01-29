using System;
using System.Net.Http;
using System.Threading.Tasks;
using FootballPredictor.Common;
using FootballPredictor.Domain.Services;
using FootballPredictor.Dto;

namespace FootballPredictor.FootballDataProvider.Http
{
    public class FootballDataMatchDataProvider : IMatchDataProvider
    {
        private const string FOOTBALL_DATA_API_ADDRESS = "https://api.football-data.org/v2/competitions/";
        private readonly IHttpClientFactory _httpClientFactory;

        public FootballDataMatchDataProvider(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<MatchDayData> GetMatchDayDataAsync(string competition, int matchday, string accessToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            
            httpClient.BaseAddress = new Uri(FOOTBALL_DATA_API_ADDRESS);
            httpClient.DefaultRequestHeaders.Add("X-Auth-Token", accessToken);

            var response = await httpClient.GetAsync($"{competition}/matches?matchday={matchday}")
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
