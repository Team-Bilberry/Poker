namespace Poker.Core
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Contracts;
    using GUI;
    using Models;
    using PokerUtilities;
    using PokerUtilities.CardsCombinationMethods;
    // TODO: Can't move the logic methods, because the fields in PokerTable.Designer.cs are private.
    public class Engine
    {
        private static IRandomProvider randomProvider = new RandomGenerator();
        private static IDealer dealer = new Dealer(randomProvider);
        private static CheckHandType checkHand = new CheckHandType();
        private static HandTypes handTypes = new HandTypes(randomProvider);
        private static PokerTable pokerTable = new PokerTable(dealer, checkHand, handTypes);

        private int neededChipsToCall;
        private int botsWithoutChips = 5;
        private double type;
        private int rounds = 0;
        private int raise = 0;
        private int playersLeft = 6;
        private int winners = 0;
        private int raisedTurn = 1;

        private bool intsadded;
        private bool changed;

        List<bool?> eliminatedPlayers = new List<bool?>();
        List<Type> Win = new List<Type>();
        List<string> CheckWinners = new List<string>();
        List<int> ints = new List<int>();
        private bool restart = false;
        private bool raising = false;

        Type sorted;
        string[] ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png", SearchOption.TopDirectoryOnly);

        int[] reserve = new int[17];
        Image[] deck = new Image[52];
        PictureBox[] cardPicture = new PictureBox[52];
        Timer timer = new Timer();
        Timer updates = new Timer();

        private int secondsLeft = 60;
        private int bigBlind = 500;
        private int smallBlind = 250;
        private int turnCount = 0;
    }
}