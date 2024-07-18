using Microsoft.Extensions.Configuration;
using Storage.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Infrastructure.StorageProviders
{
    public class LocalStorageProvider : IStorageProvider
    {
        private readonly string _storagePath;

        public LocalStorageProvider(IConfiguration configuration)
        {
            _storagePath = configuration["LocalStorage:Path"];
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task SaveFileAsync(string fileName, Stream fileStream)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            using (var fileStreamOutput = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(fileStreamOutput);
            }
        }

        public async Task<Stream> GetFileAsync(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await fileStream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        public Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }
    }


}
