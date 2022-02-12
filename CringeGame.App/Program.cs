using System.Reflection;
using System.Text.Json.Serialization;
using CringeGame.Abstractions;
using CringeGame.App;
using CringeGame.Core;
using CringeGame.Hubs;
using Microsoft.OpenApi.Models;
using Serilog;
using SignalR.Modules;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(int.Parse(port)));
builder.WebHost.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddSingleton<IGameManager, GameManager>();
builder.Services.AddSignalR(x => x.ClientTimeoutInterval = TimeSpan.FromMinutes(1));
builder.Services.AddSignalRModules<MainHub>();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("game-api", new OpenApiInfo { Title = "Cringe Game SignalR API", Version = "v1" });
    options.DocumentFilter<SignalRSwaggerGen.SignalRSwaggerGen>(new List<Assembly> { typeof(MainHub).Assembly, typeof(HubsAssemblyMarker).Assembly });
});

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors("MyPolicy");
app.MapHub<MainHub>("/game");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("game-api/swagger.json", "SignalR");
});
app.Run();