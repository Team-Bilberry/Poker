namespace Poker.Models
{
    using System.Collections.Generic;
    using Contracts;

    public class Deck : ICardCollection
    {
        public IEnumerable<ICard> Cards { get; }
    }
}
