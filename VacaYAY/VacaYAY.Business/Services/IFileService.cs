using Microsoft.AspNetCore.Http;

namespace VacaYAY.Business.Services;

public interface IFileService
{
    Task<Uri> SaveFileAsync(IFormFile file);
    Task<Azure.Response<bool>> DeleteFile(string path);
}
