using CringeGame.Hubs.Dto.RoomHub;

namespace CringeGame.Hubs.Abstractions.Hubs;

public interface IRoomHub
{
    Task<RoomDto> CreateRoom(CreateRoomDto createRoomDto);

    Task<PlayerDto> JoinRoom(JoinRoomDto joinRoomDto);

    Task LeaveRoom(LeaveRoomDto leaveRoomDto);

    Task<List<RoomDto>> GetRooms();
}