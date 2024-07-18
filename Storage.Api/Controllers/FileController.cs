using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Storage.Application;

namespace Storage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var fileName = await _fileService.UploadFileAsync(file);
            return Ok(new { FileName = fileName });
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var fileStream = await _fileService.GetFileAsync(fileName);
            if (fileStream == null)
                return NotFound();

            var contentType = "application/octet-stream";
            return File(fileStream, contentType, fileName);
        }

        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            await _fileService.DeleteFileAsync(fileName);
            return NoContent();
        }
    }

}
