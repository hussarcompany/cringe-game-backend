using CringeGame.Hubs.Dto.RoomHub;
using Microsoft.OpenApi.Models;
using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;

namespace CringeGame.Hubs.Abstractions.ClientHubs;

[SignalRHub("*")]
public interface IRoomClientHub
{
    /// <summary>
    /// Хук, сообщающий пользователям в комнате о старте игры.
    /// </summary>
    /// <param name="roomGameDto">Информация об игре.</param>
    /// <returns></returns>
    [SignalRMethod($"{nameof(RoomHub)}_{nameof(StartGame)}", OperationType.Post, AutoDiscover.Args)]
    Task StartGame(RoomGameDto roomGameDto);

    /// <summary>
    /// Хук, сообщающий пользователям в комнате об ее изменении.
    /// </summary>
    /// <param name="roomDto">Измененная модель комнаты.</param>
    /// <returns></returns>
    [SignalRMethod($"{nameof(RoomHub)}_{nameof(UpdateRoom)}", OperationType.Post, AutoDiscover.Args)]
    Task UpdateRoom(RoomDto roomDto);
}