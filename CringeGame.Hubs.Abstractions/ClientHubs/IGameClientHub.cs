using Microsoft.OpenApi.Models;
using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;

namespace CringeGame.Hubs.Abstractions.ClientHubs;

[SignalRHub("*")]
public interface IGameClientHub
{
    [SignalRMethod("GameHub_GameEnded", OperationType.Post, AutoDiscover.Args)]
    Task GameEnded();

    [SignalRMethod("GameHub_SendState", OperationType.Post, AutoDiscover.Args)]
    Task SendState();
}