namespace Poker.Contracts
{
    using System.Drawing;
    using Enums;

    public interface ICard
    {
        int Value { get; }

        Suit Suit { get; }

        Image Image { get; }
    }
}
