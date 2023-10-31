using Core.Entity;
using DAL.Configuration;
using DAL.Initializers;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext: DbContext
    {
        public DbSet<Catalog> Catalogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(CatalogConfiguration).Assembly);

            base.OnModelCreating(builder);

            CatalogDataInitializer.SeedData(builder);
        }
    }
}