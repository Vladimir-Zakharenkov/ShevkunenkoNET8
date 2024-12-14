#region Default

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.Run();


#endregion

#region Listing 7.5 Configuring the application in the Program.cs file in the SportsStore folder

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews();

//var app = builder.Build();

////app.MapGet("/", () => "Hello World!");

//app.UseStaticFiles();

//app.MapDefaultControllerRoute();

//app.Run();

#endregion

#region Listing 7.17 Configuring Entity Framework Core in the Program.cs file in the SportsStore folder

//using Microsoft.EntityFrameworkCore;
//using SportsStore.Models;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<StoreDbContext>(opts =>
//{
//    opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
//});

//var app = builder.Build();

//app.UseStaticFiles();

//app.MapDefaultControllerRoute();

//app.Run();

#endregion

#region Listing 7.20 Creating the repository service in the Program.cs file in the SportsStore folder

//using Microsoft.EntityFrameworkCore;
//using SportsStore.Models;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<StoreDbContext>(opts =>
//{
//    opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
//});

//builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

//var app = builder.Build();

//app.UseStaticFiles();

//app.MapDefaultControllerRoute();

//app.Run();

#endregion

#region Listing 7.23 Seeding the database in the Program.cs file in the SportsStore folder

//using Microsoft.EntityFrameworkCore;
//using SportsStore.Models;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<StoreDbContext>(opts =>
//{
//    opts.UseSqlServer(
//    builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
//});

//builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

//var app = builder.Build();

//app.UseStaticFiles();

//app.MapDefaultControllerRoute();

//SeedData.EnsurePopulated(app);

//app.Run();

#endregion

#region Listing 7.34 Adding a new route in the Program.cs file in the SportsStore folder

using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseSqlServer(
    builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute("pagination", "Products/Page{productPage}", new { Controller = "Home", action = "Index" });

app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);

app.Run();

#endregion