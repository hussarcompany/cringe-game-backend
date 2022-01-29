using CringeGame.Hubs;
using CringeGame.Models;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(int.Parse(port)));
builder.Services.AddSignalR();
builder.Services.AddSingleton<IGame, Game>();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
var app = builder.Build();

app.UseCors("MyPolicy");
app.MapHub<TestHub>("/test");
app.Run();