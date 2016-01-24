namespace Poker.Models
{
    using System.Drawing;
    using Contracts;
    using Enums;

    public class Card : ICard
    {
        private static Image cardBack = new Bitmap(@"..\..\Resources\Assets\Back\Back.png");

        public Card(Rank rank, Suit suit, Image cardFront)
        {
            this.Rank = rank;
            this.Suit = suit;
            this.CardFront = cardFront;
        }

        public Rank Rank { get; }

        public Suit Suit { get; }

        public Image CardFront { get; }

        public Image CardBack => cardBack;
    }
}
