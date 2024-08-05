using ShevkunenkoSite;

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

services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
    options.AppendTrailingSlash = true;
});

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

#region Работа с базой данных

services.AddScoped<IPageInfoRepository, PageInfoImplementation>();
services.AddScoped<IBackgroundFotoRepository, BackGroundFotoImplementation>();
services.AddScoped<IIconFileRepository, IconFileImplementation>();
services.AddScoped<IImageFileRepository, ImageFileImplementation>();
services.AddScoped<IMovieFileRepository, MovieFileImplementation>();
services.AddScoped<IAccessRepository, AccessImplementation>();
services.AddScoped<ITopicMovieRepository, TopicMovieImplementation>();

#endregion

#endregion

#region  Configure the HTTP request pipeline

WebApplication app = builder.Build();

//IConfiguration config = app.Configuration;

//IWebHostEnvironment env = app.Environment;

if (environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseDeveloperExceptionPage();

    SeedData.EnsurePopulated(app);
}

app.UseStatusCodePagesWithReExecute("/Error{0}");

// The default HSTS value is 30 days. You may want to change this
// for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseWebMarkupMin();

app.MapRazorPages();

#endregion

app.MapGet("/", () => DataConfig.Test /*"Hello World!"*/);

app.Run();