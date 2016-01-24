namespace Poker.Models
{
    using System.Collections.Generic;
    using Contracts;

    public class Hand : ICardCollection
    {
        public IEnumerable<ICard> Cards { get; }
    }
}
