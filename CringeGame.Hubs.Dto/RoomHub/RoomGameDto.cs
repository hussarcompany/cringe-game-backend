namespace CringeGame.Hubs.Dto.RoomHub;

public class RoomGameDto
{
    public Guid Id { get; set; }
    
    public List<PlayerDto> Players { get; set; }
}