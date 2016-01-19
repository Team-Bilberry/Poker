namespace Poker
{
    using System;
    using System.Windows.Forms;
    using Contracts;
    using GUI;
    using Models;

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

            IDealer dealer = new Dealer();

            Application.Run(new PokerTable(dealer));
        }
    }
}
