using AutoMapper;
using CringeGame.Abstractions;
using CringeGame.Hubs.Abstractions.ClientHubs;
using CringeGame.Hubs.Abstractions.Hubs;
using CringeGame.Hubs.Dto.RoomHub;
using CringeGame.Models;
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

    public async Task<GetRoomDto> CreateRoom(CreateRoomDto createRoomDto)
    {
        try
        {
            _logger.LogInformation("Попытка создания комнаты пользователем {ConnectionId}", Context.ConnectionId);
            var room = _mapper.Map<Room>(createRoomDto);
            await CreateRoomSemaphore.WaitAsync();
            Rooms.Add(room);
            CreateRoomSemaphore.Release();
            _logger.LogInformation("Создана комната {RoomId}, пользователем {ConnectionId}", room.Id,
                Context.ConnectionId);
            return _mapper.Map<GetRoomDto>(room);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при создании комнаты пользователем {ConnectionId}", Context.ConnectionId);
            CreateRoomSemaphore.Release();
            throw;
        }
    }

    public async Task JoinRoom(JoinRoomDto joinRoomDto)
    {
        try
        {
            _logger.LogInformation("Пользователь {ConnectionId} пытается зайти в комнату {RoomId}",
                Context.ConnectionId, joinRoomDto.RoomId);
            await JoinLeaveRoomSemaphore.WaitAsync();
            var room = Rooms.FirstOrDefault(x => x.Id == joinRoomDto.RoomId);
            if (room == null || room.PlayersCountCurrent == room.PlayersCount)
            {
                throw new Exception("Не найдена комната, либо нет мест.");
            }

            if (room.Players.Any(x => x.ConnectionId == Context.ConnectionId))
            {
                throw new Exception("Пользователь уже находится в этой комнате.");
            }

            room.PlayersCountCurrent += 1;
            room.Players.Add(new Player
            {
                ConnectionId = Context.ConnectionId,
                Name = joinRoomDto.UserName
            });
            if (room.PlayersCountCurrent == room.PlayersCount)
            {
                var game = _gameManager.CreateGameFor(room.Players);
                foreach (var player in room.Players)
                {
                    await Groups.AddToGroupAsync(player.ConnectionId, game.Id.ToString());
                }
                
                await Clients.Group(game.Id.ToString()).GameStart(_mapper.Map<GameStartDto>(game));
            }

            JoinLeaveRoomSemaphore.Release();
            _logger.LogInformation("Пользователь {ConnectionId} зашел в комнату {RoomId}", Context.ConnectionId,
                room.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при попытке зайти в комнату {RoomId}, пользователем {ConnectionId}",
                joinRoomDto.RoomId, Context.ConnectionId);
            JoinLeaveRoomSemaphore.Release();
            throw;
        }
    }

    public async Task LeaveRoom(LeaveRoomDto leaveRoomDto)
    {
        try
        {
            _logger.LogInformation("Пользователь {ConnectionId} пытается выйти из комнаты {RoomId}",
                Context.ConnectionId, leaveRoomDto.RoomId);
            await JoinLeaveRoomSemaphore.WaitAsync();
            var room = Rooms.FirstOrDefault(x => x.Id == leaveRoomDto.RoomId);
            var currentPlayer = room?.Players.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (currentPlayer == null)
            {
                return;
            }

            room.PlayersCountCurrent -= 1;
            room.Players.Remove(currentPlayer);
            JoinLeaveRoomSemaphore.Release();
            _logger.LogInformation("Пользователь {ConnectionId} вышел из комнаты {RoomId}", Context.ConnectionId,
                room.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при попытке выйти из комнаты {RoomId}, пользователем {ConnectionId}",
                leaveRoomDto.RoomId, Context.ConnectionId);
            JoinLeaveRoomSemaphore.Release();
            throw;
        }
    }

    public Task<List<GetRoomDto>> GetRooms()
    {
        return Task.FromResult(Rooms
            .Select(x => _mapper.Map<GetRoomDto>(x))
            .OrderByDescending(x => x.PlayersCountCurrent)
            .ToList()
        );
    }
}