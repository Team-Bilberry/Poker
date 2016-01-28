namespace Poker.Contracts
{
    using Enums;
    using System.Drawing;

    public interface ICard
    {
        Rank Rank
        {
            get;
        }

        Suit Suit
        {
            get;
        }

        Image CardFront
        {
            get;
        }

        Image CardBack
        {
            get;
        }
    }
}