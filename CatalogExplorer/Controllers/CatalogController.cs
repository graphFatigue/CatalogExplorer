using BLL.Services.Interfaces;
using Core.Entity;
using Core.Enum;
using Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace CatalogExplorer.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogs(int parentCatalogId)
        {
            var response = await _catalogService.GetCatalogsAsync(parentCatalogId);
            if (response.StatusCode == Core.Enum.StatusCode.OK && response.Description == "Found 0 elements")
            {
                Catalog catalog = new Catalog() { ParentCatalogId = parentCatalogId };
                List<Catalog> catalogs = new List<Catalog> { catalog };
                return View(catalogs);
            }
            if (response.StatusCode == Core.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return View("Error", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalog(int id)
        {
            var response = await _catalogService.GetCatalogAsync(id);
            if (response.StatusCode == Core.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id, int parentCatalogId)
        {
            if (id == 0)
            {
                return View();
            }

            var response = await _catalogService.GetCatalogAsync(id);
            if (response.StatusCode == Core.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Save(Catalog model, int parentCatalogId)
        {
            model.ParentCatalogId = parentCatalogId;
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {

                    await _catalogService.CreateAsync(model);
                }
                else
                {
                    await _catalogService.EditAsync(model.Id, model);
                }
                return RedirectToAction("GetCatalogs", "Catalog", new { parentCatalogId = model.ParentCatalogId });
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id, int pCId)
        {
            var response = await _catalogService.DeleteCatalogAsync(id);
            if (response.StatusCode == Core.Enum.StatusCode.OK)
            {   if (pCId != 0)
                {
                    return RedirectToAction("GetCatalog", "Catalog", new { id = pCId });
                } 
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Error");
        }
    }
}
