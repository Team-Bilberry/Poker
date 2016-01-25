namespace Poker.PokerUtilities
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Contracts;
    using Models;

    public static class ResetMethods
    {
        // TODO: it kinda didn't worked as intended.
        private static readonly IPokerPlayer player = new PokerPlayer(new Panel());
        private static readonly IPokerPlayer firstBot = new PokerPlayer(new Panel());
        private static readonly IPokerPlayer secondBot = new PokerPlayer(new Panel());
        private static readonly IPokerPlayer thirdBot = new PokerPlayer(new Panel());
        private static readonly IPokerPlayer fourthBot = new PokerPlayer(new Panel());
        private static readonly IPokerPlayer fifthBot = new PokerPlayer(new Panel());

        //private static Label playerStatus;
        //private static Label botOneStatus;
        //private static Label botTwoStatus;
        //private static Label botThreeStatus;
        //private static Label botFourStatus;
        //private static Label botFiveStatus;

        //private static int neededChipsToCall;
        //private static int bigBlind;
        //private static int raise;
        //private static int botsWithoutChips;
        //private static int rounds;
        //private static bool restart;
        //private static bool raising;
        //private static int winners;
        //private static int playersLeft;
        //private static int raisedTurn;
        //private static List<bool> eliminatedPlayers;
        //private static List<string> CheckWinners;
        //private static List<int> allInChips;
        //private static List<Type> Win;
        //private static Type sorted;
        //private static Label potStatus;
        //private static int secondsLeft;
        //private static int turnCount;

        //public static void ResetStatsOfPlayers()
        //{
        //    MakePlayersNonVisible();

        //    neededChipsToCall = bigBlind;
        //    raise = 0;
        //    botsWithoutChips = 5;
        //    rounds = 0;

        //    ResetPower();

        //    ResetType();

        //    player.AbleToMakeTurn = true;
        //    firstBot.AbleToMakeTurn = false;
        //    secondBot.AbleToMakeTurn = false;
        //    thirdBot.AbleToMakeTurn = false;
        //    fourthBot.AbleToMakeTurn = false;
        //    fifthBot.AbleToMakeTurn = false;

        //    player.OutOfChips = false;
        //    firstBot.OutOfChips = false;
        //    secondBot.OutOfChips = false;
        //    thirdBot.OutOfChips = false;
        //    fourthBot.OutOfChips = false;
        //    fifthBot.OutOfChips = false;

        //    player.Folded = false;
        //    firstBot.Folded = false;
        //    secondBot.Folded = false;
        //    thirdBot.Folded = false;
        //    fourthBot.Folded = false;
        //    fifthBot.Folded = false;
        //    restart = false;
        //    raising = false;

        //    ResetCall();

        //    ResetRaise();
        //    // height = 0;
        //    // width = 0;
        //    winners = 0;

        //    //this.Flop = 1;
        //    //this.Turn = 2;
        //    //this.River = 3;
        //    //this.End = 4;
        //    playersLeft = 6;
        //    raisedTurn = 1;

        //    eliminatedPlayers.Clear();
        //    CheckWinners.Clear();
        //    allInChips.Clear();
        //    Win.Clear();

        //    sorted.Current = 0;
        //    sorted.Power = 0;

        //    potStatus.Text = "0";
        //    secondsLeft = 60;
        //    turnCount = 0;

        //    ResetPlayerStatusText();
        //}

        //public static void ResetPlayerStatusText()
        //{
        //    playerStatus.Text = string.Empty;
        //    botOneStatus.Text = string.Empty;
        //    botTwoStatus.Text = string.Empty;
        //    botThreeStatus.Text = string.Empty;
        //    botFourStatus.Text = string.Empty;
        //    botFiveStatus.Text = string.Empty;
        //}

        public static void ResetType()
        {
            player.Type = -1;
            firstBot.Type = -1;
            secondBot.Type = -1;
            thirdBot.Type = -1;
            fourthBot.Type = -1;
            fifthBot.Type = -1;
        }

        public static void ResetPower()
        {
            player.Power = 0;
            firstBot.Power = 0;
            secondBot.Power = 0;
            thirdBot.Power = 0;
            fourthBot.Power = 0;
            fifthBot.Power = 0;
        }

        public static void ResetRaise()
        {
            player.Raise = 0;
            firstBot.Raise = 0;
            secondBot.Raise = 0;
            thirdBot.Raise = 0;
            fourthBot.Raise = 0;
            fifthBot.Raise = 0;
        }

        public static void ResetCall()
        {
            player.Call = 0;
            firstBot.Call = 0;
            secondBot.Call = 0;
            thirdBot.Call = 0;
            fourthBot.Call = 0;
            fifthBot.Call = 0;
        }

        public static void MakePlayersNonVisible()
        {
            player.Panel.Visible = false;
            firstBot.Panel.Visible = false;
            secondBot.Panel.Visible = false;
            thirdBot.Panel.Visible = false;
            fourthBot.Panel.Visible = false;
            fifthBot.Panel.Visible = false;
        }
    }
}