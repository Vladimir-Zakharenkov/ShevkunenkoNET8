using AddBlazorToAspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();  // добавляем сервисы для серверного рендеринга

var app = builder.Build();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();  // добавляем интерактивный рендеринг сервера 

app.MapGet("/", () => "Hello World!");

app.Run();