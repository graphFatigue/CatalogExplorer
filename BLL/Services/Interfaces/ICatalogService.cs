using Core.Entity;
using Core.Response;

namespace BLL.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<IBaseResponse<List<Catalog>>> GetCatalogsAsync(int parentCatalogId);

        Task<IBaseResponse<List<Catalog>>> GetCatalogsAsync();

        Task<IBaseResponse<Catalog>> GetCatalogAsync(int id);

        Task<IBaseResponse<Catalog>> CreateAsync(Catalog model);

        Task<IBaseResponse<bool>> DeleteCatalogAsync(int id);

        Task<IBaseResponse<Catalog>> EditAsync(int id, Catalog model);
    }
}
