using CringeGame.Hubs;
using SignalR.Modules;
using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;

namespace CringeGame.App;

[SignalRModuleHub(typeof(RoomHub))]
[SignalRModuleHub(typeof(GameHub))]
[SignalRHub(path: "/game", autoDiscover: AutoDiscover.MethodsAndArgs)]
public partial class MainHub: ModulesEntryHub
{
    public MainHub(ILogger<MainHub> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
    {
    }
    
    [SignalRHidden]
    public override async Task OnConnectedAsync()
    {
        await ModuleHubsOnConnectedAsync();
        Logger.LogInformation("Подключился пользователь {ConnectionId}", Context.ConnectionId);
    }
    
    [SignalRHidden]
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await ModuleHubsOnDisconnectedAsync(exception);
        Logger.LogInformation("Отключился пользователь {ConnectionId}", Context.ConnectionId);
    }
    
}