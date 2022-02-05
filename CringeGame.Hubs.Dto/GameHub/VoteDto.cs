namespace CringeGame.Hubs.Dto.GameHub;

public class VoteDto
{
    public Guid GameId { get; set; }
    
    public string ConnectionId { get; set; }
}