using BLL.Services;
using BLL.Services.Interfaces;
using CatalogExplorer.Extensions;
using Core.Entity;
using DAL.Extensions;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureSqlContext();
builder.Services.AddControllersWithViews();

//builder.Services.ConfigureDataAccessLayer();
builder.Services.AddScoped<IBaseRepository<Catalog>, CatalogRepository>();
builder.Services.AddScoped<ICatalogService, CatalogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
