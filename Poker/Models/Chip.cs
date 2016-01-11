namespace Poker.Models
{
    using Contracts;

    public class Chip : IChip
    {
        private int amount;

        public int Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                this.amount = value;
            }
        }

        public override string ToString()
        {
            string result = "Chips : " + this.Amount;
            return result;
        }
    }
}
