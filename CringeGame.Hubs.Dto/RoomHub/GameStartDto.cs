namespace CringeGame.Hubs.Dto.RoomHub;

public class GameStartDto
{
    public Guid Id { get; set; }
    
    public List<PlayerDto> Players { get; set; }
}