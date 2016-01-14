namespace Poker
{
    partial class AddChips
    {
        private System.Windows.Forms.Label outOfChipsLabel;
        private System.Windows.Forms.Button addChipsButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.TextBox tbAddChips;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.outOfChipsLabel = new System.Windows.Forms.Label();
            this.addChipsButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.tbAddChips = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // outOfChipsLabel
            // 
            this.outOfChipsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.outOfChipsLabel.Location = new System.Drawing.Point(48, 49);
            this.outOfChipsLabel.Name = "outOfChipsLabel";
            this.outOfChipsLabel.Size = new System.Drawing.Size(176, 23);
            this.outOfChipsLabel.TabIndex = 0;
            this.outOfChipsLabel.Text = "You ran out of chips!";
            this.outOfChipsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // addChipsButton
            // 
            this.addChipsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addChipsButton.Location = new System.Drawing.Point(12, 226);
            this.addChipsButton.Name = "addChipsButton";
            this.addChipsButton.Size = new System.Drawing.Size(75, 23);
            this.addChipsButton.TabIndex = 1;
            this.addChipsButton.Text = "Add Chips";
            this.addChipsButton.UseVisualStyleBackColor = true;
            this.addChipsButton.Click += new System.EventHandler(this.bAddChips_Click);
            // 
            // exitButton
            // 
            this.exitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exitButton.Location = new System.Drawing.Point(197, 226);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tbAddChips
            // 
            this.tbAddChips.Location = new System.Drawing.Point(91, 229);
            this.tbAddChips.Name = "tbAddChips";
            this.tbAddChips.Size = new System.Drawing.Size(100, 20);
            this.tbAddChips.TabIndex = 3;
            // 
            // AddChips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.tbAddChips);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.addChipsButton);
            this.Controls.Add(this.outOfChipsLabel);
            this.Name = "AddChips";
            this.Text = "You Ran Out Of Chips";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}