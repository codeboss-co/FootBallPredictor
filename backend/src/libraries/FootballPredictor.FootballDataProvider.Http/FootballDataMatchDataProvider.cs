using System.Linq;
using FootballPredictor.Common;
using FootballPredictor.Domain.Services;
using FootballPredictor.Dto;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace FootballPredictor.FootballDataProvider.Http
{
    /// <summary>
    ///  https://api.football-data.org/v2/competitions/PL/matches?matchday=11
    /// </summary>
    public class FootballDataMatchDataProvider : IMatchDataProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FootballDataMatchDataProvider(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<MatchDayData> GetMatchDayDataAsync(string competition, int matchday, int? season = null, CancellationToken token = default)
        {
            var httpClient = _httpClientFactory.CreateClient("football-data");

            var response = await httpClient.GetAsync(
                    $"/v2/competitions/{competition}/matches?matchday={matchday}&season={season}", token)
                                            .ConfigureAwait(false);

            // www.football-data.org/documentation/api#response-headers
            response.Headers.TryGetValues("X-Requests-Available-Minute", out var values);
            Log.Information("FootballData requests left: {count}", values.FirstOrDefault());

            MatchDayData matchData = null;
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                matchData = data.FromJsonOrNull<MatchDayData>();
                Log.Information("FootballData matches found: {matches}", matchData?.Matches?.Count ?? 0 );
            }

            return matchData;
        }
    }
}
