var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseWelcomePage();   // ����������� WelcomePageMiddleware

app.Run();
