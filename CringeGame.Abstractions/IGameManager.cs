using CringeGame.Models;

namespace CringeGame.Abstractions;

public interface IGameManager
{
    Game CreateGameFor(List<Player> players);

    Game GetGame(Guid gameId);
}