namespace VacaYAY.Business.Services;

public interface IJsonParserService
{
    public string? Serialize<T>(T obj);
    public T? Deserialize<T>(string json);
}
