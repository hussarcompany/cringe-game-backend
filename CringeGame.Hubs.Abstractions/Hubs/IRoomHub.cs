using CringeGame.Hubs.Dto.RoomHub;

namespace CringeGame.Hubs.Abstractions.Hubs;

public interface IRoomHub
{
    Task<GetRoomDto> CreateRoom(CreateRoomDto createRoomDto);

    Task JoinRoom(JoinRoomDto joinRoomDto);

    Task LeaveRoom(LeaveRoomDto leaveRoomDto);

    Task<List<GetRoomDto>> GetRooms();
}