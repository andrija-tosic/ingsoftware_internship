using Newtonsoft.Json;

namespace VacaYAY.Business.Services;
public class JsonParserService : IJsonParserService
{
    public string? Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public T? Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}
