using BLL.Services;
using BLL.Services.Interfaces;
using CatalogExplorer.Models;
using Core.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CatalogExplorer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService _catalogService;
        private readonly IFileImportService _fileImportService;

        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService, IFileImportService fileImportService)
        {
            _logger = logger;
            _catalogService = catalogService;
            _fileImportService = fileImportService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _catalogService.GetCatalogsAsync();
            if (response.StatusCode == Core.Enum.StatusCode.OK)
            {
                response.Data.OrderBy(e => e.Name).ToList();
                return View(response.Data);
            }

            return View("Error", $"{response.Description}");
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

        [HttpPost]
        public async Task<IActionResult> ImportFromDirectoryAsync(string directoryPath)
        {
            await _fileImportService.ImportFromDirectoryAsync(directoryPath);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ImportFromFileAsync(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            using (var stream = file.OpenReadStream())
            {
                await _fileImportService.ImportFromFileAsync(stream);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ExportToTextFileAsync(string filePath)
        {
            if (filePath == null || filePath.Length <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            await _fileImportService.ExportToTextFileAsync(filePath);
            return RedirectToAction(nameof(Index));
        }

    }
}