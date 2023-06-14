using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VacaYAY.Business.Services;

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IHttpService> _logger;
    private readonly IJsonParserService _jsonParser;

    public HttpService(ILogger<IHttpService> logger, IHttpClientFactory httpClientFactory, IJsonParserService jsonParser)
    {
        _logger = logger;
        _jsonParser = jsonParser;
        _httpClient = httpClientFactory.CreateClient(nameof(IHttpService));
    }

    public async Task<T?> Get<T>(string requestUri)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(requestUri, response.StatusCode, response.ReasonPhrase);
            return default;
        }

        string responseJson = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(responseJson))
        {
            throw new JsonException(response.StatusCode.ToString());
        }

        T? items = _jsonParser.Deserialize<T>(responseJson);

        return items;
    }
}
