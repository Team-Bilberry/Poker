namespace Poker.Contracts
{
    using System.Windows.Forms;

    public interface IHandType
    {
        void HighCard(
            IPokerPlayer pokerPlayer, 
            Label sStatus, 
            int neededChipsToCall, 
            TextBox potStatus, 
            ref int raise,
            ref bool raising);

        void PairTable(
            IPokerPlayer pokerPlayer,
            Label sStatus,
            int neededChipsToCall,
            TextBox potStatus,
            ref int raise,
            ref bool raising);

        void PairHand(
            IPokerPlayer pokerPlayer,
            Label sStatus,
            int neededChipsToCall,
            TextBox potStatus,
            ref int raise,
            ref bool raising, 
            ref int rounds);

        void TwoPair(
            IPokerPlayer pokerPlayer,
            Label sStatus,
            int neededChipsToCall,
            TextBox potStatus,
            ref int raise,
            ref bool raising, 
            ref int rounds);

        void ThreeOfAKind(
            IPokerPlayer pokerPlayer, 
            Label sStatus, 
            int name, 
            int neededChipsToCall, 
            TextBox potStatus,
            ref int raise, 
            ref bool raising, 
            ref int rounds);

        void Straight(
            IPokerPlayer pokerPlayer,
            Label sStatus,
            int name,
            int neededChipsToCall,
            TextBox potStatus,
            ref int raise,
            ref bool raising,
            ref int rounds);

        void Flush(
            IPokerPlayer pokerPlayer,
            Label sStatus,
            int name,
            int neededChipsToCall,
            TextBox potStatus,
            ref int raise,
            ref bool raising,
            ref int rounds);

        void FullHouse(
            IPokerPlayer pokerPlayer,
            Label sStatus,
            int name,
            int neededChipsToCall,
            TextBox potStatus,
            ref int raise,
            ref bool raising,
            ref int rounds);

        void FourOfAKind(
            IPokerPlayer pokerPlayer,
            Label sStatus,
            int name,
            int neededChipsToCall,
            TextBox potStatus,
            ref int raise,
            ref bool raising,
            ref int rounds);

        void StraightFlush(
            IPokerPlayer pokerPlayer,
            Label sStatus,
            int name,
            int neededChipsToCall,
            TextBox potStatus,
            ref int raise,
            ref bool raising,
            ref int rounds);
    }
}