using Microsoft.AspNetCore.SignalR;

namespace CringeGame.Hubs;

public class TestHub: Hub
{
    public async Task TestSBolshoiBukvi(string test)
    {
        await Clients.All.SendAsync("PoluchiVEbalo", test);
    }
}