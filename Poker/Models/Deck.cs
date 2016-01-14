namespace Poker.Models
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Contracts;
    using Enums;

    public class Deck : ICardCollection
    {
        private const string CardsPath = @"..\..\Resources\Cards";
        private readonly Bitmap backImage = new Bitmap(@"..\..\Resources\Assets\Back\Back.png");
        private readonly IDictionary<int, ICard> deck;
        private PictureBox[] holder;

        public Deck()
        {
            this.deck = new Dictionary<int, ICard>();
            this.holder = new PictureBox[52];
            this.InitializeDeck();
        }

        public IEnumerable<ICard> Cards { get; }

        private void InitializeDeck()
        {
            int number = 1;

            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    var cardName = rank + "_of_" + suit;
                    var image = Image.FromFile(CardsPath + "\\" + cardName + ".png");
                    ICard card = new Card(rank, suit, image);
                    this.deck.Add(number, card);
                    number++;
                }
            }
        }
    }
}
