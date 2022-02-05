using CringeGame.Models.Abstractions;

namespace CringeGame.Models;

public class Game
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public Deck<MemeCard> MemeDeck { get; set; }
    
    public Deck<StatementCard> StatementDeck { get; set; }
    
    public List<Player> Players { get; set; }
}