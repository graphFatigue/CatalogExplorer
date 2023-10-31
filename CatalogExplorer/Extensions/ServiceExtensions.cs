using DAL;
using Microsoft.EntityFrameworkCore;
using System;

namespace CatalogExplorer.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services)
        {

            var anotherString =
                "Data Source=DESKTOP-G9BFQFQ\\SQLEXPRESS;Initial Catalog=CatalogExplorer; Encrypt=False;Integrated Security=True;";

            services.AddDbContext<AppDbContext>(opts =>
                opts
                    .UseLazyLoadingProxies()
                    .UseSqlServer(anotherString));
        }
    }
    }
