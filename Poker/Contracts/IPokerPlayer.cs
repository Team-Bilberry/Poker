namespace Poker.Contracts
{
    using System.Drawing;
    using System.Windows.Forms;

    public interface IPokerPlayer
    {
         Panel Panel { get; }

         double Type { get; set; }

         double Power { get; set; }

         int Chips { get; set; }

         int Call { get; set; }

         int Raise { get; set; }

         bool AbleToMakeTurn { get; set; }

         bool OutOfChips { get; set; }

         bool Folded { get; set; }

         void InitializePanel(Point location);
    }
}
