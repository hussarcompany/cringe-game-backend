namespace CringeGame.Models;

public class Player
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public string ConnectionId { get; set; }

    public string Name { get; set; }
    
    public List<MemeCard> Hand { get; set; }
    
    public uint Points { get; set; }
    
    public bool HasAction { get; set; }
    
    public MemeCard SelectedCard { get; set; }
}