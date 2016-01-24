namespace Poker
{
    using System;
    using System.Windows.Forms;
    using Contracts;
    using GUI;
    using Models;
    using PokerUtilities;
    using PokerUtilities.CardsCombinationMethods;

    public static class PokerMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IRandomProvider randomProvider = new RandomGenerator();
            var dealer = new Dealer(randomProvider);
            var checkHand = new CheckHandType();
            var handTypes = new HandTypes(randomProvider);
            var pokerTable = new PokerTable(dealer, checkHand, handTypes);

            Application.Run(pokerTable);
        }
    }
}
