namespace Poker.Models
{
    using System.Drawing;
    using System.Windows.Forms;
    using Contracts;

    public class PokerPlayer : IPokerPlayer
    {
        private const int DefaultChips = 10000;
        private const int DefaultPanelHeight = 150;
        private const int DefaultPanelWidth = 180;

        public PokerPlayer(Panel panel)
        {
            this.Panel = panel;
            this.Type = -1;
            this.Power = 0;
            this.Chips = DefaultChips;
            this.Call = 0;
            this.Raise = 0;
            this.AbleToMakeTurn = false;
            this.OutOfChips = false;
            this.Folded = false;
        }

        public Panel Panel { get; }

        public double Type { get; set; }

        public double Power { get; set; }

        public int Chips { get; set; }

        public int Call { get; set; }

        public int Raise { get; set; }

        public bool AbleToMakeTurn { get; set; }

        public bool OutOfChips { get; set; }

        public bool Folded { get; set; }

        public void InitializePanel(Point location)
        {
            this.Panel.Location = location;
            this.Panel.BackColor = Color.Transparent;
            this.Panel.Height = DefaultPanelHeight;
            this.Panel.Width = DefaultPanelWidth;
            this.Panel.Visible = false;
        }
    }
}