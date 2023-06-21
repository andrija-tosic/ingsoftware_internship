using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace VacaYAY.Business.Services;

public class FileService : IFileService
{
    private readonly BlobServiceClient _blobServiceClient;

    public FileService(string connectionString)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo>> SaveFile(IFormFile file)
    {
        string newFileName = new Guid().ToString()
            + "-"
            + DateTime.Now.ToString("yyyyMMddHHmmss")
            + Path.GetExtension(file.FileName);

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("container1");

        BlobClient blobClient = containerClient.GetBlobClient(newFileName);

        Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo> response = await blobClient.UploadAsync(file.OpenReadStream());

        return response;

        //return $"{blobClient.Uri}/{newFileName}";
    }
    public async Task<Azure.Response<bool>> DeleteFile(string path)
    {
        Uri uri = new Uri(path);
        string containerName = uri.Segments[1].TrimEnd('/');
        string blobName = uri.Segments[2];

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        return await blobClient.DeleteIfExistsAsync();
    }
}
