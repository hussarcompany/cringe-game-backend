using CringeGame.Hubs.Dto.GameHub;

namespace CringeGame.Hubs.Abstractions.Hubs;

public interface IGameHub
{
    Task Vote(VoteDto voteDto);

    Task ChooseMeme(ChooseMemeDto chooseMemeDto);
}