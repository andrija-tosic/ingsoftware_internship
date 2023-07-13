using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace VacaYAY.Business.Services;

public class FileService : IFileService
{
    private readonly BlobServiceClient _blobServiceClient;

    public FileService()
    {
        _blobServiceClient = new BlobServiceClient("UseDevelopmentMode=true");
    }

    public async Task<Uri> SaveFileAsync(IFormFile file)
    {
        string newFileName = Guid.NewGuid().ToString()
            + "-"
            + DateTime.Now.ToString("yyyyMMddHHmmss")
            + Path.GetExtension(file.FileName);

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("container1");
        await containerClient.CreateIfNotExistsAsync();

        BlobClient blobClient = containerClient.GetBlobClient(newFileName);

        _ = await blobClient.UploadAsync(file.OpenReadStream(),
            new BlobHttpHeaders
            {
                ContentType = file.ContentType
            });

        return blobClient.Uri;
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
