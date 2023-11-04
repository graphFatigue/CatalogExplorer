using BLL.Services.Interfaces;
using Core.Entity;
using DAL.Repositories.Interfaces;
using System.IO;

namespace BLL.Services
{
    public class FileImportService: IFileImportService
    {
        private readonly IBaseRepository<Catalog> _catalogRepository;
        private Dictionary<string, Catalog> _addedCatalogs;

        public FileImportService(IBaseRepository<Catalog> catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public async Task ExportToTextFileAsync(string filePath)
        {
            var catalogs = await _catalogRepository.GetAllAsync();
            var lines = new List<string>();

            foreach (var catalog in catalogs)
            {
                lines.Add(BuildCatalogPath(catalog));
            }
                await File.WriteAllLinesAsync(filePath, lines);
        }

        private string BuildCatalogPath(Catalog catalog)
        {
            var pathParts = new List<string>();
            while (catalog != null)
            {
                pathParts.Insert(0, catalog.Name);
                catalog = catalog.ParentCatalog;
            }

            return string.Join("\\", pathParts);
        }

        public async Task ImportFromDirectoryAsync(string directoryPath)
        {
            var catalogs = ImportCatalogsFromDirectory(directoryPath, null);
            await SaveCatalogsToDatabaseAsync(catalogs);
        }

        public async Task ImportFromFileAsync(Stream fileStream)
        {
            var catalogs = ImportCatalogsFromFile(fileStream, null);
            await SaveCatalogsToDatabaseAsync(catalogs);
        }

        private IEnumerable<Catalog> ImportCatalogsFromDirectory(string directoryPath, Catalog parentCatalog)
        {
            var catalogs = new List<Catalog>();
            var directories = Directory.GetDirectories(directoryPath);

            var currentCatalog = new Catalog
            {
                Name = Path.GetFileName(directoryPath),
                ParentCatalog = parentCatalog,
                ParentCatalogId = parentCatalog?.Id
            };

            //var catalogsVerify = _catalogRepository.GetAllAsync();
            //var count = catalogsVerify.Where(x => x.Name == model.Name && x.ParentCatalogId == model.ParentCatalogId).Count();
            //if (count != 0)
            //{
            //    throw new Exception("Can't create catalog with existing name");
            //}

            catalogs.Add(currentCatalog);

            foreach (var dirPath in directories)
            {
                var childCatalogs = ImportCatalogsFromDirectory(dirPath, currentCatalog);
                if (childCatalogs.Any())
                {
                    currentCatalog.ChildrenCatalogs = (ICollection<Catalog>)childCatalogs;
                    catalogs.AddRange(childCatalogs);
                }
            }

            return catalogs;
        }

        private IEnumerable<Catalog> ImportCatalogsFromFile(Stream fileStream, Catalog parentCatalog = null)
        {
            _addedCatalogs = new Dictionary<string, Catalog>();
            var catalogs = new List<Catalog>();

            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    AddCatalogToHierarchy(line, parentCatalog, catalogs);
                }
            }

            return catalogs;
        }

        private void AddCatalogToHierarchy(string path, Catalog parentCatalog, List<Catalog> catalogs)
        {
            var parts = path.Split('\\', '/');
            if (parts.Length >= 1)
            {
                var catalogName = parts[0];
                if (!_addedCatalogs.ContainsKey(catalogName))
                {
                    var catalog = new Catalog
                    {
                        Name = catalogName,
                        ParentCatalog = parentCatalog,
                        ParentCatalogId = parentCatalog?.Id
                    };

                    _addedCatalogs.Add(catalogName, catalog);
                    catalogs.Add(catalog);

                    if (parts.Length > 1)
                    {
                        var subPath = string.Join("\\", parts, 1, parts.Length - 1);
                        AddCatalogToHierarchy(subPath, catalog, catalogs);
                    }
                }
                else
                {
                    var existingCatalog = _addedCatalogs[catalogName];
                    if (parts.Length > 1)
                    {
                        var subPath = string.Join("\\", parts, 1, parts.Length - 1);
                        AddCatalogToHierarchy(subPath, existingCatalog, catalogs);
                    }
                }
            }
        }

        private async Task SaveCatalogsToDatabaseAsync(IEnumerable<Catalog> catalogs)
        {
            foreach (var catalog in catalogs)
            {
                if (catalog.Id==0)
                    await _catalogRepository.CreateAsync(catalog);
            }
        }
    }
}
