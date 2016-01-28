namespace Poker.Models
{
    using Contracts;
    using System.Collections.Generic;

    public class Hand : ICardCollection
    {
        public IEnumerable<ICard> Cards
        {
            get;
        }
    }
}