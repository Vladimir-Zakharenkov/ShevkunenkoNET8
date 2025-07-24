using DI.Components;
using DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ��������� �������
builder.Services.AddTransient<ITimeService, TimeService>();
builder.Services.AddTransient<TimeFormatter>();

// ������� ��� ������� �� �������
foreach (var service in builder.Services)
{
    Console.WriteLine(service.ServiceType);
}

var app = builder.Build();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();