using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Services;

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(IHttpService));
    }

    public async Task<IList<Employee>?> GetFakeEmployees(int count)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/Employees/{count}");

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Request failed with status code: {response.StatusCode}");
        }

        string responseString = await response.Content.ReadAsStringAsync();

        if (responseString.IsNullOrEmpty())
        {
            throw new JsonException(response.StatusCode.ToString());
        }

        IList<Employee>? employees = (IList<Employee>?)JsonConvert.DeserializeObject(responseString, typeof(IList<Employee>));

        return employees;
    }
}
