using CringeGame.Abstractions;
using CringeGame.Hubs.Abstractions.ClientHubs;
using CringeGame.Hubs.Abstractions.Hubs;
using CringeGame.Hubs.Dto.GameHub;
using SignalR.Modules;

namespace CringeGame.Hubs;

public class GameHub: ModuleHub<IGameClientHub>, IGameHub
{
    private readonly IGameManager _gameManager;
    
    public GameHub(IGameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public Task Vote(VoteDto voteDto)
    {
        throw new NotImplementedException();
    }

    public Task ChooseMeme(ChooseMemeDto chooseMemeDto)
    {
        throw new NotImplementedException();
    }
}