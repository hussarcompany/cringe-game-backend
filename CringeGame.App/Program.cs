using System.Reflection;
using CringeGame.Abstractions;
using CringeGame.App;
using CringeGame.Core;
using CringeGame.Hubs;
using CringeGame.Hubs.Abstractions.ClientHubs;
using Microsoft.OpenApi.Models;
using Serilog;
using SignalR.Modules;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(int.Parse(port)));
builder.WebHost.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddSingleton<IGameManager, GameManager>();
builder.Services.AddSignalR();
builder.Services.AddSignalRModules<MainHub>();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("game-api", new OpenApiInfo { Title = "Cringe Game SignalR API", Version = "v1" });
    options.DocumentFilter<SignalRSwaggerGen.SignalRSwaggerGen>(new List<Assembly> { typeof(MainHub).Assembly, typeof(IGameClientHub).Assembly });
});

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors("MyPolicy");
app.MapHub<TestHub>("/test");
app.MapHub<MainHub>("/game");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("game-api/swagger.json", "SignalR");
});
app.Run();