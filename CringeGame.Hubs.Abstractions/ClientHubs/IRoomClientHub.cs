using CringeGame.Hubs.Dto.RoomHub;
using Microsoft.OpenApi.Models;
using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;

namespace CringeGame.Hubs.Abstractions.ClientHubs;

[SignalRHub("*")]
public interface IRoomClientHub
{
    [SignalRMethod("RoomHub_GameStart", OperationType.Post, AutoDiscover.Args)]
    Task GameStart(GameStartDto gameStartDto);
}