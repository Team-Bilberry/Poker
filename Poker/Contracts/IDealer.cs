namespace Poker.Contracts
{
    public interface IDealer
    {
        void ShufleDeck(ICardCollection deck);

        void DealCards(ICardCollection deck);
    }
}
