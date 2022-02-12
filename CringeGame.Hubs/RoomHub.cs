using AutoMapper;
using CringeGame.Abstractions;
using CringeGame.Hubs.Abstractions.ClientHubs;
using CringeGame.Hubs.Abstractions.Hubs;
using CringeGame.Hubs.Dto.RoomHub;
using CringeGame.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalR.Modules;

namespace CringeGame.Hubs;

public class RoomHub : ModuleHub<IRoomClientHub>, IRoomHub
{
    private static readonly List<Room> Rooms = new();

    private static readonly SemaphoreSlim CreateRoomSemaphore = new(1);

    private static readonly SemaphoreSlim JoinLeaveRoomSemaphore = new(1);

    private readonly IMapper _mapper;

    private readonly ILogger _logger;

    private readonly IGameManager _gameManager;

    public RoomHub(ILogger<RoomHub> logger, IMapper mapper, IGameManager gameManager)
    {
        _mapper = mapper;
        _logger = logger;
        _gameManager = gameManager;
    }

    public async Task<RoomDto> CreateRoom(CreateRoomDto createRoomDto)
    {
        await CreateRoomSemaphore.WaitAsync();
        try
        {
            _logger.LogInformation("Попытка создания комнаты пользователем {ConnectionId}", Context.ConnectionId);
            var room = _mapper.Map<Room>(createRoomDto);
            Rooms.Add(room);
            _logger.LogInformation("Создана комната {RoomId}, пользователем {ConnectionId}", room.Id,
                Context.ConnectionId);
            return _mapper.Map<RoomDto>(room);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при создании комнаты пользователем {ConnectionId}", Context.ConnectionId);
            throw;
        }
        finally
        {
            CreateRoomSemaphore.Release();
        }
    }

    public async Task<PlayerDto> JoinRoom(JoinRoomDto joinRoomDto)
    {
        await JoinLeaveRoomSemaphore.WaitAsync();
        try
        {
            _logger.LogInformation("Пользователь {ConnectionId} пытается зайти в комнату {RoomId}",
                Context.ConnectionId, joinRoomDto.RoomId);
            if (Rooms.Any(x => x.Players.Any(p => p.ConnectionId == Context.ConnectionId)))
            {
                throw new HubException($"Пользователь {Context.ConnectionId} уже есть в одной из комнат.");
            }
            
            var room = Rooms.FirstOrDefault(x => x.Id == joinRoomDto.RoomId);
            if (room == null || room.PlayersCountCurrent == room.PlayersCount)
            {
                throw new HubException("Не найдена комната, либо нет мест.");
            }

            if (room.Players.Any(x => x.ConnectionId == Context.ConnectionId))
            {
                throw new HubException("Пользователь уже находится в этой комнате.");
            }

            room.PlayersCountCurrent += 1;
            var player = new Player
            {
                ConnectionId = Context.ConnectionId,
                Name = joinRoomDto.UserName
            };
            room.Players.Add(player);
            await Clients.Clients(room.Players.Select(x => x.ConnectionId).ToList()).UpdateRoom(_mapper.Map<RoomDto>(room));
            if (room.PlayersCountCurrent == room.PlayersCount)
            {
                var game = _gameManager.CreateGameFor(room.Players);
                foreach (var roomPlayer in room.Players)
                {
                    await Groups.AddToGroupAsync(roomPlayer.ConnectionId, game.Identifier);
                }

                await Clients.Group(game.Identifier).StartGame(_mapper.Map<RoomGameDto>(game));
                Rooms.Remove(room);
            }

            _logger.LogInformation("Пользователь {ConnectionId} зашел в комнату {RoomId}", Context.ConnectionId,
                room.Id);
            return _mapper.Map<PlayerDto>(player);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при попытке зайти в комнату {RoomId}, пользователем {ConnectionId}",
                joinRoomDto.RoomId, Context.ConnectionId);
            throw;
        }
        finally
        {
            JoinLeaveRoomSemaphore.Release();
        }
    }

    public async Task LeaveRoom(LeaveRoomDto leaveRoomDto)
    {
        await JoinLeaveRoomSemaphore.WaitAsync();
        try
        {
            _logger.LogInformation("Пользователь {ConnectionId} пытается выйти из комнаты {RoomId}",
                Context.ConnectionId, leaveRoomDto.RoomId);
            var room = Rooms.FirstOrDefault(x => x.Id == leaveRoomDto.RoomId);
            var currentPlayer = room?.Players.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (currentPlayer == null)
            {
                throw new HubException("Не найдена комната, либо пользователя нет в комнате.");
            }

            room.PlayersCountCurrent -= 1;
            room.Players.Remove(currentPlayer);
            await Clients.Clients(room.Players.Select(x => x.ConnectionId).ToList()).UpdateRoom(_mapper.Map<RoomDto>(room));
            _logger.LogInformation("Пользователь {ConnectionId} вышел из комнаты {RoomId}", Context.ConnectionId,
                room.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при попытке выйти из комнаты {RoomId}, пользователем {ConnectionId}",
                leaveRoomDto.RoomId, Context.ConnectionId);
            throw;
        }
        finally
        {
            JoinLeaveRoomSemaphore.Release();
        }
    }

    public Task<List<RoomDto>> GetRooms()
    {
        return Task.FromResult(Rooms
            .Select(x => _mapper.Map<RoomDto>(x))
            .OrderByDescending(x => x.PlayersCountCurrent)
            .ToList()
        );
    }
}