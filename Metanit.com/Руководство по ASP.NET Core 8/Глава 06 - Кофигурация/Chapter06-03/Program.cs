#region Example01

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("config.json");

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["person"]} - {appConfig["company"]}");

//app.Run();

#endregion

#region Example02

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

builder.Configuration.AddJsonFile("config2.json");

app.Map("/", (IConfiguration appConfig) =>
{
    var personName = appConfig["person:profile:name"];
    var companyName = appConfig["company:name"];

    return $"{personName} - {companyName}";
});

app.Run();

#endregion