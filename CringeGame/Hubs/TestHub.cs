using CringeGame.Models;
using Microsoft.AspNetCore.SignalR;

namespace CringeGame.Hubs;

public class TestHub: Hub
{
    private readonly IGame _game;
    
    public TestHub(IGame game)
    {
        _game = game;
    }
    
    public async Task TestSBolshoiBukvi(string test)
    {
        await Clients.All.SendAsync("PoluchiVEbalo", test);
    }

    public async Task NewPlayer(string playerId)
    {
        
    }
}