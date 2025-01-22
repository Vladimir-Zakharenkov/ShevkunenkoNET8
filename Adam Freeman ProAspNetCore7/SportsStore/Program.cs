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

////app.UseStaticFiles();
//app.MapStaticAssets();

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

////app.UseStaticFiles();
//app.MapStaticAssets();

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

////app.UseStaticFiles();
//app.MapStaticAssets();

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

////app.UseStaticFiles();
//app.MapStaticAssets();

//app.MapDefaultControllerRoute();

//SeedData.EnsurePopulated(app);

//app.Run();

#endregion

#region Listing 7.34 Adding a new route in the Program.cs file in the SportsStore folder

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

////app.UseStaticFiles();
//app.MapStaticAssets();

//app.MapControllerRoute("pagination", "Products/Page{productPage}", new { Controller = "Home", action = "Index" });

//app.MapDefaultControllerRoute();

//SeedData.EnsurePopulated(app);

//app.Run();

#endregion

#region Listing 8.3 Changing the schema in the Program.cs file in the SportsStore folder

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
////app.MapStaticAssets();

//app.MapControllerRoute("catpage", "{category}/Page{productPage:int}", new { Controller = "Home", action = "Index" });

//app.MapControllerRoute("page", "Page{productPage:int}", new { Controller = "Home", action = "Index", productPage = 1 });

//app.MapControllerRoute("category", "{category}", new { Controller = "Home", action = "Index", productPage = 1 });

//app.MapControllerRoute("pagination", "Products/Page{productPage}", new { Controller = "Home", action = "Index", productPage = 1 });

//app.MapDefaultControllerRoute();

//SeedData.EnsurePopulated(app);

//app.Run();

#endregion

#region Listing 8.13 Enabling Razor Pages in the Program.cs file in the SportsStore folder

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

//builder.Services.AddRazorPages();

//var app = builder.Build();

//app.UseStaticFiles();
////app.MapStaticAssets();

//app.MapControllerRoute("catpage", "{category}/Page{productPage:int}", new { Controller = "Home", action = "Index" });

//app.MapControllerRoute("page", "Page{productPage:int}", new { Controller = "Home", action = "Index", productPage = 1 });

//app.MapControllerRoute("category", "{category}", new { Controller = "Home", action = "Index", productPage = 1 });

//app.MapControllerRoute("pagination", "Products/Page{productPage}", new { Controller = "Home", action = "Index", productPage = 1 });

//app.MapDefaultControllerRoute();

//app.MapRazorPages();

//SeedData.EnsurePopulated(app);

//app.Run();

#endregion

#region Listing 8.21 Enabling sessions in the Program.cs file in the SportsStore folder

//using Microsoft.EntityFrameworkCore;
//using SportsStore.Models;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<StoreDbContext>(opts =>
//{
//    opts.UseSqlServer(
//        builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
//});

//builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

//builder.Services.AddRazorPages();

//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession();

//var app = builder.Build();

//app.UseStaticFiles();

//app.UseSession();

//app.MapControllerRoute("catpage", "{category}/Page{productPage:int}", new { Controller = "Home", action = "Index" });

//app.MapControllerRoute("page", "Page{productPage:int}", new { Controller = "Home", action = "Index", productPage = 1 });

//app.MapControllerRoute("category", "{category}", new { Controller = "Home", action = "Index", productPage = 1 });

//app.MapControllerRoute("pagination", "Products/Page{productPage}", new { Controller = "Home", action = "Index", productPage = 1 });

//app.MapDefaultControllerRoute();

//app.MapRazorPages();

//SeedData.EnsurePopulated(app);

//app.Run();

#endregion

#region Listing 9.3 Creating the cart service in the Program.cs file in the SportsStore folder

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

builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

app.UseStaticFiles();

app.UseSession();

app.MapControllerRoute("catpage", "{category}/Page{productPage:int}", new { Controller = "Home", action = "Index" });

app.MapControllerRoute("page", "Page{productPage:int}", new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute("category", "{category}", new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute("pagination", "Products/Page{productPage}", new { Controller = "Home", action = "Index", productPage = 1 });

app.MapDefaultControllerRoute();

app.MapRazorPages();

SeedData.EnsurePopulated(app);

app.Run();

#endregion