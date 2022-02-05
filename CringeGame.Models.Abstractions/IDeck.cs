namespace CringeGame.Models.Abstractions;

public interface IDeck<out T> where T: ICard
{
    int Count { get; }
    
    int CurrentCount { get; }

    void Shuffle();

    T GetNextCard();
}