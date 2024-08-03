using Microsoft.AspNetCore.Authentication.Cookies;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

configuration.Bind("ProjectData", new DataConfig());

IWebHostEnvironment environment = builder.Environment;

#region Add services to the container

IServiceCollection services = builder.Services;

services.AddControllersWithViews();

services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaPage("Admin", "/Index");
    options.Conventions.AuthorizeAreaFolder("Admin", "/DBCRUD");
    options.Conventions.AddPageRoute("/Sitemap", "sitemap.xml");
    options.Conventions.AddPageRoute("/Robots", "robots.txt");
});

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Admin/Login");
    });

services.AddDbContext<SiteDbContext>(opts =>
{
    opts.UseSqlServer(configuration["ConnectionStrings:ShevkunenkoSite"]);

    if (environment.IsDevelopment())
    {
        opts.EnableSensitiveDataLogging(true);
    }
});

services.AddDatabaseDeveloperPageExceptionFilter();

services.Configure<RazorViewEngineOptions>(options =>
{
    options.AreaViewLocationFormats.Clear();
    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Views/Shared/Layouts/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Views/Shared/Components/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Views/Shared/Partials/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Pages/Shared/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Pages/Shared/Partials/{0}" + RazorViewEngine.ViewExtension);
});

#endregion

#region  Configure the HTTP request pipeline

WebApplication app = builder.Build();

//IConfiguration config = app.Configuration;

//IWebHostEnvironment env = app.Environment;

//if (env.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");

//    app.UseDeveloperExceptionPage();
//}

//SeedData.EnsurePopulated(app);

#endregion


app.MapGet("/", () => DataConfig.Test /*"Hello World!"*/);

app.Run();
