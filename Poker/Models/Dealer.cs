namespace Poker.Models
{
    using System;
    using Contracts;

    public class Dealer : IDealer
    {
        public string[] ShuffleDeck(string[] deck)
        {
            if (deck == null || deck.Length == 0)
            {
                throw new InvalidOperationException("Cannot shuffle empty deck.");
            }

            var randomIndex = new Random();
            for (int currentIndex = deck.Length; currentIndex > 0; currentIndex--)
            {
                int swapCardIndex = randomIndex.Next(currentIndex);
                string tempCard = deck[swapCardIndex];
                deck[swapCardIndex] = deck[currentIndex - 1];
                deck[currentIndex - 1] = tempCard;
            }

            return deck;
        }

        public void DealCards(ICardCollection deck)
        {
            throw new System.NotImplementedException();
        }
    }
}
