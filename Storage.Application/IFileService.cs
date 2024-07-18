using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<Stream> GetFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);


    }
}
