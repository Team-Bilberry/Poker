namespace Poker.Contracts
{
    using System.Windows.Forms;

    public interface IWriter
    {
        void Print(string msg);

        DialogResult PrintYesNo(string yesMessage, string noMessage, MessageBoxButtons buttons);

        DialogResult PrintYesNoQuestion(string message, string title, MessageBoxButtons buttons, MessageBoxIcon question);
    }
}