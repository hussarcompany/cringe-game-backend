using CringeGame.Abstractions;
using CringeGame.Models;

namespace CringeGame.Core;

public class GameManager: IGameManager
{
    private readonly List<Game> _games = new();

    public Game CreateGameFor(List<Player> players)
    {
        var game = new Game(players);
        _games.Add(game);
        return game;
    }

    public Game GetGame(Guid gameId)
    {
        var game = _games.FirstOrDefault(x => x.Id == gameId);
        if (game == null)
        {
            throw new Exception($"Не найдена игра {gameId}");
        }
        
        return game;
    }
}