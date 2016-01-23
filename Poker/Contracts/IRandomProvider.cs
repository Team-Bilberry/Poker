namespace Poker.Contracts
{
    public interface IRandomProvider
    {
        int Next(int maxValue);
    }
}
