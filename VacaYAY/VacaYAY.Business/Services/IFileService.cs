using Microsoft.AspNetCore.Http;

namespace VacaYAY.Business.Services;

public interface IFileService
{
    Task<Uri> SaveFile(IFormFile file);
    Task<Azure.Response<bool>> DeleteFile(string path);
}
