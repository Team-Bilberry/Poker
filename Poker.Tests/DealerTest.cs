namespace Poker.Tests
{
    using System;
    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using PokerUtilities;

    [TestClass]
    public class DealerTest
    {
        private IDealer dealer;

        [TestInitialize]
        public void DealerInit()
        {
            this.dealer = new Dealer(new RandomGenerator());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Shuffle_NullDack_ShouldThrow()
        {
            this.dealer.ShuffleDeck(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Shuffle_EmptyDack_ShouldThrow()
        {
            string[] deck = new string[0];

            this.dealer.ShuffleDeck(deck);
        }

        [TestMethod]
        public void Shuffle_NonEmptyDack_ShouldReorder()
        {
            string[] deck = {"Two", "Thre", "Four", "Five", "Six", "Two", "Thre", "Four", "Five", "Six" };

            this.dealer.ShuffleDeck(deck);

            CollectionAssert.AreNotEqual(new[] { "Two", "Thre", "Four", "Five", "Six", "Two", "Thre", "Four", "Five", "Six" }, deck, "Deck is not shuffled");
        }
    }
}
