namespace Poker.Models
{
    using Contracts;
    using Enums;
    using System.Drawing;

    public class Card : ICard
    {
        private static readonly Image cardBack = new Bitmap(@"..\..\Resources\Assets\Back\Back.png");

        public Card(Rank rank, Suit suit, Image cardFront)
        {
            this.Rank = rank;
            this.Suit = suit;
            this.CardFront = cardFront;
        }

        public Rank Rank
        {
            get;
        }

        public Suit Suit
        {
            get;
        }

        public Image CardFront
        {
            get;
        }

        public Image CardBack => cardBack;
    }
}