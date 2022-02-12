// See https://aka.ms/new-console-template for more information

using CringeGame.Hubs.Dto.GameHub;
using CringeGame.Hubs.Dto.RoomHub;
using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Hello, World!");
var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:8080/game")
    .Build();
var gameId = Guid.Empty;
connection.On("RoomHub_StartGame", (RoomGameDto param) =>
{
    gameId = param.Id;
});
connection.On("GameHub_UpdateGameState", (GameDto param) =>
{
    Console.WriteLine("End shit");
});
await connection.StartAsync();

var room = await connection.InvokeAsync<RoomDto>("RoomHub_CreateRoom", new CreateRoomDto
{
    Name = "TEST",
    PlayersCount = 1
});

await connection.InvokeAsync("RoomHub_JoinRoom", new JoinRoomDto
{
    RoomId = room.Id,
    UserName = "GOVNO"
});

await Task.Delay(1000);

await connection.InvokeAsync("GameHub_Ready", new ReadyDto
{
    GameId = gameId
});

await Task.Delay(5000);


Console.WriteLine("End");