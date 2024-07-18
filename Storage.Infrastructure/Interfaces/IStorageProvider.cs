using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Infrastructure.Interfaces
{
    public interface IStorageProvider
    {
        Task SaveFileAsync(string fileName, Stream fileStream);
        Task<Stream> GetFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
    }
}
