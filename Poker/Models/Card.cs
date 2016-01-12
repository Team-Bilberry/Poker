namespace Poker.Models
{
    using System.Drawing;
    using Contracts;
    using Enums;

    public class Card : ICard
    {
        public Card(int value, Suit suit, Image image)
        {
            this.Value = value;
            this.Suit = suit;
            this.Image = image;
        }

        public int Value { get; }

        public Suit Suit { get; }

        public Image Image { get; }
    }
}
