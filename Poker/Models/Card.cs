namespace Poker.Models
{
    using System.Drawing;
    using Contracts;
    using Enums;

    public class Card : ICard
    {
        public Card(Rank rank, Suit suit, Image image)
        {
            this.Rank = rank;
            this.Suit = suit;
            this.Image = image;
        }

        public Rank Rank { get; }

        public Suit Suit { get; }

        public Image Image { get; }
    }
}
