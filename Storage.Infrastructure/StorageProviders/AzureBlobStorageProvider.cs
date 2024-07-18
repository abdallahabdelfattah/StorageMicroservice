using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.Extensions.Configuration;
using Storage.Infrastructure.Interfaces;

namespace Storage.Infrastructure.StorageProviders
{

    public class AzureFileStorageProvider : IStorageProvider
    {
        private readonly ShareClient _shareClient;

        public AzureFileStorageProvider(IConfiguration configuration)
        {
            _shareClient = new ShareClient(
                configuration["AzureFileStorage:ConnectionString"],
                configuration["AzureFileStorage:FileShareName"]
            );
            _shareClient.CreateIfNotExists();
        }

        public async Task SaveFileAsync(string fileName, Stream fileStream)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(fileStream.Length);
            await fileClient.UploadRangeAsync(new HttpRange(0, fileStream.Length), fileStream);
        }

        public async Task<Stream> GetFileAsync(string fileName)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);

            var downloadResponse = await fileClient.DownloadAsync();
            var memoryStream = new MemoryStream();
            await downloadResponse.Value.Content.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.DeleteIfExistsAsync();
        }

        
    }

}
