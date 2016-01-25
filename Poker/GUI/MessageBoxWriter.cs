﻿namespace Poker.GUI
{
    using System.Windows.Forms;
    using Contracts;

    public class MessageBoxWriter : IWriter
    {
        public void Print(string msg)
        {
            MessageBox.Show(msg);
        }

        public DialogResult PrintYesNo(string yesMessage, string noMessage, MessageBoxButtons buttons)
        {
            var result = MessageBox.Show(yesMessage, noMessage, MessageBoxButtons.YesNo);

            return result;
        }
    }
}
