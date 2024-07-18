using Microsoft.AspNetCore.Http;
using Storage.Domain.Repositories;

namespace Storage.Application
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var fileId = Guid.NewGuid();
            var fileExtension = Path.GetExtension(file.FileName);
            var sanitizedFileExtension = SanitizeFileName(fileExtension);
            var fileName = $"{fileId}{sanitizedFileExtension}";
            fileName = SanitizeFileName(fileName);
            await _fileRepository.SaveFileAsync(fileName, file);
            return fileName;
        }


        private string SanitizeFileName(string fileName)
        {
            // Replace invalid characters with a safe character, e.g., '-'
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar, '-');
            }
            return fileName;
        }


        public async Task<Stream> GetFileAsync(string fileName)
        {
            return await _fileRepository.GetFileAsync(fileName);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _fileRepository.DeleteFileAsync(fileName);
        }
    }



}
