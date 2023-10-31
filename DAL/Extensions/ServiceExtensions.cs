using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureDataAccessLayer(
            this IServiceCollection services)
        {
            services.Scan(scan =>
                scan.FromAssemblyOf<CatalogRepository>()
                    .AddClasses(cl => cl.Where(type => type.Name.EndsWith("Repository")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            return services;
        }
    }
}
