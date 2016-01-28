﻿namespace Poker.Contracts
{
    using System.Windows.Forms;

    public interface IPlayerActions
    {
        void Fold(IPokerPlayer pokerPlayer, Label sStatus, ref bool rising);

        void Check(IPokerPlayer pokerPlayer, Label cStatus, ref bool raising);

        void Call(IPokerPlayer pokerPlayer, Label sStatus, ref bool raising, ref int neededChipsToCall,
            TextBox potStatus);

        void Raised(IPokerPlayer pokerPlayer, Label sStatus, ref bool raising, ref int raise, ref int neededChipsToCall,
            TextBox potStatus);

        void HP(IPokerPlayer pokerPlayer, Label sStatus, int n, int n1, int neededChipsToCall, TextBox potStatus,
            ref int raise, ref bool raising);

        void PH(IPokerPlayer pokerPlayer, Label sStatus, int n, int n1, int r, int neededChipsToCall, TextBox potStatus,
            ref int raise, ref bool raising, int rounds);
    }
}