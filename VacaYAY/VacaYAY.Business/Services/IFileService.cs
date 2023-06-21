using Microsoft.AspNetCore.Http;

namespace VacaYAY.Business.Services;

public interface IFileService
{
    Task<Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo>> SaveFile(IFormFile file);
    Task<Azure.Response<bool>> DeleteFile(string path);
}
