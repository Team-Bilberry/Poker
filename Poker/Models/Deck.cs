namespace Poker.Models
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Contracts;
    using Enums;
    using GUI;

    public class Deck : ICardCollection
    {
        private const string CardsPath = @"..\..\Resources\Cards";
        private readonly IDictionary<int, ICard> deck;
        private readonly PictureBox[] holder;
        private readonly PokerTable table;

        public Deck(PokerTable table)
        {
            this.deck = new Dictionary<int, ICard>();
            this.holder = new PictureBox[52];
            this.table = table;
            this.InitializeDeck();
        }

        public IEnumerable<ICard> Cards { get; }

        private IEnumerable<PictureBox> InitializeDeck()
        {
            int number = 0;
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    var cardName = rank + "_of_" + suit;
                    var image = Image.FromFile(CardsPath + "\\" + cardName + ".png");
                    ICard card = new Card(rank, suit, image);
                    this.deck.Add(number + 1, card);
                    this.holder[number] = new PictureBox();
                    this.holder[number].Image = image;

                    this.holder[number].SizeMode = PictureBoxSizeMode.StretchImage;
                    this.holder[number].Height = 130;
                    this.holder[number].Width = 80;

                    this.table.Controls.Add(this.holder[number]);
                    this.holder[number].Name = "pb" + number;
                    number++;
                }
            }

            return this.holder;
        }
    }
}
