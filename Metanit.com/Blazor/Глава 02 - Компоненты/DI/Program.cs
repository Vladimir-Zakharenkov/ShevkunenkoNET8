using DI.Components;
using DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// добавляем сервисы
builder.Services.AddTransient<ITimeService, TimeService>();
builder.Services.AddTransient<TimeFormatter>();

// выводим все сервисы на консоль
foreach (var service in builder.Services)
{
    Console.WriteLine(service.ServiceType);
}

var app = builder.Build();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();