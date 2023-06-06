namespace VacaYAY.Business.Services;

public interface IHttpService
{
    public Task<T?> Get<T>(string requestUri);
}
