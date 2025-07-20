using ShevkunenkoSite.Views.Shared.Components.Blazor;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Configuration

ConfigurationManager configuration = builder.Configuration;

configuration.AddJsonFile("DataConfig.json");

configuration.Bind("ProjectData", new DataConfig());

IWebHostEnvironment environment = builder.Environment;

#endregion

#region Add services to the container

IServiceCollection services = builder.Services;

#region Работа с Blazor

builder.Services.AddServerSideBlazor();

builder.Services.AddRazorComponents();

#endregion

#region Работа с MVC

services.AddControllersWithViews();

#endregion

#region Работа с RazorPages

services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaPage("Admin", "/Index");
    options.Conventions.AuthorizeAreaFolder("Admin", "/DBCRUD");
    options.Conventions.AddPageRoute("/Sitemap", "sitemap.xml");
    options.Conventions.AddPageRoute("/Robots", "robots.txt");
});

#endregion

#region Подключить аутентификацию

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Admin/Login");
    });

#endregion

#region Указать пути поиска файлов

services.Configure<RazorViewEngineOptions>(options =>
{
    options.PageViewLocationFormats.Add("/Views/Shared/Partials/{0}" + RazorViewEngine.ViewExtension);
    options.PageViewLocationFormats.Add("/Views/Shared/Layouts/{0}" + RazorViewEngine.ViewExtension);
    options.PageViewLocationFormats.Add("/Views/Shared/Components/{0}" + RazorViewEngine.ViewExtension);
    options.PageViewLocationFormats.Add("/Views/Shared/Components/Blazor/{0}" + RazorViewEngine.ViewExtension);

    options.AreaPageViewLocationFormats.Add("/Pages/Shared/Partials/{0}" + RazorViewEngine.ViewExtension);
    options.AreaPageViewLocationFormats.Add("/Views/Shared/Layouts/{0}" + RazorViewEngine.ViewExtension);
    options.AreaPageViewLocationFormats.Add("/Views/Shared/Partials/{0}" + RazorViewEngine.ViewExtension);

    options.ViewLocationFormats.Add("/Pages/Shared/Partials/{0}" + RazorViewEngine.ViewExtension);
    options.ViewLocationFormats.Add("/Views/Shared/Partials/{0}" + RazorViewEngine.ViewExtension);
    options.ViewLocationFormats.Add("/Views/Shared/Components/{0}" + RazorViewEngine.ViewExtension);
    options.ViewLocationFormats.Add("/Views/Shared/Components/Blazor/{0}" + RazorViewEngine.ViewExtension);
    options.ViewLocationFormats.Add("/Views/Shared/Layouts/{0}" + RazorViewEngine.ViewExtension);

    options.AreaViewLocationFormats.Clear();
    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Views/Shared/Layouts/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Views/Shared/Components/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Pages/Shared/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Pages/Shared/Partials/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/Views/Shared/Partials/{0}" + RazorViewEngine.ViewExtension);
});

#endregion

#region Кодирование в Unicode

services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

#endregion

#region RouteOptions

services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
    options.AppendTrailingSlash = true;
});

#endregion

#region MemoryCache

builder.Services.AddDistributedMemoryCache();

#endregion

#region AddSession

builder.Services.AddSession();

#endregion

#region AddWebMarkupMin

services.AddWebMarkupMin(
        options =>
        {
            options.AllowMinificationInDevelopmentEnvironment = false;
            options.AllowCompressionInDevelopmentEnvironment = false;
        })
        .AddHtmlMinification(
            options =>
            {
                options.MinificationSettings.RemoveRedundantAttributes = true;
                options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
            })
        //.AddXmlMinification()
        .AddHttpCompression();

#endregion

#region Работа с базой данных

services.AddDbContext<SiteDbContext>(opts =>
{
    opts.UseSqlServer(configuration["ConnectionStrings:ShevkunenkoSite"]);

    if (environment.IsDevelopment())
    {
        opts.EnableSensitiveDataLogging(true);
    }
});

services.AddDatabaseDeveloperPageExceptionFilter();

services.AddScoped<IPageInfoRepository, PageInfoImplementation>();
services.AddScoped<IBackgroundFotoRepository, BackGroundFotoImplementation>();
services.AddScoped<IIconFileRepository, IconFileImplementation>();
services.AddScoped<IImageFileRepository, ImageFileImplementation>();
services.AddScoped<IMovieFileRepository, MovieFileImplementation>();
services.AddScoped<IAccessRepository, AccessImplementation>();
services.AddScoped<ITopicMovieRepository, TopicMovieImplementation>();
services.AddScoped<ITextInfoRepository, TextInfoImplementation>();
services.AddScoped<IBooksAndArticlesRepository, BooksAndArticlesImplementation>();

#endregion

#endregion

#region  Configure the HTTP request pipeline

WebApplication app = builder.Build();

if (environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseDeveloperExceptionPage();
}

app.UseStatusCodePagesWithReExecute("/Shevkunenko/Error{0}");

// The default HSTS value is 30 days. You may want to change this
// for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseWebMarkupMin();

#region Маршруты

app.MapAreaControllerRoute(
        name: "videos_area",
        areaName: "videos",
        pattern: "videos/{controller=Номе}/{action=Index}/{id?}");

app.MapControllerRoute("pagination", "Video-na-saite/Stranica{pageNumber}",
    new { Controller = "AllVideo", action = "Index" });

app.MapControllerRoute("PhotoAlbum", "Shevkunenko/PhotoAlbum/Page{pageNumber}/Photo-{imageId}",
        new { Controller = "Shevkunenko", action = "PhotoAlbum" });

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area}/{controller=Shevkunenko}/{action=Index}/{id?}");

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Shevkunenko}/{action=Index}/{id?}");

app.MapRazorPages();

#endregion

#region Для запуска Blazor

app.UseAntiforgery();

app.MapBlazorHub();

app.MapFallbackToController("Blazor", "Shevkunenko");

app.MapRazorComponents<App>();

#endregion

#endregion

app.Run();