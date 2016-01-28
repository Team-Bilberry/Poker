namespace Poker
{
    using Contracts;
    using GUI;
    using Models;
    using PokerUtilities;
    using PokerUtilities.CardsCombinationMethods;
    using System;
    using System.Windows.Forms;

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

            IRandomGenerator randomGenerator = new RandomGenerator();
            var dealer = new Dealer(randomGenerator);
            var checkHand = new CheckHandType();
            var handTypes = new HandTypes(randomGenerator);
            IWriter messageBoxWriter = new MessageBoxWriter();
            var pokerTable = new PokerTable(dealer, checkHand, handTypes, messageBoxWriter);

            Application.Run(pokerTable);
        }
    }
}