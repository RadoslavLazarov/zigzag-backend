using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Web;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Configurations;
using ZigZag.Domain.Models.Spacecraft;

namespace ZigZag.Infrastructure.Services
{
    public class SpacecraftService : ISpacecraftService
    {
        private readonly HttpClient _client;
        private readonly SpacecraftApiConfiguration _spacecraftApiConfiguration;

        public SpacecraftService(
            HttpClient client,
            IOptionsMonitor<SpacecraftApiConfiguration> optionsMonitor)
        {
            _client = client;
            _spacecraftApiConfiguration = optionsMonitor.CurrentValue;
        }

        public async Task<SpacecraftsModel> GetAllSpacecrafts(int? pageNumber)
        {
            var builder = new UriBuilder(_spacecraftApiConfiguration.BaseUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (pageNumber.HasValue)
            {
                query["pageNumber"] = pageNumber.Value.ToString();
            }

            builder.Query = query.ToString();
            var url = builder.ToString();


            var httpResponse = await _client.GetAsync($"{url}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request failed with status code: {httpResponse.StatusCode}");
            }

            var responseModel = new SpacecraftsModel();
            var httpResult = await httpResponse.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(httpResult))
            {
                responseModel = JsonSerializer.Deserialize<SpacecraftsModel>(httpResult);
            }

            return responseModel;
        }
    }
}
