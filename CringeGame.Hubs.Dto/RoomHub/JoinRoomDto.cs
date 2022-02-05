namespace CringeGame.Hubs.Dto.RoomHub;

public class JoinRoomDto
{
    public Guid RoomId { get; set; }
    
    public string UserName { get; set; }
}