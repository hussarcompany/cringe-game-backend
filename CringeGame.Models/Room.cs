namespace CringeGame.Models;

public class Room
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public string Name { get; set; }
    
    public int PlayersCount { get; set; }
    
    public int PlayersCountCurrent { get; set; }

    public List<Player> Players { get; } = new();
}