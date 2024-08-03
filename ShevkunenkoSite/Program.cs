WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

configuration.Bind("ProjectData", new DataConfig());

IWebHostEnvironment environment = builder.Environment;

#region Add services to the container

IServiceCollection services = builder.Services;

services.AddDbContext<SiteDbContext>(opts =>
{
    opts.UseSqlServer(configuration["ConnectionStrings:ShevkunenkoSite"]);
    opts.EnableSensitiveDataLogging(true);
});

//services.AddDatabaseDeveloperPageExceptionFilter();

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
