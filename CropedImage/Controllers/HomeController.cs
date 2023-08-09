using CropedImage.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Diagnostics;
using System.IO;

namespace CropedImage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveCropedImage(string filename, IFormFile blob)
        {
            try
            {
                using (var image = Image.Load(blob.OpenReadStream()))
                {
                    var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    filename = Guid.NewGuid().ToString() + "_" + filename;
                    var path = Path.Combine(uploadDir, filename);
                    image.Mutate(x => x.Resize(100, 100));
                    image.Save(path);
                }
                return Json(new { Message = "ok" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving cropped image");
                return Json(new { Message = "Error" });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
