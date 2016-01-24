namespace Poker.Contracts
{
    using System.Drawing;
    using Enums;

    public interface ICard
    {
        Rank Rank { get; }

        Suit Suit { get; }

        Image CardFront { get; }

        Image CardBack { get; }
    }
}
