using CringeGame.Models.Abstractions;

namespace CringeGame.Models;

public class MemeCard: ICard
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public string Uri { get; set; }
}