using CringeGame.Hubs.Dto.GameHub;

namespace CringeGame.Hubs.Abstractions.Hubs;

public interface IGameHub
{
    Task Ready(ReadyDto readyDto);
    
    Task Vote(VoteDto voteDto);

    Task ChooseMeme(ChooseMemeDto chooseMemeDto);
}