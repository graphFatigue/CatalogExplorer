using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Initializers
{
    internal class CatalogDataInitializer
    {
        internal static void SeedData(ModelBuilder builder)
        {
            builder.Entity<Catalog>().HasData
            (
                new Catalog()
                {
                    Id = 1,
                    Name = "Creating Digital Images",
                },
                new Catalog()
                {
                    Id = 2,
                    Name = "Resources",
                    ParentCatalogId = 1,
                },
                new Catalog()
                {
                    Id = 3,
                    Name = "Evidence",
                    ParentCatalogId = 1,
                },
                new Catalog()
                {
                    Id = 4,
                    Name = "Graphic Products",
                    ParentCatalogId = 1,
                },
                new Catalog()
                {
                    Id = 5,
                    Name = "Primary Sources",
                    ParentCatalogId = 2,
                },
                new Catalog()
                {
                    Id = 6,
                    Name = "Secondary Sources",
                    ParentCatalogId = 2,
                },
                new Catalog()
                {
                    Id = 7,
                    Name = "Process",
                    ParentCatalogId = 4,
                },
                new Catalog()
                {
                    Id = 8,
                    Name = "Final Product",
                    ParentCatalogId = 4,
                });
        }
    }
}
