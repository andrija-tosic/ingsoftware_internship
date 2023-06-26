using Microsoft.AspNetCore.Http;

namespace VacaYAY.Business.Services;

public interface IHttpService
{
    Task<T?> GetAsync<T>(string requestUri);
    Task<IFormFile?> GetFormFileAsync(string requestUri);
}
