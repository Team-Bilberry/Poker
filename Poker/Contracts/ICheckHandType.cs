namespace Poker.Contracts
{
    using System.Collections.Generic;

    public interface ICheckHandType
    {
        void rStraightFlush(IPokerPlayer pokerPlayer, int[] clubes, int[] dimonds, int[] hearts, int[] spades, ref List<Type> Win, ref Type sorted);

        void rFourOfAKind(IPokerPlayer pokerPlayer, int[] Straight, ref List<Type> Win, ref Type sorted);

        void rFullHouse(IPokerPlayer pokerPlayer, ref bool done, int[] Straight, ref List<Type> Win, ref Type sorted, ref double type);

        void rFlush(IPokerPlayer pokerPlayer, ref bool vf, int[] Straight1, ref int index, ref List<Type> Win, ref Type sorted, ref int[] Reserve);

        void rStraight(IPokerPlayer pokerPlayer, int[] Straight, int index, ref List<Type> Win, ref Type sorted);

        void rThreeOfAKind(IPokerPlayer pokerPlayer, int[] Straight, int index, ref List<Type> Win, ref Type sorted);

        void rTwoPair(IPokerPlayer pokerPlayer, int index, ref List<Type> Win, ref Type sorted, ref int[] Reserve);

        void rPairTwoPair(IPokerPlayer pokerPlayer, int index, ref List<Type> Win, ref Type sorted, ref int[] Reserve);

        void rPairFromHand(IPokerPlayer pokerPlayer, int index, ref List<Type> Win, ref Type sorted, ref int[] Reserve);

        void rHighCard(IPokerPlayer pokerPlayer, int index, ref List<Type> Win, ref Type sorted, ref int[] Reserve);
    }
}
