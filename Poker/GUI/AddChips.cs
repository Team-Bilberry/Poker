namespace Poker.GUI
{
    using System;
    using System.Windows.Forms;

    public partial class AddChips : Form
    {
        private const int MaxChipsToAdd = 100000000;
        private int addedChips;

        public AddChips()
        {
            this.InitializeComponent();
            this.ControlBox = false;
            this.outOfChipsLabel.BorderStyle = BorderStyle.FixedSingle;
        }

        public int AddedChips
        {
            get
            {
                return this.addedChips;
            }

            private set
            {
                if (value < 0 || MaxChipsToAdd < value)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.addedChips = value;
            }
        }

        private void bAddChips_Click(object sender, EventArgs e)
        {
            int parsedValue;
            bool isValidNumber = int.TryParse(this.addMoreChipsAmount.Text, out parsedValue);

            if (parsedValue < 0 || MaxChipsToAdd < parsedValue)
            {
                string message = $"The chips you can add should be in range {0}..{MaxChipsToAdd}";
                MessageBox.Show(message);
                return;
            }

            if (!isValidNumber)
            {
                const string message = "This is a number only field";
                MessageBox.Show(message);
            }
            else
            {
                this.AddedChips = parsedValue;
                this.Close();
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            const string message = "Are you sure?";
            const string title = "Quit";
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
