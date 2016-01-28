namespace Poker.Contracts
{
    using System.Collections.Generic;

    public interface ICheckHandType
    {
        void StraightFlush(
            IPokerPlayer pokerPlayer, 
            int[] clubes, 
            int[] dimonds, 
            int[] hearts, 
            int[] spades,
            ref List<Type> Win, 
            ref Type sorted);

        void FourOfAKind(
            IPokerPlayer pokerPlayer, 
            int[] Straight, 
            ref List<Type> Win, 
            ref Type sorted);

        void FullHouse(
            IPokerPlayer pokerPlayer, 
            int[] Straight, 
            ref List<Type> Win, 
            ref Type sorted);

        void Flush(
            IPokerPlayer pokerPlayer, 
            int[] Straight1,
            ref List<Type> Win, 
            ref Type sorted);

        void Straight(
            IPokerPlayer pokerPlayer, 
            int[] Straight, 
            ref List<Type> Win, 
            ref Type sorted);

        void ThreeOfAKind(
            IPokerPlayer pokerPlayer, 
            int[] Straight, 
            ref List<Type> Win, 
            ref Type sorted);

        void TwoPair(
            IPokerPlayer pokerPlayer, 
            int index, 
            ref List<Type> Win, 
            ref Type sorted, 
            ref int[] Reserve);

        void PairTwoPair(
            IPokerPlayer pokerPlayer, 
            int index, 
            ref List<Type> Win, 
            ref Type sorted, 
            ref int[] Reserve);

        void PairFromHand(
            IPokerPlayer pokerPlayer, 
            int index, 
            ref List<Type> Win, 
            ref Type sorted, 
            ref int[] Reserve);

        void HighCard(
            IPokerPlayer pokerPlayer, 
            int index, 
            ref List<Type> Win, 
            ref Type sorted, 
            ref int[] Reserve);
    }
}