using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwFormat;
using SwFormat.Entity;

namespace SwFormatConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SldFileController : ControllerBase
    {
        private readonly string _modelPath;
        private static string _progromPath;

        public SldFileController(IWebHostEnvironment hostEnvironment)
        {
            _modelPath = Path.Combine(hostEnvironment.WebRootPath, "Models");
            _progromPath = hostEnvironment.WebRootPath;
        }

        /// <summary>
        /// Upload FileName
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<SldResult<SldFile>> UpLoadFileAsync(IFormFile sldfile)
        {
            if (!Regex.IsMatch(sldfile.FileName,".sld",RegexOptions.IgnoreCase))
            {
                return new SldResult<SldFile>
                {
                    Code = 202,
                    Msg = "Upload failed, the file must be a SolidWorks format"
                };
            }
            
            if (sldfile.Length > 1024 * 10000)
            {
                return new SldResult<SldFile>
                {
                    Code = 202,
                    Msg = "Upload failed, the file size must be less than 10MB"
                };
            }
            
            var sldFile = await SaveImgAsync(sldfile);

            await SaveAsHtmlAsync(sldFile, ".html");

            return new SldResult<SldFile>()
            {
                Data = sldFile
            };
        }

        [HttpGet]
        public async Task<SldResult<bool>> Query(string id)
        {
            var isExist = ExistItems.Exist(id);

            return await Task.FromResult(new SldResult<bool>()
            { 
                Code = 200,
                Data = isExist
            });
        }

        private async Task<SldFile> SaveImgAsync(IFormFile swfile)
        {
            var id = Guid.NewGuid().ToString();
             var swfileName = $"{id}{Path.GetExtension(swfile.FileName)}";
            
            if(!Directory.Exists(_modelPath))
            {
                Directory.CreateDirectory(_modelPath);
            }

            var PathStr = Path.Combine(_modelPath, swfileName);
            if (System.IO.File.Exists(PathStr))
            {
                System.IO.File.Delete(PathStr);
            }
            using (var FileStream = System.IO.File.OpenWrite(PathStr))
            {
                await swfile.CopyToAsync(FileStream);
            }
            return new SldFile( )
            {
                FileName = Path.GetFileName(swfile.FileName),
                FileId = id,
                FileUrl = $"/Models/{swfileName}",
                PreviewUrl = $"/Models/{Path.ChangeExtension(swfileName,".html")}"
            };
        }

        private Task<string> SaveAsHtmlAsync(SldFile file,string format)
        {
            var filePathName = Path.Combine(_modelPath, file.FileId+Path.GetExtension(file.FileName));
            
            return Task.FromResult(SaveAsByFormat(filePathName, format));
        }

        private string SaveAsByFormat(string filePathName,string format)
        {
            try
            {
                var resultHtml = Path.ChangeExtension(filePathName, format);
                var startInfo = new ProcessStartInfo()
                {
                    FileName = $"{_progromPath}\\EdrawingPort.exe",
                    Arguments = $"{filePathName} {format}",
                };
                using (var process = new Process()
                {
                    StartInfo = startInfo
                })
                {
                    process.Start();
                    process.WaitForExit();
                }
                
                ExistItems.Add(Path.GetFileNameWithoutExtension(filePathName));

                return $"Models/{Path.GetFileName(resultHtml)}";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
