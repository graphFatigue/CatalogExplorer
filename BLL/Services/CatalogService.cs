using BLL.Services.Interfaces;
using Core.Entity;
using Core.Enum;
using Core.Response;
using DAL;
using DAL.Repositories.Interfaces;


namespace BLL.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IBaseRepository<Catalog> _catalogRepository;
        private readonly AppDbContext _context;

        public CatalogService(IBaseRepository<Catalog> catalogRepository, AppDbContext context)
        {
            _catalogRepository = catalogRepository;
            _context = context;
        }

        public async Task<IBaseResponse<Catalog>> CreateAsync(Catalog model)
        {
            try
            {
                var catalog = new Catalog()
                {
                    Name = model.Name,
                    ParentCatalogId = model.ParentCatalogId,    
                };
                var catalogs = await _catalogRepository.GetAllAsync();
                var count = catalogs.Where(x => x.Name == model.Name && x.ParentCatalogId == model.ParentCatalogId).Count();
                if (count!=0)
                {
                    throw new Exception("Can't create catalog with existing name");
                }
                await _catalogRepository.CreateAsync(catalog);

                return new BaseResponse<Catalog>()
                {
                    StatusCode = StatusCode.OK,
                    Data = catalog
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Catalog>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteCatalogAsync(int id)
        {
            try
            {
                var catalog = await _catalogRepository.GetByIdAsync(id);
                if (catalog == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Catalog not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _catalogRepository.DeleteAsyncRecursive(catalog);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteCatalog] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Catalog>> EditAsync(int id, Catalog model)
        {
            try
            {
                var catalog = await _catalogRepository.GetByIdAsync(id);
                if (catalog == null)
                {
                    return new BaseResponse<Catalog>()
                    {
                        Description = "Album not found",
                        StatusCode = StatusCode.CatalogNotFound
                    };
                }

                catalog.Name = model.Name;
                catalog.ParentCatalogId = model.ParentCatalogId;

                await _catalogRepository.UpdateAsync(catalog);


                return new BaseResponse<Catalog>()
                {
                    Data = catalog,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Catalog>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Catalog>> GetCatalogAsync(int id)
        {
            try
            {
                var catalog = await _catalogRepository.GetByIdAsync(id);
                if (catalog == null)
                {
                    return new BaseResponse<Catalog>()
                    {
                        Description = "Catalog is not found",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                return new BaseResponse<Catalog>()
                {
                    StatusCode = StatusCode.OK,
                    Data = catalog
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Catalog>()
                {
                    Description = $"[GetCatalogAsync] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<List<Catalog>>> GetCatalogsAsync(int parentCatalogId)
        {
            try
            {
                var catalogs = await _catalogRepository.GetAllAsync();
                catalogs = catalogs.Where(x => x.ParentCatalogId == parentCatalogId).ToList();
                if (!catalogs.Any())
                {
                    return new BaseResponse<List<Catalog>>()
                    {
                        Description = "Found 0 elements",
                        StatusCode = StatusCode.OK
                    };
                }
                catalogs = catalogs.OrderByDescending(o => o.Name).ToList();
                return new BaseResponse<List<Catalog>>()
                {
                    Data = (List<Catalog>)catalogs,
                    StatusCode = StatusCode.OK
                };
            }

            catch (Exception ex)
            {

                return new BaseResponse<List<Catalog>>()
                {
                    Description = $"[GetCatalogs] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<List<Catalog>>> GetCatalogsAsync()
        {
            try
            {
                var catalogs = await _catalogRepository.GetAllAsync();
                if (!catalogs.Any())
                {
                    return new BaseResponse<List<Catalog>>()
                    {
                        Description = "Found 0 elements",
                        StatusCode = StatusCode.OK
                    };
                }
                catalogs = catalogs.OrderBy(o => o.Name).ToList();
                return new BaseResponse<List<Catalog>>()
                {
                    Data = (List<Catalog>)catalogs,
                    StatusCode = StatusCode.OK
                };
            }

            catch (Exception ex)
            {

                return new BaseResponse<List<Catalog>>()
                {
                    Description = $"[GetCatalogs] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
