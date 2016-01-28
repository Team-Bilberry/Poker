namespace Poker.Contracts
{
    public interface IDealer
    {
        string[] ShuffleDeck(string[] deck);

        void DealCards(ICardCollection deck);
    }
}