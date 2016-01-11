namespace Poker.Models
{
    using System.Drawing;
    using Contracts;

    public class Card : ICard
    {
        public Card(int value, string suit, Image image)
        {
            this.Value = value;
            this.Suit = suit;
            this.Image = image;
        }

        public int Value { get; }

        public string Suit { get; }

        public Image Image { get; }
    }
}
