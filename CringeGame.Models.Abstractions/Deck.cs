namespace CringeGame.Models.Abstractions;

public class Deck<T>: IDeck<T> where T: ICard
{
    private readonly Random _random = new();
    
    private Queue<T> _cards;

    public int Count { get; }

    public int CurrentCount => _cards.Count;

    private Deck(Queue<T> cards)
    {
        _cards = cards;
        Count = cards.Count;
    }

    public void Shuffle()
    {
        _cards = new Queue<T>(_cards.ToList().OrderBy(x => _random.Next()));
    }

    public T GetNextCard()
    {
        var card = _cards.Dequeue();
        return card;
    }

    public static Deck<T> FromCollection(IEnumerable<T> deck)
    {
        return new Deck<T>(new Queue<T>(deck));
    }
}