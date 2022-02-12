namespace CringeGame.Hubs.Dto.GameHub;

public class GamePlayerDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public uint Points { get; set; }
    
    public MemeCardDto SelectedCard { get; set; }
}