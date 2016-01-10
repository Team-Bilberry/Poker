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

         bool Turn { get; set; }

         bool FoldedTurn { get; set; }

         bool Folded { get; set; }

         void InizializePanel(Point location);
    }
}
