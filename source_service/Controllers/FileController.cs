using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using source_service.Dtos.Source;
using source_service.Model;
using source_service.Service.Interface;
// using Google.Cloud.Storage.V1;

namespace source_service.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile fileModel)
        {
            await _fileService.Upload(fileModel, "663a09790e03e88cc8635618", "public");
            return Ok();
        }

        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> Download(string name)
        {
            var download = await _fileService.GetFile(name);
            if (download == null)
            {
                return NotFound();
            }

            return File(download.File, download.ContentType, name);
        }


        [HttpGet]
        [Route("download2")]
        public async Task<IActionResult> Download2(string name)
        {
            var download = await _fileService.GetFile(name);
            if (download == null)
            {
                return NotFound();
            }

            return Ok(download);

        }





    }
}
