using Core.Entity;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
    public class FileImportService
    {
        private readonly IBaseRepository<Catalog> _catalogRepository;

        public FileImportService(IBaseRepository<Catalog> catalogRepository)
        {
            _catalogRepository = catalogRepository;
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

        private IEnumerable<Catalog> ImportCatalogsFromFile(Stream fileStream, Catalog parentCatalog)
        {
            var catalogs = new List<Catalog>();

            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Розділяємо шлях за роздільниками \ або /
                    string[] parts = line.Split('\\', '/');

                    // Якщо є хоча б одна частина, створюємо каталог
                    if (parts.Length >= 1)
                    {
                        var catalog = new Catalog
                        {
                            Name = parts[parts.Length - 1], // Остання частина - назва каталогу чи файлу
                            ParentCatalog = parentCatalog,
                            ParentCatalogId = parentCatalog?.Id
                        };

                        catalogs.Add(catalog);

                        // Якщо шлях має більше однієї частини, можливо, потрібно створити батьківські каталоги
                        if (parts.Length > 1)
                        {
                            for (int i = parts.Length - 2; i >= 0; i--)
                            {
                                var parent = new Catalog
                                {
                                    Name = parts[i],
                                    ParentCatalog = catalog,
                                    ParentCatalogId = catalog.Id
                                };

                                catalogs.Add(parent);
                                catalog = parent;
                            }
                        }
                    }
                }
            }

            return catalogs;
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
