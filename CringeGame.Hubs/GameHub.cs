using AutoMapper;
using CringeGame.Abstractions;
using CringeGame.Hubs.Abstractions.ClientHubs;
using CringeGame.Hubs.Abstractions.Hubs;
using CringeGame.Hubs.Dto.GameHub;
using CringeGame.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalR.Modules;
using GameDto = CringeGame.Hubs.Dto.GameHub.GameDto;

namespace CringeGame.Hubs;

public class GameHub: ModuleHub<IGameClientHub>, IGameHub
{
    private static readonly SemaphoreSlim ReadySemaphore = new(1);
    
    private readonly IGameManager _gameManager;

    private readonly IMapper _mapper;

    private readonly ILogger _logger;

    public GameHub(ILogger<GameHub> logger, IGameManager gameManager, IMapper mapper)
    {
        _gameManager = gameManager;
        _mapper = mapper;
        _logger = logger;
    }

    private async Task TryChangeState(Game game)
    {
        if (game.IsChangeState)
        {
            _logger.LogInformation("Начата отправка состояния игры пользователям.");
            foreach (var gamePlayer in game.Players)
            {
                gamePlayer.HasAction = false;
            }
                
            game.ChangeState();
            var gameDto = _mapper.Map<GameDto>(game);
            foreach (var player in game.Players)
            {
                gameDto.Hand = player.Hand.Select(x => _mapper.Map<MemeCardDto>(x)).ToList();
                await Clients.Client(player.ConnectionId).UpdateGameState(gameDto);
            }
            _logger.LogInformation("Отправка состояния игры пользователям прошла успешно {@GameState}.", gameDto);
        }
    }

    public async Task Ready(ReadyDto readyDto)
    {
        await ReadySemaphore.WaitAsync();
        try
        {
            var game = _gameManager.GetGame(readyDto.GameId);
            if (game.State != GameState.Readying)
            {
                throw new HubException($"Ошибочная стадия {game.State}");
            }
            
            if (game.Players.All(x => x.ConnectionId != Context.ConnectionId))
            {
                throw new HubException("Игрока нет в текущей игре");
            }

            var player = game.Players.First(x => x.ConnectionId == Context.ConnectionId);
            if (player.HasAction)
            {
                throw new HubException("Игрок уже голосовал.");
            }
            
            player.HasAction = true;
            await TryChangeState(game);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при обработке Готово.");
            throw new HubException(e.Message, e);
        }
        finally
        {
            ReadySemaphore.Release();
        }
    }

    public async Task Vote(VoteDto voteDto)
    {
        await ReadySemaphore.WaitAsync();
        try
        {
            var game = _gameManager.GetGame(voteDto.GameId);
            if (game.State != GameState.Voting)
            {
                throw new HubException($"Нельзя голосовать во время стадии {game.State}");
            }

            if (game.Players.All(x => x.ConnectionId != Context.ConnectionId))
            {
                throw new HubException("Игрока нет в текущей игре");
            }

            if (game.Players.All(x => x.Id != voteDto.UserId))
            {
                throw new HubException("Игрока за которого голосуют нет в текущей игре");
            }

            if (Context.ConnectionId == game.Players.First(x => x.Id == voteDto.UserId).ConnectionId)
            {
                throw new HubException("Нельзя голосовать за себя");
            }

            var votingPlayer = game.Players.First(x => x.ConnectionId == Context.ConnectionId);
            var player = game.Players.First(x => x.Id == voteDto.UserId);
            if (votingPlayer.HasAction)
            {
                throw new HubException("Игрок уже голосовал.");
            }
            
            votingPlayer.HasAction = true;
            player.Points++;
            await TryChangeState(game);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при попытке проголосовать");
            throw new HubException(e.Message, e);
        }
        finally
        {
            ReadySemaphore.Release();
        }
    }

    public async Task ChooseMeme(ChooseMemeDto chooseMemeDto)
    {
        await ReadySemaphore.WaitAsync();
        try
        {
            var game = _gameManager.GetGame(chooseMemeDto.GameId);
            if (game.State != GameState.ChoosingMeme)
            {
                throw new HubException($"Нельзя выбирать мем во время стадии {game.State}");
            }

            if (game.Players.All(x => x.ConnectionId != Context.ConnectionId))
            {
                throw new HubException("Игрока нет в текущей игре");
            }

            var player = game.Players.First(x => x.ConnectionId == Context.ConnectionId);
            if (player.HasAction)
            {
                throw new HubException("Игрок уже выбирал карту.");
            }
            
            player.HasAction = true;
            if (player.Hand.All(x => x.Id != chooseMemeDto.MemeCardId))
            {
                throw new HubException("У игрока нет такой карты.");
            }

            var card = player.Hand.First(x => x.Id == chooseMemeDto.MemeCardId);
            player.SelectedCard = card;
            player.Hand.Remove(card);

            await TryChangeState(game);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при выборе мема");
            throw new HubException(e.Message, e);
        }
        finally
        {
            ReadySemaphore.Release();
        }
    }
}