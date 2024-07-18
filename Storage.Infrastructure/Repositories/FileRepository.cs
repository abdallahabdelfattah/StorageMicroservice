using Microsoft.AspNetCore.Http;
using Storage.Domain.Repositories;
using Storage.Infrastructure.Interfaces;

namespace Storage.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IStorageProvider _storageProvider;

        public FileRepository(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public async Task SaveFileAsync(string fileName, IFormFile file)
        {
            using var stream = file.OpenReadStream();
            await _storageProvider.SaveFileAsync(fileName, stream);
        }

        public async Task<Stream> GetFileAsync(string fileName)
        {
            return await _storageProvider.GetFileAsync(fileName);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _storageProvider.DeleteFileAsync(fileName);
        }
    }


}
