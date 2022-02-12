using CringeGame.Models.Abstractions;

namespace CringeGame.Models;

public class Game
{
    private Deck<MemeCard> _memeDeck;

    private readonly Deck<StatementCard> _statementDeck;

    public string Identifier => Id.ToString();
    
    public Guid Id { get; } = Guid.NewGuid();
    
    public List<Player> Players { get; }

    public uint CurrentRound { get; set; } = 1;

    public uint MaxRounds { get; } = 10;

    public GameState State { get; set; }

    public string Statement { get; private set; }

    public bool IsChangeState => Players.All(x => x.HasAction);

    public Game(List<Player> players)
    {
        Players = players;
        _memeDeck = Deck<MemeCard>.FromCollection(DeckContants.MemeCards);
        _statementDeck = Deck<StatementCard>.FromCollection(DeckContants.StatementCards);
    }

    private void Start()
    {
        _memeDeck.Shuffle();
        _statementDeck.Shuffle();
        foreach (var player in Players)
        {
            player.Hand = new List<MemeCard>
                { _memeDeck.GetNextCard(), _memeDeck.GetNextCard(), _memeDeck.GetNextCard(), _memeDeck.GetNextCard() };
        }
    }

    private void AddCardToPlayers()
    {
        foreach (var player in Players)
        {
            player.Hand =  new List<MemeCard>
                { _memeDeck.GetNextCard(), _memeDeck.GetNextCard(), _memeDeck.GetNextCard(), _memeDeck.GetNextCard() };
        }
    }

    public void ChangeState()
    {
        switch (State)
        {
            case GameState.Readying:
                Start();
                Statement = _statementDeck.GetNextCard().Statement;
                State = GameState.ChoosingMeme;
                break;
            case GameState.ChoosingMeme:
                State = GameState.Voting;
                break;
            case GameState.Voting:
                if (CurrentRound == MaxRounds)
                {
                    State = GameState.Ended;
                }
                else
                {
                    CurrentRound++;
                    _memeDeck = Deck<MemeCard>.FromCollection(DeckContants.MemeCards);
                    _memeDeck.Shuffle();
                    AddCardToPlayers();
                    Statement = _statementDeck.GetNextCard().Statement;
                    State = GameState.ChoosingMeme;
                    foreach (var player in Players)
                    {
                        player.SelectedCard = null;
                    }
                }

                break;
            case GameState.Ended:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}