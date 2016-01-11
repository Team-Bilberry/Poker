namespace Poker.Contracts
{
    using System.Drawing;

    public interface ICard
    {
        int Value { get; }

        string Suit { get; }

        Image Image { get; }
    }
}
