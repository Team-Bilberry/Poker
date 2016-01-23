namespace Poker.PokerUtilities
{
    using System;
    using Contracts;

    public class RandomProvider : IRandomProvider
    {
        private readonly Random randomGenerator;

        public RandomProvider()
        {
            this.randomGenerator = new Random();
        }

        public int Next(int maxValue)
        {
            int randomValue = this.randomGenerator.Next(maxValue);

            return randomValue;
        }
    }
}
