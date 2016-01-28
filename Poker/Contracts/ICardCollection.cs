namespace Poker.Contracts
{
    using System.Collections.Generic;

    public interface ICardCollection
    {
        IEnumerable<ICard> Cards
        {
            get;
        }
    }
}