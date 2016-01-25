namespace Poker.PokerUtilities
{
    using System;
    using Contracts;

    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random randomGenerator;

        public RandomGenerator()
        {
            this.randomGenerator = new Random();
        }

        public int Next(int maxValue)
        {
            int randomValue = this.randomGenerator.Next(maxValue);

            return randomValue;
        }

        public int Next(int minValue, int maxValue)
        {
            int randomValue = this.randomGenerator.Next(minValue, maxValue);

            return randomValue;
        }
    }
}
