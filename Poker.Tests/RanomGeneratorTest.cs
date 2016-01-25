namespace Poker.Tests
{
    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PokerUtilities;

    [TestClass]
    public class RanomGeneratorTest
    {
        private IRandomGenerator random;

        [TestInitialize]
        public void RanomProviderInit()
        {
            this.random = new RandomGenerator();
        }

        [TestMethod]
        public void Next_WhitMaxValue_ShouldReturnValueInRangeZeroMaxValue()
        {
            bool isInRange = true;
            const int maxValue = 10000;

            for (int i = 0; i < maxValue; i++)
            {
                int num = this.random.Next(maxValue);
                if (0 > num || num > maxValue - 1)
                {
                    isInRange = false;
                }

                Assert.IsTrue(isInRange);
            }
        }

        [TestMethod]
        public void Next_WhitMinMaxValue_ShouldReturnValueInRangeMinMaxValue()
        {
            bool isInRange = true;
            const int minValue = 100;
            const int maxValue = 10000;

            for (int i = 0; i < maxValue; i++)
            {
                int num = this.random.Next(minValue, maxValue);
                if (minValue > num || num > maxValue - 1)
                {
                    isInRange = false;
                }

                Assert.IsTrue(isInRange);
            }
        }
    }
}