using ChatServer.Infrastructure.Repositories.BaseAbstractions;
using ChatServer.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IRepositoryWrapper Repos;

        public FileController(ILogger<AccountController> logger, IWebHostEnvironment env, IRepositoryWrapper repos)
        {
            _logger = logger;
            _env = env;
            Repos = repos;
        }

        [HttpGet("DownloadFile/{trustedFileName}")]
        public async Task<IActionResult> DownloadFile(string trustedFileName)
        {
            var uploadResult = (await Repos.Uploads.FindByConditionAsync(u => u.StoredFileName.Equals(trustedFileName))).FirstOrDefault();

            var path = Path.Combine(_env.ContentRootPath, "uploads", trustedFileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, uploadResult.ContentType, Path.GetFileName(path));
        }

        [HttpPost("UploadFile")]
        public async Task<ActionResult<List<UploadResult>>> UploadFile(List<IFormFile> files)
        {
            List<UploadResult> uploadResults = new List<UploadResult>();

            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                string trustedFileNameForFileStorage;
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;

                trustedFileNameForFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(_env.ContentRootPath, "uploads", trustedFileNameForFileStorage);

                await using FileStream fs = new(path, FileMode.Create);
                await file.CopyToAsync(fs);

                uploadResult.StoredFileName = trustedFileNameForFileStorage;
                uploadResult.ContentType = file.ContentType;
                uploadResult.DateCreated = DateTime.Now;
                uploadResult.FileSize = file.Length;
                uploadResults.Add(uploadResult);

                await Repos.Uploads.CreateAsync(uploadResult);
            }

            return Ok(uploadResults);
        }
    }
}