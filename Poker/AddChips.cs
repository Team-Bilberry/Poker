namespace Poker
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class AddChips : Form
    {
        private const int MaxChipsToAdd = 100000000;
       
        public AddChips()
        {
            FontFamily fontFamily = new FontFamily("Arial");
            this.InitializeComponent();
            this.ControlBox = false;
            this.lblOutOfChips.BorderStyle = BorderStyle.FixedSingle;
        }

        public int AddedChips { get; set; }

        private void bAddChips_Click(object sender, EventArgs e)
        {
            int parsedValue;
            bool isValidNumber = int.TryParse(this.tbAddChips.Text, out parsedValue);

            if (parsedValue > MaxChipsToAdd || parsedValue < 0)
            {
                string msg = $"The chips you can add should be in range {0}..{MaxChipsToAdd}";
                MessageBox.Show(msg);
                return;
            }

            if (!isValidNumber)
            {
                MessageBox.Show("This is a number only field");
            }
            else
            {
                this.AddedChips = parsedValue;
                this.Close();
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            var message = "Are you sure?";
            var title = "Quit";
            var result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            switch (result)
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    Application.Exit();
                    break;
            }
        }
    }
}
