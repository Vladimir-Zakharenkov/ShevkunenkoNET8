#region Example 01

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//// установка настроек конфигурации
//app.Configuration["name"] = "Tom";
//app.Configuration["age"] = "37";

//app.Run(async (context) =>
//{
//    // получение настроек конфигураци
//    string? name = app.Configuration["name"];
//    string? age = app.Configuration["age"];

//    await context.Response.WriteAsync($"{name} - {age}");
//});

//app.Run();

#endregion

#region Example02

//var builder = WebApplication.CreateBuilder();

//var dictationary = new Dictionary<string, string>
//{
//    {"name", "Tom"},
//    {"age", "37"}
//};

//builder.Configuration.AddInMemoryCollection(dictationary!);

//var app = builder.Build();

//app.Run(async (context) =>
//{
//    // получение настроек конфигураци
//    string? name = app.Configuration["name"];
//    string? age = app.Configuration["age"];

//    await context.Response.WriteAsync($"{name} - {age}");
//});

//app.Run();

#endregion

#region Example03

var builder = WebApplication.CreateBuilder();

var app = builder.Build();

// установка настроек конфигурации
app.Configuration["name"] = "Tom";
app.Configuration["age"] = "37";

// через механизм внедрения зависимостей получим сервис IConfiguration
app.Map("/", (IConfiguration appConfig) => $"{appConfig["name"]} - {appConfig["age"]}");

app.Run();

#endregion