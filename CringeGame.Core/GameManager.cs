using CringeGame.Abstractions;
using CringeGame.Models;

namespace CringeGame.Core;

public class GameManager: IGameManager
{
    private readonly List<Game> _games = new();

    public Game CreateGameFor(List<Player> players)
    {
        var game = new Game
        {
            Players = players
        };
        _games.Add(game);
        return game;
    }

    public Game GetGame(Guid gameId)
    {
        return _games.FirstOrDefault(x => x.Id == gameId);
    }
}