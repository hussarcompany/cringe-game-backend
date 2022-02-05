using CringeGame.Models.Abstractions;

namespace CringeGame.Models;

public class StatementCard: ICard
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public string Statement { get; set; }
}