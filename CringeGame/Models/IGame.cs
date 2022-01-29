namespace CringeGame.Models;

public interface IGame
{
    public GameState State { get; set; }
}