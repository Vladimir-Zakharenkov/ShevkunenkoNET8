#region Example01 ������������ ����� json

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("config.json");

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["person"]} - {appConfig["company"]}");

//app.Run();

#endregion

#region Example02 ������������ ����� json

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("config2.json");

//app.Map("/", (IConfiguration appConfig) =>
//{
//    var personName = appConfig["person:profile:name"];
//    var companyName = appConfig["company:name"];

//    return $"{personName} - {companyName}";
//});

//app.Run();

#endregion

#region Example03 ������������ ����� xml

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddXmlFile("config.xml");

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["person"]} - {appConfig["company"]}");

//app.Run();

#endregion

#region Example04 ������������ ����� xml

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddXmlFile("config2.xml");

//app.Map("/", (IConfiguration appConfig) =>
//{
//    var personName = appConfig["person:profile:name"];
//    var companyName = appConfig["company:name"];

//    return $"{personName} - {companyName}";
//});

//app.Run();

#endregion

#region Example05 ������������ ����� ini

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

builder.Configuration.AddIniFile("config.ini");

app.Map("/", (IConfiguration appConfig) => $"{appConfig["person"]} - {appConfig["company"]}");

app.Run();

#endregion