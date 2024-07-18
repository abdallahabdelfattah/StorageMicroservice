using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Domain.Repositories
{
    public interface IFileRepository
    {
        Task SaveFileAsync(string fileName, IFormFile file);
        Task<Stream> GetFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
    }


}
