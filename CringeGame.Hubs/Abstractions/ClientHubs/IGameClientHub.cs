using CringeGame.Hubs.Dto.GameHub;
using Microsoft.OpenApi.Models;
using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;

namespace CringeGame.Hubs.Abstractions.ClientHubs;

[SignalRHub("*")]
public interface IGameClientHub
{
    [SignalRMethod($"{nameof(GameHub)}_{nameof(UpdateGameState)}", OperationType.Post, AutoDiscover.Args)]
    Task UpdateGameState(GameDto game);
}